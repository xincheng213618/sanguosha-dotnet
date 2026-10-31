#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramPaidCardRuntimeCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramPaidCardRuntimeCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreProgramPaidCardRuntimeResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramPaidCardRuntimeResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramPaidCardRuntimeContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramPaidCardRuntimeContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
