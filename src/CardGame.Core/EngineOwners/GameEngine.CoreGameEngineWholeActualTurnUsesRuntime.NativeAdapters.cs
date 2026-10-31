#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreWholeActualTurnUsesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWholeActualTurnUsesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreWholeActualTurnUsesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWholeActualTurnUsesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
