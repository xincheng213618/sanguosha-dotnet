#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess : IProgramRuntimeOperations
{
    ref readonly List<CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses { get; }
}
