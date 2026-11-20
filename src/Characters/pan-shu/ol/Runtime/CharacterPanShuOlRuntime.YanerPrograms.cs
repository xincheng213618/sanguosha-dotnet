#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPanShuOlRuntime
{
    internal SkillProgramStepOutcome YanerProgramResolvePairBenefit(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var loserSeat = active.WindowContext?.SourceSeat ?? throw new InvalidOperationException("The yaner pair lost its last-hand loser.");
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreProgramLifecycleOperations.DrawProgramCards(active.Id, active.OwnerSeat, 2, null, "yaner-own", SkillProgramCardSetVisibility.Private, new($"skill-program.{(active.SkillId)}.yaner-draw"));
        IReadOnlyList<int> otherDrawn = [];
        if (_coreTableSeatQueriesOperations.IsValidPlayerSeat(loserSeat) && loserSeat != active.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[loserSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.DrawProgramCards(active.Id, loserSeat, 2, null, "yaner-other", SkillProgramCardSetVisibility.Private, new($"skill-program.{(active.SkillId)}.yaner-draw"));
            otherDrawn = _coreProgramLifecycleOperations.GetProgramCardSet(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), "yaner-other").CardIds;
        }

        var ownDrawn = _coreProgramLifecycleOperations.GetProgramCardSet(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), "yaner-own").CardIds;
        if (IsYanerMatchingDrawPair(ownDrawn))
        {
            var targetSkillId = effect.SkillIds.Single();
            SetForeignProgramBooleanState(active, targetSkillId, effect.StateId!, value: true);
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{owner.Name} 以【燕尔】摸到两张同类别的牌，【{_contentRegistryOperations._contentRegistry!.GetSkill(targetSkillId).Name}】直到其下回合开始删去“你的回合内”。", active.OwnerSeat);
        }
        if (IsYanerMatchingDrawPair(otherDrawn))
            ApplyProgramSkillRecovery(active.Id, active.OwnerSeat, loserSeat);

        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool IsYanerMatchingDrawPair(IReadOnlyList<int> cardIds)
    {
        return cardIds is [var first, var second] && GetCategory(first) == GetCategory(second);

        SkillProgramCardCategory GetCategory(int cardId)
        {
            var zones = _cardZonesOperations._cardZones;
            var card = zones.CardsAt(zones.GetLocation(cardId)).Single(item => item.Id == cardId);
            return CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        }
    }
}
