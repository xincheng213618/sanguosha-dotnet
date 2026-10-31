#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramWindowCandidateQueriesKernelCoreProgramTriggerRuntimeQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesKernelCoreProgramTriggerRuntimeQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries() => engine.ComposeNativeProgramTriggerQueries();
    }

    [ProgramCapability]
    private sealed class CoreProgramWindowCandidateQueriesKernelCoreProgramTriggerFactProjectorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramWindowCandidateQueriesKernelCoreProgramTriggerFactProjectorQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
    }
}
