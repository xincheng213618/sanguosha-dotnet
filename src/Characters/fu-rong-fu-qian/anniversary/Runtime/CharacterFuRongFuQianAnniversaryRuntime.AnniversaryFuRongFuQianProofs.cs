#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuRongFuQianAnniversaryRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44700)]
    internal bool KeepPaidAnniversaryFuPair(ProgramSkillFrame f)
    {
        if (f.AnniversaryFuRongFuQian is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && AdmitAnniversaryXuewei(AnniversaryFuPairCandidate(f), w);
    }
    internal bool AnniversaryFuPairFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryFuRongFuQian is not { } r)
            return false;
        if (r.Stage == AnniversaryFuRongFuQianStage.Drawing && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (child is DyingFrame dying)
            return r.Operation == SkillProgramEffectOp.ResolveAnniversaryFuRongFuQianXuewei && r.HpPaid && r.HpAfter <= 0 && r.Stage == AnniversaryFuRongFuQianStage.HpChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.KillerSeat is null && dying.ResumesProgramSkill;
        if (child is DeathFrame death)
            return r.Operation == SkillProgramEffectOp.ResolveAnniversaryFuRongFuQianYuguan && r.Stage == AnniversaryFuRongFuQianStage.MaximumChildren && r.MaximumAfter == 0 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.ReturnKind == DeathReturnKind.ProgramSkill;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.SourceSeat is null && (r.Stage == AnniversaryFuRongFuQianStage.HpChildren && r.HpPaid && hp.Change.Kind == HpChangeKind.Loss && hp.Change.Amount == 1 && hp.Change.HpAfter == r.HpAfter || r.Stage == AnniversaryFuRongFuQianStage.MaximumChildren && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.Amount == 1 && hp.Change.HpAfter == r.HpAfterMaximum);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AnniversaryFuRongFuQianStage.DrawChildren && moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && r.Draws.Any(d => m.Sequence > d.Outcome.SequenceBefore && m.Sequence <= d.Outcome.SequenceAfter));
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AnniversaryFuPairObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.AnniversaryFuPairFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44700)]
    internal bool HasAnniversaryFuPairDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryFuPairObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44700)]
    internal bool AdvanceAnniversaryFuPairObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryFuPairObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
