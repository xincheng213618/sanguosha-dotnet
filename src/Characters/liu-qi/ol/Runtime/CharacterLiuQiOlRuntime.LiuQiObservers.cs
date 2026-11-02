#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 450)]
    internal void ObserveLiuQiEvent(IGameEvent payload)
    {
        if (!_characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi)
            return;
        if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } response } && _characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi)
        {
            var window = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault();
            if (window is null || window.ChainDepth <= 0 || !_coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(window) || response.ParentActionId != _coreDelayedNullificationOperations.Value.NullificationParentActionId(window) || response.ActorSeat != response.ProviderSeat || response.ResponderSeat != response.ActorSeat || response.RequesterSeat is not null || response.OpponentSeat != window.SourceSeat)
                return;
            var grant = (_coreMatchParticipantsPlayersAccess._players[response.ActorSeat].WenjiTurnNameGrants ?? []).LastOrDefault(g => g.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.Source.OwnerSeat == response.ActorSeat && response.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && WenjiName(g.Name) == WenjiName(CardKind.Nullification));
            if (grant is null)
                return;
            if (response.PhysicalCards.Count == 0 && !_coreTieredRoundZeroResponsesOperations.IsIssuedTieredRoundZeroResponse(window, response))
                return;
            if (response.PhysicalCards.Any(c => !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NullificationRespondedEvent>().Any(e => e.ResolutionId == window.ParentFrameId && e.ChainDepth == window.ChainDepth && e.ResponderSeat == response.ActorSeat && e.NullificationCardId == c.CardId)))
                return;
            var qualification = new WenjiCounterspellQualification(grant, window.Id, window.ParentFrameId, window.ChainDepth, response);
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { WenjiCounterspell = qualification });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenjiCounterspellQualifiedEvent(window.Id, window.ParentFrameId, window.ChainDepth, response.ActionId, grant));
        }
        else if (payload is TurnEndedEvent ended)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ended.ActorSeat];
            owner.WenjiTurnNameGrants = null;
        }
        else if (payload is PlayerDiedEvent died)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[died.VictimSeat];
            owner.WenjiTurnNameGrants = null;
        }
    }

    internal bool LiuQiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueLiuQi(root) || child is not CardsMovedTriggerWindowFrame moved || root.LiuQi is not { } r)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == WenjiGiftReason);
    }

    internal ProgramSkillFrame? LiuQiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.LiuQiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 220)]
    internal bool TryAdvanceLiuQiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuQiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
