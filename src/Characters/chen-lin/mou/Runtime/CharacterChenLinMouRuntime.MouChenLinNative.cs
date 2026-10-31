#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenLinMouRuntime
{
    internal MouChenLinUsePlan? MouChenLinPlan(ProgramSkillFrame f, int id, int target)
    {
        if (!ValidMouChenLin(f) || f.MouChenLin is not { } r || !r.Cards.Any(c => c.Id == id) || r.Used.Contains(id) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing)
            return null;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var kind = card.Kind;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
        var color = global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
        if (!MouChenLinRuntime.DamageCard(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [id]) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, kind, [id], r.Source)) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [id], suit))
            return null;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], card, effectiveKind: kind, physicalCardIds: [id], conversionSource: r.Source) ? new(id, kind, LegalActionKind.Slash, null)
            {
                EligibleTargets = [target]
            }

            : null;
        if (kind == CardKind.Lightning)
        {
            var t = _coreMatchParticipantsPlayersAccess._players[target];
            return target == actor.Seat && !t.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(t, kind) && !_coreProgramCardPoliciesOperations.HasCardPolicy(t, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, suit, color) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target, kind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target, suit) && _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, [target]).Contains(target) ? new(id, kind, LegalActionKind.Lightning, null)
            {
                EligibleTargets = [target]
            }

            : null;
        }

        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
            return null;
        var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: [id], conversionSource: r.Source);
        var group = kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage;
        var option = options.FirstOrDefault(o => group ? o.TargetSeats.Contains(target) : o.TargetSeats.SequenceEqual([target]));
        return option is null ? null : new(id, kind, option.ActionKind, option.RequiredCardKind)
        {
            EligibleTargets = option.TargetSeats
        };
    }

    internal void IssueMouChenLinUse(ProgramSkillFrame f)
    {
        if (!ValidMouChenLin(f) || f.MouChenLin is not { Stage: MouChenLinStage.IssuingUse, Plan: { } plan, Native: null } r || MouChenLinPlan(f, plan.CardId, r.Target)is not { } canonical || canonical.Kind != plan.Kind || canonical.Action != plan.Action || canonical.Required != plan.Required || !canonical.EligibleTargets.SequenceEqual(plan.EligibleTargets))
            throw new InvalidOperationException("Zhuanwen needs its exact publicly revealed physical material and native fixed-target admission.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(plan.CardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [r.Target], plan.Kind, physicalCardIds: [card.Id], conversionSource: r.Source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouChenLinWrittenUse is null)
            throw new InvalidOperationException("Zhuanwen must bind its real Processing entity before native declaration.");
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(plan.Kind))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: plan.Kind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, plan.Kind), programSkillCardUseFrameId: f.Id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, plan.Kind);
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, plan.Kind, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
            if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, [r.Target], plan.Action, requiredCardKind: plan.Required, playedCardKind: plan.Kind);
    }

    internal void BindMouChenLinNativeUse(CardUseFrame use)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1] is not ProgramSkillFrame { MouChenLin: { Stage: MouChenLinStage.IssuingUse, Plan: { } p, Native: null } r } f)
            return;
        if (!ValidMouChenLin(f) || !p.EligibleTargets.Contains(r.Target) || use.CardId != p.CardId || use.CardKind != p.Kind || use.SourceSeat != f.OwnerSeat || use.TargetSeats is not [var target] || target != r.Target || use.PhysicalCardIds is not [var id] || id != p.CardId || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.PhysicalCards is not [{ CardId: var material, From: var from }] || material != id || from != CardLocation.Processing || !a.ConversionChain.SequenceEqual([r.Source]) || !a.TargetSeats.SequenceEqual([r.Target]))
            throw new InvalidOperationException("Zhuanwen cannot substitute its fixed target, original material or actual native actor/provider.");
        var origin = new MouChenLinNativeUse(f.Id, r.Source, r.GameplayHash, r.Turn, r.Index, target, id, p.Kind, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouChenLin = r with { Stage = MouChenLinStage.NativeUse, Native = origin } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouChenLinWrittenUse = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinUseIssuedEvent(origin));
    }

    internal bool MouChenLinUseRidesOn(ProgramSkillFrame f, CardUseFrame u) => ValidMouChenLin(f) && f.MouChenLin is { Stage: MouChenLinStage.NativeUse, Native: { } o, Plan: { } p } r && u.MouChenLinWrittenUse == o && o.FrameId == f.Id && o.Source == r.Source && o.GameplayHash == f.GameplayHash && o.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && o.Index == r.Index && o.Target == r.Target && p.CardId == o.CardId && p.Kind == o.Kind && p.EligibleTargets.Contains(o.Target) && u.Id == o.UseId && u.CardId == o.CardId && u.CardKind == o.Kind && u.SourceSeat == f.OwnerSeat && u.TargetSeats.SequenceEqual([o.Target]) && u.PhysicalCardIds is [var id] && id == o.CardId && u.Action is { Type: CardActionType.Use } a && a.ActionId == o.ActionId && a.ProviderSeat == f.OwnerSeat && a.RequesterSeat is null && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, f.OwnerSeat, f.OwnerSeat) && a.PhysicalCards is [{ CardId: var material, From: var from }] && material == id && from == CardLocation.Processing && a.ConversionChain.SequenceEqual([r.Source]) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenLinUseIssuedEvent>().Count(e => e.Origin == o) == 1;
    [NativeUseValidation(37000)]
    internal void ValidateMouChenLinUse(CardUseFrame use)
    {
        if (use.MouChenLinWrittenUse is { } o && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is not { } f || !MouChenLinUseRidesOn(f, use)))
            throw new InvalidOperationException("Zhuanwen cannot retire another or incomplete whole native Use.");
    }

    [NativeUseReturn(37000)]
    internal bool ReturnMouChenLinUse(CardUseFrame use)
    {
        if (use.MouChenLinWrittenUse is not { } o)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        if (!MouChenLinUseRidesOn(f, use) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenLinUseReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Zhuanwen requires its exact once-completed whole native card return.");
        MouChenLinRules.UseReturned(o, use.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 37000)]
    internal bool ResumeMouChenLinAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouChenLin: { Stage: MouChenLinStage.NativeUse, NativeReturned: true, Native: { } o } } f || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null)
            return false;
        MouChenLinRules.SettleUse(o);
        return true;
    }
}
