#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePlayerViewProjectionQueriesProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CorePlayerViewProjectionQueriesKernelCoreGameEngineActivePlayerEffectQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesKernelCoreGameEngineActivePlayerEffectQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries() => engine.ComposeNativeActivePlayerEffectQueries();
    }

    [ProgramCapability]
    private sealed class CorePlayerViewProjectionQueriesKernelCoreGameEnginePlayerViewProjectionQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesKernelCoreGameEnginePlayerViewProjectionQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries() => engine.ComposeNativePlayerViewVisibilityQueries();
        public global::CardGame.Core.NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries() => engine.ComposeNativePlayerViewCardsQueries();
        public global::CardGame.Core.NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries() => engine.ComposeNativePlayerViewRulesQueries();
        public global::CardGame.Core.NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries() => engine.ComposeNativePlayerViewMatchQueries();
    }
}
