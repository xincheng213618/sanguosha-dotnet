using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourceBiriSkill = "shanhe:source-biri-14039";
    private const string ShanheSourceBiriDrawReason = "skill-program.shanhe-source-biri-14039.draw";

    [ProgramCapability]
    private sealed class ShanheSourceBiriCapability(GameEngine engine) : IShanheSourceBiriHost
    {
        public SkillProgramStepOutcome PreventFireRecoverDraw(ProgramSkillFrame frame) => engine.BeginShanheSourceBiri(frame);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62047,
        SkillProgramEffectOp.PreventShanheSourceFireRecoverDraw)]
    private bool CanOfferShanheSourceBiri(ProgramTriggerLegalityContext context) =>
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null &&
        context.Owner.HasHpValue && context.WindowContext is
            { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, TargetSeat: { } target } &&
        target == context.Owner.Seat;

    private BeforeDamageProgramWindowFrame ExactShanheSourceBiriParent(ProgramSkillFrame frame)
    {
        var index = _resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (_modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is null ||
            frame.SkillId != ShanheSourceBiriSkill || frame.TriggerId != "prevent-fire" ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || index < 1 ||
            _resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame before || before.Id != context.ParentFrameId ||
            before.TargetSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat ||
            context.OwnerSeat != frame.OwnerSeat || before.RedirectedTargetSeat is not null ||
            before.Nature != DamageNature.Fire || before.Amount <= 0 || context.Amount != before.Amount ||
            before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count ||
            !MountObserverCandidateMatches(frame, before.Candidates[before.CandidateIndex].Candidate))
            throw new InvalidOperationException("Source Biri lost its original owner fire-damage candidate.");
        return before;
    }

    private SkillProgramStepOutcome BeginShanheSourceBiri(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var before = ExactShanheSourceBiriParent(frame);
        if (frame.ShanheSourceBiri is not null || before.Prevented || CurrentDamageAttempt is not { } attack ||
            attack.ResolutionId != (before.ContinuationAttackResolutionId ?? before.ParentFrameId) ||
            attack.TargetSeat != frame.OwnerSeat || attack.SourceSeat != before.SourceSeat ||
            frame.WindowContext!.SourceSeat != (attack.IsSourceLess ? null : attack.SourceSeat) ||
            !_players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Source Biri cannot prevent or pay a fire-damage attempt twice.");
        var amount = before.Amount;
        ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = new(frame.InstructionIndex, before.Id,
            before.ParentFrameId, attack.ResolutionId, amount, checked(amount * 2)) });
        AdvanceEventRulesAndQueueFact(new ShanheSourceBiriIssuedEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.InstructionIndex,
            before.Id, before.ParentFrameId, attack.ResolutionId, amount, checked(amount * 2)));
        PreventProgramCurrentDamage(frame);
        // The shared recovery primitive owns modifiers, replacement and the real
        // RecoveryFrame. Its atomic result is captured below before HP children.
        new ProgramSkillHost(this).Recover(frame.Id, frame.OwnerSeat, frame.OwnerSeat, checked(amount * 2), null, null);
        frame = GetActiveProgramFrame(frame.Id);
        if (frame.PendingRecoveryAttempts is { Count: > 0 } attempts)
        {
            if (attempts is not [var request] || request.SourceSeat != frame.OwnerSeat || request.TargetSeat != frame.OwnerSeat ||
                request.Completion != new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, frame.InstructionIndex))
                throw new InvalidOperationException("Source Biri lost its exact queued recovery request.");
            ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = frame.ShanheSourceBiri! with
                { RecoveryAttemptId = request.Id, RecoveryAttemptAmount = request.Amount } });
        }
        ContinueShanheSourceBiri(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // Read the paid native result, not owner HP after descendants have run.
    // A redirected recovery belongs to its other recipient and yields zero here.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62047)]
    private void CaptureShanheSourceBiriRecovery(RecoveryAppliedEvent applied)
    {
        if (_resolutionStack.LastOrDefault() is not RecoveryFrame recovery || applied.Amount <= 0) return;
        var parent = _resolutionStack.SingleOrDefault(item => item.Id == recovery.ParentFrameId);
        var frame = parent as ProgramSkillFrame;
        if (parent is RecoveryReplacementFrame replacement)
        {
            frame = _resolutionStack.SingleOrDefault(item => item.Id == replacement.ParentFrameId) as ProgramSkillFrame;
            if (frame?.ShanheSourceBiri is not { } queued || queued.RecoveryAttemptId != replacement.Id ||
                replacement.Attempt.Id != replacement.Id || replacement.Attempt.SourceSeat != frame.OwnerSeat ||
                replacement.Attempt.TargetSeat != frame.OwnerSeat || replacement.Attempt.Amount != queued.RecoveryAttemptAmount ||
                replacement.CandidateIndex is not null) return;
        }
        if (frame?.ShanheSourceBiri is not { Stage: ShanheSourceBiriStage.RecoveryChildren } receipt ||
            frame.SkillId != ShanheSourceBiriSkill || recovery.SourceSeat != frame.OwnerSeat || recovery.TargetSeat != frame.OwnerSeat)
            return;
        if (receipt.AppliedRecoveryFrameId is not null || applied.SourceSeat != recovery.SourceSeat ||
            applied.TargetSeat != recovery.TargetSeat || applied.Amount > recovery.Amount ||
            applied.RemainingHp != _players[frame.OwnerSeat].Hp)
            throw new InvalidOperationException("Source Biri recovery lost its once-paid native application.");
        ReplaceRuntimeFrame(frame.Id, frame with { ShanheSourceBiri = receipt with
            { AppliedRecoveryFrameId = recovery.Id, ActualRecovered = applied.Amount } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceBiriRecoveredEvent(frame.Id, recovery.Id,
            recovery.ParentFrameId, frame.OwnerSeat, applied.Amount));
    }

    private void ContinueShanheSourceBiri(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheSourceBiri is { } receipt)
        {
            if (TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) ||
                TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
                TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) ||
                TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id)) return;
            frame = GetActiveProgramFrame(frame.Id); receipt = frame.ShanheSourceBiri!;
            if (receipt.Stage == ShanheSourceBiriStage.DrawChildren || _winner != Winner.None)
            { FinishProgramSkill(frame, true); return; }
            var count = _players[frame.OwnerSeat].IsAlive ? receipt.ActualRecovered : 0;
            var sequence = _movementSequence;
            ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = receipt with
                { Stage = ShanheSourceBiriStage.DrawChildren, DrawCount = count, MovementSequenceBefore = sequence,
                    MovementSequenceAfter = sequence } });
            AdvanceEventRulesAndQueueFact(new ShanheSourceBiriDrawIssuedEvent(frame.Id, frame.OwnerSeat, receipt.ActualRecovered, count));
            if (count > 0) DrawProgramCards(frame.Id, frame.OwnerSeat, count, null, null,
                SkillProgramCardSetVisibility.Private, new(ShanheSourceBiriDrawReason));
            frame = GetActiveProgramFrame(frame.Id);
            ReplaceRuntimeTop(frame = frame with { ShanheSourceBiri = frame.ShanheSourceBiri! with
                { MovementSequenceAfter = _movementSequence } });
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62047)]
    private bool ResumeShanheSourceBiri(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceBiri: not null } frame || frame.Id != id) return false;
        ContinueShanheSourceBiri(frame); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 62047)]
    private bool ReturnShanheSourceBiriMovement(ProgramSkillFrame frame) => frame.ShanheSourceBiri is not null && ResumeShanheSourceBiri(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62047)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62047)]
    private bool CanFinishShanheSourceBiri(ProgramSkillFrame frame) => frame.ShanheSourceBiri is not null;

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62047)]
    private void AssertShanheSourceBiri(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceBiri is not { } receipt) return;
        var before = ExactShanheSourceBiriParent(frame);
        if (!before.Prevented || receipt.InstructionIndex != frame.InstructionIndex || receipt.BeforeDamageFrameId != before.Id ||
            receipt.DamageParentFrameId != before.ParentFrameId || receipt.AttackResolutionId != (before.ContinuationAttackResolutionId ?? before.ParentFrameId) ||
            receipt.PreventedAmount != before.Amount || receipt.RequestedRecoveryAmount != checked(before.Amount * 2) ||
            !Enum.IsDefined(receipt.Stage) || receipt.ActualRecovered < 0 ||
            (receipt.AppliedRecoveryFrameId is null) != (receipt.ActualRecovered == 0) ||
            receipt.MovementSequenceBefore < 0 || receipt.MovementSequenceAfter < receipt.MovementSequenceBefore ||
            receipt.DrawCount < 0 || receipt.DrawCount > receipt.ActualRecovered ||
            ProgramEventHistory<ShanheSourceBiriIssuedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray() is not [var issued] ||
            issued != new ShanheSourceBiriIssuedEvent(frame.Id,
                new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.InstructionIndex,
                before.Id, before.ParentFrameId, receipt.AttackResolutionId, before.Amount, checked(before.Amount * 2)))
            throw new InvalidOperationException("Source Biri lost its frozen prevention/recovery/draw owner receipt.");
        if (receipt.AppliedRecoveryFrameId is { } child && ProgramEventHistory<ShanheSourceBiriRecoveredEvent>()
            .Count(e => e.ProgramFrameId == frame.Id && e.RecoveryFrameId == child && e.OwnerSeat == frame.OwnerSeat &&
                e.Amount == receipt.ActualRecovered && e.RecoveryParentFrameId == (receipt.RecoveryAttemptId ?? frame.Id)) != 1)
            throw new InvalidOperationException("Source Biri lost its original paid recovery result.");
        if (ProgramEventHistory<ProgramDamagePreventedEvent>().Count(e => e.FrameId == before.Id && e.SkillId == frame.SkillId &&
                e.BindingId == frame.TriggerId && e.OwnerSeat == frame.OwnerSeat && e.TargetSeat == frame.OwnerSeat &&
                e.SourceSeat == frame.WindowContext!.SourceSeat && e.Amount == receipt.PreventedAmount) != 1 ||
            receipt.RecoveryAttemptId is { } request && (receipt.RecoveryAttemptAmount <= 0 ||
                receipt.RecoveryAttemptAmount > receipt.RequestedRecoveryAmount ||
                ProgramEventHistory<ProgramRecoveryRequestedEvent>().Count(e => e.ProgramFrameId == frame.Id &&
                    e.InstructionIndex == frame.InstructionIndex && e.GameplayHash == frame.GameplayHash &&
                    e.RecoveryFrameId == request && e.SourceSeat == frame.OwnerSeat && e.TargetSeat == frame.OwnerSeat &&
                    e.RequestedAmount == receipt.RecoveryAttemptAmount) != 1) ||
            receipt.RecoveryAttemptId is null && receipt.RecoveryAttemptAmount != 0 ||
            receipt.Stage == ShanheSourceBiriStage.DrawChildren &&
                ProgramEventHistory<ShanheSourceBiriDrawIssuedEvent>().Count(e => e.ProgramFrameId == frame.Id &&
                    e.OwnerSeat == frame.OwnerSeat && e.ActualRecovered == receipt.ActualRecovered && e.DrawCount == receipt.DrawCount) != 1)
            throw new InvalidOperationException("Source Biri lost its exact prevention, recovery request or single draw instruction.");
    }
}
