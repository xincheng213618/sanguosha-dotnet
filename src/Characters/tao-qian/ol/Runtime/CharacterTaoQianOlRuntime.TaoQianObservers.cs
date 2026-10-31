#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{

    internal bool CanContinueIssuedTaoQian(ProgramSkillFrame f)
    {
        if (f.TaoQian is not { Paid: true })
            return ContinueIssuedZhaohuo(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28120)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28120)]
    internal bool PermitIssuedTaoQian(ProgramSkillFrame f) => f.TaoQian is { Paid: true } && CanContinueIssuedTaoQian(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 180)]
    internal bool IsTaoQianMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => IsZhaohuoMovement(f, e, p) || f.TaoQian is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is TaoQianStage.NativeChildren or TaoQianStage.DiscardChildren or TaoQianStage.DrawChildren;
    internal bool TaoQianPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.TaoQian!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == TaoQianStage.DiscardChildren)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanDiscardEntityEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
                return true;
            return m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.CardKind == CardKind.WoodenOx && c.From == CardLocation.Equipment(f.OwnerSeat));
        }

        return r.Stage == TaoQianStage.DrawChildren && r.DrawOutcome is { } o && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == WenrenDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(o.RecipientSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 340)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 330)]
    internal bool IsTaoQianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => IsZhaohuoRecoveryParent(f, child) || f.TaoQian is { Paid: true, Stage: TaoQianStage.DiscardChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && child.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && child.Attempt.SourceSeat == f.OwnerSeat && child.Attempt.TargetSeat == f.OwnerSeat && child.Attempt.Amount == 1 && child.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Materials.Any(c => c.CardKind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat)) && child.Attempt.Completion.MoveReason?.Value == ZongluanDiscardReason;
    internal bool TaoQianFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.TaoQian is not { Paid: true } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == TaoQianStage.NativeSlash && use.ZongluanSlash?.ProgramFrameId == f.Id && IsIssuedZongluanUse(use);
        if (r.Stage == TaoQianStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsTaoQianRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => TaoQianPaidMovement(f, m));
    }

    internal bool TaoQianLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } c && c.ParentFrameId == w.Id && c.Window == w.Window;
    internal bool TaoQianObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterTaoQianOlOperations.TaoQianLifecycleEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? TaoQianObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterTaoQianOlOperations.TaoQianFirstChild(f, child), (i) => _characterTaoQianOlOperations.TaoQianObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1110)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1220)]
    internal bool HasTaoQianDamageObserver(long id) => HasZhaohuoDamageObserver(id) || TaoQianObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1250)]
    internal bool HasTaoQianBeforeDamageObserver(long id) => HasZhaohuoBeforeDamageObserver(id) || TaoQianObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28120)]
    internal bool AdvanceTaoQianChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && TaoQianObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
