#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoChunOlRuntime
{
    internal CardLocation CaoChunNativeCostDestination(CardActionContext action, CardActionCost cost) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), cost.From, _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId).IsGeneralWeapon && cost.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId) ? CardLocation.DrawPile : CardLocation.Processing);
    internal void BindCaoChunNativeUse(CardUseFrame use)
    {
        ProgramSkillFrame? parent = null;
        ProgramSlashRequestAcceptance? acceptance = null;
        if (use.ProgramSlashRequestExecution is { } execution)
        {
            parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == execution.ProgramFrameId) as ProgramSkillFrame;
            if (parent?.CaoChun is not { } receipt)
                return;
            acceptance = execution.Acceptance;
            if (!_slashRequests.OwnsAcceptance(parent, acceptance, parent.OwnerSeat) ||
                acceptance.Identity.Source != receipt.Source || acceptance.Identity.GameplayHash != receipt.GameplayHash)
                throw new InvalidOperationException("Shanjia native use lost its original accepted request.");
        }
        else
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
            if (index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { CaoChun.Slash.Ending: true } ending)
                parent = ending;
        }
        if (parent?.CaoChun is not { } r || use.Action is not { Type: CardActionType.Use } action)
            return;
        if (r.Slash is { NativeUseId: > 0 })
            return;
        var isEnding = acceptance is null && r.Slash is { Ending: true };
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || isEnding && action.PhysicalCards.Count != 0)
            throw new InvalidOperationException("Shanjia native use changed its real Slash or ending-use materials.");
        var before = r.Slash?.PaymentBefore ?? _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (use.AcceptedDeclarationPayment is { } declaration && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move =>
            move.CardId == declaration.Cost.CardId && move.From == declaration.Cost.From && move.To == CardLocation.Processing && move.Reason.Value == "conversion.declaration.pay") is { } declarationDeparture)
            before = Math.Min(before, declarationDeparture.Sequence - 1);

        var bound = new CaoChunNativeSlash(parent.Id, r.Source, r.GameplayHash, r.InstructionIndex,
            r.ActualTurn, r.PhaseInstance, isEnding, acceptance?.TargetSeat ?? r.Slash!.TargetSeat, action.EffectiveKind,
            use.Id, action.ActionId)
        {
            Action = action, Materials = action.PhysicalCards, ProviderSeat = action.ProviderSeat, DeclarationPayment = use.AcceptedDeclarationPayment,
            PaymentBefore = before, PaymentAfter = action.PhysicalCards.Count == 0 ? before : null,
            RoundOncePayment = r.Slash?.RoundOncePayment ?? use.RoundOnceCardlessBasicUse?.Payment,
            DrawFundedPayment = use.DrawFundedDistinctBasicUse?.Payment ?? r.Slash?.DrawFundedPayment,
            ChainedPayment = use.ChainedStateBasicUse?.Payment ?? r.Slash?.ChainedPayment
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { CaoChunSlash = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeSlashIssuedEvent(parent.Id, use.Id, action.ActionId, isEnding, bound.TargetSeat));
        if (action.PhysicalCards.Count == 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeMaterialPaidEvent(parent.Id, use.Id, action.ActionId, before, before, 0));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashPaymentCompleted, 20)]
    internal void CompleteCaoChunNativePayment(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CaoChunSlash: { } n } use)
            return;
        if (n.Materials.Count == 0)
        {
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ParentFrameId) is not ProgramSkillFrame zeroParent || !CaoChunNativeRidesOn(zeroParent, use) || n.PaymentAfter != n.PaymentBefore)
                throw new InvalidOperationException("Shanjia zero-material use lost its original native return.");
            return;
        }

        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.ParentFrameId)is not ProgramSkillFrame { CaoChun: { } r } parent || n.PaymentAfter is not null || !CaoChunNativeRidesOn(parent, use))
            throw new InvalidOperationException("Shanjia native payment lost its actual bound request.");
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (n.Materials.Any(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > n.PaymentBefore && m.Sequence <= after && m.CardId == c.CardId && m.From == c.From && (m.To == CaoChunNativeCostDestination(use.Action!, c) && m.Reason == CardMoveReasons.Use || n.DeclarationPayment is { } declaration && declaration.Cost == c && declaration.ProviderSeat == n.ProviderSeat && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay")) != 1))
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
        if (parent.CaoChun is not { Stage: CaoChunStage.NativeSlash, Slash: { } n } r || use.CaoChunSlash != n ||
            n.ParentFrameId != parent.Id || n.Source != r.Source || n.GameplayHash != parent.GameplayHash || n.NativeUseId != use.Id ||
            n.ActionId != use.Action?.ActionId || n.Action is not { } original || use.Action is not { Type: CardActionType.Use } action ||
            original.ActionId != action.ActionId || original.ParentActionId != action.ParentActionId || original.ProviderSeat != action.ProviderSeat ||
            !action.PhysicalCards.SequenceEqual(n.Materials) || !action.ConversionChain.SequenceEqual(original.ConversionChain) ||
            !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || n.Ending && n.Materials.Count != 0)
            return false;
        if (n.Ending)
            return use.ProgramSlashRequestExecution is null;
        return use.ProgramSlashRequestExecution is { } execution && execution.ProgramFrameId == parent.Id && execution.UseFrameId == use.Id &&
            execution.CardActionId == action.ActionId && execution.Acceptance.Identity.Source == n.Source &&
            _slashRequests.OwnsAcceptance(parent, execution.Acceptance, parent.OwnerSeat, n.TargetSeat);
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
    internal bool CompleteCaoChunSlash(GameEngine.AttackCompletionReceipt receipt)
    {
        if (receipt.CaoChunSlash is not { } returned || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoChun: { Stage: CaoChunStage.NativeSlash, Slash: { } n } r } parent || parent.Id != returned.ParentFrameId)
            return false;
        if (receipt.CaoChunSlash != n || n.NativeUseId != receipt.ResolutionId || n.Returned || n.Action is null || _coreCardUseLifecycleOperations.LifecycleCardUse(n.NativeUseId)is not null)
            throw new InvalidOperationException("Shanjia Slash must finish and return once through its original native attack receipt.");
        if (n.PaymentAfter is not { } after)
            throw new InvalidOperationException("Shanjia Slash return lost its exact native material invoice.");
        foreach (var cost in n.Materials)
            if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > n.PaymentBefore && m.Sequence <= after && m.CardId == cost.CardId && m.From == cost.From && (m.To == CaoChunNativeCostDestination(n.Action, cost) && m.Reason == CardMoveReasons.Use || n.DeclarationPayment is { } declaration && declaration.Cost == cost && declaration.ProviderSeat == n.ProviderSeat && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay")) != 1)
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
