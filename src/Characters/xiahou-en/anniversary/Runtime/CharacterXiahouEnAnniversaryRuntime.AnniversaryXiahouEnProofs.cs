#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouEnAnniversaryRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45000)]
    internal bool KeepIssuedAnniversaryXiahouEn(ProgramSkillFrame f)
    {
        if (f.AnniversaryXiahouEn is not null)
        {
            return true;
        }

        return f.SkillId == AnniversaryXiahouEnComposition.Shiwu && f.TriggerId == AnniversaryXiahouEnComposition.Death && f.InstructionIndex is 0 or 1 && f.WindowContext is { Window: SkillProgramTriggerWindow.OwnerDied, SourceSeat: { } killer } context && killer != f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == context.ParentFrameId && window.OwnerSeat == f.OwnerSeat && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 45000)]
    internal bool IsAnniversaryXiahouEnPaidMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryXiahouEn is not { Stage: AnniversaryXiahouEnStage.GainChildren, GainedCardId: > 0 } r || effect?.Op != SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwuDamage || f.PendingMovementContinuation != pending || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        return r.After > r.Before;
    }

    internal bool XiahouEnFirstObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryXiahouEn is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AnniversaryXiahouEnStage.GainChildren && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.GainedCardId && m.Reason.Value == XiahouEnGainReason);
        return child is ProgramLifecycleTriggerWindowFrame skills && r.Stage == AnniversaryXiahouEnStage.SkillChildren && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? XiahouEnObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterXiahouEnAnniversaryOperations.XiahouEnFirstObserverChild(f, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 45000)]
    internal bool HasXiahouEnPaidDamageObserver(long id) => XiahouEnObserverRoot()is { AnniversaryXiahouEn.Operation: SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwuDamage } root && root.WindowContext?.ParentFrameId == id;
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 45000)]
    internal bool HasXiahouEnObserverDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && XiahouEnObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 45000)]
    internal bool AdvanceXiahouEnObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XiahouEnObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
