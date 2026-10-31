#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhuoMouOlRuntime
{
    internal bool BeginMouDongZhuoOlWine(ProgramSkillFrame f)
    {
        if (!MouDongZhuoOl.Valid(f) || f.MouDongZhuoOl is not { Paid: true, Stage: MouDongZhuoOlStage.Wine, WineUseId: null } r)
            throw new InvalidOperationException("Fengshang Wine lost its original paid allocation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner.Seat, CardKind.Alcohol, [])) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Alcohol) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Alcohol, Suit.None) || _coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat))
            return false;
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.VictimSeat == owner.Seat && owner.Hp <= 0;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var a = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, owner.Seat, owner.Seat, null, null, null, CardKind.Alcohol, [owner.Seat], [], [r.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDongZhuoOl = r with { WineUseId = id } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, owner.Seat, 0, CardKind.Alcohol, [owner.Seat], PhysicalCardIds: []) { Action = a, MouDongZhuoWine = new(f.Id, f.InstructionIndex, r.Source, r.GameplayHash, dying) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlWineIssuedEvent(f.Id, id, a.ActionId, dying));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(a));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(a));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Alcohol, owner.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [owner.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, owner.Seat, CardKind.Alcohol);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(owner.Seat, CardKind.Alcohol);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(a);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, dying ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol));
        return true;
    }

    internal bool IsMouDongZhuoOlWine(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, MouDongZhuoWine: { } r, Action: { Type: CardActionType.Use } a } u || a.ActorSeat != u.SourceSeat || a.ProviderSeat != u.SourceSeat || a.EffectiveKind != CardKind.Alcohol || a.PhysicalCards.Count != 0 || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || !a.ConversionChain.SequenceEqual([r.Source]))
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1] is not ProgramSkillFrame { MouDongZhuoOl: { Paid: true, Stage: MouDongZhuoOlStage.Wine } p } f || f.Id != r.ParentId || f.InstructionIndex != r.Instruction || p.WineUseId != id || p.Source != r.Source || p.GameplayHash != r.Hash || !MouDongZhuoOl.Valid(f))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhuoOlWineIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == id && e.ActionId == a.ActionId && e.SelfDying == r.SelfDying) == 1;
    }

    internal bool ContinueMouDongZhuoOlWine(long id, ProgramSimpleCardContinuation c)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouDongZhuoWine is null)
            return false;
        if (c.CardId != 0 || !IsMouDongZhuoOlWine(id))
            throw new InvalidOperationException("Fengshang Wine lost its exact real Use.");
        var u = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        var card = new Card(0, CardKind.Alcohol, Suit.None, 0);
        if (u.MouDongZhuoWine!.SelfDying && c.Effect == SimpleCardUseEffect.Recovery)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], _coreMatchParticipantsPlayersAccess._players[u.SourceSeat], card, id, CardKind.Alcohol, c.RecoveryAmount, c.RecoveryPolicySources ?? []);
        else if (!u.MouDongZhuoWine.SelfDying && c.Effect == SimpleCardUseEffect.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], card, id);
        else
            throw new InvalidOperationException("Fengshang changed Wine's actual self-rescue mode.");
        return true;
    }

    [NativeUseValidation(34700)]
    internal void ValidateMouDongZhuoOlWine(CardUseFrame u)
    {
        if (u.MouDongZhuoWine is not null && (!IsMouDongZhuoOlWine(u.Id) || u.Step != ResolutionFrameStep.Completed || u.PendingRecoveryAttempts is { Count: > 0 } || u.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("Fengshang Wine cannot return before its native use and recovery children.");
    }

    [NativeUseReturn(34700)]
    internal void ReturnMouDongZhuoOlWine(CardUseFrame u)
    {
        if (u.MouDongZhuoWine is not { } r)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDongZhuoOl: { WineReturned: false } p } f || f.Id != r.ParentId || p.WineUseId != u.Id || u.Action is not { } a)
            throw new InvalidOperationException("Fengshang Wine lost its exact typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDongZhuoOl = p with { Stage = MouDongZhuoOlStage.WineChildren, WineReturned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlWineReturnedEvent(f.Id, u.Id, a.ActionId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
