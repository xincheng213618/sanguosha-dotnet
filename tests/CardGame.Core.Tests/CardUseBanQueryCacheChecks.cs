using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;
using CardGame.Core.NativeOwners;

internal static class CardUseBanQueryCacheChecks
{
    public static void CompleteKeysStayWithinTheirOwningReadOnlyQuery()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 161984113, PlayerCount = 8, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = "identity:classic-8", AdvanceAfterHumanCommands = false
        }, ComposedSkillContentRegistry.CreateShowcase(includeHellActivities: true));
        var players = Read<IReadOnlyList<CharacterState>>("_players");
        foreach (var player in players)
            foreach (var grant in player.SkillGrants.Grants.ToArray()) player.SkillGrants.RemoveGrant(grant.GrantId);
        Need(game.Submit(new StartGameCommand()).Accepted, "The shared query fixture starts through the real setup.");
        ProgramCompositionEntryChecks.ReachPlay(game);
        var queries = game.GetRuntimeModule<CoreGameEngineActionQueriesRuntime>();
        var useRules = game.GetRuntimeModule<CoreGameEngineCardUseModulesRuntime>();
        var issuedRules = game.GetRuntimeModule<CoreGameEngineIssuedPlayUsePoliciesRuntime>();
        var store = (TurnCardUseEffectStore)typeof(GameEngine).GetField("_turnCardUseEffects", flags)!.GetValue(game)!;
        var issued = Read<List<IssuedPlayPhaseUseProhibition>>("_issuedPlayPhaseUseProhibitions");
        var turn = Read<int>("_turnNumber");
        var turnOwner = Read<int>("_currentSeat");
        var phaseId = Read<int>("_cardUseDebitPhaseInstanceId");
        var source = new CardUseEffectSource("fixture:query-ban", "issued", 0, "fixture:query-ban-instance");
        issued.Add(new(new(source.SkillId, source.BindingId, source.OwnerSeat, source.SkillInstanceId), 0, turn, phaseId, 1));
        store.GrantActionProhibition(turn, turnOwner, 100, 0, source, [CardKind.Duel], [CardActionType.Use], affectedSeat: 1);
        Need(issuedRules.HasIssuedPlayPhaseUseBan(0) && Current() is null,
            "The fixture uses the native issued Play ban and turn action-prohibition facts, without creating a query cache.");
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var useId = frames.NextId();
        frames.Push(new CardUseFrame(useId, 0, 0, CardKind.Slash, [1]));
        ActionQuery first;
        using (queries.BeginActionQuery())
        {
            first = Current()!;
            Need(first.CardUseBans.Count == 0 &&
                Ban(0, CardKind.Slash, CardActionType.Use) &&
                !Ban(0, CardKind.Slash, CardActionType.Use, ignore: true) &&
                !Ban(1, CardKind.Slash, CardActionType.Use) &&
                Ban(1, CardKind.Duel, CardActionType.Use) &&
                !Ban(1, CardKind.Duel, CardActionType.Response) &&
                Ban(0, CardKind.Slash, CardActionType.Use, existing: useId) &&
                first.CardUseBans.Count == 6 &&
                first.CardUseBans.ContainsKey((0, CardKind.Slash, CardActionType.Use, false, null)) &&
                first.CardUseBans.ContainsKey((0, CardKind.Slash, CardActionType.Use, false, useId)),
                "Actor, effective kind, action type, issued-ban bypass and nullable existing-use identity each retain their own native answer and cache entry.");
            var entries = first.CardUseBans.Count;
            Need(Ban(0, CardKind.Slash, CardActionType.Use) && first.CardUseBans.Count == entries,
                "Repeating an identical five-part query reuses its existing answer.");
            issued.Clear();
            using (queries.BeginActionQuery())
                Need(ReferenceEquals(Current(), first) && Ban(0, CardKind.Slash, CardActionType.Use) &&
                    first.CardUseBans.Count == entries,
                    "An ordinary nested query shares its outer read-only snapshot and does not create another cache owner.");
            Need(ReferenceEquals(Current(), first), "Disposing an ordinary nested scope preserves the outer query.");
        }
        frames.Pop(useId, ResolutionFrameKind.CardUse);
        Need(Current() is null && !Ban(0, CardKind.Slash, CardActionType.Use) && Current() is null,
            "Outside a menu scope the native query immediately reads removal of the issued ban and allocates no cache owner.");
        store.GrantActionProhibition(turn, turnOwner, 101, 0, source, [CardKind.Slash], [CardActionType.Use], affectedSeat: 0);
        Need(Ban(0, CardKind.Slash, CardActionType.Use) && Current() is null,
            "A later action prohibition is immediately effective outside action-query scope.");
        using (queries.BeginActionQuery())
        {
            Need(!ReferenceEquals(Current(), first) && Current()!.CardUseBans.Count == 0 &&
                Ban(0, CardKind.Slash, CardActionType.Use) && Current()!.CardUseBans.Count == 1,
                "A new top-level query starts empty and derives the current turn prohibition instead of borrowing the previous scope.");
        }

        var zones = (CardZoneStore)typeof(GameEngine).GetField("_cardZones", flags)!.GetValue(game)!;
        var printed = game.CreateCardZoneDiagnostics().First(card => card.CardKind == CardKind.Duel);
        if (printed.Location != CardLocation.Hand(0)) zones.Move(printed.CardId, printed.Location, CardLocation.Hand(0));
        var card = zones.CardsAt(CardLocation.Hand(0)).Single(item => item.Id == printed.CardId);
        using (queries.BeginActionQuery())
        {
            var outer = Current()!;
            Need(!Ban(0, CardKind.Duel, CardActionType.Use), "The outer query starts with an allowed printed Duel.");
            var entries = outer.CardUseBans.Count;
            // Like the existing query-cache checks, mutate the host fact only
            // to distinguish isolated derivation from the outer frozen value.
            store.GrantActionProhibition(turn, turnOwner, 102, 0, source, [CardKind.Duel], [CardActionType.Use], affectedSeat: 0);
            var templates = queries.BuildMobileWumouPrintedTargetActions(players[0], card);
            Need(templates.Count == 0 && ReferenceEquals(Current(), outer) &&
                outer.CardUseBans.Count == entries && !Ban(0, CardKind.Duel, CardActionType.Use),
                "The real printed-template path uses its isolated query, sees the new ban and restores the outer query with its earlier answer intact.");
        }
        Need(Current() is null && Ban(0, CardKind.Duel, CardActionType.Use),
            "After both scopes end, the current Duel prohibition is read directly again.");

        T Read<T>(string name) => (T)global::FixtureRuntimeAccess.Engine.GetField(name, flags)!.GetValue(game)!;
        ActionQuery? Current() => Read<ActionQuery?>("_actionQuery");
        bool Ban(int actor, CardKind kind, CardActionType type, bool ignore = false, long? existing = null) =>
            useRules.IsCardUseForbidden(actor, kind, type, ignore, existing);
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
