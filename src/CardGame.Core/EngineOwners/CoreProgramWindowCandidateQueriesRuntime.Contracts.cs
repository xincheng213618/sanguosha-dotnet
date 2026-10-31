#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramWindowCandidateQueriesKernelCoreProgramTriggerRuntimeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries();
}

internal interface ICoreProgramWindowCandidateQueriesKernelCoreProgramTriggerFactProjectorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
}

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerRuntime Get();
}

internal interface ICoreProgramWindowCandidateQueriesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramWindowCandidateQueriesCharacterYuanJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYuanJiIssuedReturns(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary);
}

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerFactProjector Get();
}
