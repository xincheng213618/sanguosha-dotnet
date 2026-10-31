#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CorePlayerDecisionState : Contracts.ICorePlayerDecisionNextPromptIdAccess, Contracts.ICorePlayerDecisionPendingDecisionBackingAccess
{
    private long _nextPromptId = default !;
    internal ref long ReferenceToNextPromptId => ref _nextPromptId;

    private PendingDecision? _pendingDecisionBacking = default !;
    internal ref PendingDecision? ReferenceToPendingDecisionBacking => ref _pendingDecisionBacking;

    ref long Contracts.ICorePlayerDecisionNextPromptIdAccess._nextPromptId => ref _nextPromptId;

    ref PendingDecision? Contracts.ICorePlayerDecisionPendingDecisionBackingAccess._pendingDecisionBacking => ref _pendingDecisionBacking;
}
