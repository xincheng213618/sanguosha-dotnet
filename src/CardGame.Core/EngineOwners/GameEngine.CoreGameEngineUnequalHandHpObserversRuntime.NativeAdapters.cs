#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreUnequalHandHpObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnequalHandHpObserversCoreObserverSubtreesOperations2
    {
        public bool HasHandGainDebtObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasHandGainDebtObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreUnequalHandHpObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnequalHandHpObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreUnequalHandHpObserversCoreUnequalHandHpObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreUnequalHandHpObserversCoreUnequalHandHpObserversOperations
    {
        public bool UnequalHandHpFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.UnequalHandHpObserversEntries.UnequalHandHpFirstChild(root, child);
    }
}
