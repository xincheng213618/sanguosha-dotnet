#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePhaseStartCardControlObserversCorePhaseStartCardControlObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseStartCardControlObserversCorePhaseStartCardControlObserversOperations
    {
        public bool PhaseStartCardControlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.PhaseStartCardControlObserversEntries.PhaseStartCardControlFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CorePhaseStartCardControlObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseStartCardControlObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePhaseStartCardControlObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseStartCardControlObserversCoreObserverSubtreesOperations
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }
}
