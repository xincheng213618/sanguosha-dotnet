#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePrivateWhirlObserversCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateWhirlObserversCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CorePrivateWhirlObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateWhirlObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePrivateWhirlObserversCorePrivateWhirlObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateWhirlObserversCorePrivateWhirlObserversOperations
    {
        public bool WhirlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.PrivateWhirlObserversEntries.WhirlFirstChild(root, child);
    }
}
