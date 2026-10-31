#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActualHandGainObserversCoreActualHandGainObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualHandGainObserversCoreActualHandGainObserversOperations
    {
        public bool ActualHandGainFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.ActualHandGainObserversEntries.ActualHandGainFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreActualHandGainObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualHandGainObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreActualHandGainObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualHandGainObserversCoreObserverSubtreesOperations
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreActualHandGainObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualHandGainObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
