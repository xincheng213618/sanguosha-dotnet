#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineShenwuExpandedPoliciesState : Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess
{
    private (int Turn, int Seat, TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies = default !;
    internal ref (int Turn, int Seat, TurnPhase Phase, long Grants, bool Alive, bool Revealed)? ReferenceToShenwuFengdongDependencies => ref _shenwuFengdongDependencies;

    private long _shenwuFengdongStamp = default !;
    internal ref long ReferenceToShenwuFengdongStamp => ref _shenwuFengdongStamp;

    ref (int Turn, int Seat, TurnPhase Phase, long Grants, bool Alive, bool Revealed)? Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongDependencies => ref _shenwuFengdongDependencies;

    ref long Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongStamp => ref _shenwuFengdongStamp;
}
