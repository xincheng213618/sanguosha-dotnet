using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class HulaoRuleChecks
{
    private const string Driver = "fixture:hulao-rule-driver", Mode = "fixture:hulao-rule-mode";

    public static void AozhanPreventsWholeLargeDamageAndUnitedSuppliesSurvivors()
    {
        {
            var (g, registry) = Start("hulao:aozhan");
            Use(g, "damage", 0); Play(g);
            Require(V(g, 0).Hp == 2, "Without physical armor Aozhan does not prevent two damage.");
            Use(g, "recover", 0); Play(g);
            Use(g, "equip", 0); Play(g);
            Require(V(g, 0).Equipment.Single().Kind == CardKind.RenwangShield, "The armor is an actual native equipment use.");
            Use(g, "damage", 0); Play(g);
            Require(V(g, 0).Hp == 4 && E<ProgramDamagePreventedEvent>(g).Count(e => e.SkillId == "hulao:aozhan") == 1,
                "Aozhan prevents the entire damage instance above one, rather than capping it to one.");
            Use(g, "one-damage", 0); Play(g);
            Require(V(g, 0).Hp == 3, "One damage still applies while actual armor is equipped.");
            Cold(g, registry);
        }
        {
            var (g, registry) = Start("hulao:united");
            var ally = g.CreateSnapshot(0).Players.Single(p => p.Seat != 0 && p.TeamId == V(g, 0).TeamId).Seat;
            Use(g, "damage", 0); Play(g);
            var hand = V(g, 0).HandCount;
            Use(g, "kill", ally); Play(g);
            Require(!V(g, ally).IsAlive && V(g, 0).Hp == 3 && V(g, 0).HandCount == hand + 2,
                "An actual public ally's death heals the survivor one HP then draws two physical cards.");
            Require(E<RecoveryAppliedEvent>(g).Count(e => e.SourceSeat == 0 && e.TargetSeat == 0) == 1,
                "Public enemy observers do not issue another copy of the allied supply.");
            Cold(g, registry);
        }
        {
            var (g, registry) = Start("hulao:shenwei");
            Require(V(g, 0).HandCount == 7 && V(g, 0).HandLimit == 6 &&
                g.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 4,
                $"Two living public enemies add two actual normal-draw cards and two hand limit (hand={V(g, 0).HandCount}, limit={V(g, 0).HandLimit}).");
            var enemy = g.CreateSnapshot(0).Players.First(p => p.TeamId != V(g, 0).TeamId).Seat;
            Use(g, "kill", enemy); Play(g);
            Require(V(g, 0).HandLimit == 5, "Shenwei updates immediately from the surviving enemy count.");
            Cold(g, registry);
        }
    }

    public static void XiuluoPaysNativeOwnedDiscardBeforeRemovingRealJudgment()
    {
        foreach (var stop in new[] { false, true })
        {
            var (g, registry) = Start("hulao:xiuluo", "standard:lightning");
            var action = g.GetHumanLegalActions().First(a => a.CardId is { } id && V(g, 0).Hand.Any(c => c.Id == id && c.Kind == CardKind.Lightning));
            var delayed = action.CardId!.Value;
            Accept(g, new PlayCardCommand(0, delayed, action.TargetSeats, g.Revision, P(g)!.PromptId, action.PlayedCardKind, action.TargetCardId)); Play(g);
            Require(V(g, 0).Judgment.Single().Id == delayed, "The actual Lightning enters the owner's judgment area before a later preparation.");
            Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
            Until(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "hulao-xiuluo") == true);
            Cold(g, registry);
            if (stop)
            {
                Answer(g, c => c.Cards.Count == 0);
                Until(g, () => !g.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.HulaoXiuluo is not null));
                Require(!E<HulaoXiuluoPaidEvent>(g).Any(), "Declining the optional loop never pays an owned card.");
            }
            else
            {
                var cost = P(g)!.Choices.First(c => c.Cards.Count == 1).Cards.Single();
                Answer(g, c => c.Cards.SequenceEqual([cost]));
                Until(g, () => P(g)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "hulao-xiuluo-judgment") == true);
                Require(V(g, 0).Judgment.Single().Id == delayed && E<HulaoXiuluoPaidEvent>(g).Count() == 1,
                    "Payment and its children complete before selecting the current matching judgment entity.");
                Cold(g, registry);
                Answer(g, c => c.Cards.SequenceEqual([delayed])); Play(g);
                var paid = E<HulaoXiuluoPaidEvent>(g).Single();
                Require(paid is { OwnerSeat: 0, Iteration: 1 } && paid.CardId == cost &&
                    E<HulaoXiuluoJudgmentDiscardedEvent>(g).Single().CardId == delayed && V(g, 0).Judgment.Count == 0,
                    "The paid same-suit real hand card removes its real judgment after the payment subtree.");
                var movements = g.CardMovements.Where(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile &&
                    m.Reason.Value == "program.hulao.xiuluo.cost").ToArray();
                Require(movements.Length == 1 && g.CardMovements.Any(m => m.CardId == delayed && m.From == CardLocation.Judgment(0) &&
                    m.To == CardLocation.DiscardPile && m.Sequence > movements[0].Sequence),
                    "Native payment and judgment discards retain separate, ordered physical movements without repaying on resume.");
            }
            Cold(g, registry);
        }
    }

    private static (GameEngine, ContentRegistry) Start(string skill, string card = "standard:dodge")
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardHulaoActivityPackage(), new Fixture(skill, card));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = "team:blue", ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(g, new StartGameCommand()); Until(g, () => P(g)?.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, "fixture:hulao-rule-actor", g.Revision, P(g)!.PromptId)); Play(g);
        return (g, registry);
    }
    private static void Use(GameEngine g, string id, int target) => Accept(g, new UseProgramSkillCommand(0, Driver, id, [], [target], g.Revision, P(g)!.PromptId));
    private static PlayerSnapshot V(GameEngine g, int seat) => g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(seat => g.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>();
    private static void Answer(GameEngine g, Func<PromptChoice, bool> predicate)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, g.Revision)); }
    private static void Play(GameEngine g) => Until(g, () => P(g) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Until(GameEngine g, Func<bool> condition)
    {
        for (var i = 0; i < 180; i++)
        {
            if (condition()) return;
            var p = P(g);
            if (p?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate") == true)
                Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            else if (p?.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass") == true)
                Answer(g, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Hulao rule fixture did not reach its actual boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(g.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(restored.CardMovements), "Actual choices, private cards and physical movements restore from accepted commands.");
    }
    private static void Accept(GameEngine g, GameCommand command)
    { var result = g.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Hulao rule command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string skill, string card) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("hulao-rule-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                {"id":"one-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"recover","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTarget","amount":2}]},
                {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipment"}]}]},
              {"id":"fixture:hulao-rule-quiet","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, "fixture:hulao-rule-quiet" }.ToDictionary(id => id, id => new { name = id, description = "Small actual Hulao rule fixture." }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, "Small actual Hulao rule fixture.") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new("fixture:hulao-rule-actor", "Hulao rule actor", "", Driver, AdditionalSkillIds: [skill]));
            for (var i = 1; i < 4; i++) b.AddGeneral(new($"fixture:hulao-rule-peer-{i}", $"Hulao rule peer {i}", "", "fixture:hulao-rule-quiet",
                AdditionalSkillIds: skill == "hulao:united" ? [skill] : []));
            b.AddDeck(new("fixture:hulao-rule-deck", "Real uniform cards and native armor", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 140).Select(i => new ContentDeckPhysicalCard(i < 120 ? card : "standard:renwang_shield", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "Actual Hulao rule fixture", 4, 4, new Dictionary<string, int>(), "fixture:hulao-rule-deck", 4,
                ["fixture:hulao-rule-actor", "fixture:hulao-rule-peer-1", "fixture:hulao-rule-peer-2", "fixture:hulao-rule-peer-3"], ContentModeKind.Team,
                new Dictionary<string, int> { ["team:blue"] = 2, ["team:red"] = 2 }));
        }
    }
}
