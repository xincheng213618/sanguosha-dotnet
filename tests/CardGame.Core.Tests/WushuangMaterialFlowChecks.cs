using CardGame.Core;

internal static partial class WushuangModeChecks
{
    private const string MaterialFlowWatch = "fixture:wushuang-material-flow-watch";

    private static (GameEngine Game, ContentRegistry Registry, int CardId) PrepareMaterialFlow(string materialCase, bool slashDeck = false)
    {
        var (g, registry) = Start(slashDeck, 277, skills: ["wushuang:skill-3252"], initialHp: 20, materialCase: materialCase);
        Drive(g, () => Shop(g) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Buy(g, Shop(g).Offers[0].Id); Shopping(g);
        var offer = Shop(g).Offers.Single(item => item.CardOfferId == 4186); Buy(g, offer.Id);
        var id = E<WushuangShopEquipmentCreatedEvent>(g).Single(fact => fact.OfferId == offer.Id).CardId;
        Accept(g, new FinishWushuangShopCommand(0, g.Revision)); Play(g);
        Accept(g, new UseProgramSkillCommand(0, Driver, "acquire-material-flow", [], [], g.Revision, P(g)!.PromptId)); Play(g);
        return (g, registry, id);
    }

    private static GameEngine CompleteMaterialFlowChild(GameEngine g, ContentRegistry registry)
    {
        Cold(g, registry);
        g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        var prompt = P(g)!;
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, g.Revision));
        Play(g); return g;
    }

    public static void HuomoDodgeRetainsExactDestroyedEquipmentResponsePaymentCold()
    {
        var (g, registry, id) = PrepareMaterialFlow("huomo", slashDeck: true);
        End(g); Drive(g, () => P(g) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        Cold(g, registry);
        g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        var prompt = P(g)!;
        var choice = prompt.Choices.Single(option => option.Cards.SequenceEqual([id]) &&
            option.Parameters.GetValueOrDefault("conversion-skill-id") == "classic:huomo");
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, g.Revision));
        Drive(g, () => P(g)?.SkillPrompt?.SkillId == MaterialFlowWatch);
        var completed = g.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
        var nativeCost = completed.ResponseCompletion!.NativeCosts.Single();
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(fact => fact.CardId == id);
        Require(completed.Action.ConversionChain is [{ SkillId: "classic:huomo" }] &&
            completed.Action.PhysicalCards is [{ From.Zone: CardZoneKind.Equipment }] &&
            nativeCost.CardId == id && nativeCost.From == CardLocation.Equipment(0) && nativeCost.To == CardLocation.OutsideGame &&
            nativeCost.Reason == CardMoveReasons.Respond && destroyed.MovementSequence == nativeCost.Sequence &&
            destroyed.ParentFrameId == completed.ResponseCompletion.ParentFrameId,
            "A genuine Huomo Dodge keeps its once-paid original equipment and native destruction on the exact Slash owner, without inventing a draw-pile-top movement.");
        var actionId = completed.Action.ActionId;
        g = CompleteMaterialFlowChild(g, registry);
        Require(E<CardResponseCompletedEvent>(g).Count(fact => fact.ActionId == actionId) == 1 &&
            E<WushuangShopEquipmentDestroyedEvent>(g).Count(fact => fact.CardId == id) == 1 &&
            g.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Equipment(0)) == 1 &&
            !g.CardMovements.Any(move => move.CardId == id && (move.To == CardLocation.DrawPile || move.From == CardLocation.OutsideGame && move.Sequence > nativeCost.Sequence)),
            "Cold native defense completes once and never recycles or moves its destroyed alternative payment again.");
        Cold(g, registry);
    }

    public static void JiqiaoCountsExactDestroyedShopEquipmentAndCompletesRevealCold()
    {
        var (g, registry, id) = PrepareMaterialFlow("jiqiao");
        End(g); Drive(g, () => P(g)?.SkillPrompt?.SkillId == "ol:jiqiao" &&
            P(g)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var prompt = P(g)!;
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate").Id, g.Revision));
        Drive(g, () => P(g)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        prompt = P(g)!;
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(choice => choice.Cards.SequenceEqual([id])).Id, g.Revision));
        if (P(g)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true)
        {
            prompt = P(g)!;
            Accept(g, new AnswerPromptCommand(0, prompt.PromptId,
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards").Id, g.Revision));
        }
        Drive(g, () => P(g)?.SkillPrompt?.SkillId == MaterialFlowWatch);
        var paid = E<JiqiaoCostPaidEvent>(g).Single();
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(fact => fact.CardId == id);
        Require(paid.Invoice.PaidCount == 1 && destroyed.ParentFrameId == paid.ProgramFrameId &&
            destroyed.MovementSequence > paid.Invoice.CostBefore && destroyed.MovementSequence <= paid.Invoice.CostAfter,
            "The actual Play-start Jiqiao invoice pays the purchased equipment once through its real destruction on the original producer frame.");
        g = CompleteMaterialFlowChild(g, registry);
        Require(E<JiqiaoCompletedEvent>(g).Single(fact => fact.ProgramFrameId == paid.ProgramFrameId) is
                { PaidCount: 1, RevealedCount: 2, ClaimedCount: 2 } &&
            E<JiqiaoStageIssuedEvent>(g).Where(fact => fact.ProgramFrameId == paid.ProgramFrameId).Select(fact => fact.Stage)
                .SequenceEqual(Enum.GetValues<JiqiaoPaidStage>()) &&
            g.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Equipment(0)) == 1,
            "Cold Jiqiao uses the exact paid count to reveal and obtain two genuine non-equipment cards, retaining every native stage once.");
        Cold(g, registry);
    }

    public static void SelectedEquipmentReturnStopsAfterExactNativeDestructionCold()
    {
        var (g, registry, id) = PrepareMaterialFlow("return");
        Accept(g, new UseProgramSkillCommand(0, Driver, "return-self-equipment", [], [], g.Revision, P(g)!.PromptId));
        Drive(g, () => P(g)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        var prompt = P(g)!;
        Cold(g, registry);
        g = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        prompt = P(g)!;
        Accept(g, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(choice => choice.Cards.SequenceEqual([id])).Id, g.Revision));
        Drive(g, () => P(g)?.SkillPrompt?.SkillId == MaterialFlowWatch);
        var destroyed = E<WushuangShopEquipmentDestroyedEvent>(g).Single(fact => fact.CardId == id);
        g = CompleteMaterialFlowChild(g, registry);
        Require(g.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Equipment(0)) == 1 &&
            g.CardMovements.Single(move => move.Sequence == destroyed.MovementSequence) is { To.Zone: CardZoneKind.OutsideGame } &&
            !g.CardMovements.Any(move => move.CardId == id && move.Sequence > destroyed.MovementSequence) &&
            !g.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id),
            "The original legal same-owner equipment return drains its true destruction child once; it cannot run a second Processing transfer or fabricate a hand gain.");
        Cold(g, registry);
    }
}
