#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal bool MouHuangYueyingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouBingcai is { } r && IsPaidMouBingcai(f))
        {
            if (r.Stage == MouBingcaiStage.Drawing && f.NativeDraw is not null)
                return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
            if (child is RecoveryReplacementFrame recovery)
                return IsMouHuangYueyingRecoveryParent(f, recovery);
            if (child is HpChangedTriggerWindowFrame hp)
                return r.Stage == MouBingcaiStage.CostChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1 && r.PrintedKind == CardKind.SilverLion && r.From == CardLocation.Equipment(f.OwnerSeat);
            if (child is CardsMovedTriggerWindowFrame moved)
                return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && (moved.Batch.Movements.All(m => MouBingcaiPrimaryMovement(f, m)) || MouBingcaiOxSpill(f, moved.Batch));
        }

        if (f.MouLixian is { Paid: true } lx && ValidMouLixianStarted(f))
        {
            if (child is CardUseFrame use)
                return lx.Stage == MouLixianStage.NativeUse && use.Id == lx.UseId && use.MouLixianOrigin?.ProgramFrameId == f.Id && IsIssuedMouLixianUse(use);
            if (child is CardsMovedTriggerWindowFrame settlement)
                return settlement.ResumeProgramFrameId == f.Id && f.PendingMovementContinuation is not null && IsMouLixianNativeSettlementBatch(f, settlement.Batch);
        }

        if (f.MouBingcai is null && f.MouLixian is null)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return false;
    }

    internal bool MouHuangYueyingLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal ProgramSkillFrame? MouHuangYueyingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterHuangYueyingOlMouOperations.MouHuangYueyingFirstChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _characterHuangYueyingOlMouOperations.MouHuangYueyingLifecycleEdge(next) || _characterHuangYueyingOlMouOperations.MouLixianCardWindowEdge(next) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1510)]
    internal bool HasMouHuangYueyingDamageObserver(long id)
    {
        if (MouHuangYueyingObserverRoot()is not { } root || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id)is not DamageTriggerWindowFrame window)
            return false;
        if (_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id))
            return true;
        return root.MouLixian is { Paid: true } r && ValidMouLixianStarted(root) && r.WindowId == window.Id && root.WindowContext?.ParentFrameId == window.Id && window.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    }

    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1530)]
    internal bool HasMouHuangYueyingBeforeDamageObserver(long id) => MouHuangYueyingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28960)]
    internal bool AdvanceMouHuangYueyingChildren()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return TryAdvanceMouLixianAiNullification();
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && IsReturnedMouLixianUse(f))
        {
            ContinueMouLixian(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return MouHuangYueyingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28960)]
    internal bool ResumeMouHuangYueying(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouBingcai is not null)
        {
            if (f.NativeDraw is null)
                ContinueMouBingcai(id);
            return true;
        }

        if (f.MouLixian is not null)
        {
            ContinueMouLixian(id);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28960)]
    internal bool ReturnMouHuangYueyingMovement(ProgramSkillFrame f)
    {
        if (f.MouBingcai is not null)
        {
            ContinueMouBingcai(f.Id);
            return true;
        }

        if (f.MouLixian is not null)
        {
            ContinueMouLixian(f.Id);
            return true;
        }

        return false;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28960)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28960)]
    internal bool ContinuePaidMouHuangYueying(ProgramSkillFrame f) => f.MouBingcai is { Paid: true } && IsPaidMouBingcai(f) || f.MouLixian is { Paid: true } r && ValidMouLixianStarted(f) && (r.Stage == MouLixianStage.NativeUse && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { } use && IsIssuedMouLixianUse(use) || r.Stage == MouLixianStage.NativeChildren && r.NativeReturned && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1);
}
