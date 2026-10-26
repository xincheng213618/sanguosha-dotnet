using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void CategoryRefundCountsNativeShopEquipmentInItsExactThreeCategoryPaymentCold()
    {
        // Reuse the verified native free Zhangba/Nullification page. Sanchen is
        // acquired by a real program afterward, preserving that page's pool.
        var (game, registry) = Start(false, 28348, skills: ["wushuang:skill-3252"], grantCategoryRefund: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id);
        Shopping(game);
        var equipment = Shop(game).Offers.Single(o => o.CardOfferId == 4186);
        var trick = Shop(game).Offers.Single(o => o.CardOfferId == 4278);
        Buy(game, equipment.Id);
        Buy(game, trick.Id);
        var equipmentId = E<WushuangShopEquipmentCreatedEvent>(game).Single(e => e.OfferId == equipment.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision));
        Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-category-refund", [], [], game.Revision, P(game)!.PromptId));
        Play(game);
        var basicId = game.CreateSnapshot(0).Players[0].Hand.First(c => c.Kind == CardKind.Dodge).Id;
        var trickId = game.CreateSnapshot(0).Players[0].Hand.Single(c => c.Kind == trick.CardKind).Id;
        Accept(game, new UseProgramSkillCommand(0, "ol:sanchen", "draw-three-discard-three", [], [0], game.Revision, P(game)!.PromptId));
        Drive(game, () => P(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "draw-discard-category") == true);
        var frameId = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.DrawDiscardCategoryRefund is not null).Id;
        void Select(int card)
        {
            var prompt = P(game)!;
            var choice = prompt.Choices.Single(c => c.Cards.SequenceEqual([card]));
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }
        Select(basicId);
        Select(equipmentId);
        Require(!E<DrawDiscardCategoryDiscardPaidEvent>(game).Any(e => e.FrameId == frameId) &&
            game.CreateSnapshot(0).Players[0].Equipment.Any(c => c.Id == equipmentId),
            "A partial private selection neither destroys the equipment nor pays the original three-card instruction.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Select(trickId);
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Play(game);

        var selected = new[] { basicId, equipmentId, trickId };
        var payment = E<DrawDiscardCategoryDiscardPaidEvent>(game).Single(e => e.FrameId == frameId);
        var entities = E<DrawDiscardCategoryEntityDiscardedEvent>(game).Where(e => e.FrameId == frameId).ToArray();
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(e => e.CardId == equipmentId);
        var movement = game.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence);
        var logical = E<NativeLogicalDiscardInvocationRecordedEvent>(game).Single(e => e.ParentFrameId == frameId);
        Require(payment is { RequiredCount: 3, ActualDiscardCount: 3, DistinctNonEmpty: true } &&
            entities.Select(e => e.CardId).SequenceEqual(selected) && entities.Select(e => e.Category).Distinct().Count() == 3 &&
            destroyed.ParentFrameId == frameId && movement.CardId == equipmentId && movement.From == CardLocation.Equipment(0) &&
            movement.To == CardLocation.OutsideGame && movement.Reason.Value == "skill-program.ol:sanchen.draw-discard-category.discard" &&
            entities.Single(e => e.CardId == equipmentId).MovementSequence == movement.Sequence &&
            logical is { ActualCount: 3, HandCount: 2 } &&
            E<NativeLogicalDiscardMaterialCompletedEvent>(game).Count(e => e.InvocationId == logical.InvocationId) == 3,
            "The original instruction and shared native invoice count the same three ordered category payments, including the exact shop equipment destruction on their owning frame.");
        var started = E<DrawDiscardCategoryStartedEvent>(game).Single(e => e.FrameId == frameId);
        var refund = E<DrawDiscardCategoryRefundedEvent>(game).Single(e => e.FrameId == frameId);
        Require(E<DrawDiscardCategoryDrawIssuedEvent>(game).Count(e => e.FrameId == frameId && e.Bonus && e.ActualCount == 1) == 1 &&
            refund is { BeforeUsage: 1, AfterUsage: 0 } && refund.PhaseInstanceId == started.PhaseInstanceId &&
            refund.ActualTurnNumber == started.ActualTurnNumber && refund.ActualTurnOwnerSeat == started.ActualTurnOwnerSeat &&
            E<DrawDiscardCategoryTargetBannedEvent>(game).Count(e => e.FrameId == frameId && e.TargetSeat == 0) == 1 &&
            E<DrawDiscardCategoryCompletedEvent>(game).Single(e => e.FrameId == frameId) is
                { DistinctNonEmpty: true, BonusIssued: true, QuotaRefunded: true, TargetBanned: true } &&
            game.CardMovements.Count(m => m.CardId == equipmentId && m.From == CardLocation.Equipment(0)) == 1 &&
            !game.CardMovements.Any(m => m.CardId == equipmentId && m.To == CardLocation.DiscardPile),
            "Cold continuation issues one real bonus, refunds only the original phase debit and bans that target once, without paying or recycling the destroyed equipment again.");
        Cold(game, registry);
    }
}
