using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLowHpDrawChecks
{
    private const string Owner = "fixture:low-hp-draw-owner", Peer = "fixture:low-hp-draw-peer";
    private const string Driver = "fixture:low-hp-draw-driver", Quiet = "fixture:low-hp-draw-quiet";
    private const string Deck = "fixture:low-hp-draw-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheLowHpDrawPackage(), new Fixture()));

    public static void CurrentHealthThresholdsStackOnlyDuringTheOwnersRealDrawPhase()
    {
        var scene = Scene.Start(6);
        Need(scene.Player.Hp == 6 && scene.NormalDraws(0) == 2 && scene.Player.HandCount == 3,
            "Both owned sources remain inactive above five HP; the native opening still deals one and draws two.");
        scene.Use("draw-two");
        Need(scene.Player.HandCount == 5 && scene.NormalDraws(0) == 2 && scene.SkillDraws == 2,
            "A real skill draw uses its own fixed two-card budget rather than either normal-draw modifier.");

        scene.Use("lose-one"); scene.NextOwnPlay(expectedHp: 5, expectedDraws: 3);
        scene.Use("lose-two"); scene.NextOwnPlay(expectedHp: 3, expectedDraws: 4);
        scene.Use("heal-one"); scene.NextOwnPlay(expectedHp: 4, expectedDraws: 3);
        scene.Use("heal-two"); scene.NextOwnPlay(expectedHp: 6, expectedDraws: 2);
        Need(scene.SkillDraws == 2 && scene.Events<DamageAppliedEvent>().Length == 0 &&
                scene.Events<RecoveryAppliedEvent>().Select(e => e.Amount).SequenceEqual([1, 2]),
            "The inclusive three/five thresholds follow actual HP loss and recovery; they grant no damage and never inflate the earlier skill draw.");

        var wounded = Scene.Start(3);
        Need(wounded.Player.Hp == 3 && wounded.NormalDraws(0) == 4 && wounded.Player.HandCount == 5,
            "At three starting HP both sources add to the first actual draw phase, while the one-card initial deal remains unchanged.");
        wounded.Use("draw-two");
        Need(wounded.SkillDraws == 2 && wounded.NormalDraws(0) == 4 && wounded.Player.HandCount == 7,
            "Even when both sources qualify, actual skill draws remain exactly their declared amount.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine Game { get; } = game;
        private PendingDecision? Prompt => Game.PendingDecision;
        public PlayerSnapshot Player => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int NormalDraws(int seat) => Game.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw);
        public int SkillDraws => Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + Driver + ".Draw");
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native low-HP draw command."); }
        private void ReachPlay()
        {
            for (var step = 0; step < 180; step++)
            {
                if (Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.State.Winner == Winner.None && Prompt is not { PlayerSeat: 0 },
                    "The passive low-HP draw fixture reached an unexpected terminal or human decision.");
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The low-HP draw fixture did not reach its bounded Play boundary.");
        }
        public void Use(string activation)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], Game.Revision, Prompt!.PromptId)); ReachPlay();
        }
        public void NextOwnPlay(int expectedHp, int expectedDraws)
        {
            var before = NormalDraws(0); var enemyBefore = NormalDraws(1); var turn = Game.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId)); ReachPlay();
            Need(Game.State.TurnNumber > turn && Player.Hp == expectedHp && NormalDraws(0) - before == expectedDraws &&
                    NormalDraws(1) - enemyBefore == 2,
                "The next genuine draw phase must use current owner HP, stack the qualified sources and leave the unowned enemy's draw at two.");
        }
        public static Scene Start(int hp)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:low-hp-draw", Owner, hp, 0, 0, 0, 0,
                [], [], [], [new(Peer) { StartingHand = [] }], [], new string('a', 64))
            { TacticSkillIds = [StandardShanheLowHpDrawPackage.Kuangbao3SkillId, StandardShanheLowHpDrawPackage.Kuangbao4SkillId] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 14, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-low-hp-draw", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"keep-drawn-cards","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
                    {"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"lose-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"owner","amount":2}]},
                    {"id":"heal-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"recover","target":"owner","amount":1}]},
                    {"id":"heal-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"recover","target":"owner","amount":2}]},
                    {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"draw","target":"owner","amount":2}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[
                    {"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                     "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "体力与实际摸牌", description = "真实体力变化、摸牌与阶段。" })
                }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "残血战法使用者", "supporter", Driver, "shu", 6));
            builder.AddGeneral(new(Peer, "静默守军", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "固定闪牌堆", 1, 2, [])
                { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "残血摸牌实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
