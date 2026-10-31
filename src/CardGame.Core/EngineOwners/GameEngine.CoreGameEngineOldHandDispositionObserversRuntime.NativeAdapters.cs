#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOldHandDispositionObserversCoreOldHandDispositionObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOldHandDispositionObserversCoreOldHandDispositionObserversOperations
    {
        public bool OldHandDispositionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.OldHandDispositionObserversEntries.OldHandDispositionFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreOldHandDispositionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOldHandDispositionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreOldHandDispositionObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOldHandDispositionObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }
}
