#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramParticipantPhaseOperationsRuntime : global::CardGame.Core.IParticipantPhaseProgramEffectHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanHappyGodEntries _characterDiaoChanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeYanAnniversaryEntries _characterHeYanAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWuFuAnniversaryEntries _characterWuFuAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries _characterZhouYuOlMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries _coreProgramCardLegalityOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess;
    internal CoreProgramParticipantPhaseOperationsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanHappyGodEntries characterDiaoChanHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterHeYanAnniversaryEntries characterHeYanAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterWuFuAnniversaryEntries characterWuFuAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries characterZhouYuOlMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramCardLegalityEntries coreProgramCardLegalityOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess)
    {
        this._characterDiaoChanHappyGodOperations = characterDiaoChanHappyGodOperations;
        this._characterHeYanAnniversaryOperations = characterHeYanAnniversaryOperations;
        this._characterWuFuAnniversaryOperations = characterWuFuAnniversaryOperations;
        this._characterZhouYuOlMouOperations = characterZhouYuOlMouOperations;
        this._characterZhugeLiangWumiaoOperations = characterZhugeLiangWumiaoOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreProgramCardLegalityOperations = coreProgramCardLegalityOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess = coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess;
    }
}
