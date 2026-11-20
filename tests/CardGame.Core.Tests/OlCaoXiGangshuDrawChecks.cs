using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlCaoXiGangshuDrawChecks
{
    private const string Hero = "ol:cao-xi", Gangshu = "ol:gangshu";
    private const string Mode = "identity:classic-gangshu-draw", Deck = "fixture:gangshu-draw-deck";
    private static readonly string[] Generals = [Hero, "fixture:gangshu-slash-user", "fixture:gangshu-peer-2", "fixture:gangshu-peer-3"];

    internal static void GangshuDrawExpiresOnActualDrawOrPriorDodgeCold()
    {
        var stock = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 64).ToArray();
        var probe = Start(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        var study = probe.CreateSnapshot(0).Players[0].Hand[0].Id;
        var slash = probe.CreateSnapshot(1).Players[1].Hand[0].Id;
        stock[study - 1] = new("standard:draw_two", Suit.Heart, 7);
        stock[slash - 1] = new("standard:slash", Suit.Spade, 5);

        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock));
        var game = Start(content);
        Play(game, 0, study);
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Gangshu && HasChoice(prompt, "program-action", "activate"));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true } && HasChoice(prompt, "query", "DrawCount"));
        var started = Facts<CaoXiGangshuStartedEvent>(game).Single();
        var studyUse = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == study);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == studyUse.ResolutionId);
        Require(use is { Step: ResolutionFrameStep.Completed,
                Action: { ActorSeat: 0, Type: CardActionType.Use, EffectiveKind: CardKind.DrawTwo } action } &&
            action.ActionId == started.ActionId && action.PhysicalCards.Single().CardId == study &&
            started.CardUseFrameId == use.Id &&
            started.Source is { OwnerSeat: 0, SkillId: Gangshu } && started.Numbers.DrawCount == 2 &&
            game.CardMovements.Count(move => move.CardId == study && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            game.CreateSnapshot(1).PendingDecision is null,
            "Cao Xi privately chooses from real completed Draw Two use with one paid original card and a base draw count of two.");
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("query") == "DrawCount");
        Settle(game);
        var grant = Facts<CaoXiGangshuIncreasedEvent>(game).Single();
        Require(grant is { Query: SkillRuleQuery.DrawCount, BeforeValue: 2, AfterValue: 3, GrantedInDrawPhaseToken: null } &&
            grant.FrameId == started.FrameId && grant.Source == started.Source &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == studyUse.ResolutionId) == 1 &&
            Facts<CaoXiGangshuDrawUnitIssuedEvent>(game).Length == 0,
            "The completed native Draw Two adds one next-phase point, with no draw-phase issuance during Play.");

        var firstTurn = game.State.TurnNumber;
        var normal = Cold(game, content);
        var draw = NextDraw(normal, firstTurn, 3);
        var issued = Facts<CaoXiGangshuDrawUnitIssuedEvent>(normal).Single();
        Require(issued.GrantFrameId == grant.FrameId && issued.Source == grant.Source &&
            issued.Phase == OriginalDrawPhase(normal, draw).Phase && issued.Phase.Token > grant.GrantedAfterActualPhaseToken &&
            Facts<CaoXiGangshuUnitExpiredEvent>(normal).SequenceEqual(
                [new(grant.FrameId, 0, "actual-draw-completed", draw.FrameId)]),
            "Exactly one issuance on the next actual Draw token expires with that same phase's completion.");
        _ = NextDraw(normal, draw.ActualTurnNumber, 2);
        Require(Facts<CaoXiGangshuDrawUnitIssuedEvent>(normal).Length == 1 && Facts<CaoXiGangshuUnitExpiredEvent>(normal).Length == 1,
            "A following actual Draw receives the base number and cannot issue or expire the original point twice.");
        _ = Cold(normal, content);

        EndPlay(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Play(game, 1, slash, 0);
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        var attack = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash);
        var dodge = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards[0];
        Require(Facts<CaoXiGangshuUnitExpiredEvent>(game).Length == 0 && Facts<CaoXiGangshuDrawUnitIssuedEvent>(game).Length == 0,
            "The original point is live when the real incoming Slash first asks for Dodge.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([dodge]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Require(Facts<CaoXiGangshuActualOffsetEvent>(game).Single() is { OwnerSeat: 0, Kind: "slash" } offset &&
            offset.NativeId == attack.ResolutionId && Facts<CaoXiGangshuUnitExpiredEvent>(game).SequenceEqual(
                [new(grant.FrameId, 0, "actual-slash-offset", attack.ResolutionId)]) &&
            game.CardMovements.Count(move => move.CardId == dodge && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Respond) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == attack.ResolutionId) == 1 &&
            Facts<DamageAppliedEvent>(game).All(fact => fact.TargetSeat != 0),
            "Actual Dodge payment fully cancels the original Slash once and expires the live draw point immediately.");
        _ = NextDraw(game, firstTurn, 2);
        Require(Facts<CaoXiGangshuDrawUnitIssuedEvent>(game).Length == 0 && Facts<CaoXiGangshuUnitExpiredEvent>(game).Length == 1,
            "A point cleared by actual cancellation is unavailable to the next actual Draw.");
        _ = Cold(game, content);
    }

    internal static void GangshuSlashLimitAdmitsOneMoreNativeSlashCold()
    {
        var stock = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 64).ToArray();
        var probe = Start(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        var placement = probe.CreateSnapshot(0).Players[0].Hand.Take(4).Select(card => card.Id).ToArray();
        Require(placement.Length == 4, "The fixed opening supplies slots for one Draw Two and three real Slashes.");
        var study = placement[0];
        var slashes = placement[1..];
        stock[study - 1] = new("standard:draw_two", Suit.Heart, 7);
        foreach (var slash in slashes)
            stock[slash - 1] = new("standard:slash", Suit.Spade, 5);

        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock));
        var beforeChoice = Start(content);
        Play(beforeChoice, 0, study);
        Reach(beforeChoice, prompt => prompt.SkillPrompt?.SkillId == Gangshu && HasChoice(prompt, "program-action", "activate"));
        Activate(beforeChoice);
        Reach(beforeChoice, prompt => prompt is { PlayerSeat: 0, IsPrivate: true } && HasChoice(prompt, "query", "SlashLimit"));
        var started = Facts<CaoXiGangshuStartedEvent>(beforeChoice).Single();
        var turn = beforeChoice.State.TurnNumber;
        foreach (var (query, baseValue, permitted) in new[] { (SkillRuleQuery.DrawCount, 2, 1), (SkillRuleQuery.SlashLimit, 1, 2) })
        {
            var game = Cold(beforeChoice, content);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("query") == query.ToString());
            Settle(game);
            var grant = Facts<CaoXiGangshuIncreasedEvent>(game).Single();
            Require(grant.Query == query && grant.BeforeValue == baseValue && grant.AfterValue == baseValue + 1 &&
                grant.FrameId == started.FrameId && grant.Source == started.Source &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == started.CardUseFrameId &&
                    fact.CardId == study && fact.CardKind == CardKind.DrawTwo) == 1,
                "The selected point comes from the same completed physical Draw Two use in both branches.");

            var actionIds = new List<long>();
            foreach (var slash in slashes.Take(permitted))
            {
                Play(game, 0, slash, 1);
                Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge });
                var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash);
                Require(use is { SourceSeat: 0, CardKind: CardKind.Slash,
                        Action: { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash } } &&
                    use.TargetSeats.SequenceEqual([1]) && use.Action.PhysicalCards.Single().CardId == slash,
                    "Each admitted Slash holds its original hand entity and adjacent target on a live native use.");
                actionIds.Add(use.Action!.ActionId);
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
                Settle(game);
                Require(game.State.TurnNumber == turn &&
                    game.CardMovements.Count(move => move.CardId == slash && move.From == CardLocation.Hand(0) &&
                        move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                    Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.Id && fact.CardId == slash) == 1,
                    "Each admitted native Slash pays and settles once in Cao Xi's original Play phase.");
            }

            var blocked = slashes[permitted];
            var movesBefore = game.CardMovements.Count;
            var usesBefore = Facts<CardUseDeclaredEvent>(game).Length;
            var revision = game.Revision;
            Require(actionIds.Distinct().Count() == permitted &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == blocked) &&
                game.GetLegalActions(0).All(action => action.CardId != blocked),
                $"After {permitted} distinct physical Slashes, the next hand Slash has no legal action.");
            var refused = game.Submit(new PlayCardCommand(0, blocked, [1], revision, P(game)!.PromptId, CardKind.Slash));
            Require(!refused.Accepted && game.Revision == revision && game.CardMovements.Count == movesBefore &&
                Facts<CardUseDeclaredEvent>(game).Length == usesBefore,
                "Public submission of the over-limit Slash cannot declare the card or spend its physical entity.");
            _ = Cold(game, content);
        }
    }

    private static ActualDrawPhaseCompletedEvent NextDraw(GameEngine game, int afterTurn, int quantity)
    {
        for (var turn = 0; turn < game.PlayerCount && (game.State.TurnNumber <= afterTurn || P(game)!.PlayerSeat != 0); turn++)
        {
            EndPlay(game);
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        }
        var draw = Facts<ActualDrawPhaseCompletedEvent>(game).Single(fact =>
            fact.OwnerSeat == 0 && fact.ActualTurnNumber == game.State.TurnNumber && !fact.IsExtra);
        var phase = OriginalDrawPhase(game, draw);
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && draw.ActualTurnNumber > afterTurn &&
            !draw.Skipped && draw.DrawCount == quantity && game.CardMovements.Count(move =>
                move.Sequence > phase.MovementSequenceBefore && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == quantity,
            $"Cao Xi's next actual Draw should complete with {quantity} new physical cards, observed {draw.DrawCount}.");
        return draw;
    }

    private static ActualDiscardRecoveryPhaseStartedEvent OriginalDrawPhase(GameEngine game, ActualDrawPhaseCompletedEvent draw) =>
        Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Single(fact => fact.Phase is { ActorSeat: 0, Kind: ActualDiscardRecoveryPhaseKind.Draw } &&
            fact.Phase.TurnNumber == draw.ActualTurnNumber);

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    private static bool HasChoice(PendingDecision prompt, string name, string value) =>
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault(name) == value);

    private static void EndPlay(GameEngine game)
    {
        var prompt = P(game)!;
        Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
    }

    private static void Play(GameEngine game, int seat, int cardId, params int[] targets)
    {
        var action = game.GetLegalActions(seat).Single(action => action.CardId == cardId && action.ConversionSource is null &&
            action.TargetSeats.SequenceEqual(targets));
        Accept(game, new PlayCardCommand(seat, cardId, targets, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restore preserves physical movement, exact phase and grant facts, and all four legal player views.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The fixed identity opening gives Cao Xi the first actual Play.");
        return game;
    }

    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-gangshu-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定刚述实体牌", 4, 2, []) { PhysicalCards = stock });
            builder.AddMode(new(Mode, "刚述实际摸牌期限", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
