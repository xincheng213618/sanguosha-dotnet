#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuBanOlRuntime
{

    internal static CardMoveReason HuiyunNativePaymentReason(HuiyunNativeInvoice n) => global::CardGame.Core.RuntimeSkills.RuntimeRules_hu_ban_ol_HuiyunObservers.HuiyunNativePaymentReason(n);

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 330)]
    internal bool IsHuBanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.Huiyun is { Stage: HuiyunStage.NativeChildren or HuiyunStage.RecastCostChildren or HuiyunStage.RecastDrawChildren or HuiyunStage.DrawChildren } r && e?.Op == SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack && p == f.PendingMovementContinuation && p.SubjectSeat == HuiyunCurrent(r).TargetSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool IsHuiyunNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        if (f.Huiyun is not { Stage: HuiyunStage.NativeChildren, NativeCompleted: true, Native: { } n } r || f.PendingMovementContinuation is not { } pending || !IsHuBanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) || n.ProgramFrameId != f.Id || n.OpportunityIndex != r.Index || r.Before != n.BeforePayment || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunNativeReturnedEvent>().Where(e => e.Invoice.UseId == n.UseId).ToArray()is not [var returned] || !SameHuiyunNative(returned.Invoice, n) || returned.After != r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == n.UseId) != 1 || batch.ParentFrameId != n.UseId || batch.AwaitingProgramFrameId is not null || batch.OriginOwnerSeat != f.OwnerSeat || batch.OriginSkillId != f.SkillId || batch.OriginSkillInstanceId != f.SkillInstanceId || batch.Movements.Count == 0)
            return false;
        return batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool HuBanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Huiyun is not { } r)
            return false;
        if (child is CardUseFrame use)
            return HuiyunNativeUseRidesOn(use, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is not CardsMovedTriggerWindowFrame moved || f.PendingMovementContinuation is null)
            return false;
        if (r.Stage == HuiyunStage.NativeChildren)
            return moved.ResumeProgramFrameId == f.Id && IsHuiyunNativeSettlementBatch(f, moved.Batch);
        if (!global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
            return false;
        if (r.Stage == HuiyunStage.RecastCostChildren)
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunRecastCardEvent>().Any(p => p.FrameId == f.Id && p.Index == r.Index && p.CardId == m.CardId && p.Sequence == m.Sequence));
        return r.Stage is HuiyunStage.RecastDrawChildren or HuiyunStage.DrawChildren && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(HuiyunCurrent(r).TargetSeat) && m.Reason == (r.Stage == HuiyunStage.RecastDrawChildren ? CardMoveReasons.RecastDraw : new CardMoveReason("skill-program.hu-ban.huiyun.draw"))));
    }

    internal ProgramSkillFrame? HuBanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuBanOlOperations.HuBanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    internal int? HuBanAnyObserverRootIndex()
    {
        if (HuBanObserverRoot()is { } program)
            return _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == program.Id);
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not CardUseFrame { RecoveryPaidContinuation: { } continuation } use || !HasHuiyunTrickPaymentTail(use, continuation))
                continue;
            var first = _resolutionStackOperations._resolutionStack[i + 1];
            var native = first is CardsMovedTriggerWindowFrame moved && _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, moved) || first is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == use.Id && hp.ResumeFrameId == use.Id && hp.Continuation == PostEventContinuation.RecoveryPaidCardUse || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(first, use);
            if (native && _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)))
                return i;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 840)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 950)]
    internal bool HasHuBanDamageObserver(long id) => HuBanAnyObserverRootIndex()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > root;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 980)]
    internal bool HasHuBanBeforeDamageObserver(long id) => HuBanAnyObserverRootIndex()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > root;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27380)]
    internal bool AdvanceHuBanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuBanAnyObserverRootIndex()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27381)]
    internal bool ContinueIssuedHuiyun(ProgramSkillFrame f) => f.Huiyun is null && f.InstructionIndex is 0 or 1 && f.TriggerId == "completed-converted-fire-attack" && f.WindowContext is { } c && CanRunIssuedHuiyunCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 2100)]
    internal bool WindowSettlement_IsHuiyunNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsHuiyunNativeSettlementBatch(frame, batch);
}
