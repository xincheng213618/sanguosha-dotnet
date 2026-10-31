#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePhaseHandSeizureReceiptsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePhaseHandSeizureReceiptsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePhaseHandSeizureReceiptsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
