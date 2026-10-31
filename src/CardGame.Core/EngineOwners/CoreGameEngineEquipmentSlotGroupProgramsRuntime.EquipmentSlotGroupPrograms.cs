#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentSlotGroupProgramsRuntime
{
    internal bool ResolveEnteringChain(CharacterState player, bool requested) => requested && !player.IsChained && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.PreventEnteringChain) ? false : requested;
    internal ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat) => new(seat, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count, _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ProhibitPindianTarget), _characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[seat]));
    internal bool CanBePindianTarget(int sourceSeat, int targetSeat) => ProgramOperationLegalityPolicy.HandContest.CanSelectTarget(CreateProgramLegalityParticipant(sourceSeat), CreateProgramLegalityParticipant(targetSeat));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 140)]
    internal bool CanPayEquipmentSlotGroup(CharacterState owner, SkillProgramActivation activation) => ProgramInstructionResolver.Default.Features(activation).ForOperation(SkillProgramEffectOp.AbolishEquipmentSlotGroup).All(e => e.EquipmentSlots.Any(slot => owner.EquipmentSlotCapacity(slot) > 0));
    internal bool CanSelectProgramPindianOpponent(ProgramSkillFrame frame, int targetSeat) => _programTargetSelectionOperations.Get().CanSelectPindianOpponent(frame, targetSeat);
    void IEquipmentSlotGroupProgramHost.AbolishEquipmentSlotGroup(ProgramSkillFrame frame, IReadOnlyList<EquipmentSlot> slots) => AbolishProgramEquipmentSlotGroup(frame, slots);
    void IEquipmentSlotGroupProgramHost.RecastSelectedEquipment(ProgramSkillFrame frame) => RecastProgramSelectedEquipment(frame);
    void IEquipmentSlotGroupProgramHost.ReplaceSkillsOnPreparation(ProgramSkillFrame frame, IReadOnlyList<string> lost, string acquired) => ReplaceProgramPreparationSkills(frame, lost, acquired);
    internal void AbolishProgramEquipmentSlotGroup(ProgramSkillFrame frame, IReadOnlyList<EquipmentSlot> slots)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (frame.TriggerId is not null || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != owner.Seat || !owner.IsAlive || slots.Count == 0 || !slots.Any(slot => owner.EquipmentSlotCapacity(slot) > 0))
            throw new InvalidOperationException("Slot payment lost its exact own Play activation or remaining cost.");
        foreach (var slot in slots.Where(slot => owner.EquipmentSlotCapacity(slot) > 0))
            _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(owner, slot, 0);
    }

    internal void RecastProgramSelectedEquipment(ProgramSkillFrame frame)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (frame.TriggerId is not null || frame.SelectedCardIds is not [var id] || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != owner.Seat || !owner.IsAlive)
            throw new InvalidOperationException("Equipment recast lost its actual owner activation.");
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        if (location.OwnerSeat != owner.Seat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Equipment recast requires a real owned hand/equipment entity.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == id);
        if (!_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
            throw new InvalidOperationException("Recast entity is not equipment.");
        frame = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(frame, owner.Seat, [card.Id]);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        if (_coreProgramEquipmentRecastRecoveriesOperations.TryPauseProgramEquipmentRecast(frame, card))
            return;
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, CardMoveReasons.RecastDraw);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, card.Id, card.Kind, drawn.Count));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(frame, drawn.Count);
    }

    internal void ReplaceProgramPreparationSkills(ProgramSkillFrame frame, IReadOnlyList<string> lost, string acquired)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: var seat } || seat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            throw new InvalidOperationException("Preparation replacement lost its own actual turn boundary.");
        _coreProgramSkillChangesOperations.ReplaceProgramSkills(frame, lost, [acquired], ProgramSkillSourceRemoval.All, acquireFirst: true, announceAwakening: true);
        _coreProgramSkillChangesOperations.CommitProgramSkillChangeFact(frame, new ProgramOwnerSkillsReplacedEvent(frame.Id, frame.SkillId, seat, Array.AsReadOnly(lost.ToArray()), acquired));
    }
}
