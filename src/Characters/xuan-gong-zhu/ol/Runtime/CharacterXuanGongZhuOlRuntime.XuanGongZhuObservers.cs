#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuanGongZhuOlRuntime
{
    internal void RequireIssuedXuanGongZhu(ProgramSkillFrame f)
    {
        if (f.GaolingRecovery is not null && !_coreIdentityConcealmentOperations.ValidGaolingRecovery(f))
            throw new InvalidOperationException("Gaoling lost its exact actual appearance and selected recovery invoice.");
        if (f.QimeiDraw is not null)
            _coreQimeiProgramsOperations.RequireQimeiDraw(f);
        if (f.ZhuijiInitialPaid is null && f.ZhuijiDebt is null)
            return;
        if (!_coreZhuijiProgramsOperations.ValidZhuijiIssued(f))
            throw new InvalidOperationException("Zhuiji lost its actual source, phase and unique issued grant.");
        if (f.ZhuijiDebt is not { } r)
            return;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhuijiPlayDebtConsumedEvent>().Where(e => e.Grant == r.Grant).ToArray();
        if (!Enum.IsDefined(r.Stage) || r.Stage == ZhuijiDebtStage.ChoosingDiscard && facts.Length != 0 || r.Stage != ZhuijiDebtStage.ChoosingDiscard && (facts is not [var paid] || paid.ProgramFrameId != f.Id || !paid.Paid))
            throw new InvalidOperationException("Zhuiji debt lost its exact once-paid ending invoice.");
        if (r.Stage == ZhuijiDebtStage.DiscardChildren && !_coreZhuijiProgramsOperations.IsZhuijiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), new(f.OwnerSeat, 0, null)))
            throw new InvalidOperationException("Zhuiji ending discard lost its actual native physical movements.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 430)]
    internal void AssertRegisteredIssuedXuanGongZhu(ProgramSkillFrame frame)
    {
        if (frame.ZhuijiInitialPaid is not null || frame.ZhuijiDebt is not null || frame.GaolingRecovery is not null)
            RequireIssuedXuanGongZhu(frame);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 370)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 370)]
    internal bool CanContinueIssuedXuanGongZhu(ProgramSkillFrame f) => _coreQimeiProgramsOperations.ValidQimeiDraw(f) || _coreZhuijiProgramsOperations.ValidZhuijiIssued(f) || _coreIdentityConcealmentOperations.ValidGaolingRecovery(f);
    internal bool XuanGongZhuFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (_coreIdentityConcealmentOperations.ValidGaolingRecovery(root) && root.GaolingRecovery is { } g)
        {
            if (child is HpChangedTriggerWindowFrame gaolingHp)
                return gaolingHp.Change.ParentFrameId == root.Id && gaolingHp.ResumeFrameId == root.Id && gaolingHp.Continuation == PostEventContinuation.Program && gaolingHp.Change.Kind == HpChangeKind.Recovery && gaolingHp.Change.SourceSeat == root.OwnerSeat && gaolingHp.Change.TargetSeat == g.TargetSeat && gaolingHp.Change.Amount == 1;
            if (child is RecoveryReplacementFrame gaolingRecovery && _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(root)is { } gaolingEffect && gaolingEffect.CompiledInstruction is RecoverProgramInstruction gaolingInstruction)
                return gaolingRecovery.Attempt.TargetSeat == g.TargetSeat && gaolingRecovery.Attempt.Amount == 1 && _coreTypedRecoveryDamageCursorOperations.DirectProgramRecoveryReturnMatches(root, gaolingEffect, gaolingInstruction, gaolingRecovery);
        }

        if (_coreQimeiProgramsOperations.ValidQimeiDraw(root) && root.QimeiDraw is { } q && child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > q.Before && m.Sequence <= q.After && m.To == CardLocation.Hand(q.Receipt.BeneficiarySeat) && m.Reason.Value == "program.qimei.equal-draw");
        if (!_coreZhuijiProgramsOperations.ValidZhuijiIssued(root))
            return false;
        if (root.ZhuijiDebt is { Stage: ZhuijiDebtStage.DiscardChildren } d && child is CardsMovedTriggerWindowFrame discard)
            return discard.Batch.ParentFrameId == root.Id && discard.Batch.AwaitingProgramFrameId == root.Id && discard.Batch.OriginSkillId == root.SkillId && discard.Batch.OriginSkillInstanceId == root.SkillInstanceId && discard.Batch.Movements.Count > 0 && discard.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > d.Before && m.Sequence <= d.After && m.From.OwnerSeat == root.OwnerSeat && m.To == CardLocation.DiscardPile && m.Reason.Value == "program.zhuiji.ending-discard");
        if (root.ZhuijiDebt is { Stage: ZhuijiDebtStage.HpChildren } && child is DyingFrame dying)
            return dying.ParentFrameId == root.Id && dying.VictimSeat == root.OwnerSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Any(e => e.FrameId == root.Id && e.TargetSeat == root.OwnerSeat && e.Amount == 1 && e.RemainingHp == 0);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1 && (root.ZhuijiInitialPaid is { RecoverThenDiscard: true } && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat || root.ZhuijiDebt is { Stage: ZhuijiDebtStage.HpChildren } && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null);
        if (root.ZhuijiInitialPaid is { RecoverThenDiscard: true } && child is RecoveryReplacementFrame recovery && _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(root)is { } effect && effect.CompiledInstruction is RecoverProgramInstruction instruction)
            return recovery.Attempt.Amount == 1 && recovery.Attempt.TargetSeat == root.OwnerSeat && _coreTypedRecoveryDamageCursorOperations.DirectProgramRecoveryReturnMatches(root, effect, instruction, recovery);
        if (root.ZhuijiInitialPaid is { RecoverThenDiscard: false } && child is CardsMovedTriggerWindowFrame draw && _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(root)is { Op: SkillProgramEffectOp.Draw, Amount: 2 })
            return draw.Batch.ParentFrameId == root.Id && draw.Batch.OriginOwnerSeat == root.OwnerSeat && draw.Batch.OriginSkillId == root.SkillId && draw.Batch.OriginSkillInstanceId == root.SkillInstanceId && draw.Batch.Movements.Count is> 0 and <= 2 && draw.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m));
        return false;
    }

    internal ProgramSkillFrame? XuanGongZhuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterXuanGongZhuOlOperations.XuanGongZhuFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1550)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1660)]
    internal bool HasXuanGongZhuDamageObserver(long id) => XuanGongZhuObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 210)]
    internal bool IsXuanGongZhuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && XuanGongZhuObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 420)]
    internal bool TryAdvanceXuanGongZhuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XuanGongZhuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    SkillProgramStepOutcome global::CardGame.Core.IXuanGongZhuProgramHost.ArmQimeiPair(ProgramSkillFrame f) => _coreQimeiProgramsOperations.ArmQimeiPair(f);
    SkillProgramStepOutcome global::CardGame.Core.IXuanGongZhuProgramHost.SettleQimeiPairEquality(ProgramSkillFrame f) => _coreQimeiProgramsOperations.SettleQimeiPairEquality(f);
    SkillProgramStepOutcome global::CardGame.Core.IXuanGongZhuProgramHost.OfferZhuijiActualPlayChoice(ProgramSkillFrame f) => _coreZhuijiProgramsOperations.ArmZhuijiActualPlay(f);
    SkillProgramStepOutcome global::CardGame.Core.IXuanGongZhuProgramHost.SettleZhuijiActualPlayDebt(ProgramSkillFrame f) => _coreZhuijiProgramsOperations.SettleZhuijiActualPlayDebt(f);
    SkillProgramStepOutcome global::CardGame.Core.IXuanGongZhuProgramHost.AfterActualIdentityAppearanceRecover(ProgramSkillFrame f) => _coreIdentityConcealmentOperations.ArmGaolingRecovery(f);
}
