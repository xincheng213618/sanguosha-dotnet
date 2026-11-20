using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ClassicFunanDiscardEntryChecks
{
    private const string Funan = "classic:funan", Driver = "fixture:funan-as-duel", Watch = "fixture:funan-response-reentry";
    private const string Mode = "identity:funan-response-entry", Deck = "fixture:funan-response-deck";
    private const string Exchange = "skill-program.response-entity-exchange";
    private const string Rediscard = "skill-program.fixture:funan-response-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:funan-user", "fixture:funan-responder", "fixture:funan-peer-2", "fixture:funan-peer-3"];

    internal static void FunanCannotClaimARediscardedCounterspellFromItsOldResponse()
    {
        var (game, content) = Start();
        var play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The real opening draw exhausts the fixed deck before the response exchange.");
        var duel = game.GetHumanLegalActions().First(action => action.PlayedCardKind == CardKind.Duel &&
            action.ConversionSource?.SkillId == Driver && action.TargetSeats.SequenceEqual([1]));
        var originalCard = duel.CardId!.Value;
        Submit(game, new PlayCardCommand(0, originalCard, duel.TargetSeats, game.Revision, play.PromptId, CardKind.Duel)
            { ConversionSource = duel.ConversionSource });
        var counterspell = Next(game);
        while (counterspell is { Kind: DecisionKind.Nullification, PlayerSeat: not 1 })
        {
            Answer(game, counterspell, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
            counterspell = Next(game);
        }
        Need(counterspell is { Kind: DecisionKind.Nullification, PlayerSeat: 1 }, "The target receives a native Nullification prompt.");
        var responseCard = game.CreateSnapshot(1).Players[1].Hand.Single(card => card.Kind == CardKind.Nullification).Id;
        Answer(game, counterspell, choice => choice.Cards.SequenceEqual([responseCard]) && choice.Parameters.GetValueOrDefault("response") == "nullification");
        var invoke = Next(game);
        Need(invoke is { PlayerSeat: 0, SkillPrompt.SkillId: Funan }, "The real accepted response offers classic Funan to the original user.");
        var response = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last().Action;
        var responseDiscard = game.CardMovements.Single(move => move.CardId == responseCard && move.Reason == CardMoveReasons.NullificationFinished);
        Need(response is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification, ActorSeat: 1 } &&
             response.PhysicalCards is [var cost] && cost.CardId == responseCard && cost.From == CardLocation.Hand(1) &&
             responseDiscard.From == CardLocation.Processing && responseDiscard.To == CardLocation.DiscardPile &&
             Facts<CardActionAcceptedEvent>(game).Count(e => e.Action.ActionId == response.ActionId) == 1,
            "The accepted Nullification has an exact native Processing-to-Discard entry before Funan begins.");
        Answer(game, invoke, choice => IsAction(choice, "activate"));
        var child = Next(game);
        var paid = Facts<ProgramResponseEntityClaimedEvent>(game).Single();
        Need(child is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && paid is { SkillId: Funan, RecipientSeat: 1, Restricted: true } &&
             paid.CardIds.SequenceEqual([originalCard]) && paid.ResponseActionId == response.ActionId && paid.ClaimedActionId == response.ParentActionId &&
             game.CreateSnapshot(0).TurnProhibitedPhysicalCards!.Single().CardIds.SequenceEqual([originalCard]) &&
             game.CardMovements.Any(move => move.CardId == originalCard && move.From == CardLocation.Processing &&
                 move.To == CardLocation.Hand(1) && move.Reason.Value == Exchange) &&
             game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Funan && frame.ResponseEntityExchange is { Stage: 1 }),
            "Classic Funan has already delivered and restricted the original trick, and awaits the recipient's gain child before claiming the response.");

        var control = Cold(game, content);
        Answer(control, Next(control), choice => IsAction(choice, "skip"));
        FinishUse(control, originalCard, responseCard);
        Need(Facts<ProgramResponseEntityClaimedEvent>(control).Single(e => !e.Restricted) is { RecipientSeat: 0 } claim &&
             claim.ClaimedActionId == response.ActionId && claim.CardIds.SequenceEqual([responseCard]) &&
             control.CardMovements.Any(move => move.CardId == responseCard && move.From == CardLocation.DiscardPile &&
                 move.To == CardLocation.Hand(0) && move.Reason.Value == Exchange),
            "Without the intervening move, Funan really claims the original paid response after completing the initial gift child.");

        Answer(game, child, choice => IsAction(choice, "activate"));
        var discard = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == responseCard && move.Sequence > responseDiscard.Sequence).ToArray();
        Need(discard is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && discard.Choices.Any(choice => choice.Cards.SequenceEqual([responseCard])) &&
             pickup is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
             shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
             drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(1) && Facts<ProgramResponseEntityClaimedEvent>(game).Count() == 1,
            "The earlier real gain child reshuffles and draws the same response entity before the outer exchange resumes.");
        var restored = Cold(game, content);
        foreach (var run in new[] { game, restored })
        {
            Answer(run, Next(run), choice => choice.Cards.SequenceEqual([responseCard]));
            FinishUse(run, originalCard, responseCard);
            var later = run.CardMovements.Where(move => move.CardId == responseCard && move.Reason.Value == Rediscard).ToArray();
            Need(later.Length > 0 && later[0].From == CardLocation.Hand(1) && later[^1].To == CardLocation.DiscardPile &&
                 later[^1].Sequence > pickup[^1].Sequence && later[^1].BatchId != responseDiscard.BatchId &&
                 run.CardMovements.Count(move => move.CardId == originalCard && move.Reason.Value == Exchange) == 1,
                "A real owned discard creates a later entry for the same response entity, while the first Funan leg still pays only once.");
            Need(!Facts<ProgramResponseEntityClaimedEvent>(run).Any(e => !e.Restricted) &&
                 run.CreateCardZoneDiagnostics().Single(card => card.CardId == responseCard).Location == CardLocation.DiscardPile &&
                 !run.CardMovements.Any(move => move.CardId == responseCard && move.Reason.Value == Exchange),
                "The suspended exchange cannot use its older Nullification entry to claim the later discard.");
        }
    }

    private static void FinishUse(GameEngine game, int originalCard, int responseCard)
    {
        var next = Next(game);
        while (next.Kind == DecisionKind.Nullification)
        {
            Answer(game, next, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
            next = Next(game);
        }
        Need(next is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
             Facts<NullificationResolvedEvent>(game).Single(e => e.EffectCardId == originalCard).EffectNullified &&
             Facts<NullificationRespondedEvent>(game).Single().NullificationCardId == responseCard &&
             Facts<CardUseFinishedEvent>(game).Count(e => e.CardId == originalCard) == 1,
            "The exact native counterspell and original Duel continue once and return to the same Play phase.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Submit(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return (game, content);
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static bool IsAction(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;
    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay preserves all private card views and the suspended exchange's physical movements.");
        return restored;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt) return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Funan did not reach its next public decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> match) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Funan command rejected.");
    }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-funan-response-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:xue-zong");
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"viewAs":[{"id":"duel","inputKinds":["nullification"],"inputSuits":[],
                    "outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"original-trick-gain","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{{Exchange}}}"],"movementOccurrence":"perBatch","optional":true,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实体决斗", description = "将无懈可击当决斗使用。" },
                    [Watch] = new { name = "取得并重弃", description = "复难获牌后可摸一张牌并弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "复难测试", "supporter", Funan, "wu", 5, [Driver]));
            builder.AddGeneral(new(Generals[1], "原生响应者", "supporter", Watch, "wei", 5));
            foreach (var peer in Generals.Skip(2)) builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "六张实际无懈", 1, 2, []) { PhysicalCards = Enumerable.Range(2, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, rank)).ToArray() });
            builder.AddMode(new(Mode, "复难原响应入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
