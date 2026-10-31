#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreGuanduGeneralChoicesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralChoicesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreGuanduGeneralChoicesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralChoicesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreGuanduGeneralChoicesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralChoicesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreGuanduGeneralChoicesRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralChoicesRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }
}
