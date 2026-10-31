#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoPuOlRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 120)]
    internal bool IsZhenyingMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => effect?.Op == SkillProgramEffectOp.ResolveZhenyingSimultaneousHands && f.Zhenying is { Stage: ZhenyingStage.AdjustmentChildren } && p is { BeforeCount: 0, CoverageResultBind: null } && p.SubjectSeat == f.OwnerSeat && ValidZhenying(f);
    internal bool ZhenyingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zhenying is not { Stage: ZhenyingStage.AdjustmentChildren } r || f.PendingMovementContinuation is null || !ValidZhenying(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && (m.Reason.Value is ZhenyingDiscardReason or ZhenyingDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? ZhenyingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterHaoPuOlOperations.ZhenyingFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 430)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 540)]
    internal bool HasZhenyingDamageObserver(long id) => ZhenyingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 570)]
    internal bool HasZhenyingBeforeDamageObserver(long id) => ZhenyingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26500)]
    internal bool AdvanceZhenyingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhenyingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
