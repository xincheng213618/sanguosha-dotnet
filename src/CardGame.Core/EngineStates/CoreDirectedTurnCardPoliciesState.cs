#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreDirectedTurnCardPoliciesState : global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> _directedTurnCardPolicies = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> ReferenceToDirectedTurnCardPolicies => ref _directedTurnCardPolicies;

    private long _directedTurnCardPolicySequence = default !;
    internal ref long ReferenceToDirectedTurnCardPolicySequence => ref _directedTurnCardPolicySequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies => ref _directedTurnCardPolicies;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies => ref _directedTurnCardPolicies;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicySequence => ref _directedTurnCardPolicySequence;
}
