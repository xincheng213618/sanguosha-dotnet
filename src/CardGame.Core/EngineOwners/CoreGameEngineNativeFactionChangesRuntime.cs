#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeFactionChangesRuntime
{
    private readonly RuntimeEntries.CharacterCaoTengOlEntries _characterCaoTengOlOperations;
    private readonly RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly ICoreNativeFactionChangesCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries _corePrivateGeneralLibrariesOperations;
    private readonly RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess _coreGameEngineNativeFactionChangesFieldsAccess;
    internal CoreGameEngineNativeFactionChangesRuntime(RuntimeEntries.CharacterCaoTengOlEntries characterCaoTengOlOperations, RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, ICoreNativeFactionChangesCoreModeAllegianceOperations coreModeAllegianceOperations, RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries corePrivateGeneralLibrariesOperations, RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess coreGameEngineNativeFactionChangesFieldsAccess)
    {
        this._characterCaoTengOlOperations = characterCaoTengOlOperations;
        this._characterSunShangxiangMouMobileOperations = characterSunShangxiangMouMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._corePrivateGeneralLibrariesOperations = corePrivateGeneralLibrariesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
        this._coreGameEngineNativeFactionChangesFieldsAccess = coreGameEngineNativeFactionChangesFieldsAccess;
    }
}
