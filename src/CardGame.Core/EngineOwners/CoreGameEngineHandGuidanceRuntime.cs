#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHandGuidanceRuntime
{
    private readonly ICoreHandGuidanceCoreCommandSessionQueriesOperations _coreCommandSessionQueriesOperations;
    private readonly ICoreHandGuidanceCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreHandGuidanceCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreHandGuidanceCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreHandGuidanceCoreProgramCardUseRulesOperations _coreProgramCardUseRulesOperations;
    private readonly ICoreHandGuidanceCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreHandGuidanceCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICoreHandGuidanceCoreWenhePoisonOperations _coreWenhePoisonOperations;
    private readonly ICoreHandGuidanceCoreWushuangJinzhiOperations _coreWushuangJinzhiOperations;
    private readonly ICoreHandGuidanceCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    private readonly ICoreHandGuidanceCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreHandGuidanceCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreHandGuidanceCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess _coreCurrentTurnStatisticsSlashCountThisTurnAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHandGuidanceRuntime(ICoreHandGuidanceCoreCommandSessionQueriesOperations coreCommandSessionQueriesOperations, ICoreHandGuidanceCoreActionQueriesOperations coreActionQueriesOperations, ICoreHandGuidanceCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreHandGuidanceCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreHandGuidanceCoreProgramCardUseRulesOperations coreProgramCardUseRulesOperations, ICoreHandGuidanceCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreHandGuidanceCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICoreHandGuidanceCoreWenhePoisonOperations coreWenhePoisonOperations, ICoreHandGuidanceCoreWushuangJinzhiOperations coreWushuangJinzhiOperations, ICoreHandGuidanceCoreNativeLegalActionsOperations coreNativeLegalActionsOperations, ICoreHandGuidanceCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreHandGuidanceCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreHandGuidanceCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess coreCurrentTurnStatisticsSlashCountThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreCommandSessionQueriesOperations = coreCommandSessionQueriesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardUseRulesOperations = coreProgramCardUseRulesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreCurrentTurnStatisticsSlashCountThisTurnAccess = coreCurrentTurnStatisticsSlashCountThisTurnAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
