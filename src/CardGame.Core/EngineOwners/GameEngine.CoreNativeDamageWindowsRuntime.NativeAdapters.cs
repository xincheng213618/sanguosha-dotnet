#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeDamageWindowsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeDamageWindowsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreNativeDamageWindowsProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }
}
