#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal bool MouHuCheErEquipmentDisabled(int target) =>
        _coreMatchParticipantsPlayersAccess._players[target].SkillGrants.ArePrintedEquipmentEffectsSuppressed;
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(CharacterState player) =>
        _coreFieldEquipmentOperations.GetEquipmentWithPrintedEffects(player);
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(int seat) => KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    internal IReadOnlyList<Card> KongwuUsableWoodenOxGrain(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(player);

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
