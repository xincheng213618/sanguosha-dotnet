#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePindianClaimsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePindianClaimsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePindianClaimsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePindianClaimsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePindianClaimsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
