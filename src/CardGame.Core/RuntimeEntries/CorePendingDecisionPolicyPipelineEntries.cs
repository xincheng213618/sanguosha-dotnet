#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed partial class CorePendingDecisionPolicyPipelineEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _resolve;
    private CorePendingDecisionPolicyPipelineEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePendingDecisionPolicyPipelineEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve) => new(resolve);
}
