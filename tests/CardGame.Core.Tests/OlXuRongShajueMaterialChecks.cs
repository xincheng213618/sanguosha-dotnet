using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlXuRongShajueMaterialChecks
{
    private const string Hero = "ol:xu-rong", Xionghuo = "ol:xionghuo", Shajue = "ol:shajue";
    private const string Earlier = "fixture:shajue-early-delivery", Watch = "fixture:shajue-dying-witness";
    private const string Deliver = "skill-program.current-use-physical-claim";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";
    private const string Claim = "program.shajue2023.claim";
    private const string Mode = "identity:classic-shajue-delivered-slash", Deck = "fixture:shajue-slash-deck";
    private static readonly string[] Generals = [Hero, "fixture:shajue-slash-victim", "fixture:shajue-peer-2", "fixture:shajue-peer-3"];

    internal static void ShajueReplenishesButCannotClaimAnAlreadyDeliveredSlashCold()
    {
        var (game, content) = Start();
        Require(game.CreateSnapshot(1).Players[1].Hp == 1, "The actual Slash target starts at one health.");
        Accept(game, new UseProgramSkillCommand(0, Xionghuo, "transfer-one-marker", [], [1], game.Revision, P(game)!.PromptId));
        Settle(game);
        var marked = Facts<XionghuoMarkTransferredEvent>(game).Single();
        Require(marked is { Grant.TargetSeat: 1, PoolBefore: 3, PoolAfter: 2 }, "Real Xionghuo marks the Slash target and spends one token.");

        var action = game.GetLegalActions(0).First(move => move.Kind == LegalActionKind.Slash && move.TargetSeats.SequenceEqual([1]));
        var card = action.CardId!.Value;
        Accept(game, new PlayCardCommand(0, card, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Earlier } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == card);

        var control = Cold(game, content);
        Skip(control);
        ReachDyingWitness(control);
        var originalDying = ReadDyingProof(control, use);
        var unclaimed = control.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id);
        Require(originalDying.Entities is [{ CardId: var original, OriginalLocation: var originalLocation }] &&
            original == card && originalLocation == CardLocation.Processing && unclaimed.MaterialClaims.Count == 0 &&
            !Facts<XuRongCardsPaidEvent>(control).Any(fact => fact.Kind == Claim),
            "The earlier same-seat witness pauses the untouched original Slash in Processing before Shajue claims it.");
        ContinueWitness(control);
        var originalGain = control.CardMovements.Single(move => move.Reason.Value == Claim);
        Require(originalGain.CardId == card && originalGain.From == CardLocation.Processing && originalGain.To == CardLocation.Hand(0) &&
            Facts<XuRongCardsPaidEvent>(control).Single(fact => fact.Kind == Claim).CardIds.SequenceEqual([card]),
            "Shajue physically acquires the original damage material in the ordinary branch.");
        AssertMarkerReplenished(control, originalDying);

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Watch } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var given = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Deliver);
        var delivery = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.Single();
        Require(given.From == CardLocation.Processing && given.To == CardLocation.Hand(1) &&
            delivery is { Kind: NativeMaterialClaimKind.Delivery, RecipientSeat: 1 } && delivery.CardId == card &&
            delivery.MovementSequence == given.Sequence && !Facts<PlayerDyingEvent>(game).Any(),
            "The native finalized-target claim delivers the original Slash before damage and records that delivery on the use.");

        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        ReachDyingWitness(game);
        var dying = ReadDyingProof(game, use);
        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Rediscard);
        Require(newer.Sequence > given.Sequence && newer.BatchId != given.BatchId &&
            newer.From == CardLocation.Hand(1) && newer.To == CardLocation.DiscardPile &&
            game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id).MaterialClaims.SequenceEqual([delivery]) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            "The very same card has a new discard entry at the real dying boundary while the original Slash retains the earlier delivery.");
        game = Cold(game, content);
        _ = ReadDyingProof(game, use);
        ContinueWitness(game);
        AssertMarkerReplenished(game, dying);
        Require(game.CardMovements.All(move => move.Reason.Value != Claim) &&
            Facts<XuRongCardsPaidEvent>(game).Single(fact => fact.Kind == Claim).CardIds.Count == 0 &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Shajue must leave discard entry {newer.Sequence} alone after the original Slash already delivered that material at {given.Sequence}.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static XuRongDyingProof ReadDyingProof(GameEngine game, CardUseFrame use)
    {
        var dying = game.ResolutionStack.OfType<DyingFrame>().Single();
        var damage = game.ResolutionStack.OfType<DamageFrame>().Single(frame => frame.Id == dying.ParentFrameId);
        var proof = dying.XuRongProof!;
        Require(proof is { VictimSeat: 1, HpAtEntry: -1, RescueCardsRequired: 2, CauseSourceSeat: 0 } &&
            proof.DyingFrameId == dying.Id && proof.DamageAttemptResolutionId == use.Id && proof.NativeActionId == use.Action!.ActionId &&
            damage is { AppliedHpAfter: -1, Amount: 2, SourceSeat: 0, TargetSeat: 1 } && damage.ParentFrameId == use.Id &&
            Facts<PlayerDyingEvent>(game).Single().ResolutionId == dying.Id &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 2, RemainingHp: -1 },
            "The original marked Slash really causes one dying entry that requires two rescue cards, even after delivery.");
        return proof;
    }

    private static void AssertMarkerReplenished(GameEngine game, XuRongDyingProof dying)
    {
        Require(Facts<XionghuoPoolChangedEvent>(game).Single(fact => fact.Kind == "dying") is
                { Source.SkillId: Shajue, Before: 2, After: 3 } &&
            Facts<XuRongCompletedEvent>(game).Single(fact => fact.DyingFrameId == dying.DyingFrameId) is
                { Source.SkillId: Shajue, SubjectSeat: 1 },
            "The true dying entry still replenishes one token and completes Shajue once.");
    }

    private static bool WitnessChoice(PromptChoice choice) => choice.Parameters.GetValueOrDefault("option-id") == "continue";

    private static void ReachDyingWitness(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Earlier } && prompt.Choices.Any(WitnessChoice))
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} before the dying witness.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The real Slash did not reach its dying witness.");
    }

    private static void ContinueWitness(GameEngine game)
    {
        Answer(game, WitnessChoice);
        for (var step = 0; step < 100; step++)
        {
            if (Facts<XuRongCompletedEvent>(game).Any(fact => fact.Source.SkillId == Shajue))
                return;
            Require(P(game) is null, "Shajue finishes before publishing another public decision.");
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The original dying window did not complete Shajue.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the genuine original damage frame and the pending decision in all seat views.");
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
        public PackageManifest Manifest { get; } = new("fixture-ol-shajue-original-material", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ActorBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Earlier}}}","revision":1,"triggers":[{"id":"give-native-slash-before-damage","window":"cardUseTargetsFinalized",
                    "ownerRelation":"actor","cardKinds":["slash"],"optional":true,
                    "effects":[{"op":"claimCurrentUsePhysicalCards","target":"owner","targetRef":{"kind":"eventTarget"}}]},
                    {"id":"pause-before-shajue","window":"dyingEntering","subject":"any","priority":1000,"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"dying","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"discard-after-native-delivery","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{{Deliver}}}"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Earlier] = new { name = "原杀交付与濒死见证", description = "可以交付当前杀的原实体，并在真实濒死入口确认。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [Watch] = new { name = "取得后弃置", description = "收到原杀后弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[1], "濒死目标", "supporter", Watch, "qun", 1));
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "杀绝原实体", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class ActorBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == Hero ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Earlier] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}
