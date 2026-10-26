using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void XianniangCountsOnlyReceivedCardsWhenNativeShopEquipmentIsDestroyedCold()
    {
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 20,
            materialCase: "xianniang", publicPileStorage: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var card = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-material-flow", [], [], game.Revision, P(game)!.PromptId));
        Play(game); var hp = game.CreateSnapshot(0).Players[0].Hp;
        Accept(game, new UseProgramSkillCommand(0, Driver, "public-pile-incoming", [], [], game.Revision, P(game)!.PromptId));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "anniversary:xianniang" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P(game)!;
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
        }
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Answer(choice => choice.Parameters.GetValueOrDefault("verb") == "source" && choice.Targets.SequenceEqual([0]));
        Answer(choice => choice.Cards.SequenceEqual([card]));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == MaterialFlowWatch);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.AnniversaryHouCheng is not null);
        var paid = E<AnniversaryHouChengMovementEvent>(game).Single(fact => fact.FrameId == parent.Id);
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == card);
        var movement = game.CardMovements.Single(move => move.Sequence == destroyed.MovementSequence);
        var hpAfterIncoming = game.CreateSnapshot(0).Players[0].Hp;
        var incomingAmount = parent.WindowContext?.Amount ?? 0;
        Require(parent.AnniversaryHouCheng is { Stage: AnniversaryHouChengStage.ObtainChildren, Obtained: 0 } &&
            parent.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: 0 } &&
            incomingAmount > 0 && hpAfterIncoming == hp - incomingAmount &&
            paid is { Giving: false, ActualCount: 0, CardOwner: 0, Recipient: 0 } && destroyed.ParentFrameId == parent.Id &&
            movement.From == CardLocation.Equipment(0) && movement.To == CardLocation.OutsideGame &&
            movement.Reason.Value == "skill-program.anniversary:xianniang.obtain" && movement.Sequence > paid.Before && movement.Sequence <= paid.After,
            "The original Xianniang request records one real selected shop-equipment departure and zero actual Hand receipts before its movement child returns.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Drive(game, () => E<AnniversaryHouChengCompletedEvent>(game).Any(fact => fact.FrameId == parent.Id));
        Require(E<AnniversaryHouChengMovementEvent>(game).Count(fact => fact.FrameId == parent.Id) == 1 &&
            E<AnniversaryHouChengCompletedEvent>(game).Count(fact => fact.FrameId == parent.Id) == 1 &&
            !E<AnniversaryHouChengHpPaidEvent>(game).Any(fact => fact.FrameId == parent.Id) && game.CreateSnapshot(0).Players[0].Hp == hpAfterIncoming &&
            game.CreateCardZoneDiagnostics().Single(entity => entity.CardId == card).Location == CardLocation.OutsideGame &&
            game.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Equipment(0)) == 1 &&
            !game.CardMovements.Any(move => move.CardId == card && move.Sequence > movement.Sequence),
            "Cold returns once without inventing an obtained card, a Wine entity, an excess-gain HP charge or another move from OutsideGame.");
        Cold(game, registry);
    }
}
