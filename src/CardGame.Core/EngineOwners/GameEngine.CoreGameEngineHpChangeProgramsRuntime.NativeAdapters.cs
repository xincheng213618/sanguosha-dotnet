#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHpChangeProgramsProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CoreHpChangeProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreHpChangeProgramsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreHpChangeProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreHpChangeProgramsKernelCoreGameEngineProgramTriggerWindowsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsKernelCoreGameEngineProgramTriggerWindowsOperations
    {
        public global::CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
    }
}
