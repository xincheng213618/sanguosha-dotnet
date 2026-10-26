using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void JinzhiShopEquipmentPaysOneLogicalDiscardAndReturnsNativeUseAndResponseCold()
    {
        foreach (var response in new[] { false, true })
        {
            // The fixed native page publishes a free black Zhangba. No card or
            // creation/destruction fact is injected into the engine.
            var (game, registry) = Start(response, 277, skills: ["wushuang:skill-3252"], initialHp: 20);
            Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(game, Shop(game).Offers[0].Id);
            Shopping(game);
            var equipment = Shop(game).Offers.Single(o => o.CardOfferId == 4186);
            Buy(game, equipment.Id);
            var card = E<WushuangShopEquipmentCreatedEvent>(game).Single(e => e.OfferId == equipment.Id).CardId;
            Accept(game, new FinishWushuangShopCommand(0, game.Revision));
            Play(game);
            var hp = game.CreateSnapshot(0).Players[0].Hp;
            if (response)
            {
                End(game);
                Drive(game, () => P(game) is { Kind: DecisionKind.RespondDodge });
            }
            Cold(game, registry);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            if (response)
            {
                var prompt = P(game)!;
                var choice = prompt.Choices.Single(c => c.Parameters.GetValueOrDefault("response") == "extended-view-as" &&
                    c.Cards.SequenceEqual([card]) && c.Parameters.GetValueOrDefault("conversion-skill-id") == "wushuang:skill-3252");
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
            }
            else
            {
                var action = game.GetHumanLegalActions().Single(a => a.ProgramSkillId == "wushuang:skill-3252" &&
                    a.PlayedCardKind == CardKind.Peach && a.SelectableCardIds.SequenceEqual([card]));
                Accept(game, new UseProgramSkillCommand(0, action.ProgramSkillId!, action.ProgramActivationId!, [card], [0], game.Revision, P(game)!.PromptId));
            }
            Drive(game, () => E<WushuangJinzhiPaidEvent>(game).Any(e => e.Owner == 0 && e.Action.PhysicalCards.Any(cost => cost.CardId == card)));
            var paidActionId = E<WushuangJinzhiPaidEvent>(game).Single(e => e.Owner == 0 &&
                e.Action.PhysicalCards.Any(cost => cost.CardId == card)).Action.ActionId;
            Cold(game, registry);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            if (response) Drive(game, () => E<CardResponseCompletedEvent>(game).Any(e =>
                e.ActionId == paidActionId));
            else Play(game);

            var paid = E<WushuangJinzhiPaidEvent>(game).Single(e => e.Owner == 0 && e.Action.ActionId == paidActionId);
            var cost = paid.Action.PhysicalCards.Single();
            var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(e => e.CardId == card);
            var movement = game.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence);
            var invoice = E<NativeLogicalDiscardInvocationRecordedEvent>(game).Single(e => e.ParentFrameId == paid.FrameId);
            var material = E<NativeLogicalDiscardMaterialCompletedEvent>(game).Single(e => e.InvocationId == invoice.InvocationId);
            Require(paid is { Ordinal: 1 } && cost.CardId == card && cost.From == CardLocation.Equipment(0) &&
                paid.Action.Type == (response ? CardActionType.Response : CardActionType.Use) &&
                paid.MaterialPaymentFrameId == destroyed.ParentFrameId &&
                movement.From == cost.From && movement.To == CardLocation.OutsideGame &&
                movement.Reason == (response ? CardMoveReasons.Respond : CardMoveReasons.Use),
                "Jinzhi retains the accepted native action, exact owned equipment and destruction on its original payment parent.");
            Require(invoice is { ActualCount: 1, HandCount: 0, InitiatorSeat: 0, ExecutorSeat: 0 } &&
                invoice.Reason.Value == "skill-program.wushuang.jinzhi.discard" &&
                material.CardId == card && material.From == cost.From && material.To == CardLocation.OutsideGame &&
                material.CompletionSequence == movement.Sequence && invoice.SequenceBefore < movement.Sequence &&
                game.CardMovements.Count(m => m.CardId == card && m.From == cost.From) == 1 &&
                !game.CardMovements.Any(m => m.CardId == card && (m.To == CardLocation.DiscardPile || m.From == CardLocation.OutsideGame && m.Sequence > movement.Sequence)),
                "The already destroyed payment credits one real logical equipment discard and never moves or recycles its outside-game entity.");
            Require(E<WushuangOriginalSkillDrawEvent>(game).Count(e => e.FrameId == paid.FrameId && e.Requested == 1 && e.Actual == 1) == 1 &&
                (response ? E<CardResponseCompletedEvent>(game).Count(e => e.ActionId == paid.Action.ActionId) == 1 :
                    game.CreateSnapshot(0).Players[0].Hp == hp + 1),
                "One paid ordinal draws once, then returns to its real native Dodge response or Peach recovery after cold replay.");
            Cold(game, registry);
        }
    }
}
