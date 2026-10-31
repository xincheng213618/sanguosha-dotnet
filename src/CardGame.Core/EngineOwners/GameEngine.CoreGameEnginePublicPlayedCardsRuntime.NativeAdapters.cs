#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePublicPlayedCardsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPlayedCardsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePublicPlayedCardsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicPlayedCardsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
