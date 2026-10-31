#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed partial class CorePendingDecisionPolicyPipelineEntries
{
    private readonly Func<EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _resolve;
    private CorePendingDecisionPolicyPipelineEntries(Func<EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePendingDecisionPolicyPipelineEntries Create(Func<EngineOwners.CorePendingDecisionPolicyPipelineRuntime> resolve) => new(resolve);
}
