#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePlayerViewNativeCardSelectionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePlayerViewNativeCardSelectionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePlayerViewNativeCardSelectionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
