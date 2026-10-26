using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string PublicPileShopWatch = "fixture:wushuang-public-pile-shop-watch";

    public static void BingjieTargetPaysActualShopEquipmentDestructionOnceCold()
    {
        var (game, registry) = Start(true, 277, skills: ["wushuang:skill-3252"], publicPileStorage: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var cardId = E<WushuangShopEquipmentCreatedEvent>(game).Single(item => item.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-native-tuifeng", [], [], game.Revision, P(game)!.PromptId)); Play(game);
        Drive(game, () => P(game) is { PlayerSeat: 0 } prompt && prompt.SkillPrompt?.SkillId == "ol:bingjie" &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "discard"));
        var pending = P(game)!;
        Require(game.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == cardId) &&
            E<BingjieGrantedEvent>(game).Any(fact => fact.Source.OwnerSeat == 1),
            "The real opponent paid its actual Play-start maximum HP and issued its own native Slash target discard while the human still owns the purchased equipment.");
        Accept(game, new AnswerPromptCommand(0, pending.PromptId,
            pending.Choices.Single(choice => choice.Cards.SequenceEqual([cardId])).Id, game.Revision));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == PublicPileShopWatch);
        var producer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.MaMiDi?.Steps.Any(step => step.CardId == cardId) == true);
        var step = producer.MaMiDi!.Steps.Single(item => item.CardId == cardId);
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == cardId);
        Require(step.TargetSeat == 0 && step.From == CardLocation.Equipment(0) && step.To == CardLocation.OutsideGame &&
            destroyed.ParentFrameId == producer.Id && destroyed.MovementSequence > step.Before && destroyed.MovementSequence <= step.After,
            "Bingjie freezes the real target-owned equipment departure as OutsideGame, backed by the single native shop destruction on the same issued target-payment frame.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        pending = P(game)!;
        Accept(game, new AnswerPromptCommand(0, pending.PromptId,
            pending.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Play(game);
        Require(E<MaMiDiDiscardIssuedEvent>(game).Count(fact => fact.FrameId == producer.Id && fact.CardId == cardId) == 1 &&
            E<MaMiDiCompletedEvent>(game).Count(fact => fact.FrameId == producer.Id) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == cardId) == 1 &&
            game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Equipment(0)) == 1 &&
            game.CreateCardZoneDiagnostics().Single(entity => entity.CardId == cardId).Location == CardLocation.OutsideGame,
            "Cold target-payment return completes the original Bingjie operation once, retaining the destroyed real equipment outside the game without a second payment or fabricated discard entity.");
        Cold(game, registry);
    }

    public static void PublicPileStoragePaysDestroyedShopEquipmentWithoutInventingPileCardsCold()
    {
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], publicPileStorage: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var cardId = E<WushuangShopEquipmentCreatedEvent>(game).Single(item => item.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-native-tuifeng", [], [], game.Revision, P(game)!.PromptId)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "public-pile-incoming", [], [], game.Revision, P(game)!.PromptId));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "ol:tuifeng" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate").Id, game.Revision));
        Drive(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Cards.SequenceEqual([cardId])).Id, game.Revision));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == PublicPileShopWatch);
        var producer = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.PublicPileCashOut is not null);
        var receipt = producer.PublicPileCashOut!;
        var destruction = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == cardId);
        Require(receipt is { Operation: SkillProgramEffectOp.StoreBoundCardsInPublicPile, FrozenCount: 1 } &&
            receipt.PaidCardIds.SequenceEqual([cardId]) && receipt.PaidFrom.SequenceEqual([CardLocation.Equipment(0)]) &&
            destruction.ParentFrameId == producer.Id && game.CardMovements.Single(move => move.Sequence == destruction.MovementSequence) is
                { From.Zone: CardZoneKind.Equipment, To.Zone: CardZoneKind.OutsideGame } movement && movement.CardId == cardId &&
            movement.Sequence > receipt.Before && movement.Sequence <= receipt.After &&
            movement.Reason.Value == "skill-program.ol:tuifeng.StoreBoundCardsInPublicPile.payment",
            "The real stored equipment cost keeps its exact original instance and native invoice while the shop rule destroys its actual entity on the producing frame.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Play(game);
        Require(E<PublicPileCashOutPaidEvent>(game).Count(fact => fact.FrameId == producer.Id) == 1 &&
            E<PublicPileCashOutResolvedEvent>(game).Single(fact => fact.FrameId == producer.Id) is { FrozenCount: 1, DrawActual: 0, SlashLimitGranted: 0 } &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == cardId) == 1 &&
            game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Equipment(0)) == 1 &&
            game.CreateCardZoneDiagnostics().Single(entity => entity.CardId == cardId).Location == CardLocation.OutsideGame &&
            !game.CreateCardZoneDiagnostics().Any(entity => entity.CardId == cardId && entity.Location.Zone == CardZoneKind.PublicPersistentPile),
            "Cold return pays once and resolves storage without fabricating a public-pile card, later draw or Slash quota from destroyed equipment.");
        Cold(game, registry);
    }
}
