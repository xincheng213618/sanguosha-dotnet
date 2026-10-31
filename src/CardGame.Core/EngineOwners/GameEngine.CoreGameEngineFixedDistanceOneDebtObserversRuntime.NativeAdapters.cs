#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreFixedDistanceOneDebtObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFixedDistanceOneDebtObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreFixedDistanceOneDebtObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFixedDistanceOneDebtObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreFixedDistanceOneDebtObserversCoreFixedDistanceOneDebtObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreFixedDistanceOneDebtObserversCoreFixedDistanceOneDebtObserversOperations
    {
        public bool FixedDistanceDebtFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.FixedDistanceOneDebtObserversEntries.FixedDistanceDebtFirstChild(root, child);
    }
}
