using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileQuYiChecks
{
    private const string Driver = "fixture:mobile-fuqi-driver", Shift = "fixture:mobile-fuqi-distance";
    private const string Deck = "fixture:mobile-fuqi-deck", Mode = "identity:mobile-fuqi";

    public static void IncomingDistanceAndNonTargetCounterspellKeepNativeResponsesCold()
    {
        var s = Scene.Start(incomingOnOwner: false);
        Need(s.G.GetCombatDistance(0, 1) == 2 && s.G.GetCombatDistance(1, 0) == 1,
            "The native distance layers must make the two directions different.");
        s.Begin(CardKind.Slash, 1); s.Finish(forbiddenDodgeSeat: 1);
        Need(s.Events<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1),
            "Incoming distance one must prohibit the real target's Dodge despite outgoing distance two.");
        s.Cold();
        s.Begin(CardKind.DrawTwo);
        s.Until(() => s.Pending?.Kind == DecisionKind.Nullification);
        var use = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single().ParentFrameId;
        s.Cold(); s.Finish();
        var offered = s.Events<NullificationRequestedEvent>().Where(e => e.ResolutionId == use).ToArray();
        Need(offered.All(e => e.ResponderSeat != 1) && offered.Any(e => e.ResponderSeat == 2),
            "A nearby non-target counterspell holder must be excluded while a distant holder remains eligible.");
        Need(s.Events<CardUseFinishedEvent>().Any(e => e.ResolutionId == use),
            "The original ordinary trick must finish through its real counterspell window.");

        s = Scene.Start(incomingOnOwner: true);
        Need(s.G.GetCombatDistance(0, 1) == 1 && s.G.GetCombatDistance(1, 0) == 2,
            "The opposite witness must retain outgoing one but incoming two.");
        s.Begin(CardKind.Slash, 1);
        s.Until(() => s.Pending is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge });
        Need(s.Pending!.Choices.Any(c => c.Cards.Count == 1),
            "Outgoing distance one alone must not remove the ordinary native Dodge choice.");
        s.Cold(); s.Answer(c => c.Cards.Count == 1); s.Finish(); s.Cold();
        Need(!s.Events<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 1),
            "The paid real Dodge must return to the same Slash and prevent its damage.");
    }

    public static void LoaderPreservesDefaultsAndRejectsMisplacedResponseQualifiers()
    {
        var old = LoadPolicy("prohibitNearbyTargetResponse", qualifiers: false);
        var current = LoadPolicy("prohibitNearbyTargetResponse", qualifiers: true);
        Need(old.UseIncomingResponseDistance is null && old.IncludeNonTargetResponders is null &&
            !JsonSerializer.Serialize(old).Contains(nameof(SkillProgramCardPolicy.UseIncomingResponseDistance), StringComparison.Ordinal) &&
            current is { UseIncomingResponseDistance: true, IncludeNonTargetResponders: true },
            "Omitted qualifiers must preserve the prior policy shape and explicit qualifiers must be immutable definition data.");
        try { _ = LoadPolicy("prohibitTarget", qualifiers: true); }
        catch (InvalidOperationException e) when (e.Message.Contains("qualifiers require prohibitNearbyTargetResponse", StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException("Response qualifiers on another policy kind must be rejected by the loader.");
    }

    private static SkillProgramCardPolicy LoadPolicy(string kind, bool qualifiers)
    {
        var policy = new Dictionary<string, object> { ["id"] = "response", ["kind"] = kind, ["value"] = 1 };
        if (qualifiers) { policy["useIncomingResponseDistance"] = true; policy["includeNonTargetResponders"] = true; }
        var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id = "fixture:fuqi-policy", revision = 1, cardPolicies = new[] { policy } } } });
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:fuqi-policy"] = new { name = "Response", description = "Response" } } });
        return SkillProgramCatalog.Load(rules, presentation).Programs["fixture:fuqi-policy"].CardPolicies.Single();
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? Pending => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat, true).PendingDecision).FirstOrDefault(p => p is not null);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "The mobile Fuqi command was rejected.");
        }
        public void Answer(Func<PromptChoice, bool> pick)
        { var p = Pending!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Begin(CardKind kind, int? target = null)
        {
            var cards = G.CreateSnapshot(0, true).Players[0].Hand.Where(c => c.Kind == kind).Select(c => c.Id).ToHashSet();
            var action = G.GetLegalActions(0).First(a => a.CardId is { } id && cards.Contains(id) &&
                a.ConversionSource is null && (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, Pending!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
        }
        public void Until(Func<bool> done, int? forbiddenDodgeSeat = null)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                if (Pending is { Kind: DecisionKind.RespondDodge } p)
                { Need(p.PlayerSeat != forbiddenDodgeSeat, "Fuqi's qualified target must not receive a native Dodge prompt."); Answer(c => c.Cards.Count == 0); }
                else if (Pending is { Kind: DecisionKind.Nullification }) Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The mobile Fuqi fixture did not reach its native boundary: " + Pending?.Kind);
        }
        public void Finish(int? forbiddenDodgeSeat = null) => Until(() => G.ResolutionStack.Count == 0 && Pending is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, forbiddenDodgeSeat);
        public void Cold()
        {
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat, true)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat, true))) &&
                G.CardMovements.SequenceEqual(restored.CardMovements), "Cold restoration must preserve every viewer and the real card movements.");
            G = restored;
        }
        public static Scene Start(bool incomingOnOwner)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(incomingOnOwner));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                PlayerCount = 4, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand());
            for (var step = 0; step < 160; step++)
            {
                if (s.Pending is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return s;
                if (s.Pending is { Kind: DecisionKind.SelectGeneral } p)
                    s.Accept(new SelectGeneralCommand(p.PlayerSeat, "fixture:mobile-fuqi-seat-" + p.PlayerSeat, g.Revision, p.PromptId));
                else s.Accept(new AdvanceOneStepCommand(g.Revision));
            }
            throw new InvalidOperationException("The fixed mobile Fuqi setup did not reach Play.");
        }
    }

    private sealed class Fixture(bool incomingOnOwner) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-fuqi", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Driver, revision = 1, modifiers = new[] { new { id = "range", query = "attackRange", operation = "add", value = 3, priority = 0 } } },
                new { id = Shift, revision = 1, modifiers = new[] { new { id = "incoming", query = "incomingDistance", operation = "add", value = 1, priority = 0 } } } } });
            var text = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills =
                new Dictionary<string, object> { [Driver] = new { name = "Range", description = "Range" }, [Shift] = new { name = "Incoming", description = "Incoming" } } });
            var catalog = SkillProgramCatalog.Load(rules, text);
            foreach (var id in new[] { Driver, Shift }) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new("fixture:mobile-fuqi-seat-" + seat, "Fuqi witness", "supporter",
                seat == 0 ? "mobile:fuqi" : "standard:none", "wei", BaseHp: 12,
                AdditionalSkillIds: seat == 0 ? incomingOnOwner ? [Driver, Shift] : [Driver] : seat == 1 && !incomingOnOwner ? [Shift] : []));
            var kinds = new[] { "standard:slash", "standard:dodge", "standard:nullification", "standard:draw_two" };
            b.AddDeck(new(Deck, "Native Fuqi cards", 20, 0, []) { PhysicalCards = Enumerable.Range(0, 160)
                .Select(i => new ContentDeckPhysicalCard(kinds[i % kinds.Length], Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "Mobile Fuqi", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: Enumerable.Range(0, 4).Select(seat => "fixture:mobile-fuqi-seat-" + seat).ToArray()));
        }
    }
}
