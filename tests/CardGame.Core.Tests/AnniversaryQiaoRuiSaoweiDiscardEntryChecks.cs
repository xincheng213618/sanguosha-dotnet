using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class AnniversaryQiaoRuiSaoweiDiscardEntryChecks
{
    private const string Hero = "anniversary:qiao-rui", Aishou = "anniversary:aishou", Saowei = "anniversary:saowei";
    private const string Earlier = "fixture:saowei-early-delivery", Receiver = "fixture:saowei-rediscard";
    private const string Mode = "identity:classic-saowei-original-material", Deck = "fixture:saowei-deck";
    private const string DeliveryReason = "skill-program.current-use-physical-claim";
    private const string RediscardReason = "skill-program." + Receiver + ".SelectAndMoveOwnedCard";
    private const string ObtainReason = "skill-program.anniversary:saowei.obtain";
    private static readonly string[] Generals = [Hero, "fixture:saowei-first-attacker", "fixture:saowei-target", "fixture:saowei-peer"];

    internal static void SaoweiCannotObtainANewerDiscardOfAnAlreadyDeliveredSlashCold()
    {
        var (game, content) = Start();
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => CanInvoke(prompt, 0, Aishou));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var mark = Facts<AnniversaryQiaoRuiMarkedEvent>(game).First();
        var original = game.GetLegalActions(1).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([2]));
        Accept(game, new PlayCardCommand(1, original.CardId!.Value, original.TargetSeats, game.Revision, P(game)!.PromptId, original.PlayedCardKind));
        ReachPastDodge(game, prompt => CanInvoke(prompt, 0, Saowei));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Saowei } && prompt.Choices.Any(choice => IsUse(choice, mark.CardId)));
        Answer(game, choice => IsUse(choice, mark.CardId));
        Reach(game, prompt => CanInvoke(prompt, 0, Earlier));
        var origin = Facts<AnniversaryQiaoRuiUseIssuedEvent>(game).Single().Origin;
        var paid = game.CardMovements.Single(move => move.CardId == mark.CardId && move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == origin.UseId);
        var targetHp = game.CreateSnapshot(2).Players[2].Hp;
        Require(origin.CardId == mark.CardId && origin.Source.SkillId == Saowei && paid.Sequence > mark.EntrySequence &&
                use.TargetSeats.SequenceEqual([2]) && use.Action!.PhysicalCards.Select(card => card.CardId).SequenceEqual([mark.CardId]) &&
                use.Action.ConversionChain.SequenceEqual([origin.Source]) && Facts<AnniversaryQiaoRuiDamageEvent>(game).Length == 0,
            "Real Aishou marks the deck draw and Saowei pays that exact hand entry as a native converted Slash before its target-finalized child.");

        var control = Cold(game, content);
        Skip(control);
        ReachPastDodge(control, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        AssertResolved(control, origin, targetHp);
        var originalFinish = control.CardMovements.Single(move => move.CardId == mark.CardId && move.Reason == CardMoveReasons.UseFinished);
        var obtained = control.CardMovements.Single(move => move.CardId == mark.CardId && move.Reason.Value == ObtainReason);
        Require(originalFinish.From == CardLocation.Processing && originalFinish.To == CardLocation.DiscardPile &&
                obtained.Sequence > originalFinish.Sequence && obtained.From == originalFinish.To && obtained.To == CardLocation.Hand(0) &&
                Facts<AnniversaryQiaoRuiClaimedEvent>(control).Single() is { UseId: var claimUse, CardId: var claimId } &&
                claimUse == origin.UseId && claimId == mark.CardId &&
                control.CreateSnapshot(0).Players[0].Hand.Single(card => card.Id == mark.CardId).SkillMarkLabel is null,
            "With no earlier delivery, Saowei obtains the original native discard once and does not renew the old Ai mark.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Receiver } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([mark.CardId])));
        var delivery = game.CardMovements.Single(move => move.CardId == mark.CardId && move.Reason.Value == DeliveryReason);
        var returnedUse = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == origin.UseId);
        Require(delivery.Sequence > paid.Sequence && delivery.From == CardLocation.Processing && delivery.To == CardLocation.Hand(2) &&
                returnedUse.MaterialClaims.Any(claim => claim.CardId == mark.CardId && claim.Kind == NativeMaterialClaimKind.Delivery &&
                    claim.RecipientSeat == 2 && claim.MovementSequence == delivery.Sequence),
            "The original finalized-target child physically delivers the paid Slash material to the target as a native claim.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([mark.CardId]));
        ReachPastDodge(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        AssertResolved(game, origin, targetHp);
        var renewed = game.CardMovements.Single(move => move.CardId == mark.CardId && move.Reason.Value == RediscardReason);
        Require(renewed.Sequence > delivery.Sequence && renewed.BatchId != delivery.BatchId && renewed.From == CardLocation.Hand(2) &&
                renewed.To == CardLocation.DiscardPile &&
                game.CardMovements.All(move => move.CardId != mark.CardId || move.Reason != CardMoveReasons.UseFinished),
            "The true owned-card operation discards the same physical entity again; the relinquished original Slash does not pay or discard it twice.");
        Require(Facts<AnniversaryQiaoRuiClaimedEvent>(game).Length == 0 &&
                game.CardMovements.Last(move => move.CardId == mark.CardId) == renewed,
            $"Saowei must leave newer discard entry {renewed.Sequence} alone after its use delivered the original material at {delivery.Sequence}.");
        _ = Cold(game, content);
    }

    private static bool IsUse(PromptChoice choice, int material) => choice.Parameters.GetValueOrDefault("verb") == "use" &&
        choice.Cards.SequenceEqual([material]) && choice.Targets.SequenceEqual([2]);

    private static bool CanInvoke(PendingDecision prompt, int seat, string skill) => prompt.PlayerSeat == seat &&
        prompt.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void AssertResolved(GameEngine game, AnniversaryQiaoRuiUseOrigin origin, int targetHp)
    {
        Require(Facts<AnniversaryQiaoRuiDamageEvent>(game).Single() is { Target: 2, Amount: 1 } damage && damage.UseId == origin.UseId &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == origin.UseId) == 1 &&
                Facts<AnniversaryQiaoRuiUseReturnedEvent>(game).Single().UseId == origin.UseId &&
                game.CreateSnapshot(2).Players[2].Hp == targetHp - 1,
            "The issued native Saowei Slash applies its actual damage and completes once in both branches.");
    }

    private static void ReachPastDodge(GameEngine game, Func<PendingDecision, bool> reached)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is { } prompt)
            {
                if (reached(prompt))
                    return;
                Require(prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 }, $"Unexpected Saowei prompt {prompt.Prompt}.");
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The real Saowei Slash did not reach its settlement.");
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
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The actual first play phase belongs to Qiao Rui.");
        return (game, content);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
                JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
                game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                    restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
                Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands reproduce the exact original Slash payment, native delivery and pending owned-card choice.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-saowei-original-material", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new ActorBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Earlier}}}","revision":1,
                    "modifiers":[{"id":"reach-same-target","query":"attackRange","operation":"add","value":1,"priority":0}],
                    "triggers":[{"id":"deliver-native-slash","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],
                        "optional":true,"effects":[{"op":"claimCurrentUsePhysicalCards","target":"owner","targetRef":{"kind":"eventTarget"}}]}]},
                  {"id":"{{{Receiver}}}","revision":1,"triggers":[{"id":"discard-native-delivery","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{{DeliveryReason}}}"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Earlier] = new { name = "原杀交付", description = "攻击范围加一，且可以把当前杀的实体交给目标。" },
                    [Receiver] = new { name = "取得后弃置", description = "收到原杀后弃置一张手牌。" }
                }
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[1], "原杀使用者", "supporter", "standard:none", "wei", 6));
            builder.AddGeneral(new(Generals[2], "原杀目标", "supporter", Receiver, "qun", 6));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "shu", 6));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 44)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "扫围原结算实体", 4, 4, new Dictionary<string, int>
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
