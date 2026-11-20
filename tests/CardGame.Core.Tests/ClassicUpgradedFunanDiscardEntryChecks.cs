using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicUpgradedFunanDiscardEntryChecks
{
    private const string Funan = "classic:funan", Jiexun = "classic:jiexun";
    private const string Driver = "fixture:upgraded-funan-driver", Earlier = "fixture:upgraded-funan-earlier-response";
    private const string Mode = "identity:upgraded-funan-response-entry", Deck = "fixture:upgraded-funan-response-deck";
    private const string Exchange = "skill-program.response-entity-exchange";
    private const string Rediscard = "skill-program.fixture:upgraded-funan-earlier-response.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:upgraded-funan-owner", "fixture:upgraded-funan-responder", "fixture:upgraded-funan-jiexun-target", "fixture:upgraded-funan-peer"];

    internal static void UpgradedFunanCannotClaimARediscardedCounterspell()
    {
        var (game, content) = Start();
        var exhausted = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        InvokeJiexun(game);
        ReachOwnerPlay(game);
        Require(Facts<ProgramEscalatingDiscardStartedEvent>(game).Single() is { OwnerSeat: 0, TargetSeat: 2, PreviousCount: 0, DrawCount: 0 } &&
            game.CreateSnapshot(2).Players[2].Hand.Single().Id == exhausted && !Facts<ProgramResponseExchangeUpgradedEvent>(game).Any(),
            "The first real Jiexun counts no public diamonds, discards zero cards and does not upgrade Funan.");

        InvokeJiexun(game);
        Require(Next(game) is { PlayerSeat: 2, SkillPrompt.SkillId: Jiexun } &&
            P(game)!.Choices.Single().Cards.SequenceEqual([exhausted]), "The second Jiexun requires its target to discard their only owned card.");
        Answer(game, choice => choice.Cards.SequenceEqual([exhausted]));
        ReachOwnerPlay(game);
        var discarded = game.CardMovements.Single(move => move.CardId == exhausted && move.Reason.Value == "skill-program.public-suit-escalating-discard");
        var upgraded = Facts<ProgramResponseExchangeUpgradedEvent>(game).Single();
        Require(discarded.From == CardLocation.Hand(2) && discarded.To == CardLocation.DiscardPile &&
            game.CreateSnapshot(2).Players[2] is { HandCount: 0, Equipment.Count: 0 } &&
            Facts<ProgramEscalatingDiscardStartedEvent>(game).Select(e => e.PreviousCount).SequenceEqual([0, 1]) &&
            upgraded is { OwnerSeat: 0, LostSkillId: Jiexun, SkillId: Funan } &&
            game.CreateSnapshot(0).Players[0].Skills is { } skills && skills.Any(skill => skill.Id == Funan) && skills.All(skill => skill.Id != Jiexun) &&
            game.CreateSnapshot(0).ProgramResponseExchangeStates!.Single() is { IsUpgraded: true } state && state.SkillInstanceId == upgraded.SkillInstanceId,
            "Actual complete HE exhaustion upgrades the matching classic Funan and really removes Jiexun.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "draw-one", [], [], game.Revision, P(game)!.PromptId));
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 } &&
            game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == exhausted),
            "An actual draw removes the exhausted card from the discard pile before the later native use.");
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
        Require(prompt is { Kind: DecisionKind.Nullification, PlayerSeat: 1 }, "The target receives a native Nullification response.");
        var response = game.CreateSnapshot(1).Players[1].Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        Answer(game, choice => choice.Cards.SequenceEqual([response]) && choice.Parameters.GetValueOrDefault("response") == "nullification");
        Require(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier }, "The earlier real candidate runs in the accepted response window.");
        var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
        var entry = game.CardMovements.Single(move => move.CardId == response && move.Reason == CardMoveReasons.NullificationFinished);
        Require(window is { Continuation: ProgramCardContinuation.NullificationResponse, Action.EffectiveKind: CardKind.Nullification } &&
            window.Action.PhysicalCards is [var cost] && cost.CardId == response && cost.From == CardLocation.Hand(1) &&
            window.Candidates.Any(c => c.SkillId == Funan && c.SkillInstanceId == upgraded.SkillInstanceId) &&
            entry.From == CardLocation.Processing && entry.To == CardLocation.DiscardPile,
            "That exact paid Nullification has entered Discard and offered the already-upgraded source an original response opportunity.");

        var control = Cold(game, content);
        Skip(control);
        Require(Next(control) is { PlayerSeat: 0, SkillPrompt.SkillId: Funan }, "Upgraded Funan still offers the untouched original response card.");
        Activate(control);
        FinishUse(control, original, response);
        Require(Facts<ProgramResponseEntityClaimedEvent>(control).Single() is { SkillId: Funan, RecipientSeat: 0, Restricted: false } claim &&
            claim.ResponseActionId == window.Action.ActionId && claim.CardIds.SequenceEqual([response]) &&
            control.CardMovements.Single(move => move.Reason.Value == Exchange).To == CardLocation.Hand(0),
            "The upgraded skill claims the original response directly and keeps the original trick on its native path.");

        Activate(game);
        var discard = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == response && move.Sequence > entry.Sequence).ToArray();
        Require(discard is { PlayerSeat: 1, SkillPrompt.SkillId: Earlier } && discard.Choices.Any(choice => choice.Cards.SequenceEqual([response])) &&
            pickup is [var shuffled, var drawn] && shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            shuffled.Reason == CardMoveReasons.Reshuffle && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(1) &&
            game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(f => f.Id == window.Id),
            "The earlier candidate really draws the response entity through native reshuffle while the original acceptance is pending.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([response]));
        if (Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Funan })
        {
            Require(game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last().Id == window.Id,
                "Any remaining Funan offer still belongs to the old accepted response.");
            Activate(game);
        }
        FinishUse(game, original, response);
        var later = game.CardMovements.Where(move => move.CardId == response && move.Reason.Value == Rediscard).ToArray();
        Require(later.Length > 0 && later[0].From == CardLocation.Hand(1) && later[^1].To == CardLocation.DiscardPile &&
            later[^1].Sequence > pickup[^1].Sequence && later[^1].BatchId != entry.BatchId,
            "The card genuinely reenters the discard pile through a different action and batch.");
        Require(!Facts<ProgramResponseEntityClaimedEvent>(game).Any() && !game.CardMovements.Any(move => move.Reason.Value == Exchange) &&
            game.CreateCardZoneDiagnostics().Single(location => location.CardId == response).Location == CardLocation.DiscardPile,
            "Classic upgraded Funan cannot use the old response window to claim the later discard entry.");
    }

    private static void InvokeJiexun(GameEngine game)
    {
        EndPlay(game);
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Jiexun }, "The real end phase offers classic Jiexun.");
        Activate(game);
        Next(game);
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
    }

    private static void ReachOwnerPlay(GameEngine game)
    {
        for (var turns = 0; turns < 4; turns++)
        {
            var play = Next(game);
            Require(play.Kind == DecisionKind.PlayCard, "The intervening real turn reaches a normal Play phase.");
            if (play.PlayerSeat == 0) return;
            EndPlay(game);
        }
        throw new InvalidOperationException("The upgraded Funan fixture did not reach the owner's next turn.");
    }

    private static void FinishUse(GameEngine game, int original, int response)
    {
        while (Next(game).Kind == DecisionKind.Nullification) PassCounterspell(game);
        Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            Facts<NullificationRespondedEvent>(game).Single().NullificationCardId == response &&
            Facts<NullificationResolvedEvent>(game).Single(e => e.EffectCardId == original).EffectNullified &&
            Facts<CardUseFinishedEvent>(game).Count(e => e.CardId == original) == 1 &&
            game.CardMovements.Single(move => move.CardId == original && move.Reason == CardMoveReasons.UseFinished).To == CardLocation.DiscardPile &&
            game.CreateSnapshot(0).TurnProhibitedPhysicalCards is null,
            "The native response and original use finish once and the upgraded branch creates no legacy restriction.");
    }

    private static void EndPlay(GameEngine game)
    {
        var prompt = Next(game);
        Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
    }
    private static void PassCounterspell(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay preserves the actual Jiexun upgrade and current native response choice.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 12 }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The fixture begins in the real lord's turn.");
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-upgraded-funan-response-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:xue-zong");
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"viewAs":[{"id":"duel","inputKinds":["nullification"],"inputSuits":[],
                    "outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}],
                   "triggers":[{"id":"empty-normal-draw","window":"drawPhaseStarting","drawPhaseMode":"replacement","subject":"owner",
                    "optional":false,"effects":[{"op":"draw","target":"owner","amount":0}]}],
                   "activations":[{"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving",
                    "usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{Earlier}}}","revision":1,"triggers":[{"id":"draw-and-rediscard-accepted-card","window":"cardResponseAccepted",
                    "ownerRelation":"actor","cardKinds":["nullification"],"priority":1000,"optional":true,"effects":[
                     {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实际摸牌与用牌", description = "跳过通常摸牌，可以摸一张牌或将无懈可击当决斗使用。" },
                    [Earlier] = new { name = "取得并重弃", description = "使用无懈可击后可摸一张牌并弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "升级复难测试", "supporter", Funan, "wu", 5, [Jiexun, Driver]));
            builder.AddGeneral(new(Generals[1], "原生响应者", "supporter", Driver, "wei", 5, [Earlier]));
            foreach (var peer in Generals.Skip(2)) builder.AddGeneral(new(peer, "同桌", "supporter", Driver, "wei", 5));
            builder.AddDeck(new(Deck, "四张实际无懈", 1, 0, []) { PhysicalCards = Enumerable.Range(2, 4)
                .Select(rank => new ContentDeckPhysicalCard("standard:nullification", Suit.Club, rank)).ToArray() });
            builder.AddMode(new(Mode, "升级复难原响应入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
