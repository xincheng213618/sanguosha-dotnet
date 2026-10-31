#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    internal void ObserveLiuQiPlayBoundary(int ownerSeat, int phaseInstance, bool skipped)
    {
        if (!HasTunjiangCapability || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || ownerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var entry = new LiuQiPlayEntry(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ownerSeat, phaseInstance, skipped);
        if ((owner.LiuQiPlayEntries ?? []).Contains(entry))
            return;
        owner.LiuQiPlayEntries = (owner.LiuQiPlayEntries ?? []).Append(entry).ToArray();
        _characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuQiPlayEnteredEvent(entry));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 450)]
    internal void ObserveLiuQiEvent(IGameEvent payload)
    {
        if (!_characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi && !HasTunjiangCapability)
            return;
        if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } response } && _characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi)
        {
            var window = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault();
            if (window is null || window.ChainDepth <= 0 || !_coreDelayedNullificationOperations.HasNullificationEffectOwner(window) || response.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(window) || response.ActorSeat != response.ProviderSeat || response.ResponderSeat != response.ActorSeat || response.RequesterSeat is not null || response.OpponentSeat != window.SourceSeat)
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
        else if (payload is TargetsConfirmedEvent targets && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(targets.ResolutionId)is { } use && use.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && targets.TargetSeats.SequenceEqual(use.TargetSeats))
        {
            var action = use.Action;
            if (action is not null && (action.Type != CardActionType.Use || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind))
                return;
            if (action is null && (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use)))
                return;
            if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id && e.SourceSeat == use.SourceSeat && e.CardId == use.CardId && e.CardKind == use.CardKind))
                return;
            var seats = use.CardKind == CardKind.BorrowedSword ? targets.TargetSeats.Where((_, i) => i % 2 == 0) : targets.TargetSeats;
            foreach (var target in seats.Where(s => s != use.SourceSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(s)).Distinct())
            {
                var receipt = new LiuQiForeignTargetUse(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, use.SourceSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, use.Id, action?.ActionId, target);
                var owner = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
                if ((owner.LiuQiForeignTargetUses ?? []).Contains(receipt))
                    continue;
                owner.LiuQiForeignTargetUses = (owner.LiuQiForeignTargetUses ?? []).Append(receipt).ToArray();
                _characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi = true;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuQiForeignUseRecordedEvent(receipt));
            }
        }
        else if (payload is TurnEndedEvent ended)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ended.ActorSeat];
            owner.WenjiTurnNameGrants = null;
            owner.LiuQiPlayEntries = null;
            owner.LiuQiForeignTargetUses = null;
        }
        else if (payload is PlayerDiedEvent died)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[died.VictimSeat];
            owner.WenjiTurnNameGrants = null;
            owner.LiuQiPlayEntries = null;
            owner.LiuQiForeignTargetUses = null;
        }
    }

    internal bool HasUntargetedUnskippedLiuQiPlay(int owner)
    {
        if (owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || _coreMatchParticipantsPlayersAccess._players[owner].LiuQiPlayEntries is not { Count: > 0 } entries)
            return false;
        return entries.Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && !e.Skipped) && !entries.Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Skipped) && !(_coreMatchParticipantsPlayersAccess._players[owner].LiuQiForeignTargetUses ?? []).Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    }

    internal bool LiuQiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueLiuQi(root) || child is not CardsMovedTriggerWindowFrame moved || root.LiuQi is not { } r)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == (r.Stage == LiuQiStage.GiftChildren ? WenjiGiftReason : TunjiangDrawReason));
    }

    internal ProgramSkillFrame? LiuQiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.LiuQiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 220)]
    internal bool TryAdvanceLiuQiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuQiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
