#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreParticipantHandDamageRidesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandDamageRidesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreParticipantHandDamageRidesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandDamageRidesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreParticipantHandDamageRidesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandDamageRidesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
