#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangMobileShiRuntime
{
    internal IReadOnlyList<int> ShiCaoShuangMarkedIds => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangSheQuanMarkedEvent>().Select(entry => entry.CardId).Distinct().ToArray();

    internal bool ShiCaoShuangIsMarkedCard(int cardId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangSheQuanMarkedEvent>().Any(entry => entry.CardId == cardId);
    internal int ShiCaoShuangSheQuanRank(int holderSeat, int cardId) => ShiCaoShuangIsMarkedCard(cardId) && ShiCaoShuangHasSkill(holderSeat, ShiCaoShuangComposition.SheQuanSkill) ? 13 : 1;
    internal int ShiCaoShuangRoundTriggers(bool dianyi, int owner, string windowKind) => dianyi ? _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangDianYiRoundTriggerEvent>().Count(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.WindowKind == windowKind && entry.OwnerSeat == owner) : _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangSheQuanRoundTriggerEvent>().Count(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.WindowKind == windowKind && entry.OwnerSeat == owner);
    internal int ShiCaoShuangDianYiUsesThisRound(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangDianYiRoundTriggerEvent>().Count(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.OwnerSeat == owner);
    internal int ShiCaoShuangDianYiGainsThisRound(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangDianYiGainEvent>().Where(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber).Sum(entry => entry.Count);
    internal int ShiCaoShuangTimings(string skillId, string? windowKind) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangJianZhuanTimingEvent>().Count(entry => entry.SkillId == skillId && (windowKind is null || entry.WindowKind == windowKind));
    internal IReadOnlyList<ShiCaoShuangNiZunUsedEvent> ShiCaoShuangNiZunUseEntriesThisRound(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangNiZunUsedEvent>().Where(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber).ToArray();
    internal int ShiCaoShuangNiZunUsesThisRound(int owner) => ShiCaoShuangNiZunUseEntriesThisRound(owner).Count;
    internal IReadOnlyList<int> ShiCaoShuangMarkedInOtherHands(int owner) => ShiCaoShuangMarkedIds.Where(cardId => _cardZonesOperations._cardZones.GetLocation(cardId)is { Zone: CardZoneKind.Hand, OwnerSeat: { } seat } && seat != owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
    internal IReadOnlyList<int> ShiCaoShuangStashableIds(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangMarkedCardDiscardedEvent>().Where(entry => entry.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber).Select(entry => entry.CardId).Distinct().Where(cardId => _cardZonesOperations._cardZones.GetLocation(cardId)is { Zone: CardZoneKind.DiscardPile }).ToArray();
    internal IReadOnlyList<int> ShiCaoShuangStashedIds(int owner)
    {
        if (!_cardZonesOperations._cardZones.HasPrivatePersistentCards(owner))
            return[];
        var location = new CardLocation(CardZoneKind.PrivatePersistentPile, owner, ShiCaoShuangComposition.NiZunPileId);
        return _cardZonesOperations._cardZones.CardsAt(location).Select(card => card.Id).ToArray();
    }

    internal void SyncShiCaoShuangMarker(int seat)
    {
        if (!HasShiCaoShuangCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return;
        var count = ShiCaoShuangMarkedIds.Select(cardId => _cardZonesOperations._cardZones.GetLocation(cardId)).Count(location => location.Zone == CardZoneKind.Hand && location.OwnerSeat == seat);
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = player.Markers.GetValueOrDefault(PlayerMarkerKind.ShiCaoShuangSheQuan);
        if (before == count)
            return;
        var key = (PlayerMarkerKind.ShiCaoShuangSheQuan, seat);
        if (count == 0)
        {
            player.Markers.Remove(PlayerMarkerKind.ShiCaoShuangSheQuan);
            player.MarkerSourceCounts.Remove(key);
        }
        else
        {
            player.Markers[PlayerMarkerKind.ShiCaoShuangSheQuan] = count;
            player.MarkerSourceCounts[key] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(_coreResolutionIdentityBoundaryOperations._resolutionSequence, seat, PlayerMarkerKind.ShiCaoShuangSheQuan, count - before, count, seat, "skill-program.mobile:shi-shequan.sync"));
    }

    // Observes completed engine facts only: the owner's used cards per turn,
    // marked cards entering the discard pile, and damage consuming the pending
    // NiZun penalty.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 67900)]
    internal void ObserveShiCaoShuangTurnFacts(IGameEvent payload)
    {
        if (!HasShiCaoShuangCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        switch (payload)
        {
            case CardUsedEvent { CardId: var cardId, SourceSeat: var actor }when _coreActualTurnProgressTurnNumberAccess._turnNumber > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && ShiCaoShuangHasSkill(actor, ShiCaoShuangComposition.NiZunSkill):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoShuangOwnerCardUsedEvent(_coreActualTurnProgressTurnNumberAccess._turnNumber, cardId));
                break;
            case CardMovedEvent { CardId: var cardId, From: var from, To: var to }when ShiCaoShuangIsMarkedCard(cardId):
                if (to.Zone == CardZoneKind.DiscardPile && _coreActualTurnProgressTurnNumberAccess._turnNumber > 0)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoShuangMarkedCardDiscardedEvent(_coreActualTurnProgressTurnNumberAccess._turnNumber, cardId, from.OwnerSeat ?? -1));
                if (from.OwnerSeat is { } lost)
                    SyncShiCaoShuangMarker(lost);
                if (to.OwnerSeat is { } gained)
                    SyncShiCaoShuangMarker(gained);
                break;
            case DamageAppliedEvent { TargetSeat: var target }when _coreMatchParticipantsPlayersAccess._players[target].IsAlive && ShiCaoShuangHasSkill(target, ShiCaoShuangComposition.NiZunSkill) && ShiCaoShuangNiZunUseEntriesThisRound(target).Count > 0 && ShiCaoShuangNiZunDamagePenaltyPending(target):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoShuangNiZunPenaltyConsumedEvent(_coreActualRoundProgressRoundNumberAccess._roundNumber, ShiCaoShuangNiZunUsesThisRound(target)));
                break;
        }
    }

    internal bool ShiCaoShuangNiZunDamagePenaltyPending(int target)
    {
        var usesTotal = ShiCaoShuangNiZunUsesThisRound(target);
        if (usesTotal == 0)
            return false;
        var lastConsumed = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiCaoShuangNiZunPenaltyConsumedEvent>().LastOrDefault(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        return lastConsumed is null || usesTotal - lastConsumed.Count > 0;
    }
}
