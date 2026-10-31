#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeFrameAutomaticFacts(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, NativeStepCountQuery readStepCount, NativeStepAtQuery readStepAt, GetNativeMatchesTarget getMatchesTarget) : INativeFrameAutomaticFacts
{
    public bool HasPendingDecision => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is not null;

    public int StepCount(NativeFrameAutomaticPhase phase) => readStepCount(phase);
    public NativeFrameAutomaticStepMetadata StepAt(NativeFrameAutomaticPhase phase, int index) => readStepAt(phase, index);
    public bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => getMatchesTarget(phase, index)(frame);
}
