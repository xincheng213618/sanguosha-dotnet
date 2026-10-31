#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheSourceBiriObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceBiriObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceBiriObserversCoreShanheSourceBiriObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceBiriObserversCoreShanheSourceBiriObserversOperations
    {
        public bool ShanheSourceBiriFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.ShanheSourceBiriObserversEntries.ShanheSourceBiriFirstChild(root, child);
        public bool ShanheSourceBiriObserverEdge(int index) => engine.ShanheSourceBiriObserversEntries.ShanheSourceBiriObserverEdge(index);
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceBiriObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceBiriObserversCoreObserverSubtreesOperations
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
