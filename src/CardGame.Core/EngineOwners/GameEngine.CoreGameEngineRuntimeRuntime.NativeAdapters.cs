#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRuntimeResolutionSchedulerAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeResolutionSchedulerOperations
    {
        public ref readonly global::CardGame.Core.ResolutionScheduler _resolutionScheduler => ref engine._resolutionScheduler;
    }

    [ProgramCapability]
    private sealed class CoreRuntimeKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeKernelCoreMainOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }

    [ProgramCapability]
    private sealed class CoreRuntimeResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRuntimeMatchExecutionLoopAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeMatchExecutionLoopOperations
    {
        public ref readonly global::CardGame.Core.MatchExecutionLoop _matchExecutionLoop => ref engine._matchExecutionLoop;
    }

    [ProgramCapability]
    private sealed class CoreRuntimeKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreRuntimeProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CoreRuntimeKernelCoreGameEngineProgramContinuationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeKernelCoreGameEngineProgramContinuationsOperations
    {
        public bool TryReturnMovement(global::CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, global::CardGame.Core.ProgramSkillFrame frame, bool afterPayment = false) => __nativeReceiver.TryReturnMovement(engine, frame, afterPayment);
    }

    [ProgramCapability]
    private sealed class CoreRuntimeNativeCardGameCoreProgramWindowReturnCatalogAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeNativeCardGameCoreProgramWindowReturnCatalogOperations
    {
        public void Return(global::CardGame.Core.ProgramWindowReturnCatalog __nativeReceiver, global::CardGame.Core.ProgramSkillFrame frame, bool completed) => __nativeReceiver.Return(engine, frame, completed);
    }

    [ProgramCapability]
    private sealed class CoreRuntimeModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuntimeModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}
