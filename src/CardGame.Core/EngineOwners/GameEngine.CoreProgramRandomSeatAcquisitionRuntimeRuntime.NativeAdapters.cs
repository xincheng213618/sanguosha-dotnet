#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramRandomSeatAcquisitionRuntimeContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRandomSeatAcquisitionRuntimeContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreProgramRandomSeatAcquisitionRuntimeRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRandomSeatAcquisitionRuntimeRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreProgramRandomSeatAcquisitionRuntimeResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRandomSeatAcquisitionRuntimeResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
