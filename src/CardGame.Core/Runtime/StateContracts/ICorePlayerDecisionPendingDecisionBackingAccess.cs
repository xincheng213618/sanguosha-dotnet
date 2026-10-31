#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICorePlayerDecisionPendingDecisionBackingAccess : IProgramRuntimeOperations
{
    ref PendingDecision? _pendingDecisionBacking { get; }
}
