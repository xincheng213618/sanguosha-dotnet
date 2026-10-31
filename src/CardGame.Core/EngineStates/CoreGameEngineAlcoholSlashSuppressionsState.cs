#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAlcoholSlashSuppressionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnOwnSkillSuppression> _currentTurnOwnSkillSuppressions = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnOwnSkillSuppression> ReferenceToCurrentTurnOwnSkillSuppressions => ref _currentTurnOwnSkillSuppressions;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnOwnSkillSuppression> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions => ref _currentTurnOwnSkillSuppressions;
}
