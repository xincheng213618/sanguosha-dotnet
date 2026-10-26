using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCaoruiCompanionChecks
{
    private const string Driver = "fixture:luanxin-driver", Enemy = "fixture:luanxin-enemy";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(),
        new StandardClassicGeneralPackage(), new StandardShanheCaoruiCompanionPackage(), new Fixture()));

    public static void NativeGrowthForeignDiscardAndActualNextDrawRecoverCold()
    {
        var normal = Registry.Value.Generals[StandardShanheCaoruiCompanionPackage.NormalId];
        var elite = Registry.Value.Generals[StandardShanheCaoruiCompanionPackage.EliteId];
        Need(normal.BaseHp == 3 && normal.SkillIds.ToHashSet().SetEquals(["classic:huituo", "classic:mingjian"]) &&
            elite.SkillIds.ToHashSet().SetEquals(["classic:huituo", "classic:mingjian", StandardShanheCaoruiCompanionPackage.LuanxinSkillId]),
            "Both helpers retain the precise printed support skills, and only the elite has Luanxin.");
        var setup = new ShanheBattleSetup(71, 1, "luanxin", "fixture:luanxin-owner", 3, 0, 0, 0, 0, [Driver],
            [new(CardKind.Snatch, Suit.Spade, 3), new(CardKind.Dodge, Suit.Diamond, 9)], [],
            [new(Enemy) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 5), new(CardKind.Dismantlement, Suit.Spade, 12)] }], [], new string('a', 64));
        var scene = new Scene(GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 8
        }, Registry.Value));
        scene.Submit(new StartGameCommand()); scene.Play(0);
        scene.Use("suffer");
        Need(scene.V().Hp == 2 && scene.V().MaxHp == 4,
            "One real received damage raises only maximum health by one without healing the lost point.");
        scene.Cold(); scene.Use("hit");
        Need(scene.V().Hp == 3 && scene.V().MaxHp == 4 && scene.V(1).Hp == 9,
            "One real outgoing damage heals the skill owner by one through native recovery.");

        var oldHandLimit = scene.V().HandLimit;
        scene.Use("discard-self", [scene.V().Hand.First(card => card.Kind == CardKind.Dodge).Id]);
        Need(scene.V().HandLimit == oldHandLimit && scene.E<ProgramPersistentHandLimitChangedEvent>().Length == 0,
            "A real self-paid hand discard is not a foreign discard and grants no permanent hand capacity.");
        scene.Card(0, CardKind.Snatch, 1);
        Need(scene.E<ShanheLuanxinNextDrawIssuedEvent>().Length == 1,
            "An actual Snatch hand transfer through processing issues one next-Draw benefit.");
        scene.Cold();
        var beforeDraw = scene.G.CardMovements.Last().Sequence;
        scene.Submit(new EndPlayPhaseCommand(0, scene.G.Revision, scene.G.PendingDecision!.PromptId)); scene.Play(0);
        Need(scene.E<ProgramPersistentHandLimitChangedEvent>() is [{ Amount: 1, OwnerSeat: 0 }] &&
            scene.V().HandLimit == oldHandLimit + 1,
            "An opponent's completed Dismantlement hand discard grants one persistent hand limit through its exact native invoice.");
        Need(scene.G.CardMovements.Count(move => move.Sequence > beforeDraw && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 3 &&
            scene.E<ShanheLuanxinNextDrawAssignedEvent>().Length == 1,
            "The earned bonus adds one to the next actual draw phase and moves three real cards.");
        scene.Cold();
        beforeDraw = scene.G.CardMovements.Last().Sequence;
        scene.Submit(new EndPlayPhaseCommand(0, scene.G.Revision, scene.G.PendingDecision!.PromptId)); scene.Play(0);
        Need(scene.G.CardMovements.Count(move => move.Sequence > beforeDraw && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 2 &&
            scene.E<ShanheLuanxinNextDrawAssignedEvent>().Length == 1,
            "A later draw phase returns to base two after the one earned entitlement is consumed.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native Luanxin command rejected."); }
        public void Play(int actor)
        {
            for (var step = 0; step < 160; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat == actor) return;
                if (G.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") is "decline" or "skip")
                        ?? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
                }
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The Luanxin flow did not reach ordinary Play.");
        }
        public void Use(string activation, int[]? cards = null)
        {
            var prompt = G.PendingDecision!;
            Submit(new UseProgramSkillCommand(0, Driver, activation, cards ?? [], activation == "discard-self" ? [] : [1], G.Revision, prompt.PromptId));
            Play(0);
        }
        public void Card(int actor, CardKind kind, int target)
        {
            var card = G.CreateSnapshot(actor).Players[actor].Hand.First(item => item.Kind == kind);
            Submit(new PlayCardCommand(actor, card.Id, [target], G.Revision, G.PendingDecision!.PromptId));
            Play(actor);
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), Registry.Value);
            Need(SnapshotJson.Serialize(G.CreateSnapshot(0)) == before, "Cold replay preserves physical cards, growth and pending draw grants.");
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-luanxin", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"suffer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"discard-self","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"discardSelected","target":"owner","amount":1}]}
                  ]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "乱心原生驱动", description = "真实损伤和支付" } } }));
            builder.AddSkill(new(Driver, "驱动", "真实损伤") { Program = catalog.Programs[Driver] });
            builder.AddGeneral(new("fixture:luanxin-owner", "乱心持有者", "", StandardShanheCaoruiCompanionPackage.LuanxinSkillId, "wei", 3));
            builder.AddGeneral(new(Enemy, "对手", "", null, "qun", 10));
            builder.AddDeck(new("fixture:luanxin-deck", "固定实体闪", 0, 2, [new("standard:dodge", 100)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "乱心原生流程", 2, 2, new Dictionary<string, int>(), "fixture:luanxin-deck",
                GeneralPoolIds: ["fixture:luanxin-owner", Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
