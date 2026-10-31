#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuLianShiMobileRuntime
{
    void IMobileBuLianShiAnxuHost.Prepare(ProgramSkillFrame frame) => PrepareMobileBuLianShiAnxu(frame);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 72400)]
    internal void RecordMobileBuLianShiAnxuDelivery(CardMovedEvent fact)
    {
        if (fact.Reason.Value != "skill-program.mobile:anxu.SelectAndMoveOwnedCard" || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: "mobile:anxu", InstructionIndex: 2, SelectedTargetSeats: [var recipient, var donor] } f || fact.To != CardLocation.Hand(recipient) || fact.From != CardLocation.Hand(donor) && fact.From != CardLocation.Processing || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()is not { } movement || movement.CardId != fact.CardId || movement.From != fact.From || movement.To != fact.To || movement.Reason != fact.Reason || !_activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.TryPeek(out var batch))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileBuLianShiAnxuDeliveredEvent(f.Id, f.SkillInstanceId, f.GameplayHash, recipient, donor, movement.CardId, batch, movement.Sequence));
    }

    internal bool MobileBuLianShiAnxuPaid(ProgramSkillFrame f) => f.SkillId == "mobile:anxu" && f.TriggerId is null && f.ActivationId == "donor-gives-to-lower-hand" && f.InstructionIndex >= 2 && f.SelectedTargetSeats is [var recipient, var donor] && f.CardSetBindings.SingleOrDefault(b => b.Name == "mobile-anxu-gift")is { CardIds: [var card] } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileBuLianShiAnxuDeliveredEvent>().Count(e => e.FrameId == f.Id && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash && e.RecipientSeat == recipient && e.DonorSeat == donor && e.CardId == card && e.BatchId > f.Id && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == card && (m.From == CardLocation.Hand(donor) || m.From == CardLocation.Processing) && m.To == CardLocation.Hand(recipient) && m.Reason.Value == "skill-program.mobile:anxu.SelectAndMoveOwnedCard")) == 1;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 72400)]
    internal bool KeepMobileBuLianShiAnxuPaid(ProgramSkillFrame f) => MobileBuLianShiAnxuPaid(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 72400)]
    internal bool ReturnMobileBuLianShiAnxuPaid(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 2 || !MobileBuLianShiAnxuPaid(f) || f.SelectedTargetSeats is not [_, var donor] || f.PendingMovementContinuation != new ProgramMovementContinuation(donor, 0, null))
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    internal void PrepareMobileBuLianShiAnxu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "mobile:anxu" || f.TriggerId is not null || f.InstructionIndex != 3 || f.SelectedTargetSeats is not [var recipient, var donor] || recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat || _coreProgramLifecycleOperations.GetProgramCardSet(f, "mobile-anxu-gift")is not { CardIds.Count: 1, SourceLocations: [var boundLocation] } || boundLocation != CardLocation.Hand(recipient) || !MobileBuLianShiAnxuPaid(f) || f.ChoiceBindings.Any(b => b.Name == "mobile-anxu-equality"))
            throw new InvalidOperationException("Mobile Anxu compares only its original unequal pair after the actual donor's native gift children return.");
        var first = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).Count;
        var second = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[donor]).Count;
        var equal = _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && _coreMatchParticipantsPlayersAccess._players[donor].IsAlive && first == second;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("mobile-anxu-equality", equal ? "equal" : "unequal", f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileBuLianShiAnxuComparedEvent(f.Id, recipient, donor, first, second, equal));
    }
}
