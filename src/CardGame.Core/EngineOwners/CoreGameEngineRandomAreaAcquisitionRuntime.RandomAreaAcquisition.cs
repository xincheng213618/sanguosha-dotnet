#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionRuntime
{
    internal static CardConversionSource RandomAreaSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_RandomAreaAcquisition.RandomAreaSource(frame);
    internal static CardMoveReason RandomAreaReason(ProgramSkillFrame frame, int index) => RuntimeSkills.RuntimeRules_Core_RandomAreaAcquisition.RandomAreaReason(frame, index);
    internal bool ExactRandomAreaParent(ProgramSkillFrame frame, out DamageTriggerWindowFrame window, out DamageFrame damage)
    {
        window = null !;
        damage = null !;
        if (frame.InstructionIndex != 1 || frame.WindowContext is not { TargetSeat: { } target, OccurrenceIndex: >= 0 } context || target != frame.OwnerSeat || !ObtainRandomAreaFromEachOtherThenTurnOverDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(frame)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(frame, out var actualWindow, out var actualDamage, out _) || actualWindow.Candidates[actualWindow.CandidateIndex].OccurrenceIndex != context.OccurrenceIndex || context.OccurrenceIndex >= actualDamage.Amount)
            return false;
        window = actualWindow;
        damage = actualDamage;
        return true;
    }

    internal SkillProgramStepOutcome ObtainRandomAreaFromEachOtherThenTurnOver(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.RandomAreaAcquisition is not null || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || !ExactRandomAreaParent(frame, out var window, out var damage))
            throw new InvalidOperationException("Random area acquisition needs its exact positive native damage-point owner candidate.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var receipt = new ProgramRandomAreaAcquisitionReceipt(frame.Id, frame.InstructionIndex, RandomAreaSource(frame), frame.GameplayHash, window.Id, damage.Id, frame.WindowContext!.OccurrenceIndex, damage.Amount)
        {
            OtherSeats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != frame.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RandomAreaAcquisition = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomAreaAcquisitionStartedEvent(receipt));
        ResumeRandomAreaAcquisition(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1245)]
    internal bool ResumeRandomAreaAcquisition(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RandomAreaAcquisition: not null } frame || frame.Id != id)
            return false;
        while (true)
        {
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRandomAreaAcquisitionProofsOperations.Value.RequireRandomAreaAcquisition(frame);
            var receipt = frame.RandomAreaAcquisition!;
            if (receipt.Stage is RandomAreaAcquisitionStage.MovementChildren or RandomAreaAcquisitionStage.TurnoverChildren)
            {
                if (DrainRandomAreaChildren(frame))
                    return true;
                if (receipt.Stage == RandomAreaAcquisitionStage.TurnoverChildren)
                {
                    FinishRandomAreaAcquisition(frame, false);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, RandomAreaAcquisition = receipt = receipt with { Stage = RandomAreaAcquisitionStage.Visiting, NextSeatIndex = receipt.NextSeatIndex + 1 } });
            }

            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || receipt.Steps.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            {
                FinishRandomAreaAcquisition(frame, true);
                return true;
            }

            if (receipt.NextSeatIndex == receipt.OtherSeats.Count)
            {
                var before = owner.IsFaceDown;
                var prevented = _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(owner);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RandomAreaAcquisition = receipt with { Stage = RandomAreaAcquisitionStage.TurnoverChildren, TurnoverIssued = true, TurnoverPrevented = prevented, FaceDownBefore = before, FaceDownAfter = prevented ? before : !before } });
                _coreProgramLifecycleOperations.TurnOverProgramTarget(frame.Id, frame.OwnerSeat, frame.OwnerSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomAreaAcquisitionTurnoverIssuedEvent(frame.Id, before, owner.IsFaceDown, prevented));
                continue;
            }

            var target = receipt.OtherSeats[receipt.NextSeatIndex];
            var eligible = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? ObtainRandomAreaFromEachOtherThenTurnOverDescriptor.Areas.Where(z => RandomAreaCards(target, z).Count > 0).ToArray() : [];
            if (eligible.Length == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RandomAreaAcquisition = receipt with { NextSeatIndex = receipt.NextSeatIndex + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomAreaAcquisitionSeatSkippedEvent(frame.Id, receipt.NextSeatIndex, target));
                continue;
            }

            var randomBefore = _randomOperations._random.State;
            var zone = eligible[_randomOperations._random.Next(eligible.Length)];
            var materials = RandomAreaCards(target, zone).Select(c => new DamageTargetRegionMaterial(c.Id, c.Kind)).ToArray();
            var step = new RandomAreaAcquisitionStepReceipt(receipt.NextSeatIndex, target, zone, randomBefore, _randomOperations._random.State, 0, _coreCardMovementJournalMovementSequenceAccess._movementSequence, _coreCardMovementJournalMovementSequenceAccess._movementSequence)
            {
                EligibleZones = eligible,
                Materials = materials
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null), RandomAreaAcquisition = receipt with { Stage = RandomAreaAcquisitionStage.MovementChildren } });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), CardLocation.Hand(frame.OwnerSeat), RandomAreaReason(frame, step.SeatIndex), (batch, records) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var paid = step with
                {
                    MovementBatchId = batch,
                    SequenceAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
                    Movements = records
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { RandomAreaAcquisition = active.RandomAreaAcquisition!with { Steps = active.RandomAreaAcquisition.Steps.Append(paid).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomAreaAcquisitionRegionPaidEvent(id, paid));
            });
        }
    }

    internal IReadOnlyList<Card> RandomAreaCards(int seat, CardZoneKind zone) => zone switch
    {
        CardZoneKind.Hand => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]),
        CardZoneKind.Equipment => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]),
        // A pending native judgment result is not a card in this character's area.
        CardZoneKind.Judgment => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[seat]).Where(c => !_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == seat && j.CardId == c.Id)).ToArray(),
        _ => throw new InvalidOperationException("Random acquisition requires an owned HEJ area.")};
    internal bool DrainRandomAreaChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    internal void FinishRandomAreaAcquisition(ProgramSkillFrame frame, bool cancelled)
    {
        var r = frame.RandomAreaAcquisition!;
        if (frame.PendingMovementContinuation is not null || r.Stage == RandomAreaAcquisitionStage.MovementChildren)
            throw new InvalidOperationException("Random acquisition cannot retire a paid region before its children return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RandomAreaAcquisition = r with { Stage = RandomAreaAcquisitionStage.Finished, Cancelled = cancelled } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomAreaAcquisitionCompletedEvent(frame.Id, r.Steps.Count, r.Steps.Sum(s => s.ActualObtained), r.TurnoverIssued, cancelled));
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 1245)]
    internal bool ReturnRandomAreaMovement(ProgramSkillFrame frame) => frame.RandomAreaAcquisition is not null && ResumeRandomAreaAcquisition(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 1245)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1245)]
    internal bool CanContinueRandomAreaAcquisition(ProgramSkillFrame frame) => frame.RandomAreaAcquisition is { } r && (r.Steps.Count > 0 || r.TurnoverIssued) && _coreRandomAreaAcquisitionProofsOperations.Value.ValidRandomAreaAcquisition(frame);
    SkillProgramStepOutcome IRandomAreaAcquisitionHost.ObtainRandomAreaFromEachOtherThenTurnOver(ProgramSkillFrame frame) => ObtainRandomAreaFromEachOtherThenTurnOver(frame);
}
