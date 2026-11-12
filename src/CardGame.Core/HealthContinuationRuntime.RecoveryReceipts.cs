namespace CardGame.Core;

internal sealed partial class HealthContinuationRuntime
{
    internal void RecordProgramRecoveryRequest(ResolutionFrame? parent, RecoveryAttempt attempt)
    {
        if (parent is not ProgramSkillFrame owner || attempt.Completion.Producer != RecoveryAttemptProducer.Program) return;
        var index = facts.FrameIndex(owner.Id);
        if (index < 0 || facts.FrameAt(index) is not ProgramSkillFrame current ||
            current.InstructionIndex != owner.InstructionIndex || current.GameplayHash != owner.GameplayHash ||
            current.SkillId != owner.SkillId || current.SkillInstanceId != owner.SkillInstanceId ||
            current.PendingRecoveryAttempts is not { } requests ||
            requests.Where(request => request.Id == attempt.Id).ToArray() is not [var issued] ||
            (issued with { ProgramOrigin = null }) != (attempt with { ProgramOrigin = null }) ||
            attempt.Completion != new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, owner.InstructionIndex) ||
            TypedRecoveryPausedEffect(current) is null)
            throw new InvalidOperationException("Program recovery request lost its exact instruction owner.");
        var origin = new ProgramRecoveryOrigin(owner.Id, owner.InstructionIndex, owner.GameplayHash);
        if (attempt.ProgramOrigin is not null && attempt.ProgramOrigin != origin)
            throw new InvalidOperationException("A program recovery request cannot borrow another issuing instruction.");
        if (issued.ProgramOrigin is { } previous)
        {
            if (previous != origin)
                throw new InvalidOperationException("A program recovery request cannot replace its issuing instruction.");
            return;
        }
        frames.Replace(current.Id, current with
        {
            PendingRecoveryAttempts = Array.AsReadOnly(requests.Select(request => request.Id == issued.Id
                ? request with { ProgramOrigin = origin } : request).ToArray())
        });
        events.AdvanceFact(new ProgramRecoveryRequestedEvent(owner.Id, owner.InstructionIndex,
            owner.GameplayHash, attempt.Id, attempt.SourceSeat, attempt.TargetSeat, attempt.Amount, attempt.HpBefore)
        { HappyQiaoBefore = attempt.HappyQiaoBefore });
    }

    internal bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame owner, RecoveryReplacementFrame recovery)
    {
        var attempt = recovery.Attempt;
        if (recovery.Id != attempt.Id || recovery.ParentFrameId != owner.Id || recovery.Id <= owner.Id ||
            recovery.Return.ResumeFrameId != owner.Id || recovery.Return.CardId is not null || recovery.Return.CardKind is not null ||
            !(recovery.Return.Continuation == PostEventContinuation.Program ||
              recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && owner.PendingMovementContinuation is not null) ||
            attempt.ProgramOrigin is not { } origin || origin.ProgramFrameId != owner.Id ||
            origin.InstructionIndex != owner.InstructionIndex || origin.GameplayHash != owner.GameplayHash ||
            attempt.Completion != new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, owner.InstructionIndex) ||
            attempt.Amount <= 0 ||
            (attempt.HpBefore is null) != (attempt.HappyQiaoBefore is not null) ||
            !facts.IsValidPlayerSeat(attempt.SourceSeat) || !facts.IsValidPlayerSeat(attempt.TargetSeat) ||
            !TypedRecoveryApplicationBoundsMatch(recovery)) return false;
        var paused = TypedRecoveryPausedEffect(owner);
        if (paused is null) return false;
        // The origin identifies the issuer; primitive Recover also retains its
        // accepted source, amount and target bounds for every receipt reader.
        return paused.CompiledInstruction is not RecoverProgramInstruction instruction ||
            !IsTypedRecoveryTargetSupported(instruction) || PrimitiveProgramRecoveryRequestMatches(owner, paused, instruction, attempt);
    }

    // One actual recovery owns one HP invoice. Native operations and primitive
    // Recover use the same RecoveryFrame, including synchronous target batches.
    internal void RecordProgramRecoveryApplication(RecoveryFrame recovery, HpChangeContext change)
    {
        var index = facts.FrameIndex(recovery.ParentFrameId);
        if (index < 0 || facts.FrameAt(index) is not ProgramSkillFrame owner) return;
        if (change.ParentFrameId != owner.Id || change.Kind != HpChangeKind.Recovery ||
            change.SourceSeat != recovery.SourceSeat || change.TargetSeat != recovery.TargetSeat ||
            change.HpBefore != recovery.HpBefore || change.Amount <= 0 || change.Amount > recovery.Amount)
            throw new InvalidOperationException("Program recovery lost its real child and HP invoice.");
        var receipt = new ProgramRecoveryApplicationReceipt(change.Id, recovery.Id, owner.InstructionIndex,
            owner.GameplayHash, recovery.SourceSeat, recovery.TargetSeat, recovery.Amount, change.HpBefore, change.HpAfter);
        var previous = owner.RecoveryApplications?.SingleOrDefault(application =>
            application.HpChangeId == change.Id || application.RecoveryFrameId == recovery.Id);
        if (previous is not null)
        {
            if (previous != receipt)
                throw new InvalidOperationException("A program recovery cannot replace its actual health application.");
            return;
        }
        frames.Replace(owner.Id, owner with
        { RecoveryApplications = Array.AsReadOnly((owner.RecoveryApplications ?? []).Append(receipt).ToArray()) });
        events.AdvanceFact(new ProgramRecoveryAppliedEvent(owner.Id, receipt));
    }

    internal bool ProgramRecoveryReceiptMatches(ProgramSkillFrame owner, HpChangedTriggerWindowFrame hp)
    {
        if (!RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramHpReturnMatches(owner, hp) || hp.Change.Kind != HpChangeKind.Recovery)
            return false;
        ProgramRecoveryApplicationReceipt? receipt = null;
        var receipts = owner.RecoveryApplications;
        for (var index = 0; index < (receipts?.Count ?? 0); index++)
        {
            var candidate = receipts![index];
            if (candidate.HpChangeId != hp.Change.Id) continue;
            if (receipt is not null) return false;
            receipt = candidate;
        }
        if (receipt is null ||
            receipt.InstructionIndex != owner.InstructionIndex || receipt.GameplayHash != owner.GameplayHash ||
            TypedRecoveryPausedEffect(owner) is null || receipt.SourceSeat != hp.Change.SourceSeat ||
            receipt.TargetSeat != hp.Change.TargetSeat || receipt.HpBefore != hp.Change.HpBefore || receipt.HpAfter != hp.Change.HpAfter ||
            hp.Change.Amount != receipt.HpAfter - receipt.HpBefore || hp.Change.Amount <= 0 ||
            hp.Change.Amount > receipt.RequestedAmount || receipt.RecoveryFrameId <= owner.Id)
            return false;
        return true;
    }

    internal void ClearProgramRecoveryApplication(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Recovery || change.ParentFrameId is not { } parent) return;
        var index = facts.FrameIndex(parent);
        if (index < 0 || facts.FrameAt(index) is not ProgramSkillFrame { RecoveryApplications: { } receipts } owner) return;
        var remaining = receipts.Where(r => r.HpChangeId != change.Id).ToArray();
        frames.Replace(owner.Id, owner with { RecoveryApplications = remaining.Length == 0 ? null : remaining });
    }
}
