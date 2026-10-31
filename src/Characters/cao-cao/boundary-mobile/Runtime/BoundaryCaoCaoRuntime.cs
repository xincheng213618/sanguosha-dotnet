namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class BoundaryCaoCaoRuntime(IProgramRuntimeFrames frames, IProgramEventHistoryQueries history,
    ICardMovementHistoryQueries movements, ICardMovementBatchQueue batches, IProgramDamageCardMaterialQueries materials,
    IProgramDamageCardClaims claims, IBoundCardMovementContinuationHost movementReturns) : IBoundaryCaoCaoHost
{
    public SkillProgramStepOutcome ClaimDamageCards(ProgramSkillFrame frame)
    {
        claims.Claim(frame.Id, frame.OwnerSeat, new(BoundaryCaoCaoMobile.ClaimReason));
        return movementReturns.AwaitBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    // The ordinary claim owns material/group accounting; the committed claim
    // and its original damage window prove the same paid movement return.
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 58700)]
    private bool ClaimMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards ||
            frame.SkillId != BoundaryCaoCaoMobile.Jianxiong || frame.InstructionIndex != 1 ||
            pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 ||
            pending.CoverageResultBind is not null || CommittedClaim(frame) is not { } claim) return false;
        var journal = movements.Read();
        return Enumerable.Range(0, batches.PendingBatchCount).Select(batches.PendingBatchAt)
            .Concat(frames.OfType<CardsMovedTriggerWindowFrame>().Select(window => window.Batch)).Any(batch =>
            batch.ParentFrameId == frame.Id && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId &&
            batch.OriginOwnerSeat == frame.OwnerSeat && batch.Movements.Count == claim.CardIds.Count &&
            batch.Movements.All(movement => claim.CardIds.Contains(movement.CardId) && movement.From == CardLocation.Processing &&
                movement.To == CardLocation.Hand(frame.OwnerSeat) && movement.Reason.Value == BoundaryCaoCaoMobile.ClaimReason && journal.Contains(movement)));
    }

    private ProgramDamageCardsClaimedEvent? CommittedClaim(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryCaoCaoMobile.Jianxiong || frame.InstructionIndex is < 1 or > 2 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.TargetSeat != frame.OwnerSeat)
            return null;
        if (frames.ParentOf(frame.Id) is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.ParentFrameId != context.DamageFrameId || window.TriggerWindow != context.Window || window.TargetSeat != frame.OwnerSeat ||
            window.SourceSeat != context.SourceSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame,
                window.Candidates[window.CandidateIndex].ToProgramCandidate()) ||
            frames.ParentOf(window.Id) is not DamageFrame damage || damage.Id != context.DamageFrameId ||
            damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount) return null;
        var attack = materials.Read(damage.ParentFrameId);
        if (attack is not { Applied: true }) return null;
        var committed = history.Read<ProgramDamageCardsClaimedEvent>().Where(item => item.FrameId == frame.Id).ToArray();
        var physical = attack.CardIds.ToHashSet();
        var journal = movements.Read();
        return committed is [var claim] && claim.SkillId == frame.SkillId && claim.BindingId == frame.TriggerId && claim.OwnerSeat == frame.OwnerSeat &&
            claim.CardIds.Count > 0 && claim.CardIds.Distinct().Count() == claim.CardIds.Count && claim.CardIds.All(id => physical.Contains(id) &&
                journal.Any(movement => movement.CardId == id && movement.From == CardLocation.Processing &&
                    movement.To == CardLocation.Hand(frame.OwnerSeat) && movement.Reason.Value == BoundaryCaoCaoMobile.ClaimReason)) ? claim : null;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 58700)]
    private bool ContinuePaidClaim(ProgramSkillFrame frame) => CommittedClaim(frame) is not null;

    // The already-paid claim returns on its original damage proof. The normal
    // executor then runs Draw once even when the gain child removed the skill.
    [ProgramContinuation(ProgramContinuationKind.Movement, 58700)]
    private bool ReturnPaidClaim(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryCaoCaoMobile.Jianxiong || frame.InstructionIndex != 1 ||
            frame.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } ||
            subject != frame.OwnerSeat) return false;
        if (CommittedClaim(frame) is null)
            throw new InvalidOperationException("Mobile Jianxiong movement return lost its original paid damage-card claim.");
        frames.ReplaceTop(frame with { PendingMovementContinuation = null });
        frames.AdvanceProgram(frame.Id);
        return true;
    }
}
