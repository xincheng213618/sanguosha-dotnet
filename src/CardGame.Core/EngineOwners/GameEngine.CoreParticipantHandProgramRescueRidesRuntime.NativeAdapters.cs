#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreParticipantHandProgramRescueRidesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandProgramRescueRidesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreParticipantHandProgramRescueRidesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandProgramRescueRidesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreParticipantHandProgramRescueRidesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreParticipantHandProgramRescueRidesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
