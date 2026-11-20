using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicZhongyongOriginalSlashChecks
{
    private const string ZhouCang = "classic:zhou-cang", Zhongyong = "classic:zhongyong";
    private const string CaoCao = "boundary:cao-cao-mobile", Jianxiong = "boundary:jianxiong-mobile";
    private const string Watch = "fixture:zhongyong-slash-claim-child";
    private const string Claim = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";
    private const string Gift = "skill-program.classic:zhongyong.completed-card-gift";
    private const string Mode = "identity:classic-zhongyong-original-slash", Deck = "fixture:zhongyong-original-slash-deck";
    private static readonly string[] Generals = [ZhouCang, CaoCao, "fixture:zhongyong-slash-recipient", "fixture:zhongyong-slash-peer"];

    internal static void ZhongyongCannotGiftTheRediscardedOriginalSlashCold()
    {
        var (game, content) = Start();
        var slash = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]));
        var card = slash.CardId!.Value;
        Accept(game, new PlayCardCommand(0, card, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        ReachJianxiong(game);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);
        Require(use is { SourceSeat: 0, Action.Type: CardActionType.Use } && use.Action.PhysicalCards.Single().CardId == card &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            !Finished(game, use.Id, card), "The native Slash actually deals damage and offers the earlier claim before it finishes.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Zhongyong });
        var original = control.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        Require(original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile && Finished(control, use.Id, card) &&
            P(control)!.Choices.Any(choice => Gives(choice, card)) &&
            game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Count == 0,
            "Without the earlier claim, the completed original Slash leaves its real cleanup entry for the private gift.");
        Answer(control, choice => Gives(choice, card));
        Settle(control);
        var gifted = Facts<CompletedCardGiftedEvent>(control).Single();
        var gift = control.CardMovements.Single(move => move.Reason.Value == Gift);
        Require(gifted is { OwnerSeat: 0, RecipientSeat: 2, IsRed: false } && gifted.CardActionId == use.Action!.ActionId &&
            gifted.CardIds.SequenceEqual([card]) && gift.CardId == card && gift.From == CardLocation.DiscardPile &&
            gift.To == CardLocation.Hand(2) && gift.Sequence > original.Sequence,
            "The real black Slash is given once to the chosen recipient through its original use.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var delivery = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Claim);
        var material = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Single();
        Require(delivery.From == CardLocation.Processing && delivery.To == CardLocation.Hand(1) &&
            material is { Kind: NativeMaterialClaimKind.Delivery, RecipientSeat: 1 } && material.CardId == card &&
            material.MovementSequence == delivery.Sequence && !Finished(game, use.Id, card),
            "The genuine Jianxiong gain child interrupts the original Slash after its exact native material delivery.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == Zhongyong);
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(1) && newer.To == CardLocation.DiscardPile && newer.Sequence > delivery.Sequence &&
            newer.BatchId != delivery.BatchId && Finished(game, use.Id, card) &&
            game.CardMovements.All(move => move.CardId != card || move.Reason != CardMoveReasons.UseFinished),
            "The original Slash finishes without reclaiming the delivery, and the later true discard is the only entry into discard.");

        if (P(game)?.SkillPrompt?.SkillId == Zhongyong)
        {
            Require(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zhongyong)
                .WindowContext?.CardUse?.ParentCardUseFrameId == use.Id, "Any gift offer still belongs to the completed original Slash.");
            Answer(game, choice => Gives(choice, card));
        }
        Settle(game);
        Require(!Facts<CompletedCardGiftedEvent>(game).Any() && game.CardMovements.All(move => move.Reason.Value != Gift) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Classic Zhongyong must leave entry {newer.Sequence} in discard after the original Slash delivered that entity at {delivery.Sequence}.");
    }

    private static bool Gives(PromptChoice choice, int card) => choice.Parameters.GetValueOrDefault("gift-option") == "give" &&
        choice.Cards.SequenceEqual([card]) && choice.Targets.SequenceEqual([2]);

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static bool Finished(GameEngine game, long use, int card) => Facts<CardUseFinishedEvent>(game)
        .Count(fact => fact.ResolutionId == use && fact.CardKind == CardKind.Slash && fact.CardId == card) == 1;

    private static void ReachJianxiong(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Jianxiong })
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} before Jianxiong.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not reach the earlier claim offer.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the pending decision and exact original movements in all seat views.");
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
        public PackageManifest Manifest { get; } = new("fixture-classic-zhongyong-original-slash", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ClaimantBuilder(builder), ZhouCang, CaoCao, "boundary:cao-cao");
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"discard-after-native-claim","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{{Claim}}}"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "取得后弃置", description = "奸雄获得伤害牌后，弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体黑杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "忠勇原杀", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class ClaimantBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == CaoCao ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Watch] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}
