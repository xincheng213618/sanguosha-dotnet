#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess : IProgramRuntimeOperations
{
    ref long _currentTurnSkillSuppressionRevision { get; }

    ref (long Projection, long Suppression) _currentTurnQualificationDependencies { get; }

    ref long _currentTurnQualificationStamp { get; }
}
