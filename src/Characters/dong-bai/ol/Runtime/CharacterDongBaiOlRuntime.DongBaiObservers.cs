#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongBaiOlRuntime
{
    [ProgramCardLegality(ProgramCardLegalityStage.HandPhysicalBan, 10)]
    internal bool IsXiaHuiPhysicalRestricted(int actorSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && _coreMatchParticipantsPlayersAccess._players[actorSeat].XiaHuiRestrictions?.Any(r => r.RecipientSeat == actorSeat && r.CardId == cardId && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(actorSeat)) == true;
    internal bool IsXiaHuiDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => intent == OwnedCardMoveIntent.Discard && from == CardLocation.Hand(actorSeat) && IsXiaHuiPhysicalRestricted(actorSeat, card.Id);
    internal bool IsXiaHuiHandLimitExempt(int owner, Card card) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner) && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[owner], SkillProgramCardPolicyKind.XiaHuiBlackHandAndGainedEntityRestrictions).Any() && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card)is Suit.Spade or Suit.Club;
    internal void ObserveDongBaiMovement(CardMovementRecord movement, IReadOnlyList<XiaHuiMovementSource>? originalSources = null)
    {
        if (movement.From is { Zone: CardZoneKind.Hand, OwnerSeat: { } departed } && _coreMatchParticipantsPlayersAccess._players[departed].XiaHuiRestrictions is { Count: > 0 } old)
        {
            foreach (var r in old.Where(r => r.CardId == movement.CardId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiaHuiRestrictionReleasedEvent(departed, r.CardId, movement.Sequence, "entity-left-hand"));
            _coreMatchParticipantsPlayersAccess._players[departed].XiaHuiRestrictions = Array.AsReadOnly(old.Where(r => r.CardId != movement.CardId).ToArray());
        }

        if (movement.From.OwnerSeat is not { } owner || movement.To is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || owner == recipient || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || originalSources is null)
            return;
        foreach (var item in originalSources.Where(s => s.Source.OwnerSeat == owner && s.OriginalSuit is Suit.Spade or Suit.Club))
        {
            var restriction = new XiaHuiCardRestriction(item.Source, item.GameplayHash, recipient, movement.CardId, movement.Sequence, item.OriginalSuit);
            var recipientExisting = _coreMatchParticipantsPlayersAccess._players[recipient].XiaHuiRestrictions ?? [];
            if (recipientExisting.Any(r => r.Source == restriction.Source && r.CardId == restriction.CardId && r.MovementSequence == movement.Sequence))
                continue;
            _coreMatchParticipantsPlayersAccess._players[recipient].XiaHuiRestrictions = Array.AsReadOnly(recipientExisting.Append(restriction).ToArray());
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiaHuiRestrictionGrantedEvent(restriction));
        }
    }

    internal void ObserveDongBaiHpDecreased(int seat, int before, int after)
    {
        if (after >= before || _coreMatchParticipantsPlayersAccess._players[seat].XiaHuiRestrictions is not { Count: > 0 } restrictions)
            return;
        _coreMatchParticipantsPlayersAccess._players[seat].XiaHuiRestrictions = null;
        foreach (var r in restrictions)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiaHuiRestrictionReleasedEvent(seat, r.CardId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, "recipient-hp-decreased"));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 580)]
    internal void ObserveDongBaiRecipientDeath(PlayerDiedEvent died) => CleanupDongBaiRecipientDeath(died.VictimSeat);
    internal void CleanupDongBaiRecipientDeath(int seat)
    {
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreMatchParticipantsPlayersAccess._players[seat].XiaHuiRestrictions is not { Count: > 0 } restrictions)
            return;
        _coreMatchParticipantsPlayersAccess._players[seat].XiaHuiRestrictions = null;
        foreach (var r in restrictions)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiaHuiRestrictionReleasedEvent(seat, r.CardId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, "recipient-died"));
    }

    internal bool LianZhuFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueLianZhu(root) || root.LianZhu is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == LianZhuReason(r.Stage));
    }

    internal ProgramSkillFrame? LianZhuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterDongBaiOlOperations.LianZhuFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 140)]
    internal bool TryAdvanceLianZhuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LianZhuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    [ProgramPipelineHook(ProgramPipelinePhase.ObserveCommittedMovement, 100)]
    internal void ObserveMovementBatchDongBai(CardMovementBatchBuilder batch, CardMovementRecord movement) => ObserveDongBaiMovement(movement, batch.XiaHuiOriginalSources is { } sources && sources.TryGetValue(movement.CardId, out var originalSources) ? originalSources : null);
}
