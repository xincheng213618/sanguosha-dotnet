#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramTargetSelectionRuntimePortsRuntime : global::CardGame.Core.IProgramTargetFacts, global::CardGame.Core.IProgramTargetResolvers, global::CardGame.Core.IProgramTargetSelectionProtocols
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeJinMobileEntries _characterZhugeJinMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries _coreDeathProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries _coreOwnedExactCountAndNamedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackEffectsEntries _coreProgramAttackEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries _coreProgramCardUseRolesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEquipmentExchangesEntries _coreProgramEquipmentExchangesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOrdinaryTrickUsesEntries _coreProgramOrdinaryTrickUsesOperations;
    private readonly ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations _coreProgramTargetResolversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ZhulingEntries _coreShanheS5ZhulingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceCaoSongTrioEntries _coreShanheSourceCaoSongTrioOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries _coreTeamActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTriggeredCardProgramsEntries _coreTriggeredCardProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramPopulationAndArrowBarrageOperationsEntries _coreProgramPopulationAndArrowBarrageOperationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramTargetSelectionRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhugeJinMobileEntries characterZhugeJinMobileOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries coreForeignEquipmentDiscardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries coreDeathProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries coreOwnedExactCountAndNamedTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackEffectsEntries coreProgramAttackEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries coreProgramCardUseRolesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEquipmentExchangesEntries coreProgramEquipmentExchangesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOrdinaryTrickUsesEntries coreProgramOrdinaryTrickUsesOperations, ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations coreProgramTargetResolversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ZhulingEntries coreShanheS5ZhulingOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceCaoSongTrioEntries coreShanheSourceCaoSongTrioOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries coreTeamActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTriggeredCardProgramsEntries coreTriggeredCardProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreProgramPopulationAndArrowBarrageOperationsEntries coreProgramPopulationAndArrowBarrageOperationsOperations, global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterZhugeJinMobileOperations = characterZhugeJinMobileOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDeathProgramsOperations = coreDeathProgramsOperations;
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._coreOwnedExactCountAndNamedTargetsOperations = coreOwnedExactCountAndNamedTargetsOperations;
        this._coreProgramAttackEffectsOperations = coreProgramAttackEffectsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramEquipmentExchangesOperations = coreProgramEquipmentExchangesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramOrdinaryTrickUsesOperations = coreProgramOrdinaryTrickUsesOperations;
        this._coreProgramTargetResolversOperations = coreProgramTargetResolversOperations;
        this._coreShanheS5ZhulingOperations = coreShanheS5ZhulingOperations;
        this._coreShanheSourceCaoSongTrioOperations = coreShanheSourceCaoSongTrioOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreTriggeredCardProgramsOperations = coreTriggeredCardProgramsOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreProgramPopulationAndArrowBarrageOperationsOperations = coreProgramPopulationAndArrowBarrageOperationsOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
