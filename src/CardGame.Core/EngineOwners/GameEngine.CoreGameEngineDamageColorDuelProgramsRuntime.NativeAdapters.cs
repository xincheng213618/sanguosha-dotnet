#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDamageColorDuelProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageColorDuelProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDamageColorDuelProgramsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageColorDuelProgramsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreDamageColorDuelProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageColorDuelProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
