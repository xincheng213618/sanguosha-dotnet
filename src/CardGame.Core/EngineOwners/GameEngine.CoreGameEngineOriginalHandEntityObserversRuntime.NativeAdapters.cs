#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOriginalHandEntityObserversCoreOriginalHandEntityObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreOriginalHandEntityObserversOperations
    {
        public bool OriginalHandEntityFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.OriginalHandEntityObserversEntries.OriginalHandEntityFirstChild(f, child);
        public bool OriginalHandEntityObserverEdge(int child) => engine.OriginalHandEntityObserversEntries.OriginalHandEntityObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CoreOriginalHandEntityObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreOriginalHandEntityObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOriginalHandEntityObserversCoreObserverSubtreesOperations
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
