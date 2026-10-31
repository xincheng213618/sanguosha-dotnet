#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEquipmentPairDyingCardObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairDyingCardObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreEquipmentPairDyingCardObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairDyingCardObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
