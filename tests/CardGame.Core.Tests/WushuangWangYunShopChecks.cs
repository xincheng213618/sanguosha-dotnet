using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string WangYunShopWatch = "fixture:wushuang-wang-yun-watch";

    public static void WangYunJingongShopEquipmentPaysExactNativeUseAndReturnsCold()
    {
        // Grant after the fixed shop page is issued: printed/acquired skills
        // must not change this seed's original native equipment offer list.
        var (game, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 20, grantWangYunTrick: true);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(game, Shop(game).Offers[0].Id); Shopping(game);
        var offer = Shop(game).Offers.Single(item => item.CardOfferId == 4186);
        Buy(game, offer.Id);
        var created = E<WushuangShopEquipmentCreatedEvent>(game).Single(fact => fact.OfferId == offer.Id);
        Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
        Accept(game, new UseProgramSkillCommand(0, Driver, "acquire-native-jingong", [], [], game.Revision, P(game)!.PromptId)); Play(game);
        var activation = game.GetHumanLegalActions().Single(action => action.ProgramSkillId == "ol:jingong" &&
            action.ProgramActivationId == "three-random-tricks");
        Require(activation.SelectableCardIds.Contains(created.CardId) && game.CreateCardZoneDiagnostics().Single(card => card.CardId == created.CardId).Location == CardLocation.Equipment(0),
            "The real purchased equipment remains the original publicly equipped entity and is published by the newly granted native Jingong activation.");
        Accept(game, new UseProgramSkillCommand(0, activation.ProgramSkillId!, activation.ProgramActivationId!, [created.CardId], [], game.Revision, P(game)!.PromptId));
        Drive(game, () => P(game)?.SkillPrompt?.SkillId == "ol:jingong" && P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "trick"));
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.WangYunTrick is not null);
        var original = root.WangYunTrick!;
        Require(original.CardId == created.CardId && original.From == CardLocation.Equipment(0) && original.Samples.Distinct().Count() == 3,
            "The original unpaid random-trick frame binds the exact physical equipment source and three genuine sampled native templates.");
        Cold(game, registry);
        var templateCheckpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var publishedKinds = P(game)!.Choices.Where(item => item.Parameters.GetValueOrDefault("branch") == "trick")
            .Select(item => Enum.Parse<CardKind>(item.Parameters["effective-kind"])).Distinct().ToArray();
        Require(publishedKinds.Length > 0 && publishedKinds.All(original.Samples.Contains),
            "Only original random templates that have a currently published legal target are replayed; unavailable sampled names never become forged choices.");
        foreach (var kind in publishedKinds)
        {
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(templateCheckpoint), registry);
            var choice = P(game)!.Choices.First(item => item.Parameters.GetValueOrDefault("effective-kind") == kind.ToString());
            Accept(game, new AnswerPromptCommand(0, P(game)!.PromptId, choice.Id, game.Revision));
            Drive(game, () => P(game)?.SkillPrompt?.SkillId == WangYunShopWatch);
            var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.WangYunNativeOrigin?.ProgramFrameId == root.Id);
            var paid = E<WangYunNativePaymentCompletedEvent>(game).Single(fact => fact.Origin.UseId == use.Id).Origin;
            var material = paid.Materials.Single();
            var destroyed = E<WushuangShopEquipmentDestroyedEvent>(game).Single(fact => fact.CardId == created.CardId);
            var movement = game.CardMovements.Single(move => move.Sequence == destroyed.MovementSequence);
            Require(paid is { Kind: "trick", PaymentCompleted: true } && paid.Source == original.Source && paid.GameplayHash == original.GameplayHash &&
                paid.ProgramFrameId == root.Id && paid.ActionId == use.Action!.ActionId && paid.Actor == 0 && paid.OutputKind == kind && use.CardKind == kind &&
                JsonSerializer.Serialize(use.WangYunNativeOrigin) == JsonSerializer.Serialize(paid) && use.Action.PhysicalCards.SequenceEqual(paid.Materials) &&
                material.CardId == created.CardId && material.CardKind == original.PrintedKind && material.From == original.From &&
                created.Seat == 0 && destroyed.ParentFrameId == use.Id && movement.CardId == material.CardId && movement.CardKind == material.CardKind &&
                movement.From == material.From && movement.To == CardLocation.OutsideGame && movement.Reason == CardMoveReasons.Use &&
                paid.Before < movement.Sequence && movement.Sequence <= paid.After,
                "The same original skill/source/hash/native Use pays its accepted equipment once through the actual shop destruction ledger on that exact Use parent, rather than pretending it reached Processing.");
            Cold(game, registry);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Accept(game, new AnswerPromptCommand(0, P(game)!.PromptId,
                P(game)!.Choices.Single(item => item.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
            Play(game);
            Require(E<WangYunNativePaymentCompletedEvent>(game).Count(fact => fact.Origin.UseId == use.Id) == 1 &&
                E<WangYunNativeUseReturnedEvent>(game).Count(fact => fact.UseId == use.Id && fact.ProgramFrameId == root.Id && fact.ActionId == paid.ActionId) == 1 &&
                E<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.Id) == 1 &&
                E<WushuangShopEquipmentDestroyedEvent>(game).Count(fact => fact.CardId == created.CardId) == 1 &&
                game.CardMovements.Count(move => move.CardId == created.CardId && move.From == original.From) == 1 &&
                !game.CardMovements.Any(move => move.CardId == created.CardId && (move.To == CardLocation.Processing || move.To == CardLocation.DiscardPile ||
                    move.From == CardLocation.OutsideGame && move.Sequence > movement.Sequence)) &&
                !game.ResolutionStack.Any(frame => frame.Id == root.Id || frame.Id == use.Id),
                "Cold continuation completes the original native trick and returns to its exact Jingong parent once, without paying, destroying or recycling the accepted equipment again.");
            Cold(game, registry);
        }
    }
}
