using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicJuxiangOriginalBarbarianChecks
{
    private const string CaoCao = "boundary:cao-cao-mobile", Jianxiong = "boundary:jianxiong-mobile";
    private const string ZhuRong = "classic:zhu-rong", Source = "fixture:juxiang-source", Peer = "fixture:juxiang-peer";
    private const string Juxiang = "classic:juxiang";
    private const string Watch = "fixture:juxiang-native-claim-child";
    private const string Claim = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";
    private const string Mode = "identity:classic-juxiang-original-barbarian", Deck = "fixture:juxiang-original-barbarian-deck";
    private static readonly string[] Generals = [Source, CaoCao, ZhuRong, Peer];

    internal static void JuxiangCannotClaimTheRediscardedOriginalBarbarianCold()
    {
        var (game, content) = Start();
        var barbarian = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.BarbarianAssault);
        var card = barbarian.CardId!.Value;
        var claimantHp = game.CreateSnapshot(2).Players[2].Hp;
        Accept(game, new PlayCardCommand(0, card, barbarian.TargetSeats, game.Revision, P(game)!.PromptId, barbarian.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Jianxiong });

        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);
        Require(use is { SourceSeat: 0, CardKind: CardKind.BarbarianAssault, Action.Type: CardActionType.Use } &&
            use.Action.PhysicalCards.Single().CardId == card && Facts<GroupCardUsedEvent>(game).Single().ResolutionId == use.Id &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } && !Finished(game, use.Id, card),
            "The original native Barbarian deals actual damage and offers Jianxiong during the group use.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        var original = control.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        var gain = control.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.JuxiangGain);
        Require(Finished(control, use.Id, card) && original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            gain.Sequence > original.Sequence && gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(2) &&
            Facts<JuxiangCardClaimedEvent>(control).Single() is { OwnerSeat: 2 } received && received.ResolutionId == use.Id &&
            received.CardIds.SequenceEqual([card]) && control.CreateSnapshot(2).Players[2].Hp == claimantHp,
            "The untouched Barbarian finishes, enters discard on its own cleanup and is then obtained once by the immune Zhu Rong.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var delivery = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Claim);
        var originalClaim = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Single();
        Require(delivery.From == CardLocation.Processing && delivery.To == CardLocation.Hand(1) &&
            originalClaim is { Kind: NativeMaterialClaimKind.Delivery, RecipientSeat: 1 } && originalClaim.CardId == card &&
            originalClaim.MovementSequence == delivery.Sequence && !Finished(game, use.Id, card),
            "Jianxiong already delivered the original physical material while its group use continues under the real gain child.");

        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Settle(game);
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(1) && newer.To == CardLocation.DiscardPile && newer.Sequence > delivery.Sequence &&
            newer.BatchId != delivery.BatchId && Finished(game, use.Id, card) && game.CreateSnapshot(2).Players[2].Hp == claimantHp &&
            game.CardMovements.All(move => move.CardId != card || move.Reason != CardMoveReasons.UseFinished),
            "The original group use still finishes after the real later discard without moving the material in its own cleanup.");
        Require(!Facts<JuxiangCardClaimedEvent>(game).Any() && game.CardMovements.All(move => move.Reason != CardMoveReasons.JuxiangGain) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Classic Juxiang must leave entry {newer.Sequence} in discard after this Barbarian delivered the entity at {delivery.Sequence}.");
    }

    internal static void JuxiangSkipsTheActualUserAndReachesTheEligibleClaimantCold()
    {
        var (game, content) = Start(sourceHasJuxiang: true);
        var source = game.CreateSnapshot(0).Players[0];
        var claimant = game.CreateSnapshot(2).Players[2];
        Require(source.Skills?.Any(skill => skill.Id == Juxiang) == true && claimant.Skills?.Any(skill => skill.Id == Juxiang) == true,
            "Both the lower-numbered actual user and the other living character have the real Juxiang skill.");
        var barbarian = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.BarbarianAssault);
        var card = barbarian.CardId!.Value;
        Accept(game, new PlayCardCommand(0, card, barbarian.TargetSeats, game.Revision, P(game)!.PromptId, barbarian.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Jianxiong });
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);
        Require(use is { SourceSeat: 0, CardKind: CardKind.BarbarianAssault, Action.Type: CardActionType.Use } &&
            use.Action.PhysicalCards.Single().CardId == card && use.MaterialClaims.Count == 0,
            "The lower-numbered Juxiang holder is the actual user of the native physical Barbarian Assault.");
        game = Cold(game, content);
        Skip(game);
        Settle(game);
        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        Require(Finished(game, use.Id, card) && original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            game.CreateSnapshot(2).Players[2].Hp == claimant.Hp,
            "The original physical Barbarian is fully resolved and discarded, and the other Juxiang holder remains immune and alive.");
        Require(Facts<JuxiangCardClaimedEvent>(game).ToArray() is [{ OwnerSeat: 2 } claim] && claim.ResolutionId == use.Id &&
            claim.CardIds.SequenceEqual([card]) && game.CardMovements.Where(move => move.CardId == card && move.Reason == CardMoveReasons.JuxiangGain).ToArray() is [var gain] &&
            gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(2) && gain.Sequence > original.Sequence,
            "The actual user cannot claim their own Barbarian or block the other living and eligible holder from claiming its exact original entry.");
    }

    private static bool Finished(GameEngine game, long useId, int card) => Facts<CardUseFinishedEvent>(game)
        .Count(fact => fact.ResolutionId == useId && fact.CardKind == CardKind.BarbarianAssault && fact.CardId == card) == 1;

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the active group and the exact accepted movement on each viewer's boundary.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool sourceHasJuxiang = false)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(sourceHasJuxiang));
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

    private sealed class Fixture(bool sourceHasJuxiang) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-juxiang-original-barbarian", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ClaimantBuilder(builder), CaoCao, "boundary:cao-cao", ZhuRong);
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
            builder.AddGeneral(new(Source, "同桌", "supporter", sourceHasJuxiang ? Juxiang : "standard:none", "wei", 8));
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体南蛮", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:barbarian_assault", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "巨象原南蛮", 4, 4, new Dictionary<string, int>
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
