#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuJingAnniversaryRuntime
{
    internal bool AnniversaryXuJingMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryXuJing is not { Stage: AnniversaryXuJingStage.MovementChildren or AnniversaryXuJingStage.DrawChildren } r || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || effect?.Op != (r.Mode == "caixia" ? SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia : SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu))
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray();
        if (r.Stage == AnniversaryXuJingStage.MovementChildren)
            return r.MovementSource is { } source && r.MovementDestination is { } destination && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingMovedEvent>().Count(e => e == new AnniversaryXuJingMovedEvent(f.Id, r.CardId, source, destination, r.Before, r.After)) == 1 && (source == destination ? r.Before == r.After && moves.Length == 0 : moves is [var movement] && movement.CardId == r.CardId && movement.From == source && movement.To == destination && movement.Reason == AnniversaryXuJingMoveReason);
        return r.Draws.LastOrDefault()is { } draw && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After && draw.RecipientSeat == r.TargetSeat && draw.ActualDrawIds.All(id => moves.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason == AnniversaryXuJingDrawReason) == 1);
    }

    internal bool AnniversaryXuJingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryXuJing is not { } r)
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryXuJingStage.Drawing && draw.Context.RecipientSeat == r.TargetSeat && draw.Context.RequestedCount == r.Count && draw.Context.ParentFrameId == f.Id;
        if (r.Stage is not (AnniversaryXuJingStage.MovementChildren or AnniversaryXuJingStage.DrawChildren))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } pending && AnniversaryXuJingMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Return.Continuation == (r.Stage == AnniversaryXuJingStage.MovementChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == (r.Stage == AnniversaryXuJingStage.MovementChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AnniversaryXuJingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.AnniversaryXuJingFirstChild(frame, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 43100)]
    internal bool AnniversaryXuJingDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryXuJingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 43100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 43100)]
    internal bool AnniversaryXuJingRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryXuJing is null)
            return false;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == recovery.Id);
        return root >= 0 && child > root && AnniversaryXuJingFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && (child == root + 1 || Enumerable.Range(root + 2, child - root - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i))) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 43100)]
    internal bool AdvanceAnniversaryXuJingChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryXuJingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
