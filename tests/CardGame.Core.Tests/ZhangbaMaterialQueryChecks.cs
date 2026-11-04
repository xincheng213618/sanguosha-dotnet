using System.Collections;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class ZhangbaMaterialQueryChecks
{
    public static void OrderedPairsFreezeMaterialsAtQueryTimeAndRefreshNextRead()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 161984113, PlayerCount = 8, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = "identity:classic-8", AdvanceAfterHumanCommands = false
        }, ComposedSkillContentRegistry.CreateShowcase(includeHellActivities: true));
        var players = (IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine.GetField("_players", flags)!.GetValue(game)!;
        foreach (var player in players)
            foreach (var grant in player.SkillGrants.Grants.ToArray()) player.SkillGrants.RemoveGrant(grant.GrantId);
        Require(game.Submit(new StartGameCommand()).Accepted, "The finite-deck Spear fixture starts through the real setup.");
        ProgramCompositionEntryChecks.ReachPlay(game);
        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", flags)!.GetValue(game)!;
        // Reuse the long-hand query fixture's host-only placement: each item is
        // a distinct real entity, without paying or resolving a synthetic use.
        foreach (var location in new[] { CardLocation.Hand(0), CardLocation.Equipment(0) })
            foreach (var card in zones.CardsAt(location).ToArray()) zones.Move(card.Id, location, CardLocation.DiscardPile);
        var spear = game.CreateCardZoneDiagnostics().First(card => card.CardKind == CardKind.ZhangbaSerpentSpear);
        zones.Move(spear.CardId, spear.Location, CardLocation.Equipment(0));
        var materials = zones.CardsAt(CardLocation.DrawPile).Take(4).OrderByDescending(card => card.Id).ToArray();
        foreach (var card in materials) zones.Move(card.Id, CardLocation.DrawPile, CardLocation.Hand(0));
        var ids = materials.Select(card => card.Id).Order().ToArray();
        var expected = SixPairs(ids);
        var owner = players[0];
        var queries = game.GetRuntimeModule<CoreNativeResponseMaterialQueriesRuntime>();
        var all = queries.GetZhangbaSlashPairs(owner);
        Require(SamePairs(all, expected) && SamePairs(queries.EnumerateZhangbaSlashPairs(owner), expected),
            "Full and lazy queries preserve all six canonical pairs in card-ID order, independently of physical Hand insertion order.");
        var first = queries.FirstZhangbaSlashPair(owner);
        Require(first is not null && first.Select(card => card.Id).SequenceEqual(expected[0]) &&
            all.All(pair => pair is IList { IsReadOnly: true }) && first is IList { IsReadOnly: true },
            "First returns the same first pair and every published pair retains the existing read-only material contract.");
        Require(queries.FindZhangbaSlashPair(owner, [ids[0], ids[2]])?.Select(card => card.Id).SequenceEqual([ids[0], ids[2]]) == true &&
            queries.FindZhangbaSlashPair(owner, [ids[2], ids[0]])?.Select(card => card.Id).SequenceEqual([ids[0], ids[2]]) == true &&
            queries.FindZhangbaSlashPair(owner, [ids[0], ids[0]]) is null &&
            queries.FindZhangbaSlashPair(owner, [ids[0], spear.CardId]) is null &&
            queries.FindZhangbaSlashPair(owner, []) is null &&
            queries.FindZhangbaSlashPair(owner, [ids[0]]) is null &&
            queries.FindZhangbaSlashPair(owner, [ids[0], ids[1], ids[2]]) is null,
            "Find accepts exactly two distinct available entities in either request order and returns their canonical pair; incomplete, duplicate and missing material requests fail.");

        PairFactsRespectTheirReadOnlyQuery(game, players, zones, spear.CardId, all[0][0], all[0][1]);

        // No GetEnumerator/MoveNext occurs before changing the Hand: the call,
        // rather than first enumeration, owns the legal material snapshot.
        var frozen = queries.EnumerateZhangbaSlashPairs(owner);
        zones.Move(ids[0], CardLocation.Hand(0), CardLocation.DiscardPile);
        var incoming = zones.CardsAt(CardLocation.DrawPile)[0];
        zones.Move(incoming.Id, CardLocation.DrawPile, CardLocation.Hand(0));
        Require(SamePairs(frozen, expected) && SamePairs(frozen, expected) &&
            frozen.All(pair => pair is IList { IsReadOnly: true }),
            "An unstarted enumerable retains its original four eligible entities across removal, replacement and repeated enumeration.");
        var currentIds = zones.CardsAt(CardLocation.Hand(0)).Select(card => card.Id).Order().ToArray();
        var refreshed = SixPairs(currentIds);
        Require(SamePairs(queries.EnumerateZhangbaSlashPairs(owner), refreshed) &&
            SamePairs(queries.GetZhangbaSlashPairs(owner), refreshed) &&
            queries.FirstZhangbaSlashPair(owner)!.Select(card => card.Id).SequenceEqual(refreshed[0]) &&
            queries.FindZhangbaSlashPair(owner, [ids[0], ids[1]]) is null &&
            queries.FindZhangbaSlashPair(owner, [incoming.Id, ids[1]]) is not null,
            "A new query samples the current inventory instead of borrowing the prior enumerable's departed material.");
        foreach (var card in zones.CardsAt(CardLocation.Hand(0)).Take(3).ToArray())
            zones.Move(card.Id, CardLocation.Hand(0), CardLocation.DiscardPile);
        Require(queries.GetZhangbaSlashPairs(owner).Count == 0 && !queries.EnumerateZhangbaSlashPairs(owner).Any() &&
            queries.FirstZhangbaSlashPair(owner) is null && queries.FindZhangbaSlashPair(owner, refreshed[0]) is null,
            "An equipped Spear with only one current material has no full, lazy, first or requested pair.");
    }

    private static void PairFactsRespectTheirReadOnlyQuery(GameEngine game, IReadOnlyList<CharacterState> players,
        CardZoneStore zones, int spearId, Card first, Card second)
    {
        var query = game.GetRuntimeModule<CoreGameEngineActionQueriesRuntime>();
        var facts = game.GetRuntimeModule<CoreGameEngineZhangbaSelectionsRuntime>();
        var actor = players[0];
        ZhangbaSlashPairFacts original;
        using (query.BeginActionQuery())
        {
            original = facts.GetZhangbaSlashPairFacts(actor, first, second);
            Require(ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first with { }, second with { })) &&
                    original.LegalTargets.Contains(1) && original.LegalTargets.Contains(2),
                "One owning read-only query reuses the complete original pair facts by Card value and retains the real Spear's adjacent and distant targets.");
            using (query.BeginActionQuery())
                Require(ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first, second)),
                    "A nested ordinary read shares its existing owning query rather than issuing another pair fact.");
            Require(ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first, second)) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first with { Kind = first.Kind == CardKind.Dodge ? CardKind.Slash : CardKind.Dodge }, second)) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first with { Suit = first.Suit == Suit.Heart ? Suit.Club : Suit.Heart }, second)) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first with { Rank = first.Rank == 13 ? 12 : 13 }, second)) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, first, second with { Rank = second.Rank == 13 ? 12 : 13 })) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(actor, second, first)) &&
                    !ReferenceEquals(original, facts.GetZhangbaSlashPairFacts(players[1], first, second)),
                "Same-Id kind, suit or rank appearances, either material, pair order and actor are independent fact inputs; a disposed nested read must not erase the outer result.");
            var red = facts.GetZhangbaSlashPairFacts(actor, first with { Suit = Suit.Heart }, second with { Suit = Suit.Heart });
            Require(red.Suit == Suit.Heart && red.Color == true && original.LegalTargets is IList<int> { IsReadOnly: true },
                "A same-Id effective face has its own suit/color facts and every legal-target collection is frozen.");
            var rejected = false;
            try { ((IList<int>)original.LegalTargets)[0] = actor.Seat; }
            catch (NotSupportedException) { rejected = true; }
            Require(rejected, "The caller cannot mutate a retained pair's legal targets.");
        }
        var outside = facts.GetZhangbaSlashPairFacts(actor, first, second);
        Require(!ReferenceEquals(original, outside) && !ReferenceEquals(outside, facts.GetZhangbaSlashPairFacts(actor, first, second)) &&
                outside.LegalTargets.SequenceEqual(original.LegalTargets),
            "Outside an owning query, each read is fresh even when its complete facts are equal.");

        // Controlled native inputs change only after the held read has ended.
        players[1].IsAlive = false;
        try
        {
            Require(!facts.GetZhangbaSlashPairFacts(actor, first, second).LegalTargets.Contains(1) && original.LegalTargets.Contains(1),
                "An outside read sees current target life without rewriting the earlier immutable result.");
            using (query.BeginActionQuery())
            {
                var changed = facts.GetZhangbaSlashPairFacts(actor, first, second);
                Require(!ReferenceEquals(original, changed) && !changed.LegalTargets.Contains(1) &&
                        ReferenceEquals(changed, facts.GetZhangbaSlashPairFacts(actor, first, second)),
                    "The next query samples current target state once instead of borrowing the previous query's cached target.");
            }
        }
        finally { players[1].IsAlive = true; }
        zones.Move(spearId, CardLocation.Equipment(0), CardLocation.DiscardPile);
        try
        {
            using (query.BeginActionQuery())
            {
                var unarmed = facts.GetZhangbaSlashPairFacts(actor, first, second);
                Require(!unarmed.LegalTargets.Contains(2) && facts.GetZhangbaSlashGroupTargets(actor).Count == 0 && original.LegalTargets.Contains(2),
                    "A later query reads the changed real equipment range, while group admission independently rejects an absent Spear.");
            }
        }
        finally { zones.Move(spearId, CardLocation.DiscardPile, CardLocation.Equipment(0)); }
        using (query.BeginActionQuery())
            Require(facts.GetZhangbaSlashPairFacts(actor, first, second).LegalTargets.SequenceEqual(original.LegalTargets),
                "Restoring the actual equipment between queries restores its legal targets through a new fact read.");
    }

    private static int[][] SixPairs(int[] ids) =>
        [[ids[0], ids[1]], [ids[0], ids[2]], [ids[0], ids[3]], [ids[1], ids[2]], [ids[1], ids[3]], [ids[2], ids[3]]];

    private static bool SamePairs(IEnumerable<IReadOnlyList<Card>> actual, IReadOnlyList<int[]> expected) =>
        actual.Select(pair => string.Join(",", pair.Select(card => card.Id))).SequenceEqual(expected.Select(pair => string.Join(",", pair)));

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
