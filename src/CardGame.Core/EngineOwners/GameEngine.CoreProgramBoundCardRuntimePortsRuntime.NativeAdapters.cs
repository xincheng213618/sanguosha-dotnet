#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramBoundCardRuntimePortsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreProgramBoundCardRuntimePortsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramBoundCardRuntimePortsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
