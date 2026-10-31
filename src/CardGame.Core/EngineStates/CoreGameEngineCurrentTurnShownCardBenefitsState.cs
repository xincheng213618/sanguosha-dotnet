#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCurrentTurnShownCardBenefitsState : Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess
{
    private List<CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses = [];
    internal ref readonly List<CurrentTurnDirectedHeartSlashBonus> ReferenceToCurrentTurnHeartSlashBonuses => ref _currentTurnHeartSlashBonuses;

    ref readonly List<CurrentTurnDirectedHeartSlashBonus> Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses => ref _currentTurnHeartSlashBonuses;
}
