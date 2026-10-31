#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMobileGodRuntime
{
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MaterialPayment, 40600)]
    internal bool PayMobileJwReservedSlash(IReadOnlyList<Card> cards, int provider, CardLocation? source, CardMoveReason reason)
    {
        if (source != CardLocation.Processing || reason != CardMoveReasons.Use || cards is not [var card] || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MobileJiangWeiReturn: { } ret } use)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == ret.ParentId);
        if (parent is not { MobileJiangWei: { Stage: MobileJiangWeiStage.SlashIssued } r } || r.Use != ret || provider != parent.OwnerSeat || ret.CardId != card.Id || ret.Kind != card.Kind || ret.UseId != use.Id || ret.ActionId != use.Action?.ActionId || ret.From != CardLocation.Processing || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Processing || !r.Shown.Any(s => s.Id == card.Id && s.Kind == card.Kind && s.From == CardLocation.DrawPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == s.ProcessingSequence && m.CardId == card.Id && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == "mobile.xinghun.show") == 1) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileJiangWeiUseIssuedEvent>().Count(e => e.Return == ret) != 1)
            throw new InvalidOperationException("Xinghun's shown deck Slash requires its already-paid original Processing entry and exact native Use.");
        return true;
    }

    internal void NextMobileJwSlash(ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Index >= r.Shown.Count)
        {
            ReturnMobileJwShownTop(f);
            return;
        }

        var shown = r.Shown[r.Index];
        var from = shown.From == CardLocation.DrawPile ? CardLocation.Processing : shown.From;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Index = r.Index + 1 } });
        r = f.MobileJiangWei!;
        var card = _coreCardAttackStateOperations.GetAttackCard(shown.Id);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(shown.Kind) || card.Kind != shown.Kind || _cardZonesOperations._cardZones.GetLocation(card.Id) != from || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.Target], card, effectiveKind: card.Kind, physicalCardIds: [card.Id]))
        {
            NextMobileJwSlash(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.SlashIssued, Use = null, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.Target], card, card.Kind, f.OwnerSeat, countsTowardSlashLimit: false, programSkillCardUseFrameId: f.Id, physicalSourceLocation: from);
    }

    internal bool IsMobileJwXinghunSlashSource(long? parentId, int actor, int target, Card card, CardKind kind, int physicalOwner, CardLocation from, IReadOnlyList<Card>? materials, bool countsTowardLimit)
    {
        if (parentId is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: { Stage: MobileJiangWeiStage.SlashIssued, Use: null } r } f || f.Id != id || r.Operation != SkillProgramEffectOp.ResolveMobileJiangWeiXinghun || actor != f.OwnerSeat || physicalOwner != actor || target != r.Target || countsTowardLimit || materials is not null || kind != card.Kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || r.Index < 1 || r.Index > r.Shown.Count || r.Shown[r.Index - 1] is not { } shown || shown.Id != card.Id || shown.Kind != kind || from != (shown.From == CardLocation.DrawPile ? CardLocation.Processing : shown.From) || _cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            return false;
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], card, effectiveKind: kind, physicalCardIds: [card.Id]);
    }

    internal void BindMobileJwXinghunSlashUse(long useId, long? parentId)
    {
        if (parentId is not { } id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == id)is not { MobileJiangWei: { Stage: MobileJiangWeiStage.SlashIssued, Use: null } r } f)
            return;
        var shown = r.Shown[r.Index - 1];
        var from = shown.From == CardLocation.DrawPile ? CardLocation.Processing : shown.From;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { Type: CardActionType.Use, PhysicalCards: [var material], ConversionChain.Count: 0 } action } use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.RequesterSeat is not null || action.EffectiveKind != shown.Kind || !action.TargetSeats.SequenceEqual([r.Target]) || material.CardId != shown.Id || material.CardKind != shown.Kind || material.From != from || use.SourceSeat != f.OwnerSeat || use.CardId != shown.Id || use.PhysicalCardIds is not [var physical] || physical != shown.Id)
            throw new InvalidOperationException("Xinghun lost its exact public shown physical Slash and native Use declaration.");
        var ret = new MobileJiangWeiUseReturn(f.Id, f.InstructionIndex, r.Source, f.GameplayHash, useId, action.ActionId, shown.Id, shown.Kind, from, r.Target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MobileJiangWei = r with { Use = ret } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { MobileJiangWeiReturn = ret, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiangWeiUseIssuedEvent(ret));
    }

    [NativeUseValidation(40600)]
    internal void ValidateMobileJwUse(CardUseFrame use)
    {
        if (use.MobileJiangWeiReturn is not { } ret)
            return;
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == ret.ParentId)is not { MobileJiangWei: { Stage: MobileJiangWeiStage.SlashIssued } r } parent || r.Use != ret || ret.Source != MobileJwSource(parent) || ret.GameplayHash != parent.GameplayHash || ret.InstructionIndex != parent.InstructionIndex || use.Id != ret.UseId || use.Action?.ActionId != ret.ActionId || use.CardId != ret.CardId || use.CardKind != ret.Kind || use.Action.PhysicalCards is not [var c] || c.CardId != ret.CardId || c.From != ret.From || use.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileJiangWeiUseIssuedEvent>().Count(e => e.Return == ret) != 1)
            throw new InvalidOperationException("Xinghun cannot replace its whole native Slash return.");
    }

    [NativeUseReturn(40600)]
    internal void ReturnMobileJwUse(CardUseFrame use)
    {
        if (use.MobileJiangWeiReturn is not { } ret)
            return;
        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(ret.ParentId);
        var r = parent.MobileJiangWei!;
        if (r.Use != ret || r.Stage != MobileJiangWeiStage.SlashIssued || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileJiangWeiUseReturnedEvent>().Any(e => e.UseId == use.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Xinghun must wait for its once-paid physical Slash and complete native card cleanup.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent = parent with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.SlashReturned } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiangWeiUseReturnedEvent(parent.Id, use.Id, ret.ActionId));
        if (use.CardAttack is not { Active: true })
            ContinueMobileJw(parent.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 40600)]
    internal bool ReturnMobileJwAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: { Stage: MobileJiangWeiStage.SlashReturned, Use: { } ret } } f || ret.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(ret.UseId)is not null)
            throw new InvalidOperationException("Xinghun attack completed outside its original Use parent.");
        ContinueMobileJw(f.Id);
        return true;
    }
}
