#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangMobileRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 82300)]
    internal bool KeepPaidMobileDuanfa(ProgramSkillFrame f)
    {
        if (f.MobileDuanfa is not { Stage: not MobileDuanfaStage.Cards, ActualCount: > 0 })
            return false;
        return true;
    }

    internal bool MobileDuanfaObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileDuanfa is not { Stage: MobileDuanfaStage.CostChildren } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && r.Cards.Contains(m.CardId));
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.Kind == HpChangeKind.Recovery;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileDuanfaDiscard;
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileDuanfaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhouFangMobileOperations.MobileDuanfaObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 82300)]
    internal bool MobileDuanfaDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileDuanfaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 82300)]
    internal bool AdvanceMobileDuanfaObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileDuanfaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
