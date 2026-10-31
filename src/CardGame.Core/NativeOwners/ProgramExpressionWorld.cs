#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramExpressionWorld(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramExpressionsEntries> getCoreGameEngineProgramExpressionsRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries> getCoreGameEngineHealthQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries> getCoreNativeRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, ProgramSkillFrame frame) : IProgramExpressionWorld
{
    public int? Number(string name) => getCoreGameEngineProgramExpressionsRuntime().ReadProgramExpressionNumber(frame, name);
    public int? BindingStat(string stat, string name)
    {
        if (frame.CardSetBindings.SingleOrDefault(binding => binding.Name == name)is not { } binding)
            return null;
        if (stat == "boundCount")
            return binding.CardIds.Count;
        if (binding.CardIds.Count == 0 && stat is "boundSuitCount" or "boundDistinctSuitCount")
            return 0;
        if (binding.Visibility != SkillProgramCardSetVisibility.Public && binding.SourceLocations.Any(location => location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return null;
        var cards = binding.CardIds.Select(id => getCardZoneStore().CardsAt(getCardZoneStore().GetLocation(id)).Single(card => card.Id == id)).ToArray();
        return stat switch
        {
            "boundSuitCount" => (binding.FrozenSelectedSuits ?? cards.Select(card => card.Suit).ToArray()).Where(suit => suit != Suit.None).Distinct().Count(),
            "boundDistinctSuitCount" => (binding.FrozenSelectedSuits ?? cards.Select(card => card.Suit).ToArray()).Distinct().Count(),
            "boundRank" => cards is [var card] ? card.Rank : null,
            "boundIsEquipment" => cards is [var card] ? EquipmentCatalog.IsEquipment(card.Kind) ? 1 : 0 : null,
            _ => null
        };
    }

    public int? Participant(string name) => Valid(name switch
    {
        "owner" => frame.OwnerSeat,
        "selectedTarget" => frame.SelectedTargetSeats is [var seat] ? seat : null,
        "eventSource" => frame.WindowContext?.SourceSeat,
        "eventTarget" => frame.WindowContext?.TargetSeat,
        _ => null
    });
    public IReadOnlyList<int>? ParticipantSet(string name) => name switch
    {
        "living" => matchParticipantsState.ReferenceToPlayers.Where(player => player.IsAlive).Select(player => player.Seat).ToArray(),
        "livingOthers" => matchParticipantsState.ReferenceToPlayers.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat).Select(player => player.Seat).ToArray(),
        _ => null
    };
    public int? Stat(string stat, int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        return stat switch
        {
            "hp" => getCoreGameEngineHealthQueriesRuntime().UnambiguousHp(player),
            "lostHp" => getCoreGameEngineHealthQueriesRuntime().UnambiguousHp(player)is { } hp ? player.MaxHp - hp : null,
            "maxHp" => player.MaxHp,
            "handCount" => getCorePhysicalOwnedCardQueriesRuntime().GetHand(player).Count,
            "handLimit" => getCoreNativeRuleQueriesRuntime().GetHandLimit(player),
            "equipmentCount" => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(player).Count,
            _ => null
        };
    }

    public bool? Flag(string flag, int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        return flag switch
        {
            "alive" => player.IsAlive,
            "hasHp" => player.HasHpValue,
            "faceDown" => player.IsFaceDown,
            "chained" => player.IsChained,
            _ => null
        };
    }

    private int? Valid(int? seat) => seat is { } value && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(value) ? value : null;
}
