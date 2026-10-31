#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRandomAreaAcquisitionObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRandomAreaAcquisitionObserversCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CoreRandomAreaAcquisitionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRandomAreaAcquisitionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRandomAreaAcquisitionObserversCoreRandomAreaAcquisitionObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRandomAreaAcquisitionObserversCoreRandomAreaAcquisitionObserversOperations
    {
        public bool RandomAreaFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.RandomAreaAcquisitionObserversEntries.RandomAreaFirstChild(root, child);
        public bool RandomAreaObserverEdge(int index) => engine.RandomAreaAcquisitionObserversEntries.RandomAreaObserverEdge(index);
    }
}
