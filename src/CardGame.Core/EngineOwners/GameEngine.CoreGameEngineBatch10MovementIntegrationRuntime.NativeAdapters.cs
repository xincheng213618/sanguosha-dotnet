#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreBatch10MovementIntegrationCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreBatch10MovementIntegrationCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreBatch10MovementIntegrationContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreBatch10MovementIntegrationContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
