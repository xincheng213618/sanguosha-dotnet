#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSourceBeigongChildrenCoreSourceBeigongChildrenAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceBeigongChildrenCoreSourceBeigongChildrenOperations
    {
        public bool SourceBeigongEdge(int i) => engine.SourceBeigongChildrenEntries.SourceBeigongEdge(i);
        public bool SourceBeigongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SourceBeigongChildrenEntries.SourceBeigongFirstChild(f, child);
        public bool SourceBeigongSuffix(int n) => engine.SourceBeigongChildrenEntries.SourceBeigongSuffix(n);
    }

    [ProgramCapability]
    private sealed class CoreSourceBeigongChildrenResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceBeigongChildrenResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSourceBeigongChildrenCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceBeigongChildrenCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
