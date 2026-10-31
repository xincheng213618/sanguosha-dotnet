#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZouShiLeRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 270)]
    internal bool IsHuoxinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.Huoxin is { Paid: true, Stage: HuoxinStage.ObtainChildren, PaidSequence: > 0 } && effect is { Op: SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool HuoxinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Huoxin is not { Stage: HuoxinStage.ObtainChildren } r || !ValidHuoxin(f))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Id == r.BatchId && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == CardLocation.Hand(r.TargetSeat) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == HuoxinObtainReason && m.Sequence == r.PaidSequence;
    }

    internal bool HuoxinObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (_characterZouShiLeOperations.HuoxinFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? HuoxinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZouShiLeOperations.HuoxinFirstChild(f, child), (i) => _characterZouShiLeOperations.HuoxinObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1220)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1330)]
    internal bool HasHuoxinDamageObserver(long id) => HuoxinObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1360)]
    internal bool HasHuoxinBeforeDamageObserver(long id) => HuoxinObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 450)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 440)]
    internal bool IsHuoxinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Huoxin is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && HuoxinFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && HuoxinObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28951)]
    internal bool AdvanceHuoxinChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuoxinObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
