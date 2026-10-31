#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess : IProgramRuntimeOperations
{
    ref readonly List<DirectedTurnCardPolicy> _directedTurnCardPolicies { get; }
}
