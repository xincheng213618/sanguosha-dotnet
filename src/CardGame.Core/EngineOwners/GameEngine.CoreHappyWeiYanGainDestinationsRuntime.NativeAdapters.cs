#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHappyWeiYanGainDestinationsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHappyWeiYanGainDestinationsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreHappyWeiYanGainDestinationsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHappyWeiYanGainDestinationsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreHappyWeiYanGainDestinationsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHappyWeiYanGainDestinationsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
