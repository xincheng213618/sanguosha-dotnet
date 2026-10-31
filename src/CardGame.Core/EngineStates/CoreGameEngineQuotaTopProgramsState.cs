#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineQuotaTopProgramsState : Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess
{
    private Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions = [];
    internal ref readonly Dictionary<(int Turn, int Target, string Skill), long> ReferenceToFirstTurnTargetActions => ref _firstTurnTargetActions;

    ref readonly Dictionary<(int Turn, int Target, string Skill), long> Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess._firstTurnTargetActions => ref _firstTurnTargetActions;
}
