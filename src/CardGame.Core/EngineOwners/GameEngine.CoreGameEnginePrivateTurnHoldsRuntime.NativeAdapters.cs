#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePrivateTurnHoldsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateTurnHoldsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePrivateTurnHoldsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateTurnHoldsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
