namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 62054)]
    private bool SourceMouWineQuotaExemption(long useId, int actorSeat) =>
        IsSourceMouMode && LifecycleCardUse(useId)?.SourceSeat == actorSeat && IsShanheSourceMouWine(useId);

    private bool BeginShanheSourceMouWine(ProgramSkillFrame f)
    {
        if (!ValidSourceMou(f) || f.ShanheSourceMou is not { Paid: true, Stage: ShanheSourceMouStage.Wine, WineUseId: null } r)
            throw new InvalidOperationException("Source Fengshang wine lost its original paid allocation.");
        var owner = _players[f.OwnerSeat];
        if (!owner.IsAlive || _winner != Winner.None || IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use) ||
            !TargetRules.AllowsNativeUseMaterial(new(owner.Seat, CardKind.Alcohol, [])) ||
            IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Alcohol) ||
            IsCardTargetProhibited(owner, CardKind.Alcohol, Suit.None) || HasSelfCardTargetProhibition(owner.Seat)) return false;
        var dying = ActiveDying is { } d && d.VictimSeat == owner.Seat && owner.Hp <= 0;
        var id = ++_resolutionSequence;
        var action = CaptureFactionAction(new CardActionContext(++_cardActionSequence,
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use,
            owner.Seat, owner.Seat, null, null, null, CardKind.Alcohol, [owner.Seat], [], [r.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        ReplaceRuntimeTop(f with { ShanheSourceMou = r with { WineUseId = id } });
        PushRuntimeFrame(new CardUseFrame(id, owner.Seat, 0, CardKind.Alcohol, [owner.Seat], PhysicalCardIds: [])
        { Action = action, ShanheSourceMouWine = new(f.Id, f.InstructionIndex, r.Source, r.Hash, dying) });
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouWineIssuedEvent(f.Id, id, action.ActionId, dying));
        AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (TracksPlayCardHistory) AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Alcohol, owner.Seat));
        AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [owner.Seat]));
        RecordYingboCardUse(id, owner.Seat, CardKind.Alcohol); RecordProgramUsedBasicCard(owner.Seat, CardKind.Alcohol); RecordActualPlayPhaseUse(action);
        BeginSimpleCardUse(id, new(0, dying ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol)); return true;
    }
    private bool IsShanheSourceMouWine(long id)
    {
        if (LifecycleCardUse(id) is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0,
                ShanheSourceMouWine: { } r, Action: { Type: CardActionType.Use } action } use ||
            action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.EffectiveKind != CardKind.Alcohol ||
            action.PhysicalCards.Count != 0 || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || !action.ConversionChain.SequenceEqual([r.Source])) return false;
        var index = _resolutionStack.FindIndex(frame => frame.Id == id);
        return index > 0 && _resolutionStack[index - 1] is ProgramSkillFrame { ShanheSourceMou: { Paid: true, Stage: ShanheSourceMouStage.Wine } paid } f &&
            f.Id == r.ParentId && f.InstructionIndex == r.Instruction && paid.WineUseId == id && paid.Source == r.Source && paid.Hash == r.Hash &&
            ValidSourceMou(f) && ProgramEventHistory<ShanheSourceMouWineIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == id && e.ActionId == action.ActionId && e.SelfDying == r.SelfDying) == 1;
    }
    private bool ContinueShanheSourceMouWine(long id, ProgramSimpleCardContinuation continuation)
    {
        if (LifecycleCardUse(id)?.ShanheSourceMouWine is null) return false;
        if (continuation.CardId != 0 || !IsShanheSourceMouWine(id)) throw new InvalidOperationException("Source Fengshang wine lost its actual Use.");
        var use = LifecycleCardUse(id)!; SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        var card = new Card(0, CardKind.Alcohol, Suit.None, 0);
        if (use.ShanheSourceMouWine!.SelfDying && continuation.Effect == SimpleCardUseEffect.Recovery)
            CompleteRecoveryCardUse(_players[use.SourceSeat], _players[use.SourceSeat], card, id, CardKind.Alcohol, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (!use.ShanheSourceMouWine.SelfDying && continuation.Effect == SimpleCardUseEffect.Alcohol) CompleteAlcoholUse(_players[use.SourceSeat], card, id);
        else throw new InvalidOperationException("Source Fengshang changed wine's actual self-rescue mode.");
        return true;
    }
    [NativeUseValidation(62054)]
    private void ValidateSourceMouWine(CardUseFrame use)
    { if (use.ShanheSourceMouWine is not null && (!IsShanheSourceMouWine(use.Id) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null)) throw new InvalidOperationException("Source Fengshang wine cannot return before its original recovery children."); }
    [NativeUseReturn(62054)]
    private void ReturnSourceMouWine(CardUseFrame use)
    {
        if (use.ShanheSourceMouWine is not { } returned) return;
        if (LifecycleCardUse(use.Id) is not null || _resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceMou: { WineReturned: false } r } f ||
            f.Id != returned.ParentId || r.WineUseId != use.Id || use.Action is not { } action)
            throw new InvalidOperationException("Source Fengshang wine lost its exact typed parent return.");
        ReplaceRuntimeTop(f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.WineChildren, WineReturned = true } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouWineReturnedEvent(f.Id, use.Id, action.ActionId)); AdvanceRuntimeProgram(f.Id);
    }
}
