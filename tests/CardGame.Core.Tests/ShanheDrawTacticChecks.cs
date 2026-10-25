using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDrawTacticChecks
{
    private const string Owner = "fixture:draw-tactic-owner", Driver = "fixture:draw-tactic-driver";
    private const string Quiet = "fixture:draw-tactic-quiet", Deck = "fixture:draw-tactic-deck";

    public static void AdditiveNormalDrawAndOwnEndingKeepNativeDiscardAndSkillDraw()
    {
        var s = Scene.Start([StandardShanheDrawTacticsPackage.MopaiSkillId,
            StandardShanheDrawTacticsPackage.Mopai2SkillId, StandardShanheDrawTacticsPackage.JixingjunSkillId,
            StandardShanheDrawTacticsPackage.HoufaXianzhiSkillId]);
        Need(s.NormalDraws(0).Length == 5 && s.V(0).HandCount == 7 && s.V(0).HandLimit == 3 &&
            s.G.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                move.Reason == CardMoveReasons.InitialDeal) == 2,
            "The real first draw is 2+1+2+1-1 physical cards, while initial dealing and the native hand limit retain their own counts.");
        s.DrawOne();
        Need(s.V(0).HandCount == 8 && s.ProgramDraws(Driver).Length == 1 && s.NormalDraws(0).Length == 5,
            "An actual one-card skill draw stays one rather than applying normal draw-phase modifiers.");
        s.NextOwnPlay();
        var ending = s.ProgramDraws(StandardShanheDrawTacticsPackage.HoufaXianzhiSkillId);
        Need(s.DiscardedByOwner == 5 && s.V(0).HandLimit == 3 && s.V(0).HandCount == 11 &&
            s.NormalDraws(0).Length == 10 && ending.Length == 3 &&
            ending.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.G.CardMovements.Count(move => move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile &&
                move.Reason == CardMoveReasons.HandLimitDiscard) == 5,
            "The legal five-card discard first returns eight to capacity three; own ending then draws three exactly once across the three other turns, followed by the next real five-card draw.");
        Need(s.NormalDraws(0).Select(move => move.CardId).Concat(ending.Select(move => move.CardId)).Distinct().Count() == 13,
            "Both draw phases and the separate ending payment move distinct real deck entities into hand.");
    }

    public static void TeamDrawAddsTwoForEveryOtherPublicAllyOnly()
    {
        var s = Scene.Start([StandardShanheDrawTacticsPackage.DuipaiSkillId]);
        Need(s.NormalDraws(0).Length == 2 && s.V(0).HandCount == 4,
            "The actual aura holder keeps the ordinary two-card draw and receives no self bonus.");
        s.NextOwnPlay();
        Need(s.V(0).TeamId == s.V(1).TeamId && s.V(0).TeamId == s.V(2).TeamId &&
            s.V(0).TeamId != s.V(3).TeamId && s.NormalDraws(0).Length == 4 &&
            s.NormalDraws(1).Length == 4 && s.NormalDraws(2).Length == 4 && s.NormalDraws(3).Length == 2 &&
            s.V(1).HandCount == 4 && s.V(2).HandCount == 4 && s.V(3).HandCount == 2,
            "Both other public teammates each really draw four entities on their normal phase; the owner and the enemy remain at two despite sharing a printed faction.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
    private static string Peer(int seat) => "fixture:draw-tactic-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public int DiscardedByOwner { get; private set; }
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public CardMovementRecord[] NormalDraws(int seat) => G.CardMovements.Where(move =>
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw).ToArray();
        public CardMovementRecord[] ProgramDraws(string skillId) => G.CardMovements.Where(move =>
            move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + skillId + ".Draw").ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native draw-tactic command."); }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 300; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded draw-tactic scene ended before its expected phase boundary.");
                if (P is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards } discard)
                {
                    var cards = discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray();
                    Accept(new DiscardCardsCommand(0, cards, discard.PromptId, G.Revision));
                    DiscardedByOwner += cards.Length;
                }
                else if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices[0].Id, G.Revision));
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native draw-tactic phase boundary.");
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void DrawOne()
        {
            Accept(new UseProgramSkillCommand(0, Driver, "draw-one", [], [], G.Revision, P!.PromptId));
            Play();
        }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => G.State.TurnNumber > turn && P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(string[] tactics)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDrawTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:draw-tactics", Owner, 3, 0, 0, 0, 0, [], [], [],
                [new(Peer(3)) { StartingHand = [] }],
                [new(Peer(1)) { StartingHand = [] }, new(Peer(2)) { StartingHand = [] }], new string('d', 64))
                { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-draw-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "原生摸牌战法驱动", description = "真实摸牌、弃牌和阶段" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "摸牌战法使用者", "supporter", Driver, "qun", 3));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定公开队伍角色", "supporter", Quiet, "qun", 12));
            builder.AddDeck(new(Deck, "固定实体闪", 2, 2, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "摸牌战法实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 3, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
