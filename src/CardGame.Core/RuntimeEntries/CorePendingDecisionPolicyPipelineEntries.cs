#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePendingDecisionPolicyPipelineEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _resolve;
    private CorePendingDecisionPolicyPipelineEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePendingDecisionPolicyPipelineEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve) => new(resolve);
}
