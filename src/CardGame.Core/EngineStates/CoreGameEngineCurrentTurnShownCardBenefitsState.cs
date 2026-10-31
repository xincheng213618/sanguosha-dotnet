#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCurrentTurnShownCardBenefitsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnDirectedHeartSlashBonus> ReferenceToCurrentTurnHeartSlashBonuses => ref _currentTurnHeartSlashBonuses;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnDirectedHeartSlashBonus> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess._currentTurnHeartSlashBonuses => ref _currentTurnHeartSlashBonuses;
}
