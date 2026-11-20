#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuZhaoAnniversaryRuntime
{
    internal bool ValidHuZhaoMidu(ProgramSkillFrame f)
    {
        if (f.HuZhaoMidu is not { } r || f.SkillId != HuZhaoComposition.Midu || f.TriggerId is not null || f.ActivationId != "abolish-or-restore" || f.InstructionIndex is < 1 or > 2 || r.Stage != HuZhaoMiduStage.Completed && f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != HuZhaoSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.TurnOwner != f.OwnerSeat || r.AvailableMask is < 0 or > 63 || r.SelectedMask is < 0 or > 63 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuZhaoMiduStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new HuZhaoMiduStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.TurnOwner, r.AvailableMask))
            return false;
        var payments = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuZhaoMiduPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var completed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuZhaoMiduCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var draw = CompletedHuZhaoDraw(f);
        if (!r.Paid)
            return r.Stage is HuZhaoMiduStage.Areas or HuZhaoMiduStage.Recipient && !r.Restore && r.Recipient == -1 && draw is null && payments.Length == 0 && completed.Length == 0 && r.Before == 0 && r.After == 0 && (r.Stage != HuZhaoMiduStage.Areas || r.SelectedMask == 0) && (r.SelectedMask & r.AvailableMask) == r.SelectedMask;
        if (payments is not [var paid] || paid.Source != r.Source || paid.GameplayHash != r.GameplayHash || paid.SelectedMask != r.SelectedMask || paid.Restore != r.Restore || paid.Recipient != r.Recipient || paid.Before != r.Before || paid.After != paid.Before || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || r.Stage is HuZhaoMiduStage.Areas or HuZhaoMiduStage.Recipient || (r.Restore ? !IsSingleHuZhaoArea(r.SelectedMask) || (r.SelectedMask & r.AvailableMask) != 0 || r.Recipient != f.OwnerSeat || draw is not null : (r.SelectedMask & r.AvailableMask) != r.SelectedMask))
            return false;
        if (draw is not null && (draw.SequenceBefore < r.After ||
            draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
            return false;
        if (r.Stage == HuZhaoMiduStage.Completed)
        {
            var (recipient, requestedDraw) = HuZhaoDrawRequest(f, r);
            return completed is [var done] && done == new HuZhaoMiduCompletedEvent(f.Id, r.Restore, recipient, requestedDraw) && f.ChoiceBindings.Count(b => b.Name == "midu-branch" && b.ChooserSeat == f.OwnerSeat && b.OptionId == (r.Restore ? "restore" : "abolish")) == 1;
        }
        if (completed.Length != 0 || r.Restore && r.Stage != HuZhaoMiduStage.CostChildren)
            return false;
        if (r.Stage == HuZhaoMiduStage.Drawing)
            return !r.Restore && (draw is { RequestedCount: > 0 } ? f.NativeDraw is null :
                NativeDrawProducerRuntime.PendingOperationDraw(f, SkillProgramEffectOp.ResolveAnniversaryHuZhaoMidu, HuZhaoDrawReason) is { RequestedCount: > 0 });
        return r.Stage == HuZhaoMiduStage.CostChildren ? draw is null : r.Stage == HuZhaoMiduStage.DrawChildren && draw is { RequestedCount: > 0 };
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 43000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 43000)]
    internal bool HuZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.HuZhaoMidu is { Stage: HuZhaoMiduStage.CostChildren, Restore: false } r && ValidHuZhaoMidu(f) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason("equipment.slot-abolished") && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(f.OwnerSeat) && m.Reason.Value == "equipment.slot-abolished" && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion);
    internal bool HuZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HuZhaoMidu is not { } r || !ValidHuZhaoMidu(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == HuZhaoMiduStage.Drawing && NativeDrawProducerRuntime.MatchesOperationAttempt(f, draw, SkillProgramEffectOp.ResolveAnniversaryHuZhaoMidu, HuZhaoDrawReason);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            var outcome = CompletedHuZhaoDraw(f);
            var (before, after) = outcome is not null && r.Stage == HuZhaoMiduStage.DrawChildren
                ? (outcome.SequenceBefore, outcome.SequenceAfter)
                : (r.Before, r.After);
            return f.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) &&
                moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) &&
                    m.Sequence > before && m.Sequence <= after && (m.Reason == CardMoveReasons.Reshuffle ||
                    r.Stage == HuZhaoMiduStage.DrawChildren && m.Reason == HuZhaoDrawReason && m.From == CardLocation.DrawPile ||
                    r.Stage == HuZhaoMiduStage.CostChildren && (m.Reason.Value == "equipment.slot-abolished" && m.From == CardLocation.Equipment(f.OwnerSeat) ||
                        m.Reason.Value == HuZhaoJudgmentReason && m.From == CardLocation.Judgment(f.OwnerSeat))));
        }
        if (child is RecoveryReplacementFrame recovery)
            return HuZhaoRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == HuZhaoMiduStage.CostChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        return child is ProgramLifecycleTriggerWindowFrame skills && ProgramChildWindowRuntime.MatchesSkillsChangedChild(f.Id, skills) && r.Paid;
    }

    internal ProgramSkillFrame? HuZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HuZhaoFirstChild(f, c), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 43000)]
    internal bool AdvanceHuZhaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
