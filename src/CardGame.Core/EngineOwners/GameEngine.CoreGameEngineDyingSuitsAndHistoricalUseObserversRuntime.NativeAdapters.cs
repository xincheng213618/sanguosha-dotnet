#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDyingSuitsAndHistoricalUseObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDyingSuitsAndHistoricalUseObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
