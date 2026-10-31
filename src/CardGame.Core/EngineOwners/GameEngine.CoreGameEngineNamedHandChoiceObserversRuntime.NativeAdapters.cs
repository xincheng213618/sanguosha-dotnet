#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNamedHandChoiceObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedHandChoiceObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNamedHandChoiceObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedHandChoiceObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreNamedHandChoiceObserversCoreNamedHandChoiceObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedHandChoiceObserversCoreNamedHandChoiceObserversOperations
    {
        public bool NamedHandChoiceFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.NamedHandChoiceObserversEntries.NamedHandChoiceFirstChild(root, child);
    }
}
