using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string ShopPaidConverter = "fixture:shop-equipment-delayed", ShopPaidConversionWatch = "fixture:shop-paid-conversion-watch";

    public static void PurchasedEquipmentNullificationAndDelayedPlacementKeepExactDestroyedPaymentCold()
    {
        foreach (var response in new[] { true, false })
        {
            var (g, registry) = Start(false, 277, skills: ["wushuang:skill-3252"], initialHp: 1,
                aiQuota: 0, grantWangYunTrick: true, mixedTricks: true);
            Drive(g, () => Shop(g) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(g, Shop(g).Offers[0].Id); Shopping(g);
            var offer = Shop(g).Offers.Single(o => o.CardOfferId == 4186);
            Buy(g, offer.Id);
            var gear = E<WushuangShopEquipmentCreatedEvent>(g).Single(e => e.OfferId == offer.Id).CardId;
            Accept(g, new FinishWushuangShopCommand(0, g.Revision)); Play(g);
            Accept(g, new UseProgramSkillCommand(0, Driver, "shop-conversion-setup", [], [], g.Revision, P(g)!.PromptId)); Play(g);
            Cold(g, registry);
            var action = response ? g.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.DrawTwo) :
                g.GetHumanLegalActions().Single(a => a.CardId == gear && a.PlayedCardKind == CardKind.SupplyShortage);
            var before = E<CardUseDeclaredEvent>(g).Select(e => e.ResolutionId).ToHashSet();
            Accept(g, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, g.Revision, P(g)!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource,
                    AdditionalConversionSources = action.AdditionalConversionSources ?? [] });
            if (response)
            {
                Drive(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
                Cold(g, registry);
                g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
                var prompt = P(g)!;
                var choice = prompt.Choices.Single(c => c.Cards.SequenceEqual([gear]) &&
                    c.Parameters.GetValueOrDefault("conversion-skill-id") == "wushuang:skill-3329");
                Accept(g, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, g.Revision));
            }
            Drive(g, () => P(g)?.SkillPrompt?.SkillId == ShopPaidConversionWatch);
            var use = E<CardUseDeclaredEvent>(g).Last(e => !before.Contains(e.ResolutionId)).ResolutionId;
            var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(e => e.CardId == gear);
            var moved = g.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence);
            Require(moved.From == CardLocation.Equipment(0) && moved.To == CardLocation.OutsideGame &&
                moved.Reason == (response ? CardMoveReasons.Nullification : CardMoveReasons.Use) &&
                g.ResolutionStack.Any(f => f.Id == destroyed.ParentFrameId) &&
                (!response ? destroyed.ParentFrameId == use : g.ResolutionStack.OfType<NullificationWindowFrame>().Any(f => f.Id == destroyed.ParentFrameId)),
                "The original equipped shop entity pays once on its exact use/counterspell window and is truly destroyed before its real accepted child.");
            Cold(g, registry);
            g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
            var paid = P(g)!;
            Accept(g, new AnswerPromptCommand(0, paid.PromptId, paid.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, g.Revision));
            Play(g);
            Require(E<WushuangShopEquipmentDestroyedEvent>(g).Count(e => e.CardId == gear) == 1 &&
                g.CardMovements.Count(m => m.CardId == gear && m.From == CardLocation.Equipment(0)) == 1 &&
                !g.CardMovements.Any(m => m.CardId == gear && (m.To == CardLocation.DiscardPile || m.To.Zone == CardZoneKind.Judgment ||
                    m.From == CardLocation.OutsideGame && m.Sequence > moved.Sequence)) &&
                E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == use) == 1,
                "Returning cold neither discards nor resurrects the once-destroyed material; its native use finishes exactly once.");
            Require(response ? E<NullificationResolvedEvent>(g).Any(e => e.ResolutionId == use && e.EffectNullified) &&
                    E<CardRespondedEvent>(g).Count(e => e.CardId == gear && e.EffectiveCardKind == CardKind.Nullification) == 1 :
                    !E<DelayedCardPlacedEvent>(g).Any(e => e.ResolutionId == use),
                "The paid Nullification actually cancels its trick; a destroyed delayed-trick material cannot be placed in Judgment.");
            Cold(g, registry);
        }
    }
}
