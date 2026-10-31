#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduBasicConversionsRuntime
{
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    internal CoreGameEngineGuanduBasicConversionsRuntime(IKernelMatchModeRulesAccess modeRulesOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
