#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFinalizedCardTargetsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreFinalizedCardTargetsModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CoreFinalizedCardTargetsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreFinalizedCardTargetsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
