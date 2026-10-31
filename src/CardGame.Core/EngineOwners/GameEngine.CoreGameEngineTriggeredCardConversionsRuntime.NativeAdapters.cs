#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTriggeredCardConversionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTriggeredCardConversionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreTriggeredCardConversionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTriggeredCardConversionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreTriggeredCardConversionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTriggeredCardConversionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
