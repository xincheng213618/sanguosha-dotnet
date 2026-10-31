#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDynamicDiscardDamageObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDynamicDiscardDamageObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDynamicDiscardDamageObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDynamicDiscardDamageObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
