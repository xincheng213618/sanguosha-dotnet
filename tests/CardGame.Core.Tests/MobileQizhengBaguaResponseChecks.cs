using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileQizhengBaguaResponseChecks
{
    private const string Mode = "identity:classic-mobile-qizheng-bagua", Deck = "fixture:mobile-qizheng-bagua-deck";
    private const string Bagua = "standard:bagua", Qizheng = "mobile:qizheng-xiangsheng";
    private static readonly string[] Generals = ["fixture:qizheng-bagua-target", "fixture:qizheng-bagua-actor",
        "fixture:qizheng-bagua-peer-2", "fixture:qizheng-bagua-peer-3"];

    public static void QizhengRegularAllowsActualBaguaPlayedDodgeCold()
    {
        CheckJudgment(Suit.Heart);
        CheckJudgment(Suit.Spade);
    }

    private static void CheckJudgment(Suit judgmentSuit)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(judgmentSuit));
        var game = Start(content);
        var armor = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.BaguaFormation).Id;
        Accept(game, new PlayCardCommand(0, armor, [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var actual = game.GetLegalActions(1).First(action => action.Kind == LegalActionKind.QizhengXiangsheng &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([0]));
        Accept(game, new PlayCardCommand(1, actual.CardId!.Value, actual.TargetSeats, game.Revision, P(game)!.PromptId, actual.PlayedCardKind));
        Reach(game, prompt => IsQizheng(prompt, 1) && prompt.Choices.Any(choice => Branch(choice) == "designation"));
        Answer(game, choice => Branch(choice) == "designation" && choice.Parameters.GetValueOrDefault("value") == "regular");
        Reach(game, prompt => IsQizheng(prompt, 0) && prompt.Choices.Any(choice => Branch(choice) == "pass"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == actual.CardId);
        var original = Facts<QizhengEffectStartedEvent>(game).Single();
        Require(use is { SourceSeat: 1, CardKind: CardKind.QizhengXiangsheng } && use.TargetSeats.SequenceEqual([0]) &&
            original.UseId == use.Id && original.Target == 0 && game.CreateSnapshot(0).Players[0].Equipment.Single().Id == armor &&
            game.CreateSnapshot(1).PendingDecision is null && !Facts<JudgmentRequestedEvent>(game).Any() && !Facts<QizhengResolvedEvent>(game).Any(),
            "The equipped target is privately answering a real physical Qizheng whose secret regular designation is still sealed.");
        game = Cold(game, content);

        var succeeds = judgmentSuit == Suit.Heart;
        if (succeeds)
        {
            var decline = Cold(game, content);
            Answer(decline, choice => Branch(choice) == "pass");
            Reach(decline, prompt => IsQizheng(prompt, 1) && prompt.Choices.Any(choice => Branch(choice) == "hand"));
            Answer(decline, choice => Branch(choice) == "hand");
            Reach(decline, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            Require(decline.CardMovements.Single(move => move.Reason.Value == "card.effect.qizheng.obtain") is { } gain &&
                gain.From == CardLocation.Hand(0) && gain.To == CardLocation.Hand(1) &&
                Facts<QizhengResolvedEvent>(decline).Single() is { Ambush: false, ResponseKind: null },
                "Without a played Dodge the regular designation really claims one original target card.");
        }

        Require(P(game)!.Choices.Any(choice => Branch(choice) == "bagua" && choice.Cards.Count == 0),
            "The original Qizheng response must offer the real equipped Bagua to try a zero-material played Dodge.");
        Answer(game, choice => Branch(choice) == "bagua");
        int? fallbackCardId = null;
        if (!succeeds)
        {
            Reach(game, prompt => IsQizheng(prompt, 0) && prompt.Choices.Any(choice => Branch(choice) == "pass"));
            var fallback = P(game)!.Choices.First(choice => Branch(choice) == "response" && choice.Parameters.GetValueOrDefault("response") == "dodge");
            fallbackCardId = fallback.Cards.Single();
            Require(P(game)!.Choices.All(choice => Branch(choice) != "bagua") && game.CreateSnapshot(1).PendingDecision is null &&
                !Facts<QizhengResponsePaidEvent>(game).Any() && !Facts<QizhengResolvedEvent>(game).Any(),
                "A black Bagua judgment returns to the original private response, with a physical Dodge still possible and no second Bagua.");
            game = Cold(game, content);
            Answer(game, choice => choice.Id == fallback.Id);
        }

        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var outcome = Facts<JudgmentResolvedEvent>(game).Single(entry => entry.Reason == JudgmentReasons.BaguaDefense);
        Require(outcome is { TargetSeat: 0, CardId: > 0 } && outcome.Succeeded == succeeds && outcome.Suit == judgmentSuit && outcome.ParentResolutionId == use.Id &&
            Facts<CardActionAcceptedEvent>(game).Count(entry => entry.Action is { Type: CardActionType.Response, ActorSeat: 0, ProviderSeat: 0, EffectiveKind: CardKind.Dodge } response &&
                response.ParentActionId == use.Action!.ActionId && (fallbackCardId is { } cardId
                    ? response.PhysicalCards.Select(cost => cost.CardId).SequenceEqual([cardId]) : response.PhysicalCards.Count == 0)) == 1 &&
            Facts<QizhengResponsePaidEvent>(game).Single() is { Kind: CardKind.Dodge } paid && paid.UseId == use.Id &&
            Facts<QizhengResolvedEvent>(game).Single() is { Ambush: false, ResponseKind: CardKind.Dodge } resolved && resolved.UseId == use.Id &&
            game.CardMovements.All(move => move.Reason.Value != "card.effect.qizheng.obtain") &&
            game.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == armor) &&
            Facts<CardUseFinishedEvent>(game).Count(entry => entry.ResolutionId == use.Id) == 1,
            "Bagua judges on the original use, and only the red judgment or the actual fallback Dodge prevents the regular claim.");
    }

    private static bool IsQizheng(PendingDecision prompt, int seat) => prompt is { Kind: DecisionKind.ProgramTrigger,
        IncomingCard: CardKind.QizhengXiangsheng, IsPrivate: true } && prompt.PlayerSeat == seat;
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the private designation and each player's actual response view.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        for (var seed = 43; seed < 103; seed++)
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 4, ModeId = Mode,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
            Accept(game, new StartGameCommand());
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
            {
                Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
                Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            }
            var hands = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).Players[seat].Hand).ToArray();
            if (hands[0].Any(card => card.Kind == CardKind.BaguaFormation) && hands[1].Any(card => card.Kind == CardKind.QizhengXiangsheng) &&
                hands.SelectMany(hand => hand).Count(card => card.Kind is CardKind.BaguaFormation or CardKind.QizhengXiangsheng) == 4)
                return game;
        }
        throw new InvalidOperationException("The fixture needs a real Bagua at seat zero and Qizheng at seat one, leaving only the chosen judgment suit in the draw pile.");
    }

    private sealed class Fixture(Suit judgmentSuit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-qizheng-bagua", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var general in Generals) builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "八卦、奇正与固定花色判定牌", 11, 0, []) { PhysicalCards =
                [new(Bagua, Suit.Spade, 2), new(Bagua, Suit.Club, 2), new(Qizheng, Suit.Spade, 4), new(Qizheng, Suit.Club, 3),
                    .. Enumerable.Range(0, 48).Select(index => new ContentDeckPhysicalCard("standard:dodge", judgmentSuit, index % 13 + 1))] });
            builder.AddMode(new(Mode, "奇正真实八卦响应", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
