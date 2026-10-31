#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlcoholSlashSuppressionsState : Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess
{
    private List<CurrentTurnOwnSkillSuppression> _currentTurnOwnSkillSuppressions = [];
    internal ref readonly List<CurrentTurnOwnSkillSuppression> ReferenceToCurrentTurnOwnSkillSuppressions => ref _currentTurnOwnSkillSuppressions;

    ref readonly List<CurrentTurnOwnSkillSuppression> Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions => ref _currentTurnOwnSkillSuppressions;
}
