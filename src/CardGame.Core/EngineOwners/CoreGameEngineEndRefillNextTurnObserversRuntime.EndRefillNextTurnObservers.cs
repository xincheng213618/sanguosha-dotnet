#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEndRefillNextTurnObserversRuntime
{
    internal bool EndRefillFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.EndRefillNextTurn is not { Stage: EndRefillNextTurnStage.MovementChildren } r || root.PendingMovementContinuation is null)
            return false;
        _coreEndRefillNextTurnOperations.RequireEndRefillNextTurnReceipt(root);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == CoreGameEngineEndRefillNextTurnRuntime.EndRefillReason(root));
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
    }

    internal ProgramSkillFrame? EndRefillObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.EndRefillFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 230)]
    internal bool IsEndRefillDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && EndRefillObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 60)]
    internal bool TryAdvanceEndRefillSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && EndRefillObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
