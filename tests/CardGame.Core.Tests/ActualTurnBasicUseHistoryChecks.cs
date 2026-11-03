using System.Text.Json;
using CardGame.Core;

internal static class ActualTurnBasicUseHistoryChecks
{
    public static void BasicPrefixRetainsNativeOrderNamesAndTurnEndBoundary()
    {
        var committed = new List<EventEnvelope>();
        var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex(committed, pending);
        static CardActionContext Action(long id, int actor, CardKind kind, CardActionType type = CardActionType.Use, int? provider = null) =>
            new(id, null, type, actor, provider ?? actor, null, type == CardActionType.Response ? actor : null,
                null, kind, [], [], []);
        static NativeCardUseHistorySnapshot Native(long id, int actor, CardKind kind, long? action = null, int turn = 2, int owner = 1) =>
            new(id, turn, owner, actor, kind, action, []);
        static CardUseHistoryFact[] Basics(ProgramEventHistoryIndex index, CardUseHistoryPoint point) =>
            index.ReadCardUseFacts(point, CardUseHistoryView.ActualUses).Where(fact =>
                fact.NativeUseId is not null && fact.DeclaredKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash or CardKind.Dodge or CardKind.Peach or CardKind.Alcohol ||
                fact.Type == CardActionType.Response && fact.IsSelfNativeResponseUse && fact.EffectiveKind == CardKind.Dodge).ToArray();
        history.Append(new TurnStartedEvent(1, 0));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(10, 0, CardKind.Peach, 100, 1, 0)));
        history.Append(new TurnEndedEvent(1, 0));
        history.Append(new TurnStartedEvent(2, 1));
        history.Append(new ActualCardActionRecordedEvent(Action(199, 3, CardKind.Peach), 2, 1, TurnPhase.Play, 2, false));
        history.Append(new ActualCardActionRecordedEvent(Action(200, 1, CardKind.Slash), 2, 1, TurnPhase.Play, 2, false));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(19, 1, CardKind.Duel)));
        var slash = Native(20, 1, CardKind.Slash, 200);
        history.Append(new NativeCardUseHistoryRecordedEvent(slash));
        history.Append(new NativeCardUseHistoryRecordedEvent(slash with { ActorSeat = 3, EffectiveKind = CardKind.FireSlash }));
        history.Append(new ActualCardActionRecordedEvent(Action(201, 2, CardKind.Dodge, CardActionType.Response), 2, 1, TurnPhase.Play, 2, true));
        history.Append(new ActualCardActionRecordedEvent(Action(202, 3, CardKind.Dodge, CardActionType.Response, 0), 2, 1, TurnPhase.Play, 2, false));
        history.Append(new ActualCardActionRecordedEvent(Action(203, 0, CardKind.Dodge, CardActionType.Response), 2, 1, TurnPhase.Play, 2, false));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(23, 0, CardKind.Alcohol)));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(24, 3, CardKind.Peach)));
        Require(history.FindTurnEndPoint(2, 1) is null, "A current turn has no invented completed-turn boundary.");
        var livePoint = new CardUseHistoryPoint(2, 1, history.CardUseHistoryCursor);
        var beforeEnd = Basics(history, livePoint);
        Require(beforeEnd.Select(fact => fact.EffectiveKind).SequenceEqual(new CardKind?[]
                { CardKind.FireSlash, CardKind.Dodge, CardKind.Alcohol, CardKind.Peach }) &&
            beforeEnd.Select(fact => fact.ActorSeat).SequenceEqual([1, 2, 0, 3]) &&
            beforeEnd[0] is { NativeUseId: 20, ActionId: 200, DeclaredKind: CardKind.Slash } &&
            beforeEnd[1] is { NativeUseId: null, ActionId: 201, IsSelfNativeResponseUse: true } &&
            beforeEnd[2] is { NativeUseId: 23, ActionId: null } && slash.EffectiveKind == CardKind.Slash,
            "The common view retains native order, effective fire Slash, self-supplied Dodge and Action-null Wine, excluding supplied and group-card responses without rewriting source facts.");
        var actual = history.ReadCardUseFacts(livePoint, CardUseHistoryView.ActualUses);
        Require(actual.Where(fact => fact.NativeUseId is not null).Take(2).Select(fact => fact.NativeUseId).SequenceEqual(new long?[] { 19, 20 }) &&
            !actual.Any(fact => fact.ActionId is 199 or 202 or 203) &&
            history.ReadCardUseFacts(livePoint, CardUseHistoryView.Latest).Single(fact => fact.NativeUseId == 20).ActorSeat == 3 &&
            actual.Single(fact => fact.NativeUseId == 20).ActorSeat == 1,
            "Accepted-only cards do not occupy the native declaration sequence, and later actor replacement changes latest history without moving the original actual-use actor.");
        history.Append(new TurnEndedEvent(2, 1));
        var ended = history.FindTurnEndPoint(2, 1)!;
        history.Append(new NativeCardUseHistoryRecordedEvent(slash with { ActorSeat = 0, EffectiveKind = CardKind.ThunderSlash }));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(25, 0, CardKind.Peach)));
        history.Append(new TurnStartedEvent(3, 2));
        history.Append(new NativeCardUseHistoryRecordedEvent(Native(30, 2, CardKind.Peach, turn: 3, owner: 2)));
        var afterEnd = Basics(history, history.FindTurnEndPoint(2, 1)!);
        Require(history.FindTurnEndPoint(2, 1) == ended && SameUses(afterEnd, beforeEnd) &&
            Basics(history, new(2, 1, history.CardUseHistoryCursor)).Length == beforeEnd.Length + 1,
            "The original real TurnEnded cutoff excludes later AfterEnd uses and kind changes while an explicit later scope read still sees them.");
        var firstThree = afterEnd.Take(3).ToArray();
        Require(firstThree.Length == 3 && firstThree.Take(2).Count(fact => fact.ActorSeat == 1) == 1 &&
            firstThree.Take(2).Count(fact => fact.ActorSeat == 3) == 0 && firstThree[2].EffectiveKind == CardKind.Alcohol,
            "The same frozen first-two actors and third-basic name remain available without a character snapshot or private prefix ledger.");
        var world = new CardUseHistoryTestWorld(history, ended with { CurrentNativeUseId = 20 });
        Require(world.Bool("""["useKindIs","currentUse",["fireSlash"],"effective"]""") == true &&
            world.Bool("""["useKindIs","currentUse",["slash"]]""") == true &&
            world.Bool("""["useKindIs","currentUse",["fireSlash"]]""") == false &&
            world.Int("""["count",["takeFirst",["cardUses","window","actualUses"],2],true]""") == 2,
            "Effective kind is an explicit read; default declared-kind matching and takeFirst retain their independent meanings.");
        var replayed = new ProgramEventHistoryIndex(committed.ToArray(), pending.ToList());
        Require(replayed.FindTurnEndPoint(2, 1) == ended && SameUses(Basics(replayed, ended), afterEnd),
            "Rebuilding the common index from the same journal retains the exact frozen completed-turn prefix.");
        PendingTurnEndReplacementAndRollbackRebuildTheIndex();
    }

    private static void PendingTurnEndReplacementAndRollbackRebuildTheIndex()
    {
        var pending = new List<IGameEvent>
        {
            new TurnStartedEvent(1, 0),
            new NativeCardUseHistoryRecordedEvent(new(10, 1, 0, 0, CardKind.Alcohol, null, [])),
            new TurnEndedEvent(1, 0)
        };
        var history = new ProgramEventHistoryIndex([], pending);
        var first = history.FindTurnEndPoint(1, 0)!;
        Require(first.Cursor == 3 && history.ReadCardUseFacts(first, CardUseHistoryView.ActualUses).Count == 1,
            "A pending TurnEnded resolves to its full journal position.");
        pending[2] = new RoundStartedEvent(1, 0);
        Require(history.FindTurnEndPoint(1, 0) is null, "A same-count pending replacement removes the old completed-turn cutoff.");
        pending[2] = new TurnEndedEvent(1, 0);
        Require(history.FindTurnEndPoint(1, 0) == first, "Restoring the original pending end rebuilds its one exact cutoff.");
        history.ClearPending();
        Require(history.FindTurnEndPoint(1, 0) is null && history.ReadCardUseFacts(first, CardUseHistoryView.ActualUses).Count == 0,
            "Rolling back pending history clears both actual-use membership and the old boundary cursor.");
        history.Append(new TurnStartedEvent(2, 1));
        history.Append(new TurnEndedEvent(2, 1));
        Require(history.FindTurnEndPoint(1, 0) is null && history.FindTurnEndPoint(2, 1) is { Cursor: 2 } next &&
            history.ReadCardUseFacts(next, CardUseHistoryView.ActualUses).Count == 0,
            "A later equal-size journal cannot inherit an earlier turn's cursor or native uses.");
    }

    private static void Require(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private static bool SameUses(IEnumerable<CardUseHistoryFact> left, IEnumerable<CardUseHistoryFact> right) =>
        left.Select(fact => JsonSerializer.Serialize(fact)).SequenceEqual(right.Select(fact => JsonSerializer.Serialize(fact)));
}
