#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawPhaseAttemptsEntries
{
    internal bool TryBeginNormalDrawAttemptPhase(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayed, bool afterStartingWindow = false, bool replaced = false, int adjustment = 0, int? frozenBase = default) => _resolve().TryBeginNormalDrawAttemptPhase(owner, delayed, afterStartingWindow, replaced, adjustment, frozenBase);
}
