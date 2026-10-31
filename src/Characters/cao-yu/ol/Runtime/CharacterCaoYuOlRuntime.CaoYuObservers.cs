#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal static bool CaoYuChildren(CaoYuStage stage) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cao_yu_ol_CaoYuObservers.CaoYuChildren(stage);

    internal bool IsCaoYuIssuedMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.CaoYu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage is CaoYuStage.GongjieDrawChildren or CaoYuStage.XiangxuDrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == (r.Stage == CaoYuStage.GongjieDrawChildren ? GongjieDrawReason : XiangxuDrawReason) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        var reason = r.Stage switch
        {
            CaoYuStage.GongjieGiftChildren => GongjieGiftReason,
            CaoYuStage.XiangxuDiscardChildren => XiangxuDiscardReason,
            CaoYuStage.XiangzuGiftChildren => XiangzuGiftReason,
            _ => null
        };
        if (reason is null)
            return false;
        return r.Materials.Any(c => c.CardId == m.CardId && c.From == m.From && m.To == (r.Stage == CaoYuStage.XiangxuDiscardChildren ? CardLocation.DiscardPile : CardLocation.Hand(r.Target!.Value)) && m.Reason.Value == reason) || r.GrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(f.OwnerSeat));
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 170)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 160)]
    internal bool IsCaoYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.CaoYu is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement)
            return false;
        return r.Stage is CaoYuStage.XiangxuRecoveryChildren or CaoYuStage.XiangzuRecoveryChildren && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == r.Recovery && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 || r.Stage is CaoYuStage.GongjieGiftChildren or CaoYuStage.XiangzuGiftChildren && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat)) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == (r.Stage == CaoYuStage.GongjieGiftChildren ? GongjieGiftReason : XiangzuGiftReason);
    }

    internal bool CaoYuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.CaoYu is not { } r || !CaoYuChildren(r.Stage))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsCaoYuRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsCaoYuIssuedMovement(f, m));
    }

    internal ProgramSkillFrame? CaoYuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.CaoYuFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27440)]
    internal bool AdvanceCaoYuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoYuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
