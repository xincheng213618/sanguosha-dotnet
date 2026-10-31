#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSimultaneousPindianObserversCoreSimultaneousPindianObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianObserversCoreSimultaneousPindianObserversOperations
    {
        public bool SimultaneousFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.SimultaneousPindianObserversEntries.SimultaneousFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianObserversCoreObserverSubtreesOperations2
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreSimultaneousPindianObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSimultaneousPindianObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
