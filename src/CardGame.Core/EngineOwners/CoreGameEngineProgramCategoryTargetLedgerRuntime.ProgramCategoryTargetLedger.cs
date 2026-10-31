#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerRuntime
{
    void IControlProgramEffectHost.ConsumeCategoryTargetLedger(ProgramSkillFrame frame, string usageId) => ConsumeProgramCategoryTargetLedger(frame, usageId);
    void IControlProgramEffectHost.ReplaceCurrentCardUseActor(ProgramSkillFrame frame, int actorSeat) => _coreProgramCardUseRolesOperations.ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    SkillProgramStepOutcome IControlProgramEffectHost.AddCurrentCardUseTarget(ProgramSkillFrame frame, int targetSeat) => _coreProgramCardUseRolesOperations.AddCurrentProgramCardUseTarget(frame, targetSeat);
    void IControlProgramEffectHost.ReduceCurrentDamage(ProgramSkillFrame frame, int amount) => _coreProgramDamageReductionOperations.Value.ReduceProgramCurrentDamage(frame, amount);
    internal bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, IReadOnlyList<int> cardIds, IReadOnlyList<int> targetSeats)
    {
        if (cardIds.Count != 1 || targetSeats.Count != 1 || targetSeats[0] == ownerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeats[0]].IsAlive)
            return false;
        var location = _cardZonesOperations._cardZones.GetLocation(cardIds[0]);
        if (location.OwnerSeat != ownerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            return false;
        var card = _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(card => card.Id == cardIds[0]);
        if (card is null)
            return false;
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        return _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, skillId, $"{(usageId)}:category:{(category)}", SkillUsageScope.Phase) == 0 && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, skillId, $"{(usageId)}:target:{(targetSeats[0])}", SkillUsageScope.Phase) == 0;
    }

    internal void ConsumeProgramCategoryTargetLedger(ProgramSkillFrame program, string usageId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(program.Id);
        if (active.TriggerId is not null || !CanActivateCategoryTargetLedger(active.OwnerSeat, active.SkillId, usageId, active.SelectedCardIds, active.SelectedTargetSeats))
            throw new InvalidOperationException("Category/target usage must be validated before any activation payment.");
        var location = _cardZonesOperations._cardZones.GetLocation(active.SelectedCardIds[0]);
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == active.SelectedCardIds[0]).Kind);
        Consume($"{(usageId)}:category:{(category)}", SkillUsageScope.Phase, 1);
        Consume($"{(usageId)}:target:{(active.SelectedTargetSeats[0])}", SkillUsageScope.Phase, 1);
        Consume(usageId, SkillUsageScope.Turn, int.MaxValue);
        void Consume(string key, SkillUsageScope scope, int limit)
        {
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(active.OwnerSeat, active.SkillId, key, scope, limit))
                throw new InvalidOperationException("A validated category/target ledger could not advance.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(active.OwnerSeat, active.SkillId, key, scope, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(active.OwnerSeat, active.SkillId, key, scope)));
        }
    }

    internal int GetProgramCategoryTargetTurnUsage(ProgramSkillFrame frame) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), SkillUsageScope.Turn);
    internal int GetProgramPhaseSkillUsage(ProgramSkillFrame frame, string usageId) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, usageId, SkillUsageScope.Phase);
}
