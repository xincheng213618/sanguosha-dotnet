namespace CardGame.Core;

public sealed record WushuangJiushiDamageFaceEvent(long DamageFrameId, int OwnerSeat, bool WasFaceDown) : IGameEvent;
public sealed record WushuangDiscardPhaseBoundaryEvent(int TurnNumber, int ActorSeat, long MovementSequence) : IGameEvent;
