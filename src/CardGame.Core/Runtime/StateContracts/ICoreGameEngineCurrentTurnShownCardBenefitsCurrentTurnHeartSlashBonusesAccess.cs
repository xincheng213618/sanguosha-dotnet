#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses { get; }
}
