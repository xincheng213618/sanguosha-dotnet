#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPairPaymentsEntries
{
    internal bool CanActivateEquipmentPairPayment(global::CardGame.Core.CharacterState owner, string skillId, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanActivateEquipmentPairPayment(owner, skillId, features);
    internal bool CanResumePaidEquipmentPair(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanResumePaidEquipmentPair(frame);
    internal bool EquipmentPairCostIsPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().EquipmentPairCostIsPaid(frame);
    internal global::CardGame.Core.SkillProgramEffect EquipmentPairOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().EquipmentPairOwnedSelectionEffect(frame, effect);
    internal global::CardGame.Core.ProgramSkillFrame FreezePaidEquipmentExchangeStart(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FreezePaidEquipmentExchangeStart(frame);
    internal bool IsIssuedEquipmentPairPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsIssuedEquipmentPairPayment(frame);
    internal bool IsPayableEquipmentPair(int ownerSeat, int first, int second, string skillId, string instanceId) => _resolve().IsPayableEquipmentPair(ownerSeat, first, second, skillId, instanceId);
    internal int LegalEquipmentPairCostCount(int ownerSeat, string skillId, string instanceId) => _resolve().LegalEquipmentPairCostCount(ownerSeat, skillId, instanceId);
    internal bool MatchesEquipmentPairOwnedCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().MatchesEquipmentPairOwnedCost(frame, bind, card, from);
    internal global::CardGame.Core.CardLocation OwnedPaymentDiscardDestination(int cardId) => _resolve().OwnedPaymentDiscardDestination(cardId);
    internal global::System.Collections.Generic.IReadOnlyList<(int First, int Second)> PayableEquipmentPairs(int ownerSeat, string skillId, string instanceId) => _resolve().PayableEquipmentPairs(ownerSeat, skillId, instanceId);
    internal void ResolveEquipmentPairPaymentChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveEquipmentPairPaymentChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiEquipmentPairPayment(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiEquipmentPairPayment(decision, frame);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectEquipmentPairAndPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectEquipmentPairAndPayment(frame, effect);
}
