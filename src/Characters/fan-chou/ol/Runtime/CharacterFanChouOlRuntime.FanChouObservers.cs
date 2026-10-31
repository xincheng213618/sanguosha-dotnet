#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanChouOlRuntime
{
    internal bool FanChouFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueFanChou(root) || root.FanChou is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.Id != moved.Id || moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            // Wooden Ox empties its grain after the equipment payment is recorded.
            // Accept only that native child batch of the exact paid equipment removal.
            bool IsPaidGrainMovement(CardMovementRecord movement) => r.Stage == FanChouStage.MovementChildren && r.Branch is "field" or "other-give" or "other-discard" && r.PaidBatchId is> 0 && moved.Batch.ParentBatchId == r.PaidBatchId && moved.Batch.Id > r.PaidBatchId && r.From is { Zone: CardZoneKind.Equipment, OwnerSeat: { } owner } && r.SelectedId is { } cardId && _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).Kind == CardKind.WoodenOx && movement.Sequence > r.After && movement.From == CardLocation.WoodenOxGrain(owner) && movement.To == CardLocation.DiscardPile && movement.Reason == CardMoveReasons.WoodenOxGrainDiscard;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == FanChouStage.DrawChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == FanChouDrawReason : moved.Batch.Id == r.PaidBatchId && m.CardId == r.SelectedId && m.From == r.From && (r.Branch == "other-discard" ? m.To == CardLocation.DiscardPile && m.Reason.Value == FanChouDiscardReason : m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == (r.Branch == "field" ? FanChouFieldReason : r.Branch == "deck" ? FanChouDeckReason : FanChouGiftReason))) || IsPaidGrainMovement(m)));
        }

        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        var removed = r.Stage == FanChouStage.MovementChildren && r.SelectedId is { } id && r.From is { Zone: CardZoneKind.Equipment, OwnerSeat: { } seat } from && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.CardKind == CardKind.SilverLion && m.From == from && m.Sequence > r.Before && m.Sequence <= r.After);
        if (!removed || r.From?.OwnerSeat is not { } victim)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == victim && hp.Change.TargetSeat == victim && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == victim && recovery.Attempt.TargetSeat == victim && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == r.SelectedId && m.From == r.From && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason == recovery.Attempt.Completion.MoveReason);
    }

    internal ProgramSkillFrame? FanChouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterFanChouOlOperations.FanChouFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 310)]
    internal bool HasFanChouDamageObserver(long id) => FanChouObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 400)]
    internal bool HasFanChouBeforeDamageObserver(long id) => FanChouObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 310)]
    internal bool TryAdvanceFanChouSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FanChouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
