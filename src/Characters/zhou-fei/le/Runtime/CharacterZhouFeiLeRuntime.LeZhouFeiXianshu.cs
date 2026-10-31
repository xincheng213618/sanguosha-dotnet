#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal const string LeZhouFeiXianshuGiftReason = "skill-program.le-zhou-fei.xianshu.gift";
    internal const string LeZhouFeiXianshuDrawReason = "skill-program.le-zhou-fei.xianshu.draw";
    internal static CardConversionSource LeZhouFeiXianshuSource(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_fei_le_LeZhouFeiXianshu.LeZhouFeiXianshuSource(frame);
    SkillProgramStepOutcome global::CardGame.Core.ILeZhouFeiXianshuHost.Resolve(ProgramSkillFrame frame) => BeginLeZhouFeiXianshu(frame);
    bool global::CardGame.Core.ILeZhouFeiXianshuSelectionQuery.CanGive(int ownerSeat, Card card) => HasLingkongHandEntityMark(ownerSeat, card.Id);
    internal ActualDiscardRecoveryPhaseKey? LeZhouFeiXianshuPlay(int ownerSeat)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == ownerSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.ActorSeat == ownerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28320, SkillProgramEffectOp.ResolveXianshuGiftDrawAndHealth)]
    internal bool CanActivateLeZhouFeiXianshu(ProgramActivationLegalityContext gate) => LeZhouFeiXianshuPlay(gate.OwnerSeat)is not null && _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Any(card => HasLingkongHandEntityMark(gate.OwnerSeat, card.Id)) && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != gate.OwnerSeat);
    internal SkillProgramStepOutcome BeginLeZhouFeiXianshu(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.LeZhouFeiXianshu is not null)
        {
            ContinueLeZhouFeiXianshu(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.SkillId != "le:xianshu" || frame.ActivationId != "marked-hand-gift" || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds is not [var cardId] || frame.SelectedTargetSeats is not [var targetSeat] || LeZhouFeiXianshuPlay(frame.OwnerSeat)is not { } phase || targetSeat == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || !HasLingkongHandEntityMark(frame.OwnerSeat, cardId))
            throw new InvalidOperationException("Xianshu needs its current owner's exact marked Hand gift and other living recipient.");
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(c => c.Id == cardId);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var receipt = new LeZhouFeiXianshuReceipt(LeZhouFeiXianshuSource(frame), frame.GameplayHash, phase, frame.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, card.Id, card.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card), targetSeat, before)
        {
            Stage = LeZhouFeiXianshuStage.GiftChildren,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LeZhouFeiXianshu = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuGiftStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, phase, receipt.ActualTurn, receipt.TurnOwner, card.Id, card.Kind, receipt.GiftSuit, targetSeat, before));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(targetSeat), new(LeZhouFeiXianshuGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, targetSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batchId, records) =>
        {
            var paid = records.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { LeZhouFeiXianshu = current.LeZhouFeiXianshu!with { GiftBatchId = batchId, GiftSequence = paid.Sequence, After = paid.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuGiftPaidEvent(frame.Id, batchId, paid.CardId, paid.From, paid.To, paid.Sequence));
        }));
        ContinueLeZhouFeiXianshu(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainLeZhouFeiXianshu(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, frame.LeZhouFeiXianshu!.HealthAction == XianshuHealthAction.LoseHp ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id);
    internal void ContinueLeZhouFeiXianshu(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeZhouFeiXianshu: { } receipt } frame || frame.Id != frameId || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (frame.NativeDraw is not null || receipt.Stage == LeZhouFeiXianshuStage.Drawing)
            return;
        if (receipt.Stage == LeZhouFeiXianshuStage.HealthIssuing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                CompleteLeZhouFeiXianshu(frame);
                return;
            }

            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, receipt.TargetSeat, 1);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frameId)
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            receipt = frame.LeZhouFeiXianshu!;
            if (receipt.Stage == LeZhouFeiXianshuStage.HealthIssuing)
                throw new InvalidOperationException("Xianshu's native HP loss returned without applying or preventing its one deduction.");
        }

        if (DrainLeZhouFeiXianshu(frame))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        receipt = frame.LeZhouFeiXianshu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            CompleteLeZhouFeiXianshu(frame);
            return;
        }

        if (receipt.Stage == LeZhouFeiXianshuStage.GiftChildren)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat];
            var count = owner.HasHpValue && target.HasHpValue ? (int)Math.Min(5L, Math.Abs((long)owner.Hp - target.Hp)) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LeZhouFeiXianshu = receipt with { Stage = LeZhouFeiXianshuStage.Drawing, RequestedDraw = count, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuDrawRequestedEvent(frameId, owner.HasHpValue, owner.HasHpValue ? owner.Hp : 0, target.HasHpValue, target.HasHpValue ? target.Hp : 0, count));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, count, new(LeZhouFeiXianshuDrawReason));
            return;
        }

        if (receipt.Stage == LeZhouFeiXianshuStage.DrawChildren)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat];
            var action = XianshuHealthAction.None;
            if (target.IsAlive && owner.HasHpValue && target.HasHpValue)
            {
                if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(receipt.GiftSuit) && target.Hp <= owner.Hp)
                    action = XianshuHealthAction.Recover;
                else if ((receipt.GiftSuit is Suit.Spade or Suit.Club) && target.Hp >= owner.Hp)
                    action = XianshuHealthAction.LoseHp;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LeZhouFeiXianshu = receipt with { HealthAction = action, Stage = action == XianshuHealthAction.LoseHp ? LeZhouFeiXianshuStage.HealthIssuing : LeZhouFeiXianshuStage.HealthChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(receipt.TargetSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuHealthEvaluatedEvent(frameId, receipt.GiftSuit, owner.HasHpValue, owner.HasHpValue ? owner.Hp : 0, target.HasHpValue, target.HasHpValue ? target.Hp : 0, target.IsAlive, action));
            if (action == XianshuHealthAction.Recover)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frameId, frame.OwnerSeat, receipt.TargetSeat, 1, null, null);
            ContinueLeZhouFeiXianshu(frameId);
            return;
        }

        if (receipt.Stage == LeZhouFeiXianshuStage.HealthChildren)
        {
            CompleteLeZhouFeiXianshu(frame);
            return;
        }

        throw new InvalidOperationException("Xianshu resumed outside its exact paid native stage.");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveXianshuGiftDrawAndHealth)]
    internal void ReturnLeZhouFeiXianshuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var receipt = frame.LeZhouFeiXianshu!;
        if (receipt.Stage != LeZhouFeiXianshuStage.Drawing || frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != receipt.RequestedDraw || outcome.SequenceBefore < receipt.Before || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.DrawOutcome is not null)
            throw new InvalidOperationException("Xianshu's native draw returned to another recipient, amount or already-consumed stage.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LeZhouFeiXianshu = receipt with { Stage = LeZhouFeiXianshuStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuDrawReturnedEvent(frame.Id, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueLeZhouFeiXianshu(frame.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 28320)]
    internal void ObserveLeZhouFeiXianshuHealth(IGameEvent fact)
    {
        if (fact is ProgramSkillHpLostEvent lost)
            CaptureLeZhouFeiXianshuHpLoss(lost);
        else if (fact is IdentityHpDeductionPreventedEvent prevented)
            CaptureLeZhouFeiXianshuPreventedHpLoss(prevented);
    }

    internal void CaptureLeZhouFeiXianshuHpLoss(ProgramSkillHpLostEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeZhouFeiXianshu: { Stage: LeZhouFeiXianshuStage.HealthIssuing, HealthAction: XianshuHealthAction.LoseHp } receipt } frame || fact.FrameId != frame.Id || fact.SkillId != frame.SkillId || fact.TargetSeat != receipt.TargetSeat || receipt.HpLossCaptured)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { LeZhouFeiXianshu = receipt with { Stage = LeZhouFeiXianshuStage.HealthChildren, HpLossCaptured = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuHpLossAppliedEvent(frame.Id, fact.TargetSeat, fact.Amount, fact.RemainingHp, false));
    }

    internal void CaptureLeZhouFeiXianshuPreventedHpLoss(IdentityHpDeductionPreventedEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not IdentityAppearanceFrame appearance || appearance.Id != fact.AppearanceFrameId || appearance.Receipt is not { Entry: IdentityAppearanceEntry.ProgramHpLoss, ParentFrameId: { } parent } native || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parent)is not ProgramSkillFrame { LeZhouFeiXianshu: { Stage: LeZhouFeiXianshuStage.HealthIssuing, HealthAction: XianshuHealthAction.LoseHp } receipt } frame || native.OwnerSeat != receipt.TargetSeat || native.OriginalInstructionIndex != frame.InstructionIndex || fact.RequestedDeduction != 1 || receipt.HpLossCaptured)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { LeZhouFeiXianshu = receipt with { Stage = LeZhouFeiXianshuStage.HealthChildren, HpLossCaptured = true, AppearancePrevented = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuHpLossAppliedEvent(frame.Id, receipt.TargetSeat, 0, _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].Hp, true));
    }

    internal void CompleteLeZhouFeiXianshu(ProgramSkillFrame frame)
    {
        var receipt = frame.LeZhouFeiXianshu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianshuCompletedEvent(frame.Id, receipt.CardId, receipt.TargetSeat, receipt.HealthAction ?? XianshuHealthAction.None));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28320)]
    internal bool ResumeLeZhouFeiXianshu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LeZhouFeiXianshu: not null } frame || frame.Id != id)
            return false;
        if (frame.NativeDraw is null)
            ContinueLeZhouFeiXianshu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28320)]
    internal bool ReturnLeZhouFeiXianshuMovement(ProgramSkillFrame frame)
    {
        if (frame.LeZhouFeiXianshu is null)
            return false;
        ContinueLeZhouFeiXianshu(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28320)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28320)]
    internal bool ContinuePaidLeZhouFeiXianshu(ProgramSkillFrame frame) => HasPaidLeZhouFeiXianshuSelection(frame);
}
