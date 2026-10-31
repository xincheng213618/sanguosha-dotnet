#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheS5ZhulingKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ZhulingKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ZhulingKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ZhulingKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ZhulingResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ZhulingResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
