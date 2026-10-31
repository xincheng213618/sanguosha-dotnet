#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramCapabilitiesRuntime : IActionCategoryGiftProgramHost, IConfiguredConversionProgramHost
{
    private readonly RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries _coreActionCategoryGiftsOperations;
    private readonly RuntimeEntries.CoreGameEngineConfiguredConversionsEntries _coreConfiguredConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramSkillChangesEntries _coreProgramSkillChangesOperations;
    internal CoreProgramCapabilitiesRuntime(RuntimeEntries.CoreGameEngineActionCategoryGiftsEntries coreActionCategoryGiftsOperations, RuntimeEntries.CoreGameEngineConfiguredConversionsEntries coreConfiguredConversionsOperations, RuntimeEntries.CoreGameEngineProgramSkillChangesEntries coreProgramSkillChangesOperations)
    {
        this._coreActionCategoryGiftsOperations = coreActionCategoryGiftsOperations;
        this._coreConfiguredConversionsOperations = coreConfiguredConversionsOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
    }
}
