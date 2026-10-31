#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool IsMoTianenMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MoTianen is { PaymentSealed: true, Stage: MoTianenStage.DiscardChildren or MoTianenStage.ObtainChildren } && effect is { Op: SkillProgramEffectOp.ResolveMoSunQuanTianenUniqueUse, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MoTianenLionRemoval(ProgramSkillFrame f, int seat) => f.MoTianen is { PaymentSealed: true, Branch: MoTianenBranch.Different, Material: { PrintedKind: CardKind.SilverLion } material } && material.From == CardLocation.Equipment(seat);
    internal bool MoTianenFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MoTianen is not { } r || !ValidMoTianen(f))
            return false;
        if (r.Stage == MoTianenStage.QuanyuChild)
            return MoQuanyuChildRidesOn(child, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => r.Movements.Any(p => p.CardId == m.CardId && p.PrintedKind == m.CardKind && p.From == m.From && p.To == m.To && p.Reason == m.Reason.Value && p.Sequence == m.Sequence));
        if (child is RecoveryReplacementFrame recovery)
            return IsMoSunQuanPaidRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount == 1 && MoTianenLionRemoval(f, hp.Change.TargetSeat);
        return MoSunQuanPaidStateChild(f, child);
    }

    internal bool MoQiangangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => f.MoQiangang is { Stage: MoQiangangStage.TianenLossChildren or MoQiangangStage.EntryChildren } && ValidMoQiangang(f) && MoSunQuanPaidStateChild(f, child);
    internal bool MoSunQuanPaidStateChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
    }

    internal bool MoSunQuanPaidObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || _characterSunQuanOlMagicOperations.MoQuanyuFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || _characterSunQuanOlMagicOperations.MoTianenFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || _characterSunQuanOlMagicOperations.MoQiangangFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MoSunQuanPaidObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterSunQuanOlMagicOperations.MoTianenFirstChild(f, child) || _characterSunQuanOlMagicOperations.MoQiangangFirstChild(f, child), (index) => _characterSunQuanOlMagicOperations.MoSunQuanPaidObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 640)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 580)]
    internal bool IsMoSunQuanPaidRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MoTianen is null && f.MoQiangang is null)
            return false;
        if (recovery.ParentFrameId == f.Id)
            return ValidMoTianen(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == MoTianenDiscardReason && MoTianenLionRemoval(f, recovery.Attempt.TargetSeat);
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && (MoTianenFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) || MoQiangangFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1])) && MoSunQuanPaidObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28920)]
    internal bool AdvanceMoSunQuanPaidChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MoSunQuanPaidObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
