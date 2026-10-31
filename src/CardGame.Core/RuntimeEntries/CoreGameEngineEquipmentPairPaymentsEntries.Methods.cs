#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPairPaymentsEntries
{
    internal bool CanActivateEquipmentPairPayment(CharacterState owner, string skillId, ProgramInstructionFeatures features) => _resolve().CanActivateEquipmentPairPayment(owner, skillId, features);
    internal bool CanResumePaidEquipmentPair(ProgramSkillFrame frame) => _resolve().CanResumePaidEquipmentPair(frame);
    internal bool EquipmentPairCostIsPaid(ProgramSkillFrame frame) => _resolve().EquipmentPairCostIsPaid(frame);
    internal SkillProgramEffect EquipmentPairOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().EquipmentPairOwnedSelectionEffect(frame, effect);
    internal ProgramSkillFrame FreezePaidEquipmentExchangeStart(ProgramSkillFrame frame) => _resolve().FreezePaidEquipmentExchangeStart(frame);
    internal bool IsIssuedEquipmentPairPayment(ProgramSkillFrame frame) => _resolve().IsIssuedEquipmentPairPayment(frame);
    internal bool IsPayableEquipmentPair(int ownerSeat, int first, int second, string skillId, string instanceId) => _resolve().IsPayableEquipmentPair(ownerSeat, first, second, skillId, instanceId);
    internal int LegalEquipmentPairCostCount(int ownerSeat, string skillId, string instanceId) => _resolve().LegalEquipmentPairCostCount(ownerSeat, skillId, instanceId);
    internal bool MatchesEquipmentPairOwnedCost(ProgramSkillFrame frame, string bind, Card card, CardLocation from) => _resolve().MatchesEquipmentPairOwnedCost(frame, bind, card, from);
    internal CardLocation OwnedPaymentDiscardDestination(int cardId) => _resolve().OwnedPaymentDiscardDestination(cardId);
    internal IReadOnlyList<(int First, int Second)> PayableEquipmentPairs(int ownerSeat, string skillId, string instanceId) => _resolve().PayableEquipmentPairs(ownerSeat, skillId, instanceId);
    internal void ResolveEquipmentPairPaymentChoice(PromptChoice choice) => _resolve().ResolveEquipmentPairPaymentChoice(choice);
    internal PromptChoice SelectAiEquipmentPairPayment(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiEquipmentPairPayment(decision, frame);
    internal SkillProgramStepOutcome SelectEquipmentPairAndPayment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectEquipmentPairAndPayment(frame, effect);
}
