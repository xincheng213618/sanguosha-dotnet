#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiMouRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 220)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 210)]
    internal bool IsMouCaoPiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.MouCaoPi is { Stage: MouCaoPiStage.RecoveryChildren, Option: "grow" } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, f.InstructionIndex);
    internal bool MouCaoPiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouCaoPi is not { Stage: not MouCaoPiStage.Choosing } r)
            return false;
        if (child is NativeDrawAttemptFrame)
            return r.Stage == MouCaoPiStage.DrawRequest && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f) && f.NativeDraw is { RequestedCount: 3, Operation: SkillProgramEffectOp.MouCaoPiXingshang };
        if (child is RecoveryReplacementFrame recovery)
            return IsMouCaoPiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.TargetSeat == r.TargetSeat && (r.Stage == MouCaoPiStage.RecoveryChildren && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.Amount == 1 || r.Stage == MouCaoPiStage.MaximumChildren && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.Amount == 1);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return r.Stage == MouCaoPiStage.DrawChildren && child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason.Value == MouCaoPiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    }

    internal ProgramSkillFrame? MouCaoPiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCaoPiMouOperations.MouCaoPiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 980)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1090)]
    internal bool HasMouCaoPiDamageObserver(long id) => MouCaoPiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1120)]
    internal bool HasMouCaoPiBeforeDamageObserver(long id) => MouCaoPiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28000)]
    internal bool AdvanceMouCaoPiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouCaoPiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
