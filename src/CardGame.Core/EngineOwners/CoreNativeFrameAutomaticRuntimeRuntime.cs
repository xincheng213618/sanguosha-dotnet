#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeFrameAutomaticRuntimeRuntime : global::CardGame.Core.INativeFrameAutomaticFacts, global::CardGame.Core.INativeFrameAutomaticRules
{
    private readonly ICoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeOperations _coreModeNativeFrameRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchesOperations _nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreNativeFrameAutomaticRuntimeRuntime(ICoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeOperations coreModeNativeFrameRuntimeOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineOwners.IKernelMatchesOperations nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreModeNativeFrameRuntimeOperations = coreModeNativeFrameRuntimeOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations = nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
