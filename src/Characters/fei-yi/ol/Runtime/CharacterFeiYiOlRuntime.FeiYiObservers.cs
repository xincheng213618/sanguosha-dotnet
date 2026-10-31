#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{

    internal bool FeiYiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.FeiYi is not { Paid: true } r || root.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == FeiYiStage.DiscardChildren ? m.From == CardLocation.Hand(root.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == FeiYiDiscardReason : m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == FeiYiDrawReason));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return false;
    }

    internal ProgramSkillFrame? FeiYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterFeiYiOlOperations.FeiYiFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 470)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 580)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 610)]
    internal bool HasFeiYiDamageObserver(long id) => FeiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26600)]
    internal bool AdvanceFeiYiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FeiYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
