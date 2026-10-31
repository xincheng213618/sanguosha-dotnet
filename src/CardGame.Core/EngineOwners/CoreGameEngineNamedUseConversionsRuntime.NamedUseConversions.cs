#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedUseConversionsRuntime
{
    internal static string NamedUseLedgerId(SkillProgramViewAs rule) => RuntimeSkills.RuntimeRules_Core_NamedUseConversions.NamedUseLedgerId(rule);
    internal bool IsNamedUseConversionAvailable(CharacterState owner, IndexedSkillProgramInstance instance, SkillProgramViewAs rule) => (!rule.NoDying || _coreDyingCursorRuntimeOperations.ActiveDying is null && !_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Hp <= 0)) && (!rule.UnusedOutputNameThisGame || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, instance.SkillId, NamedUseLedgerId(rule), SkillUsageScope.Game) == 0);
    internal void ConsumeNamedUseConversion(CardConversionSource source)
    {
        if (_corePlayCardHistoryOperations.ViewAsRule(source)is not { UnusedOutputNameThisGame: true } rule)
            return;
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(source.OwnerSeat, source.SkillId, NamedUseLedgerId(rule), SkillUsageScope.Game, 1))
            throw new InvalidOperationException("The converted output name was already used this game.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(source.OwnerSeat, source.SkillId, NamedUseLedgerId(rule), SkillUsageScope.Game, 1));
    }

    internal void ResetTurnProgramBooleanStates()
    {
        foreach (var key in _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.Keys.ToArray())
        {
            var definition = _coreProgramPersistentStateOperations.GetProgramBooleanStateDefinition2(key.SkillId, key.StateId);
            if (definition.ResetScope != SkillProgramStateResetScope.Turn || _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates[key] == definition.InitialValue)
                continue;
            _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates[key] = definition.InitialValue;
            if (definition.Visibility == SkillProgramStateVisibility.Public)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBooleanStateChangedEvent(key.OwnerSeat, key.SkillId, key.SkillInstanceId, key.StateId, definition.InitialValue, definition.Visibility));
        }
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NamedConversionBan, 10)]
    internal bool NamedConversionUnavailable(ProgramConfiguredViewAsQuery query) => !IsNamedUseConversionAvailable(query.Owner, query.Instance, query.Rule);
}
