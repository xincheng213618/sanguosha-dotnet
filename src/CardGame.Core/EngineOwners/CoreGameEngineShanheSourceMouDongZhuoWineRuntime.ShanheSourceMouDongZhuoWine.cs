#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoWineRuntime
{
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 62054)]
    internal bool SourceMouWineQuotaExemption(long useId, int actorSeat) => _coreShanheSourceMouDongZhuoOperations.IsSourceMouMode && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.SourceSeat == actorSeat && IsShanheSourceMouWine(useId);
    internal bool BeginShanheSourceMouWine(ProgramSkillFrame f)
    {
        if (!_coreShanheSourceMouDongZhuoOperations.ValidSourceMou(f) || f.ShanheSourceMou is not { Paid: true, Stage: ShanheSourceMouStage.Wine, WineUseId: null } r)
            throw new InvalidOperationException("Source Fengshang wine lost its original paid allocation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner.Seat, CardKind.Alcohol, [])) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, owner.Seat, CardKind.Alcohol) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Alcohol, Suit.None) || _coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat))
            return false;
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.VictimSeat == owner.Seat && owner.Hp <= 0;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, owner.Seat, owner.Seat, null, null, null, CardKind.Alcohol, [owner.Seat], [], [r.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceMou = r with { WineUseId = id } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, owner.Seat, 0, CardKind.Alcohol, [owner.Seat], PhysicalCardIds: []) { Action = action, ShanheSourceMouWine = new(f.Id, f.InstructionIndex, r.Source, r.Hash, dying) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouWineIssuedEvent(f.Id, id, action.ActionId, dying));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Alcohol, owner.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [owner.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, owner.Seat, CardKind.Alcohol);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(owner.Seat, CardKind.Alcohol);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, dying ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol));
        return true;
    }

    internal bool IsShanheSourceMouWine(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, ShanheSourceMouWine: { } r, Action: { Type: CardActionType.Use } action } use || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.EffectiveKind != CardKind.Alcohol || action.PhysicalCards.Count != 0 || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || !action.ConversionChain.SequenceEqual([r.Source]))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { ShanheSourceMou: { Paid: true, Stage: ShanheSourceMouStage.Wine } paid } f && f.Id == r.ParentId && f.InstructionIndex == r.Instruction && paid.WineUseId == id && paid.Source == r.Source && paid.Hash == r.Hash && _coreShanheSourceMouDongZhuoOperations.ValidSourceMou(f) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceMouWineIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == id && e.ActionId == action.ActionId && e.SelfDying == r.SelfDying) == 1;
    }

    internal bool ContinueShanheSourceMouWine(long id, ProgramSimpleCardContinuation continuation)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.ShanheSourceMouWine is null)
            return false;
        if (continuation.CardId != 0 || !IsShanheSourceMouWine(id))
            throw new InvalidOperationException("Source Fengshang wine lost its actual Use.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        var card = new Card(0, CardKind.Alcohol, Suit.None, 0);
        if (use.ShanheSourceMouWine!.SelfDying && continuation.Effect == SimpleCardUseEffect.Recovery)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[use.SourceSeat], card, id, CardKind.Alcohol, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (!use.ShanheSourceMouWine.SelfDying && continuation.Effect == SimpleCardUseEffect.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], card, id);
        else
            throw new InvalidOperationException("Source Fengshang changed wine's actual self-rescue mode.");
        return true;
    }

    [NativeUseValidation(62054)]
    internal void ValidateSourceMouWine(CardUseFrame use)
    {
        if (use.ShanheSourceMouWine is not null && (!IsShanheSourceMouWine(use.Id) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("Source Fengshang wine cannot return before its original recovery children.");
    }

    [NativeUseReturn(62054)]
    internal void ReturnSourceMouWine(CardUseFrame use)
    {
        if (use.ShanheSourceMouWine is not { } returned)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceMou: { WineReturned: false } r } f || f.Id != returned.ParentId || r.WineUseId != use.Id || use.Action is not { } action)
            throw new InvalidOperationException("Source Fengshang wine lost its exact typed parent return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.WineChildren, WineReturned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouWineReturnedEvent(f.Id, use.Id, action.ActionId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
