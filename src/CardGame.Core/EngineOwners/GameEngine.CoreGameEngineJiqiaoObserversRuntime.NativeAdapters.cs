#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreJiqiaoObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJiqiaoObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreJiqiaoObserversCoreJiqiaoObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJiqiaoObserversCoreJiqiaoObserversOperations
    {
        public bool JiqiaoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.JiqiaoObserversEntries.JiqiaoFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CoreJiqiaoObserversCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJiqiaoObserversCoreObserverSubtreesOperations2
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }
}
