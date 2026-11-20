using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlChengGongYingKuangxiangDrawChecks
{
    private const string General = "ol:cheng-gong-ying", Skill = "ol:kuangxiang";
    private const string Recipient = "fixture:kuangxiang-draw-recipient", Deck = "fixture:kuangxiang-draw-deck";
    private const string AdjustmentReason = "skill-program.cheng-gong-ying.kuangxiang.draw";

    internal static void OneRedGrantAffectsOnlyRecipientsNextActualDrawCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "kuangxiang-draw", General, 4, 0, 0, 0, 0, [], [], [],
            [new ShanheCombatantDefinition(Recipient, Hp: 8)
            {
                StartingHand = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4)]
            }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, content);
        Submit(game, new StartGameCommand());
        var preparation = Preparation(game);
        var sourceTurn = Events<TurnStartedEvent>(game).Single(fact => fact.ActorSeat == 0).TurnNumber;
        Need(game.CreateSnapshot(0).Players[1].HandCount == 3, "The real target has exactly three hand cards before Kuangxiang.");
        Answer(game, preparation, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, () => Prompt(game) is { SkillPrompt.SkillId: Skill } prompt &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "target"));
        Answer(game, Prompt(game)!, choice => choice.Targets.SequenceEqual([1]));
        var play = Play(game);

        var started = Events<ChengGongYingStartedEvent>(game).Single();
        var issued = Events<KuangxiangRedDrawIssuedEvent>(game).Single();
        var red = Events<KuangxiangCardObtainedEvent>(game).Single();
        var adjustment = game.CardMovements.Single(move => move.Sequence == red.MovementSequence);
        Need(started is { Source.SkillId: Skill, Source.OwnerSeat: 0, TurnOwner: 0 } &&
             started.Turn == sourceTurn && issued is { Recipient: 1, Count: 1 } &&
             red is { Recipient: 1, EffectiveSuit: Suit.Heart } && red.FrameId == started.FrameId &&
             red.Source == started.Source && issued.FrameId == started.FrameId &&
             adjustment.CardId == red.CardId && adjustment.From == CardLocation.DrawPile &&
             adjustment.To == CardLocation.Hand(1) && adjustment.Reason.Value == AdjustmentReason &&
             game.CreateSnapshot(1).Players[1].HandCount == 4,
            "The real Preparation gives the other seat one red draw entity and issues exactly one original adjustment.");
        Need(ActualDraws(game, sourceTurn, 0) == 2 && !Events<KuangxiangRedDrawConsumedEvent>(game).Any(),
            "The issuer's ordinary Draw still moves two cards while the recipient's payment is outstanding.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(Play(game).PromptId == play.PromptId && Events<KuangxiangRedDrawIssuedEvent>(game).Single() == issued &&
             Events<KuangxiangCardObtainedEvent>(game).Single() == red && !Events<KuangxiangRedDrawConsumedEvent>(game).Any(),
            "Cold replay restores the same outstanding grant and the original red entity before the recipient's actual Draw.");
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        Reach(game, () => RecipientPlayCount(game) == 1);
        var firstTurn = Events<TurnStartedEvent>(game).Single(fact => fact.ActorSeat == 1).TurnNumber;
        var firstPhase = DrawPhase(game, firstTurn);
        var consumed = Events<KuangxiangRedDrawConsumedEvent>(game).Single();
        Need(firstTurn > sourceTurn && firstPhase.Token > issued.LastActualPhaseToken &&
             consumed is { Recipient: 1, Count: 1 } && consumed.GrantFrameId == issued.FrameId &&
             consumed.Phase == firstPhase && ActualDraws(game, firstTurn, 1) == 1,
            "The next actual recipient Draw consumes this grant on its exact phase and moves only one normal card.");

        var nextPreparation = Preparation(game);
        Answer(game, nextPreparation, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
        var secondPlay = Play(game);
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, secondPlay.PromptId));
        Reach(game, () => RecipientPlayCount(game) == 2);
        var secondTurn = Events<TurnStartedEvent>(game).Last(fact => fact.ActorSeat == 1).TurnNumber;
        var secondPhase = DrawPhase(game, secondTurn);
        Need(secondTurn > firstTurn && secondPhase.Token > firstPhase.Token && ActualDraws(game, secondTurn, 1) == 2 &&
             Events<KuangxiangRedDrawIssuedEvent>(game).Single() == issued &&
             Events<KuangxiangRedDrawConsumedEvent>(game).Single() == consumed &&
             game.CardMovements.Count(move => move.Reason.Value == AdjustmentReason) == 1,
            "The same recipient's second actual Draw returns to two moves without a second grant, adjustment or consumption.");
    }

    private static PendingDecision? Prompt(GameEngine game) => game.CreateSnapshot(0).PendingDecision;

    private static PendingDecision Preparation(GameEngine game)
    {
        Reach(game, () => Prompt(game) is { SkillPrompt.SkillId: Skill } prompt &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        return Prompt(game)!;
    }

    private static PendingDecision Play(GameEngine game)
    {
        Reach(game, () => Prompt(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0);
        return Prompt(game)!;
    }

    private static ActualDiscardRecoveryPhaseKey DrawPhase(GameEngine game, int turn) =>
        Events<ActualDiscardRecoveryPhaseStartedEvent>(game).Select(fact => fact.Phase).Single(phase =>
            phase is { ActualTurnOwnerSeat: 1, ActorSeat: 1, Kind: ActualDiscardRecoveryPhaseKind.Draw } && phase.TurnNumber == turn);

    private static int ActualDraws(GameEngine game, int turn, int seat) =>
        game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw);

    private static int RecipientPlayCount(GameEngine game) =>
        Events<PhaseChangedEvent>(game).Count(fact => fact is { ActorSeat: 1, Phase: TurnPhase.Play });

    private static IEnumerable<T> Events<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 140; step++)
        {
            if (ready())
                return;
            var prompt = Prompt(game);
            Need(game.State.Winner == Winner.None && (prompt is not { PlayerSeat: 0 } ||
                 prompt.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count > 0),
                $"Unexpected Kuangxiang boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}, prompt {prompt?.Kind}, skill {prompt?.SkillPrompt?.SkillId}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Kuangxiang fixture did not reach the required prompt or actual phase.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> pick) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The Kuangxiang fixture command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:kuangxiang-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, General);
            builder.AddGeneral(new(Recipient, "匡襄受领者", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定红牌的普通摸牌", 0, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 24)
                    .Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "匡襄实际摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [General, Recipient], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
