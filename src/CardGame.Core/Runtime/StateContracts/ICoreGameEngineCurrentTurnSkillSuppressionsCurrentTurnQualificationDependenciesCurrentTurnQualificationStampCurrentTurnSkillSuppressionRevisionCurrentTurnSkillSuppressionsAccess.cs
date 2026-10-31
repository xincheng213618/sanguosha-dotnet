#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess : IProgramRuntimeOperations
{
    ref readonly List<CurrentTurnNonLockedSkillSuppression> _currentTurnSkillSuppressions { get; }

    ref long _currentTurnSkillSuppressionRevision { get; }

    ref (long Projection, long Suppression) _currentTurnQualificationDependencies { get; }

    ref long _currentTurnQualificationStamp { get; }
}
