using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanhePreparationTacticChecks
{
    private const string Owner = "fixture:preparation-owner", Driver = "fixture:preparation-driver";
    private const string Quiet = "fixture:preparation-quiet", Deck = "fixture:preparation-deck";
    private static readonly string[] Tactics =
    [StandardShanhePreparationTacticsPackage.ChoubeiSkillId,
        StandardShanhePreparationTacticsPackage.Choubei2SkillId,
        StandardShanhePreparationTacticsPackage.Choubei3SkillId];
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanhePreparationTacticsPackage(), new Fixture()));

    public static void ActualGameStartingDrawsStackAfterInitialDealAndNeverRepeatWithoutOwnership()
    {
        var s = Scene.Create(Tactics);
        var initial = s.Game.CardMovements.ToArray();
        var nextPhysicalCards = s.Game.CreateCardZoneDiagnostics().Where(card => card.Location == CardLocation.DrawPile)
            .OrderByDescending(card => card.ZoneIndex).Take(6).Select(card => card.CardId).ToArray();
        Need(initial.Length == 8 && initial.All(move => move.Reason == CardMoveReasons.InitialDeal &&
                move.From == CardLocation.DrawPile && move.TurnNumber == 0) &&
            Enumerable.Range(0, 4).All(seat => s.View(seat).HandCount == 2),
            "Owning all three sources leaves the actual ordinary two-card initial deal unchanged before the game starts.");

        s.Start();
        var opening = s.PreparationMoves();
        var bindings = s.PreparationBindings();
        var starts = s.Events<ProgramBindingStartedEvent>().Where(binding => Tactics.Contains(binding.SkillId)).ToArray();
        Need(opening.Length == 6 && opening.Select(move => move.CardId).SequenceEqual(nextPhysicalCards) &&
            opening.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                move.TurnNumber == 0 && move.Sequence > initial[^1].Sequence) &&
            opening.Select(move => move.CardId).Distinct().Count() == 6 &&
            !opening.Select(move => move.CardId).Intersect(initial.Select(move => move.CardId)).Any() &&
            starts.Length == 3 && bindings.Length == 3 &&
            Tactics.Select((skill, index) => (skill, amount: index + 1)).All(item =>
                starts.Count(binding => binding.SkillId == item.skill && binding.OwnerSeat == 0 &&
                    binding.Window == SkillProgramTriggerWindow.GameStarting) == 1 &&
                bindings.Count(binding => binding.SkillId == item.skill && binding.OwnerSeat == 0 &&
                    binding.Window == SkillProgramTriggerWindow.GameStarting && binding.Activated && binding.Completed &&
                    starts.Any(start => start.FrameId == binding.FrameId && start.SkillId == binding.SkillId &&
                        start.BindingId == binding.BindingId && start.SkillInstanceId == binding.SkillInstanceId)) == 1 &&
                opening.Count(move => move.Reason.Value == "skill-program." + item.skill + ".Draw") == item.amount) &&
            s.View(0).HandCount == 10 && Enumerable.Range(1, 3).All(seat => s.View(seat).HandCount == 2),
            "The three held sources separately draw one, two and three real next cards from the already shuffled deck at turn zero, after InitialDeal and before the ordinary first draw phase. Actual facts: " +
            JsonSerializer.Serialize(new
            {
                Checks = new
                {
                    OpeningCount = opening.Length == 6,
                    DeckOrder = opening.Select(move => move.CardId).SequenceEqual(nextPhysicalCards),
                    PhysicalTurnZeroAfterDeal = opening.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                        move.TurnNumber == 0 && move.Sequence > initial[^1].Sequence),
                    Unique = opening.Select(move => move.CardId).Distinct().Count() == 6,
                    Fresh = !opening.Select(move => move.CardId).Intersect(initial.Select(move => move.CardId)).Any(),
                    BindingCount = starts.Length == 3 && bindings.Length == 3,
                    Hands = s.View(0).HandCount == 10 && Enumerable.Range(1, 3).All(seat => s.View(seat).HandCount == 2)
                },
                nextPhysicalCards, opening, starts, bindings,
                Sources = Tactics.Select((skill, index) => new
                {
                    skill, Expected = index + 1,
                    Actual = opening.Count(move => move.Reason.Value == "skill-program." + skill + ".Draw"),
                    Bindings = bindings.Where(binding => binding.SkillId == skill)
                }),
                Hands = Enumerable.Range(0, 4).Select(seat => s.View(seat).HandCount),
                AllDrawInvoices = s.Events<NativeDrawInvocationRecordedEvent>()
            }));
        // NativeDrawInvocation is an optional consumer-driven fact. This minimal registry
        // has no tracking consumer, so actual bindings and physical moves prove the draw.
        Need(s.Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 2 &&
            s.Game.CardMovements.All(move => move.From != CardLocation.OutsideGame),
            "Each GameStarting binding completes once; the normal two-card phase remains separate and no random hand entity is fabricated outside the physical deck.");

        var firstTurn = s.Game.State.TurnNumber;
        s.NextOwnPlay();
        Need(s.Game.State.TurnNumber > firstTurn && s.PreparationMoves().SequenceEqual(opening) &&
            s.PreparationBindings().SequenceEqual(bindings) &&
            s.Events<ProgramBindingStartedEvent>().Where(binding => Tactics.Contains(binding.SkillId)).SequenceEqual(starts) &&
            s.Game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 4 &&
            s.View(0).HandCount == 12,
            "Crossing an actual full turn cycle adds only the next normal draw; none of the opening source grants fires again.");

        var unowned = Scene.Create([]);
        unowned.Start();
        Need(unowned.PreparationMoves().Length == 0 && unowned.PreparationBindings().Length == 0 &&
            unowned.Events<ProgramBindingStartedEvent>().All(binding => !Tactics.Contains(binding.SkillId)) &&
            unowned.View(0).HandCount == 4 && Enumerable.Range(1, 3).All(seat => unowned.View(seat).HandCount == 2),
            "Registering the sources without an actual holder grants no opening cards or startup binding to any player.");
    }

    private static void Need(bool value, string why)
    { if (!value) throw new InvalidOperationException(why); }
    private static string Peer(int seat) => "fixture:preparation-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] PreparationMoves() => Game.CardMovements.Where(move =>
            Tactics.Any(skill => move.Reason.Value == "skill-program." + skill + ".Draw")).ToArray();
        public ProgramBindingResolvedEvent[] PreparationBindings() => Events<ProgramBindingResolvedEvent>()
            .Where(binding => Tactics.Contains(binding.SkillId)).ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real preparation tactic command."); }
        public void Start() { Accept(new StartGameCommand()); Play(); }
        public void NextOwnPlay()
        { Accept(new EndPlayPhaseCommand(0, Game.Revision, Game.PendingDecision!.PromptId)); Play(); }
        private void Play()
        {
            for (var step = 0; step < 160; step++)
            {
                if (Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                if (Game.PendingDecision is { PlayerSeat: 0 } prompt)
                    throw new InvalidOperationException("Unexpected preparation fixture prompt: " + JsonSerializer.Serialize(prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded preparation fixture play boundary.");
        }
        public static Scene Create(string[] tactics)
        {
            var setup = new ShanheBattleSetup(83, 1, "fixture:preparation-tactics", Owner, 20, 0, 0, 0, 0,
                [], [], [], [new(Peer(1)), new(Peer(2)), new(Peer(3))], [], new string('c', 64))
                { TacticSkillIds = tactics };
            return new(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 8
            }, Registry.Value));
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-preparation-tactics", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddSkill(new(Driver, "开局筹备角色", "无额外触发行为。"));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
                  "{{{Quiet}}}":{"name":"固定守军","description":"摸牌后跳过出牌阶段。"}
                }}
                """);
            builder.AddSkill(new(Quiet, "固定守军", "摸牌后跳过出牌阶段。")
                { Program = catalog.Programs[Quiet], ProgramPresentation = catalog.Presentations[Quiet] });
            builder.AddGeneral(new(Owner, "筹备战法持有者", "supporter", Driver, "qun", 20));
            foreach (var seat in Enumerable.Range(1, 3))
                builder.AddGeneral(new(Peer(seat), "固定守军", "supporter", Quiet, "wei", 20));
            builder.AddDeck(new(Deck, "可追踪的随机实体闪", 2, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 40)
                    .Select(index => new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "筹备真实开局", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
