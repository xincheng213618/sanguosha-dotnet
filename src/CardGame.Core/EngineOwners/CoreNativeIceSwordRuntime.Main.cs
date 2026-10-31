#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeIceSwordRuntime
{
    internal int GetIceSwordTargetCardCount(CharacterState target, int actorSeat) => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actorSeat, card, CardLocation.Hand(target.Seat), OwnedCardMoveIntent.Discard)) + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actorSeat, card, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard));
    internal bool TryBeginIceSwordChoice(NativeOwners.CardAttackHandle attack, int preventedDamageAmount)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!_coreNativeDamageHandlesOperations.UsesFormalIceSword || attack.IceSwordAttempted || attack.IsChainPropagation || attack.EffectiveCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) || !source.IsAlive || !target.IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.IceSword) || GetIceSwordTargetCardCount(target, source.Seat) == 0)
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is not null)
        {
            throw new InvalidOperationException("The engine cannot open two Ice Sword choices at once.");
        }

        attack.MarkIceSwordAttempted();
        _coreNativeCardContinuationCursorsOperations.ActiveIceSword = _kernelCoreMainOperations.CreateIceSwordHandle(attack, preventedDamageAmount);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        PublishIceSwordChoice(_coreNativeCardContinuationCursorsOperations.ActiveIceSword);
        return true;
    }

    internal void PublishIceSwordChoice(NativeOwners.IceSwordHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveIceSword, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack))
        {
            throw new InvalidOperationException("The Ice Sword choice has no current Slash continuation.");
        }

        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
        var handSlots = Enumerable.Range(0, hand.Count).Where(slot => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(source.Seat, hand[slot], CardLocation.Hand(target.Seat), OwnedCardMoveIntent.Discard)).ToArray();
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(source.Seat, card, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard)).ToArray();
        if (handSlots.Length + equipment.Length == 0)
        {
            FinishIceSwordPrevention(pending);
            return;
        }

        var choices = new List<PromptChoice>();
        foreach (var slot in pending.Activated ? handSlots : [])
        {
            choices.Add(new PromptChoice(new ChoiceId($"ice-sword.hand-slot-{(slot)}.resolution-{(attack.ResolutionId)}.pick-{(pending.DiscardedCardIds.Count + 1)}"), $"弃置 {(target.Name)} 的第 {(slot + 1)} 个暗手牌位。", [], [target.Seat], new Dictionary<string, string> { ["action"] = "ice-sword-discard", ["target-zone"] = "hand", ["slot-index"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture), ["pick-index"] = (pending.DiscardedCardIds.Count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        foreach (var card in pending.Activated ? equipment : [])
        {
            choices.Add(new PromptChoice(new ChoiceId($"ice-sword.equipment-{(card.Id)}.resolution-{(attack.ResolutionId)}.pick-{(pending.DiscardedCardIds.Count + 1)}"), $"弃置 {(target.Name)} 装备区的【{(card.DisplayName)}】。", [card.Id], [target.Seat], new Dictionary<string, string> { ["action"] = "ice-sword-discard", ["target-zone"] = "equipment", ["pick-index"] = (pending.DiscardedCardIds.Count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        if (!pending.Activated)
        {
            choices.Add(new PromptChoice(new ChoiceId($"ice-sword.activate.resolution-{(attack.ResolutionId)}"), "发动【寒冰剑】", [], [], new Dictionary<string, string> { ["action"] = "ice-sword-activate" }));
            choices.Add(new PromptChoice(new ChoiceId($"ice-sword.damage.resolution-{(attack.ResolutionId)}"), $"不发动【寒冰剑】，对 {(target.Name)} 造成 {(pending.PreventedDamageAmount)} 点伤害。", [], [], new Dictionary<string, string> { ["action"] = "ice-sword-damage" }));
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.IceSword, source.Seat, pending.Activated ? pending.DiscardedCardIds.Count == 0 ? $"【寒冰剑】已发动，请选择弃置 {(target.Name)} 的第一张牌。" : $"【寒冰剑】已防止伤害，请继续弃置 {(target.Name)} 的第二张牌。" : $"你的【{(CardCatalog.Get(CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack)).DisplayName)}】将对 {(target.Name)} 造成 {(pending.PreventedDamageAmount)} 点伤害，是否发动【寒冰剑】？", pending.Activated ? equipment.Select(card => card.Id).ToArray() : [], pending.Activated ? [target.Seat] : [], SourceSeat: source.Seat, IncomingCard: attack.EffectiveCardKind)
        {
            PromptId = source.IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveIceSwordChoice(PromptChoice selected)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveIceSword ?? throw new InvalidOperationException("There is no Ice Sword choice to resolve.");
        var attack = pending.Attack;
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.IceSword } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The Ice Sword choice is not the current Slash continuation.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var action = selected.Parameters.GetValueOrDefault("action");
        if (action == "ice-sword-activate")
        {
            if (pending.Activated || selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("Ice Sword activation must precede its card selections.");
            pending.Activated = true;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (_coreNativeEquipmentInvocationsOperations.TryBeginIceSwordEquipmentInvocation(pending))
                return;
            PublishIceSwordChoice(pending);
            return;
        }

        if (action == "ice-sword-damage")
        {
            if (pending.Activated || selected.Cards.Count != 0 || selected.Targets.Count != 0)
            {
                throw new InvalidOperationException("Ice Sword damage may only be retained before its first discard.");
            }

            _coreNativeCardContinuationCursorsOperations.ActiveIceSword = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IceSwordResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, Used: false, PreventedDamageAmount: 0, DiscardedCardIds: []));
            _coreMatchLogOperations.AddLog("EquipmentSkipped", $"{(source.Name)} 未发动【寒冰剑】，{(target.Name)} 继续受到 {(pending.PreventedDamageAmount)} 点伤害。", source.Seat, target.Seat);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }

            return;
        }

        if (!pending.Activated || action != "ice-sword-discard" || !selected.Targets.SequenceEqual([target.Seat]))
        {
            throw new InvalidOperationException("The Ice Sword discard choice is malformed.");
        }

        Card discarded;
        var targetZone = selected.Parameters.GetValueOrDefault("target-zone");
        if (targetZone == "hand" && selected.Cards.Count == 0 && selected.Parameters.TryGetValue("slot-index", out var slotText) && int.TryParse(slotText, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var slot) && slot >= 0 && slot < _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count)
        {
            discarded = _corePhysicalOwnedCardQueriesOperations.GetHand(target)[slot];
        }
        else if (targetZone == "equipment" && selected.Cards.Count == 1 && _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).FirstOrDefault(card => card.Id == selected.Cards[0])is { } equipment)
        {
            discarded = equipment;
        }
        else
        {
            throw new InvalidOperationException("The selected Ice Sword target card is no longer available.");
        }

        if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(source.Seat, discarded, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(target, discarded), OwnedCardMoveIntent.Discard))
        {
            if (GetIceSwordTargetCardCount(target, source.Seat) == 0)
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                FinishIceSwordPrevention(pending);
            }
            else
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                PublishIceSwordChoice(pending);
            }

            return;
        }

        AppendIceSwordLogicalDiscard(pending, discarded);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _corePhysicalMovementFacadesOperations.MoveCard(discarded, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(target, discarded), CardLocation.DiscardPile, CardMoveReasons.IceSwordDiscard);
        pending.AddDiscardedCard(discarded.Id);
        _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(source.Name)} 发动【寒冰剑】，弃置 {(target.Name)} 的【{(discarded.DisplayName)}】。", source.Seat, target.Seat);
        if (pending.DiscardedCardIds.Count < 2 && GetIceSwordTargetCardCount(target, source.Seat) > 0)
        {
            PublishIceSwordChoice(pending);
            return;
        }

        FinishIceSwordPrevention(pending);
    }

    internal void FinishIceSwordPrevention(NativeOwners.IceSwordHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveIceSword, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) || !pending.Activated || pending.DiscardedCardIds.Count > 2)
        {
            throw new InvalidOperationException("The completed Ice Sword prevention is invalid.");
        }

        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        _coreNativeCardContinuationCursorsOperations.ActiveIceSword = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IceSwordResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, Used: true, pending.PreventedDamageAmount, Array.AsReadOnly(pending.DiscardedCardIds.ToArray())));
        _coreMatchLogOperations.AddLog("DamagePrevented", $"{(source.Name)} 的【寒冰剑】防止了对 {(target.Name)} 的 {(pending.PreventedDamageAmount)} 点伤害，并依次弃置其 {(pending.DiscardedCardIds.Count)} 张牌。", source.Seat, target.Seat);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void AppendIceSwordLogicalDiscard(NativeOwners.IceSwordHandle pending, Card material)
    {
        if (!_coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards)
            return;
        var attack = pending.Attack;
        var from = _cardZonesOperations._cardZones.GetLocation(material.Id);
        if (from.OwnerSeat != attack.TargetSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Ice Sword must retain its current original target's owned primary discard.");
        var draft = pending.LogicalDiscardInvocation;
        if (draft is null)
        {
            if (pending.DiscardedCardIds.Count != 0)
                throw new InvalidOperationException("Ice Sword lost the beginning of its whole discard invocation.");
            draft = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(attack.SourceSeat, attack.SourceSeat, [material.Id], CardMoveReasons.IceSwordDiscard, pending.OwnerFrameId);
        }
        else
        {
            AssertIceSwordLogicalDiscard(pending);
            if (draft.Materials.Any(m => m.CardId == material.Id))
                throw new InvalidOperationException("Ice Sword cannot discard the same primary material twice.");
            draft = draft with
            {
                Materials = draft.Materials.Append(new(material.Id, from)).ToArray()
            };
        }

        pending.LogicalDiscardInvocation = draft;
    }

    internal void AssertIceSwordLogicalDiscard(NativeOwners.IceSwordHandle pending)
    {
        var draft = pending.LogicalDiscardInvocation;
        if (draft is null)
        {
            if (_coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards && pending.DiscardedCardIds.Count != 0)
                throw new InvalidOperationException("A paid Ice Sword choice lost its typed whole-discard invocation.");
            return;
        }

        var attack = pending.Attack;
        if (!_coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards || draft.InvocationId <= 0 || draft.InvocationId > _coreResolutionIdentityBoundaryOperations._resolutionSequence || draft.TurnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || draft.ActualTurnOwnerSeat != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || draft.InitiatorSeat != attack.SourceSeat || draft.ExecutorSeat != attack.SourceSeat || draft.ParentFrameId != pending.OwnerFrameId || draft.Reason != CardMoveReasons.IceSwordDiscard || !draft.Materials.Select(m => m.CardId).SequenceEqual(pending.DiscardedCardIds) || draft.Materials.Select(m => m.CardId).Distinct().Count() != draft.Materials.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.InvocationId == draft.InvocationId) || draft.Materials.Any(m => m.From.OwnerSeat != attack.TargetSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.CardId == m.CardId && move.Sequence > draft.SequenceBefore && move.From == m.From && move.Reason == draft.Reason && (move.To == CardLocation.DiscardPile || m.From.Zone == CardZoneKind.Equipment && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, move.To) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(move)) && move.To == CardLocation.OutsideGame))))
            throw new InvalidOperationException("Ice Sword lost its exact source, target, paid primary prefix or once-only whole invocation.");
    }
}
