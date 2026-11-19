using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlGuzhengChecks
{
    private const string Guzheng = "boundary:guzheng", Driver = "fixture:ol-guzheng-discard", Watch = "fixture:ol-guzheng-observer";
    private const string Mode = "identity:ol-guzheng-test", Deck = "fixture:ol-guzheng-deck";
    private const string ReturnReason = "skill-program.boundary:guzheng.actual-discard-return";
    private const string ClaimReason = "skill-program.boundary:guzheng.actual-discard-claim";
    private static readonly string[] Generals = ["fixture:ol-guzheng-source", "fixture:ol-guzheng-owner",
        "fixture:ol-guzheng-peer-2", "fixture:ol-guzheng-peer-3"];

    public static void ImmediateSameSourceBatchAndPaidChildrenReplay()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        var single = Discard(game, 1).Single();
        var batch = Discard(game, 3);
        var offer = Next(game);
        Need(offer.SkillPrompt?.SkillId == Guzheng && offer.PlayerSeat == 1 && game.State.Phase == TurnPhase.Play,
            "Only the qualifying batch opens an immediate OL Guzheng offer during Play.");
        var discarded = game.CardMovements.Where(move => batch.Contains(move.CardId) &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile).ToArray();
        Need(discarded.Length == batch.Length && discarded.Select(move => move.BatchId).Distinct().Count() == 1,
            "The qualifying input must be one genuine physical discard batch.");
        Answer(game, "program-action", "activate");
        var returns = Next(game);
        Need(returns.Choices.Select(choice => choice.Cards.Single()).ToHashSet().SetEquals(batch) &&
            returns.Choices.All(choice => choice.Targets.SequenceEqual([0])),
            "Only this batch's cards can be returned, to their original discard owner.");
        var selected = returns.Choices.First(choice => choice.Cards.SequenceEqual([batch[0]]));
        Submit(game, new AnswerPromptCommand(1, returns.PromptId, selected.Id, game.Revision));
        var given = game.Events.Select(e => e.Payload).OfType<ActualDiscardRecoveryReturnedEvent>().Single();
        Need(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } &&
            given.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && given.CardId == batch[0] &&
            Moves(game, ReturnReason).Single().To == CardLocation.Hand(0) && Moves(game, ClaimReason).Length == 0,
            "The paid return and its original gain child precede the claim.");
        game = Cold(game, content);
        Answer(game, "option-id", "continue");
        var skip = Cold(game, content);
        Answer(skip, "option", "skip");
        Need(Next(skip) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Moves(skip, ClaimReason).Length == 0,
            "The skill owner can leave the remaining cards after the paid return.");

        Answer(game, "option", "claim");
        Need(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Watch },
            "The claim must wait for its actual gain child.");
        var gains = Moves(game, ClaimReason);
        Need(gains.Select(move => move.CardId).SequenceEqual(discarded.Where(move => move.CardId != given.CardId).Select(move => move.CardId)) &&
            gains.Length == 2 && gains.Select(move => move.BatchId).Distinct().Count() == 1 &&
            gains.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(1)),
            "The two remaining originals are acquired together as one native movement batch.");
        game = Cold(game, content);
        Answer(game, "option-id", "continue");
        var later = Discard(game, 2);
        Need(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.Events.Select(e => e.Payload).OfType<ActualDiscardRecoveryReturnedEvent>().Count() == 1 &&
            Moves(game, ClaimReason).Length == 2 && game.CreateCardZoneDiagnostics().Where(card => later.Contains(card.CardId) ||
                card.CardId == single).All(card => card.Location == CardLocation.DiscardPile),
            "A later real batch in the same phase must respect the already consumed phase quota.");
        _ = Cold(game, content);
    }

    private static CardMovementRecord[] Moves(GameEngine game, string reason) =>
        game.CardMovements.Where(move => move.Reason.Value == reason).ToArray();

    private static int[] Discard(GameEngine game, int count)
    {
        var play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The original actor must still be in Play.");
        var cards = game.CreateSnapshot(0).Players[0].Hand.Take(count).Select(card => card.Id).ToArray();
        Submit(game, new UseProgramSkillCommand(0, Driver, "discard", cards, [], game.Revision, play.PromptId));
        return cards;
    }

    private static void Answer(GameEngine game, string key, string value)
    {
        var prompt = Next(game);
        var choice = prompt.Choices.Single(c => c.Parameters.GetValueOrDefault(key) == value);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            foreach (var seat in Enumerable.Range(0, game.PlayerCount))
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("OL Guzheng did not reach the next public command boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        Next(game);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements),
            "Command replay must restore every seat's view and the exact physical deliveries.");
        return restored;
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "OL Guzheng command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-guzheng", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:zhang-zhao-zhang-hong");
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"discard","minCards":1,"maxCards":3,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,
                     "targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"captureSelectedCards","target":"owner","resultBind":"paid"},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"paid","destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[
                    {"id":"native-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{{ReturnReason}}}","{{{ClaimReason}}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"gain-seen","options":[{"id":"continue"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Watch }.ToDictionary(id => id, id => new { name = id, description = "固政原生弃牌与获得场景",
                    optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "原弃牌角色", "supporter", Driver, "wei", 4, [Watch]));
            builder.AddGeneral(new(Generals[1], "界二张", "supporter", Guzheng, "wu", 3, [Watch]));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "装备牌", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "固政规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
