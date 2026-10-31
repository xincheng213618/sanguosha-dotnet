#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCommandSessionQueriesCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandSessionQueriesCommandSessionOperations
    {
        public ref readonly global::CardGame.Core.CommandSession _commandSession => ref engine._commandSession;
    }

    [ProgramCapability]
    private sealed class CoreCommandSessionQueriesKernelCoreGameEngineCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandSessionQueriesKernelCoreGameEngineCommandSessionOperations
    {
        public long _revision { get => engine._revision; }
        public global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }
}
