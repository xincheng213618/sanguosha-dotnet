using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicLijunDiscardEntryChecks
{
    private const string Lord = "classic:sun-liang", Lijun = "classic:lijun";
    private const string CaoCao = "boundary:cao-cao-mobile", Jianxiong = "boundary:jianxiong-mobile";
    private const string Watch = "fixture:lijun-claim-child";
    private const string Claim = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards";
    private const string Rediscard = "skill-program.fixture:lijun-claim-child.SelectAndMoveOwnedCard";
    private const string Gift = "skill-program.faction-cost-gift.obtain", Reward = "skill-program.faction-cost-gift.reward";
    private const string Mode = "identity:classic-lijun-entry", Deck = "fixture:lijun-entry-deck";
    private static readonly string[] Generals = [Lord, "fixture:lijun-wu", CaoCao, "fixture:lijun-peer"];

    internal static void LijunCannotGiftTheRediscardedMaterialOfTheOldSlashCold()
    {
        var (game, content) = Start();
        PlaySlash(game, 0, 3);
        ReachWithNativeDodge(game, prompt => IsPlay(prompt, 0));
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: "classic:kuizhu" });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("mode") == "skip");
        Reach(game, prompt => IsPlay(prompt, 1));

        var card = PlaySlash(game, 1, 2);
        ReachWithNativeDodge(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Jianxiong });
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);
        var actionId = use.Action!.ActionId;
        Require(use.SourceSeat == 1 && use.Action.PhysicalCards.Single().CardId == card &&
            Facts<DamageAppliedEvent>(game).Any(fact => fact is { SourceSeat: 1, TargetSeat: 2, Amount: 1 }),
            "The Wu character's real Play-phase Slash deals damage and offers Jianxiong on that same physical material.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => IsLijun(prompt, 1, "give"));
        var cleanup = control.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        Require(cleanup.From == CardLocation.Processing && cleanup.To == CardLocation.DiscardPile && Finished(control, use.Id, card),
            "Declining the earlier claim leaves the material for its original Slash cleanup and Lijun offer.");
        Answer(control, choice => GiftMode(choice) == "give" && choice.Cards.SequenceEqual([card]) && choice.Targets.SequenceEqual([0]));
        Reach(control, prompt => IsLijun(prompt, 0, "reward"));
        Answer(control, choice => GiftMode(choice) == "reward");
        Reach(control, prompt => IsPlay(prompt, 1));
        var gifted = Facts<CompletedFactionCostGiftedEvent>(control).Single();
        Require(gifted is { ProviderSeat: 1, RecipientSeat: 0, SkillId: Lijun } && gifted.ActionId == actionId &&
            gifted.CardIds.SequenceEqual([card]) && control.CardMovements.Single(move => move.Reason.Value == Gift) is { CardId: var given, From: var from, To: var to } &&
            given == card && from == CardLocation.DiscardPile && to == CardLocation.Hand(0) &&
            control.CardMovements.Count(move => move.Reason.Value == Reward && move.To == CardLocation.Hand(1)) == 1 &&
            Facts<TurnRuleModifierGrantedEvent>(control).Count(fact =>
                fact.Modifier is { Query: SkillRuleQuery.SlashLimit, Operation: SkillRuleOperation.Add, Amount: 1, AffectedSeat: 1 }) == 1 &&
            control.GetLegalActions(1).Any(action => action.Kind == LegalActionKind.Slash),
            "The original use really gives the card to the lord, and the accepted reward draws and enables an additional Slash.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var delivery = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Claim);
        var material = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Single();
        Require(delivery.From == CardLocation.Processing && delivery.To == CardLocation.Hand(2) &&
            material is { Kind: NativeMaterialClaimKind.Delivery, RecipientSeat: 2 } && material.CardId == card &&
            material.MovementSequence == delivery.Sequence && !Finished(game, use.Id, card),
            "The original Slash records the genuine earlier Jianxiong delivery before its gain child discards the material.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => IsPlay(prompt, 1) || IsLijun(prompt, 1, "give"));
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.From == CardLocation.Hand(2) && newer.To == CardLocation.DiscardPile && newer.Sequence > delivery.Sequence &&
            newer.BatchId != delivery.BatchId && Finished(game, use.Id, card) &&
            game.CardMovements.All(move => move.CardId != card || move.Reason != CardMoveReasons.UseFinished),
            "The completed original Slash keeps its delivery, and the entity reaches discard only through the later actual discard.");

        if (P(game) is { } stale && IsLijun(stale, 1, "give"))
        {
            Answer(game, choice => GiftMode(choice) == "give" && choice.Cards.SequenceEqual([card]));
            Reach(game, prompt => IsLijun(prompt, 0, "reward"));
            Answer(game, choice => GiftMode(choice) == "decline");
        }
        Reach(game, prompt => IsPlay(prompt, 1));
        Require(!Facts<CompletedFactionCostGiftedEvent>(game).Any() &&
            game.CardMovements.All(move => move.Reason.Value != Gift && move.Reason.Value != Reward) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Lijun must leave entry {newer.Sequence} in discard after the original Slash delivered that material at {delivery.Sequence}.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static bool Finished(GameEngine game, long use, int card) => Facts<CardUseFinishedEvent>(game)
        .Any(fact => fact.ResolutionId == use && fact.CardKind == CardKind.Slash && fact.CardId == card);
    private static bool IsPlay(PendingDecision prompt, int seat) => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == seat;
    private static string? GiftMode(PromptChoice choice) => choice.Parameters.GetValueOrDefault("mode");
    private static bool IsLijun(PendingDecision prompt, int seat, string mode) => prompt.PlayerSeat == seat &&
        prompt.SkillPrompt?.SkillId == Lijun && prompt.Choices.Any(choice => GiftMode(choice) == mode);

    private static int PlaySlash(GameEngine game, int actor, int target)
    {
        var slash = game.GetLegalActions(actor).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([target]));
        var id = slash.CardId!.Value;
        Accept(game, new PlayCardCommand(actor, id, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        return id;
    }

    private static void ReachWithNativeDodge(GameEngine game, Func<PendingDecision, bool> goal)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (goal(prompt))
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} while settling Slash.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not reach its public decision boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves both the exact movement entries and the pending decision in all seat views.");
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
        public PackageManifest Manifest { get; } = new("fixture-classic-lijun-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ClaimantBuilder(builder), Lord, CaoCao, "boundary:cao-cao");
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"discard-after-native-claim","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{{Claim}}}"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "取得后弃置", description = "奸雄真正获得伤害牌后，弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Generals[1], "用杀吴将", "supporter", "standard:none", "wu", 8));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 40)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "立军原用牌", 4, 4, new Dictionary<string, int>
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
