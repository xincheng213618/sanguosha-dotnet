#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreOwnedExactCountAndNamedTargetsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnedExactCountAndNamedTargetsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreOwnedExactCountAndNamedTargetsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnedExactCountAndNamedTargetsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreOwnedExactCountAndNamedTargetsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreOwnedExactCountAndNamedTargetsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
