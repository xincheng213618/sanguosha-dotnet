#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSequentialDeckKindGrowthRuntime
{
    internal static CardConversionSource SequentialDeckKindGrowthSource(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SequentialDeckKindGrowth.SequentialDeckKindGrowthSource(frame);
    internal static string SequentialDeckKindGrowthReason(long frameId, int step) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SequentialDeckKindGrowth.SequentialDeckKindGrowthReason(frameId, step);
    internal bool HasSequentialDeckKindGrowthParent(ProgramSkillFrame frame)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && frame.TriggerId is not null && frame.InstructionIndex == 1 && frame.SelectedCardIds.Count == 0 && frame.SelectedTargetSeats.Count == 0 && frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context && context.OwnerSeat == frame.OwnerSeat && context.SourceSeat == frame.OwnerSeat && context.TargetSeat == frame.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Id == context.ParentFrameId && parent.OwnerSeat == frame.OwnerSeat && parent.Window == context.Window && parent.Continuation == ProgramLifecycleContinuation.NormalTurnStart && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) && AcquireDeckKindSequentiallyAndGrowOnShortfallDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(frame));
    }

    internal SkillProgramStepOutcome AcquireDeckKindSequentiallyAndGrowOnShortfall(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.SequentialDeckKindGrowth is not null || !HasSequentialDeckKindGrowthParent(frame) || effect.Op != SkillProgramEffectOp.AcquireDeckKindSequentiallyAndGrowOnShortfall || effect.OutputKind != CardKind.Duel || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Sequential deck-kind acquisition requires its exact own preparation candidate.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !owner.HasHpValue || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var receipt = new ProgramSequentialDeckKindGrowthReceipt(frame.Id, frame.InstructionIndex, SequentialDeckKindGrowthSource(frame), frame.GameplayHash, frame.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, effect.OutputKind.Value, _characterTaishiCiMouOlOperations.SequentialDeckKindGrowthRequestedCount(effect, owner));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SequentialDeckKindGrowth = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SequentialDeckKindGrowthStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.PreparationWindowId, receipt.TurnNumber, receipt.OutputKind, receipt.RequestedCount));
        ResumeSequentialDeckKindGrowth(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1244)]
    internal bool ResumeSequentialDeckKindGrowth(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SequentialDeckKindGrowth: not null } frame || frame.Id != frameId)
            return false;
        while (true)
        {
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            var receipt = frame.SequentialDeckKindGrowth!;
            if (receipt.Stage == SequentialDeckKindGrowthStage.GainChildren)
            {
                if (DrainSequentialDeckKindGrowthChildren(frame))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, SequentialDeckKindGrowth = receipt = receipt with { Stage = SequentialDeckKindGrowthStage.Ready } });
            }

            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            {
                FinishSequentialDeckKindGrowth(frame, cancelled: true);
                return true;
            }

            if (receipt.Steps.Count == receipt.RequestedCount)
            {
                FinishSequentialDeckKindGrowth(frame, cancelled: false);
                return true;
            }

            var failed = receipt.Steps.Where(step => step.ActualObtained == 0).Select(step => step.CardId).ToHashSet();
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(item => item.Kind == receipt.OutputKind && !failed.Contains(item.Id));
            if (card is null)
            {
                FinishSequentialDeckKindGrowth(frame, cancelled: false);
                return true;
            }

            var before = (long)_coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var stepIndex = receipt.Steps.Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null), SequentialDeckKindGrowth = receipt with { Stage = SequentialDeckKindGrowthStage.GainChildren } });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(frame.OwnerSeat), new(SequentialDeckKindGrowthReason(frame.Id, stepIndex)), (batch, movements) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
                var step = new SequentialDeckKindGrowthStepReceipt(stepIndex, card.Id, batch, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, movements.Count(move => move.CardId == card.Id && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(frame.OwnerSeat)))
                {
                    Movements = movements
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { SequentialDeckKindGrowth = active.SequentialDeckKindGrowth!with { Steps = active.SequentialDeckKindGrowth.Steps.Append(step).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SequentialDeckKindGrowthStepPaidEvent(frameId, step));
            });
        // Drain this exact batch before observing the pile again. In the no-child
        // case iterate here rather than recursively re-entering the instruction.
        }
    }

    internal bool DrainSequentialDeckKindGrowthChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    internal void FinishSequentialDeckKindGrowth(ProgramSkillFrame frame, bool cancelled)
    {
        var receipt = frame.SequentialDeckKindGrowth!;
        if (receipt.Stage != SequentialDeckKindGrowthStage.Ready || frame.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Sequential deck-kind growth cannot finish before its issued gain children return.");
        var shortfall = receipt.RequestedCount - receipt.ActualObtained;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SequentialDeckKindGrowth = receipt with { Stage = SequentialDeckKindGrowthStage.Finished, Shortfall = shortfall, Cancelled = cancelled } });
        if (!cancelled)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SequentialDeckKindGrowthGrantedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.PreparationWindowId, receipt.TurnNumber, receipt.OutputKind, receipt.RequestedCount, receipt.ActualObtained, _characterTaishiCiMouOlOperations.SequentialDeckKindGrowthGrantAmount(receipt.Source, frame.Id, shortfall)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SequentialDeckKindGrowthCompletedEvent(frame.Id, receipt.Steps.Count, receipt.ActualObtained, shortfall, cancelled));
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 1244)]
    internal bool ReturnSequentialDeckKindGrowthMovement(ProgramSkillFrame frame) => frame.SequentialDeckKindGrowth is not null && ResumeSequentialDeckKindGrowth(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 1244)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1244)]
    internal bool CanContinueSequentialDeckKindGrowth(ProgramSkillFrame frame) => frame.SequentialDeckKindGrowth is { Steps.Count: > 0 } && ValidSequentialDeckKindGrowth(frame);
    internal static bool SameSequentialDeckKindGrowthStep(SequentialDeckKindGrowthStepReceipt left, SequentialDeckKindGrowthStepReceipt right) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SequentialDeckKindGrowth.SameSequentialDeckKindGrowthStep(left, right);
    internal bool ValidSequentialDeckKindGrowthStep(long frameId, int owner, CardKind kind, SequentialDeckKindGrowthStepReceipt step)
    {
        if (step.Index < 0 || step.BatchId <= 0 || step.SequenceBefore < 0 || step.SequenceAfter < step.SequenceBefore || step.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || step.ActualObtained is < 0 or > 1 || _coreAdvancedLifecycleOperations.GetAdvancedCard(step.CardId).Kind != kind || !step.Movements.SequenceEqual(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > step.SequenceBefore && move.Sequence <= step.SequenceAfter)) || step.Movements.Any(move => move.CardId != step.CardId || move.CardKind != kind || move.From != CardLocation.DrawPile || move.Reason.Value != SequentialDeckKindGrowthReason(frameId, step.Index)) || step.Movements.Count(move => move.To == CardLocation.Hand(owner)) != step.ActualObtained)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthStepPaidEvent>().Where(item => item.ProgramFrameId == frameId && item.Step.Index == step.Index).ToArray()is [var issued] && SameSequentialDeckKindGrowthStep(step, issued.Step);
    }

    internal bool ValidSequentialDeckKindGrowth(ProgramSkillFrame frame)
    {
        if (frame.SequentialDeckKindGrowth is not { } receipt || !HasSequentialDeckKindGrowthParent(frame) || receipt.ProgramFrameId != frame.Id || receipt.InstructionIndex != frame.InstructionIndex || receipt.Source != SequentialDeckKindGrowthSource(frame) || receipt.GameplayHash != frame.GameplayHash || receipt.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash || receipt.PreparationWindowId != frame.WindowContext!.ParentFrameId || receipt.TurnNumber < 1 || receipt.OutputKind != CardKind.Duel || receipt.RequestedCount < 0 || !Enum.IsDefined(receipt.Stage) || receipt.Steps.Count > receipt.RequestedCount || !receipt.Steps.Select(step => step.Index).SequenceEqual(Enumerable.Range(0, receipt.Steps.Count)) || receipt.Steps.Any(step => !ValidSequentialDeckKindGrowthStep(frame.Id, frame.OwnerSeat, receipt.OutputKind, step)))
            return false;
        for (var index = 1; index < receipt.Steps.Count; index++)
            if (receipt.Steps[index].SequenceBefore < receipt.Steps[index - 1].SequenceAfter || receipt.Steps.Take(index).Any(step => step.CardId == receipt.Steps[index].CardId && step.ActualObtained == 0))
                return false;
        var starts = _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthStartedEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        if (starts is not [var start] || start != new SequentialDeckKindGrowthStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.PreparationWindowId, receipt.TurnNumber, receipt.OutputKind, receipt.RequestedCount) || _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthStepPaidEvent>().Count(item => item.ProgramFrameId == frame.Id) != receipt.Steps.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat && item.SkillId == frame.SkillId && item.BindingId == frame.TriggerId && item.SkillInstanceId == frame.SkillInstanceId && item.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) != 1)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var startIndex = Array.FindIndex(history, item => ReferenceEquals(item, start));
        var turn = history.Take(startIndex).OfType<TurnStartedEvent>().LastOrDefault();
        if (startIndex < 0 || turn is null || turn.TurnNumber != receipt.TurnNumber || turn.ActorSeat != frame.OwnerSeat || history.Take(startIndex).OfType<TurnEndedEvent>().Any(item => item.TurnNumber == turn.TurnNumber && item.ActorSeat == turn.ActorSeat))
            return false;
        var growth = history.OfType<SequentialDeckKindGrowthGrantedEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        var completed = history.OfType<SequentialDeckKindGrowthCompletedEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        if (receipt.Stage != SequentialDeckKindGrowthStage.Finished)
            return receipt.Shortfall is null && !receipt.Cancelled && growth.Length == 0 && completed.Length == 0 && (receipt.Stage == SequentialDeckKindGrowthStage.Ready ? frame.PendingMovementContinuation is null : receipt.Steps.Count > 0 && frame.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == frame.OwnerSeat);
        return frame.PendingMovementContinuation is null && receipt.Shortfall == receipt.RequestedCount - receipt.ActualObtained && (receipt.Cancelled ? growth.Length == 0 : growth is [var granted] && ValidSequentialDeckKindGrowthGrant(granted)) && completed is [var finished] && finished == new SequentialDeckKindGrowthCompletedEvent(frame.Id, receipt.Steps.Count, receipt.ActualObtained, receipt.Shortfall.Value, receipt.Cancelled);
    }

    internal bool ValidSequentialDeckKindGrowthGrant(SequentialDeckKindGrowthGrantedEvent growth)
    {
        if (growth.ProgramFrameId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(growth.Source.OwnerSeat) || growth.Amount < 0 || growth.RequestedCount < 0 || growth.ActualObtained < 0 || growth.ActualObtained > growth.RequestedCount || growth.TurnNumber < 1 || growth.PreparationWindowId <= 0 || growth.OutputKind != CardKind.Duel || _contentRegistryOperations._contentRegistry.GetSkill(growth.Source.SkillId).Program is not { } program || program.GameplayHash != growth.GameplayHash || program.Triggers.SingleOrDefault(item => item.Id == growth.Source.BindingId)is not { } trigger || !AcquireDeckKindSequentiallyAndGrowOnShortfallDescriptor.IsTrigger(trigger) || (trigger.Effects[0].NumberExpression is null && growth.RequestedCount != trigger.Effects[0].Amount) || growth.Amount != _characterTaishiCiMouOlOperations.SequentialDeckKindGrowthGrantAmount(growth.Source, growth.ProgramFrameId, growth.RequestedCount - growth.ActualObtained))
            return false;
        var starts = _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthStartedEvent>().Where(item => item.ProgramFrameId == growth.ProgramFrameId).ToArray();
        var steps = _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthStepPaidEvent>().Where(item => item.ProgramFrameId == growth.ProgramFrameId).ToArray();
        return starts is [var start] && start == new SequentialDeckKindGrowthStartedEvent(growth.ProgramFrameId, growth.Source, growth.GameplayHash, growth.PreparationWindowId, growth.TurnNumber, growth.OutputKind, growth.RequestedCount) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(item => item.FrameId == growth.ProgramFrameId && item.OwnerSeat == growth.Source.OwnerSeat && item.SkillId == growth.Source.SkillId && item.BindingId == growth.Source.BindingId && item.SkillInstanceId == growth.Source.SkillInstanceId && item.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) == 1 && steps.Length <= growth.RequestedCount && steps.Select(item => item.Step.Index).SequenceEqual(Enumerable.Range(0, steps.Length)) && steps.Select(item => item.Step.BatchId).Distinct().Count() == steps.Length && steps.Sum(item => item.Step.ActualObtained) == growth.ActualObtained && steps.All(item => ValidSequentialDeckKindGrowthStep(growth.ProgramFrameId, growth.Source.OwnerSeat, growth.OutputKind, item.Step));
    }

    internal int SequentialDeckKindGrowthBonus(int owner)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AcquireDeckKindSequentiallyAndGrowOnShortfall))
            return 0;
        var events = _coreProgramEventHistoryOperations.ProgramEventHistory<SequentialDeckKindGrowthGrantedEvent>().Where(item => item.Source.OwnerSeat == owner).ToArray();
        if (events.Select(item => item.ProgramFrameId).Distinct().Count() != events.Length || events.Any(item => !ValidSequentialDeckKindGrowthGrant(item)))
            throw new InvalidOperationException("Permanent deck-shortfall growth lost its once-issued acquisition ledger.");
        return events.Aggregate(0, (sum, item) => checked(sum + item.Amount));
    }

    SkillProgramStepOutcome global::CardGame.Core.ISequentialDeckKindGrowthHost.AcquireDeckKindSequentiallyAndGrowOnShortfall(ProgramSkillFrame frame, SkillProgramEffect effect) => AcquireDeckKindSequentiallyAndGrowOnShortfall(frame, effect);
    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeContributions, 10)]
    internal void AddSequentialDeckKindGrowthBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.AddFiniteContribution(contributions, $"issued:{(player.Seat)}:sequential-deck-shortfall:attack-range", SequentialDeckKindGrowthBonus(player.Seat));
}
