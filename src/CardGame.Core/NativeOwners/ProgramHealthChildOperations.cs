#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthChildOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IProgramHealthChildOperations
{
    public bool AdvanceObserverSubtree() => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is null && getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(true);
}
