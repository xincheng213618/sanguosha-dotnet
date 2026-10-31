#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreForeignContestAidObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignContestAidObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreForeignContestAidObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreForeignContestAidObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
