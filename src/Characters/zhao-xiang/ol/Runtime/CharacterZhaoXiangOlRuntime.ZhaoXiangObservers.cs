#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoXiangOlRuntime
{
    internal bool ZhaoXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhaoXiang is not { } r || !ValidZhaoXiang(f))
            return false;
        if (r.Stage == "movement" && child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage is not ("replacement-children" or "recovery-children"))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat && (r.Stage == "replacement-children" ? hp.Change.Kind == HpChangeKind.MaximumHp : hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.Amount <= r.RecoveryRequested);
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == "recovery-children" && recovery.ParentFrameId == f.Id && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == r.RecoveryRequested && recovery.Attempt.ActualTurnNumber == r.Turn && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == r.InstructionIndex;
        return false;
    }

    internal ProgramSkillFrame? ZhaoXiangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterZhaoXiangOlOperations.ZhaoXiangFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 120)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 120)]
    internal bool CanContinueZhaoXiang(ProgramSkillFrame f) => f.ZhaoXiang is not null && ValidZhaoXiang(f);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 210)]
    internal bool HasZhaoXiangDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && ZhaoXiangObserverRoot()is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 280)]
    internal bool HasZhaoXiangBeforeDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && ZhaoXiangObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 180)]
    internal bool TryAdvanceZhaoXiangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhaoXiangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
