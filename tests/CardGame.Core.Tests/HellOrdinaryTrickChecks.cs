using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class HellOrdinaryTrickChecks
{
    private const string Mode = "fixture:hell-ordinary", Actor = "fixture:hell-ordinary-actor", Driver = "fixture:hell-ordinary-driver", Quiet = "fixture:hell-ordinary-quiet";
    private const string Tiered = "boundary:jiaozhao-round-current", State = "boundary-jiaozhao-round-tier";

    public static void QicePermissionFilterKeepsOtherNamesAndUnpaidMaterials()
    {
        var (game, registry) = Start("classic:qice");
        var sacrifice = game.CreateSnapshot(0).Players.Single(p => p.GeneralId == "fixture:hell-ordinary-sacrifice").Seat;
        Accept(game, new UseProgramSkillCommand(0, Driver, "kill", [], [sacrifice], game.Revision, P(game)!.PromptId)); Play(game);
        Require(E<HellWanghunGrantLostEvent>(game).Any(e => e.TargetSeat == 3), "The permission regression has a genuine living enemy with Wanghun-lost grants, so ReturnSoul would otherwise have a real target.");
        var materials = V(game).Hand.Select(c => c.Id).ToArray(); var before = game.CardMovements.Count;
        Accept(game, new UseProgramSkillCommand(0, Driver, "ban-soul", [], [], game.Revision, P(game)!.PromptId)); Play(game);
        Require(game.GetHumanLegalActions().Any(a => a.ProgramSkillId == "classic:qice" && a.ProgramActivationId == "all-hand-as-ordinary-trick") &&
            V(game).Hand.Select(c => c.Id).SequenceEqual(materials) && game.CardMovements.Count == before &&
            !E<ProgramSkillStartedEvent>(game).Any(e => e.SkillId == "classic:qice") && !E<ProgramViewAsConvertedEvent>(game).Any(e => e.SkillId == "classic:qice"),
            "The real ReturnSoul Use prohibition leaves other ordinary tricks available and spends neither the all-hand material nor Qice's phase-once activation quota while publishing legal actions.");
        Cold(game, registry);
        Accept(game, new UseProgramSkillCommand(0, "classic:qice", "all-hand-as-ordinary-trick", materials, [], game.Revision, P(game)!.PromptId));
        Until(game, () => P(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "use-all-hand-as-ordinary-trick") == true);
        Require(P(game)!.Choices.Any(c => c.Parameters.ContainsKey("card-kind")) &&
            P(game)!.Choices.All(c => c.Parameters.GetValueOrDefault("card-kind") != nameof(CardKind.ReturnSoul)) &&
            V(game).Hand.Select(c => c.Id).SequenceEqual(materials) && game.CardMovements.Count == before &&
            !E<ProgramViewAsConvertedEvent>(game).Any(e => e.SkillId == "classic:qice"),
            "The true all-hand producer's native name menu obeys the same Use prohibition: no ReturnSoul option, other legal names retained, and no material paid before a legal name is chosen.");
        Cold(game, registry);
    }

    private static (GameEngine, ContentRegistry) Start(string skill)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(),
            new StandardClassicGeneralPackage(), new StandardHellActivityPackage(), new Fixture(skill));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 5, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:allies", ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(game, new StartGameCommand()); Until(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(0, Actor, game.Revision, P(game)!.PromptId)); Play(game); return (game, registry);
    }

    private static PlayerSnapshot V(GameEngine game) => game.CreateSnapshot(0).Players[0];
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 5).Select(s => game.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> E<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static void Play(GameEngine game) => Until(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Answer(GameEngine game, Func<PromptChoice, bool> select) { var prompt = P(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(select).Id, game.Revision)); }
    private static void Until(GameEngine game, Func<bool> condition)
    {
        for (var i = 0; i < 220; i++)
        {
            if (condition()) return;
            var p = P(game);
            if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")) Answer(game, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            else if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass")) Answer(game, c => c.Parameters.GetValueOrDefault("response") == "pass");
            else if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "let-die")) Answer(game, c => c.Parameters.GetValueOrDefault("response") == "let-die");
            else if (p is { PlayerSeat: 0, Kind: not DecisionKind.PlayCard } && p.Choices.Count > 0) Answer(game, _ => true);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Hell ordinary-trick fixture missed its native boundary: " + JsonSerializer.Serialize(P(game)));
    }
    private static void Cold(GameEngine game, ContentRegistry registry)
    {
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 5).All(s => SnapshotJson.Serialize(game.CreateSnapshot(s)) == SnapshotJson.Serialize(copy.CreateSnapshot(s))) &&
            JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(copy.CardMovements), "Derived names, true skill restoration and paid native materials cold-replay from their real accepted commands.");
    }
    private static void Accept(GameEngine game, GameCommand command) { var result = game.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Hell ordinary-trick command rejected."); }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(string skill) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("hell-ordinary-fixture", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"viewAs":[{"id":"fixture-tier-ref","inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false,"conversionStateId":"{{{State}}}"}],"activations":[
                {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]},
                {"id":"upgrade","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"upgradeConversionTier","target":"owner","stateId":"{{{State}}}"}]},
                {"id":"ban-soul","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantTurnCardActionProhibition","target":"owner","cardKinds":["returnSoul"],"actionTypes":["use"]}]},
                {"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}]},
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"skip-play","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, id => new { name = id, description = "Small native Hell ordinary-trick fixture." }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, "Small native Hell ordinary-trick fixture.") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            string[] players = [Actor, "fixture:hell-ordinary-sacrifice", "fixture:hell-ordinary-ally"];
            b.AddGeneral(new(Actor, Actor, "", Driver, "wei", 4, [skill, "classic:jizhi"]));
            b.AddGeneral(new(players[1], players[1], "", Quiet, "wei", 4, ["hell:wanghun"]));
            b.AddGeneral(new(players[2], players[2], "", Quiet, "wei", 4));
            const string boss = "fixture:hell-ordinary-boss", hidden = "fixture:hell-ordinary-hidden", auxiliary = "fixture:hell-ordinary-auxiliary", deck = "fixture:hell-ordinary-deck";
            foreach (var id in new[] { boss, hidden, auxiliary }) b.AddGeneral(new(id, id, "", Quiet, "qun", 12));
            b.AddDeck(new(deck, "Real one-color dodge materials", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 180).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, Mode, 5, 5, new Dictionary<string, int>(), deck, 3, players.Concat([boss, hidden, auxiliary]).ToArray(), ContentModeKind.Team,
                new Dictionary<string, int> { ["team:allies"] = 3, ["team:boss"] = 2 })
            { BossActivity = new(players, [new("one", [boss]), new("two", [boss]), new("three", [boss]), new("four", [boss, hidden])], 5, []), HellActivity = new(1, auxiliary, hidden) });
        }
    }
}
