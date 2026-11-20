using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileFunanDiscardEntryChecks
{
    private const string Funan = "mobile:funan", Driver = "fixture:mobile-funan-as-duel", Earlier = "fixture:mobile-funan-earlier-response";
    private const string Mode = "identity:mobile-funan-response-entry", Deck = "fixture:mobile-funan-response-deck";
    private const string Exchange = "skill-program.response-entity-exchange";
    private const string Rediscard = "skill-program.fixture:mobile-funan-earlier-response.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:mobile-funan-user", "fixture:mobile-funan-responder", "fixture:mobile-funan-peer-2", "fixture:mobile-funan-peer-3"];

    internal static void FunanCannotClaimARediscardedCounterspellAfterAnEarlierAcceptedCandidate()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));

        Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The actual Play phase starts with all six physical cards dealt or drawn.");
        var duel = game.GetHumanLegalActions().First(action => action.PlayedCardKind == CardKind.Duel &&
            action.ConversionSource?.SkillId == Driver && action.TargetSeats.SequenceEqual([1]));
        var original = duel.CardId!.Value;
        Accept(game, new PlayCardCommand(0, original, duel.TargetSeats, game.Revision, P(game)!.PromptId, CardKind.Duel)
            { ConversionSource = duel.ConversionSource });

        var prompt = Next(game);
        while (prompt is { Kind: DecisionKind.Nullification, PlayerSeat: not 1 })
        {
            PassCounterspell(game);
            prompt = Next(game);
        }
        Require(prompt is { Kind: DecisionKind.Nullification, PlayerSeat: 1 }, "The original target can play its actual native counterspell.");
        var card = game.CreateSnapshot(1).Players[1].Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        Answer(game, choice => choice.Cards.SequenceEqual([card]) && choice.Parameters.GetValueOrDefault("response") == "nullification");
        Require(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier }, "The earlier response trigger runs before the mobile claim in the same window.");
        var responseWindow = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
        var paid = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.NullificationFinished);
        Require(responseWindow is { Continuation: ProgramCardContinuation.NullificationResponse, Action.EffectiveKind: CardKind.Nullification } &&
            responseWindow.Action.PhysicalCards is [var cost] && cost.CardId == card && cost.From == CardLocation.Hand(1) &&
            responseWindow.Candidates.Any(c => c.SkillId == Funan) && paid.From == CardLocation.Processing && paid.To == CardLocation.DiscardPile &&
            Facts<CardActionAcceptedEvent>(game).Count(e => e.Action.ActionId == responseWindow.Action.ActionId) == 1,
            "A single genuine accepted Nullification owns the first discard and has both scheduled observers.");

        var control = Cold(game, content);
        Skip(control);
        Require(Next(control) is { PlayerSeat: 0, SkillPrompt.SkillId: Funan }, "The untouched original entry is offered to mobile Funan.");
        Activate(control);
        FinishUse(control, original, card);
        Require(Facts<ProgramResponseEntityClaimedEvent>(control).Single() is { SkillId: Funan, RecipientSeat: 0, Restricted: false } claim &&
            claim.ResponseActionId == responseWindow.Action.ActionId && claim.CardIds.SequenceEqual([card]) &&
            control.CardMovements.Single(move => move.Reason.Value == Exchange).To == CardLocation.Hand(0),
            "Mobile Funan directly claims that response and issues no original-trick restriction or gift.");

        Activate(game);
        var discard = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == card && move.Sequence > paid.Sequence).ToArray();
        Require(discard is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier } && discard.Choices.Any(choice => choice.Cards.SequenceEqual([card])) &&
            game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(frame => frame.Id == responseWindow.Id) &&
            pickup is [var shuffled, var drawn] && shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            shuffled.Reason == CardMoveReasons.Reshuffle && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(1),
            "The earlier accepted-response candidate really reshuffles and draws that physical counterspell with the original window still open.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        if (Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Funan })
        {
            Require(game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last().Id == responseWindow.Id,
                "Any remaining offer must use the original response window.");
            Activate(game);
        }
        FinishUse(game, original, card);
        var later = game.CardMovements.Where(move => move.CardId == card && move.Reason.Value == Rediscard).ToArray();
        Require(later.Length > 0 && later[0].From == CardLocation.Hand(1) && later[^1].To == CardLocation.DiscardPile &&
            later[^1].Sequence > pickup[^1].Sequence && later[^1].BatchId != paid.BatchId,
            "The earlier candidate makes a real second discard of the same physical card into a different batch.");
        Require(!Facts<ProgramResponseEntityClaimedEvent>(game).Any() && !game.CardMovements.Any(move => move.Reason.Value == Exchange) &&
            game.CreateCardZoneDiagnostics().Single(location => location.CardId == card).Location == CardLocation.DiscardPile,
            "Mobile Funan cannot use its original accepted-response window to claim the card's later discard entry.");
    }

    private static void FinishUse(GameEngine game, int original, int response)
    {
        while (Next(game).Kind == DecisionKind.Nullification) PassCounterspell(game);
        Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            Facts<NullificationRespondedEvent>(game).Single().NullificationCardId == response &&
            Facts<NullificationResolvedEvent>(game).Single(e => e.EffectCardId == original).EffectNullified &&
            Facts<CardUseFinishedEvent>(game).Count(e => e.CardId == original) == 1 &&
            game.CardMovements.Single(move => move.CardId == original && move.Reason == CardMoveReasons.UseFinished).To == CardLocation.DiscardPile,
            "The native counterspell and the original Duel finish normally without a gift of the Duel material.");
    }

    private static void PassCounterspell(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay preserves the accepted response cursor, private choices and all physical moves.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-funan-response-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:xue-zong");
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"viewAs":[{"id":"duel","inputKinds":["nullification"],"inputSuits":[],
                    "outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}]},
                  {"id":"{{{Earlier}}}","revision":1,"triggers":[{"id":"draw-and-rediscard-accepted-card","window":"cardResponseAccepted",
                    "ownerRelation":"actor","cardKinds":["nullification"],"priority":1000,"optional":true,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实体决斗", description = "将无懈可击当决斗使用。" },
                    [Earlier] = new { name = "取得并重弃", description = "使用无懈可击后可摸一张牌并弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "移动复难测试", "supporter", Funan, "wu", 5, [Driver]));
            builder.AddGeneral(new(Generals[1], "原生响应者", "supporter", Earlier, "wei", 5));
            foreach (var peer in Generals.Skip(2)) builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "六张实际无懈", 1, 2, []) { PhysicalCards = Enumerable.Range(2, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, rank)).ToArray() });
            builder.AddMode(new(Mode, "移动复难原响应入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
