#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYuanMobileShiRuntime
{
    internal bool ValidShiGy(ProgramSkillFrame f) => f.ShiGuoYuan is { } r && f.InstructionIndex == 1 && r.Source == ShiGySource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && r.WindowId == f.WindowContext?.ParentFrameId && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyStartedEvent>().Count(e => e == new ShiGyStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.WindowId, f.TriggerId!)) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 48100)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 48100)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 48100)]
    internal bool ShiGyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.ShiGuoYuan is not { } r || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || !ValidShiGy(f) || effect?.Op is not (SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao or SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng or SkillProgramEffectOp.ResolveMobileShiGuoYuanChenshe))
            return false;
        return r.Stage is ShiGyStage.Movement or ShiGyStage.DrawChildren or ShiGyStage.RecoveryChildren or ShiGyStage.LostSkillChildren || r.Stage == ShiGyStage.NativeTail && r.Returned && r.Native is not null;
    }

    internal bool IsShiGyNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.ShiGuoYuan is { Stage: ShiGyStage.NativeTail, Returned: true, Native: { } n } r && ValidShiGy(f) && r.NativeBatches.Contains(b.Id) && b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyNativeBatchEvent>().Count(e => e == new ShiGyNativeBatchEvent(f.Id, n.UseId, b.Id)) == 1;
    internal bool ShiGyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShiGuoYuan is not { } r || !ValidShiGy(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiGyStage.Drawing && draw.Context.RecipientSeat == r.Victim && draw.Context.RequestedCount == 2 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardUseFrame use)
            return r.Stage == ShiGyStage.NativeUse && IsShiGyNative(use);
        if (child is CardDeclarationFrame d)
            return MatchesShiGyDeclaration(f, d);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ShiGyMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && (IsShiGyNativeSettlementBatch(f, moved.Batch) || moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiGyChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (index) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || _characterGuoYuanMobileShiOperations.ShiGyDeclarationEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index, dying));
    internal ProgramSkillFrame? ShiGyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuoYuanMobileShiOperations.ShiGyFirstChild(f, child), (i) => _characterGuoYuanMobileShiOperations.ShiGyChildSuffix(i));
    internal bool ShiGyDeclarationEdge(int i)
    {
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not CardDeclarationFrame d || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == d.Return.ParentFrameId)is not ProgramSkillFrame root || !MatchesShiGyDeclaration(root, d))
            return false;
        if (_resolutionStackOperations._resolutionStack[i] is CardDeclarationChallengeFrame challenge)
            return d.Stage == CardDeclarationStage.Challenging && challenge.ParentFrameId == d.Id && challenge.SeatIndex >= 0 && challenge.SeatIndex < challenge.EligibleSeats.Count;
        if (_resolutionStackOperations._resolutionStack[i] is CardsMovedTriggerWindowFrame moved)
            return d.Stage is CardDeclarationStage.Paying or CardDeclarationStage.Cleaning && d.ActiveChildFrameId == moved.Id && moved.ResumeDeclarationFrameId == d.Id && moved.Batch.ParentFrameId == d.Id && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == d.Payment.Cost.CardId && (d.Stage == CardDeclarationStage.Paying ? m.From == d.Payment.Cost.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay" : m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value is "conversion.declaration.failed" or "conversion.declaration.cancelled");
        return _resolutionStackOperations._resolutionStack[i] is ProgramLifecycleTriggerWindowFrame skills && d.Stage == CardDeclarationStage.Granting && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == d.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeCardDeclaration;
    }
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 48100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 48100)]
    internal bool ShiGyDamageChild(long id) => ShiGyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 48100)]
    internal bool ShiGyBeforeDamageChild(long id) => ShiGyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 48100)]
    internal bool ShiGyDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiGyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48100)]
    internal bool RetainShiGy(ProgramSkillFrame f) => f.ShiGuoYuan is not null && ValidShiGy(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 48100)]
    internal bool AdvanceShiGyChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiGyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 48100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 48100)]
    internal bool ShiGyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.ShiGuoYuan is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && ShiGyFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || ShiGyChildSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiGuoYuanPublicStates(CharacterState owner, string skillId)
    {
        if (skillId is not (ShiGyXiugeng or ShiGyQingdao or ShiGyChenshe))
            return[];
        var states = new List<ProgramPublicRuleStateSnapshot>();
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId))
        {
            if (skillId == ShiGyXiugeng)
            {
                states.Add(new(instance.SkillInstanceId, "xiugeng-hand-limit", ProgramPublicRuleStateKind.PersistentHandLimit, ShiGyHandLimitBonus(owner.Seat)));
                if (ShiGyRecord(owner.Seat, instance.SkillInstanceId)is { } record)
                    states.AddRange(_coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyHandRecordedEvent>().Where(e => e.FrameId == record.FrameId).Select(e => new ProgramPublicRuleStateSnapshot(instance.SkillInstanceId, "xiugeng-seat-" + e.Target, ProgramPublicRuleStateKind.GameActivationProgress, e.HandCount)));
            }
            else
                states.Add(new(instance.SkillInstanceId, skillId == ShiGyQingdao ? "qingdao-ready" : "chenshe-ready", ProgramPublicRuleStateKind.GameActivationProgress, 1));
        }

        return states.ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 500)]
    internal bool WindowSettlement_IsShiGyNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsShiGyNativeSettlementBatch(frame, batch);
}
