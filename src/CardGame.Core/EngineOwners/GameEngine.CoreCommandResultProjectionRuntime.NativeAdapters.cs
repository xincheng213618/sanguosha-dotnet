#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCommandResultProjectionKernelCoreGameEngineCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandResultProjectionKernelCoreGameEngineCommandSessionOperations
    {
        public long _revision { get => engine._revision; }
    }
}
