#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePublicPileSourcesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePublicPileSourcesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
