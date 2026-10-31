#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePublicPileCollectionContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCollectionContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePublicPileCollectionResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCollectionResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePublicPileCollectionCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileCollectionCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
