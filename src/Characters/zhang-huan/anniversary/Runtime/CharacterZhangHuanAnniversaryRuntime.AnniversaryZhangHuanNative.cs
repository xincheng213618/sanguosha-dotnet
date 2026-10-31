#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHuanAnniversaryRuntime
{
    internal static bool SameZhangHuanAction(LegalAction a, LegalAction b) => a.Kind == b.Kind && a.CardId == b.CardId && a.PlayedCardKind == b.PlayedCardKind && a.TargetSeat == b.TargetSeat && a.TargetCardId == b.TargetCardId && a.ProgramActivationId == b.ProgramActivationId && a.ConversionSource == b.ConversionSource && (a.AdditionalConversionSources ?? []).SequenceEqual(b.AdditionalConversionSources ?? []) && a.TargetSeats.SequenceEqual(b.TargetSeats);
    internal static bool SameZhangHuanNative(AnniversaryZhangHuanNative a, AnniversaryZhangHuanNative b) => (a.FrameId, a.Instruction, a.Source, a.Hash, a.Turn, a.Actor, a.Discard, a.UseId, a.ActionId) == (b.FrameId, b.Instruction, b.Source, b.Hash, b.Turn, b.Actor, b.Discard, b.UseId, b.ActionId) && SameZhangHuanAction(a.Action, b.Action);
    internal IReadOnlyList<int> ZhangHuanExpectedTargets(AnniversaryZhangHuanNative n)
    {
        var a = n.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.Actor];
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(n.Discard.CardId), null);
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guandu)
            return guandu;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } dragon)
            return dragon;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(a.PlayedCardKind ?? card.Kind, actor.Seat, a.TargetSeats);
        var kind = a.PlayedCardKind ?? card.Kind;
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, card.Suit, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46300)]
    internal void BindZhangHuanUse(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryZhangHuan: { Stage: AnniversaryZhangHuanStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        if (!ValidZhangHuan(f) || use.CardId != n.Discard.CardId || use.SourceSeat != f.OwnerSeat || use.CardKind != (n.Action.PlayedCardKind ?? n.Discard.Kind) || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.ConversionChain.Count != 0 || a.PhysicalCards is not [{ CardId: var material, From: var from }] || material != n.Discard.CardId || from != CardLocation.DiscardPile || !_cardZonesOperations._cardZones.Contains(from, material) || !ZhangHuanDiscards().Contains(n.Discard) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == material)?.Sequence != n.Discard.Sequence || !use.TargetSeats.SequenceEqual(ZhangHuanExpectedTargets(n)))
            throw new InvalidOperationException("Shuguo changed its actual discarded entity, discard executor, native actor or accepted targets.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryZhangHuan = r with { Stage = AnniversaryZhangHuanStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversaryZhangHuanUse = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanUseIssuedEvent(f.Id, use.Id, a.ActionId, material, n.Discard.Sequence, n.Discard.ExecutorSeat));
    }

    internal bool IsZhangHuanNative(CardUseFrame use) => use.AnniversaryZhangHuanUse is { } n && use.Id == n.UseId && use.CardId == n.Discard.CardId && use.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ProviderSeat == n.Actor && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, n.Actor, n.Actor) && a.EffectiveKind == (n.Action.PlayedCardKind ?? n.Discard.Kind) && a.PhysicalCards is [{ CardId: var id, From: var from }] && id == n.Discard.CardId && from == CardLocation.DiscardPile && use.PhysicalCardIds is [var physical] && physical == id && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.FrameId)is ProgramSkillFrame { AnniversaryZhangHuan.Native: { } pending } parent && ValidZhangHuan(parent) && SameZhangHuanNative(n, pending) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHuanUseIssuedEvent>().Count(e => e == new AnniversaryZhangHuanUseIssuedEvent(n.FrameId, n.UseId, n.ActionId, id, n.Discard.Sequence, n.Discard.ExecutorSeat)) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 46300)]
    internal bool ZhangHuanEndingSlashDebit(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { AnniversaryZhangHuanUse.Actor: var owner } use && owner == actor && IsZhangHuanNative(use);
    [NativeUseValidation(46300)]
    internal void ValidateZhangHuanNativeReturn(CardUseFrame use)
    {
        if (use.AnniversaryZhangHuanUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsZhangHuanNative(use)))
            throw new InvalidOperationException("Shuguo cannot retire a different or incomplete actual discarded-card use.");
    }

    [NativeUseReturn(46300)]
    internal void ReturnZhangHuanUse(CardUseFrame use)
    {
        if (use.AnniversaryZhangHuanUse is not { } n)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        var r = f.AnniversaryZhangHuan!;
        if (!IsZhangHuanNative(use) || r.Returned || r.Used.Contains(n.Discard.CardId) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == n.Discard.CardId) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == n.Discard.CardId && m.Sequence > r.Before && m.From == CardLocation.DiscardPile && m.To == CardLocation.Processing && m.Reason == (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(n.Discard.Kind) ? CardMoveReasons.EquipmentUse : CardMoveReasons.Use)) != 1)
            throw new InvalidOperationException("Shuguo must pay and complete this exact original native entity once.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryZhangHuan = r with { Returned = true, Used = r.Used.Append(n.Discard.CardId).ToArray(), Stop = n.Discard.ExecutorSeat != f.OwnerSeat } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanUseReturnedEvent(f.Id, use.Id, n.ActionId, n.Discard.CardId));
        if (use.CardAttack is not { Active: true })
            SettleZhangHuanUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 46300)]
    internal bool ReturnZhangHuanAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangHuan: { Stage: AnniversaryZhangHuanStage.NativeUse, Returned: true } } f)
            return false;
        SettleZhangHuanUse(f);
        return true;
    }

    internal void SettleZhangHuanUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryZhangHuan!;
        var n = r.Native!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null)
            throw new InvalidOperationException("Shuguo must wait for all ordered targets and the complete native card-use tail.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var batch in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanNativeBatchEvent(f.Id, n.UseId, batch.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r with { Stage = AnniversaryZhangHuanStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueZhangHuan(f.Id);
    }

    internal bool IsAnniversaryZhangHuanSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.AnniversaryZhangHuan is { Stage: AnniversaryZhangHuanStage.NativeTail, Native: { } n, Returned: true } r && ValidZhangHuan(f) && r.NativeBatches.Contains(b.Id) && b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.Count > 0 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHuanNativeBatchEvent>().Count(e => e == new AnniversaryZhangHuanNativeBatchEvent(f.Id, n.UseId, b.Id)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1300)]
    internal bool WindowSettlement_IsAnniversaryZhangHuanSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryZhangHuanSettlementBatch(frame, batch);
}
