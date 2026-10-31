#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePendingDecisionPolicyPipelineEntries
{
    internal PendingDecision? _pendingDecision { get => _resolve()._pendingDecision; set => _resolve()._pendingDecision = value; }
}
