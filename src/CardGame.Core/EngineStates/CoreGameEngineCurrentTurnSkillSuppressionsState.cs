#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCurrentTurnSkillSuppressionsState : Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess
{
    private long _currentTurnSkillSuppressionRevision = default !;
    internal ref long ReferenceToCurrentTurnSkillSuppressionRevision => ref _currentTurnSkillSuppressionRevision;

    ref long Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision => ref _currentTurnSkillSuppressionRevision;

}
