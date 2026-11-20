#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.AppendNumericRuleContributions, 40)]
    internal void AddKongwuDrawContribution(int seat, SkillRuleQuery query, List<RuleQueryContribution> contributions)
    {
        var turn = _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        if (query != SkillRuleQuery.DrawCount || turn._currentSeat != seat || turn._turnProgression.OwnerSeat != seat)
            return;

        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var turnNumber = turn._turnNumber;
        var debtCount = MouHuCheErRuntime.ActiveDebtOrigins(player).Count(origin =>
            _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuCheErDebtTurnEvent>()
                .Any(due => due.FrameId == origin.FrameId && due.Target == seat && due.Turn == turnNumber));
        CoreGameEngineRuleQueriesRuntime.AddFiniteContribution(contributions, $"issued:{seat}:mou-kongwu:draw", -debtCount);
    }
}
