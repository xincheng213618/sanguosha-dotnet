#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuBanOlRuntime
{
    internal bool IsHuiyunExactHandQuery(CharacterState actor, int? id)
    {
        if (!HasHuBanCapability || id is not { } card || card <= 0 || !actor.IsAlive || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huiyun: { } r } f || r.Stage is not (HuiyunStage.HandUseChoice or HuiyunStage.Issuing) || r.Index < 0 || r.Index >= r.Opportunities.Count || HuiyunCurrent(r).TargetSeat != actor.Seat || r.Option is not (1 or 2) || !_corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(c => c.Id == card))
            return false;
        if (r.Stage == HuiyunStage.Issuing)
            return r.Native is { UseId: 0, ActionId: 0 } n && n.ProgramFrameId == f.Id && n.OpportunityIndex == r.Index && n.ActorSeat == actor.Seat && n.CardId == card;
        return r.Stage == HuiyunStage.HandUseChoice && (r.Option == 2 || HuiyunCurrent(r).ShownCardId == card);
    }

    internal IReadOnlyList<LegalAction> HuiyunActions(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        var o = HuiyunCurrent(r);
        if (r.Stage != HuiyunStage.HandUseChoice || r.Option == 3 || !_coreMatchParticipantsPlayersAccess._players[o.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var ids = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[o.TargetSeat]).Where(c => r.Option == 2 || c.Id == o.ShownCardId).Select(c => c.Id).ToArray();
        return Array.AsReadOnly(ids.SelectMany(id => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[o.TargetSeat], includeProgramActions: false, selectedPhysicalCardId: id)).Where(a => a.CardId is { } id && ids.Contains(id) && a.ConversionSource is null && (a.AdditionalConversionSources?.Count ?? 0) == 0 && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral)).Select(HuiyunFreeze.Action).ToArray());
    }

    internal IReadOnlyList<int> HuiyunInitialTargets(HuiyunNativeInvoice n)
    {
        var a = n.Plan;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        var card = _coreCardAttackStateOperations.GetAttackCard(n.CardId);
        var kind = n.Kind;
        if (a.Kind == LegalActionKind.Peach)
            return[n.ActorSeat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(kind, n.ActorSeat, a.TargetSeats);
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(n.ActorSeat, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, card.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    internal void BindHuiyunFollowupUse(CardUseFrame use)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { Huiyun: { Stage: HuiyunStage.Issuing, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        if (use.Action is not { Type: CardActionType.Use } a || n.ProgramFrameId != f.Id || n.OpportunityIndex != r.Index || n.ActorSeat != HuiyunCurrent(r).TargetSeat || n.Source != r.Origin.Source || n.GameplayHash != f.GameplayHash || use.SourceSeat != n.ActorSeat || use.CardId != n.CardId || use.CardKind != n.Kind || use.PhysicalCardIds is not [var physical] || physical != n.CardId || a.ActorSeat != n.ActorSeat || a.ProviderSeat != n.ActorSeat || a.RequesterSeat is not null || a.PhysicalCards is not [var cost] || cost.CardId != n.CardId || cost.From != CardLocation.Hand(n.ActorSeat) || a.ConversionChain.Count != 0 || !use.TargetSeats.SequenceEqual(HuiyunInitialTargets(n)))
            throw new InvalidOperationException("Huiyun follow-up changed its consenting target's one natural original Hand entity, native targets or actual provider.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Huiyun = r with { Stage = HuiyunStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { HuiyunFollowup = bound, UnlimitedUse = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) || u.UnlimitedUse });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunNativeIssuedEvent(bound));
    }

    internal bool SameHuiyunNative(HuiyunNativeInvoice a, HuiyunNativeInvoice b) => a.ProgramFrameId == b.ProgramFrameId && a.OpportunityIndex == b.OpportunityIndex && a.Source == b.Source && a.GameplayHash == b.GameplayHash && a.ActorSeat == b.ActorSeat && a.CardId == b.CardId && a.Kind == b.Kind && a.BeforePayment == b.BeforePayment && a.UseId == b.UseId && a.ActionId == b.ActionId && System.Text.Json.JsonSerializer.Serialize(a.Plan) == System.Text.Json.JsonSerializer.Serialize(b.Plan);
    internal bool HuiyunNativeUseRidesOn(CardUseFrame use, ProgramSkillFrame f)
    {
        if (use.HuiyunFollowup is not { } n || f.Huiyun is not { Stage: HuiyunStage.NativeUse, Native: { } pending } r || !SameHuiyunNative(n, pending) || r.NativeCompleted || n.ProgramFrameId != f.Id || n.OpportunityIndex != r.Index || n.ActorSeat != HuiyunCurrent(r).TargetSeat || n.Source != r.Origin.Source || n.GameplayHash != f.GameplayHash || use.Id != n.UseId || use.CardId != n.CardId || use.Action is not { Type: CardActionType.Use } current || current.ActionId != n.ActionId || current.ProviderSeat != n.ActorSeat || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, n.ActorSeat, n.ActorSeat) || use.PhysicalCardIds is not [var id] || id != n.CardId || _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunNativeIssuedEvent>().Count(e => SameHuiyunNative(e.Invoice, n)) != 1)
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }

        return a.EffectiveKind == n.Kind && a.PhysicalCards is [{ CardId: var cost, From: { Zone: CardZoneKind.Hand, OwnerSeat: var owner } }] && cost == n.CardId && owner == n.ActorSeat && a.ConversionChain.Count == 0;
    }

    internal bool HasIssuedHuiyunFollowup(long? id, int actorSeat) => id is { } native && _coreCardUseLifecycleOperations.LifecycleCardUse(native)is { HuiyunFollowup: { } n } use && n.ActorSeat == actorSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.ProgramFrameId)is { } parent && HuiyunNativeUseRidesOn(use, parent);
    [NativeUseValidation(27380)]
    internal void ValidateHuiyunNativeUse(CardUseFrame use)
    {
        if (use.HuiyunFollowup is { } n && (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.ProgramFrameId)is not { } f || !HuiyunNativeUseRidesOn(use, f)))
            throw new InvalidOperationException("Huiyun lost the genuine natural-card child and exact once-issued owning native return.");
    }

    [NativeUseReturn(27380)]
    internal void ReturnHuiyunNativeUse(CardUseFrame use)
    {
        if (use.HuiyunFollowup is not { } n)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.ProgramFrameId);
        var r = f.Huiyun!;
        if (r.Stage != HuiyunStage.NativeUse || r.NativeCompleted || r.Native is null || !SameHuiyunNative(r.Native, n) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunNativeReturnedEvent>().Any(e => e.Invoice.UseId == n.UseId))
            throw new InvalidOperationException("Huiyun cannot return or pay its natural-card use twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { NativeCompleted = true } });
        if (use.CardAttack is { Active: true })
            return;
        SettleHuiyunNative(f);
    }

    internal void SettleHuiyunNative(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        if (!r.NativeCompleted || r.Native is not { } n || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null)
            throw new InvalidOperationException("Huiyun must wait for the whole genuine native use and attack tail.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Huiyun = r with { Stage = HuiyunStage.NativeChildren, Before = n.BeforePayment, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(n.ActorSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunNativeReturnedEvent(n, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27380)]
    internal bool ReturnHuiyunAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huiyun: { Stage: HuiyunStage.NativeUse, NativeCompleted: true, Native: { } n } } f || n.UseId != completion.ResolutionId)
            return false;
        SettleHuiyunNative(f);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27381)]
    internal void ExpireHuiyunAlcohol(IGameEvent fact)
    {
        if (fact is not ActualTurnCursorReturnedEvent ended || !HasHuBanCapability)
            return;
        foreach (var actor in _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunNativeIssuedEvent>().Select(e => e.Invoice).Where(n => n.Plan.Kind == LegalActionKind.Alcohol && _coreProgramEventHistoryOperations.ProgramEventHistory<JianjiOrdinaryAlcoholPaidEvent>().Any(p => p.NativeUseId == n.UseId && p.ActualTurn == ended.ActualTurn)).Select(n => n.ActorSeat).Distinct().ToArray())
            if (_coreMatchParticipantsPlayersAccess._players[actor].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[actor].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(actor));
            }
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 40)]
    internal bool HasIssuedHuiyunSlashDebitExemption(long useId, int actorSeat) => HasIssuedHuiyunFollowup(useId, actorSeat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 100)]
    internal bool HuiyunSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => IsHuiyunExactHandQuery(query.Actor, query.PhysicalCardIds is [var huiyunCardId] ? huiyunCardId : query.PhysicalCard?.Id);
}
