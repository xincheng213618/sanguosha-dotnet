#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess : IProgramRuntimeOperations
{
    ref GameEngine.DelayedTurnEffects _pendingTurnDelayedEffects { get; }
}
