using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void SuishiReturnsItsExactDestroyedNativeShopMaterialCold()
    {
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 3, materialCase: "suishi");
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var card = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id).CardId;
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-material-flow", [], [], game.Revision, P(game)!.PromptId));
        Play(game); End(game);
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "anniversary:suishi" &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P(game)!;
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
        }
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Answer(choice => choice.Parameters.GetValueOrDefault("verb") == "name" && choice.Parameters.GetValueOrDefault("value") == nameof(CardKind.Slash));
        var normalSlashLabel = $"当【{CardCatalog.Get(CardKind.Slash).DisplayName}】使用";
        Answer(choice => choice.Parameters.GetValueOrDefault("verb") == "use" && choice.Cards.SequenceEqual([card]) &&
            choice.Targets.SequenceEqual([1]) && choice.Description.Contains(normalSlashLabel, StringComparison.Ordinal));
        var issued = E<AnniversaryYanXiangUseIssuedEvent>(game).Single(fact => fact.CardId == card);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == issued.UseId);
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == card);
        var movement = game.CardMovements.Single(move => move.Sequence == destroyed.MovementSequence);
        Require(use.CardKind == CardKind.Slash && use.AnniversaryYanXiangUse is { } origin && origin.FrameId == issued.FrameId && origin.CardId == card &&
            use.Action is { Type: CardActionType.Use, ActorSeat: 0, ProviderSeat: 0 } action && action.ActionId == issued.ActionId &&
            action.PhysicalCards is [{ CardId: var paid, From: var from }] && paid == card && from == CardLocation.Equipment(0) &&
            destroyed.ParentFrameId == use.Id && movement.From == from && movement.To == CardLocation.OutsideGame && movement.Reason == CardMoveReasons.Use,
            "Suishi retains the exact issued HE material, accepted action and native Use owner when shop equipment is destroyed by its real payment.");
        Cold(game, registry);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Drive(game, () => E<AnniversaryYanXiangCompletedEvent>(game).Any(fact => fact.FrameId == issued.FrameId));
        Require(E<AnniversaryYanXiangUseIssuedEvent>(game).Count(fact => fact.UseId == issued.UseId) == 1 &&
            E<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == issued.UseId && fact.CardId == card) == 1 &&
            E<AnniversaryYanXiangUseReturnedEvent>(game).Count(fact => fact.UseId == issued.UseId) == 1 &&
            E<AnniversaryYanXiangCompletedEvent>(game).Count(fact => fact.FrameId == issued.FrameId) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == card) == 1 &&
            game.CreateCardZoneDiagnostics().Single(entity => entity.CardId == card).Location == CardLocation.OutsideGame &&
            !game.CardMovements.Any(move => move.CardId == card && move.Sequence > movement.Sequence),
            "Cold finishes the original whole Use and skill once, accepting its exact destroyed-payment receipt without a second payment or fake Processing/Discard move.");
        Cold(game, registry);
    }
}
