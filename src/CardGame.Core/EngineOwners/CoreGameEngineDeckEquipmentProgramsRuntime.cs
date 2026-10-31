#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineDeckEquipmentProgramsRuntime : global::CardGame.Core.IDeckEquipmentProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries _characterDuXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries _characterGuoZhaoAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuoJunOlEntries _characterHuoJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiangChongOlEntries _characterXiangChongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries _coreCharacterHostOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly ICoreDeckEquipmentProgramsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries _coreFinalizedCardTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries _coreFriendTrioOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries _coreProgramPhaseColorRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries _coreNativeEquipmentUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess _coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess;
    internal CoreGameEngineDeckEquipmentProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries characterDuXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries characterGuoZhaoAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuoJunOlEntries characterHuoJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXiangChongOlEntries characterXiangChongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries coreCharacterHostOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, ICoreDeckEquipmentProgramsCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries coreFinalizedCardTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries coreFriendTrioOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries coreProgramPhaseColorRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries coreNativeEquipmentUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess)
    {
        this._characterDuXiOlOperations = characterDuXiOlOperations;
        this._characterGuoZhaoAnniversaryOperations = characterGuoZhaoAnniversaryOperations;
        this._characterHuoJunOlOperations = characterHuoJunOlOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterXiangChongOlOperations = characterXiangChongOlOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreCharacterHostOperations = coreCharacterHostOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFinalizedCardTargetsOperations = coreFinalizedCardTargetsOperations;
        this._coreFriendTrioOperations = coreFriendTrioOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPhaseColorRestrictionsOperations = coreProgramPhaseColorRestrictionsOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeEquipmentUseOperations = coreNativeEquipmentUseOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess = coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess;
    }
}
