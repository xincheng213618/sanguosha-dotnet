using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void PurchasedBlackEquipmentQixiFinishesItsExactDestroyedPaymentCold()
    {
        var (game, registry) = Start(false, 277, skills: ["classic:qixi"], initialHp: 20, aiQuota: 0);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id);
        Shopping(game);
        var offer = Shop(game).Offers.Single(o => o.CardOfferId == 4186);
        Buy(game, offer.Id);
        var material = E<WushuangShopEquipmentCreatedEvent>(game).Single(e => e.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision));
        Play(game);
        Cold(game, registry);
        var before = game.CreateSnapshot(0).Players[1].HandCount;
        var action = game.GetHumanLegalActions().Single(a => a.CardId == material && a.Kind == LegalActionKind.Dismantlement &&
            a.ConversionSource?.SkillId == "classic:qixi");
        Accept(game, new PlayCardCommand(0, material, action.TargetSeats, game.Revision, P(game)!.PromptId,
            action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
        Drive(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.SelectTargetCard });
        var use = E<CardUseDeclaredEvent>(game).Last(e => e.CardId == material).ResolutionId;
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(e => e.CardId == material);
        Require(destroyed.ParentFrameId == use && game.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence) is
            { From.Zone: CardZoneKind.Equipment, To: var destination } && destination == CardLocation.OutsideGame,
            "Qixi pays with the actual equipped shop entity on its own native use; destruction does not cancel the pending Dismantlement.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First().Id, game.Revision));
        Play(game);
        Require(game.CreateSnapshot(0).Players[1].HandCount == before - 1 &&
            E<TargetCardDiscardedEvent>(game).Count(e => e.ResolutionId == use) == 1 &&
            E<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == use) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(e => e.CardId == material) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Equipment(0)) == 1 &&
            !game.CardMovements.Any(m => m.CardId == material && m.Sequence > destroyed.MovementSequence) &&
            game.ResolutionStack.All(frame => frame.Id != use) && game.CreateSnapshot(0).ProcessingCardCount == 0,
            "Returning cold discards the selected enemy card and finishes the trick once without trying to move the paid material out of OutsideGame.");
        Cold(game, registry);
    }
}
