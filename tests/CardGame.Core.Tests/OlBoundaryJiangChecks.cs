using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlBoundaryJiangChecks
{
    private const string Jiang = "boundary:jiang-current";
    private const string Claim = "reclaim-first-discard-batch";
    private const string Driver = "fixture:ol-jiang-driver", Mode = "identity:ol-jiang-test", Deck = "fixture:ol-jiang-deck";
    private const string DiscardReason = "skill-program.fixture:ol-jiang-driver.DiscardSelected";
    private const string ClaimReason = "skill-program.boundary:jiang-current.ClaimMovedCards";
    private static readonly string[] Generals = ["fixture:ol-jiang-owner", "fixture:ol-jiang-peer-1",
        "fixture:ol-jiang-peer-2", "fixture:ol-jiang-peer-3"];

    public static void FirstGenuineBatchOffersOnceAndClaimsItsQualifyingEntries()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The fixture owner must start in Play.");
        var ordinaryUse = FindCard(game, 0, IsRedSlash);
        Submit(game, new PlayCardCommand(0, ordinaryUse, [1], game.Revision, play.PromptId, CardKind.Slash));
        var draw = Next(game);
        Require(draw is { PlayerSeat: 0, SkillPrompt.SkillId: Jiang } &&
            draw.Choices.Any(choice => choice.Parameters.GetValueOrDefault("binding-id") == "draw-on-using-red-slash"),
            "The normal red Slash still has its own Jiang draw trigger.");
        AnswerAction(game, draw, "skip");
        if (Next(game) is { Kind: DecisionKind.RespondDodge } dodge)
            Answer(game, dodge, choice => choice.Cards.Count == 0);
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Zone(game, ordinaryUse) == CardLocation.DiscardPile,
            "The used Slash must retire normally, without a claim prompt.");

        var ownDuel = FindCard(game, 0, card => card.Kind == CardKind.Duel);
        Discard(game, 0, ownDuel);
        var first = Next(game);
        Require(IsClaim(first),
            $"The first real discard must offer Jiang, including the owner's own discard. " +
            $"Prompt: {first.Kind} for seat {first.PlayerSeat}. Movement: {game.CardMovements.Last(move => move.CardId == ownDuel)}.");
        game = Cold(game, content);
        var initialHp = game.CreateSnapshot(0).Players[0].Hp;
        AnswerAction(game, Next(game), "skip");
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Zone(game, ownDuel) == CardLocation.DiscardPile,
            "Declining a first discard must leave it in the pile.");

        var laterSlash = FindCard(game, 0, IsRedSlash);
        Discard(game, 0, laterSlash);
        play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CreateSnapshot(0).Players[0].Hp == initialHp && Zone(game, laterSlash) == CardLocation.DiscardPile,
            "A later real discard of the other card kind must not offer Jiang in the same turn.");
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));

        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 }, "The next actual turn must reach Play.");
        var duel = FindCard(game, 1, card => card.Kind == CardKind.Duel);
        var redSlash = FindCard(game, 1, IsRedSlash);
        var blackSlash = FindCard(game, 1, card => card.Kind == CardKind.Slash && card.Suit is Suit.Spade or Suit.Club);
        var selected = new[] { blackSlash, duel, redSlash };
        Discard(game, 1, selected);
        var originals = game.CardMovements.Where(move => selected.Contains(move.CardId) &&
            move.From == CardLocation.Hand(1) && move.To == CardLocation.DiscardPile && move.Reason.Value == DiscardReason).ToArray();
        Require(originals.Length == 3 && originals[0].BatchId > 0 && originals.Select(move => move.BatchId).Distinct().Count() == 1,
            "The three named physical cards must actually be discarded simultaneously.");
        var offer = Next(game);
        Require(IsClaim(offer), "The first real discard batch in the next turn must offer Jiang.");
        var declinedBatch = Cold(game, content);
        AnswerAction(declinedBatch, Next(declinedBatch), "skip");
        Require(Next(declinedBatch) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 } &&
            selected.All(card => Zone(declinedBatch, card) == CardLocation.DiscardPile) &&
            declinedBatch.CreateSnapshot(0).Players[0].Hp == initialHp,
            "A declined mixed batch must not offer again for its other qualifying card.");

        AnswerAction(game, offer, "activate");
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 },
            "The mixed batch must offer one activation and then resume the original turn.");
        var claims = game.CardMovements.Where(move => move.Reason.Value == ClaimReason).ToArray();
        Require(claims.Select(move => move.CardId).ToHashSet().SetEquals([duel, redSlash]) && claims.Length == 2 &&
            claims[0].BatchId > 0 && claims.Select(move => move.BatchId).Distinct().Count() == 1 &&
            claims.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)) &&
            Zone(game, blackSlash) == CardLocation.DiscardPile && game.CreateSnapshot(0).Players[0].Hp == initialHp - 1,
            "One point of HP buys both qualifying entries in one gain batch, leaving the black Slash in the pile.");
    }

    private static bool IsRedSlash(CardSnapshot card) => card.Kind == CardKind.Slash && card.Suit is Suit.Heart or Suit.Diamond;

    private static int FindCard(GameEngine game, int seat, Func<CardSnapshot, bool> match) =>
        game.CreateSnapshot(seat).Players[seat].Hand.First(match).Id;

    private static CardLocation Zone(GameEngine game, int card) =>
        game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location;

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Replay must restore the original batch choice for every seat.");
        return restored;
    }

    private static void Discard(GameEngine game, int seat, params int[] cards)
    {
        var play = Next(game);
        Require(play.Kind == DecisionKind.PlayCard && play.PlayerSeat == seat, "A discard driver requires its actual Play prompt.");
        Submit(game, new UseProgramSkillCommand(seat, Driver, cards.Length == 1 ? "one" : "three", cards, [],
            game.Revision, play.PromptId));
    }

    private static bool IsClaim(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jiang } &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("binding-id") == Claim);

    private static void AnswerAction(GameEngine game, PendingDecision prompt, string action) =>
        Answer(game, prompt, choice => choice.Parameters.GetValueOrDefault("program-action") == action);

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> match) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 150; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }

        throw new InvalidOperationException("Jiang did not reach a public command boundary.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "Jiang command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-boundary-jiang", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:sun-ce", "classic:sun-jian", "classic:zhou-yu");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"no-unplanned-discard","query":"handLimit","operation":"add","value":30,"priority":0}],
                  "activations":[
                    {"id":"one","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
                      "usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                    {"id":"three","minCards":3,"maxCards":3,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
                      "usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":3}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "弃牌", description = "弃置指定的手牌。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            var skill = catalog.Presentations[Driver];
            builder.AddSkill(new(Driver, skill.Name, skill.Description) { Program = catalog.Programs[Driver], ProgramPresentation = skill });
            builder.AddGeneral(new(Generals[0], "激昂角色", "supporter", Jiang, "wu", 4, [Driver]));
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "其他角色", "supporter", Driver, "wei", 8));
            builder.AddDeck(new(Deck, "决斗与红黑杀", 18, 2, []) { PhysicalCards = Enumerable.Range(0, 96).Select(index =>
                (index % 3) switch
                {
                    0 => new ContentDeckPhysicalCard("standard:duel", Suit.Spade, 1),
                    1 => new ContentDeckPhysicalCard("standard:slash", Suit.Heart, 7),
                    _ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)
                }).ToArray() });
            builder.AddMode(new(Mode, "激昂规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
