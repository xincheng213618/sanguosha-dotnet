namespace CardGame.Core;
public sealed record DirectedTurnCardPolicy(long GrantSequence, int TurnNumber, int TurnSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int ActorSeat, int TargetSeat, IReadOnlyList<CardKind> CardKinds, DirectedTurnCardPolicyEffect Effects);
public sealed record DirectedTurnCardPolicyGrantedEvent(DirectedTurnCardPolicy Policy) : IGameEvent;
