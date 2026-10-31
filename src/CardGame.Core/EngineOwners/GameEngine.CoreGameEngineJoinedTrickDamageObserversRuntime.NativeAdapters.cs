#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreJoinedTrickDamageObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJoinedTrickDamageObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CoreJoinedTrickDamageObserversCoreJoinedTrickDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJoinedTrickDamageObserversCoreJoinedTrickDamageObserversOperations
    {
        public bool JoinedTrickDamageRewardFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.JoinedTrickDamageObserversEntries.JoinedTrickDamageRewardFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CoreJoinedTrickDamageObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreJoinedTrickDamageObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
