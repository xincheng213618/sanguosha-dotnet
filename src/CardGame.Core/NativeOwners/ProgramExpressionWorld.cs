#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramExpressionWorld(Func<RuntimeEntries.CoreGameEngineProgramExpressionsEntries> getCoreGameEngineProgramExpressionsRuntime, Func<CardZoneStore> getCardZoneStore, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineHealthQueriesEntries> getCoreGameEngineHealthQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreNativeRuleQueriesEntries> getCoreNativeRuleQueriesRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, ProgramSkillFrame? frame, ICardEntityMarkQueries? cardMarks = null, ISelectionWoundedQueries? wounded = null, RuntimeEntries.CoreGameEnginePublicPileSourcesEntries? piles = null, Func<int, int?>? equipmentSuitCount = null,
    int? triggerOwner = null, ProgramSkillWindowContext? triggerContext = null,
    SkillProgramTriggerFacts? triggerFacts = null, IActualCardUseHistoryQueries? history = null,
    Func<CardUseHistoryPoint>? currentHistoryPoint = null,
    IActualPhaseHistoryQueries? phaseHistory = null) : IProgramExpressionWorld
{
    private int Owner => frame?.OwnerSeat ?? triggerOwner!.Value;
    private ProgramSkillWindowContext? Context => frame?.WindowContext ?? triggerContext;
    private CardUseHistoryPoint? WindowPoint => Context?.HistoryPoint ?? Context?.Facts?.HistoryPoint ?? triggerFacts?.HistoryPoint;
    public int? Number(string name) => frame is null ? null : getCoreGameEngineProgramExpressionsRuntime().ReadProgramExpressionNumber(frame, name);
    public int? WindowStat(string stat, string participant) =>
        ProgramExpressions.ReadWindowStat((Context?.Facts ?? triggerFacts), stat, participant, Context?.TargetSeat is not null);
    public int? CardMarkCount(int seat, string mark, CardZoneKind zone) =>
        getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) ? cardMarks?.CountMarked(seat, mark, zone) : null;
    public int? HandPrintedSuitCount(int seat, Suit suit) => Valid(seat) is null ? null :
        getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count(card => card.Suit == suit);
    public bool? SameTeam(int first, int second) => Valid(first) is null || Valid(second) is null ? null :
        matchParticipantsState.ReferenceToPlayers[first].TeamId is { } team &&
        matchParticipantsState.ReferenceToPlayers[second].TeamId == team;
    public int? PublicPileCount(int seat, string skillId) => Valid(seat) is null || piles is null ? null :
        piles.PublicPileSources(seat).Where(source => source.SkillId == skillId).Sum(source => piles.PublicPileCards(source).Count);
    public int? BindingStat(string stat, string name)
    {
        if (frame is null) return null;
        if (stat == "choiceNumber") return frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == name)?.Number;
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
        "owner" => Owner,
        "selectedTarget" => frame?.SelectedTargetSeats is [var seat] ? seat : null,
        "selectedFirst" => frame?.SelectedTargetSeats.Count >= 1 ? frame.SelectedTargetSeats[0] : null,
        "selectedSecond" => frame?.SelectedTargetSeats.Count >= 2 ? frame.SelectedTargetSeats[1] : null,
        "eventSource" => Context?.SourceSeat,
        "eventTarget" => Context?.TargetSeat,
        "turnOwner" => (WindowPoint ?? currentHistoryPoint?.Invoke())?.TurnOwnerSeat,
        _ => null
    });
    public IReadOnlyList<int>? ParticipantSet(string name) => name switch
    {
        "living" => matchParticipantsState.ReferenceToPlayers.Where(player => player.IsAlive).Select(player => player.Seat).ToArray(),
        "livingOthers" => matchParticipantsState.ReferenceToPlayers.Where(player => player.IsAlive && player.Seat != Owner).Select(player => player.Seat).ToArray(),
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
            "equipmentSuitCount" => equipmentSuitCount?.Invoke(seat),
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
            "ruleWounded" => wounded?.IsRuleWounded(seat),
            _ => null
        };
    }

    private readonly Dictionary<(CardUseHistoryPoint Point, CardUseHistoryView View), IReadOnlyList<CardUseHistoryFact>> _historyReads = [];
    private IReadOnlyList<CardUseHistoryFact> ReadHistory(CardUseHistoryPoint point, CardUseHistoryView view)
    {
        if (_historyReads.TryGetValue((point, view), out var cached)) return cached;
        var result = history!.ReadCardUseFacts(point, view);
        _historyReads.Add((point, view), result);
        return result;
    }
    public IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view)
    {
        var point = moment == "window" ? WindowPoint : currentHistoryPoint?.Invoke();
        return point is null || history is null ? null : ReadHistory(point,
            view == "observations" ? CardUseHistoryView.Observations :
            view == "confirmations" ? CardUseHistoryView.Confirmations :
            view == "accepted" ? CardUseHistoryView.Accepted :
            view == "finalizations" ? CardUseHistoryView.Finalizations : CardUseHistoryView.Latest);
    }
    private readonly Dictionary<CardUseHistoryPoint, CardUseHistoryFact?> _currentUses = [];
    public int? PhaseCount(string moment, int actor, ActualDiscardRecoveryPhaseKind? kind, ActualPhaseHistoryOutcome outcome)
    {
        var point = moment == "window" ? WindowPoint : currentHistoryPoint?.Invoke();
        return point is null ? null : phaseHistory?.CountPhases(point.ActualTurn, point.TurnOwnerSeat,
            actor, kind, outcome, point.Cursor);
    }
    public CardUseHistoryFact? CurrentUse()
    {
        if (WindowPoint is not { } point || history is null) return null;
        if (_currentUses.TryGetValue(point, out var cached)) return cached;
        var result = ReadHistory(point, CardUseHistoryView.Latest).SingleOrDefault(fact =>
            point.CurrentActionId is { } action ? fact.ActionId == action :
            point.CurrentNativeUseId is { } native && fact.NativeUseId == native);
        _currentUses.Add(point, result);
        return result;
    }

    private int? Valid(int? seat) => seat is { } value && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(value) ? value : null;
}
