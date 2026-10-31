#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnNonLockedSkillSuppression> _currentTurnSkillSuppressions { get; }

    ref long _currentTurnSkillSuppressionRevision { get; }
}
