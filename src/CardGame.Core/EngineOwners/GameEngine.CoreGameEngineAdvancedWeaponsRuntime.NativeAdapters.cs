#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAdvancedWeaponsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedWeaponsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedWeaponsRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedWeaponsRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedWeaponsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedWeaponsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
