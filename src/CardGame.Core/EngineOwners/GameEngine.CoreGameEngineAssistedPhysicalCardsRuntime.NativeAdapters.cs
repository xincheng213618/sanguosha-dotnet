#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAssistedPhysicalCardsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAssistedPhysicalCardsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreAssistedPhysicalCardsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAssistedPhysicalCardsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAssistedPhysicalCardsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAssistedPhysicalCardsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
