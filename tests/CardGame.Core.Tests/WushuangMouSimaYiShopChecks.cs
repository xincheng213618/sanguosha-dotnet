using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string MouSmShopGiveWatch = "fixture:wushuang-quanmou-shop-give-watch";

    public static void QuanmouShopEquipmentSettlesExactGiveWithoutRecipientGainCold()
    {
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 20, simaYiShopGive: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var created = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id);
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "observe-native-quanmou-give", [], [], game.Revision, P(game)!.PromptId)); Play(game);
        End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "mou:quanmou" && P(game)!.PlayerSeat == 0 &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.OwnerSeat == 1 && frame.MouSimaYi is { Stage: MouSmStage.Give, Target: 0 }));
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.MouSimaYi is { Stage: MouSmStage.Give });
        var original = parent.MouSimaYi!;
        var recipientHand = game.CreateSnapshot(1).Players[1].HandCount;
        var choice = P(game)!.Choices.Single(item => item.Cards.SequenceEqual([created.CardId]));
        Require(created.Seat == 0 && parent.OwnerSeat == 1 && original.Source.SkillId == "mou:quanmou" &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == created.CardId).Location == CardLocation.Equipment(0),
            "The actual opponent's native Quanmou publishes the purchased equipment as the original human giver's physical choice.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Accept(game, new AnswerPromptCommand(0, P(game)!.PromptId, choice.Id, game.Revision));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == MouSmShopGiveWatch);
        var suspended = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var paid = E<MouSmGivePaidEvent>(game).Single(fact => fact.FrameId == parent.Id);
        var destruction = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == created.CardId);
        var movement = game.CardMovements.Single(move => move.Sequence == paid.Sequence);
        Require(suspended.MouSimaYi is { Stage: MouSmStage.GiveChildren } receipt && receipt.Source == original.Source &&
            receipt.GameplayHash == original.GameplayHash && receipt.Target == 0 && receipt.Materials is [{ Id: var id, From: var from }] &&
            id == created.CardId && from == CardLocation.Equipment(0) && paid.Giver == 0 && paid.CardId == id &&
            receipt.Before < movement.Sequence && receipt.After == movement.Sequence &&
            movement.CardId == id && movement.From == from && movement.To == CardLocation.OutsideGame &&
            movement.Reason.Value == "skill-program.mou:quanmou.give" && destruction.MovementSequence == movement.Sequence &&
            destruction.ParentFrameId == parent.Id &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.Batch.ParentFrameId == parent.Id &&
                window.Batch.OriginSkillId == parent.SkillId && window.Batch.OriginSkillInstanceId == parent.SkillInstanceId &&
                window.Batch.Movements.Any(move => move.Sequence == movement.Sequence)) &&
            game.CreateSnapshot(1).Players[1].HandCount == recipientHand &&
            !E<NativeCardsGivenEvent>(game).Any(fact => fact.ProducerId == parent.Id),
            "The exact original Give source/parent pays once through the shop's matching creation/destruction invoice; the destroyed entity never becomes a recipient hand gain or native delivered-gift reward.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Accept(game, new AnswerPromptCommand(0, P(game)!.PromptId,
            P(game)!.Choices.Single(item => item.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Drive(game, () => E<MouSmDebtIssuedEvent>(game).Any(fact => fact.FrameId == parent.Id));
        Require(E<MouSmGivePaidEvent>(game).Count(fact => fact.FrameId == parent.Id) == 1 &&
            E<MouSmDebtIssuedEvent>(game).Single(fact => fact.FrameId == parent.Id) is { Target: 0 } debt &&
            debt.Source == original.Source && debt.GameplayHash == original.GameplayHash && debt.Yin == original.Yin &&
            E<MouSmPolarityEvent>(game).Single(fact => fact.FrameId == parent.Id).Yin == !original.Yin &&
            E<MouSmCompletedEvent>(game).Count(fact => fact.FrameId == parent.Id) == 1 &&
            !game.ResolutionStack.Any(frame => frame.Id == parent.Id) &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == created.CardId) == 1 &&
            game.CardMovements.Count(move => move.CardId == created.CardId && move.From == CardLocation.Equipment(0)) == 1 &&
            !game.CardMovements.Any(move => move.CardId == created.CardId && (move.To == CardLocation.Hand(1) ||
                move.From == CardLocation.OutsideGame && move.Sequence > movement.Sequence)) &&
            !E<NativeCardsGivenEvent>(game).Any(fact => fact.ProducerId == parent.Id),
            "Cold return preserves the one settled entity departure and original Quanmou polarity/debt continuation without replaying payment, creating delivery rewards or reviving the destroyed equipment.");
        Cold(game, registry);
    }
}
