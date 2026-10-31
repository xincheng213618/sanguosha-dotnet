#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCurrentEnhancementVirtualReturnContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreCurrentEnhancementVirtualReturnResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
