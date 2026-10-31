#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27260)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27260)]
    internal bool ContinuePaidYaDan(ProgramSkillFrame f)
    {
        if (f.YaDan is not { } r || r.Entities.Count == 0 && r.NativeOrigin is null)
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27261)]
    internal bool ContinueIssuedQingya(ProgramSkillFrame f)
    {
        if (f.YaDan is not null || f.InstructionIndex is not (0 or 1) || f.TriggerId != "use-next-actual-phase" || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.ActualPhaseEnded } context || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.ResolveQingyaNextActualPhaseUse }])
            return false;
        return IsIssuedQingyaCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 290)]
    internal bool IsYaDanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.YaDan is not { Stage: QingyaStage.DiscardChildren or QingyaStage.NativeChildren } || !IsYaDanOperation(effect?.Op ?? default) || pending != f.PendingMovementContinuation)
            return false;
        return true;
    }

    internal bool YaDanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YaDan is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == QingyaStage.NativeUse && use.QingyaOrigin == r.NativeOrigin && IsIssuedQingyaUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == QingyaStage.DiscardChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == QingyaDiscardReason && m.From.Zone == CardZoneKind.Hand && m.To == CardLocation.DiscardPile);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? YaDanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterYaDanOlOperations.YaDanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 750)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 860)]
    internal bool HasYaDanDamageObserver(long id) => YaDanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 890)]
    internal bool HasYaDanBeforeDamageObserver(long id) => YaDanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27260)]
    internal bool AdvanceYaDanObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YaDanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
