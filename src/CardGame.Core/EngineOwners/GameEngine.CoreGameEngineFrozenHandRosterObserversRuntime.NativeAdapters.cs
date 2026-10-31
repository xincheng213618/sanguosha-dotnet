#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFrozenHandRosterObserversCoreFrozenHandRosterObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFrozenHandRosterObserversCoreFrozenHandRosterObserversOperations
    {
        public bool FrozenHandRosterFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.FrozenHandRosterObserversEntries.FrozenHandRosterFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CoreFrozenHandRosterObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFrozenHandRosterObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreFrozenHandRosterObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFrozenHandRosterObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }
}
