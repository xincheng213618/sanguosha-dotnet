#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnObserversCoreEndRefillNextTurnObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnObserversCoreEndRefillNextTurnObserversOperations
    {
        public bool EndRefillFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.EndRefillNextTurnObserversEntries.EndRefillFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }
}
