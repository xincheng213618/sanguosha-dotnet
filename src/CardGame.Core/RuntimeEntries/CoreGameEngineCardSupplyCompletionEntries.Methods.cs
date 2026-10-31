#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSupplyCompletionEntries
{
    internal bool CardSupplyCompletionFirstChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _resolve().CardSupplyCompletionFirstChild(window, child);
    internal ProgramCardTriggerWindowFrame? CardSupplyCompletionObserverRoot() => _resolve().CardSupplyCompletionObserverRoot();
    internal bool CardSupplyCompletionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().CardSupplyCompletionStructuralEdge(parent, child);
    internal void ContinueCardSupplyAfterCompletion(NativeOwners.CardAttackHandle attack, ProgramCardTriggerWindowFrame window) => _resolve().ContinueCardSupplyAfterCompletion(attack, window);
    internal bool IsCardSupplyCompletionCostPaid(ProgramCardTriggerWindowFrame window, CardActionCost cost) => _resolve().IsCardSupplyCompletionCostPaid(window, cost);
    internal bool IsCardSupplyCompletionEntry(CardActionContext action, long useId, long requestOwnerId, CardActionCost cost, CardMovementRecord moved) => _resolve().IsCardSupplyCompletionEntry(action, useId, requestOwnerId, cost, moved);
    internal bool IsCardSupplyCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _resolve().IsCardSupplyCompletionHealthChild(window, child);
    internal bool ReturnCardSupplyCompletionHpChange(HpChangedTriggerWindowFrame child) => _resolve().ReturnCardSupplyCompletionHpChange(child);
    internal bool ReturnCardSupplyCompletionMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnCardSupplyCompletionMovement(child);
    internal bool ReturnCardSupplyCompletionRecovery(RecoveryReplacementFrame child) => _resolve().ReturnCardSupplyCompletionRecovery(child);
    internal bool TryBeginProvidedCardSupplyCompletion(NativeOwners.CardAttackHandle attack, NativeOwners.FactionCardRequestHandle? request) => _resolve().TryBeginProvidedCardSupplyCompletion(attack, request);
    internal bool TryDrainCardSupplyCompletionCosts(ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardSupplyCompletionCosts(supplied);
    internal bool TryDrainCardSupplyCompletionHealth(ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardSupplyCompletionHealth(supplied);
    internal bool ValidCardSupplyCompletionWindow(ProgramCardTriggerWindowFrame window) => _resolve().ValidCardSupplyCompletionWindow(window);
}
