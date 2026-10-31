namespace CardGame.Core;
public sealed record DirectedTurnCardPolicy(long GrantSequence, int TurnNumber, int TurnSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int ActorSeat, int TargetSeat, IReadOnlyList<CardKind> CardKinds, DirectedTurnCardPolicyEffect Effects);
public sealed record DirectedTurnCardPolicyGrantedEvent(DirectedTurnCardPolicy Policy) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DirectedTurnCardPolicy> _directedTurnCardPolicies => ref _directedTurnCardPolicies2.ReferenceToDirectedTurnCardPolicies;
    private ref long _directedTurnCardPolicySequence => ref _directedTurnCardPolicies2.ReferenceToDirectedTurnCardPolicySequence;
}
