#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreDelayedTurnEffectsState : Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess
{
    private GameEngine.DelayedTurnEffects _pendingTurnDelayedEffects = default !;
    internal ref GameEngine.DelayedTurnEffects ReferenceToPendingTurnDelayedEffects => ref _pendingTurnDelayedEffects;

    ref GameEngine.DelayedTurnEffects Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects => ref _pendingTurnDelayedEffects;
}
