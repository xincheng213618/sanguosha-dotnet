#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramDodgeResponsesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreProgramDodgeResponsesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramDodgeResponsesKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreProgramDodgeResponsesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
