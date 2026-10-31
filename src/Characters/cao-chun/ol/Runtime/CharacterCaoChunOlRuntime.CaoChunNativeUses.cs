#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoChunOlRuntime
{
    internal CardLocation CaoChunNativeCostDestination(CardActionContext action, CardActionCost cost) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), cost.From, _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId).IsGeneralWeapon && cost.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId) ? CardLocation.DrawPile : CardLocation.Processing);
    internal void BindCaoChunNativeUse(CardUseFrame use)
    {
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        var parent = i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame { CaoChun: { Stage: CaoChunStage.NativeSlash } } direct ? direct : _characterLiuFengBoundaryOperations.BoundaryLiuFengNativeParent(use);
        if (parent is not { CaoChun: { Stage: CaoChunStage.NativeSlash, Slash: { } n } r })
            return;
        if (n.NativeUseId != 0)
        {
            if (n.NativeUseId != use.Id || use.CaoChunSlash != n)
                throw new InvalidOperationException("Shanjia cannot bind another native Slash.");
            return;
        }

        if (use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != parent.OwnerSeat || a.ProviderSeat != (n.ProviderSeat ?? parent.OwnerSeat) || a.EffectiveKind != n.Kind || use.CardKind != n.Kind || !use.TargetSeats.SequenceEqual([n.TargetSeat]) || !a.PhysicalCards.Select(c => (c.CardId, c.CardKind, c.From)).SequenceEqual(n.Materials.Select(c => (c.CardId, c.CardKind, c.From))) || a.PhysicalCards.Count != (n.Ending ? 0 : n.Materials.Count) || n.ParentFrameId != parent.Id || n.Source != r.Source || n.GameplayHash != r.GameplayHash || n.InstructionIndex != parent.InstructionIndex || n.Ending && use.CardId != 0 || !n.Ending && n.Materials.Count == 0 && n.RoundOncePayment is null && n.DrawFundedPayment is null && n.ChainedPayment is null && !_characterGuoTuOlOperations.HasWeijieSpecializedPayment(parent, use) && !(_characterLiuZhangOlOperations.HasZonghuSpecializedPayment(parent, use) || _characterHuanShuJunOlOperations.HasHuanpeiProgramPayment(parent, use)))
            throw new InvalidOperationException("Shanjia native Slash changed its accepted issuer, exact materials or target.");
        var bound = n with
        {
            NativeUseId = use.Id,
            ActionId = a.ActionId,
            Action = a,
            Materials = a.PhysicalCards,
            PaymentAfter = n.Materials.Count == 0 ? n.PaymentBefore : null
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { CaoChun = r with { Slash = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { CaoChunSlash = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeSlashIssuedEvent(parent.Id, use.Id, a.ActionId, n.Ending, n.TargetSeat));
        if (n.Materials.Count == 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeMaterialPaidEvent(parent.Id, use.Id, a.ActionId, n.PaymentBefore, n.PaymentBefore, 0));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashPaymentCompleted, 20)]
    internal void CompleteCaoChunNativePayment(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CaoChunSlash: { } n } use)
            return;
        if (n.Materials.Count == 0)
        {
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ParentFrameId)is not ProgramSkillFrame zeroParent || !CaoChunNativeRidesOn(zeroParent, use) || n.PaymentAfter != n.PaymentBefore || !n.Ending && n.RoundOncePayment is null && n.DrawFundedPayment is null && n.ChainedPayment is null && !_characterGuoTuOlOperations.HasWeijieSpecializedPayment(zeroParent, use) && !(_characterLiuZhangOlOperations.HasZonghuSpecializedPayment(zeroParent, use) || _characterHuanShuJunOlOperations.HasHuanpeiProgramPayment(zeroParent, use)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaNativeMaterialPaidEvent>().Count(e => e.FrameId == zeroParent.Id && e.NativeUseId == id && e.ActionId == n.ActionId && e.Before == n.PaymentBefore && e.After == n.PaymentBefore && e.Count == 0) != 1 || n.RoundOncePayment is { } independent && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == independent) != 1)
                throw new InvalidOperationException("Shanjia zero-material native payment lost its exact issued independent or ending-use receipt.");
            return;
        }

        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ParentFrameId)is not ProgramSkillFrame { CaoChun: { } r } parent || n.PaymentAfter is not null || !CaoChunNativeRidesOn(parent, use))
            throw new InvalidOperationException("Shanjia native payment lost its actual bound request.");
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (n.Materials.Any(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > n.PaymentBefore && m.Sequence <= after && m.CardId == c.CardId && m.From == c.From && m.To == CaoChunNativeCostDestination(use.Action!, c) && m.Reason == CardMoveReasons.Use) != 1))
            throw new InvalidOperationException("Shanjia actual requested Slash material was not paid exactly once.");
        var paid = n with
        {
            PaymentAfter = after
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { CaoChun = r with { Slash = paid } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, f => f with { CaoChunSlash = paid });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeMaterialPaidEvent(parent.Id, id, n.ActionId, n.PaymentBefore, after, n.Materials.Count));
    }

    internal bool CaoChunNativeRidesOn(ProgramSkillFrame parent, CardUseFrame use)
    {
        if (parent.CaoChun is not { Stage: CaoChunStage.NativeSlash, Slash: { } n } r || use.CaoChunSlash != n || n.ParentFrameId != parent.Id || n.Source != r.Source || n.GameplayHash != parent.GameplayHash || n.NativeUseId != use.Id || n.ActionId != use.Action?.ActionId || n.Action is not { } original || use.Action is not { } a || original.ActionId != a.ActionId || original.ParentActionId != a.ParentActionId || original.ProviderSeat != a.ProviderSeat || !a.PhysicalCards.SequenceEqual(n.Materials) || !a.ConversionChain.SequenceEqual(original.ConversionChain) || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, parent.OwnerSeat, n.ProviderSeat ?? parent.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || n.Ending && n.Materials.Count != 0)
            return false;
        if (n.DrawFundedPayment is { } funded && (use.DrawFundedDistinctBasicUse?.Payment != funded || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(funded)))
            return false;
        if (n.ChainedPayment is { } chained && (use.ChainedStateBasicUse?.Payment != chained || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chained)))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaNativeSlashIssuedEvent>().Count(e => e.FrameId == parent.Id && e.NativeUseId == use.Id && e.ActionId == n.ActionId && e.Ending == n.Ending && e.TargetSeat == n.TargetSeat) == 1;
    }

    [NativeUseReturn(30)]
    internal bool CompleteCaoChunUseReturn(CardUseFrame use)
    {
        if (use.CaoChunSlash is null)
            return false;
        _coreDrawFundedDistinctBasicReturnsOperations.ReturnDrawFundedDistinctBasicUse(use);
        _coreChainedStateBasicReturnsOperations.ReturnChainedStateBasicUse(use);
        return true;
    }

    [NativeUseValidation(70)]
    internal void AssertCaoChunNativeUse(CardUseFrame use)
    {
        if (use.CaoChunSlash is not { } n)
            return;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ParentFrameId)is not ProgramSkillFrame parent || !CaoChunNativeRidesOn(parent, use))
            throw new InvalidOperationException("Shanjia lost its exact owned native Slash or legitimate actor replacement.");
    }

    // Native AttackCompletionReceipt is captured before its actual CardUseFrame is removed.
    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 140)]
    internal bool CompleteCaoChunSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt receipt)
    {
        if (receipt.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoChun: { Stage: CaoChunStage.NativeSlash, Slash: { } n } r } parent || parent.Id != parentId)
            return false;
        if (receipt.CaoChunSlash != n || n.NativeUseId != receipt.ResolutionId || n.Returned || n.Action is null || _coreCardUseLifecycleOperations.LifecycleCardUse(n.NativeUseId)is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == n.NativeUseId) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaNativeSlashIssuedEvent>().Count(e => e.FrameId == parent.Id && e.NativeUseId == n.NativeUseId && e.ActionId == n.ActionId) != 1)
            throw new InvalidOperationException("Shanjia Slash must finish and return once through its original native attack receipt.");
        if (n.RoundOncePayment is { } independent && (n.Action.ConversionChain is not [var actualSource] || actualSource != independent.Source || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == independent) != 1))
            throw new InvalidOperationException("Shanjia independent Slash lost its real once-round payment.");
        if (n.PaymentAfter is not { } after || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaNativeMaterialPaidEvent>().Count(e => e.FrameId == parent.Id && e.NativeUseId == n.NativeUseId && e.ActionId == n.ActionId && e.Before == n.PaymentBefore && e.After == after && e.Count == n.Materials.Count) != 1)
            throw new InvalidOperationException("Shanjia Slash return lost its exact native material invoice.");
        foreach (var cost in n.Materials)
            if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > n.PaymentBefore && m.Sequence <= after && m.CardId == cost.CardId && m.From == cost.From && m.To == CaoChunNativeCostDestination(n.Action, cost) && m.Reason == CardMoveReasons.Use) != 1)
                throw new InvalidOperationException("Shanjia requested Slash cannot return without its actual material payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeSlashReturnedEvent(parent.Id, n.NativeUseId, n.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CaoChun = r with { Slash = n with { Returned = true } } });
        ResumeCaoChun(parent.Id);
        return true;
    }

    internal bool IsCaoChunNativeEdge(int index)
    {
        if (index < 1 || index >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame use || use.CaoChunSlash is null)
            return false;
        return CaoChunNativeRidesOn(parent, use);
    }
}
