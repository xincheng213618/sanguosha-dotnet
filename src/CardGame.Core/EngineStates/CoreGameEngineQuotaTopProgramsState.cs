#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineQuotaTopProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess
{
    private global::System.Collections.Generic.Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Target, string Skill), long> ReferenceToFirstTurnTargetActions => ref _firstTurnTargetActions;

    ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Target, string Skill), long> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess._firstTurnTargetActions => ref _firstTurnTargetActions;
}
