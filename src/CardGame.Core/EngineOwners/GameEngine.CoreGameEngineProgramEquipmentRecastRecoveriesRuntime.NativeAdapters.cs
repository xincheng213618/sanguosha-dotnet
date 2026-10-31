#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramEquipmentRecastRecoveriesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramEquipmentRecastRecoveriesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreProgramEquipmentRecastRecoveriesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramEquipmentRecastRecoveriesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramEquipmentRecastRecoveriesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramEquipmentRecastRecoveriesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
