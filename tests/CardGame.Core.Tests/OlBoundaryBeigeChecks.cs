using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlBoundaryBeigeChecks
{
    private const string Hero = "boundary:cai-wen-ji", Beige = "boundary:beige", Claim = "fixture:beige-earlier-claim";
    private const string Mode = "identity:classic-beige-payment-entry", Deck = "fixture:beige-deck";
    private const string Payment = "skill-program.damage-judgment-suit.payment";
    private const string Refund = "skill-program.damage-judgment-suit.reclaim-payment";
    private const string Take = "skill-program.fixture:beige-earlier-claim.ClaimMovedCards";
    private const string Rediscard = "skill-program.fixture:beige-earlier-claim.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:beige-attacker", Hero, "fixture:beige-claimant", "fixture:beige-victim"];

    internal static void BeigeRefundsOnlyTheOriginalMatchingRankPaymentCold()
    {
        var (game, content) = Start();
        var slash = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        ReachBeigeOffer(game);
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Beige } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("payment-kind") == "owner-payment"));
        var judgment = Facts<JudgmentResolvedEvent>(game).Single();
        var hand = game.CreateSnapshot(1).Players[1].Hand;
        var material = hand.FirstOrDefault(card => card.Rank == judgment.Rank && card.Suit != judgment.Suit);
        Require(judgment is { TargetSeat: 3, Reason: "skill.boundary.beige", Rank: 7 } && material is not null,
            $"The real damage judgment must offer a same-rank different-suit hand payment. Judgment {judgment.Suit}/{judgment.Rank}; hand {string.Join(", ", hand.Select(card => $"{card.Suit}/{card.Rank}"))}.");
        var cardId = material!.Id;
        Answer(game, choice => choice.Cards.SequenceEqual([cardId]));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => Action(choice) == "activate"));

        var paid = Facts<ProgramJudgmentSuitPaymentCommittedEvent>(game).Single();
        var original = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Payment);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == paid.FrameId);
        Require(paid is { OwnerSeat: 1, RankMatched: true, SuitMatched: false } && paid.JudgmentFrameId == judgment.ResolutionId &&
            paid.PaidCardId == cardId && paid.PaidMovementSequence == original.Sequence &&
            original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile &&
            frame.DamageJudgmentSuitPayment is { Stage: ProgramDamageJudgmentPaymentStage.PaymentChildren } draft &&
            draft.PaidMovementSequence == original.Sequence && draft.PaidTo == CardLocation.DiscardPile &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 3, Amount: 1 },
            "One actual Slash damage and judgment produce an exact original rank-matching payment before the earlier claimant acts.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        var returned = Facts<ProgramJudgmentSuitPaymentCardClaimedEvent>(control).Single();
        var refund = control.CardMovements.Single(move => move.Reason.Value == Refund);
        Require(returned is { IsJudgmentCard: false } && returned.FrameId == paid.FrameId && returned.CardId == cardId &&
            returned.MovementSequence == refund.Sequence && refund.From == CardLocation.DiscardPile && refund.To == CardLocation.Hand(1) &&
            refund.Sequence > original.Sequence && Facts<ProgramJudgmentSuitPaymentCommittedEvent>(control).SequenceEqual([paid]),
            "If the exact payment remains at its original entry, Beige really returns the paid card once.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([cardId])));
        var recovered = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Take);
        Require(recovered.From == CardLocation.DiscardPile && recovered.To == CardLocation.Hand(2) && recovered.Sequence > original.Sequence,
            "The earlier movement child actually collects the payment before the public re-discard choice.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([cardId]));
        Settle(game);
        var newer = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(2) && newer.To == CardLocation.DiscardPile && newer.BatchId != original.BatchId &&
            newer.Sequence > recovered.Sequence && Facts<ProgramJudgmentSuitPaymentCommittedEvent>(game).SequenceEqual([paid]),
            "The actual re-discard creates a separate entry without authorizing or paying Beige again.");
        Require(!Facts<ProgramJudgmentSuitPaymentCardClaimedEvent>(game).Any() && game.CardMovements.All(move => move.Reason.Value != Refund) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location == CardLocation.DiscardPile,
            $"Beige cannot refund the newer entry {newer.Sequence} through the original payment at entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void ReachBeigeOffer(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Beige } && prompt.Choices.Any(choice => Action(choice) == "activate"))
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} before the actual damage offer.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not reach the Beige damage window.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, 4).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original damage, payment and all seat views of the pending decision.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
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
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-beige-payment-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-refund","window":"discardPileReceived",
                    "subject":"owner","discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Payment}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Claim] = new { name = "先领再弃", description = "领取悲歌弃牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Claim, catalog.Presentations[Claim].Name, catalog.Presentations[Claim].Description)
                { Program = catalog.Programs[Claim], ProgramPresentation = catalog.Presentations[Claim] });
            builder.AddGeneral(new(Generals[0], "打出杀者", "supporter", "standard:none", "qun", 8));
            builder.AddGeneral(new(Generals[2], "先行领取者", "supporter", Claim, "qun", 8));
            builder.AddGeneral(new(Generals[3], "受伤者", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定点数的两色杀", 6, 0, []) { PhysicalCards = Enumerable.Range(0, 36)
                .Select(index => new ContentDeckPhysicalCard("standard:slash", index % 2 == 0 ? Suit.Spade : Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "悲歌原付款", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
