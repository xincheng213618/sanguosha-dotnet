using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class OlDuFuRenShunshiNumericChecks
{
    private const string Hero = "ol:du-fu-ren", Shunshi = "ol:shunshi";
    private const string Peer = "fixture:shunshi-numeric-peer", Deck = "fixture:shunshi-numeric-deck";

    internal static void PreparationGiftFundsThreeActualPhasesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Shunshi } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var opening = game.CreateSnapshot(0).Players[0];
        var giftCard = opening.Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var slashes = opening.Hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id).ToArray();
        Require(opening.Hp == 3 && opening.Hand.Count == 4 && slashes.Length == 3,
            "The actual Du Fu Ren starts preparation with three physical Slashes and one card to give.");
        var preparation = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
            .Single(frame => frame.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow);
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Shunshi } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "gift"));
        var original = Facts<DuFuRenStartedEvent>(game).Single();
        Require(original.WindowId == preparation.Id && original.Source is { SkillId: Shunshi, OwnerSeat: 0 } &&
            original.TurnOwner == 0 && original.DamageFrameId is null && Facts<DuFuRenGiftIssuedEvent>(game).Length == 0,
            "The private gift belongs to the original preparation window and has no paid phase benefits yet.");
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "gift" &&
            choice.Cards.SequenceEqual([giftCard]) && choice.Targets.SequenceEqual([2]));
        Settle(game);

        var gift = Facts<DuFuRenGiftIssuedEvent>(game).Single();
        var draw = PhaseStart(game, original.Turn, ActualDiscardRecoveryPhaseKind.Draw);
        var play = PhaseStart(game, original.Turn, ActualDiscardRecoveryPhaseKind.Play);
        Require(gift.FrameId == original.FrameId && gift.Source == original.Source && gift.RecipientSeat == 2 && gift.CardId == giftCard &&
            game.CardMovements.Count(move => move.Sequence == gift.MovementSequence && move.CardId == giftCard &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(2) && move.Reason.Value == "skill-program.du-fu-ren.shunshi.gift") == 1 &&
            game.CardMovements.Count(move => move.Sequence > draw.MovementSequenceBefore && move.Sequence <= play.MovementSequenceBefore &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw) == 3 &&
            Facts<DuFuRenPhaseIssuedEvent>(game).SequenceEqual([new(gift.FrameId, draw.Phase), new(gift.FrameId, play.Phase)]),
            "One actual gift gives three physical cards in the next normal Draw and one allowance in the original Play.");

        foreach (var slash in slashes.Take(2))
        {
            Require(game.GetLegalActions(0).Any(action => action.CardId == slash && action.Kind == LegalActionKind.Slash &&
                action.ConversionSource is null && action.TargetSeats.SequenceEqual([1])),
                "The original hand Slash has an ordinary legal action against the same adjacent target.");
            Accept(game, new PlayCardCommand(0, slash, [1], game.Revision, P(game)!.PromptId, CardKind.Slash));
            Settle(game);

            var use = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash && fact.SourceSeat == 0);
            var action = Facts<CardActionAcceptedEvent>(game).Single(fact => fact.Action is
                { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash } &&
                fact.Action.PhysicalCards.Count == 1 && fact.Action.PhysicalCards[0].CardId == slash).Action;
            Require(action.ConversionChain.Count == 0 && action.TargetSeats.SequenceEqual([1]) &&
                Facts<CardUseDebitRecordedEvent>(game).Count(fact => fact.Debit.CardActionId == action.ActionId &&
                    fact.Debit is { ActorSeat: 0, Phase: TurnPhase.Play, Query: SkillRuleQuery.SlashLimit } && fact.Debit.TurnNumber == original.Turn) == 1 &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.ResolutionId && fact.CardId == slash && fact.CardKind == CardKind.Slash) == 1 &&
                game.CardMovements.Count(move => move.CardId == slash && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1,
                "Each admitted Slash pays one physical hand card, debits the ordinary limit and completes once.");
        }

        var handBeforeDiscard = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).Order().ToArray();
        Require(handBeforeDiscard.Length == 4 && handBeforeDiscard.Contains(slashes[2]) &&
            game.GetLegalActions(0).All(action => action.CardId != slashes[2]),
            "The third physical Slash remains in hand after both ordinary allowances have been spent.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        bool OriginalTurnEnded() => Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == original.Turn);
        for (var step = 0; step < 35 && !OriginalTurnEnded() && P(game) is not { PlayerSeat: 0, Kind: DecisionKind.DiscardCards }; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));

        var discard = PhaseStart(game, original.Turn, ActualDiscardRecoveryPhaseKind.Discard);
        var afterDiscard = game.CreateSnapshot(0).Players[0];
        Require(OriginalTurnEnded() && afterDiscard.Hp == 3 && afterDiscard.Hand.Select(card => card.Id).Order().SequenceEqual(handBeforeDiscard) &&
            game.CardMovements.All(move => move.TurnNumber != original.Turn || move.From != CardLocation.Hand(0) || move.Reason != CardMoveReasons.HandLimitDiscard) &&
            Facts<DuFuRenGiftIssuedEvent>(game).SequenceEqual([gift]) && Facts<DuFuRenPhaseIssuedEvent>(game).SequenceEqual(
                [new(gift.FrameId, draw.Phase), new(gift.FrameId, play.Phase), new(gift.FrameId, discard.Phase)]),
            "The actual Discard grants the third independent benefit and keeps the same four hand cards at three HP.");
        _ = Cold(game, content);
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static ActualDiscardRecoveryPhaseStartedEvent PhaseStart(GameEngine game, int turn, ActualDiscardRecoveryPhaseKind kind) =>
        Facts<ActualDiscardRecoveryPhaseStartedEvent>(game).Single(fact => fact.Phase.ActorSeat == 0 &&
            fact.Phase.ActualTurnOwnerSeat == 0 && fact.Phase.TurnNumber == turn && fact.Phase.Kind == kind);

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Facts<DuFuRenGiftIssuedEvent>(restored).SequenceEqual(Facts<DuFuRenGiftIssuedEvent>(game)) &&
            Facts<DuFuRenPhaseIssuedEvent>(restored).SequenceEqual(Facts<DuFuRenPhaseIssuedEvent>(game)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold command replay preserves the real gift, phase issuances, entity movement and seat-specific choices.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        ShanheStartingCard[] opening = [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Slash, Suit.Spade, 3),
            new(CardKind.Slash, Suit.Club, 4), new(CardKind.Slash, Suit.Diamond, 5)];
        var setup = new ShanheBattleSetup(31, 1, "shunshi-actual-phases", Hero, 3, 0, 0, 0, 0, [], opening, [],
            Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-du-fu-ren-shunshi-numeric", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "固定顺世实体牌", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 32).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "顺世真实阶段", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
