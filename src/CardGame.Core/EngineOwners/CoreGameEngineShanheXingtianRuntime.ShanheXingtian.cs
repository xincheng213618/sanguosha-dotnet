#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheXingtianRuntime
{
    internal const string ShanheXingtianSkill = "shanhe:xingtian-axe-20766-effect";
    internal bool HasShanheXingtianSeal(int seat) => _turnCardUseEffectsOperations._turnCardUseEffects.TargetRestrictions.Any(r => r.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && r.TargetSeat == seat && r.Restriction == SkillProgramCardTargetRestriction.CannotUseOrPlayForTurn);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 59300)]
    internal bool ShanheXingtianCardUseForbidden(ProgramCardUseQuery query) => HasShanheXingtianSeal(query.ActorSeat);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 59300)]
    internal bool ShanheXingtianNativeResponseForbidden(ProgramNativeResponseQuery query) => HasShanheXingtianSeal(query.Provider.Seat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 59300, SkillProgramEffectOp.ApplyShanheXingtianSeal)]
    internal bool CanOfferShanheXingtian(ProgramTriggerLegalityContext gate) => gate.SkillId != ShanheXingtianSkill || gate.WindowContext is { TargetSeat: { } target } && target != gate.OwnerSeat && !HasShanheXingtianSeal(target) && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat]).Any(c => c.Kind == CardKind.ShanheXingtianAxe && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(gate.OwnerSeat, gate.SkillId, gate.Candidate.SkillInstanceId, c)) && _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(gate.OwnerSeat)).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat]).Count >= 2;
    internal bool HasShanheXingtianPaidCost(ProgramSkillFrame f)
    {
        if (f.SkillId != ShanheXingtianSkill || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, TargetSeat: { } target } || target == f.OwnerSeat || f.CardSetBindings.SingleOrDefault(b => b.Name == "axe-cost")is not { CardIds.Count: 2, SourceLocations.Count: 2 } cost)
            return false;
        var invoices = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat && e.SubjectSeat == f.OwnerSeat && e.ActualCount == 2 && e.Reason.Value == $"skill-program.{(f.SkillId)}.MoveBoundCards").ToArray();
        if (invoices is not [var invoice])
            return false;
        return cost.CardIds.Select((id, i) => (id, from: cost.SourceLocations[i])).All(c => c.from.OwnerSeat == f.OwnerSeat && c.from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.InvocationId == invoice.InvocationId && e.CardId == c.id && e.From == c.from && e.CompletionSequence > invoice.SequenceBefore && e.CompletionSequence <= invoice.SequenceAfter) == 1);
    }

    internal void ApplyShanheXingtianSeal(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!HasShanheXingtianPaidCost(f) || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == f.WindowContext!.ParentFrameId)is not { } parent || parent.Action.ActorSeat != f.OwnerSeat || parent.Action.EffectiveDesignatedTargetSeats is not [var original] || original != target)
            throw new InvalidOperationException("The S5 axe lost its exact use, target or completed two-card cost.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return;
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(f);
        var index = f.InstructionIndex - 1;
        var source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f);
        var ban = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.Id, -2 * index - 1, source, SkillProgramCardTargetRestriction.CannotUseOrPlayForTurn, target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(ban));
        var armor = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.Id, -2 * index - 2, source, SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn, target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(armor));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 59300)]
    internal bool ContinueShanheXingtianPaidCost(ProgramSkillFrame f) => HasShanheXingtianPaidCost(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 59300)]
    internal bool ReturnShanheXingtianPaidMovement(ProgramSkillFrame f)
    {
        if (!HasShanheXingtianPaidCost(f))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (effect is not { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "axe-cost", AwaitMovementTriggers: true } || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } movement || movement.SubjectSeat != f.OwnerSeat)
            throw new InvalidOperationException("The paid S5 axe lost its exact native discard-child return.");
        // Removing the source axe is a legal material payment. This original
        // invoice owns the continuation before the generic live-source guard.
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    void IShanheXingtianHost.ApplyShanheXingtianSeal(ProgramSkillFrame f) => ApplyShanheXingtianSeal(f);
}
