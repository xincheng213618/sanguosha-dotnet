#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSupplyCompletionEntries
{
    internal bool CardSupplyCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => _resolve().CardSupplyCompletionFirstChild(window, child);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? CardSupplyCompletionObserverRoot() => _resolve().CardSupplyCompletionObserverRoot();
    internal bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().CardSupplyCompletionStructuralEdge(parent, child);
    internal void ContinueCardSupplyAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ContinueCardSupplyAfterCompletion(attack, window);
    internal bool IsCardSupplyCompletionCostPaid(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost) => _resolve().IsCardSupplyCompletionCostPaid(window, cost);
    internal bool IsCardSupplyCompletionEntry(global::CardGame.Core.CardActionContext action, long useId, long requestOwnerId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord moved) => _resolve().IsCardSupplyCompletionEntry(action, useId, requestOwnerId, cost, moved);
    internal bool IsCardSupplyCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => _resolve().IsCardSupplyCompletionHealthChild(window, child);
    internal bool ReturnCardSupplyCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child) => _resolve().ReturnCardSupplyCompletionHpChange(child);
    internal bool ReturnCardSupplyCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnCardSupplyCompletionMovement(child);
    internal bool ReturnCardSupplyCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().ReturnCardSupplyCompletionRecovery(child);
    internal bool TryBeginProvidedCardSupplyCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request) => _resolve().TryBeginProvidedCardSupplyCompletion(attack, request);
    internal bool TryDrainCardSupplyCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardSupplyCompletionCosts(supplied);
    internal bool TryDrainCardSupplyCompletionHealth(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardSupplyCompletionHealth(supplied);
    internal bool ValidCardSupplyCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ValidCardSupplyCompletionWindow(window);
}
