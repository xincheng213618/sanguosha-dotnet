#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramWindowCandidateQueriesRuntime : IProgramWindowCandidateQueries
{
    private readonly RuntimeEntries.CharacterYuanJiOlEntries _characterYuanJiOlOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly IKernelCreateProgramTriggerFactQueriesOperations _kernelCoreProgramTriggerFactProjectorQueriesOperations;
    private readonly IKernelCreateProgramTriggerQueriesOperations _kernelCoreProgramTriggerRuntimeQueriesOperations;
    private readonly ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations _programTriggerFactProjectorOperations;
    private readonly ICoreProgramWindowCandidateQueriesProgramTriggerOperations _programTriggerOperations;
    internal CoreProgramWindowCandidateQueriesRuntime(RuntimeEntries.CharacterYuanJiOlEntries characterYuanJiOlOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, IKernelCreateProgramTriggerFactQueriesOperations kernelCoreProgramTriggerFactProjectorQueriesOperations, IKernelCreateProgramTriggerQueriesOperations kernelCoreProgramTriggerRuntimeQueriesOperations, ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations programTriggerFactProjectorOperations, ICoreProgramWindowCandidateQueriesProgramTriggerOperations programTriggerOperations)
    {
        this._characterYuanJiOlOperations = characterYuanJiOlOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._kernelCoreProgramTriggerFactProjectorQueriesOperations = kernelCoreProgramTriggerFactProjectorQueriesOperations;
        this._kernelCoreProgramTriggerRuntimeQueriesOperations = kernelCoreProgramTriggerRuntimeQueriesOperations;
        this._programTriggerFactProjectorOperations = programTriggerFactProjectorOperations;
        this._programTriggerOperations = programTriggerOperations;
    }
}
