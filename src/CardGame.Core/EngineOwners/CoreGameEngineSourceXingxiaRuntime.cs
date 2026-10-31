#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSourceXingxiaRuntime
{
    private readonly ICoreSourceXingxiaCardZonesOperations _cardZonesOperations;
    private readonly ICoreSourceXingxiaContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreSourceXingxiaResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSourceXingxiaCoreForeignEquipmentDiscardPoliciesOperations _coreForeignEquipmentDiscardPoliciesOperations;
    private readonly ICoreSourceXingxiaCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICoreSourceXingxiaCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreSourceXingxiaCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreSourceXingxiaCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSourceXingxiaCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreSourceXingxiaCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreSourceXingxiaCoreSourceBeigongOperations _coreSourceBeigongOperations;
    private readonly ICoreSourceXingxiaCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreSourceXingxiaCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreSourceXingxiaCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreSourceXingxiaCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreSourceXingxiaKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineSourceXingxiaRuntime(ICoreSourceXingxiaCardZonesOperations cardZonesOperations, ICoreSourceXingxiaContentRegistryOperations contentRegistryOperations, ICoreSourceXingxiaResolutionStackOperations resolutionStackOperations, ICoreSourceXingxiaCoreForeignEquipmentDiscardPoliciesOperations coreForeignEquipmentDiscardPoliciesOperations, ICoreSourceXingxiaCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICoreSourceXingxiaCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreSourceXingxiaCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreSourceXingxiaCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSourceXingxiaCoreRuntimeOperations coreRuntimeOperations, ICoreSourceXingxiaCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreSourceXingxiaCoreSourceBeigongOperations coreSourceBeigongOperations, ICoreSourceXingxiaCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreSourceXingxiaCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreSourceXingxiaCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreSourceXingxiaCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreSourceXingxiaKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreForeignEquipmentDiscardPoliciesOperations = coreForeignEquipmentDiscardPoliciesOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreSourceBeigongOperations = coreSourceBeigongOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
