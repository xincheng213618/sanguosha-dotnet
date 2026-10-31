#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePrepPoisonObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries _coreOrderedPrintedSkillLossObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries _corePrepPoisonOutcomesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEnginePrepPoisonObserversRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries coreOrderedPrintedSkillLossObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries corePrepPoisonOutcomesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreOrderedPrintedSkillLossObserversOperations = coreOrderedPrintedSkillLossObserversOperations;
        this._corePrepPoisonOutcomesOperations = corePrepPoisonOutcomesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
