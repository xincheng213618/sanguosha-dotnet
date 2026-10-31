namespace CardGame.Core;
public sealed record CurrentTurnDirectedHeartSlashBonus(int TurnNumber, int TurnOwnerSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int TargetSeat);
public sealed record CurrentTurnDirectedHeartSlashBonusGrantedEvent(CurrentTurnDirectedHeartSlashBonus Bonus) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses => ref _currentTurnShownCardBenefits.ReferenceToCurrentTurnHeartSlashBonuses;
}
