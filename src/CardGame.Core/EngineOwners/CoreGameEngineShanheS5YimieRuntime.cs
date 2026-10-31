#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5YimieRuntime : global::CardGame.Core.IShanheS5YimieHost
{
    private readonly ICoreShanheS5YimieCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICoreShanheS5YimieContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5YimieResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5YimieCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheS5YimieCoreHealthQueriesOperations _coreHealthQueriesOperations;
    private readonly ICoreShanheS5YimieCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheS5YimieCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreShanheS5YimieCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5YimieCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5YimieCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5YimieCoreNativeCardClassificationsOperations _coreNativeCardClassificationsOperations;
    private readonly ICoreShanheS5YimieCoreNativeDamageCompletionOperations _coreNativeDamageCompletionOperations;
    private readonly ICoreShanheS5YimieCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICoreShanheS5YimieCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreShanheS5YimieCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreShanheS5YimieCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreShanheS5YimieCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheS5YimieKernelCoreGameEngineProgramAttackAttemptOperations _kernelCoreGameEngineProgramAttackAttemptOperations;
    private readonly ICoreShanheS5YimieKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly ICoreShanheS5YimieKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5YimieRuntime(ICoreShanheS5YimieCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICoreShanheS5YimieContentRegistryOperations contentRegistryOperations, ICoreShanheS5YimieResolutionStackOperations resolutionStackOperations, ICoreShanheS5YimieCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheS5YimieCoreHealthQueriesOperations coreHealthQueriesOperations, ICoreShanheS5YimieCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheS5YimieCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreShanheS5YimieCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5YimieCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5YimieCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5YimieCoreNativeCardClassificationsOperations coreNativeCardClassificationsOperations, ICoreShanheS5YimieCoreNativeDamageCompletionOperations coreNativeDamageCompletionOperations, ICoreShanheS5YimieCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICoreShanheS5YimieCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreShanheS5YimieCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreShanheS5YimieCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreShanheS5YimieCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheS5YimieKernelCoreGameEngineProgramAttackAttemptOperations kernelCoreGameEngineProgramAttackAttemptOperations, ICoreShanheS5YimieKernelCoreMainOperations kernelCoreMainOperations, ICoreShanheS5YimieKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreHealthQueriesOperations = coreHealthQueriesOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageCompletionOperations = coreNativeDamageCompletionOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineProgramAttackAttemptOperations = kernelCoreGameEngineProgramAttackAttemptOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
