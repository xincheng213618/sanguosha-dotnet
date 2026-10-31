#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuLeRuntime
{
    internal const string PingluObtainReason = "skill-program.le-zhou-yu.pinglu.obtain";
    internal bool HasLeZhouYuPingluCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolvePingluInRangeRandomHands);

    internal static CardConversionSource PingluSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_zhou_yu_le_LeZhouYuPinglu.PingluSource(frame);
    SkillProgramStepOutcome ILeZhouYuPingluHost.Resolve(ProgramSkillFrame frame) => BeginPinglu(frame);
    internal ActualDiscardRecoveryPhaseKey? PingluPlay(int actor)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.ActorSeat == actor && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    }

    internal int[] PingluTargets(int actor) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor && _coreNativeRuleQueriesOperations.GetCombatDistance(actor, player.Seat) <= _coreNativeRuleQueriesOperations.GetAttackRange(actor)).OrderBy(player => _coreTableOrderOperations.RelativeTableOrder(actor, player.Seat)).Select(player => player.Seat).ToArray();
    internal bool HasPingluCustody(int owner, ActualDiscardRecoveryPhaseKey phase)
    {
        var starts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PingluStartedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Phase == phase).Select(e => e.FrameId).ToHashSet();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PingluEntityObtainedEvent>().Any(e => starts.Contains(e.FrameId) && e.To == CardLocation.Hand(owner) && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Hand(owner) && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == e.CardId && m.Sequence > e.Sequence && m.From == CardLocation.Hand(owner) && m.To != CardLocation.Hand(owner)));
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28820, SkillProgramEffectOp.ResolvePingluInRangeRandomHands)]
    internal bool CanActivatePinglu(ProgramActivationLegalityContext gate) => PingluPlay(gate.OwnerSeat)is { } phase && !HasPingluCustody(gate.OwnerSeat, phase) && PingluTargets(gate.OwnerSeat).Any(seat => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0);
    internal SkillProgramStepOutcome BeginPinglu(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.Pinglu is not null)
        {
            ContinuePinglu(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.SkillId != "le:pinglu" || frame.ActivationId != "all-in-range-random-hand" || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || PingluPlay(frame.OwnerSeat)is not { } phase || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || HasPingluCustody(frame.OwnerSeat, phase))
            throw new InvalidOperationException("Pinglu requires its real current Play, eligible source and released original Hand cohort.");
        var targets = PingluTargets(frame.OwnerSeat);
        if (!targets.Any(seat => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count > 0))
            throw new InvalidOperationException("Pinglu cannot issue an empty random obtain.");
        var samples = new List<PingluTargetSample>(targets.Length);
        foreach (var target in targets)
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]);
            var beforeRandom = _randomOperations._random.State;
            var card = hand.Count > 0 ? hand[_randomOperations._random.Next(hand.Count)] : null;
            samples.Add(new(samples.Count, target, hand.Count, card?.Id, card?.Kind, card is null ? null : CardLocation.Hand(target), beforeRandom, _randomOperations._random.State));
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var receipt = new PingluReceipt(PingluSource(frame), frame.GameplayHash, phase, frame.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, before)
        {
            Stage = PingluStage.ObtainChildren,
            Targets = samples,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Pinglu = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PingluStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, phase, receipt.ActualTurn, receipt.TurnOwner, targets.Length, before));
        foreach (var sample in receipt.Targets)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PingluTargetSampledEvent(frame.Id, sample.Index, sample.TargetSeat, sample.HandCount, sample.CardId, sample.PrintedKind, sample.From, sample.RandomBefore, sample.RandomAfter, before));
        var selected = receipt.Targets.Where(s => s.CardId is not null).ToArray();
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selected.Select(s => s.CardId!.Value).ToArray(), CardLocation.Hand(frame.OwnerSeat), new(PingluObtainReason), (batchId, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { Pinglu = current.Pinglu!with { BatchId = batchId, After = after, ActualCount = moves.Count(m => m.To == CardLocation.Hand(frame.OwnerSeat)) } });
            for (var index = 0; index < moves.Count; index++)
            {
                var move = moves[index];
                var sample = selected.Single(s => s.CardId == move.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PingluEntityObtainedEvent(frame.Id, index, sample.TargetSeat, move.CardId, move.CardKind, move.From, move.To, move.Sequence, batchId));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PingluObtainPaidEvent(frame.Id, batchId, moves.Count, moves.Count(m => m.To == CardLocation.Hand(frame.OwnerSeat)), before, after));
        }, disclosure: CardMovementDisclosure.FaceDown);
        ContinuePinglu(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinuePinglu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Pinglu: { } receipt } frame || frame.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PingluCompletedEvent(id, receipt.ActualCount));
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28820)]
    internal bool ResumePinglu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Pinglu: not null } frame || frame.Id != id)
            return false;
        ContinuePinglu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28820)]
    internal bool ReturnPingluMovement(ProgramSkillFrame frame)
    {
        if (frame.Pinglu is null)
            return false;
        ContinuePinglu(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28820)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28820)]
    internal bool CanContinueIssuedPinglu(ProgramSkillFrame frame) => ValidPingluStarted(frame) && frame.Pinglu is { ActualCount: >= 0 };
}
