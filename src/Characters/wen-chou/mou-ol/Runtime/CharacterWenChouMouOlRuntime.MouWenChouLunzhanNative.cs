#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenChouMouOlRuntime
{
    internal void IssueMouWenChouDuel(ProgramSkillFrame f, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption option)
    {
        var r = f.MouWenChouOl!;
        var cards = MouWenChouMaterials(f);
        if (!ValidMouWenChouOl(f) || r.Stage != MouWenChouOlStage.Targets || MouWenChouCountUsed(f.OwnerSeat, cards.Length) || !MouWenChouOptions(f).Any(o => o.Id == option.Id))
            throw new InvalidOperationException("Lunzhan must issue its exact current unpaid native Duel option.");
        var suitSet = cards.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)).Distinct().ToArray();
        var suit = suitSet.Length == 1 ? suitSet[0] : Suit.None;
        var colorSet = cards.Select(c => _coreActionDiscardColorsOperations.CapturePhysicalCardColor(f.OwnerSeat, c)).Distinct().ToArray();
        var color = colorSet.Length == 1 ? colorSet[0] : null;
        var rank = cards.Length == 1 ? cards[0].Rank : 0;
        var card = cards.FirstOrDefault() ?? new(0, CardKind.Duel, Suit.None, 0);
        var origin = new MouWenChouLunzhanOrigin(f.Id, r.Source, f.GameplayHash, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence + 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, cards.Length, card.Id, suit, rank, color);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { Stage = MouWenChouOlStage.Issuing, UseId = origin.UseId, ActionId = origin.ActionId, NativeTargets = option.TargetSeats, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouLunzhanIssuedEvent(origin));
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, option.TargetSeats, CardKind.Duel, physicalCardIds: f.SelectedCardIds, conversionSource: cards.Length == 0 ? null : r.Source);
        if (id != origin.UseId || !IsMouWenChouDuel(id))
            throw new InvalidOperationException("Lunzhan changed its signed native Use identity.");
        foreach (var group in cards.GroupBy(c => _cardZonesOperations._cardZones.GetLocation(c.Id)))
            _corePhysicalMovementFacadesOperations.MoveCards(group.ToArray(), group.Key, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(id, f.SkillId, f.ActivationId, f.OwnerSeat, f.SelectedCardIds, CardKind.Duel, true, option.TargetSeats));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, option.TargetSeats, LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    internal CardActionContext PrepareMouWenChouAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouWenChouOl: { Stage: MouWenChouOlStage.Issuing } r } f)
            return action;
        var o = _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouLunzhanIssuedEvent>().Single(e => e.Origin.ProgramFrameId == f.Id).Origin;
        if (!ValidMouWenChouOl(f) || action.ActionId != o.ActionId || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.Type != CardActionType.Use || action.EffectiveKind != CardKind.Duel || !action.TargetSeats.SequenceEqual(r.NativeTargets) || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(f.SelectedCardIds) || !action.ConversionChain.SequenceEqual(o.MaterialCount == 0 ? [] : [o.Source]))
            throw new InvalidOperationException("Lunzhan's declaration lost its original materials, participant or accepted native targets.");
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: o.Suit, effectiveRank: o.Rank, effectiveIsRed: o.IsRed));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33100)]
    internal void ObserveMouWenChouDeclaration(IGameEvent fact)
    {
        if (!HasMouWenChouOl || fact is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { Action: { } action } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { MouWenChouOl: { Stage: MouWenChouOlStage.Issuing } r } f)
        {
            var o = _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouLunzhanIssuedEvent>().Single(e => e.Origin.ProgramFrameId == f.Id).Origin;
            if (!ValidMouWenChouOl(f) || o.UseId != use.Id || o.ActionId != action.ActionId || use.CardKind != CardKind.Duel || use.CardId != o.CardId || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(f.SelectedCardIds))
                throw new InvalidOperationException("Lunzhan cannot attach to another producer's declaration.");
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouWenChouLunzhan = o });
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouWenChouOl = r with { Stage = MouWenChouOlStage.NativeUse } });
        }

        if (action.Type == CardActionType.Use && action.ActorSeat == action.ProviderSeat && action.PhysicalCards.Count > 0 && action.PhysicalCards.All(c => c.From == CardLocation.Hand(action.ActorSeat)) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).Select(c => c.Id).Order().SequenceEqual(action.PhysicalCards.Select(c => c.CardId).Order()))
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouWenChouAllHand = true });
    }

    internal bool IsMouWenChouDuel(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouWenChouLunzhan: { } o, Action: { Type: CardActionType.Use } a } use || use.Id != o.UseId || use.CardId != o.CardId || use.CardKind != CardKind.Duel || a.ActionId != o.ActionId || a.EffectiveKind != CardKind.Duel || a.ProviderSeat != o.Source.OwnerSeat || a.RequesterSeat is not null || a.EffectiveSuit != o.Suit || a.EffectiveRank != o.Rank || a.EffectiveIsRed != o.IsRed || a.PhysicalCards.Count != o.MaterialCount || !a.ConversionChain.SequenceEqual(o.MaterialCount == 0 ? [] : [o.Source]) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.ProgramFrameId)is not ProgramSkillFrame { MouWenChouOl: { Stage: MouWenChouOlStage.NativeUse, NativeReturned: false } r } parent || !ValidMouWenChouOl(parent) || r.UseId != id || r.ActionId != a.ActionId || r.Source != o.Source || parent.GameplayHash != o.GameplayHash || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual(parent.SelectedCardIds) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouLunzhanIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat);
    }

    internal bool IsMouWenChouZeroDuel(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, PhysicalCardIds.Count: 0 } && IsMouWenChouDuel(id);
    internal long? MouWenChouDuelProgramParent(long id) => IsMouWenChouDuel(id) ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.MouWenChouLunzhan!.ProgramFrameId : null;
    internal Card ReadMouWenChouUseAppearance(long id, CardAppearanceReference appearance)
    {
        if (!IsMouWenChouZeroDuel(id))
            return _characterXuHuangMouMobileOperations.ReadMouXuHuangUseAppearance(id, appearance);
        if (appearance.Id != 0 || appearance.Kind != CardKind.Duel || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("Lunzhan's empty Duel lost its signed neutral appearance.");
        return new(0, CardKind.Duel, Suit.None, 0);
    }

    [NativeUseValidation(33100)]
    internal void ValidateMouWenChouDuel(CardUseFrame use)
    {
        if (use.MouWenChouLunzhan is not null && (!IsMouWenChouDuel(use.Id) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null))
            throw new InvalidOperationException("Lunzhan must complete its whole original native Use and children before returning.");
    }

    [NativeUseReturn(33100)]
    internal void ReturnMouWenChouDuel(CardUseFrame use)
    {
        if (use.MouWenChouLunzhan is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.MouWenChouOl!;
        if (!ValidMouWenChouOl(f) || r.UseId != use.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null)
            throw new InvalidOperationException("Lunzhan's native Use can return exactly once to its own parent.");
        var pendingAttack = use.CardAttack is { Active: true };
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouNativeBatchSignedEvent(f.Id, use.Id, b.Id, b.ParentBatchId, b.Movements.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { NativeReturned = true, AttackReturned = !pendingAttack, NativeBatchIds = batches.Select(b => b.Id).ToArray() } });
        if (!pendingAttack)
            CompleteMouWenChouDuel(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 33100)]
    internal bool ReturnMouWenChouDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouWenChouOl: { Stage: MouWenChouOlStage.NativeUse, NativeReturned: true, AttackReturned: false } r } f || r.UseId != c.ResolutionId)
            return false;
        if (c.ProgramFrameId != f.Id || !ValidMouWenChouOl(f))
            throw new InvalidOperationException("Lunzhan's Duel cannot return to another program.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouWenChouOl = r with { AttackReturned = true } });
        CompleteMouWenChouDuel(f.Id);
        return true;
    }

    internal void CompleteMouWenChouDuel(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouWenChouOl!;
        if (!r.NativeReturned || !r.AttackReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Lunzhan cannot finish before both native returns.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouLunzhanReturnedEvent(f.Id, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouWenChouOl = r with { Stage = MouWenChouOlStage.Children }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueMouWenChouOl(id);
    }

    internal bool IsMouWenChouNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.MouWenChouOl is { Stage: MouWenChouOlStage.Children, NativeReturned: true, AttackReturned: true } r && ValidMouWenChouOl(f) && r.NativeBatchIds.Contains(b.Id) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouNativeBatchSignedEvent>().Count(e => e == new MouWenChouNativeBatchSignedEvent(f.Id, r.UseId, b.Id, b.ParentBatchId, b.Movements.Count)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1700)]
    internal bool WindowSettlement_IsMouWenChouNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouWenChouNativeSettlementBatch(frame, batch);
}
