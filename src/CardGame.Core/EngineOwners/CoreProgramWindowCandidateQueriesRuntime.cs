#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramWindowCandidateQueriesRuntime : global::CardGame.Core.IProgramWindowCandidateQueries
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries _characterYuanJiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramTriggerFactQueriesOperations _kernelCoreProgramTriggerFactProjectorQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramTriggerQueriesOperations _kernelCoreProgramTriggerRuntimeQueriesOperations;
    private readonly ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations _programTriggerFactProjectorOperations;
    private readonly ICoreProgramWindowCandidateQueriesProgramTriggerOperations _programTriggerOperations;
    internal CoreProgramWindowCandidateQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries characterYuanJiOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.EngineOwners.IKernelCreateProgramTriggerFactQueriesOperations kernelCoreProgramTriggerFactProjectorQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCreateProgramTriggerQueriesOperations kernelCoreProgramTriggerRuntimeQueriesOperations, ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations programTriggerFactProjectorOperations, ICoreProgramWindowCandidateQueriesProgramTriggerOperations programTriggerOperations)
    {
        this._characterYuanJiOlOperations = characterYuanJiOlOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._kernelCoreProgramTriggerFactProjectorQueriesOperations = kernelCoreProgramTriggerFactProjectorQueriesOperations;
        this._kernelCoreProgramTriggerRuntimeQueriesOperations = kernelCoreProgramTriggerRuntimeQueriesOperations;
        this._programTriggerFactProjectorOperations = programTriggerFactProjectorOperations;
        this._programTriggerOperations = programTriggerOperations;
    }
}
