#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramActualPlaySlashLossProgramPhaseScheduleAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramActualPlaySlashLossProgramPhaseScheduleOperations
    {
        public ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule => ref engine._programPhaseSchedule;
    }

    [ProgramCapability]
    private sealed class CoreProgramActualPlaySlashLossContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramActualPlaySlashLossContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreProgramActualPlaySlashLossResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramActualPlaySlashLossResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
