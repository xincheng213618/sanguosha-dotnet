#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess : IProgramRuntimeOperations
{
    ref readonly List<CurrentTurnOwnSkillSuppression> _currentTurnOwnSkillSuppressions { get; }
}
