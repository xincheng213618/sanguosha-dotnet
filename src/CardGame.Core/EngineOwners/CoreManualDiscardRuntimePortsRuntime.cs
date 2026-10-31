#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreManualDiscardRuntimePortsRuntime : IManualDiscardCommands, IManualDiscardFacts, IManualDiscardTransitions
{
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlannedPhaseQuantitiesEntries _corePlannedPhaseQuantitiesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CoreNativeRuleQueriesEntries _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly IKernelAcceptAndRejectOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    internal CoreManualDiscardRuntimePortsRuntime(RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEnginePlannedPhaseQuantitiesEntries corePlannedPhaseQuantitiesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CoreNativeRuleQueriesEntries coreNativeRuleQueriesOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, IKernelAcceptAndRejectOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess)
    {
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._corePlannedPhaseQuantitiesOperations = corePlannedPhaseQuantitiesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
    }
}
