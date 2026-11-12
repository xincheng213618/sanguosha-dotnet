#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoHappyGodRuntime
{
    internal bool HappyZlMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.HappyZhangLiao is not { } r || e?.Op != r.Op || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || r.Stage is not (HappyZlStage.GainChildren or HappyZlStage.SlotChildren or HappyZlStage.RestoreChildren or HappyZlStage.DrawChildren or HappyZlStage.DiscardChildren))
            return false;
        return r.Payments.Count > 0 && (r.Stage != HappyZlStage.DrawChildren || r.Draw is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == r.Restored);
    }

    internal bool HappyZlFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HappyZhangLiao is not { } r || f.PendingMovementContinuation is not { } p || !HappyZlMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return false;
        var moves = r.Payments.Last().Moves;
        if (child is CardsMovedTriggerWindowFrame m)
            return m.Batch.ParentFrameId == f.Id && m.Batch.OriginOwnerSeat == f.OwnerSeat && m.Batch.OriginSkillId == f.SkillId && m.Batch.OriginSkillInstanceId == f.SkillInstanceId && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, m) && m.Batch.Movements.All(x => moves.Contains(x));
        if (child is RecoveryReplacementFrame recovery)
            return moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == recovery.Attempt.TargetSeat) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == hp.Change.TargetSeat);
        return false;
    }

    internal ProgramSkillFrame? HappyZlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HappyZlFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40980)]
    internal bool HappyZlDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && HappyZlObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40980)]
    internal bool AdvanceHappyZlChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyZlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40980)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40980)]
    internal bool RetainHappyZlPaid(ProgramSkillFrame f)
    {
        if (f.HappyZhangLiao is not { Payments.Count: > 0 })
            return false;
        return true;
    }
}
