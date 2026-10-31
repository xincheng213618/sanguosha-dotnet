#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiHappyGodRuntime
{
    internal bool HappyZjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.HappyZhenJi is not { } r || e?.Op != r.Op || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || r.Payments.Count == 0 || r.Stage is not (HappyZjStage.EffectChildren or HappyZjStage.RewardChildren or HappyZjStage.StoreChildren or HappyZjStage.FeifuChildren or HappyZjStage.ReturnChildren))
            return false;
        return true;
    }

    internal bool HappyZjFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HappyZhenJi is not { } r)
            return false;
        if (r.Stage == HappyZjStage.Damaging && f.AttackAttempt is not null && f.AttackReturn is not null)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (f.PendingMovementContinuation is not { } p || !HappyZjMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return false;
        var moves = r.Payments.Last().Moves;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => moves.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == recovery.Attempt.TargetSeat) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && moves.Any(m => m.From.Zone == CardZoneKind.Equipment && m.CardKind == CardKind.SilverLion && m.From.OwnerSeat == hp.Change.TargetSeat);
        return false;
    }

    internal ProgramSkillFrame? HappyZjObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HappyZjFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 41240)]
    internal bool HappyZjDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && HappyZjObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 41240)]
    internal bool AdvanceHappyZjChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyZjObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41240)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41240)]
    internal bool RetainHappyZjPaid(ProgramSkillFrame f) => f.HappyZhenJi is { Stage: not (HappyZjStage.Branch or HappyZjStage.Discard or HappyZjStage.SelectStore) };
}
