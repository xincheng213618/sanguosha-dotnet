using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicShicaiOriginalSlashChecks
{
    private const string XuYou = "classic:xu-you", Shicai = "classic:shicai";
    private const string CaoCao = "boundary:cao-cao-mobile", Jianxiong = "boundary:jianxiong-mobile";
    private const string Watch = "fixture:shicai-native-claim-child";
    private const string Claim = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";
    private const string Top = "skill-program.completed-top.place", Draw = "skill-program.completed-top.draw";
    private const string Mode = "identity:classic-shicai-original-slash", Deck = "fixture:shicai-original-slash-deck";
    private static readonly string[] Generals = [XuYou, CaoCao, "fixture:shicai-peer-2", "fixture:shicai-peer-3"];

    internal static void ShicaiCannotTopTheRediscardedOriginalSlashCold()
    {
        var (game, content) = Start();
        var slash = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]));
        var card = slash.CardId!.Value;
        Accept(game, new PlayCardCommand(0, card, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Jianxiong });
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);
        Require(use is { SourceSeat: 0, CardKind: CardKind.Slash, Action.Type: CardActionType.Use } &&
            use.Action.PhysicalCards.Single().CardId == card &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } && !Finished(game, use.Id, card),
            "Xu You's first real basic use is an original physical Slash still resolving during the Jianxiong offer.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, IsShicaiOffer);
        var original = control.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        var pileBefore = Pile(control);
        Require(Finished(control, use.Id, card) && original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile,
            "Normal native cleanup of this original Slash offers Shicai from its actual UseFinished entry.");
        Activate(control);
        Settle(control);
        var placed = control.CardMovements.Single(move => move.Reason.Value == Top);
        var drawn = control.CardMovements.Single(move => move.Reason.Value == Draw);
        Require(placed.CardId == card && placed.From == CardLocation.DiscardPile && placed.To == CardLocation.DrawPile &&
            placed.Sequence > original.Sequence && Pile(control)[0] == card && drawn.CardId == pileBefore[^1] &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && drawn.Sequence > placed.Sequence,
            "Shicai puts the original Slash on top and Cunmu really draws the previous bottom card once.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var delivery = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Claim);
        var originalClaim = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Single();
        Require(delivery.From == CardLocation.Processing && delivery.To == CardLocation.Hand(1) &&
            originalClaim is { Kind: NativeMaterialClaimKind.Delivery, RecipientSeat: 1 } && originalClaim.CardId == card &&
            originalClaim.MovementSequence == delivery.Sequence && !Finished(game, use.Id, card),
            "The real gain child pauses at Jianxiong's original native delivery, before the Slash completes.");

        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || IsShicaiOffer(prompt));
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(1) && newer.To == CardLocation.DiscardPile && newer.Sequence > delivery.Sequence &&
            newer.BatchId != delivery.BatchId && Finished(game, use.Id, card) &&
            game.CardMovements.All(move => move.CardId != card || move.Reason != CardMoveReasons.UseFinished),
            "The original Slash completes without writing a cleanup entry after this independent discard.");
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            game.CardMovements.All(move => move.Reason.Value is not (Top or Draw)) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Classic Shicai must leave entry {newer.Sequence} in discard and withhold its offer after original delivery {delivery.Sequence}.");
    }

    private static bool IsShicaiOffer(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Shicai };

    private static bool Finished(GameEngine game, long useId, int card) => Facts<CardUseFinishedEvent>(game)
        .Count(fact => fact.ResolutionId == useId && fact.CardKind == CardKind.Slash && fact.CardId == card) == 1;

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the decision and exact original use movements for all seats.");
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
        public PackageManifest Manifest { get; } = new("fixture-classic-shicai-original-slash", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ClaimantBuilder(builder), XuYou, CaoCao, "boundary:cao-cao");
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
            builder.AddMode(new(Mode, "恃才原杀", 4, 4, new Dictionary<string, int>
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
