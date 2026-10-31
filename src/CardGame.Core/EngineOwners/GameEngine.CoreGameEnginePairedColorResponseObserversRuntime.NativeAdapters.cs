#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePairedColorResponseObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedColorResponseObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePairedColorResponseObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedColorResponseObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePairedColorResponseObserversCorePairedColorResponseObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedColorResponseObserversCorePairedColorResponseObserversOperations
    {
        public bool PairedColorDispositionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.PairedColorResponseObserversEntries.PairedColorDispositionFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CorePairedColorResponseObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePairedColorResponseObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }
}
