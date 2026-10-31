#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawPhaseAttemptsRuntime
{
    internal bool TryBeginNormalDrawAttemptPhase(CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayed, bool afterStartingWindow = false, bool replaced = false, int adjustment = 0, int? frozenBase = null) => _ownedDrawPhaseOperations.Get().TryBeginNormalDrawAttemptPhase(owner, (int)delayed, afterStartingWindow, replaced, adjustment, frozenBase);
}
