#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePhysicalCardDrawModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CorePhysicalCardDrawModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}
