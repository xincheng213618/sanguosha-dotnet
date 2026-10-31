#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreNativeDrawThresholdRuntimePortsRuntime : INativeDrawThresholdNativeOperations
{
    private readonly RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    internal CoreNativeDrawThresholdRuntimePortsRuntime(RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations)
    {
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
    }
}
