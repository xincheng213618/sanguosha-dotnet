#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26520)]
    internal void AssertDuFuRen(ProgramSkillFrame f)
    {
        if (f.DuFuRen is not { } r)
            return;
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f);
        if (f.TriggerId is null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || r.Source != DuFuRenSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || effect is null || !DuFuRenComposition.IsOperation(effect.Op) || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.WindowId == r.WindowId && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.DamageFrameId == r.DamageFrameId && e.BatchId == r.BatchId) != 1)
            throw new InvalidOperationException("Du Fu Ren lost its owning instruction, actual turn, immutable start or payment interval.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (effect.Op == SkillProgramEffectOp.ResolveYiseOwnedGain)
        {
            if (r.Stage is not (DuFuRenStage.RedChoice or DuFuRenStage.RecoveryChildren) || r.CardId is not null || r.From is not null || r.GiftSequence != 0 || r.DamageSourceSeat is not null || r.DamageFrameId is not null || r.RecipientSeat != context.TargetSeat || r.BatchId != context.MovementBatch?.Id || CurrentDuFuRenGains(candidate, context)is not { } gains || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenBlackIssuedEvent>().Count(e => e.FrameId == f.Id) != (gains.Any(g => g.Suit is Suit.Spade or Suit.Club) ? 1 : 0))
                throw new InvalidOperationException("Yise lost its exact original owned-gain batch and recipient.");
        }
        else
        {
            if (r.Stage is not (DuFuRenStage.GiftChoice or DuFuRenStage.GiftChildren) || r.BatchId is not null || r.DamageFrameId != context.DamageFrameId || r.Stage == DuFuRenStage.GiftChoice && (r.CardId is not null || r.From is not null || r.GiftSequence != 0 || r.RecipientSeat is not null) || context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && (_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } prep || prep.CandidateIndex < 0 || prep.CandidateIndex >= prep.Candidates.Count || prep.Candidates[prep.CandidateIndex] != candidate) || context.Window == SkillProgramTriggerWindow.AfterDamageApplied && (_resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } damage || damage.ParentFrameId != r.DamageFrameId || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || damage.Candidates[damage.CandidateIndex].ToProgramCandidate() != candidate))
                throw new InvalidOperationException("Shunshi lost its exact preparation or actual after-damage parent candidate.");
            if (r.Stage == DuFuRenStage.GiftChildren && (r.RecipientSeat is not { } target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat || target == r.DamageSourceSeat || r.CardId is not { } card || r.From is not { } from || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.GiftSequence <= r.Before || r.GiftSequence > r.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.GiftSequence && m.CardId == card && m.From == from && m.To == CardLocation.Hand(target) && m.Reason.Value == DuFuRenGiftReason) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenGiftIssuedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.RecipientSeat == target && e.CardId == card && e.From == from && e.MovementSequence == r.GiftSequence) != 1))
                throw new InvalidOperationException("Shunshi lost its one genuine paid HE gift and issued independent phase benefits.");
        }
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 670)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 610)]
    internal bool IsDuFuRenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.DuFuRen is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != f.OwnerSeat || recovery.Attempt.Amount != 1)
            return false;
        return r.Stage == DuFuRenStage.RecoveryChildren && recovery.Attempt.TargetSeat == r.RecipientSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 || r.Stage == DuFuRenStage.GiftChildren && r.From?.Zone == CardZoneKind.Equipment && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == DuFuRenGiftReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.CardKind == CardKind.SilverLion);
    }

    internal bool DuFuRenFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.DuFuRen is not { } r || root.PendingMovementContinuation is null)
            return false;
        AssertDuFuRen(root);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == DuFuRenStage.GiftChildren && moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements is [var m] && m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.From == r.From && m.To == CardLocation.Hand(r.RecipientSeat!.Value) && m.Reason.Value == DuFuRenGiftReason;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is RecoveryReplacementFrame recovery)
            return IsDuFuRenRecoveryParent(root, recovery);
        if (child is not HpChangedTriggerWindowFrame hp || hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount != 1 || hp.Change.SourceSeat != root.OwnerSeat)
            return false;
        return r.Stage == DuFuRenStage.RecoveryChildren && hp.Change.TargetSeat == r.RecipientSeat || r.Stage == DuFuRenStage.GiftChildren && r.From?.Zone == CardZoneKind.Equipment && hp.Change.TargetSeat == root.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.GiftSequence && m.CardId == r.CardId && m.CardKind == CardKind.SilverLion);
    }

    internal ProgramSkillFrame? DuFuRenObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterDuFuRenOlOperations.DuFuRenFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 450)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 560)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 590)]
    internal bool HasDuFuRenDamageObserver(long id) => DuFuRenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 420)]
    internal bool IsDuFuRenDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && DuFuRenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26520)]
    internal bool AdvanceDuFuRenSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DuFuRenObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
