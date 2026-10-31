#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess : IProgramRuntimeOperations
{
    ref readonly List<DirectedTurnCardPolicy> _directedTurnCardPolicies { get; }

    ref long _directedTurnCardPolicySequence { get; }
}
