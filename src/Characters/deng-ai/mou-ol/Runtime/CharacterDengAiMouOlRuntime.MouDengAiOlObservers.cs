#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMouOlRuntime
{
    internal bool IsMouDengAiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation m) => f.MouDengAiOl is { Stage: MouDengAiOlStage.Children or MouDengAiOlStage.CostChildren } r && ValidMouDengAiOl(f) && effect?.Op == r.Operation && m == f.PendingMovementContinuation && m.SubjectSeat == f.OwnerSeat && m.BeforeCount == 0 && m.CoverageResultBind is null;
    internal bool MouDengAiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouDengAiOl is not { } r || !ValidMouDengAiOl(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } m && IsMouDengAiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), m))
            return IsMouDengAiNativeSettlementBatch(f, moved.Batch) && moved.ResumeProgramFrameId == f.Id || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(x => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(x));
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Paid && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.TargetSeat == f.OwnerSeat;
        if (child is CardUseFrame use)
            return r.Stage == MouDengAiOlStage.NativeUse && use.Id == r.UseId && IsMouDengAiSnatch(use.Id);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouDengAiNativeEdge(int n)
    {
        if (_resolutionStackOperations._resolutionStack[n - 1] is CardUseFrame use && IsMouDengAiSnatch(use.Id) && _resolutionStackOperations._resolutionStack[n] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { MouDengAiJiewan: not null } native && IsMouDengAiSnatch(native.Id) && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return false;
    }

    internal bool MouDengAiObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || MouDengAiNativeEdge(n) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouDengAiFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouDengAiObserverSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterDengAiMouOlOperations.MouDengAiObserverEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouDengAiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDengAiMouOlOperations.MouDengAiFirstChild(f, c), (n) => _characterDengAiMouOlOperations.MouDengAiObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33400)]
    internal bool IsMouDengAiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouDengAiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33400)]
    internal bool AdvanceMouDengAiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDengAiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33400)]
    internal bool ContinuePaidMouDengAi(ProgramSkillFrame f) => f.MouDengAiOl is { Paid: true } && ValidMouDengAiOl(f);
}
