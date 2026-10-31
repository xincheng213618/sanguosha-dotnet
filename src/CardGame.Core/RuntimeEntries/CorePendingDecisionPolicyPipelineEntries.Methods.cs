#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePendingDecisionPolicyPipelineEntries
{
    internal global::CardGame.Core.PendingDecision? _pendingDecision { get => _resolve()._pendingDecision; set => _resolve()._pendingDecision = value; }
}
