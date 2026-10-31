#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterGongsunZanBoundaryMobileRuntime
{
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    internal CharacterGongsunZanBoundaryMobileRuntime(RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations)
    {
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}
