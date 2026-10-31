using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class LongHandActionQueryChecks
{
    public static void SpearQueriesIgnoreUnrelatedHistoryAndRefreshMaterials()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var registry = ComposedSkillContentRegistry.CreateShowcase(includeHellActivities: true);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 161984113, PlayerCount = 8, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = "identity:classic-8", AdvanceAfterHumanCommands = false
        }, registry);
        var players = (IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine.GetField("_players", flags)!.GetValue(game)!;
        foreach (var player in players)
            foreach (var grant in player.SkillGrants.Grants.ToArray()) player.SkillGrants.RemoveGrant(grant.GrantId);
        Require(game.Submit(new StartGameCommand()).Accepted, "The long-hand fixture must start.");
        ProgramCompositionEntryChecks.ReachPlay(game);
        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", flags)!.GetValue(game)!;
        // Host-only finite-deck placement isolates the query cost, rather than
        // simulating several complete turns to accumulate a large Hand.
        foreach (var location in new[] { CardLocation.Hand(0), CardLocation.Equipment(0) })
            foreach (var card in zones.CardsAt(location).ToArray()) zones.Move(card.Id, location, CardLocation.DiscardPile);
        var spear = game.CreateCardZoneDiagnostics().First(card => card.CardKind == CardKind.ZhangbaSerpentSpear);
        zones.Move(spear.CardId, spear.Location, CardLocation.Equipment(0));
        foreach (var card in zones.CardsAt(CardLocation.DrawPile).Take(18).ToArray())
            zones.Move(card.Id, CardLocation.DrawPile, CardLocation.Hand(0));
        var build = global::FixtureRuntimeAccess.Engine.GetMethod("BuildLegalActions", flags)!;
        IReadOnlyList<LegalAction> Query() => (IReadOnlyList<LegalAction>)build.Invoke(game, [players[0], true, null])!;
        var initial = Query();
        var expected = JsonSerializer.Serialize(initial);
        var baselineAllocated = GC.GetAllocatedBytesForCurrentThread();
        var baselineStarted = Stopwatch.GetTimestamp();
        var baseline = Query();
        var baselineElapsed = Stopwatch.GetElapsedTime(baselineStarted).TotalMilliseconds;
        baselineAllocated = GC.GetAllocatedBytesForCurrentThread() - baselineAllocated;
        Require(expected == JsonSerializer.Serialize(baseline), "A warm query must preserve the original action inventory.");
        var groups = initial.Single(action => action.EquipmentKind == CardKind.ZhangbaSerpentSpear).SelectableCardGroupTargets!;
        Require(groups.Count > 100, "The fixture must query many complete Spear material pairs and targets.");

        var events = (List<EventEnvelope>)global::FixtureRuntimeAccess.Engine.GetField("_events", flags)!.GetValue(game)!;
        for (var index = 0; index < 6000; index++)
            events.Add(new(new EventId(events.Count + 1L), null, events.Count + 1L, game.Revision,
                "long-hand-query-fixture", new UnrelatedHistoryEvent(index)));
        var expandedAllocated = GC.GetAllocatedBytesForCurrentThread();
        var started = Stopwatch.GetTimestamp();
        var expanded = Query();
        var elapsed = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
        expandedAllocated = GC.GetAllocatedBytesForCurrentThread() - expandedAllocated;
        Require(expected == JsonSerializer.Serialize(expanded),
            "Unrelated committed history must preserve every exact material pair, target and legal action.");
        Console.WriteLine($"Long Hand: 18 cards, {groups.Count} Spear group targets, {events.Count} facts; query {elapsed:F1} ms.");
        var warmAllocated = GC.GetAllocatedBytesForCurrentThread();
        var warmStarted = Stopwatch.GetTimestamp();
        var warm = Query();
        var warmElapsed = Stopwatch.GetElapsedTime(warmStarted).TotalMilliseconds;
        warmAllocated = GC.GetAllocatedBytesForCurrentThread() - warmAllocated;
        Require(expected == JsonSerializer.Serialize(warm), "Repeated queries over expanded history must retain exact actions.");
        Console.WriteLine($"Long Hand profile: baseline {baselineElapsed:F1} ms / {baselineAllocated} bytes; " +
            $"expanded {elapsed:F1} ms / {expandedAllocated} bytes; warm expanded {warmElapsed:F1} ms / {warmAllocated} bytes.");

        var removed = zones.CardsAt(CardLocation.Hand(0))[0].Id;
        zones.Move(removed, CardLocation.Hand(0), CardLocation.DiscardPile);
        var fresh = Query().Single(action => action.EquipmentKind == CardKind.ZhangbaSerpentSpear);
        Require(fresh.SelectableCardGroupTargets!.All(group => !group.CardIds.Contains(removed)),
            "A later read must discard query-local answers and validate the current material inventory.");

        zones.Move(spear.CardId, CardLocation.Equipment(0), CardLocation.DiscardPile);
        var crossbow = game.CreateCardZoneDiagnostics().First(card => card.CardKind == CardKind.Crossbow);
        zones.Move(crossbow.CardId, crossbow.Location, CardLocation.Equipment(0));
        var begin = global::FixtureRuntimeAccess.Engine.GetMethod("BeginActionQuery", flags)!;
        var limit = global::FixtureRuntimeAccess.Engine.GetMethod("GetSlashUseLimit", flags)!;
        int Limit(int? excluded = null, EquipmentSlot? slot = null) => (int)limit.Invoke(game, [players[0], excluded, slot])!;
        using ((IDisposable)begin.Invoke(game, [])!)
            Require(Limit() == int.MaxValue && Limit(crossbow.CardId) == 1 && Limit(slot: EquipmentSlot.Weapon) == 1 &&
                Limit() == int.MaxValue,
                "One query must distinguish the current weapon from a weapon or slot consumed as material.");
        zones.Move(crossbow.CardId, CardLocation.Equipment(0), CardLocation.DiscardPile);
        using ((IDisposable)begin.Invoke(game, [])!)
            Require(Limit() == 1, "The next query must recompute its Slash limit after equipment changes.");
    }

    private sealed record UnrelatedHistoryEvent(int Index) : IGameEvent;
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
