#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeFactionChangesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoTengOlEntries _characterCaoTengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries _characterSunShangxiangMouMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly ICoreNativeFactionChangesCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries _corePrivateGeneralLibrariesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess _coreGameEngineNativeFactionChangesFieldsAccess;
    internal CoreGameEngineNativeFactionChangesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoTengOlEntries characterCaoTengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunShangxiangMouMobileEntries characterSunShangxiangMouMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, ICoreNativeFactionChangesCoreModeAllegianceOperations coreModeAllegianceOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries corePrivateGeneralLibrariesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess coreGameEngineNativeFactionChangesFieldsAccess)
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
