#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5YimieObserversRuntime
{
    internal bool ShanheS5YimieCostFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.ShanheS5YimieCost is not { } r || !_coreShanheS5YimieOperations.HasShanheS5YimieCostPaid(root))
            return false;
        _coreShanheS5YimieOperations.RequireShanheS5YimieOrigin(r);
        if (child is DyingFrame dying)
            return r.CostHpBefore == 1 && dying.ParentFrameId == root.Id && dying.VictimSeat == root.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null;
        return child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.HpBefore == r.CostHpBefore && hp.Change.HpAfter == r.CostHpBefore - 1 && hp.Change.Amount == 1;
    }

    internal ProgramSkillFrame? ShanheS5YimieCostObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreShanheS5YimieObserversOperations.ShanheS5YimieCostFirstChild(root, child), (i) => _coreObserverSubtreesOperations2.HasMovementObserverSuffix(i));
    internal bool ShanheS5YimieRecoveryFirstChild(ShanheS5YimieRecoveryFrame root, ResolutionFrame child)
    {
        if (!root.Requested)
            return false;
        var o = root.Receipt.Origin;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == root.Id && recovery.Return.ResumeFrameId == root.Id && recovery.Return.Continuation == PostEventContinuation.ShanheS5YimieRecovery && recovery.Attempt.SourceSeat == o.Source.OwnerSeat && recovery.Attempt.TargetSeat == o.TargetSeat && recovery.Attempt.Amount == o.X && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program;
        return child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.ShanheS5YimieRecovery && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == o.Source.OwnerSeat && hp.Change.TargetSeat == o.TargetSeat && hp.Change.Amount > 0;
    }

    internal ShanheS5YimieRecoveryFrame? ShanheS5YimieRecoveryObserverRoot()
    {
        for (var i = 1; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
            if (_resolutionStackOperations._resolutionStack[i] is ShanheS5YimieRecoveryFrame root && _resolutionStackOperations._resolutionStack[i - 1].Id == root.ParentFrameId && _resolutionStackOperations._resolutionStack[i - 1].ShanheS5YimieDamage == root.Receipt && ShanheS5YimieRecoveryFirstChild(root, _resolutionStackOperations._resolutionStack[i + 1]) && _coreObserverSubtreesOperations.HasMovementObserverSuffix(i))
                return root;
        return null;
    }

    internal bool HasShanheS5YimieRecoveryContinuation(long attackId) => ShanheS5YimieRecoveryObserverRoot()is { } root && root.ParentFrameId == attackId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attackId)?.ShanheS5YimieDamage == root.Receipt;
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 5795)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 5795)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamagePrelude, 5795)]
    internal bool HasShanheS5YimieObserver(long id) => ShanheS5YimieCostObserverRoot()is { } cost && (cost.ShanheS5YimieCost!.BeforeDamageWindowId == id || _resolutionStackOperations._resolutionStack.SkipWhile(f => f.Id != cost.Id).Any(f => f.Id == id)) || ShanheS5YimieRecoveryObserverRoot()is { } recovery && _resolutionStackOperations._resolutionStack.SkipWhile(f => f.Id != recovery.Id).Any(f => f.Id == id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 5795, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 5795)]
    internal bool HasShanheS5YimieDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && (ShanheS5YimieCostObserverRoot()is { } cost && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == cost.Id) || ShanheS5YimieRecoveryObserverRoot()is { } recovery && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == recovery.Id));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 5795)]
    internal bool AdvanceShanheS5YimieChildren()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || ShanheS5YimieCostObserverRoot()is null && ShanheS5YimieRecoveryObserverRoot()is null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is NativeDrawAttemptFrame native)
        {
            _coreRuntimeOperations.AdvanceRuntimeFrame(native.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
