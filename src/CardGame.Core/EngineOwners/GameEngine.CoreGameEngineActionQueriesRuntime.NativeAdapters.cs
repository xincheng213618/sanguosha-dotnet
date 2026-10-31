#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActionQueriesKernelCoreGameEngineActionQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActionQueriesKernelCoreGameEngineActionQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(global::CardGame.Core.NativeOwners.ActionQuery? previous = default) => engine.ComposeNativeActionQueryScope(previous);
    }
}
