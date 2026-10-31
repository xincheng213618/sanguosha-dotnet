#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreDelayedTurnEffectsState : global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess
{
    private global::CardGame.Core.GameEngine.DelayedTurnEffects _pendingTurnDelayedEffects = default !;
    internal ref global::CardGame.Core.GameEngine.DelayedTurnEffects ReferenceToPendingTurnDelayedEffects => ref _pendingTurnDelayedEffects;

    ref global::CardGame.Core.GameEngine.DelayedTurnEffects global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects => ref _pendingTurnDelayedEffects;
}
