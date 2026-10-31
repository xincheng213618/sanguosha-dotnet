#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDelayedNullificationResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDelayedNullificationResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDelayedNullificationCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDelayedNullificationCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
