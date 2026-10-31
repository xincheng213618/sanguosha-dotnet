#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeActionDispatchRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuMobileGodEntries _characterLuBuMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnViewAsEntries _coreActualTurnViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries _coreDragonBoatCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries _coreDrawFundedDistinctBasicOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries _coreHellSoulCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHulaoCardsEntries _coreHulaoCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHuxinMirrorEntries _coreHuxinMirrorOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries _coreMantianGuohaiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries _coreNextActualUseTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramIronChainTargetsEntries _coreProgramIronChainTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMultiTargetDuelsEntries _coreProgramMultiTargetDuelsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRedAdditionalTargetsEntries _coreRedAdditionalTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries _coreRoundOnceCardlessNativeUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuCardsEntries _coreShenwuCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSingleCardTrickConversionsEntries _coreSingleCardTrickConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnexpectedAssaultEntries _coreUnexpectedAssaultOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries _coreWenheCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries _coreNationalGeneralRevealOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDuelEntries _coreNativeDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries _coreNativeEquipmentUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryCardsEntries _coreNativeRecoveryCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeWoodenOxEntries _coreNativeWoodenOxOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreNativeActionDispatchCoreProgramActionModulesOperations _coreProgramActionModulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess;
    internal CoreNativeActionDispatchRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterLuBuMobileGodEntries characterLuBuMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnViewAsEntries coreActualTurnViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries coreDragonBoatCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries coreDrawFundedDistinctBasicOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries coreHellSoulCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHulaoCardsEntries coreHulaoCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHuxinMirrorEntries coreHuxinMirrorOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries coreMantianGuohaiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries coreNextActualUseTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramIronChainTargetsEntries coreProgramIronChainTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMultiTargetDuelsEntries coreProgramMultiTargetDuelsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRedAdditionalTargetsEntries coreRedAdditionalTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries coreRoundOnceCardlessNativeUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuCardsEntries coreShenwuCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSingleCardTrickConversionsEntries coreSingleCardTrickConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineUnexpectedAssaultEntries coreUnexpectedAssaultOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries coreWenheCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries coreNationalGeneralRevealOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDuelEntries coreNativeDuelOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries coreNativeEquipmentUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryCardsEntries coreNativeRecoveryCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeWoodenOxEntries coreNativeWoodenOxOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreNativeActionDispatchCoreProgramActionModulesOperations coreProgramActionModulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterLuBuMobileGodOperations = characterLuBuMobileGodOperations;
        this._characterXunYuMobileGodOperations = characterXunYuMobileGodOperations;
        this._coreActualTurnViewAsOperations = coreActualTurnViewAsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreDragonBoatCardsOperations = coreDragonBoatCardsOperations;
        this._coreDrawFundedDistinctBasicOffersOperations = coreDrawFundedDistinctBasicOffersOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._coreHellSoulCardsOperations = coreHellSoulCardsOperations;
        this._coreHulaoCardsOperations = coreHulaoCardsOperations;
        this._coreHuxinMirrorOperations = coreHuxinMirrorOperations;
        this._coreMantianGuohaiOperations = coreMantianGuohaiOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramIronChainTargetsOperations = coreProgramIronChainTargetsOperations;
        this._coreProgramMultiTargetDuelsOperations = coreProgramMultiTargetDuelsOperations;
        this._coreRedAdditionalTargetsOperations = coreRedAdditionalTargetsOperations;
        this._coreRoundOnceCardlessNativeUsesOperations = coreRoundOnceCardlessNativeUsesOperations;
        this._coreShenwuCardsOperations = coreShenwuCardsOperations;
        this._coreSingleCardTrickConversionsOperations = coreSingleCardTrickConversionsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
        this._coreUnexpectedAssaultOperations = coreUnexpectedAssaultOperations;
        this._coreWenheCardsOperations = coreWenheCardsOperations;
        this._coreNationalGeneralRevealOperations = coreNationalGeneralRevealOperations;
        this._coreNativeDuelOperations = coreNativeDuelOperations;
        this._coreNativeEquipmentUseOperations = coreNativeEquipmentUseOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativeRecoveryCardsOperations = coreNativeRecoveryCardsOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._coreNativeTrickUseOperations = coreNativeTrickUseOperations;
        this._coreNativeWoodenOxOperations = coreNativeWoodenOxOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramActionModulesOperations = coreProgramActionModulesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess = coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess;
        this._coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess = coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess;
    }
}
