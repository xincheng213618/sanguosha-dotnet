#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFrameAutomaticRuntimeRuntime
{
    bool global::CardGame.Core.INativeFrameAutomaticFacts.HasPendingDecision => _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null;

    int global::CardGame.Core.INativeFrameAutomaticFacts.StepCount(NativeFrameAutomaticPhase phase) => _coreModeNativeFrameRuntimeOperations.StepCount(phase);
    NativeFrameAutomaticStepMetadata global::CardGame.Core.INativeFrameAutomaticFacts.StepAt(NativeFrameAutomaticPhase phase, int index) => _coreModeNativeFrameRuntimeOperations.StepAt(phase, index);
    bool global::CardGame.Core.INativeFrameAutomaticFacts.Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => _nativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations.Matches(phase, index, frame);
    void global::CardGame.Core.INativeFrameAutomaticRules.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
