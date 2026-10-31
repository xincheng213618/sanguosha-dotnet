#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal bool CanContinueIssuedGuanHai(ProgramSkillFrame f)
    {
        if (f.GuanHai is { Paid: true })
        {
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.TriggerId is null || f.WindowContext is not { } c)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        return IsIssuedXieweiCandidate(candidate, c) || IsIssuedGuanHaiYouqueCandidate(candidate, c);
    }
    internal bool GuanHaiMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.GuanHai!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == GuanHaiStage.XieweiChildren)
            return _coreCardUseLifecycleOperations.LifecycleCardUse(r.NativeUseId)?.XieweiPayments?.Any(p => p.Id == r.PaymentId && ValidXieweiPayment(p) && p.Movements.Contains(m)) == true;
        if (r.Stage == GuanHaiStage.Pindian && _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().SingleOrDefault(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId)is { Result: { } result })
            return new[]
            {
                result.SourceCardId,
                result.OpponentCardId
            }.Contains(m.CardId) && (m.Reason == CardMoveReasons.PindianReveal && m.To == CardLocation.Processing && (m.From == (m.CardId == result.SourceCardId ? r.Pile!.Location : CardLocation.Hand(result.OpponentSeat)) || m.From == CardLocation.DrawPile) || m.Reason == CardMoveReasons.PindianFinish && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile || m.Reason.Value == "program.pindian.claim" && m.From == CardLocation.Processing && m.To.Zone == CardZoneKind.Hand || m.Reason.Value == "pindian.reserve-top" && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        if (m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == GuanHaiStage.DiscardChildren)
            return r.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == CardLocation.DiscardPile && m.Reason.Value == YouqueDiscardReason;
        if (r.Stage == GuanHaiStage.RewardChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == YouqueDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return false; // The zero-entity native Slash has no material settlement.
    }

    internal bool GuanHaiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.GuanHai is not { Paid: true } r)
            return false;
        if (child is PindianFrame p)
            return r.Stage == GuanHaiStage.Pindian && p.ParentFrameId == f.Id && p.Id == r.PindianId && IsGuanHaiPindianSource(p, _corePindianOwnedPileSourcesOperations.HasPaidOwnedPilePindianSource(p));
        if (child is CardUseFrame use)
            return r.Stage == GuanHaiStage.NativeSlash && use.YouqueSlash?.ProgramFrameId == f.Id && IsIssuedYouqueUse(use);
        if (r.Stage == GuanHaiStage.RewardDraw && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage is GuanHaiStage.XieweiChildren or GuanHaiStage.DiscardChildren or GuanHaiStage.RewardChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is not CardsMovedTriggerWindowFrame moved || moved.Batch.Movements.Count == 0 || !moved.Batch.Movements.All(m => GuanHaiMovementMatches(f, m)))
            return false;
        return r.Stage == GuanHaiStage.XieweiChildren ? moved.Batch.ParentFrameId == r.NativeUseId && (moved.Batch.AwaitingProgramFrameId == f.Id || moved.ResumeProgramFrameId == f.Id) : CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && (r.Stage == GuanHaiStage.Pindian ? moved.Batch.ParentFrameId == r.PindianId : moved.Batch.ParentFrameId == f.Id);
    }

    internal ProgramSkillFrame? GuanHaiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.GuanHaiFirstChild(f, child), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 270)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 260)]
    internal bool IsGuanHaiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.GuanHai is not { Paid: true } || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return false;
        return _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id) > _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id) && GuanHaiObserverRoot()?.Id == f.Id;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27578)]
    internal bool AdvanceGuanHaiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && GuanHaiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
