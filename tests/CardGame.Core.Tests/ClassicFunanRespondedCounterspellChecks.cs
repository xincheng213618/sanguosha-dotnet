using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicFunanRespondedCounterspellChecks
{
    private const string Funan = "classic:funan", Driver = "fixture:counterspell-gift-duel", Watch = "fixture:counterspell-gift-reentry";
    private const string Mode = "identity:counterspell-gift-entry", Deck = "fixture:counterspell-gift-deck";
    private const string Exchange = "skill-program.response-entity-exchange";
    private const string Rediscard = "skill-program.fixture:counterspell-gift-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:counterspell-gift-actor", "fixture:counterspell-gift-owner", "fixture:counterspell-gift-counter", "fixture:counterspell-gift-peer"];

    internal static void FunanGiftsOnlyTheDirectlyAnsweredCounterspellEntryCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var play = Next(game);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "All four native materials are physically dealt before the first Play phase.");
        var duel = game.GetHumanLegalActions().First(action => action.PlayedCardKind == CardKind.Duel &&
            action.ConversionSource?.SkillId == Driver && action.TargetSeats.SequenceEqual([1]));
        var outer = duel.CardId!.Value;
        var first = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        var second = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        Accept(game, new PlayCardCommand(0, outer, duel.TargetSeats, game.Revision, play.PromptId, CardKind.Duel)
            { ConversionSource = duel.ConversionSource });

        Respond(game, seat: 1, card: first, depth: 0);
        var earlier = Next(game);
        var window = game.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        var response = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
        var paid = game.CardMovements.Single(move => move.CardId == first && move.Reason == CardMoveReasons.NullificationFinished);
        Require(earlier is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && window.ChainDepth == 1 && response.ParentFrameId == window.Id &&
            response is { Continuation: ProgramCardContinuation.NullificationResponse, Action.EffectiveKind: CardKind.Nullification } &&
            response.Action.PhysicalCards is [{ CardId: var cost, From: var source }] && cost == first && source == CardLocation.Hand(1) &&
            window.ResponseRuleNode?.Action.ActionId == response.Action.ActionId &&
            paid.From == CardLocation.Processing && paid.To == CardLocation.DiscardPile,
            "The early public choice belongs to the same native chain as the first genuine counterspell and its settled entry.");

        var control = Cold(game, content);
        Skip(control);
        Respond(control, seat: 2, card: second, depth: 1);
        Require(Next(control) is { PlayerSeat: 1, SkillPrompt.SkillId: Funan }, "Funan can offer an exchange for the untouched first counterspell.");
        var reply = AssertDirectReply(control, window, response.Action, second);
        Activate(control);
        FinishOuter(control, window, outer, first, second);
        Require(Facts<ProgramResponseEntityClaimedEvent>(control).ToArray() is
            [{ Restricted: true, RecipientSeat: 2 } gift, { Restricted: false, RecipientSeat: 1 } take] &&
            gift.RespondedActionId == response.Action.ActionId && gift.ClaimedActionId == response.Action.ActionId && gift.CardIds.SequenceEqual([first]) &&
            take.ClaimedActionId == reply.ActionId && take.ResponseActionId == reply.ActionId && take.CardIds.SequenceEqual([second]) &&
            Zone(control, first) == CardLocation.Hand(2) && Zone(control, second) == CardLocation.Hand(1) && Zone(control, outer) == CardLocation.DiscardPile,
            "The original first counterspell is delivered with its restriction and then the actual second response is acquired.");

        Activate(game);
        var rediscard = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == first && move.Sequence > paid.Sequence).ToArray();
        Require(rediscard is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } && rediscard.Choices.Any(choice => choice.Cards.SequenceEqual([first])) &&
            game.ResolutionStack.OfType<NullificationWindowFrame>().Single().Id == window.Id &&
            pickup is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(1),
            "The previous counterspell really leaves its original discard entry for the earlier accepted-response effect.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([first]));
        var fresh = game.CardMovements.Last(move => move.CardId == first && move.To == CardLocation.DiscardPile);
        Require(fresh.Sequence > pickup[^1].Sequence && fresh.BatchId != paid.BatchId &&
            game.CardMovements.Any(move => move.CardId == first && move.From == CardLocation.Hand(1) &&
                move.Sequence > pickup[^1].Sequence && move.Sequence <= fresh.Sequence && move.Reason.Value == Rediscard),
            "The earlier effect makes a real, separate new discard of the same physical card.");

        Respond(game, seat: 2, card: second, depth: 1);
        var next = Next(game);
        if (next is { PlayerSeat: 1, SkillPrompt.SkillId: Funan })
        {
            AssertDirectReply(game, window, response.Action, second);
            Activate(game);
        }
        FinishOuter(game, window, outer, first, second);
        Require(!Facts<ProgramResponseEntityClaimedEvent>(game).Any() && !game.CardMovements.Any(move => move.Reason.Value == Exchange) &&
            Zone(game, first) == CardLocation.DiscardPile && Zone(game, second) == CardLocation.DiscardPile,
            "The exchange cannot deliver the prior counterspell from its later discard entry or acquire the directly answering card.");
    }

    private static CardActionContext AssertDirectReply(GameEngine game, NullificationWindowFrame original, CardActionContext first, int card)
    {
        var native = game.ResolutionStack.OfType<NullificationWindowFrame>().Single();
        var reply = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
        Require(native.Id == original.Id && native.ChainDepth == 2 && reply.ParentFrameId == native.Id &&
            native.ResponseRuleNode?.Action.ActionId == reply.Action.ActionId && reply.Action is { Type: CardActionType.Response, ActorSeat: 2, EffectiveKind: CardKind.Nullification } &&
            reply.Action.PhysicalCards is [{ CardId: var material }] && material == card && reply.Action.ParentActionId == first.ParentActionId,
            "The second accepted counterspell belongs to the directly answering node of the same original chain.");
        return reply.Action;
    }

    private static void Respond(GameEngine game, int seat, int card, int depth)
    {
        var prompt = Next(game);
        while (prompt is { Kind: DecisionKind.Nullification } && prompt.PlayerSeat != seat)
        {
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
            prompt = Next(game);
        }
        Require(prompt.Kind == DecisionKind.Nullification && prompt.PlayerSeat == seat &&
            game.ResolutionStack.OfType<NullificationWindowFrame>().Single().ChainDepth == depth,
            $"The native chain offers counterspell {depth + 1} to seat {seat}.");
        Answer(game, choice => choice.Cards.SequenceEqual([card]) && choice.Parameters.GetValueOrDefault("response") == "nullification");
    }

    private static void FinishOuter(GameEngine game, NullificationWindowFrame original, int outer, int first, int second)
    {
        var prompt = Next(game);
        while (prompt.Kind is DecisionKind.Nullification or DecisionKind.RespondSlash)
        {
            Answer(game, choice => choice.Cards.Count == 0);
            prompt = Next(game);
        }
        Require(prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            Facts<NullificationRespondedEvent>(game).Where(fact => fact.WindowFrameId == original.Id).ToArray() is
                [{ ChainDepth: 1, EffectNullified: true } firstFact, { ChainDepth: 2, EffectNullified: false } secondFact] &&
            firstFact.NullificationCardId == first && secondFact.NullificationCardId == second &&
            Facts<NullificationResolvedEvent>(game).Single(fact => fact.WindowFrameId == original.Id) is { EffectNullified: false, ChainDepth: 2 } &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == outer && fact.ResolutionId == original.ParentFrameId) == 1,
            "Both original native responses settle in order and the exact outer Duel finishes once.");
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return game;
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == id).Location;

    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restoration preserves the exact native response chain, cursor and reactions.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-funan-counterspell-gift", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:xue-zong");
            var rules = $$"""
                {"skills":[
                  {"id":"{{Driver}}","revision":1,"viewAs":[{"id":"duel","inputKinds":["nullification"],"inputSuits":[],
                    "outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"draw-and-discard-first-counterspell","window":"cardResponseAccepted",
                    "ownerRelation":"actor","cardKinds":["nullification"],"priority":1000,"optional":true,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实体决斗", description = "将无懈可击当决斗使用。" },
                    [Watch] = new { name = "取得并重弃", description = "使用无懈可击后可摸一张牌并弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "原实体锦囊使用者", "supporter", Driver, "wei", 5));
            builder.AddGeneral(new(Generals[1], "原无懈与复难", "supporter", Funan, "wu", 5, [Watch]));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "shu", 5));
            builder.AddDeck(new(Deck, "四张真实无懈", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 4).Select(rank => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "经典复难上一无懈", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
