namespace CardGame.Core;

public sealed record DamageReplacedWithHpLossEvent(long ParentResolutionId, int PolicyOwnerSeat,
    int TargetSeat, int Amount, int RemainingHp) : IGameEvent;
