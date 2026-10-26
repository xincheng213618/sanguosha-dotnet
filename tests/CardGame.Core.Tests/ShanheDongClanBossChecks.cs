using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDongClanBossChecks
{
    private const string Player = "fixture:dong-clan-player", Driver = "fixture:dong-clan-driver";

    public static void ScreenshotDongZhuoTransitionsToShieldDongMinWithoutCuanchaoCold()
    {
        ShanheEncounterDefinition? observed = null;
        foreach (var difficulty in new[] { 1, 2, 3 })
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(difficulty,
                StandardShanheCampaignCatalog.Returning2026EditionId);
            var node = definition.Nodes.Single(node => node.Id == "luoyang");
            var encounter = node.Encounters!.Single(encounter => encounter.Id == "luoyang:dong-clan-screenshot");
            Need(node.ChapterNumber == 3 && node.Kind == ShanheNodeKind.BossBattle &&
                encounter is { Enemies.Count: 1, Allies.Count: 0, VictoryGold: 500 } &&
                encounter.Enemies[0] is { GeneralId: StandardShanheS5BossPackage.DongZhuoLuoyangId,
                    MaxHpBonus: 6, Hp: 10, DrawBonus: 4, SlashBonus: 1 } &&
                encounter.Enemies[0].QualifiedPrintedLordSkillIds!.SequenceEqual([StandardShanheS5BossPackage.DongZhuoBaonueId]) &&
                encounter.BossTransition is { GeneralId: StandardShanheS5BossPackage.DongMinBaseId,
                    MaxHp: 8, Hp: 8, DrawCards: 0 } &&
                node.Encounters!.Any(other => other.Id.StartsWith("luoyang:dongmin-current-", StringComparison.Ordinal)),
                "The independent final-boss recipe keeps the screenshot's exact two bodies and qualified original Lord skill, without replacing the older local Dong Min route or applying its generic final-stage growth.");
            observed = encounter;
        }
        var actual = observed!;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            new StandardShanheBossPackage(), new StandardShanheS5BossPackage(), new Fixture());
        var setup = new ShanheBattleSetup(91, 1, "fixture:dong-clan", Player, 30, 0, 0, 0, 0,
            [], [], [], actual.Enemies, [], new string('d', 64)) { BossTransition = actual.BossTransition };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Need(game.CreateSnapshot(0).Players[1] is { Hp: 10, MaxHp: 10, HandCount: 4 } &&
            game.CreateSnapshot(0).Players[1].Skills!.Select(skill => skill.ContentId)
                .Contains(StandardShanheS5BossPackage.DongZhuoBaonueId),
            "The actual first stage starts at 10/10 with the four observed skills, including the explicitly qualified printed Baonue.");
        Submit(game, new StartGameCommand()); Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Hurt(game); Reach(game, () => Events<ShanheBossStageStartedEvent>(game).Length == 1);
        Need(game.CreateSnapshot(0) is { Winner: Winner.None, CurrentSeat: 1 } &&
            game.CreateSnapshot(0).Players[1] is { GeneralId: StandardShanheS5BossPackage.DongMinBaseId,
                Hp: 8, MaxHp: 8, HandCount: 4 } &&
            game.CreateSnapshot(0).Players[1].Skills!.Select(skill => skill.ContentId)
                .SequenceEqual([StandardShanheS5BossPackage.DungongId]),
            "Lethal first-stage damage starts the 8/8 shield-only Dong Min with no invented refill, retained Baonue, or added Cuanchao.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(restored.Events) == JsonSerializer.Serialize(game.Events),
            "The actual post-transition native boundary restores without changing the form or replaying the transition.");
        game = restored;
        Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
            move.Reason == CardMoveReasons.Draw) == 6, "The second form's first native draw inherits the observed six-card normal draw.");
        Hurt(game); Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(Events<DamageAppliedEvent>(game).Last() is { TargetSeat: 1, Amount: 3 } &&
            game.CreateSnapshot(0).Players[1].Hp == 5,
            "The actual shield-only second form caps a genuine twenty-damage attempt to three.");
    }

    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static PendingDecision? Prompt(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static void Hurt(GameEngine game) => Submit(game, new UseProgramSkillCommand(0, Driver, "hurt", [], [1], game.Revision, Prompt(game)!.PromptId));
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Dong clan native command rejected.");
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 140; step++)
        {
            if (reached()) return;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Dong clan native boundary missing: " + JsonSerializer.Serialize(Prompt(game)));
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:dong-clan-boss", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"hurt","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Driver}}":{"name":"真实伤害驱动","description":"验证截图首领实际阶段与伤害上限。"}
                }
                }
                """);
            builder.AddSkill(new(Driver, "真实伤害驱动", "实际伤害") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Player, "玩家", "supporter", Driver, "shu", 30));
            builder.AddDeck(new("fixture:dong-clan-deck", "短局牌堆", 4, 2, [new("standard:peach", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "董氏凶傑原生短局", 2, 2, new Dictionary<string, int>(), "fixture:dong-clan-deck",
                GeneralPoolIds: [Player, StandardShanheS5BossPackage.DongZhuoLuoyangId, StandardShanheS5BossPackage.DongMinBaseId],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
