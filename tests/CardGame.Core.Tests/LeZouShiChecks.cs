using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class LeZouShiChecks
{
    private const string Owner = "fixture:le-zou-shi-owner", Driver = "fixture:le-zou-shi-driver";
    private const string Probe = "fixture:le-zou-shi-probe", Watch = "fixture:le-zou-shi-watch";
    private const string Mode = "identity:classic-le-zou-shi-marks", Obtain = "skill-program.le-zou-shi.huoxin.obtain";

    public static void PermanentMarksPreserveHuoxinAdmissionAndMovementCold()
    {
        var s = Scene.Start();
        var original = s.Events<CardEntityMarkedEvent>().Where(e => e.Mark == "zheng" && e.InitialOwnerSeat == 0).Select(e => e.CardId).ToArray();
        s.Need(original.Length == 4 && original.All(id => !s.DiscardEligible(0).Contains(id)),
            "The real opening program marks its four physical entities and exempts them only for the holder with Yunzheng.");
        s.Use("draw-stock"); s.Play();
        var shown = s.View(1).Hand.First();
        s.Need(!s.Marked(shown.Id) && s.Effective(1, Probe), "The other holder begins with an unmarked entity and an effective ordinary skill.");

        s.StartHuoxin(shown.Id, shown.Suit);
        s.Play();
        s.Need(s.Marked(shown.Id) && !s.Effective(1, Probe) &&
            !s.Game.CardMovements.Any(m => m.CardId == shown.Id && m.From == CardLocation.Hand(1) && m.To == CardLocation.Hand(0)),
            "An unmarked different-suit entity is revealed and marked, without becoming obtainable because of that new mark.");

        s.StartHuoxin(shown.Id, shown.Suit);
        s.Need(s.Pending!.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "obtain"),
            "The same already-marked entity is obtainable on another different-suit real Use.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "obtain");
        s.Reach(() => s.Optional(Watch));
        s.Need(s.Game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any() &&
            s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == "le:huoxin") && s.ObtainCount(shown.Id) == 1,
            "The actual obtain is paid before its native movement child returns to the original Huoxin frame.");
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        s.Need(s.ObtainCount(shown.Id) == 1 && s.Events<CardEntityMarkedEvent>().Count(e => e.Mark == "zheng" && e.CardId == shown.Id) == 1,
            "Cold continuation completes the original movement once and does not mark or obtain the entity twice.");

        var given = original.First(id => s.View(0).Hand.Any(card => card.Id == id));
        s.Use("give", [given], [1]); s.Play();
        s.Need(s.DiscardEligible(1).Contains(given) && !s.DiscardEligible(0).Contains(shown.Id),
            "A marked card received by another holder does not carry Yunzheng's personal hand-limit benefit with it.");
        s.Use("lose-yunzheng"); s.Play(); s.Cold();
        s.Need(!s.Effective(0, "le:yunzheng") && !s.Effective(1, Probe) && s.Marked(given) &&
            s.DiscardEligible(1).Contains(given) && s.DiscardEligible(0).Contains(shown.Id),
            "Real source loss retains the other marked holder's applied suppression, while the former source loses its own exemption.");
        s.Use("grant-yunzheng"); s.Play(); s.Cold();
        s.Need(s.Effective(0, "le:yunzheng") && !s.DiscardEligible(0).Contains(shown.Id) && s.DiscardEligible(1).Contains(given),
            "A real independent grant restores only the current holder's skill-dependent hand-limit exemption.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine Game { get; private set; } = game;
        public PendingDecision? Pending => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Game)!)[seat];
        public bool Effective(int seat, string skill) => Game.NationalSkillsEntries.HasRuntimeSkill(Player(seat), skill);
        public int[] DiscardEligible(int seat) => Game.DeferredCardsEntries.GetDiscardEligibleHand(Player(seat)).Select(card => card.Id).ToArray();
        public bool Marked(int id) => Events<CardEntityMarkedEvent>().Any(e => e.Mark == "zheng" && e.CardId == id);
        public int ObtainCount(int id) => Game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(1) && m.To == CardLocation.Hand(0) && m.Reason.Value == Obtain);
        public bool Optional(string skill) => Pending?.SkillPrompt?.SkillId == skill &&
            Pending.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Use(string activation, int[]? cards = null, int[]? targets = null) =>
            Accept(new UseProgramSkillCommand(0, Driver, activation, cards ?? [], targets ?? [], Game.Revision, Pending!.PromptId));
        public void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = Pending ?? throw new InvalidOperationException("Missing real owner prompt.");
            var choice = prompt.Choices.First(select);
            Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
        }
        public void StartHuoxin(int cardId, Suit shownSuit)
        {
            var unmarked = View(0).Hand.Where(card => !Marked(card.Id) && card.Suit != shownSuit).Select(card => card.Id).ToHashSet();
            var use = Game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.IronChain &&
                action.CardId is { } id && unmarked.Contains(id) && action.ConversionSource is null && action.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, use.CardId!.Value, use.TargetSeats, Game.Revision, Pending!.PromptId,
                use.PlayedCardKind, use.TargetCardId));
            Reach(() => Optional("le:huoxin"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => Pending?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "target") == true);
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([1]));
            var slot = Array.FindIndex(View(1).Hand.ToArray(), card => card.Id == cardId);
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Parameters.GetValueOrDefault("slot") == slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public void Play() => Reach(() => Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 150; step++)
            {
                if (done()) return;
                if (Pending is not { PlayerSeat: 0 } prompt) Accept(new AdvanceOneStepCommand(Game.Revision));
                else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected owner boundary: " + JsonSerializer.Serialize(prompt));
            }
            throw new InvalidOperationException("The requested native boundary did not arrive.");
        }
        public void Accept(GameCommand command)
        {
            var result = Game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(result.Error));
        }
        private string State() => JsonSerializer.Serialize(new
        {
            View = SnapshotJson.Serialize(Game.CreateSnapshot(0)), Game.CardMovements,
            Frames = Game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType()))
        });
        public void Cold()
        {
            var before = State();
            Game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(Game.CreateCheckpoint())), registry);
            Need(State() == before, "Cold replay changes the current original-frame/card movement state.");
        }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry), registry);
            scene.Accept(new StartGameCommand()); scene.Reach(() => scene.Pending is { Kind: DecisionKind.SelectGeneral });
            scene.Accept(new SelectGeneralCommand(0, Owner, scene.Game.Revision, scene.Pending!.PromptId)); scene.Play();
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-le-zou-shi-marks", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "le:zou-shi");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"draw-stock","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":12},{"op":"draw","target":"owner","amount":12}]},
                    {"id":"give","minCards":1,"maxCards":1,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"giveSelected","target":"selectedTarget"}]},
                    {"id":"lose-yunzheng","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["le:yunzheng"]},"until":[]}]},
                    {"id":"grant-yunzheng","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["le:yunzheng"]}]}]},
                  {"id":"{{Probe}}","revision":1,"modifiers":[{"id":"probe","query":"handLimit","operation":"add","value":1,"priority":0}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"obtain-child","window":"cardsGained","subject":"owner","optional":true,"movementOccurrence":"perBatch","destinationZones":["hand"],"movementReasons":["{{Obtain}}"],"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "Driver", description = "Real fixture commands" },
                        [Probe] = new { name = "Probe", description = "Ordinary nonlocked rule" }, [Watch] = new { name = "Watch", description = "Pause the real obtain child" } } }));
            foreach (var id in new[] { Driver, Probe, Watch })
                builder.AddSkill(new(id, id, "Shared mark flow") { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id],
                    ExecutionForms = SkillExecutionForm.State | SkillExecutionForm.Trigger, ActionForms = id == Driver ? SkillActionForm.Active : SkillActionForm.None });
            builder.AddGeneral(new(Owner, "Zou Shi mark flow", "supporter", "le:yunzheng", "qun", 8, ["le:huoxin", Driver, Watch]));
            var peers = Enumerable.Range(1, 3).Select(seat => "fixture:le-zou-shi-other-" + seat).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "Other holder", "supporter", Probe, "shu", 8));
            const string cardId = "fixture:le-zou-shi-chain";
            var card = CardCatalog.Get(CardKind.IronChain);
            builder.AddCard(new(cardId, card.DisplayName, card.CategoryName, "Physical native chain", CardKind.IronChain));
            builder.AddDeck(new("fixture:le-zou-shi-deck", "Fixed two-suit deck", 4, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(index => new ContentDeckPhysicalCard(cardId, index < 20 ? Suit.Spade : Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "Shared permanent marks", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                "fixture:le-zou-shi-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
