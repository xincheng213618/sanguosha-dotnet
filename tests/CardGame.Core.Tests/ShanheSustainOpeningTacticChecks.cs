using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSustainOpeningTacticChecks
{
    private const string Feedback = StandardShanheSustainOpeningTacticsPackage.RuoFankuiSkillId;
    private const string Reserve = StandardShanheSustainOpeningTacticsPackage.LiangcaoChubeiSkillId;
    private const string Owner = "fixture:sustain-opening-owner", Enemy = "fixture:sustain-opening-enemy";
    private const string Driver = "fixture:sustain-opening-driver", Deck = "fixture:sustain-opening-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheSustainOpeningTacticsPackage(), new Fixture()));

    public static void ReceivedDamagePointsDrawRealCardsWhileHpLossStaysSeparate()
    {
        var s = Scene.Start(12, [Feedback]);
        s.Use("incoming-two", [1]);
        Need(s.View.Hp == 10 && s.FeedbackMoves.Length == 2 && s.View.HandCount == 2 &&
            s.Events<DamageAppliedEvent>() is [{ SourceSeat: 1, TargetSeat: 0, Amount: 2 }],
            "A genuine two-point received damage instance invokes one native draw per point and moves two real cards into the holder's hand.");
        s.Use("lose-one");
        Need(s.View.Hp == 9 && s.FeedbackMoves.Length == 2 &&
            s.Events<DamageAppliedEvent>().Length == 1 &&
            s.Events<ProgramSkillHpLostEvent>().Single() is { TargetSeat: 0, Amount: 1 },
            "Direct native HP loss changes health without creating received damage or another feedback draw.");
        s.Use("armor-three");
        s.Use("incoming-two", [1]);
        Need(s.View is { Hp: 9, Armor: 1 } && s.FeedbackMoves.Length == 4 &&
            s.Events<DamageAppliedEvent>().Last() is { SourceSeat: 1, TargetSeat: 0, Amount: 2,
                ArmorPayment: { ArmorAbsorbed: 2, HpLossAmount: 0 } },
            "Full Armor absorption preserves two actual damage points and therefore two feedback draws even though no HP is lost.");
        s.Use("incoming-three", [1]);
        Need(s.View is { Hp: 7, Armor: 0, HandCount: 7 } && s.FeedbackMoves.Length == 7 &&
            s.Events<DamageAppliedEvent>().Last() is { Amount: 3,
                ArmorPayment: { ArmorAbsorbed: 1, HpLossAmount: 2 } } &&
            s.FeedbackMoves.All(move => move.CardId > 0 && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.FeedbackMoves.Select(move => move.CardId).Distinct().Count() == 7 &&
            s.Events<ProgramBindingResolvedEvent>().Count(binding => binding.SkillId == Feedback && binding.Completed && binding.Activated) == 7,
            "Partial absorption still pays all three actual damage points, keeping the seven separately resolved draws and seven distinct physical deck entities.");
        var absent = Scene.Start(12, []);
        absent.Use("incoming-two", [1]);
        Need(absent.View.Hp == 10 && absent.View.HandCount == 0 && absent.FeedbackMoves.Length == 0,
            "Registering the source without an actual holder grants no damage draw.");
    }

    public static void OpeningRecoveryPaysOnlyForWoundedHolderAndNeverRepeats()
    {
        foreach (var initialHp in new[] { 10, 12 })
        {
            var s = Scene.Start(initialHp, [Reserve]);
            var recovered = initialHp < 12 ? 1 : 0;
            Need(s.View.Hp == initialHp + recovered &&
                s.Events<RecoveryAppliedEvent>().Count(fact => fact.TargetSeat == 0 && fact.Amount == 1) == recovered &&
                s.Events<RecoveryAppliedEvent>().All(fact => fact.TargetSeat == 0) &&
                s.Events<ProgramBindingStartedEvent>().Count(binding => binding.SkillId == Reserve &&
                    binding.OwnerSeat == 0 && binding.Window == SkillProgramTriggerWindow.GameStarting) == 1,
                "The held source uses the actual GameStarting recovery window once: a wounded holder recovers one and a full-health holder gains no recovery benefit.");
            s.Use("lose-one");
            var afterLoss = s.View.Hp;
            s.NextOwnPlay();
            Need(s.View.Hp == afterLoss && s.Events<RecoveryAppliedEvent>().Length == recovered &&
                s.Events<ProgramBindingStartedEvent>().Count(binding => binding.SkillId == Reserve) == 1,
                "Losing real HP and crossing a later natural turn cannot repeat the already completed opening recovery, including after a full-health opening.");
        }
        var absent = Scene.Start(10, []);
        Need(absent.View.Hp == 10 && absent.Events<RecoveryAppliedEvent>().Length == 0,
            "An injured player without the source receives no opening recovery.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] FeedbackMoves => Game.CardMovements.Where(move =>
            move.Reason.Value == "skill-program." + Feedback + ".Draw").ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native sustain tactic command."); }
        public void Use(string activation, int[]? targets = null)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], Game.Revision, Game.PendingDecision!.PromptId));
            Play();
        }
        public void NextOwnPlay()
        { Accept(new EndPlayPhaseCommand(0, Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        private void Play()
        {
            for (var step = 0; step < 100; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(Game.PendingDecision?.PlayerSeat != 0, "The bounded sustain scene reached an unexpected human prompt.");
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded sustain tactic play boundary.");
        }
        public static Scene Start(int hp, string[] tactics)
        {
            var setup = new ShanheBattleSetup(83, 1, "fixture:sustain-opening", Owner, hp, 0, 0, 0, 0,
                [], [], [], [new(Enemy)], [], new string('e', 64)) { TacticSkillIds = tactics };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sustain-opening-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"incoming-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]},
                    {"id":"incoming-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":3}]},
                    {"id":"lose-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"armor-three","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"grantArmor","target":"owner","amount":3}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生续航驱动", description = "真实伤害、失去体力与护甲支付" } } }));
            builder.AddSkill(new(Driver, "原生续航驱动", "真实伤害、失去体力与护甲支付")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "续航战法持有者", "supporter", Driver, "shu", 12));
            builder.AddGeneral(new(Enemy, "固定守军", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "零自然摸牌的实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(index =>
                new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "续航战法实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
