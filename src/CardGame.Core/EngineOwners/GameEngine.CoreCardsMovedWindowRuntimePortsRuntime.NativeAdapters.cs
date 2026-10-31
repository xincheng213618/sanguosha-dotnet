#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCardsMovedWindowRuntimePortsProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CoreCardsMovedWindowRuntimePortsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
