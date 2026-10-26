using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuideSkillsChecks
{
    private const string Owner = "fixture:guide-owner", Ally = "fixture:guide-ally", Enemy = "fixture:guide-enemy", Driver = "fixture:guide-driver";
    public static void BuwuUsesActualAlliedDamageAndPreventsFriendlyDamageCold()
    {
        var (game, registry) = Create(false);
        Play(ref game);
        Need(Damage(game).Any(item => item.SourceSeat == 1 && item.TargetSeat == 2 && item.Amount == 2) &&
            Draws(game) == 2, "A different actual allied source gains one damage and the Buwu owner draws the final two damage points.");
        Use(game, "hurt", [2]); Play(ref game);
        Need(Damage(game).Count(item => item.TargetSeat == 2 && item.Amount == 2) == 2 && Draws(game) == 4,
            "The owner also counts as friendly, increasing a real skill hit and drawing its actual amount.");
        Use(game, "hurt", [1]); Play(ref game);
        Use(game, "hurt", [0]); Play(ref game);
        Need(game.CreateSnapshot(0).Players[0].Hp == 6 && game.CreateSnapshot(0).Players[1].Hp == 6 &&
            game.Events.Count(item => item.Payload is ProgramDamagePreventedEvent prevented && prevented.SkillId == StandardShanheGuideSkillsPackage.BuwuSkillId) == 2 && Draws(game) == 4,
            "Friendly-to-friendly and self damage are prevented, producing no damage draw.");
        Use(game, "request", [2]); Play(ref game);
        Need(Damage(game).Any(item => item.SourceSeat == 2 && item.TargetSeat == 0 && item.Amount == 1) && Draws(game) == 4,
            "Enemy damage against the team receives neither Buwu's bonus nor its draw.");
        game = Cold(game, registry);
        return;
        static DamageAppliedEvent[] Damage(GameEngine engine) => engine.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().ToArray();
        static int Draws(GameEngine engine) => engine.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program.shanhe-buwu.draw");
    }
    public static void ShuanghunRecoversMaximumRefillsMaximumAndRestoresOnlyWithLivingAllyCold()
    {
        var (game, registry) = Create(true); Play(ref game);
        for (var occurrence = 0; occurrence < 2; occurrence++)
        {
            Use(game, "die", []); Play(ref game);
            var owner = game.CreateSnapshot(0).Players[0];
            Need(owner is { Hp: 6, MaxHp: 6, HandCount: 6, IsChained: false, IsFaceDown: false, HandLimit: 10 } &&
                game.Events.Count(item => item.Payload is ShanheShuanghunUseRestoredEvent) == occurrence + 1 &&
                owner.SkillRuntimeStates!.Single(state => state.SkillId == StandardShanheGuideSkillsPackage.ShuanghunSkillId).Usages.All(usage => usage.Scope != SkillUsageScope.Game),
                "The mandatory rescue restores both states, recovers to actual maximum HP, refills to maximum HP rather than modified hand limit and returns its paid use while another ally lives.");
            game = Cold(game, registry);
        }
        Use(game, "kill-ally", [1]); Play(ref game);
        Need(!game.CreateSnapshot(0).Players[1].IsAlive, "The original allied participant is genuinely dead before the final rescue.");
        Use(game, "die", []); Play(ref game); game = Cold(game, registry);
        var final = game.CreateSnapshot(0).Players[0];
        Need(final.Hp == 6 && final.SkillRuntimeStates!.Single(state => state.SkillId == StandardShanheGuideSkillsPackage.ShuanghunSkillId)
            .Usages.Any(usage => usage.Scope == SkillUsageScope.Game && usage.Count == 1) &&
            game.Events.Count(item => item.Payload is ShanheShuanghunUseRestoredEvent) == 2,
            "With no other living ally the final paid rescue remains consumed across cold replay.");
    }

    private static (GameEngine, ContentRegistry) Create(bool rebirth)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheGuideSkillsPackage(), new Fixture(rebirth));
        var setup = new ShanheBattleSetup(71, 1, "guide-" + rebirth, Owner, 6, 0, 0, 0, 0, [], [], [],
            [new(Enemy) { StartingHand = [new(CardKind.Slash, Suit.Spade, 7)] }], [new(Ally) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Submit(game, new StartGameCommand()); return (game, registry);
    }
    private static void Use(GameEngine game, string activation, IReadOnlyList<int> targets) =>
        Submit(game, new UseProgramSkillCommand(0, Driver, activation, [], targets, game.Revision, game.PendingDecision!.PromptId));
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Guide skill native command rejected."); }
    private static void Play(ref GameEngine game)
    {
        for (var step = 0; step < 200; step++)
        {
            if (game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
            if (game.PendingDecision is { PlayerSeat: 0 } pending)
            {
                var choice = pending.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "activate") ??
                    pending.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "let-die") ?? pending.Choices[0];
                Submit(game, new AnswerPromptCommand(0, pending.PromptId, choice.Id, game.Revision));
            }
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The guide skill did not return to its native Play boundary.");
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var serialized = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(serialized), registry);
        Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == serialized &&
            SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "Actual damage, refill and exact limited-skill availability survive cold journal replay without repeated payments.");
        return restored;
    }
    private sealed class Fixture(bool rebirth) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guide-skills", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                 {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"larger-hand-limit","query":"handLimit","operation":"add","value":4,"priority":0}],"activations":[
                  {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                  {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"request"}]},
                  {"id":"die","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"owner","chained":true},{"op":"setFaceState","target":"owner","faceDown":true},{"op":"loseHp","target":"owner","amount":7}]},
                  {"id":"kill-ally","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":8}]}]},
                 {"id":"fixture:guide-ally-hit","revision":1,"triggers":[{"id":"opening-hit","window":"gameStarting","subject":"owner","optional":false,
                  "effects":[{"op":"selectAllPublicTeamTargets","target":"owner","targetKind":"publicTeamEnemies","allowEmpty":false},{"op":"damageParticipants","target":"selectedTargets","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Driver] = new { name = "真实规则驱动", description = "伤害、濒死及队友存活" }, ["fixture:guide-ally-hit"] = new { name = "友军开局实伤", description = "对敌人造成真实伤害" } } }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program });
            builder.AddGeneral(new(Owner, "行者", "", rebirth ? StandardShanheGuideSkillsPackage.ShuanghunSkillId : StandardShanheGuideSkillsPackage.BuwuSkillId, "qun", 6, [Driver]));
            builder.AddGeneral(new(Ally, "友军", "", rebirth ? null : "fixture:guide-ally-hit", "shu", 6));
            builder.AddGeneral(new(Enemy, "敌军", "", null, "wei", 12));
            builder.AddDeck(new("fixture:guide-deck", "实际实体闪", 0, 0, [new("standard:dodge", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "攻略技能", 3, 3, new Dictionary<string, int>(), "fixture:guide-deck",
                GeneralPoolIds: [Owner, Ally, Enemy], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
