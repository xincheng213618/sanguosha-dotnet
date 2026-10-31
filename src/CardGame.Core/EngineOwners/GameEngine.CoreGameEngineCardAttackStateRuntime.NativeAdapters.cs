#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCardAttackStateCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardAttackStateCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCardAttackStateResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardAttackStateResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
