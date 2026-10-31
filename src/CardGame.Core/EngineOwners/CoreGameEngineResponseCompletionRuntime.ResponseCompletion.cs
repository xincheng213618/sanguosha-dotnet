#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseCompletionRuntime
{
    internal static bool IsResponseCompletionContinuation(ProgramCardContinuation continuation) => RuntimeSkills.RuntimeRules_Core_ResponseCompletion.IsResponseCompletionContinuation(continuation);
    internal bool HasCardResponseCompletedObserver(CardActionContext action, ProgramCardContinuation continuation) => _responseCompletionOperations.Get().HasCardResponseCompletedObserver(action, continuation);
    internal bool IsJueshengPaidDuelResponse(CardActionContext action, ProgramCardContinuation continuation) => _responseCompletionOperations.Get().IsJueshengPaidDuelResponse(action, continuation);
    internal bool TryBeginCardResponseCompleted(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation originalContinuation) => _responseCompletionOperations.Get().TryBeginCardResponseCompleted(attack?.ResolutionId, action, originalContinuation);
    internal CardMovementRecord[] CaptureResponseCompletionCosts(CardActionContext a, ProgramCardContinuation continuation, long parentFrameId) => _responseCompletionOperations.Get().CaptureResponseCompletionCosts(a, continuation, parentFrameId);
    internal static bool IsResponseCompletionEntryReason(CardActionContext action, ProgramCardContinuation continuation, CardMoveReason reason) => RuntimeSkills.RuntimeRules_Core_ResponseCompletion.IsResponseCompletionEntryReason(action, continuation, reason);
    internal static CardMoveReason ResponseCompletionFinishReason(CardMovementRecord entered, ProgramCardContinuation continuation) => RuntimeSkills.RuntimeRules_Core_ResponseCompletion.ResponseCompletionFinishReason(entered, continuation);
    internal bool IsResponseCompletionEntry(CardActionContext action, ProgramCardContinuation continuation, long parentFrameId, CardActionCost cost, CardMovementRecord moved) => _responseCompletionOperations.Get().IsResponseCompletionEntry(action, continuation, parentFrameId, cost, moved);
    internal static CardResponseCompletedEvent ResponseCompletionFact(ProgramCardTriggerWindowFrame window) => RuntimeSkills.RuntimeRules_Core_ResponseCompletion.ResponseCompletionFact(window);
    internal static CardActionContext ResponseCompletionActorAction(ProgramCardTriggerWindowFrame window) => RuntimeSkills.RuntimeRules_Core_ResponseCompletion.ResponseCompletionActorAction(window);
    internal bool IsResponseCompletionCostFinished(ProgramCardTriggerWindowFrame window, CardActionCost cost) => _responseCompletionOperations.Get().IsResponseCompletionCostFinished(window, cost);
    internal bool IsResponseCompletionProgramDuelParent(ProgramSkillFrame frame, ProgramCardContinuation continuation) => _responseCompletionOperations.Get().IsResponseCompletionProgramDuelParent(frame, continuation);
    internal bool ValidResponseCompletionParent(ProgramCardTriggerWindowFrame window) => _responseCompletionOperations.Get().ValidResponseCompletionParent(window);
    internal bool ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame window) => _responseCompletionOperations.Get().ValidResponseCompletionWindow(window);
    internal bool TryDrainCardResponseCompletionCosts(ProgramCardTriggerWindowFrame supplied) => _responseCompletionOperations.Get().TryDrainCardResponseCompletionCosts(supplied);
    internal bool ResponseCompletionFirstChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _responseCompletionOperations.Get().ResponseCompletionFirstChild(window, child);
    internal bool ReturnCardResponseCompletionMovement(CardsMovedTriggerWindowFrame child) => _responseCompletionOperations.Get().ReturnCardResponseCompletionMovement(child);
    internal void ContinueCardResponseAfterCompletion(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame window) => _responseCompletionOperations.Get().ContinueCardResponseAfterCompletion(attack?.ResolutionId, window);
    internal bool ResponseCompletionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _responseCompletionOperations.Get().ResponseCompletionStructuralEdge(parent, child);
}
