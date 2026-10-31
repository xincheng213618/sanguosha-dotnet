#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess : IProgramRuntimeOperations
{
    ref (int Turn, int Seat, TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies { get; }

    ref long _shenwuFengdongStamp { get; }
}
