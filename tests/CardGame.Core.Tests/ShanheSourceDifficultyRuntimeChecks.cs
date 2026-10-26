using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDifficultyRuntimeChecks
{
    private const string Hero = "fixture:source-growth-player", Enemy = "fixture:source-growth-front", Boss = "fixture:source-growth-boss";
    private const string Driver = "fixture:source-growth-driver";

    public static void FrozenPreviewActualArmorDamageAndConditionalBossRegistryCold()
    {
        var definition = Definition(25);
        var session = Create(definition);
        var preview = session.PreviewEncounter("front");
        var grown = preview.Enemies.Single();
        Need(grown is { Hp: 8, MaxHpBonus: 4, InitialHandBonus: 8, DrawBonus: 5, SlashBonus: 3,
                InitialArmor: 9, ArmorRecoveryPerRound: 2, ArmorRecoveryCap: 3, ExtraDamage: 2 },
            "Tier 25 projects the verified day/chapter table onto the actual base recipe.");
        Accept(session, new MoveShanheNodeCommand("front", session.Revision));
        var setup = session.PendingBattleSetup!;
        Need(JsonSerializer.Serialize(preview.Enemies) == JsonSerializer.Serialize(setup.EnemyRoster) && setup.SourceDifficultyId == 25,
            "Read-only preview and the frozen actual battle carry identical source numeric inputs.");
        Need(setup.MarketSetup is { Definition.SourceDifficultyId: null, Definition.SourceEnemyGrowth: null, Options.SourceDifficultyId: null },
            "Only the detached shopping recipe omits the source execution contract; it can round-trip through battle JSON.");
        var scene = new Scene(setup);
        scene.Play();
        Need(scene.G.CreateSnapshot(0).Players[1] is { Hp: 8, MaxHp: 8, Armor: 9, HandCount: 12 },
            "Source HP, maximum HP, initial armor and real opening cards enter native player state.");
        scene.PlayerRound(2);
        Need(scene.G.CreateSnapshot(0).Players[0].Hp == 8 && scene.G.CreateSnapshot(0).Players[1].Armor == 9 &&
            scene.Events<CardUseDeclaredEvent>().Count(fact => fact.SourceSeat == 1 && fact.CardKind == CardKind.Slash) == 4 &&
            scene.Events<DamageAppliedEvent>().Where(fact => fact.SourceSeat == 1 && fact.TargetSeat == 0).All(fact => fact.Amount == 3) &&
            scene.G.CardMovements.Count(movement => movement.TurnNumber == 2 && movement.To == CardLocation.Hand(1) && movement.Reason == CardMoveReasons.Draw) == 7 &&
            scene.Events<ShanheSourceArmorRestoredEvent>().Length == 0,
            "Real AI receives the draw/Slash quotas and damage bonus; an initial armor value above the recovery cap is never reduced to it.");
        scene.Use("ten", 1); scene.Cold();
        Need(scene.G.CreateSnapshot(0).Players[1] is { Armor: 0, Hp: 7 }, "Actual damage consumes source initial armor before HP.");
        scene.PlayerRound(3);
        Need(scene.G.CreateSnapshot(0).Players[1].Armor == 2, "The next actual round restores its configured armor amount.");
        scene.PlayerRound(4); scene.Cold();
        Need(scene.G.CreateSnapshot(0).Players[1].Armor == 3 && scene.Events<ShanheSourceArmorRestoredEvent>().Select(fact => fact.Amount).SequenceEqual([2, 1]),
            "Recovery grants only the missing amount at the cap, with no duplicated grant after cold replay.");
        scene.Use("twenty", 1, completed: true);
        var hp = scene.G.CreateSnapshot(0).Players[0].Hp;
        Need(session.CompleteBattle(scene.G, session.Revision).Accepted && session.Snapshot.Hp == hp && hp == 8,
            "Tier six and above do not apply the authored automatic victory heal.");
        session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
        var boss = session.PreviewEncounter("boss");
        Need(boss.Enemies.Single().GeneralId == Boss && boss.Enemies.Single().Hp == 10,
            "A later boss obtains its own conditional registry and source growth instead of reusing the front encounter's content.");
        Accept(session, new MoveShanheNodeCommand("boss", session.Revision));
        Need(JsonSerializer.Serialize(boss.Enemies) == JsonSerializer.Serialize(session.PendingBattleSetup!.EnemyRoster),
            "The later source boss retains preview parity after campaign cold replay.");

        var ordinary = Create(Definition(1)).PreviewEncounter("front").Enemies.Single();
        Need(ordinary is { Hp: null, MaxHpBonus: 0, InitialHandBonus: 0, DrawBonus: 0, SlashBonus: 0, InitialArmor: 0 },
            "Ordinary one keeps its authored base recipe without additional source growth.");
        Need(Create(Definition(6, terrain: true)).PreviewEncounter("front").TerrainSkillId is null &&
            Create(Definition(11, terrain: true)).PreviewEncounter("front").TerrainSkillId == StandardShanheTerrainPackage.ForestId,
            "Source terrain opens at hard one, independently of the three local difficulty values.");
        try { _ = Create(definition with { SourceEnemyGrowthHash = new string('0', 64) }); throw new InvalidOperationException("Stale source data hash was accepted."); }
        catch (ArgumentException) { }
    }

    public static void SourceSecondFormUsesItsOwnGrownNativeNumbersCold()
    {
        var definition = Definition(25, stage: true);
        var session = Create(definition);
        var transition = session.PreviewEncounter("front").BossTransition!;
        Need(transition is { MaxHp: 9, Hp: 9, DrawCards: 12, Combatant.DrawBonus: 6, Combatant.SlashBonus: 4,
                Combatant.InitialArmor: 9, Combatant.ArmorRecoveryPerRound: 2, Combatant.ExtraDamage: 3 },
            "A source second form grows from its explicit stage health and its own native stat recipe.");
        Accept(session, new MoveShanheNodeCommand("front", session.Revision));
        var scene = new Scene(session.PendingBattleSetup!); scene.Play();
        scene.Use("twenty", 1, transformed: true); scene.Cold();
        Need(scene.G.CreateSnapshot(0).Players[1] is { GeneralId: Boss, Hp: 9, MaxHp: 9, Armor: 9 } &&
            scene.Events<ShanheBossStageStartedEvent>().Single() is { MaxHp: 9, Hp: 9 },
            "The real lethal transition installs the second form's grown health and initial armor once.");
        scene.Play();
        Need(scene.G.CardMovements.Count(movement => movement.TurnNumber == 2 && movement.To == CardLocation.Hand(1) && movement.Reason == CardMoveReasons.Draw) == 8,
            "The transformed boss's real draw phase uses its stage growth rather than the former general's draw bonus.");
    }

    private static ShanheCampaignDefinition Definition(int difficulty, bool terrain = false, bool stage = false)
    {
        var first = new ShanheEncounterDefinition("front-fight", "前战", [new(Enemy)
        {
            StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray(), ArmorRecoveryCap = 3
        }], HealAfterVictory: 4)
        {
            TerrainSkillId = terrain ? StandardShanheTerrainPackage.ForestId : null,
            BossTransition = stage ? new(0, Boss, 5, 5, 4)
            { Combatant = new(Boss, DrawBonus: 1, SlashBonus: 1) { ExtraDamage = 1, ArmorRecoveryCap = 3 } } : null
        };
        var calculator = StandardShanheSourceEnemyGrowth.Season5;
        return new("fixture:source-growth-" + difficulty, "来源难度", [Hero], ["front"],
            [new("front", "前战", ShanheNodeKind.BossBattle, ["market"], [first]) { ChapterNumber = 3 },
             new("boss", "后来首领", ShanheNodeKind.BossBattle, [], [new("later", "后来首领", [new(Boss)])]) { ChapterNumber = 3 },
             new("market", "常驻集市", ShanheNodeKind.Shop, ["boss"], Choices: [new("supply", "补给", 1, RewardId: "fixture:source-supply", LeavesNode: false)]) { ChapterNumber = 3 }],
            [new("fixture:source-supply", "补给", Heal: 1)])
        { SourceDifficultyId = difficulty, SourceEnemyGrowth = calculator, SourceEnemyGrowthHash = calculator.ContentHash, SourceGrowthPolicy = ShanheSourceDifficultyRuntime.PolicyId };
    }
    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition) => ShanheCampaignSession.Create(definition,
        new(71, Hero) { SourceDifficultyId = definition.SourceDifficultyId, UseCodexRewardPool = true, PersistentMarket = true }, Registry);
    private static ContentRegistry Registry(ShanheBattleSetup setup) => ContentRegistry.Build(new StandardContentPackage(),
        new StandardShanheTerrainPackage(), new StandardShanheMarketPackage(), new Fixture(setup));
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Source campaign command rejected."); }

    private sealed class Scene
    {
        private readonly ContentRegistry _registry;
        public GameEngine G { get; private set; }
        public Scene(ShanheBattleSetup setup)
        {
            _registry = Registry(setup);
            G = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 30 }, _registry);
            Submit(new StartGameCommand());
        }
        public T[] Events<T>() where T : IGameEvent => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void PlayerRound(int round) => Reach(() => G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            G.ResolutionStack.Count == 0 && Events<RoundStartedEvent>().Last().RoundNumber == round, endPlay: true);
        public void Use(string id, int target, bool completed = false, bool transformed = false)
        {
            Play(); Submit(new UseProgramSkillCommand(0, Driver, id, [], [target], G.Revision, G.PendingDecision!.PromptId));
            if (completed) Reach(() => G.State.Status == EngineStatus.Completed && G.ResolutionStack.Count == 0);
            else if (transformed) Reach(() => Events<ShanheBossStageStartedEvent>().Length == 1);
            else Play();
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), _registry);
            Need(before == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold replay preserves all native source numeric state and owning resolution boundaries.");
        }
        private void Submit(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Source battle command rejected."); }
        private void Reach(Func<bool> stop, bool endPlay = false)
        {
            for (var step = 0; step < 240; step++)
            {
                if (stop()) return;
                if (G.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard)
                    {
                        if (G.ResolutionStack.Count != 0) { Submit(new AdvanceOneStepCommand(G.Revision)); continue; }
                        if (!endPlay) throw new InvalidOperationException("Unexpected source Play boundary.");
                        Submit(new EndPlayPhaseCommand(0, G.Revision, prompt.PromptId)); continue;
                    }
                    var choice = prompt.Choices.FirstOrDefault(candidate => candidate.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    if (choice is not null) { Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision)); continue; }
                    throw new InvalidOperationException("Unexpected source human prompt: " + JsonSerializer.Serialize(prompt));
                }
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source battle did not reach its bounded native boundary: " + JsonSerializer.Serialize(G.PendingDecision));
        }
    }

    private sealed class Fixture(ShanheBattleSetup setup) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-runtime", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var program = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                {"id":"ten","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":10}]},
                {"id":"twenty","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "来源伤害", description = "原生数值边界" } } }));
            builder.AddSkill(new(Driver, "来源伤害", "原生数值边界") { Program = program.Programs[Driver], ProgramPresentation = program.Presentations[Driver] });
            builder.AddGeneral(new(Hero, "来源玩家", "", Driver, "qun", 20));
            // Deliberately register only this encounter's NPC package, including its reserved second form.
            var ids = setup.EnemyRoster.Select(member => member.GeneralId).Concat(setup.BossTransition is { } transition ? [transition.GeneralId] : Array.Empty<string>()).Distinct().ToArray();
            foreach (var id in ids) builder.AddGeneral(new(id, id == Enemy ? "前战小兵" : "后来首领", "", "standard:none", "qun", id == Enemy ? 4 : 6));
            builder.AddDeck(new("fixture:source-runtime-deck", "固定闪牌堆", 0, 2, [new("standard:dodge", 180)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "来源难度原生短局", 2, 2, new Dictionary<string, int>(), "fixture:source-runtime-deck",
                GeneralPoolIds: new[] { Hero }.Concat(ids).ToArray(), ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
