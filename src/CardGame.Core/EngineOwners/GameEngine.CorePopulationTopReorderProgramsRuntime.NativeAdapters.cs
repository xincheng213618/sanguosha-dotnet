#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePopulationTopReorderProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePopulationTopReorderProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePopulationTopReorderProgramsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePopulationTopReorderProgramsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePopulationTopReorderProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePopulationTopReorderProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
