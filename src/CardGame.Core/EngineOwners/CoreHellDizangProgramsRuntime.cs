#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreHellDizangProgramsRuntime : global::CardGame.Core.IHellDizangProgramHost
{
    private readonly ICoreHellDizangProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreHellDizangProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHellDizangProgramsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreHellDizangProgramsCoreEquipmentRecastsOperations _coreEquipmentRecastsOperations;
    private readonly ICoreHellDizangProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreHellDizangProgramsCoreNativeDrawAttemptsOperations _coreNativeDrawAttemptsOperations;
    private readonly ICoreHellDizangProgramsCoreNativeRecastInvocationsOperations _coreNativeRecastInvocationsOperations;
    private readonly ICoreHellDizangProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreHellDizangProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreHellDizangProgramsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreHellDizangProgramsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreHellDizangProgramsCoreJudgmentCursorRuntimeOperations _coreJudgmentCursorRuntimeOperations;
    private readonly ICoreHellDizangProgramsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreHellDizangProgramsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreHellDizangProgramsCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreHellDizangProgramsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreHellDizangProgramsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICoreHellDizangProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreHellDizangProgramsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreHellDizangProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreHellDizangProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess _coreMatchDefinitionModeDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreHellDizangProgramsRuntime(ICoreHellDizangProgramsContentRegistryOperations contentRegistryOperations, ICoreHellDizangProgramsResolutionStackOperations resolutionStackOperations, ICoreHellDizangProgramsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreHellDizangProgramsCoreEquipmentRecastsOperations coreEquipmentRecastsOperations, ICoreHellDizangProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreHellDizangProgramsCoreNativeDrawAttemptsOperations coreNativeDrawAttemptsOperations, ICoreHellDizangProgramsCoreNativeRecastInvocationsOperations coreNativeRecastInvocationsOperations, ICoreHellDizangProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreHellDizangProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreHellDizangProgramsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreHellDizangProgramsCoreTableOrderOperations coreTableOrderOperations, ICoreHellDizangProgramsCoreJudgmentCursorRuntimeOperations coreJudgmentCursorRuntimeOperations, ICoreHellDizangProgramsCoreMatchLogOperations coreMatchLogOperations, ICoreHellDizangProgramsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreHellDizangProgramsCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreHellDizangProgramsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreHellDizangProgramsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICoreHellDizangProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreHellDizangProgramsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreHellDizangProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreHellDizangProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess coreMatchDefinitionModeDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreEquipmentRecastsOperations = coreEquipmentRecastsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeDrawAttemptsOperations = coreNativeDrawAttemptsOperations;
        this._coreNativeRecastInvocationsOperations = coreNativeRecastInvocationsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreJudgmentCursorRuntimeOperations = coreJudgmentCursorRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchDefinitionModeDefinitionPlayerCountAccess = coreMatchDefinitionModeDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
