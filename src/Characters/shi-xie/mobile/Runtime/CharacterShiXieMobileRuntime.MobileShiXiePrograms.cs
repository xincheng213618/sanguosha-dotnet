#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterShiXieMobileRuntime
{
    SkillProgramStepOutcome IMobileShiXieHost.Change(ProgramSkillFrame frame, SkillProgramEffectOp op) => ChangeMobileShiXieDistance(frame, op);
    internal bool MobileShiXieEndingParent(ProgramSkillFrame f) => f.SkillId == MobileShiXieComposition.Lixia && f.TriggerId == "one-other-ending-benefit" && f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && c.OwnerSeat == f.OwnerSeat && c.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && c.TargetSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } parent && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && c.OccurrenceIndex == candidate.OccurrenceIndex;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 78900)]
    internal bool CanOfferMobileShiXie(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (!t.Effects.Any(e => MobileShiXieComposition.Handles(e.Op)))
            return true;
        if (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat].IsAlive || c.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || c.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat)
            return false;
        if (candidate.SkillId == MobileShiXieComposition.Biluan)
            return c.Window == SkillProgramTriggerWindow.DrawPhaseStarting && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat && _coreNativeRuleQueriesOperations.GetCombatDistance(p.Seat, candidate.OwnerSeat) == 1);
        return candidate.SkillId == MobileShiXieComposition.Lixia && c.Window == SkillProgramTriggerWindow.TurnEnding && candidate.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, candidate.OwnerSeat);
    }

    internal SkillProgramStepOutcome ChangeMobileShiXieDistance(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Mobile Shi Xie cannot reissue another instruction's permanent distance change.");
        var increase = op == SkillProgramEffectOp.IncreaseMobileShiXieIncomingDistanceByLivingFactions;
        if (increase ? f.SkillId != MobileShiXieComposition.Biluan || f.TriggerId != "skip-normal-draw-for-distance" || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreNativeRuleQueriesOperations.GetCombatDistance(p.Seat, f.OwnerSeat) == 1) : !MobileShiXieCourtesySelected(f) || f.NativeDraw is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Shi Xie lost its native Draw replacement or already-issued exact Ending benefit.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        if (!increase)
            return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, MobileShiXieCourtesyRecipient(f), 1, new("skill-program.mobile:lixia.Draw"));
        IssueMobileShiXieDistance(f, _coreRuleQueriesOperations.GetLivingFactionCount());
        return SkillProgramStepOutcome.Continue;
    }

    internal void IssueMobileShiXieDistance(ProgramSkillFrame f, int amount)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Shi Xie cannot settle the original distance instruction twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiXieIncomingDistanceChangedEvent(f.Id, f.InstructionIndex, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, amount));
    }

    [ProgramDistanceRule(ProgramDistanceStage.IncomingContributions, 78900)]
    internal IEnumerable<RuleQueryContribution> MobileShiXieIncomingDistance(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Where(e => e.Source.OwnerSeat == target).Select(e => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-shi-xie:{(e.ProgramFrameId)}:{(e.InstructionIndex)}", SkillRuleOperation.Add, e.Amount));
    internal bool MobileShiXieCourtesySelected(ProgramSkillFrame f)
    {
        if (!MobileShiXieEndingParent(f) || f.ChoiceBindings.SingleOrDefault(b => b.Name == MobileShiXieComposition.Benefit)is not { ChooserSeat: var chooser, OptionId: "self" or "current" } choice || chooser != f.OwnerSeat)
            return false;
        return f.InstructionIndex == 2;
    }

    internal int MobileShiXieCourtesyRecipient(ProgramSkillFrame f) => f.ChoiceBindings.Single(b => b.Name == MobileShiXieComposition.Benefit).OptionId == "self" ? f.OwnerSeat : _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat;
    [ProgramNativeDrawReturn(SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance)]
    internal void ReturnMobileShiXieCourtesyDraw(ProgramSkillFrame supplied, NativeDrawOutcome drawn)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MobileShiXieCourtesySelected(f) || f.NativeDraw is not null || drawn.RecipientSeat != MobileShiXieCourtesyRecipient(f) || drawn.RequestedCount != 1 || drawn.ActualDrawIds.Count > 1 || drawn.SequenceBefore > drawn.SequenceAfter || drawn.SequenceAfter > _coreCardMovementJournalMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Lixia lost the original chosen one-card native draw return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiXieCourtesyDrawIssuedEvent(f.Id, f.InstructionIndex, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), f.GameplayHash, drawn.RecipientSeat, drawn.ActualDrawIds.Count, drawn.SequenceAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 78900)]
    internal bool SettleMobileShiXieCourtesy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.NativeDraw is not null || !MobileShiXieCourtesySelected(f) || !MobileShiXieCourtesyIssued(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieIncomingDistanceChangedEvent>().Any(e => e.ProgramFrameId == f.Id))
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return false;
        IssueMobileShiXieDistance(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), -1);
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    internal bool MobileShiXieCourtesyIssued(ProgramSkillFrame f)
    {
        if (!MobileShiXieCourtesySelected(f))
            return false;
        var recipient = MobileShiXieCourtesyRecipient(f);
        return f.NativeDraw is { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, RequestedCount: 1, Operation: SkillProgramEffectOp.DecreaseMobileShiXieIncomingDistance, InstructionIndex: 2 } native && native.Recipients.SequenceEqual([recipient]) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiXieCourtesyDrawIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == 2 && e.Source == CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f) && e.GameplayHash == f.GameplayHash && e.RecipientSeat == recipient && e.ActualCount is >= 0 and <= 1 && e.MovementSequence <= _coreCardMovementJournalMovementSequenceAccess._movementSequence);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 78900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 78900)]
    internal bool KeepMobileShiXieIssuedCourtesy(ProgramSkillFrame f) => MobileShiXieCourtesyIssued(f);
}
