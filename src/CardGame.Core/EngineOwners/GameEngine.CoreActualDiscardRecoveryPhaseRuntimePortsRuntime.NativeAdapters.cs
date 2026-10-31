#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActualDiscardRecoveryPhaseRuntimePortsModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualDiscardRecoveryPhaseRuntimePortsModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }
}
