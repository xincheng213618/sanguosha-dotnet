#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRecipientCategoryMarkObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientCategoryMarkObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreRecipientCategoryMarkObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRecipientCategoryMarkObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
