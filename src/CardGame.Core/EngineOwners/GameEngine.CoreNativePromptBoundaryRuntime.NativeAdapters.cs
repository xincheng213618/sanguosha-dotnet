#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativePromptBoundaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreNativePromptBoundaryProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }
}
