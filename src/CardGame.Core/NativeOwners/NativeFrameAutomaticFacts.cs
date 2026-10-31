#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeFrameAutomaticFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::CardGame.Core.NativeOwners.NativeStepCountQuery readStepCount, global::CardGame.Core.NativeOwners.NativeStepAtQuery readStepAt, global::CardGame.Core.NativeOwners.GetNativeMatchesTarget getMatchesTarget) : INativeFrameAutomaticFacts
{
    public bool HasPendingDecision => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is not null;

    public int StepCount(NativeFrameAutomaticPhase phase) => readStepCount(phase);
    public NativeFrameAutomaticStepMetadata StepAt(NativeFrameAutomaticPhase phase, int index) => readStepAt(phase, index);
    public bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => getMatchesTarget(phase, index)(frame);
}
