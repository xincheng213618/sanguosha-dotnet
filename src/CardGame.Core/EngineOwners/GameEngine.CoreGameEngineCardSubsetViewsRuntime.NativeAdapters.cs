#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCardSubsetViewsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardSubsetViewsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCardSubsetViewsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardSubsetViewsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
