#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheXiashengRuntime : global::CardGame.Core.IShanheXiashengHost
{
    private readonly ICoreShanheXiashengContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheXiashengResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheXiashengCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreShanheXiashengCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShanheXiashengCoreDesignatedExtraTargetsOperations _coreDesignatedExtraTargetsOperations;
    private readonly ICoreShanheXiashengCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICoreShanheXiashengCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheXiashengCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheXiashengCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreShanheXiashengCoreRecipientCategorySlashTargetsOperations _coreRecipientCategorySlashTargetsOperations;
    private readonly ICoreShanheXiashengCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheXiashengCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreShanheXiashengCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreShanheXiashengCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheXiashengCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreShanheXiashengKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheXiashengRuntime(ICoreShanheXiashengContentRegistryOperations contentRegistryOperations, ICoreShanheXiashengResolutionStackOperations resolutionStackOperations, ICoreShanheXiashengCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreShanheXiashengCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShanheXiashengCoreDesignatedExtraTargetsOperations coreDesignatedExtraTargetsOperations, ICoreShanheXiashengCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICoreShanheXiashengCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheXiashengCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheXiashengCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreShanheXiashengCoreRecipientCategorySlashTargetsOperations coreRecipientCategorySlashTargetsOperations, ICoreShanheXiashengCoreRuntimeOperations coreRuntimeOperations, ICoreShanheXiashengCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreShanheXiashengCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreShanheXiashengCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheXiashengCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreShanheXiashengKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRecipientCategorySlashTargetsOperations = coreRecipientCategorySlashTargetsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
