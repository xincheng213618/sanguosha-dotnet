#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePairedHandRecastObserversCorePairedHandRecastObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedHandRecastObserversCorePairedHandRecastObserversOperations
    {
        public bool PairedHandRecastFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.PairedHandRecastObserversEntries.PairedHandRecastFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CorePairedHandRecastObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedHandRecastObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePairedHandRecastObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedHandRecastObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePairedHandRecastObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedHandRecastObserversCoreObserverSubtreesOperations
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }
}
