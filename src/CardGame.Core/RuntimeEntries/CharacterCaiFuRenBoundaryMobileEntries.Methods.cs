#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiFuRenBoundaryMobileEntries
{
    internal void AnswerBoundaryMobileQieting(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryMobileQieting(frame, choice);
    internal void AssertBoundaryMobileQieting(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertBoundaryMobileQieting(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryMobileQietingHandChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryMobileQietingHandChoices(frame);
    internal global::CardGame.Core.CardSnapshot[] BoundaryMobileQietingPrivateCards(int viewer) => _resolve().BoundaryMobileQietingPrivateCards(viewer);
    internal bool BoundaryMobileQietingRecoveryMaterial(global::CardGame.Core.ProgramSkillFrame frame, int source, int target, int amount, int? cardId, global::CardGame.Core.CardMoveReason? reason) => _resolve().BoundaryMobileQietingRecoveryMaterial(frame, source, target, amount, cardId, reason);
    internal bool CaiFuRenQietingDisqualified(string skill, global::CardGame.Core.OrdinaryQietingTurnContext qualification) => _resolve().CaiFuRenQietingDisqualified(skill, qualification);
    internal global::CardGame.Core.PromptChoice ChooseBoundaryMobileQietingAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseBoundaryMobileQietingAi(decision, frame);
    internal bool IsBoundaryMobileQietingHpChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => _resolve().IsBoundaryMobileQietingHpChild(frame, hp);
    internal bool IsBoundaryMobileQietingPaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord move) => _resolve().IsBoundaryMobileQietingPaidMovement(frame, move);
    internal bool IsBoundaryMobileQietingRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsBoundaryMobileQietingRecovery(frame, recovery);
}
