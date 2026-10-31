#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardObserversCoreObserverSubtreesOperations
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardObserversCoreOutsidePhaseDrawDiscardObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardObserversCoreOutsidePhaseDrawDiscardObserversOperations
    {
        public bool OutsidePhaseDrawDiscardFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.OutsidePhaseDrawDiscardObserversEntries.OutsidePhaseDrawDiscardFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreOutsidePhaseDrawDiscardObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOutsidePhaseDrawDiscardObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
