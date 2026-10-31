#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    internal bool QigongPaidAdapterChild(ProgramSkillFrame f, ResolutionFrame child) => child switch
    {
        DrawFundedDistinctBasicFrame draw => QigongAdapterActor(f, draw.Payment.Source.OwnerSeat, draw.Payment.TargetSeat, draw.Payment.Cursor, draw.Payment.ParentFrameId, draw.Payment.RequestFrameId) && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(draw.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicPaidEvent>().Count(e => e.PaymentFrameId == draw.Id) == 1,
        ChainedStateBasicFrame chain => QigongAdapterActor(f, chain.Payment.Source.OwnerSeat, chain.Payment.TargetSeat, chain.Payment.Cursor, chain.Payment.ParentFrameId, chain.Payment.RequestFrameId) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chain.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicPaidEvent>().Count(e => e.Payment == chain.Payment) == 1,
        _ => false
    };

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 320)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 320)]
    internal bool CanContinueLvKuangLvXiang(ProgramSkillFrame f)
    {
        if (f.LvKuangLvXiang is not { } r)
            return false;
        var paid = r.Payments.Count > 0 || r.Native is { Paid: true } || r.Declaration is not null && _resolutionStackOperations._resolutionStack.OfType<CardDeclarationFrame>().Any(d => MatchesQigongDeclaration(f, d)) || _resolutionStackOperations._resolutionStack.Any(child => QigongPaidAdapterChild(f, child));
        if (!paid)
            return false;
        return true;
    }

    internal bool LvKuangLvXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LvKuangLvXiang is not { } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is CardUseFrame use)
            return QigongCardUseRidesOn(use, f);
        if (IsQigongDeclarationChild(f, child) || QigongPaidAdapterChild(f, child))
            return true;
        if (!CanContinueLvKuangLvXiang(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && r.Payments.LastOrDefault()is { } p)
            return moved.Batch.Id == p.BatchId && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.Hand(p.Recipient) && m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == LiehouReason;
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool LvKuangLvXiangObserverSuffix(int index)
    {
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is DrawFundedDistinctBasicFrame)
            return _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicPaidSuffix(index + 1);
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is ChainedStateBasicFrame)
            return _coreChainedStateBasicReturnsOperations.ChainedStateBasicPaidSuffix(index + 1);
        return _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || this.QigongDeclarationObserverEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i, d)) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, d));
    }

    internal ProgramSkillFrame? LvKuangLvXiangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LvKuangLvXiangFirstChild(f, c), (i) => this.LvKuangLvXiangObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 390)]
    internal bool TryAdvanceLvKuangLvXiangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LvKuangLvXiangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
