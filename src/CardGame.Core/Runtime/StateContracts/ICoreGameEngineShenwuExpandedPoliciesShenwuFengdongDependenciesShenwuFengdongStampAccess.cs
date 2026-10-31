#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref (int Turn, int Seat, global::CardGame.Core.TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies { get; }

    ref long _shenwuFengdongStamp { get; }
}
