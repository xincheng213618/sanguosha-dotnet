#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreDirectedTurnCardPoliciesState : Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess, Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess
{
    private List<DirectedTurnCardPolicy> _directedTurnCardPolicies = [];
    internal ref readonly List<DirectedTurnCardPolicy> ReferenceToDirectedTurnCardPolicies => ref _directedTurnCardPolicies;

    private long _directedTurnCardPolicySequence = default !;
    internal ref long ReferenceToDirectedTurnCardPolicySequence => ref _directedTurnCardPolicySequence;

    ref readonly List<DirectedTurnCardPolicy> Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies => ref _directedTurnCardPolicies;

    ref readonly List<DirectedTurnCardPolicy> Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicies => ref _directedTurnCardPolicies;

    ref long Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess._directedTurnCardPolicySequence => ref _directedTurnCardPolicySequence;
}
