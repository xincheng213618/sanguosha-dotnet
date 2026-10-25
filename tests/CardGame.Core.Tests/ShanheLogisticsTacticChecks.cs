using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLogisticsTacticChecks
{
    private const string Owner = "fixture:logistics-tactic-owner", Enemy = "fixture:logistics-tactic-enemy";
    private const string Driver = "fixture:logistics-tactic-driver", Quiet = "fixture:logistics-tactic-quiet";
    private const string Deck = "fixture:logistics-tactic-deck";

    public static void StackedInitialPenaltiesClampBeforeDealAndKeepNormalDrawBonuses()
    {
        foreach (var clamp in new[] { false, true })
        {
            string[] tactics = clamp
                ? [StandardShanheLogisticsTacticsPackage.ZhaomuHouqin2SkillId,
                    StandardShanheLogisticsTacticsPackage.ZhaomuHouqinSkillId,
                    StandardShanheLogisticsTacticsPackage.ZhaomuHouqin3SkillId]
                : [StandardShanheLogisticsTacticsPackage.ZhaomuHouqin2SkillId,
                    StandardShanheLogisticsTacticsPackage.ZhaomuHouqinSkillId];
            var initial = clamp ? 0 : 1;
            var normalDraw = clamp ? 7 : 4;
            var s = Scene.Create(tactics);
            Need(s.V(0).HandCount == initial && s.V(1).HandCount == 4 &&
                s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.InitialDeal) == initial &&
                s.G.CardMovements.All(m => m.From == CardLocation.DrawPile && m.Reason == CardMoveReasons.InitialDeal),
                "The actual four-card deal adds every owner-only initial penalty before issuing entities: -3 deals one and -5 clamps to zero, while the opponent still receives four.");

            s.Start();
            Need(s.NormalDraws(0) == normalDraw && s.NormalDraws(1) == 0 && s.V(0).HandCount == initial + normalDraw,
                "The first genuine Draw phase independently adds the acquired logistics bonuses to the ordinary two-card request.");
            s.DrawOne();
            Need(s.V(0).HandCount == initial + normalDraw + 1 &&
                s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + Driver + ".Draw") == 1,
                "A real program Draw1 remains one entity, without inheriting the normal Draw-phase additions or repeating an initial penalty.");
            s.NextOwnPlay();
            Need(s.NormalDraws(0) == normalDraw * 2 && s.NormalDraws(1) == 2 &&
                s.V(0).HandCount == initial + normalDraw * 2 + 1 &&
                s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.InitialDeal) == initial,
                "The next real own Draw phase repeats only its persistent draw bonus; the opponent draws two and the completed initial deal is never charged or issued again.");
        }
    }

    public static void NegativeInitialModifiersRejectConditionalForeignAndDynamicDefinitions()
    {
        const string skill = "fixture:negative-initial-definition";
        var rules = JsonNode.Parse($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{skill}}","revision":1,"modifiers":[
              {"id":"initial","query":"initialHandSize","operation":"add","value":-1,"priority":0}]}]}
            """)!;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [skill] = new { name = "负初始手牌", description = "固定起手扣减" } } });
        _ = SkillProgramCatalog.Load(rules.ToJsonString(), presentation);
        foreach (var change in new Action<JsonObject>[]
        {
            node => node["recipientScope"] = "otherPublicTeamAllies",
            node => node["condition"] = new JsonObject { ["kind"] = "wounded" },
            node => { node.Remove("value"); node["valueExpression"] = "ownerLostHp"; },
            node => node["operation"] = "set",
            node => node["value"] = 0,
            node => node["value"] = -1025
        })
        {
            var invalid = JsonNode.Parse(rules.ToJsonString())!;
            change(invalid["skills"]![0]!["modifiers"]![0]!.AsObject());
            try { _ = SkillProgramCatalog.Load(invalid.ToJsonString(), presentation); }
            catch (InvalidOperationException error) when (error.Message.Contains("modifiers[0]", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("A negative initial modifier must retain owner-only, unconditional, fixed, non-zero Add and bounded-value validation.");
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public int NormalDraws(int seat) => G.CardMovements.Count(m => m.To == CardLocation.Hand(seat) && m.Reason == CardMoveReasons.Draw);
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected logistics tactic command."); }
        private void Play(int? afterTurn = null)
        {
            for (var step = 0; step < 150; step++)
            {
                if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                    (afterTurn is null || G.State.TurnNumber > afterTurn.Value)) return;
                Need(P?.PlayerSeat != 0, "An unexpected owner prompt interrupted the small logistics Draw-phase fixture.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The logistics fixture did not reach its bounded real Play phase.");
        }
        public void Start() { Accept(new StartGameCommand()); Play(); }
        public void DrawOne()
        { Accept(new UseProgramSkillCommand(0, Driver, "draw-one", [], [], G.Revision, P!.PromptId)); Play(); }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Play(turn);
        }
        public static Scene Create(string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheLogisticsTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:logistics-tactics", Owner, 4, 0, 0, 0, 0,
                [], [], [], [new(Enemy)], [], new string('c', 64)) { TacticSkillIds = tactics };
            return new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-logistics-tactics", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"capacity","query":"handLimit","operation":"add","value":20,"priority":0}],
                  "activations":[{"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "后勤共享流程", description = "真实额外摸一张牌" } } }));
            builder.AddSkill(new(Driver, "后勤共享流程", "真实额外摸一张牌")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddSkill(new(Quiet, "固定敌军", "无额外能力"));
            builder.AddGeneral(new(Owner, "后勤战法拥有者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Enemy, "普通敌军", "supporter", Quiet, "wei", 4));
            builder.AddDeck(new(Deck, "固定实体闪", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "招募后勤实际派牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
