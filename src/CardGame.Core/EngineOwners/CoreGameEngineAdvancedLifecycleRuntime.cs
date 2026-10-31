#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedLifecycleRuntime
{
    private readonly RuntimeEntries.CharacterGuoJiaMobileGodEntries _characterGuoJiaMobileGodOperations;
    private readonly RuntimeEntries.CharacterLiuHongOlEntries _characterLiuHongOlOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelObservedSkillGrantRevisionsAccess _observedSkillGrantRevisionsOperations;
    private readonly IKernelMatchRandomAccess _randomOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleChoicesEntries _coreAdvancedLifecycleChoicesOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly ICoreAdvancedLifecycleCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineStrategicProgramsEntries _coreStrategicProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreAdvancedLifecycleCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess _coreMatchDefinitionGeneralPoolAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess _coreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess;
    internal CoreGameEngineAdvancedLifecycleRuntime(RuntimeEntries.CharacterGuoJiaMobileGodEntries characterGuoJiaMobileGodOperations, RuntimeEntries.CharacterLiuHongOlEntries characterLiuHongOlOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelObservedSkillGrantRevisionsAccess observedSkillGrantRevisionsOperations, IKernelMatchRandomAccess randomOperations, IKernelFrameStoreAccess resolutionStackOperations, IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineAdvancedLifecycleChoicesEntries coreAdvancedLifecycleChoicesOperations, RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, ICoreAdvancedLifecycleCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, RuntimeEntries.CoreGameEngineStrategicProgramsEntries coreStrategicProgramsOperations, RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, ICoreAdvancedLifecycleCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess, EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess coreMatchDefinitionGeneralPoolAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess coreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess)
    {
        this._characterGuoJiaMobileGodOperations = characterGuoJiaMobileGodOperations;
        this._characterLiuHongOlOperations = characterLiuHongOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._observedSkillGrantRevisionsOperations = observedSkillGrantRevisionsOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreAdvancedLifecycleChoicesOperations = coreAdvancedLifecycleChoicesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreStrategicProgramsOperations = coreStrategicProgramsOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess = coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
        this._coreMatchDefinitionGeneralPoolAccess = coreMatchDefinitionGeneralPoolAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess = coreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess;
    }
}
