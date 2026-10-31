#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> _directedTurnCardPolicies { get; }

    ref long _directedTurnCardPolicySequence { get; }
}
