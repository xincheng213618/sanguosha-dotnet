#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePlannedPhaseQuantitiesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlannedPhaseQuantitiesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePlannedPhaseQuantitiesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlannedPhaseQuantitiesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
