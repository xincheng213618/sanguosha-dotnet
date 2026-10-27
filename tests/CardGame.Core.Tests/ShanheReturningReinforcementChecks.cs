using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheReturningReinforcementChecks
{
    private const string Driver = "fixture:returning-reinforcement-stage", Deck = "fixture:returning-reinforcement-deck";
    private static readonly string[] NewIds = ["dagu:xusheng-source-native", "huanyuan:sunce-source-native",
        "huanyuan:xunyu-source-native", "dagu:qiangwang-source-native"];

    public static void BothLocalEntriesSelectCompleteThreeTierProfilesAndCarryThemIntoTerrainRoutes()
    {
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var definition = Definition(difficulty, codex);
            var growth = difficulty - 1;
            Need(definition.SourceDifficultyId is null && definition.SourceEnemyGrowth is null &&
                definition.SourceEnemyGrowthHash is null && definition.SourceGrowthPolicy is null,
                "Both recommended local entries retain their local identity and do not attach the 25-tier execution contract.");
            foreach (var id in NewIds.Concat(new[] { "huanyuan:zhangniujiao-s5", "luoyang:gaichao-s5" }))
            {
                var node = definition.Nodes.Single(candidate => candidate.Id == id.Split(':')[0]);
                var encounter = node.Encounters!.Single(candidate => candidate.Id == id);
                var setup = Enter(definition, node, encounter);
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
                Need(setup.SourceDifficultyId is null && setup.EnemyRoster.Select(member => member.GeneralId)
                        .SequenceEqual(encounter.Enemies.Select(member => member.GeneralId)) &&
                    1 + setup.EnemyRoster.Count + setup.AllyRoster.Count <= 8 &&
                    setup.EnemyRoster.All(member => registry.IsGeneralPlayable(member.GeneralId)) &&
                    (setup.BossTransition is null || registry.IsGeneralPlayable(setup.BossTransition.GeneralId)),
                    "A fresh real session previews and moves into each actual candidate, keeping its complete roster and factory dependencies within eight seats.");
                if (NewIds.Contains(id))
                {
                    var leader = setup.EnemyRoster[0];
                    var expected = id switch
                    {
                        "dagu:xusheng-source-native" => (StandardShanheSourceXuShengPackage.MediumGeneralId, 7,
                            new[] { new ShanheStartingCard(CardKind.GhostDragonCrescentBlade, Suit.Club, 5) }),
                        "huanyuan:sunce-source-native" => (StandardShanheSourceSunCePackage.SunCeId, 6,
                            new[] { new ShanheStartingCard(CardKind.BlackLightArmor, Suit.Diamond, 2) }),
                        "huanyuan:xunyu-source-native" => (StandardShanheSourceXunYuPackage.GeneralId, 3,
                            new[] { new ShanheStartingCard(CardKind.GouXiang, Suit.Club, 2) }),
                        _ => (StandardShanheSourceQiangWangPackage.GeneralId, 7,
                            new[] { new ShanheStartingCard(CardKind.WushuangHalberd, Suit.Heart, 12), new ShanheStartingCard(CardKind.LionBelt, Suit.Club, 2) })
                    };
                    var xunyu = id == "huanyuan:xunyu-source-native";
                    Need(encounter.SourceBattleId is null && encounter.SourceCityId is null && setup.BossTransition is null &&
                        leader.GeneralId == expected.Item1 && leader.Hp == expected.Item2 + (xunyu ? 0 : 2 * growth) &&
                        leader.MaxHpBonus == 3 + 2 * growth && leader.InitialHandBonus == 1 + growth &&
                        leader.DrawBonus == 1 && leader.SlashBonus == 0 && leader.StartingHand is null &&
                        leader.SourceDifficultyAttributes is null && (leader.StartingEquipment?.Count ?? 0) == 0 &&
                        leader.StartingCards!.SequenceEqual(expected.Item3) &&
                        leader.StartingCards!.All(card => registry.Cards.Values.Any(content => content.LegacyKind == card.Kind)) &&
                        setup.EnemyRoster.Count == (xunyu ? 2 : 1),
                        "The new local source forms carry the authored HP, resources and extra hand equipment rather than raw weak source bodies or fabricated official cities.");
                    if (xunyu)
                        Need(leader.InitialArmor == 2 + growth && leader.ArmorRecoveryPerRound == 1 && leader.ArmorRecoveryCap == 2 + growth &&
                            setup.EnemyRoster[1] is { GeneralId: StandardShanheLocalTroopPackage.GuardId, DrawBonus: 0 } guard &&
                            guard.Hp == 6 + growth && guard.MaxHpBonus == growth && guard.InitialHandBonus == growth,
                            "Xun Yu retains three current HP for Quhu, while his authored armor, cap and real guard provide increasing durability.");
                }
                else if (id == "huanyuan:zhangniujiao-s5")
                {
                    var first = setup.EnemyRoster[0]; var stage = setup.BossTransition!;
                    var firstGeneral = difficulty == 1 ? StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId :
                        difficulty == 2 ? StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId : StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId;
                    Need(first.GeneralId == firstGeneral && first.Hp == 6 + 2 * growth && first.MaxHpBonus == 3 + 2 * growth &&
                        first.InitialHandBonus == 1 + growth && first.DrawBonus == 1 && first.SlashBonus == 0 &&
                        stage.GeneralId == (difficulty == 1 ? StandardShanheSourceZhangYanPackage.ZhangYanMediumId : StandardShanheSourceZhangYanPackage.ZhangYanHardId) &&
                        stage.Hp == 12 + 2 * growth && stage.MaxHp == stage.Hp && stage.DrawCards == 3 + growth &&
                        stage.Combatant is { SlashBonus: 1 } second && second.DrawBonus == 1 + growth &&
                        second.GeneralId == stage.GeneralId && second.Hp == stage.Hp && second.MaxHpBonus == 8 + 2 * growth &&
                        stage.StartingEquipment!.SequenceEqual([new ShanheStartingCard(CardKind.ShanheSanshou, Suit.Heart, 12)]),
                        "The existing local Yellow Turban encounter now contains the complete native Zhang Yan stage and its separate numeric member.");
                }
                else
                {
                    var stage = setup.BossTransition!;
                    Need(stage.GeneralId == StandardShanheSourceShenNuStagePackage.GeneralId && stage.MaxHp == 20 + 4 * growth &&
                        stage.Hp == stage.MaxHp && stage.DrawCards == 3 + growth && stage.Combatant is { SlashBonus: 1 } second &&
                        second.DrawBonus == 1 + growth && second.GeneralId == stage.GeneralId && second.Hp == stage.Hp &&
                        second.MaxHpBonus == 4 * growth && stage.StartingEquipment!.SequenceEqual([
                            new ShanheStartingCard(CardKind.ShuraInfernoHalberd, Suit.Heart, 12), new(CardKind.LionBelt, Suit.Club, 2)]),
                        "The local final encounter uses real Shen Nu and its authored stage resources, not a repeat of its first body.");
                }
            }
            foreach (var (site, template) in new[] { ("yingyang-terrain", "dagu"), ("xiaopingjin-terrain", "dagu"),
                         ("yique-terrain", "dagu"), ("mengjin-terrain", "huanyuan"), ("yingchuan-terrain", "huanyuan") })
            {
                var clone = definition.Nodes.Single(node => node.Id == site);
                foreach (var id in NewIds.Where(id => id.StartsWith(template + ":", StringComparison.Ordinal))
                    .Concat(template == "huanyuan" ? new[] { "huanyuan:zhangniujiao-s5" } : Array.Empty<string>()))
                {
                    var copied = clone.Encounters!.Single(encounter => encounter.Id == site + ":" + id);
                    var original = definition.Nodes.Single(node => node.Id == template).Encounters!.Single(encounter => encounter.Id == id);
                    Need(copied.Enemies.Select(member => (member.GeneralId, member.Hp, member.MaxHpBonus, member.InitialHandBonus, member.DrawBonus))
                            .SequenceEqual(original.Enemies.Select(member => (member.GeneralId, member.Hp, member.MaxHpBonus, member.InitialHandBonus, member.DrawBonus))) &&
                        copied.BossTransition?.GeneralId == original.BossTransition?.GeneralId &&
                        (difficulty == 3 ? copied.TerrainSkillId is not null : copied.TerrainSkillId is null),
                        "Terrain branches copy the newly integrated candidates and native stages before applying their own difficulty-qualified terrain.");
                }
            }
        }
    }

    private static ShanheCampaignDefinition Definition(int difficulty, bool codex) => codex
        ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
        : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);

    private static ShanheBattleSetup Enter(ShanheCampaignDefinition definition, ShanheNodeDefinition node,
        ShanheEncounterDefinition encounter, bool sturdyPlayer = false)
    {
        var entry = definition with
        {
            StartingNodeIds = [node.Id], RandomStartingNode = false,
            StartingBonuses = sturdyPlayer ? new(MaxHp: 30) : definition.StartingBonuses,
            Nodes = [node with { NextNodeIds = [], Encounters = [encounter], EliteUpgradeNodeId = null, StoryEvents = null, Choices = [] }]
        };
        var session = ShanheCampaignSession.Create(entry, new(71, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var preview = session.PreviewEncounter(node.Id);
        var result = session.Submit(new MoveShanheNodeCommand(node.Id, session.Revision));
        Need(result.Accepted && preview.Id == encounter.Id && session.PendingBattleSetup is not null,
            result.Error ?? "The retained real candidate must be enterable through Preview and Move.");
        return session.PendingBattleSetup!;
    }

    private static GameEngine Start(ShanheBattleSetup setup, bool withDriver)
    {
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new NativeFixture(),
            mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        if (withDriver) setup = setup with { ExtraSkillIds = setup.ExtraSkillIds.Append(Driver).ToArray() };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count,
            HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        Submit(game, new StartGameCommand()); return game;
    }
    private static bool HumanPlay(GameEngine game) => game.CreateSnapshot(0).PendingDecision is
        { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;
    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 360; step++)
        {
            if (ready()) return;
            var prompt = game.CreateSnapshot(0).PendingDecision;
            Need(game.State.Winner == Winner.None && (prompt is null ||
                    prompt.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count != 0),
                $"Unexpected local reinforcement boundary: {game.CreateSnapshot(0).PendingDecision?.Kind}/{game.State.CurrentSeat}/{game.State.Phase}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The local reinforcement flow exceeded its bounded native boundary.");
    }
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real local reinforcement command."); }
    private static void Need(bool yes, string reason) { if (!yes) throw new InvalidOperationException(reason); }

    private sealed class NativeFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:returning-reinforcement-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Driver, revision = 1, activations = new[] { new { id = "lethal", minCards = 0, maxCards = 0,
                    minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = 1,
                    effects = new[] { new { op = "damage", target = "selectedTarget", amount = 20 } } } } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实际致死伤害", description = "原生20点伤害进入当前首领的真实阶段。" } } }));
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "真实固定无主动牌", 4, 2, [new("standard:dodge", 120)]));
        }
    }
}
