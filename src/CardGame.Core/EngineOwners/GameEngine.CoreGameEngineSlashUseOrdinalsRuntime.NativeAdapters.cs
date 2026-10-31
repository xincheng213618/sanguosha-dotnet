#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSlashUseOrdinalsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashUseOrdinalsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSlashUseOrdinalsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSlashUseOrdinalsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
