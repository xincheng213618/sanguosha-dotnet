namespace CardGame.Core;
public sealed record CurrentTurnDirectedHeartSlashBonus(int TurnNumber, int TurnOwnerSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int TargetSeat);
public sealed record CurrentTurnDirectedHeartSlashBonusGrantedEvent(CurrentTurnDirectedHeartSlashBonus Bonus) : IGameEvent;
