using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShiDongZhaoYingjiaPreludeChecks
{
    private const string Hero = "mobile:shi-dong-zhao", Yingjia = "mobile:shi-yingjia";
    private const string Peer = "fixture:yingjia-peer", Deck = "fixture:yingjia-prelude-deck";
    private const string Cost = "skill.mobile-shi-dong-zhao.discard", StartDraw = "skill-program.mobile-shi-dong-zhao.draw";

    internal static void IssuedForeignExtraDrawPrecedesActualTurnCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(79, 1, "yingjia-prelude", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Heart, 8), new(CardKind.Dodge, Suit.Diamond, 4)], [],
            Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] }).ToArray(),
            [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(record => record.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The published Yingjia command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready(); step++)
            {
                Need(Prompt()?.PlayerSeat != 0 && game.State.Winner == Winner.None, "Yingjia reached an unexpected human choice.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "Yingjia did not reach its expected boundary.");
        }
        void Answer(PromptChoice choice) => Submit(new AnswerPromptCommand(0, Prompt()!.PromptId, choice.Id, game.Revision));
        void Play() => Reach(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        Submit(new StartGameCommand());
        Play();
        var firstRound = Facts<RoundStartedEvent>().Single();
        var tricks = game.CreateSnapshot(0).Players[0].Hand.Where(card => card.Kind == CardKind.DrawTwo).Select(card => card.Id).ToArray();
        Need(tricks.Length == 2, "The original hand has two physical cards of the same Trick name.");
        foreach (var card in tricks)
        {
            var action = game.GetHumanLegalActions().Single(item => item.CardId == card && item.ConversionSource is null);
            Submit(new PlayCardCommand(0, card, action.TargetSeats, game.Revision, Prompt()!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        Need(Facts<ActualTurnTrickUseRecordedEvent>().Count(fact => fact is { ActorSeat: 0, TurnNumber: 1, EffectiveKind: CardKind.DrawTwo }) == 2,
            "Two real same-name Trick uses qualify the original actual turn for Yingjia.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Yingjia &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(Prompt()!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "payment") == true);
        var payment = Prompt()!.Choices.First(choice => choice.Parameters.GetValueOrDefault("branch") == "payment");
        Answer(payment);
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "target") == true);
        var target = Prompt()!.Choices.Single(choice => choice.Targets.SequenceEqual([3]));
        var paid = Facts<MobileShiDongZhaoCostPaidEvent>().Single();
        Need(paid.CardId == payment.Cards.Single() && game.CardMovements.Count(move => move.CardId == paid.CardId &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile && move.Reason.Value == Cost) == 1 &&
            Facts<MobileShiDongZhaoExtraGrantedEvent>().Length == 0 && Facts<TurnStartedEvent>().Length == 1,
            "The original hand entity is paid while the choice of extra-turn recipient is still pending.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
            restored.CardMovements.SequenceEqual(game.CardMovements),
            "Cold replay preserves the paid cost, original frame and exact private recipient choice.");
        game = restored;
        Answer(target);
        Reach(() =>
        {
            if (game.ResolutionStack.OfType<BeiMiHuTurnPreludeFrame>().SingleOrDefault() is { } prelude)
                Need(!prelude.RoundAdvanced && prelude.Proof.ActualRoundNumber == firstRound.RoundNumber &&
                    prelude.Proof.UpcomingOwnerSeat == 3 && prelude.Proof.UpcomingTurnNumber == 2 && Facts<TurnStartedEvent>().Length == 1,
                    "Any published extra-turn prelude still belongs to the same round before the second actual start.");
            return Facts<TurnStartedEvent>().Any(fact => fact is { ActorSeat: 3, TurnNumber: 2 });
        });

        var grant = Facts<MobileShiDongZhaoExtraGrantedEvent>().Single();
        var enteredFact = game.Events.Single(item => item.Payload is MobileShiDongZhaoExtraEnteredEvent);
        var entered = (MobileShiDongZhaoExtraEnteredEvent)enteredFact.Payload;
        var actualStart = game.Events.Single(item => item.Payload is TurnStartedEvent { ActorSeat: 3, TurnNumber: 2 });
        var queuedExtra = Facts<ProgramExtraTurnPendedEvent>().Single(item => item.SkillId == Yingjia);
        var draws = game.CardMovements.Where(move => move.Reason.Value == StartDraw).ToArray();
        Need(grant is { TargetSeat: 3, IssuedTurn: 1, Source.OwnerSeat: 0 } && grant.FrameId == paid.FrameId &&
            queuedExtra.FrameId == grant.FrameId && queuedExtra.Seat == 3 && entered.TargetSeat == 3 &&
            entered.GrantFrameId == grant.FrameId && entered.UpcomingTurn == 2 && enteredFact.Sequence < actualStart.Sequence &&
            game.CreateSnapshot(3).Players[3].GeneralId == Peer && Facts<RoundStartedEvent>().SequenceEqual([firstRound]) &&
            draws.Length == 2 && draws.Select(move => move.CardId).Distinct().Count() == 2 && draws.All(move =>
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(3) && move.TurnNumber == grant.IssuedTurn),
            "The issued right starts a foreign extra turn and pays two physical draws in the original round before it actually starts.");
        Reach(() => Facts<TurnStartedEvent>().Length == 3);
        Need(Facts<TurnStartedEvent>().Select(fact => fact.ActorSeat).SequenceEqual([0, 3, 1]) &&
            Facts<MobileShiDongZhaoCostPaidEvent>().SequenceEqual([paid]) && Facts<MobileShiDongZhaoExtraEnteredEvent>().SequenceEqual([entered]),
            "The extra turn returns to the normal successor without paying the original cost or entering the grant again.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-yingjia-prelude", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "固定额外回合受领者", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "迎驾固定牌堆", 0, 0, [new("standard:dodge", 36)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "迎驾额外回合", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
