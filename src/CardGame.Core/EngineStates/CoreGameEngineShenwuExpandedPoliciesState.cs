#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineShenwuExpandedPoliciesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess
{
    private (int Turn, int Seat, global::CardGame.Core.TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies = default !;
    internal ref (int Turn, int Seat, global::CardGame.Core.TurnPhase Phase, long Grants, bool Alive, bool Revealed)? ReferenceToShenwuFengdongDependencies => ref _shenwuFengdongDependencies;

    private long _shenwuFengdongStamp = default !;
    internal ref long ReferenceToShenwuFengdongStamp => ref _shenwuFengdongStamp;

    ref (int Turn, int Seat, global::CardGame.Core.TurnPhase Phase, long Grants, bool Alive, bool Revealed)? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongDependencies => ref _shenwuFengdongDependencies;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongStamp => ref _shenwuFengdongStamp;
}
