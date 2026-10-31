#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangHappyGodRuntime
{
    void IHappyZhugeLiangHost.GrantMist(ProgramSkillFrame frame) => GrantHappyDawu(frame);
    internal void GrantHappyDawu(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var bound = _coreProgramLifecycleOperations.GetProgramCardSet(frame, "stars");
        if (bound.CardIds.Count == 0)
            return;
        if (frame.SkillId != HappyZhugeLiangComposition.Dawu || frame.TriggerId != "self-mist" || frame.InstructionIndex != 3 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || frame.PendingMovementContinuation is not null || bound is not { CardIds: [var star], SourceLocations: [var from] } || from != new CardLocation(CardZoneKind.PrivateReserve, frame.OwnerSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyDawuGrantedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Happy Dawu lost its exact once-paid original Ending instruction and private star source.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var starts = history.Select((payload, index) => (payload, index)).Where(item => item.payload is ProgramBindingStartedEvent start && start.FrameId == frame.Id && start.SkillId == frame.SkillId && start.OwnerSeat == frame.OwnerSeat && start.SkillInstanceId == frame.SkillInstanceId && start.BindingId == frame.TriggerId && start.Window == SkillProgramTriggerWindow.TurnEnding).ToArray();
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}");
        var costs = starts is [var started] ? history.Skip(started.index + 1).OfType<CardMovedEvent>().Where(m => m.CardId == star && m.From == from && m.To != from && m.Reason == reason).ToArray() : [];
        if (costs is not [var cost] || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == star && m.From == cost.From && m.To == cost.To && m.Reason == reason)is not { } movement)
            throw new InvalidOperationException("Happy Dawu requires the exact physical star movement after this binding actually started.");
        _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, HappyZhugeLiangComposition.MistState, true);
        _coreProgramBooleanStateExpirationOperations.ScheduleProgramBooleanStateExpiration(frame, [HappyZhugeLiangComposition.MistState], SkillProgramDamageModifierExpiration.NextOwnerTurnStart);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDawuGrantedEvent(frame.Id, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, movement.Sequence));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 41480)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyDawuNormalDamage(GameEngine.IDamageAttempt attack)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantHappyZhugeLiangDawu) || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != DamageNature.Normal)
            return[];
        var grant = _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases.FirstOrDefault(lease => lease.Source.OwnerSeat == attack.TargetSeat && lease.Source.SkillId == HappyZhugeLiangComposition.Dawu && lease.Source.BindingId == "self-mist" && lease.StateId == HappyZhugeLiangComposition.MistState && lease.Expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart && _coreProgramPersistentStateOperations.GetProgramBooleanState2(lease.Source.OwnerSeat, lease.Source.SkillId, lease.Source.SkillInstanceId, lease.StateId));
        return grant is null ? [] : [(grant.Source, -1)];
    }
}
