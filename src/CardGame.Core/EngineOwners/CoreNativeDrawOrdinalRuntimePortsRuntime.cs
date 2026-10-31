#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreNativeDrawOrdinalRuntimePortsRuntime : INativeDrawOrdinalSkillInstances
{
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    internal CoreNativeDrawOrdinalRuntimePortsRuntime(RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations)
    {
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
    }
}
