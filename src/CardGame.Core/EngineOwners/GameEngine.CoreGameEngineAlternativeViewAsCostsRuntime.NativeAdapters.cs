#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAlternativeViewAsCostsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternativeViewAsCostsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAlternativeViewAsCostsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAlternativeViewAsCostsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
