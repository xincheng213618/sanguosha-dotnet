#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.GameEngine.DelayedTurnEffects _pendingTurnDelayedEffects { get; }
}
