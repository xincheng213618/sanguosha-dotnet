#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenJiaoMobileShiRuntime
{
    internal bool HasChenJiaoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveChenJiaoQingYan);

    internal bool ChenJiaoHasSkill(int seat, string skillId) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Any(grant => grant.IsEnabled && grant.SkillId == skillId) || _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[seat]).Contains(skillId, StringComparer.Ordinal);
    internal static bool ChenJiaoIsRed(Card card) => card.Suit is Suit.Heart or Suit.Diamond;
    internal bool ChenJiaoQingYanLocked(int owner) => ChenJiaoQingYanPendingIds(owner).Count > 0;
    internal IReadOnlyList<int> ChenJiaoQingYanPendingIds(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ChenJiaoQingYanShownEvent>().Where(entry => entry.OwnerSeat == owner).Select(entry => entry.CardId).Distinct().Where(cardId => _cardZonesOperations._cardZones.GetLocation(cardId)is { Zone: CardZoneKind.Hand, OwnerSeat: var seat } && seat == owner).ToArray();
    internal int ChenJiaoQingYanUsesThisRound(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ChenJiaoQingYanUsedEvent>().Count(entry => entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && entry.OwnerSeat == owner);
    internal IReadOnlyList<int> ChenJiaoCeDuanShownIdsThisActivation(long frameId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ChenJiaoCeDuanShownEvent>().Where(entry => entry.FrameId == frameId).Select(entry => entry.CardId).ToArray();
    internal int ChenJiaoCeDuanDamageCount(int sourceSeat, int targetSeat) => _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(entry => entry.SourceSeat == sourceSeat && entry.TargetSeat == targetSeat);
    internal void SyncChenJiaoMarker(int seat)
    {
        if (!HasChenJiaoCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return;
        var count = ChenJiaoQingYanPendingIds(seat).Count;
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = player.Markers.GetValueOrDefault(PlayerMarkerKind.ChenJiaoQingYanShown);
        if (before == count)
            return;
        var key = (PlayerMarkerKind.ChenJiaoQingYanShown, seat);
        if (count == 0)
        {
            player.Markers.Remove(PlayerMarkerKind.ChenJiaoQingYanShown);
            player.MarkerSourceCounts.Remove(key);
        }
        else
        {
            player.Markers[PlayerMarkerKind.ChenJiaoQingYanShown] = count;
            player.MarkerSourceCounts[key] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(_coreResolutionIdentityBoundaryOperations._resolutionSequence, seat, PlayerMarkerKind.ChenJiaoQingYanShown, count - before, count, seat, "skill-program.mobile:shi-qingyan.sync"));
    }

    // Observes completed engine facts only: card movements resync the shown marker.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68100)]
    internal void ObserveChenJiaoFacts(IGameEvent payload)
    {
        if (!HasChenJiaoCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        switch (payload)
        {
            case CardMovedEvent { CardId: var cardId, From: var from, To: var to }:
                if (from.OwnerSeat is { } lost && (from.Zone == CardZoneKind.Hand || to.Zone == CardZoneKind.Hand))
                    SyncChenJiaoMarker(lost);
                if (to.OwnerSeat is { } gained && to.Zone == CardZoneKind.Hand)
                    SyncChenJiaoMarker(gained);
                break;
        }
    }
}
