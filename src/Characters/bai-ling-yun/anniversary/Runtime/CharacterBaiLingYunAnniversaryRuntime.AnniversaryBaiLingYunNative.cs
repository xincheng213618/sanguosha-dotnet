#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaiLingYunAnniversaryRuntime
{
    internal bool IsAnniversaryBaiLingYunSlashSource(long? programId, int source, int target, Card card, CardKind kind, int provider, CardLocation from, IReadOnlyList<Card>? physical, bool counts) => from == CardLocation.Processing && counts && source == provider && (card.Kind == kind || card.Kind == CardKind.Slash && kind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[source])) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (physical is null || physical is [var material] && material.Id == card.Id) && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryBly: { Stage: AnniversaryBlyStage.IssuingUse, Plan: { } p } } f && f.Id == programId && f.OwnerSeat == source && p.CardId == card.Id && p.Targets.Count > 0 && p.Targets[0] == target && _cardZonesOperations._cardZones.Contains(CardLocation.Processing, card.Id) && ValidAnniversaryBly(f) && AnniversaryBlyUsePaid(f);
    internal void IssueAnniversaryBlyUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryBly!;
        var p = r.Plan!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId);
        if (r.Stage != AnniversaryBlyStage.UsePayment || !AnniversaryBlyUsePaid(f) || !_cardZonesOperations._cardZones.Contains(CardLocation.Processing, card.Id))
            throw new InvalidOperationException("Linghui must use its exact privately viewed entity after its original real payment.");
        var a = AnniversaryBlyUseActions(f).Single(a => AnniversaryBlyActionMatches(a, p));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.IssuingUse, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (a.ProgramActivationId is "next-card-target-adjustment" or "next-actual-use-target-adjustment")
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = a.TargetSeats;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind))
        {
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[a.TargetSeats[0]], card, p.Kind, actor.Seat, countsTowardSlashLimit: true, usesZhuqueFan: p.Kind == CardKind.FireSlash && card.Kind == CardKind.Slash && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), programSkillCardUseFrameId: f.Id, physicalSourceLocation: CardLocation.Processing);
            return;
        }

        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, a.TargetSeats, card.Kind, physicalCardIds: [card.Id], designatedTargetSeats: card.Kind == CardKind.BorrowedSword ? a.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : null);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.AnniversaryBlyUse is null)
            throw new InvalidOperationException("Linghui lost its actual declared physical use.");
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
        {
            if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(actor, card, id))
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(actor, card, id);
            return;
        }

        if (card.Kind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Recovery));
            return;
        }

        if (card.Kind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (card.Kind == CardKind.Lightning && !_coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability && _coreCardActionsOperations.TryBeginDelayedCardUsePrograms(id))
            return;
        var required = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(card.Kind) ? _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, card.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), enforceUsePermission: true, actualEffectiveColor: _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, card), hasActualColor: true, physicalCardIds: [card.Id]).FirstOrDefault(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && o.TargetSeats.SequenceEqual(a.TargetSeats))?.RequiredCardKind : null;
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, a.Kind, a.TargetCardId, required, card.Kind);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 43600)]
    internal void BindAnniversaryBlyNative(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryBly: { Stage: AnniversaryBlyStage.IssuingUse, Plan: { } p } r } f)
            return;
        if (!ValidAnniversaryBly(f) || !AnniversaryBlyUsePaid(f) || use.CardId != p.CardId || use.CardKind != p.Kind || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.PhysicalCards is not [{ CardId: var material, From: var from }] || material != p.CardId || from != CardLocation.Processing || !a.TargetSeats.SequenceEqual(_coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(p.Kind, f.OwnerSeat, p.Targets)) || a.ConversionChain.Count != 0)
            throw new InvalidOperationException("Linghui changed its actual private entity, actor/provider, printed kind or native targets.");
        var origin = new AnniversaryBlyNative(f.Id, f.InstructionIndex, r.Source, r.Hash, r.Turn, p.CardId, use.Id, a.ActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversaryBlyUse = origin });
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryBlyNativeIssuedEvent(origin));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MaterialPayment, 43600)]
    internal bool PayAnniversaryBlyProcessing(IReadOnlyList<Card> cards, int provider, CardLocation? from, CardMoveReason reason) => from == CardLocation.Processing && reason == CardMoveReasons.Use && cards is [var card] && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { AnniversaryBlyUse: { } o } u && provider == o.Source.OwnerSeat && card.Id == o.CardId && _cardZonesOperations._cardZones.Contains(CardLocation.Processing, card.Id) && IsAnniversaryBlyNative(u);
    internal bool IsAnniversaryBlyNative(CardUseFrame use) => use.AnniversaryBlyUse is { } o && use.Id == o.UseId && use.CardId == o.CardId && use.Action is { Type: CardActionType.Use } a && a.ActionId == o.ActionId && a.ProviderSeat == o.Source.OwnerSeat && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == o.CardId && from == CardLocation.Processing && a.EffectiveKind == use.CardKind && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is { AnniversaryBly: { } r } parent && ValidAnniversaryBly(parent) && r.Source == o.Source && r.Hash == o.Hash && r.UseId == use.Id && r.ActionId == a.ActionId && r.Plan?.CardId == id && AnniversaryBlyUsePaid(parent) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryBlyNativeIssuedEvent>().Count(e => e.Origin == o) == 1;
    [NativeUseValidation(43600)]
    internal void ValidateAnniversaryBlyNativeReturn(CardUseFrame use)
    {
        if (use.AnniversaryBlyUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsAnniversaryBlyNative(use)))
            throw new InvalidOperationException("Linghui must retire its original complete physical use.");
    }

    [NativeUseReturn(43600)]
    internal void ReturnAnniversaryBlyNative(CardUseFrame use)
    {
        if (use.AnniversaryBlyUse is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.AnniversaryBly!;
        if (!IsAnniversaryBlyNative(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == o.CardId) != 1)
            throw new InvalidOperationException("Linghui cannot return a different or unfinished use or pay its reward twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryBly = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryBlyNativeReturnedEvent(f.Id, use.Id, o.ActionId));
        if (use.CardAttack is not { Active: true })
            SettleAnniversaryBlyNative(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 43600)]
    internal bool ReturnAnniversaryBlyAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryBly: { Stage: AnniversaryBlyStage.NativeUse, Returned: true } } f)
            return false;
        SettleAnniversaryBlyNative(f);
        return true;
    }

    internal void SettleAnniversaryBlyNative(ProgramSkillFrame f)
    {
        var r = f.AnniversaryBly!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Linghui must await the actual native use tail.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryBlyNativeBatchEvent(f.Id, r.UseId, b.Id, b.Movements.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueAnniversaryBly(f.Id);
    }
}
