using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void SujiPaysExactNativeShopEquipmentAndReturnsOnceCold()
    {
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 20, materialCase: "suji");
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var card = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Require(game.CreateSnapshot(0).Players[0].Equipment.Any(item => item.Id == card && item.Kind == CardKind.ZhangbaSerpentSpear),
            "The fixed real shop page creates and equips the black Zhangba entity before acquiring Suji.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-material-flow", [], [], game.Revision, P(game)!.PromptId));
        Play(game); End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "ol:suji" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P(game)!;
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
        }
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Drive(game, () => game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.ZhangYan?.Stage == ZhangYanStage.SujiCard));
        Answer(choice => choice.Cards.SequenceEqual([card]));
        Drive(game, () => game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.ZhangYan?.Stage == ZhangYanStage.SujiTarget));
        Answer(choice => choice.Targets.SequenceEqual([1]));
        Drive(game, () => E<ZhangYanSujiPaidEvent>(game).Any(fact => fact.CardId == card));
        var issued = E<ZhangYanSujiIssuedEvent>(game).Single(fact => fact.Return.CardId == card).Return;
        var paid = E<ZhangYanSujiPaidEvent>(game).Single(fact => fact.CardId == card);
        var destruction = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == card);
        var movement = game.CardMovements.Single(move => move.Sequence == destruction.MovementSequence);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == issued.UseId);
        Require(issued.From == CardLocation.Equipment(0) && issued.Source.SkillId == "ol:suji" &&
            destruction.ParentFrameId == issued.UseId && paid.UseId == issued.UseId && paid.FrameId == issued.ProgramFrameId &&
            paid.To == CardLocation.OutsideGame && movement.From == issued.From && movement.To == paid.To &&
            movement.Reason == CardMoveReasons.Use && movement.Sequence > paid.Before && movement.Sequence <= paid.After &&
            use.Action is { Type: CardActionType.Use, ActorSeat: 0, ProviderSeat: 0 } action &&
            action.ActionId == issued.ActionId && action.PhysicalCards.Single().CardId == card &&
            use.PaymentMovementReceipt?.Batch.Id == paid.BatchId,
            "Suji pays its exact accepted native equipment on the real Use, with one matching movement invoice and shop-destruction parent before responses return.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Play(game);
        Require(E<ZhangYanSujiIssuedEvent>(game).Count(fact => fact.Return == issued) == 1 &&
            E<ZhangYanSujiPaidEvent>(game).Count(fact => fact.UseId == issued.UseId) == 1 &&
            E<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == issued.UseId && fact.CardId == card) == 1 &&
            E<ZhangYanSujiReturnedEvent>(game).Count(fact => fact.Return == issued) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == card) == 1 &&
            game.CreateCardZoneDiagnostics().Single(entity => entity.CardId == card).Location == CardLocation.OutsideGame &&
            game.CardMovements.Count(move => move.CardId == card && move.From == issued.From) == 1 &&
            !game.CardMovements.Any(move => move.CardId == card && move.Sequence > movement.Sequence),
            "Cold finishes and returns the original native Slash once without moving an already destroyed material again or inventing a discarded entity.");
        Cold(game, registry);
    }
}
