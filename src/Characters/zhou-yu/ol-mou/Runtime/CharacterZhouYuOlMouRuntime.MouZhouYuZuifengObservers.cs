#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal bool MouZuifengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouZuifeng is not { Paid: true } r || !IsPaidMouZuifeng(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == MouZuifengStage.NativeUse && use.Id == r.UseId && use.MouZuifengAlcohol?.ProgramFrameId == f.Id && IsIssuedMouZuifengAlcoholUse(use);
        if (child is DamageFrame damage)
            return r.Stage == MouZuifengStage.DamageChildren && r.FireIssued && !r.FireReturned && damage.ParentFrameId == f.Id && f.AttackAttempt is { } attack && damage.SourceSeat == attack.SourceSeat && damage.TargetSeat == attack.TargetSeat && damage.Amount == attack.DamageAmount && damage.Nature == attack.Nature;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == MouZuifengStage.DamageChildren && r.FireIssued && !r.FireReturned && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && (r.Stage == MouZuifengStage.CostChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && MouZuifengCostBatch(f, moved.Batch) || r.Stage == MouZuifengStage.NativeChildren && moved.ResumeProgramFrameId == f.Id && IsMouZuifengNativeSettlementBatch(f, moved.Batch));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return false;
    }

    internal bool MouZuifengCardWindowEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsIssuedMouZuifengAlcoholUse(use) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId && window.Action.ActorSeat == use.SourceSeat && window.Action.EffectiveKind == CardKind.Alcohol;
        if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame parent || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } native || !IsIssuedMouZuifengAlcoholUse(native) || parent.Action.ActionId != native.Action!.ActionId || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame child || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, candidate);
    }

    internal bool MouZuifengLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal bool MouZuifengObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || this.MouZuifengCardWindowEdge(n) || this.MouZuifengLifecycleEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: { Paid: true } } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouZuifengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouZuifengFirstChild(f, child), (index) => this.MouZuifengObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 620)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 560)]
    internal bool IsMouZuifengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MouZuifeng is null || recovery.ParentFrameId == f.Id)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var childIndex = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return rootIndex >= 0 && childIndex > rootIndex + 1 && MouZuifengFirstChild(f, _resolutionStackOperations._resolutionStack[rootIndex + 1]) && MouZuifengObserverSuffix(rootIndex) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[childIndex - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29020)]
    internal bool AdvanceMouZuifengChildren()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouZuifeng: { Stage: MouZuifengStage.NativeChildren, UseReturned: true } } f && IsPaidMouZuifeng(f) && _coreCardUseLifecycleOperations.LifecycleCardUse(f.MouZuifeng.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengAlcoholReturnedEvent>().Count(e => e == new MouZuifengAlcoholReturnedEvent(f.Id, f.MouZuifeng.UseId, f.MouZuifeng.ActionId)) == 1)
        {
            ContinueMouZuifeng(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return MouZuifengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    }
}
