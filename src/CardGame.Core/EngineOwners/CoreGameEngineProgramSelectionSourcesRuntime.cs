#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSelectionSourcesRuntime : global::CardGame.Core.IProgramSelectionSourceQuery
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries _coreHealthQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries _coreHpLossSlashMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries _coreNextActualUseTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOrdinaryTrickUsesEntries _coreProgramOrdinaryTrickUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramSelectionSourcesRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries coreHealthQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries coreHpLossSlashMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries coreNextActualUseTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOrdinaryTrickUsesEntries coreProgramOrdinaryTrickUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreHealthQueriesOperations = coreHealthQueriesOperations;
        this._coreHpLossSlashMaterialsOperations = coreHpLossSlashMaterialsOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._coreProgramOrdinaryTrickUsesOperations = coreProgramOrdinaryTrickUsesOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
