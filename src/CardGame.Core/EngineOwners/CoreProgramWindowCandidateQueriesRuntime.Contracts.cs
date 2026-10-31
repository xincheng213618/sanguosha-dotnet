#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerRuntime Get();
}

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerFactProjector Get();
}
