#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterGuYongMobileRuntime : IMobileGuYongHost
{
    private readonly RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries _coreProgramCategoryChallengesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    internal CharacterGuYongMobileRuntime(RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries coreProgramCategoryChallengesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations)
    {
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}
