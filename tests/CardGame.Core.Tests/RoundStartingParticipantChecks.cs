using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class RoundStartingParticipantChecks
{
    private const string Mode = "identity:round-participants";
    private const string Fengji = "ol:fengji";

    public static void ForeignOwnerReceivesRoundStartAndColdReturns()
    {
        var registry = CreateRegistry();
        var game = StartAtForeignRoundPrompt(registry);
        var round = Facts<RoundStartedEvent>(game).Single();
        Require(round.ActorSeat != 1 && game.State.CurrentSeat == round.ActorSeat,
            "The actual round must begin in a different participant's turn.");
        game = Cold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "fengji-option"));
        game = Cold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("accept") == "false");
        Require(Facts<ProgramFengjiOptionChosenEvent>(game).Where(choice => choice.OwnerSeat == 1).ToArray() is
            [{ Option: FengjiOptionKind.Draw, Accepted: false }],
            "The foreign owner commits the first round option once.");
        game = Cold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("accept") == "false");
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        var choices = Facts<ProgramFengjiOptionChosenEvent>(game).Where(choice => choice.OwnerSeat == 1).ToArray();
        Require(choices.Length == 2 && choices.All(choice => choice.OwnerSeat == 1 &&
            choice.RoundNumber == round.RoundNumber && !choice.Accepted) &&
            choices.Select(choice => choice.Option).Distinct().Count() == 2 &&
            Facts<ProgramBindingStartedEvent>(game).Count(binding => binding.SkillId == Fengji && binding.OwnerSeat == 1) == 1,
            "Later turns in the same round must not re-open the completed two-option opportunity.");

        var turn = game.State.TurnNumber;
        var draws = NormalDrawsBeforePlay(game);
        Require(Facts<RoundStartedEvent>(game).SequenceEqual([round]) && draws is [var drawn] &&
            game.CardMovements.Count(move => move.CardId == drawn.CardId && move.TurnNumber == turn && move.From == drawn.From &&
                move.To == drawn.To && move.Reason == drawn.Reason) == 1,
            "The original round's declined draw reduction adds one actual card during Chen Deng's own normal Draw before Play.");
        _ = Cold(game, registry);
    }

    public static void FengjiAdmitsTwoPhysicalSlashesInTheSameRound()
    {
        var stock = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 5), 32).ToArray();
        // The same seeded deal and draw locate the three physical slots without changing any peer's cards.
        var probe = ReachPlay(CreateRegistry(2, stock), verifyRecovery: false);
        var dealt = Dealt(probe);
        var drawn = NormalDrawsBeforePlay(probe).Select(move => move.CardId).ToArray();
        Require(dealt.Length == 2 && drawn.Length == 1, "The fixed round supplies two dealt cards and one actual Draw card.");
        foreach (var id in dealt.Concat(drawn))
            stock[id - 1] = new("standard:slash", Suit.Spade, 7);

        var registry = CreateRegistry(2, stock);
        var game = ReachPlay(registry, verifyRecovery: true);
        var turn = game.State.TurnNumber;
        var round = Facts<RoundStartedEvent>(game).Single();
        var choices = Facts<ProgramFengjiOptionChosenEvent>(game).Where(choice => choice.OwnerSeat == 1).ToArray();
        Require(round.ActorSeat != 1 && choices is
            [{ Option: FengjiOptionKind.Draw, Accepted: false }, { Option: FengjiOptionKind.Slash, Accepted: false }] &&
            choices.All(choice => choice.RoundNumber == round.RoundNumber),
            $"Chen Deng must decline Draw then Slash in foreign-started round {round.RoundNumber}. Actual choices: {string.Join(", ", choices.Select(choice => $"{choice.Option}={choice.Accepted} (round {choice.RoundNumber})"))}.");
        var actualDealt = Dealt(game);
        Require(actualDealt.SequenceEqual(dealt),
            $"Expected opening Slash ids {string.Join(", ", dealt)}. Actual initial deal: {string.Join(", ", actualDealt)}.");
        var draws = NormalDrawsBeforePlay(game);
        Require(draws is [{ CardKind: CardKind.Slash } draw] && draw.CardId == drawn[0] &&
            game.CardMovements.Count(move => move.CardId == draw.CardId && move.TurnNumber == turn &&
                move.From == draw.From && move.To == draw.To && move.Reason == draw.Reason) == 1,
            $"Expected only the original Slash {drawn[0]} from Draw before Play in turn {turn}. Actual draws: {string.Join(", ", draws.Select(move => $"{move.CardId} ({move.CardKind})"))}.");

        foreach (var id in new[] { drawn[0], dealt[0] })
        {
            var legal = game.GetLegalActions(1).Single(action => action.CardId == id && action.Kind == LegalActionKind.Slash &&
                action.ConversionSource is null && action.TargetSeats.SequenceEqual([2]));
            Accept(game, new PlayCardCommand(1, id, legal.TargetSeats, game.Revision, game.PendingDecision!.PromptId, CardKind.Slash));
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            var use = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == id && fact.SourceSeat == 1 && fact.CardKind == CardKind.Slash);
            var action = Facts<CardActionAcceptedEvent>(game).Single(fact => fact.Action is
                { Type: CardActionType.Use, ActorSeat: 1, EffectiveKind: CardKind.Slash } &&
                fact.Action.PhysicalCards is [{ CardId: var paid }] && paid == id).Action;
            Require(game.State.TurnNumber == turn && action.TargetSeats.SequenceEqual([2]),
                $"Slash {id} must target the same adjacent seat during turn {turn}.");
            Require(game.CardMovements.Count(move => move.CardId == id && move.TurnNumber == turn && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1,
                $"Slash {id} must pay its exact hand entity once.");
            Require(Facts<CardUseDebitRecordedEvent>(game).Count(fact => fact.Debit is { ActorSeat: 1, Query: SkillRuleQuery.SlashLimit, Phase: TurnPhase.Play } &&
                    fact.Debit.CardActionId == action.ActionId && fact.Debit.TurnNumber == turn) == 1,
                $"Slash {id} must debit the ordinary limit once in Play for action {action.ActionId}.");
            Require(Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.ResolutionId && fact.CardId == id && fact.CardKind == CardKind.Slash) == 1,
                $"Slash {id} must finish its original use {use.ResolutionId} once.");
        }

        var blocked = dealt[1];
        var revision = game.Revision;
        var movementCount = game.CardMovements.Count;
        Require(game.GetLegalActions(1).All(action => action.CardId != blocked), "The third physical Slash is absent from legal actions.");
        var refused = game.Submit(new PlayCardCommand(1, blocked, [2], revision, game.PendingDecision!.PromptId, CardKind.Slash));
        Require(!refused.Accepted && game.Revision == revision && game.CardMovements.Count == movementCount &&
            game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == blocked),
            "The third Slash is rejected by the public command and stays in Chen Deng's hand.");
        _ = Cold(game, registry);

        static int[] Dealt(GameEngine game) => game.CardMovements.Where(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(1) && move.Reason == CardMoveReasons.InitialDeal).Select(move => move.CardId).ToArray();

        static GameEngine ReachPlay(ContentRegistry registry, bool verifyRecovery)
        {
            var game = StartAtForeignRoundPrompt(registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "fengji-option"));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("accept") == "false");
            if (verifyRecovery) game = Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("accept") == "false");
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            return game;
        }
    }

    private static CardMovedEvent[] NormalDrawsBeforePlay(GameEngine game)
    {
        var turn = game.State.TurnNumber;
        var start = game.Events.Single(entry => entry.Payload is TurnStartedEvent { ActorSeat: 1 } started && started.TurnNumber == turn);
        var play = game.Events.Single(entry => entry.Sequence > start.Sequence &&
            entry.Payload is PhaseChangedEvent { ActorSeat: 1, Phase: TurnPhase.Play });
        return game.Events.Where(entry => entry.Sequence > start.Sequence && entry.Sequence < play.Sequence)
            .Select(entry => entry.Payload).OfType<CardMovedEvent>().Where(move => move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(1) && move.Reason == CardMoveReasons.Draw).ToArray();
    }

    private static ContentRegistry CreateRegistry(int openingHandSize = 1, IReadOnlyList<ContentDeckPhysicalCard>? stock = null) =>
        ContentRegistry.Build(new StandardContentPackage(), new Fixture(ContentRegistry.Build(new StandardContentPackage(),
            new StandardActiveSkillExpansionPackage(), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage()), openingHandSize, stock));

    private static GameEngine StartAtForeignRoundPrompt(ContentRegistry registry)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 1, HumanRole = Role.Loyalist,
            ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false, MaxTurns = 4
        }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral);
        Accept(game, new SelectGeneralCommand(1, game.PendingDecision!.ValidContentIds.First(), game.Revision, game.PendingDecision.PromptId));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Fengji &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        return game;
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        string State(GameEngine value) => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, value.PlayerCount).Select(seat => SnapshotJson.Serialize(value.CreateSnapshot(seat))).ToArray(),
            Frames = JsonSerializer.Serialize(value.ResolutionStack),
            Events = value.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())).ToArray(),
            value.CardMovements
        });
        Require(State(game) == State(restored), "Cold recovery must reproduce every viewer, round choice, owning frame and physical movement.");
        return restored;
    }

    private static void Reach(GameEngine game, Func<PendingDecision, bool> expected)
    {
        for (var step = 0; step < 80; step++)
        {
            if (game.PendingDecision is { PlayerSeat: 1 } prompt)
            {
                if (expected(prompt)) return;
                throw new InvalidOperationException($"The round participant missed its boundary: {prompt.Kind}: {prompt.Prompt}");
            }
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed round fixture exceeded 80 real commands.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> expected)
    {
        var prompt = game.PendingDecision ?? throw new InvalidOperationException("The round owner lost its prompt.");
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(expected).Id, game.Revision));
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.ToString() ?? "The real round command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture(ContentRegistry source, int openingHandSize, IReadOnlyList<ContentDeckPhysicalCard>? stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("round-participant-fixture", new(1, 0, 0), []);

        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddSkill(source.GetSkill(Fengji));
            builder.AddSkill(source.GetSkill("ol:xuanhui"));
            builder.AddGeneral(source.Generals["ol:chen-deng"]);
            for (var seat = 0; seat < 3; seat++)
                builder.AddGeneral(source.Generals["ol:chen-deng"] with { Id = $"fixture:round-plain-{seat}" });
            builder.AddDeck(new("fixture:round-deck", "轮开始夹具", openingHandSize, 0, [])
            {
                PhysicalCards = stock ?? Enumerable.Repeat(new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 5), 32).ToArray()
            });
            builder.AddMode(new(Mode, "轮开始夹具", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:round-deck", GeneralCandidateCount: 4,
                GeneralPoolIds: ["ol:chen-deng", "fixture:round-plain-0", "fixture:round-plain-1", "fixture:round-plain-2"]));
        }
    }
}
