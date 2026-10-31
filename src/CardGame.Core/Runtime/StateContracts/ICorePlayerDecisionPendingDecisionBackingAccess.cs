#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICorePlayerDecisionPendingDecisionBackingAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.PendingDecision? _pendingDecisionBacking { get; }
}
