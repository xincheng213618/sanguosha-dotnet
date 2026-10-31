#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthChildOperations(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IProgramHealthChildOperations
{
    public bool AdvanceObserverSubtree() => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is null && getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(true);
}
