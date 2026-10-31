#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunRuOlRuntime
{

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 50)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 40)]
    internal bool IsSunRuOlRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery)
    {
        if (producer.SunRuOlWeimian is not { Paid: true } r || r.Source != SunRuOlSource(producer) || recovery.ParentFrameId != producer.Id || recovery.Return != new RecoveryReplacementReturn(PostEventContinuation.Program, producer.Id))
            return false;
        if (r.Stage == SunRuOlWeimianStage.RecoveryChildren)
            return recovery.Attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, producer.InstructionIndex) && recovery.Attempt.SourceSeat == producer.OwnerSeat && recovery.Attempt.TargetSeat == r.RecipientSeat && recovery.Attempt.Amount == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<SunRuOlWeimianBenefitIssuedEvent>().Any(e => e.FrameId == producer.Id && e.Benefit == 2 && e.RecipientSeat == r.RecipientSeat);
        return r.Stage == SunRuOlWeimianStage.CostChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason("equipment.slot-abolished") && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == producer.OwnerSeat && recovery.Attempt.TargetSeat == producer.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.Equipment(producer.OwnerSeat) && m.Reason.Value == "equipment.slot-abolished" && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion);
    }

    internal bool SunRuOlFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.SunRuOlChishi is null && root.SunRuOlWeimian is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            var before = root.SunRuOlChishi?.SequenceBefore ?? root.SunRuOlWeimian!.SequenceBefore;
            var after = root.SunRuOlChishi?.SequenceAfter ?? root.SunRuOlWeimian!.SequenceAfter;
            var reason = root.SunRuOlChishi is not null ? SunRuOlChishiDrawReason : root.SunRuOlWeimian!.Stage switch
            {
                SunRuOlWeimianStage.CostChildren => "equipment.slot-abolished",
                SunRuOlWeimianStage.DiscardChildren => SunRuOlRefreshDiscardReason,
                SunRuOlWeimianStage.DrawChildren => SunRuOlRefreshDrawReason,
                _ => ""
            };
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && m.Reason.Value == reason);
        }

        if (child is RecoveryReplacementFrame recovery)
            return IsSunRuOlRecoveryParent(root, recovery);
        if (child is HpChangedTriggerWindowFrame hp && root.SunRuOlWeimian is { Paid: true } w)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && (w.Stage == SunRuOlWeimianStage.CostChildren && hp.Change.TargetSeat == root.OwnerSeat || w.Stage == SunRuOlWeimianStage.RecoveryChildren && hp.Change.TargetSeat == w.RecipientSeat && hp.Change.SourceSeat == root.OwnerSeat);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && CanContinuePaidSunRuOl(root);
    }

    internal ProgramSkillFrame? SunRuOlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterSunRuOlOperations.SunRuOlFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 340)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 450)]
    internal bool HasSunRuOlDamageObserver(long id) => SunRuOlObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26110)]
    internal bool TryAdvanceSunRuOlSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SunRuOlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
