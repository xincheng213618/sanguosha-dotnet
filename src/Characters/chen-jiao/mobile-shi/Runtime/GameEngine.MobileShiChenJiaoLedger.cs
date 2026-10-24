namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasChenJiaoCapability =>
        _contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveChenJiaoQingYan);
    private bool ChenJiaoHasSkill(int seat, string skillId) =>
        _players[seat].SkillGrants.Grants.Any(grant => grant.IsEnabled && grant.SkillId == skillId) ||
        EnabledContentSkillIds(_players[seat]).Contains(skillId, StringComparer.Ordinal);

    private static bool ChenJiaoIsRed(Card card) => card.Suit is Suit.Heart or Suit.Diamond;

    // 清严's lock is derived on read: a shown card still at its owner's hand keeps
    // the skill失效; once the card has left the hand the condition was met again.
    private bool ChenJiaoQingYanLocked(int owner) =>
        ChenJiaoQingYanPendingIds(owner).Count > 0;

    private IReadOnlyList<int> ChenJiaoQingYanPendingIds(int owner) =>
        ProgramEventHistory<ChenJiaoQingYanShownEvent>()
            .Where(entry => entry.OwnerSeat == owner)
            .Select(entry => entry.CardId).Distinct()
            .Where(cardId => _cardZones.GetLocation(cardId) is { Zone: CardZoneKind.Hand, OwnerSeat: var seat } &&
                seat == owner)
            .ToArray();

    private int ChenJiaoQingYanUsesThisRound(int owner) =>
        ProgramEventHistory<ChenJiaoQingYanUsedEvent>()
            .Count(entry => entry.Round == _roundNumber && entry.OwnerSeat == owner);

    private IReadOnlyList<int> ChenJiaoCeDuanShownIdsThisActivation(long frameId) =>
        ProgramEventHistory<ChenJiaoCeDuanShownEvent>()
            .Where(entry => entry.FrameId == frameId)
            .Select(entry => entry.CardId).ToArray();

    private int ChenJiaoCeDuanDamageCount(int sourceSeat, int targetSeat) =>
        ProgramEventHistory<DamageAppliedEvent>()
            .Count(entry => entry.SourceSeat == sourceSeat && entry.TargetSeat == targetSeat);

    // The 清严 marker mirrors how many shown cards still sit in the owner's hand.
    private void SyncChenJiaoMarker(int seat)
    {
        if (!HasChenJiaoCapability || !IsValidPlayerSeat(seat) || !_players[seat].IsAlive) return;
        var count = ChenJiaoQingYanPendingIds(seat).Count;
        var player = _players[seat];
        var before = player.Markers.GetValueOrDefault(PlayerMarkerKind.ChenJiaoQingYanShown);
        if (before == count) return;
        var key = (PlayerMarkerKind.ChenJiaoQingYanShown, seat);
        if (count == 0)
        { player.Markers.Remove(PlayerMarkerKind.ChenJiaoQingYanShown); player.MarkerSourceCounts.Remove(key); }
        else
        {
            player.Markers[PlayerMarkerKind.ChenJiaoQingYanShown] = count;
            player.MarkerSourceCounts[key] = count;
        }
        AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(_resolutionSequence, seat,
            PlayerMarkerKind.ChenJiaoQingYanShown, count - before, count, seat,
            "skill-program.mobile:shi-qingyan.sync"));
    }

    // Observes completed engine facts only: card movements resync the shown marker.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68100)]
    private void ObserveChenJiaoFacts(IGameEvent payload)
    {
        if (!HasChenJiaoCapability || _winner != Winner.None) return;
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
