#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramLifecycleResolutionSchedulerAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleResolutionSchedulerOperations
    {
        public ref readonly global::CardGame.Core.ResolutionScheduler _resolutionScheduler => ref engine._resolutionScheduler;
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreProgramTriggerRuntimeQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreProgramTriggerRuntimeQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries() => engine.ComposeNativeProgramTriggerQueries();
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreProgramTriggerFactProjectorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreProgramTriggerFactProjectorQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
        public global::CardGame.Core.NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries() => engine.ComposeNativeProgramTriggerActionFactQueries();
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleProgramInteractionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleProgramInteractionsOperations
    {
        public ref global::CardGame.Core.ProgramInteractionRouter? _programInteractions => ref engine._programInteractions;
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreProgramLifecycleContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
