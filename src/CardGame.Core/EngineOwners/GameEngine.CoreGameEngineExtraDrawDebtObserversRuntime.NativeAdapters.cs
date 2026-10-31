#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreExtraDrawDebtObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraDrawDebtObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreExtraDrawDebtObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreExtraDrawDebtObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
