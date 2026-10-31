#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreInitialHandModifiersModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreInitialHandModifiersModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreInitialHandModifiersModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreInitialHandModifiersModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }
}
