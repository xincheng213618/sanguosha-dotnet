#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeJinMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 35600)]
    internal bool MouZhugeJinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouZhugeJin is { Committed: true } r && effect?.Op == r.Operation && MouZhugeJin.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouZhugeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouZhugeJinStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouZhugeJinMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouZhugeJinStage.CostChildren && r.Paying is { From.Zone: CardZoneKind.Equipment } card)
        {
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id && m.From == card.From && m.CardKind == CardKind.SilverLion && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MouZhugeJinRuntime.DiscardReason);
            if (lion && child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && rr.Attempt.SourceSeat == card.From.OwnerSeat && rr.Attempt.TargetSeat == card.From.OwnerSeat;
            if (lion && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == card.From.OwnerSeat;
        }

        if (r.Stage == MouZhugeJinStage.LossChildren)
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1 && e.RemainingHp == r.HpBefore - 1) == 1;
            if (paid && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.HpBefore == r.HpBefore && hp.Change.HpAfter == r.HpBefore - 1;
            if (paid && child is DyingFrame dying)
                return dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouZhugeJinEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouZhugeJinFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouZhugeJinSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterZhugeJinMouOperations.MouZhugeJinEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouZhugeJinRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhugeJinMouOperations.MouZhugeJinFirstChild(f, c), (n) => _characterZhugeJinMouOperations.MouZhugeJinSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 35600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 35600)]
    internal bool MouZhugeJinDamageWindow(long id) => MouZhugeJinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 35600)]
    internal bool MouZhugeJinBeforeDamage(long id) => MouZhugeJinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 35600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35600)]
    internal bool MouZhugeJinDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouZhugeJinRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 35600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 35600)]
    internal bool MouZhugeJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouZhugeJinFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouZhugeJinFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouZhugeJinSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35600)]
    internal bool MouZhugeJinAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZhugeJinRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
