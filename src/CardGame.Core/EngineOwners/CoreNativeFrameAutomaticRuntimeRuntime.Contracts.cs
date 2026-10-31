#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int StepCount(global::CardGame.Core.NativeFrameAutomaticPhase phase);
    global::CardGame.Core.NativeFrameAutomaticStepMetadata StepAt(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index);
    bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame);
    bool TryAdvance(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeFrameAutomaticRuntimeKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreNativeFrameAutomaticRuntimeNativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeFrameAutomaticRuntimeCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}
