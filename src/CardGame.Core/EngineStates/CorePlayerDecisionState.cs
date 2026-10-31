#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CorePlayerDecisionState : global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess, global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess
{
    private long _nextPromptId = default !;
    internal ref long ReferenceToNextPromptId => ref _nextPromptId;

    private global::CardGame.Core.PendingDecision? _pendingDecisionBacking = default !;
    internal ref global::CardGame.Core.PendingDecision? ReferenceToPendingDecisionBacking => ref _pendingDecisionBacking;

    ref long global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess._nextPromptId => ref _nextPromptId;

    ref global::CardGame.Core.PendingDecision? global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess._pendingDecisionBacking => ref _pendingDecisionBacking;
}
