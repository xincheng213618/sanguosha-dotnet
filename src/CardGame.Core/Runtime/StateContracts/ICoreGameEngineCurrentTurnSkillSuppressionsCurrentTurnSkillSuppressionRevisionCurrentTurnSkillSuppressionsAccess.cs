#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess : IProgramRuntimeOperations
{
    ref readonly List<CurrentTurnNonLockedSkillSuppression> _currentTurnSkillSuppressions { get; }

    ref long _currentTurnSkillSuppressionRevision { get; }
}
