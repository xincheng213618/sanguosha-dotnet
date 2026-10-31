#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeMultiTargetActionsRuntime
{
    private readonly ICoreNativeMultiTargetActionsCharacterWuJingOlOperations _characterWuJingOlOperations;
    private readonly ICoreNativeMultiTargetActionsCoreForeignEquipmentDiscardPoliciesOperations _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly ICoreNativeMultiTargetActionsCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreNativeMultiTargetActionsCoreCardConversionsOperations _coreCardConversionsOperations;
    private readonly ICoreNativeMultiTargetActionsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreNativeMultiTargetActionsCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreNativeMultiTargetActionsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreNativeMultiTargetActionsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreNativeMultiTargetActionsCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreNativeMultiTargetActionsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeMultiTargetActionsRuntime(ICoreNativeMultiTargetActionsCharacterWuJingOlOperations characterWuJingOlOperations, ICoreNativeMultiTargetActionsCoreForeignEquipmentDiscardPoliciesOperations coreForeignEquipmentDiscardPoliciesOperations, ICoreNativeMultiTargetActionsCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreNativeMultiTargetActionsCoreCardConversionsOperations coreCardConversionsOperations, ICoreNativeMultiTargetActionsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreNativeMultiTargetActionsCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreNativeMultiTargetActionsCoreTableOrderOperations coreTableOrderOperations, ICoreNativeMultiTargetActionsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreNativeMultiTargetActionsCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreNativeMultiTargetActionsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
