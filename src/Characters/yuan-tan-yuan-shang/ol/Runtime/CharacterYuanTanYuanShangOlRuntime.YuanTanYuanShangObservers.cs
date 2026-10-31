#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanTanYuanShangOlRuntime
{
    internal bool NeifaFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.Neifa is not { Segments.Count: > 0 } r || !CanContinueNeifa(root))
            return false;
        var paid = r.Segments.Last();
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.Id != moved.Id || moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            bool Material(CardMovementRecord m) => m.Sequence > paid.Before && m.Sequence <= paid.After && (paid.Name is "draw" or "equipment-draw" ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == (paid.Name == "draw" ? NeifaDrawReason : NeifaEquipmentReason) : moved.Batch.Id == paid.BatchId && m.CardId == paid.CardId && m.From == paid.From && m.To == (paid.Name == "discard" ? CardLocation.DiscardPile : CardLocation.Hand(root.OwnerSeat)) && m.Reason.Value == (paid.Name == "discard" ? NeifaDiscardReason : NeifaFieldReason));
            bool Grain(CardMovementRecord m) => paid.Name is "field" or "discard" && paid.BatchId is> 0 && moved.Batch.ParentBatchId == paid.BatchId && moved.Batch.Id > paid.BatchId && paid.From is { Zone: CardZoneKind.Equipment, OwnerSeat: { } owner } && paid.CardId is { } id && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.WoodenOx && m.Sequence > paid.After && m.From == CardLocation.WoodenOxGrain(owner) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (Material(m) || Grain(m)));
        }

        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (paid.CardId is not { } cardId || paid.From is not { Zone: CardZoneKind.Equipment, OwnerSeat: { } victim } from || _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).Kind != CardKind.SilverLion || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.From == from && m.Sequence > paid.Before && m.Sequence <= paid.After))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == victim && hp.Change.TargetSeat == victim && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == victim && recovery.Attempt.TargetSeat == victim && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.From == from && m.Sequence > paid.Before && m.Sequence <= paid.After && m.Reason == recovery.Attempt.Completion.MoveReason);
    }

    internal ProgramSkillFrame? NeifaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.NeifaFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 370)]
    internal bool TryAdvanceNeifaSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && NeifaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
