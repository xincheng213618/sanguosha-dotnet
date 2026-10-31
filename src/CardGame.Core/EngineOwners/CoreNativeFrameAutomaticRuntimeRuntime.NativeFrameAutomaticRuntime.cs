#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFrameAutomaticRuntimeRuntime
{
    bool INativeFrameAutomaticFacts.HasPendingDecision => _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null;

    int INativeFrameAutomaticFacts.StepCount(NativeFrameAutomaticPhase phase) => _coreModeNativeFrameRuntimeOperations.StepCount(phase);
    NativeFrameAutomaticStepMetadata INativeFrameAutomaticFacts.StepAt(NativeFrameAutomaticPhase phase, int index) => _coreModeNativeFrameRuntimeOperations.StepAt(phase, index);
    bool INativeFrameAutomaticFacts.Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => _nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations.Matches(phase, index, frame);
    void INativeFrameAutomaticRules.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
