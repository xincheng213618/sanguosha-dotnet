#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEndingPairSlashObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndingPairSlashObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreEndingPairSlashObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndingPairSlashObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
