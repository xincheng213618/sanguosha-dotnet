#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineSharedPhaseSkillsRuntime : global::CardGame.Core.IRandomPhaseSkillHost
{
    private readonly ICoreSharedPhaseSkillsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreSharedPhaseSkillsRandomOperations _randomOperations;
    private readonly ICoreSharedPhaseSkillsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSharedPhaseSkillsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreSharedPhaseSkillsCorePlayPhaseSkillGrantsOperations _corePlayPhaseSkillGrantsOperations;
    private readonly ICoreSharedPhaseSkillsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreSharedPhaseSkillsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSharedPhaseSkillsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreSharedPhaseSkillsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreSharedPhaseSkillsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreSharedPhaseSkillsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreSharedPhaseSkillsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreSharedPhaseSkillsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreSharedPhaseSkillsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreSharedPhaseSkillsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreSharedPhaseSkillsKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEngineSharedPhaseSkillsRuntime(ICoreSharedPhaseSkillsContentRegistryOperations contentRegistryOperations, ICoreSharedPhaseSkillsRandomOperations randomOperations, ICoreSharedPhaseSkillsResolutionStackOperations resolutionStackOperations, ICoreSharedPhaseSkillsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreSharedPhaseSkillsCorePlayPhaseSkillGrantsOperations corePlayPhaseSkillGrantsOperations, ICoreSharedPhaseSkillsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreSharedPhaseSkillsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSharedPhaseSkillsCoreRuntimeOperations coreRuntimeOperations, ICoreSharedPhaseSkillsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreSharedPhaseSkillsCoreMatchLogOperations coreMatchLogOperations, ICoreSharedPhaseSkillsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreSharedPhaseSkillsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreSharedPhaseSkillsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreSharedPhaseSkillsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreSharedPhaseSkillsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreSharedPhaseSkillsKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayPhaseSkillGrantsOperations = corePlayPhaseSkillGrantsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
