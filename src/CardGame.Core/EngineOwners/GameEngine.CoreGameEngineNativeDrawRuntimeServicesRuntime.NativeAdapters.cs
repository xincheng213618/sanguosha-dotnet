#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeDrawRuntimeServicesNativeDrawReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesNativeDrawReturnsOperations
    {
        public void ReturnOperation(global::CardGame.Core.SkillProgramEffectOp operation, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome)
        {
            var method = global::CardGame.Core.EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value[operation];
            ((global::System.Action<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.NativeDrawOutcome>)engine.BindRuntimeHandler(method, typeof(global::System.Action<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.NativeDrawOutcome>)))(frame, outcome);
        }
    }

    [ProgramCapability]
    private sealed class CoreNativeDrawRuntimeServicesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreNativeDrawRuntimeServicesProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDrawRuntimeServicesProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }
}
