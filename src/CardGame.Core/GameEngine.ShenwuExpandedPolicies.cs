namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref (int Turn, int Seat, global::CardGame.Core.TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies => ref _shenwuExpandedPolicies.ReferenceToShenwuFengdongDependencies;
    private ref long _shenwuFengdongStamp => ref _shenwuExpandedPolicies.ReferenceToShenwuFengdongStamp;
}
