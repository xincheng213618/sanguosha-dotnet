#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiFuRenBoundaryMobileEntries
{
    internal void AnswerBoundaryMobileQieting(ProgramSkillFrame frame, PromptChoice choice) => _resolve().AnswerBoundaryMobileQieting(frame, choice);
    internal IReadOnlyList<PromptChoice> BoundaryMobileQietingHandChoices(ProgramSkillFrame frame) => _resolve().BoundaryMobileQietingHandChoices(frame);
    internal CardSnapshot[] BoundaryMobileQietingPrivateCards(int viewer) => _resolve().BoundaryMobileQietingPrivateCards(viewer);
    internal bool BoundaryMobileQietingRecoveryMaterial(ProgramSkillFrame frame, int source, int target, int amount, int? cardId, CardMoveReason? reason) => _resolve().BoundaryMobileQietingRecoveryMaterial(frame, source, target, amount, cardId, reason);
    internal bool CaiFuRenQietingDisqualified(string skill, OrdinaryQietingTurnContext qualification) => _resolve().CaiFuRenQietingDisqualified(skill, qualification);
    internal PromptChoice ChooseBoundaryMobileQietingAi(PendingDecision decision, ProgramSkillFrame frame) => _resolve().ChooseBoundaryMobileQietingAi(decision, frame);
    internal bool IsBoundaryMobileQietingHpChild(ProgramSkillFrame frame, HpChangedTriggerWindowFrame hp) => _resolve().IsBoundaryMobileQietingHpChild(frame, hp);
    internal bool IsBoundaryMobileQietingPaidMovement(ProgramSkillFrame frame, CardMovementRecord move) => _resolve().IsBoundaryMobileQietingPaidMovement(frame, move);
    internal bool IsBoundaryMobileQietingRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsBoundaryMobileQietingRecovery(frame, recovery);
}
