using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void PurchasedEquipmentJianyingAlcoholFinishesExactDestroyedMaterialCold()
    {
        var (g, registry, id) = PrepareMaterialFlow("alcohol");
        var action = g.GetHumanLegalActions().Single(a => a.CardId == id && a.PlayedCardKind == CardKind.Alcohol &&
            a.ConversionSource?.SkillId == "boundary:jianying-mobile");
        Accept(g, new PlayCardCommand(0, id, action.TargetSeats, g.Revision, P(g)!.PromptId,
            action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource,
                AdditionalConversionSources = action.AdditionalConversionSources ?? [] });
        Drive(g, () => P(g)?.SkillPrompt?.SkillId == MaterialFlowWatch);
        var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == id);
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(e => e.CardId == id);
        var movement = g.CardMovements.Single(m => m.Sequence == destroyed.MovementSequence);
        Require(use.CardKind == CardKind.Alcohol && use.ProgramUseCommitted && !use.ProgramUseAccepted &&
                use.PhysicalCardIds is [var material] && material == id &&
                use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Alcohol } original &&
                original.ActorSeat == 0 && original.ProviderSeat == 0 &&
                original.ConversionChain is [{ SkillId: "boundary:jianying-mobile", BindingId: "as-alcohol" }] &&
                original.PhysicalCards is [var cost] && cost.CardId == id && cost.From == CardLocation.Equipment(0) &&
                cost.CardKind == movement.CardKind && E<CardActionAcceptedEvent>(g).All(e => e.Action.ActionId != original.ActionId) &&
                destroyed.ParentFrameId == use.Id && movement.From == cost.From &&
                movement.To == CardLocation.OutsideGame && movement.Reason == CardMoveReasons.Use &&
                E<WushuangShopEquipmentCreatedEvent>(g).Count(e => e.CardId == id && e.Seat == 0) == 1,
            "The real Jianying Alcohol retains its exact committed native action, owned equipped material and single producer destruction before its committed child; target acceptance still follows this child.");
        var useId = use.Id;
        var actionId = use.Action!.ActionId;
        g = CompleteMaterialFlowChild(g, registry);
        Require(E<CardActionAcceptedEvent>(g).Count(e => e.Action.ActionId == actionId) == 1 &&
                E<AlcoholAppliedEvent>(g).Count(e => e.ResolutionId == useId && e.SourceSeat == 0 && e.DamageBonus == 1) == 1 &&
                E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == useId && e.CardId == id && e.CardKind == CardKind.Alcohol) == 1 &&
                E<WushuangShopEquipmentDestroyedEvent>(g).Count(e => e.CardId == id) == 1 &&
                g.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Equipment(0)) == 1 &&
                !g.CardMovements.Any(m => m.CardId == id && m.Sequence > movement.Sequence) &&
                g.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location == CardLocation.OutsideGame &&
                g.CreateSnapshot(0).ProcessingCardCount == 0 && g.ResolutionStack.Count == 0 &&
                g.GetHumanLegalActions().All(a => a.ConversionSource?.SkillId != "boundary:jianying-mobile"),
            "Cold completion applies the real Alcohol once, spends the shared phase conversion once, and never fabricates Processing, discard or another movement for the destroyed material.");
        Cold(g, registry);
    }
}
