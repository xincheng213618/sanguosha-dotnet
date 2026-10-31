#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRicePileObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRicePileObserversCoreObserverSubtreesOperations2
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreRicePileObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRicePileObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRicePileObserversCoreRicePileObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRicePileObserversCoreRicePileObserversOperations
    {
        public bool RiceFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.RicePileObserversEntries.RiceFirstChild(root, child);
    }
}
