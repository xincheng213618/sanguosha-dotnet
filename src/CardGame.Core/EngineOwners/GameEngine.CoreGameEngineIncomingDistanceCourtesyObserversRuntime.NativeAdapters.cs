#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreIncomingDistanceCourtesyObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIncomingDistanceCourtesyObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreIncomingDistanceCourtesyObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIncomingDistanceCourtesyObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreIncomingDistanceCourtesyObserversCoreIncomingDistanceCourtesyObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreIncomingDistanceCourtesyObserversCoreIncomingDistanceCourtesyObserversOperations
    {
        public bool IncomingDistanceCourtesyFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.IncomingDistanceCourtesyObserversEntries.IncomingDistanceCourtesyFirstChild(root, child);
    }
}
