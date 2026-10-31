#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionRuntime
{
    internal bool TryBeginExtraForeignSelection(NullificationWindowFrame pending, CharacterState target)
    {
        if (!(_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId) || _characterZhugeGuoLeOperations.IsLeChengyanEffectUse(pending.ParentFrameId) || _coreWushuangTargetCardsOperations.IsWushuangExtraForeignSelection(pending.ParentFrameId) || _coreMantianGuohaiOperations.IsMantianGuohaiUse(pending.ParentFrameId)) || pending.ActionKind is not (LegalActionKind.Snatch or LegalActionKind.Dismantlement or LegalActionKind.MantianGuohai))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)!;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
        var slots = new List<ExtraForeignCardSlot>();
        slots.AddRange(Enumerable.Range(0, hand.Count).Select(slot => new ExtraForeignCardSlot(slot, null, null)));
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(target)))
        {
            var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
            if (pending.ActionKind == LegalActionKind.Dismantlement && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(pending.SourceSeat, card, location, OwnedCardMoveIntent.Discard))
                continue;
            slots.Add(new(slots.Count, card.Id, location));
        }

        if (slots.Count == 0)
        {
            _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.PublicTargetMissing);
            return true;
        }

        var allowFinish = _coreWushuangTargetCardsOperations.IsWushuangExtraForeignSelection(use.Id);
        if (allowFinish)
            slots.Add(new(slots.Count, null, null));
        var receipt = new ExtraForeignCardSelection(use.Action!.ActionId, hand.Count, Array.AsReadOnly(slots.ToArray()))
        {
            AllowFinish = allowFinish
        };
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.AwaitingResponse);
        _coreRuntimeOperations.PushRuntimeFrame(new TargetCardSelectionFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, use.Id, use.SourceSeat, target.Seat, use.CardId, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), pending.ActionKind, Array.AsReadOnly(slots.Select(slot => slot.Slot).ToArray())) { ExtraForeignSelection = receipt });
        var choices = slots.Select(slot => new PromptChoice(new($"extra-effect.foreign.{(use.Id)}.{(slot.Slot)}"), allowFinish && slot.Slot == slots.Count - 1 ? "结束额外弃置" : slot.PublicCardId is { } cardId ? $"选择 {(target.Name)} 的【{(_coreCardAttackStateOperations.GetAttackCard(cardId).DisplayName)}】" : _coreWushuangCombatTacticsOperations.WushuangHasVisibleTargetHand(use.SourceSeat, pending.EffectCardKind) ? $"选择【{(hand[slot.Slot].DisplayName)}】（{(hand[slot.Slot].RankText)}）" : $"选择 {(target.Name)} 的一张暗牌（牌位 {(slot.Slot + 1)}）", slot.PublicCardId is { } visible ? [visible] : [], [target.Seat], new Dictionary<string, string> { ["action"] = "target-card-slot", ["slot-index"] = slot.Slot.ToString(CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.SelectTargetCard, use.SourceSeat, "选择目标区域的一张牌。", [], [target.Seat], use.SourceSeat, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use))
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return true;
    }

    internal bool TryResolveExtraForeignSelection(TargetCardSelectionFrame pending, int slot)
    {
        if (pending.ExtraForeignSelection is not { } r)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != pending.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)is not { Action: { } action } use || action.ActionId != r.ActionId || !(_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id) || _characterZhugeGuoLeOperations.IsLeChengyanEffectUse(use.Id) || _coreWushuangTargetCardsOperations.IsWushuangExtraForeignSelection(use.Id) || _coreMantianGuohaiOperations.IsMantianGuohaiUse(use.Id)) || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat]).Count != r.HandCount || r.Slots.SingleOrDefault(s => s.Slot == slot)is not { } selected)
            throw new InvalidOperationException("Extra foreign selection lost its current slot or actual use.");
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat];
        Card chosen;
        CardLocation from;
        if (selected.PublicCardId is { } publicId && selected.PublicLocation is { } location)
        {
            if (_cardZonesOperations._cardZones.GetLocation(publicId) != location || location.OwnerSeat != target.Seat || location.Zone is not (CardZoneKind.Equipment or CardZoneKind.Judgment))
                throw new InvalidOperationException("Extra foreign selection lost its frozen public card.");
            chosen = _coreCardAttackStateOperations.GetAttackCard(publicId);
            from = location;
            if (pending.ActionKind == LegalActionKind.Dismantlement && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(use.SourceSeat, chosen, from, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("Extra foreign selection cannot discard protected equipment.");
        }
        else if (r.AllowFinish && slot == r.Slots.Count - 1)
        {
            _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.TargetCardSelection);
            _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
            _coreWushuangTargetCardsOperations.FinishWushuangTargetCards(use);
            return true;
        }
        else
        {
            chosen = _corePhysicalOwnedCardQueriesOperations.GetHand(target)[slot];
            from = CardLocation.Hand(target.Seat);
        }

        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, true);
        _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.TargetCardSelection);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        _coreNativeTrickEffectsOperations.ApplyTargetCardEffect(use.Id, card, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), use.SourceSeat, target.Seat, pending.ActionKind, chosen, from);
        return true;
    }

    internal bool IsExtraForeignPublicAnswer(TargetCardSelectionFrame pending, PromptChoice selected)
    {
        if (pending.ExtraForeignSelection is not { } r || selected.Cards is not [var cardId] || selected.Parameters.GetValueOrDefault("action") != "target-card-slot" || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), out var slot))
            return false;
        return r.Slots.Any(s => s.Slot == slot && s.PublicCardId == cardId) && selected.Targets.SequenceEqual([pending.TargetSeat]);
    }
}
