#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOwnPublicPileConversionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreOwnPublicPileConversionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreOwnPublicPileConversionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
