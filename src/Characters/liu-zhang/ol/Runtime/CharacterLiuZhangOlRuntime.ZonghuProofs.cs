#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal bool ZonghuNativeNeedCurrent(ZonghuReceipt r)
    {
        if (r.Need.Intent == ZonghuIntent.Play)
            return r.Need.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && r.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
        var original = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Need.RequestFrameId);
        if (original is null || CaptureZonghuNeed(r.OriginalDecision, original)is not { } n)
            return false;
        if (SameZonghuNeed(n, r.Need))
            return true;
        return r.Paid is not null && r.TargetSeat is { } target && r.Need.Targets.Contains(target) && n.Targets.Contains(target) && SameZonghuNeed(n with { Targets = r.Need.Targets }, r.Need);
    }

    internal bool ValidZonghuPayment(ZonghuPaidOrigin p)
    {
        if (p.AttemptId <= p.Need.RequestFrameId || p.Source.SkillId != "ol:zonghu" || p.Source.BindingId != "native-use-request" || p.Source.OwnerSeat != p.Need.ActorSeat || p.Need.ProviderSeat != p.Need.ActorSeat || p.Need.RequesterSeat is not null || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } program || program.GameplayHash != p.GameplayHash || p.ActualTurn <= 0 || p.RequiredCount is < 1 or > 3 || p.Gifts.Count != p.RequiredCount || p.Gifts.Select(g => g.CardId).Distinct().Count() != p.Gifts.Count || p.Before < 0 || p.After <= p.Before)
            return false;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuCostPaidEvent>().Where(e => e.Source.OwnerSeat == p.Source.OwnerSeat && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner).ToArray()is not [var paid] || paid != new ZonghuCostPaidEvent(p.AttemptId, p.Source, p.GameplayHash, p.ActualTurn, p.ActualTurnOwner, p.RecipientSeat, p.RequiredCount, p.Before, p.After))
            return false;
        for (var i = 0; i < p.Gifts.Count; i++)
        {
            var g = p.Gifts[i];
            if (g.From != CardLocation.Hand(p.Source.OwnerSeat) && g.From != CardLocation.Equipment(p.Source.OwnerSeat) || g.Sequence <= p.Before || g.Sequence > p.After || g.BatchId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuGiftSelectedEvent>().Count(e => e == new ZonghuGiftSelectedEvent(p.AttemptId, i, g.CardId)) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuGiftPaidEvent>().Count(e => e == new ZonghuGiftPaidEvent(p.AttemptId, i, g.CardId, g.Kind, g.From, p.RecipientSeat, g.Sequence, g.BatchId)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == g.Sequence && m.CardId == g.CardId && m.From == g.From && m.To == CardLocation.Hand(p.RecipientSeat) && m.Reason.Value == ZonghuGiftReason) != 1)
                return false;
        }

        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuDeclaredEvent>().Count(e => e == new ZonghuDeclaredEvent(p.AttemptId, p.OutputKind, p.TargetSeat)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuRecipientSelectedEvent>().Count(e => e == new ZonghuRecipientSelectedEvent(p.AttemptId, p.RecipientSeat)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuStartedEvent>().Count(e => e.FrameId == p.AttemptId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner && e.Intent == p.Need.Intent && e.OwnerFrameId == p.Need.OwnerFrameId && e.RequestFrameId == p.Need.RequestFrameId) == 1;
    }

    internal bool ZonghuParent(ProgramSkillFrame f)
    {
        if (f.Zonghu is not { } r)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (r.Need.Intent == ZonghuIntent.Play)
            return index == 0 && r.Need.OwnerFrameId == 0 && r.Need.RequestFrameId == 0 && r.Need.ActorSeat == f.OwnerSeat;
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.Need.RequestFrameId)
            return false;
        var native = _resolutionStackOperations._resolutionStack[index - 1];
        return native is ResponseWindowFrame response ? response.ParentFrameId == r.Need.OwnerFrameId : native.Id == r.Need.OwnerFrameId && native is CardUseFrame or ProgramSkillFrame or DyingFrame;
    }
    internal bool ZonghuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zonghu is not { Stage: ZonghuStage.CostChildren, Paid: { } p } r)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuZhangRecoveryParent(f, recovery);
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsZonghuGiftMovement(f, m));
    }

    internal bool IsZonghuGiftMovement(ProgramSkillFrame f, CardMovementRecord m) => f.Zonghu is { Stage: ZonghuStage.CostChildren, Paid: { } paid } r && m.Sequence > r.Before && m.Sequence <= r.After && (paid.Gifts.Any(g => g.Sequence == m.Sequence && g.CardId == m.CardId && g.From == m.From && m.To == CardLocation.Hand(paid.RecipientSeat) && m.Reason.Value == ZonghuGiftReason) || r.GrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && paid.Gifts.Any(g => g.Kind == CardKind.WoodenOx && g.From == CardLocation.Equipment(f.OwnerSeat)));
    internal ProgramSkillFrame? ZonghuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.ZonghuFirstChild(f, c) || this.FengweiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27512)]
    internal bool AdvanceZonghuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZonghuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
