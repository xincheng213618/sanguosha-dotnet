#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceDingxiNativeRuntime
{
    internal bool SourceDingxiEntry(ProgramSkillFrame f, SourceDingxiReceipt r) => f.SkillId == SourceBeigongIds.Dingxi && r.CardId > 0 && r.OriginalCardIds.Contains(r.CardId) && r.DiscardSequence > 0 && r.EntrySequence > r.DiscardSequence && r.EntrySequence <= _coreCardMovementJournalCardMovementsAccess._cardMovements.Count && _coreCardMovementJournalCardMovementsAccess._cardMovements[r.EntrySequence - 1] is { } entry && entry.CardId == r.CardId && entry.From == CardLocation.DiscardPile && entry.To == CardLocation.Processing && entry.Reason.Value == "skill-program.source-dingxi.enter" && f.WindowContext?.MovementBatch is { } batch && batch.Id == r.BatchId && batch.ParentFrameId == r.OriginalUseId && batch.Movements.Any(m => m.Sequence == r.DiscardSequence && m.CardId == r.CardId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) && _coreProgramEventHistoryOperations.ProgramEventHistory<SourceDingxiDiscardQualifiedEvent>().Count(e => e.BatchId == r.BatchId && e.OriginalUseId == r.OriginalUseId && e.OriginalActionId == r.OriginalActionId && e.OwnerSeat == f.OwnerSeat && e.CardId == r.CardId && e.DiscardSequence == r.DiscardSequence) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<SourceDingxiEnteredEvent>().Count(e => e.ProgramFrameId == f.Id && e.CardId == r.CardId && e.OwnerSeat == f.OwnerSeat && e.OriginalDiscardSequence == r.DiscardSequence && e.EntrySequence == r.EntrySequence) == 1;
    internal void IssueSourceDingxiUse(ProgramSkillFrame f, global::CardGame.Core.GameEngine.SourceDingxiUsePlan plan)
    {
        var r = f.SourceDingxi!;
        if (r.Stage != SourceDingxiStage.Issuing || !SourceDingxiEntry(f, r))
            throw new InvalidOperationException("Dingxi lost its actual original discard-to-Processing payment.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [r.LeftSeat], plan.Kind, physicalCardIds: [card.Id]);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.SourceDingxiUse is null)
            throw new InvalidOperationException("Dingxi must bind its raw original material before declaration.");
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(plan.Kind))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[r.LeftSeat];
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
        }
        else
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, [r.LeftSeat], plan.Action, requiredCardKind: plan.Required, playedCardKind: plan.Kind);
    }

    internal void BindSourceDingxiNativeUse(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { SourceDingxi: { Stage: SourceDingxiStage.Issuing } r } f)
            return;
        if (!SourceDingxiEntry(f, r) || use.SourceSeat != f.OwnerSeat || use.CardId != r.CardId || use.CardKind != _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind || use.TargetSeats is not [var target] || target != r.LeftSeat || use.Action is not { Type: CardActionType.Use, ConversionChain.Count: 0, PhysicalCards: [{ From: var from, CardId: var id }] } a || from != CardLocation.Processing || id != r.CardId || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null)
            throw new InvalidOperationException("Dingxi cannot substitute its fixed left target, original physical card, actor/provider or conversion chain.");
        var origin = new SourceDingxiNativeOrigin(f.Id, r.OriginalUseId, r.OriginalActionId, f.OwnerSeat, r.LeftSeat, id, use.CardKind, r.DiscardSequence, r.EntrySequence, f.SkillInstanceId, f.GameplayHash, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { SourceDingxi = r with { Stage = SourceDingxiStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { SourceDingxiUse = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceDingxiIssuedEvent(origin));
    }

    internal bool IsSourceDingxiOriginalActionCost(CardActionContext action, CardActionCost cost) => action is { Type: CardActionType.Use, ConversionChain.Count: 0, PhysicalCards: [var only] } && only == cost && cost.From == CardLocation.Processing && action.ActorSeat == action.ProviderSeat && action.RequesterSeat is null && action.EffectiveKind == cost.CardKind && _coreProgramEventHistoryOperations.ProgramEventHistory<SourceDingxiIssuedEvent>().Any(e => e.Origin.ActionId == action.ActionId && e.Origin.OwnerSeat == action.ActorSeat && e.Origin.CardId == cost.CardId && e.Origin.Kind == cost.CardKind && e.Origin.EntrySequence > 0 && _coreCardMovementJournalCardMovementsAccess._cardMovements[e.Origin.EntrySequence - 1] is { } entry && entry.CardId == cost.CardId && entry.From == CardLocation.DiscardPile && entry.To == CardLocation.Processing && entry.Reason.Value == "skill-program.source-dingxi.enter");
    internal bool SourceDingxiUseRidesOn(ProgramSkillFrame f, CardUseFrame use) => f.SourceDingxi is { Stage: SourceDingxiStage.NativeUse } r && SourceDingxiEntry(f, r) && use.SourceDingxiUse is { } o && o.ProgramFrameId == f.Id && o.SkillInstanceId == f.SkillInstanceId && o.GameplayHash == f.GameplayHash && o.UseId == r.UseId && use.Id == r.UseId && o.ActionId == r.ActionId && o.CardId == r.CardId && o.OwnerSeat == f.OwnerSeat && o.LeftSeat == r.LeftSeat && o.OriginalUseId == r.OriginalUseId && o.OriginalActionId == r.OriginalActionId && o.EntrySequence == r.EntrySequence && o.DiscardSequence == r.DiscardSequence && use.CardId == o.CardId && use.CardKind == o.Kind && use.SourceSeat == f.OwnerSeat && use.TargetSeats.SequenceEqual([r.LeftSeat]) && use.PhysicalCardIds is [var physical] && physical == r.CardId && use.Action is { } a && a.ActionId == o.ActionId && a.TargetSeats.SequenceEqual([r.LeftSeat]) && a.PhysicalCards is [var cost] && IsSourceDingxiOriginalActionCost(a, cost) && _coreProgramEventHistoryOperations.ProgramEventHistory<SourceDingxiIssuedEvent>().Count(e => e.Origin == o) == 1;
    internal bool IsSourceDingxiRetainedEntity(long useId, int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Processing && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { SourceDingxiUse: { } o } use && o.CardId == cardId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.ProgramFrameId)is { } f && SourceDingxiUseRidesOn(f, use);
    [NativeUseValidation(24510)]
    internal void ValidateSourceDingxiUse(CardUseFrame use)
    {
        if (use.SourceDingxiUse is { } o && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.ProgramFrameId)is not { } f || !SourceDingxiUseRidesOn(f, use)))
            throw new InvalidOperationException("Dingxi cannot retire an incomplete or unrelated whole native Use.");
    }

    [NativeUseReturn(24510)]
    internal bool ReturnSourceDingxiUse(CardUseFrame use)
    {
        if (use.SourceDingxiUse is not { } o)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        if (!SourceDingxiUseRidesOn(f, use) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<SourceDingxiReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Dingxi requires its once-completed exact original native card return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceDingxiReturnedEvent(f.Id, use.Id, o.CardId));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { SourceDingxi = f.SourceDingxi!with { Stage = SourceDingxiStage.Returned } });
        if (use.CardAttack is not { Active: true })
            _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 24510)]
    internal bool ResumeSourceDingxiAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceDingxi: { Stage: SourceDingxiStage.Returned } } f)
            return false;
        _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
        return true;
    }
}
