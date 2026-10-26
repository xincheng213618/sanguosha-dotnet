using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTeamGrowthChecks
{
    private const string Hero = "fixture:team-growth-hero", AllyA = "fixture:team-growth-a", AllyB = "fixture:team-growth-b",
        Enemy = "fixture:team-growth-enemy", Driver = "fixture:team-growth-driver";
    private static readonly string[] Loadout = ["common-draw", "common-slash", "rare-opening", "rare-armor", "epic-opening", ShanheTeamTactics.HealthRewardId];

    public static void AutomaticSetPersistentMarketNativeStartAndForgedMarkerCold()
    {
        var basis = Definition();
        var definition = basis with
        {
            Nodes = basis.Nodes.Select(node => node.Id == "first" ? node with { NextNodeIds = ["shop"] } : node)
                .Append(new ShanheNodeDefinition("shop", "常驻集市", ShanheNodeKind.Shop, ["second"],
                Choices: [new("buy-supply", "补给", RewardId: "market-supply", LeavesNode: false)]) { ChapterNumber = 1 }).ToArray(),
            Rewards = basis.Rewards.Append(new ShanheRewardDefinition("market-supply", "补给", Gold: 1)).ToArray()
        };
        var run = ShanheCampaignSession.Create(definition,
            new(71, Hero) { PlayerRank = 1, EquippedTacticIds = Loadout, PersistentMarket = true, UseCodexRewardPool = true }, MarketRegistry);
        Need(run.Snapshot.TacticIds.Contains(ShanheTeamTactics.SetRewardId), "The real persistent expedition activates its qualified automatic set.");
        Accept(run, new MoveShanheNodeCommand("first", run.Revision));
        var setup = run.PendingBattleSetup!;
        Need(setup.MarketSetup is not null && setup.SharedAllyRank == 3,
            "The initial market and the next native battle carry the same acquired set and derived minimum rank.");
        var market = ShanheCampaignSession.CreateBattleMarketSession(setup.MarketSetup!, MarketRegistry);
        Need(market.Snapshot.TacticIds.SequenceEqual(run.Snapshot.TacticIds),
            "A legitimate automatic set survives market import without changing its tactic acquisition order.");
        var missingMarker = setup.MarketSetup! with { State = setup.MarketSetup!.State with
            { TacticIds = setup.MarketSetup.State.TacticIds.Where(id => id != ShanheTeamTactics.SetRewardId).ToArray() } };
        Need(ShanheCampaignSession.CreateBattleMarketSession(missingMarker, MarketRegistry).Snapshot.TacticIds.Contains(ShanheTeamTactics.SetRewardId),
            "The transfer recomputes a genuinely qualified automatic marker rather than trusting its serialized presence.");
        var forged = setup.MarketSetup! with { State = setup.MarketSetup.State with
            { TacticIds = setup.MarketSetup.State.TacticIds.Where(id => id != "common-slash").ToArray() } };
        var rejected = false;
        try { _ = ShanheCampaignSession.CreateBattleMarketSession(forged, MarketRegistry); }
        catch (ArgumentException) { rejected = true; }
        Need(rejected, "An automatic set marker without its actual qualifying source tactics is rejected.");
        var registry = MarketRegistry(setup);
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
            restored.CreateShanheMarketSnapshot(0)!.TacticIds.Contains(ShanheTeamTactics.SetRewardId),
            "The real market-backed battle starts and cold-restores with the automatic set available to its market.");
        var coldRun = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(run.CreateCheckpoint())),
            definition, MarketRegistry);
        Need(ShanheBattleSetup.ValueEquals(coldRun.PendingBattleSetup, setup), "Cold campaign replay keeps the same set-qualified native market recipe.");
        return;

        static ContentRegistry MarketRegistry(ShanheBattleSetup battle) => ContentRegistry.Build(new StandardContentPackage(),
            new StandardShanheTeamGrowthPackage(), new StandardShanheRankPrivilegePackage(), new StandardShanheMarketPackage(), new Fixture(battle));
    }

    public static void QualityThresholdMinimumRankAndNativeHealthPrecedeXianfu()
    {
        var definition = Definition();
        foreach (var complete in new[] { false, true })
        {
            var run = ShanheCampaignSession.Create(definition, new(71, Hero)
            { PlayerRank = 1, EquippedTacticIds = complete ? Loadout : Loadout.Where(id => id != "common-slash").ToArray() }, Registry);
            Need(run.Snapshot.TacticIds.Contains(ShanheTeamTactics.SetRewardId) == complete,
                "The team set activates only after every printed-quality threshold, including two common members, is satisfied.");
            var preview = run.PreviewEncounter("first");
            Need(preview.Allies is { Count: 2 } && preview.Allies.All(member => member.MaxHpBonus == (complete ? 1 : 0) &&
                member.InitialHandBonus == (complete ? 1 : 0) && member.DrawBonus == 0 && member.SlashBonus == (complete ? 1 : 0)),
                "A first-rank player shares precisely the third-rank numerical recipe after the set activates; a missing common member grants none.");
            Accept(run, new MoveShanheNodeCommand("first", run.Revision)); var setup = run.PendingBattleSetup!;
            Need(setup.SharedAllyRank == (complete ? 3 : 0) && JsonSerializer.Serialize(setup.AllyRoster) == JsonSerializer.Serialize(preview.Allies!),
                "The native battle recipe and its preview share the same derived rank and complete allied roster.");
            var game = Battle(setup); Accept(game, new StartGameCommand()); Play(game);
            var players = game.CreateSnapshot(0).Players;
            Need(players[0].MaxHp == 10 && players[1].MaxHp == (complete ? 7 : 6) && players[2].MaxHp == players[1].MaxHp &&
                players[1].Hp == players[1].MaxHp && players[2].Hp == players[2].MaxHp && players[3].MaxHp == 10,
                "Team health uses real allied maximum-health and recovery operations; neither its owner nor enemies receive those three points.");
            var events = game.Events.Select(envelope => envelope.Payload).ToArray();
            var relation = Array.FindIndex(events, value => value is XianfuRelationArmedEvent);
            var maximum = Array.FindLastIndex(events, value => value is MaximumHpChangedEvent { SkillId: ShanheTeamTactics.HealthSkillId });
            var recoveries = events.Select((value, index) => (value, index)).Where(item => item.value is RecoveryAppliedEvent { TargetSeat: 1 or 2 }).ToArray();
            Need(relation > maximum && maximum >= 0 && recoveries.Length >= 2 && recoveries.All(item => item.index < relation),
                "Every actual opening team-health recovery finishes before Xianfu establishes its game-start relationship.");
        }
    }

    public static void FifthRankAllyRebirthIsIndependentAndConsumedAcrossBattlesCold()
    {
        var definition = Definition(); var run = ShanheCampaignSession.Create(definition,
            new(71, Hero) { PlayerRank = 5, EquippedTacticIds = Loadout }, Registry);
        Accept(run, new MoveShanheNodeCommand("first", run.Revision)); var setup = run.PendingBattleSetup!;
        var bonus = ShanheGeneralRanks.GetBonuses(5);
        Need(setup.SharedAllyRank == 5 && setup.AllyRoster.All(member => member.MaxHpBonus == bonus.MaxHp &&
            member.InitialHandBonus == bonus.InitialHand && member.DrawBonus == bonus.Draw && member.SlashBonus == bonus.Slash &&
            member.ExtraSkillIds!.Contains(ShanheTeamTactics.AllyRebirthSkillId)) && setup.RebirthAvailable,
            "Fifth-rank sharing carries all four existing numerical bonuses plus separate allied rebirth sources while preserving the player's entitlement.");
        var game = Battle(setup); Accept(game, new StartGameCommand()); Play(game);
        Strike(game, "eleven", 1); Strike(game, "eleven", 2);
        var uses = game.Events.Select(envelope => envelope.Payload).OfType<SkillUsageConsumedEvent>()
            .Where(usage => usage.SkillId == ShanheTeamTactics.AllyRebirthSkillId && usage.Scope == SkillUsageScope.Game).ToArray();
        Need(uses.Select(usage => usage.SkillOwnerSeat).ToHashSet().SetEquals([1, 2]) && uses.Length == 2 &&
            !game.Events.Any(envelope => envelope.Payload is SkillUsageConsumedEvent { SkillOwnerSeat: 0, SkillId: ShanheGeneralRanks.RebirthSkillId }),
            "Two distinct allies each pay their own one-use native rebirth and never spend the player's campaign rebirth.");
        Strike(game, "five", 1);
        Need(!game.CreateSnapshot(0).Players[1].IsAlive && game.Events.Select(envelope => envelope.Payload).OfType<SkillUsageConsumedEvent>()
            .Count(usage => usage.SkillId == ShanheTeamTactics.AllyRebirthSkillId && usage.SkillOwnerSeat == 1) == 1,
            "A second lethal hit to one ally cannot replay its already consumed same-battle revival.");
        var prompt = game.PendingDecision!;
        Accept(game, new UseProgramSkillCommand(0, Driver, "ten", [], [3], game.Revision, prompt.PromptId));
        Reach(game, () => game.State.Status == EngineStatus.Completed);
        Accept(run, new CompleteShanheBattleCommand(GameCheckpointJson.Serialize(game.CreateCheckpoint()), run.Revision));
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(run.CreateCheckpoint())), definition, Registry);
        Accept(restored, new MoveShanheNodeCommand("second", restored.Revision)); var later = restored.PendingBattleSetup!;
        Need(later.RebirthAvailable && later.SharedAllyRank == 5 && later.AllyRoster.All(member =>
            !(member.ExtraSkillIds ?? []).Contains(ShanheTeamTactics.AllyRebirthSkillId) && member.MaxHpBonus == bonus.MaxHp),
            "Cold campaign replay preserves each persistent companion's spent expedition revival without removing rank statistics or the player's untouched privilege.");
    }

    private static ShanheCampaignDefinition Definition()
    {
        ShanheRewardDefinition Member(string id, string skill, int rarity) => new(id, id)
        { TacticSkillId = skill, Rarity = rarity, TacticSchoolId = ShanheTeamTactics.SchoolId };
        return new("fixture:team-growth", "队友流成长", [Hero], ["first"],
            [new("first", "首战", ShanheNodeKind.Battle, ["second"], [new("first-encounter", "首战", [new(Enemy) { StartingHand = [] }])]),
             new("second", "次战", ShanheNodeKind.Battle, [], [new("second-encounter", "次战", [new(Enemy) { StartingHand = [] }])])],
            [Member("common-draw", "shanhe:duipai-2026", 1), Member("common-slash", "shanhe:duisha-2026", 1),
             new("rare-opening", "普通同行") { Rarity = 2, TacticSchoolId = ShanheTeamTactics.SchoolId,
                 OpeningCompanionPool = [new(AllyA) { StartingHand = [] }] },
             Member("rare-armor", "shanhe:duijia-2026", 2),
             new("epic-opening", "精英同行") { Rarity = 3, TacticSchoolId = ShanheTeamTactics.SchoolId,
                 OpeningCompanionPool = [new(AllyB) { StartingHand = [] }] },
             Member(ShanheTeamTactics.HealthRewardId, ShanheTeamTactics.HealthSkillId, 3),
             new(ShanheTeamTactics.SetRewardId, "队友流套装") { Rarity = 4 }])
        { TeamRankSetRewardId = ShanheTeamTactics.SetRewardId };
    }

    private static ContentRegistry Registry(ShanheBattleSetup setup) => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheTeamGrowthPackage(), new StandardShanheRankPrivilegePackage(), new Fixture(setup));
    private static GameEngine Battle(ShanheBattleSetup setup) => GameEngine.CreateStandard(new()
    {
        Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count, HumanSeat = 0, HumanRole = null,
        HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
        UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
    }, Registry(setup));
    private static void Accept(ShanheCampaignSession run, ShanheCampaignCommand command)
    { var result = run.Submit(command); Need(result.Accepted, result.Error ?? "Team growth campaign command rejected."); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Team growth native command rejected."); }
    private static void Strike(GameEngine game, string activation, int target)
    {
        Accept(game, new UseProgramSkillCommand(0, Driver, activation, [], [target], game.Revision, game.PendingDecision!.PromptId)); Play(game);
    }
    private static void Play(GameEngine game) => Reach(game, () => game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0);
    private static void Reach(GameEngine game, Func<bool> stop)
    {
        for (var step = 0; step < 160; step++) { if (stop()) return; Accept(game, new AdvanceOneStepCommand(game.Revision)); }
        throw new InvalidOperationException("Team growth did not reach its bounded native boundary: " + JsonSerializer.Serialize(game.PendingDecision));
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture(ShanheBattleSetup setup) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:team-growth", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var bundle in new[] { "shanhe-tactics-2026", "ol-xi-zhi-cai" })
            {
                var assembly = typeof(StandardShanheTeamGrowthPackage).Assembly;
                using var rules = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + ".rules.json")!);
                using var shown = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + ".presentation.json")!);
                var catalog = SkillProgramCatalog.Load(rules.ReadToEnd(), shown.ReadToEnd());
                foreach (var (id, program) in catalog.Programs)
                    builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            }
            var c = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                "activations":[{"id":"eleven","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"damage","target":"selectedTarget","amount":11}]},
                 {"id":"five","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"damage","target":"selectedTarget","amount":5}]},
                 {"id":"ten","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"damage","target":"selectedTarget","amount":10}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "队友原生损伤", description = "真实濒死与战斗结算" } } }));
            builder.AddSkill(new(Driver, "队友原生损伤", "真实伤害") { Program = c.Programs[Driver] });
            builder.AddGeneral(new(Hero, "玩家", "", Driver, "qun", 10));
            builder.AddGeneral(new(AllyA, "甲同行", "", setup.PlayerRank == 5 ? null : "ol:xianfu", "wei", 3));
            builder.AddGeneral(new(AllyB, "乙同行", "", null, "wei", 3));
            builder.AddGeneral(new(Enemy, "对手", "", null, "qun", 10));
            builder.AddDeck(new("fixture:team-growth-deck", "固定实体牌堆", 0, 0, [new("standard:dodge", 100)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "队友成长原生短局", 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count,
                1 + setup.AllyRoster.Count + setup.EnemyRoster.Count, new Dictionary<string, int>(), "fixture:team-growth-deck",
                GeneralPoolIds: [Hero, AllyA, AllyB, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1 + setup.AllyRoster.Count, [ShanheBattleSetup.EnemyTeamId] = setup.EnemyRoster.Count }));
        }
    }
}
