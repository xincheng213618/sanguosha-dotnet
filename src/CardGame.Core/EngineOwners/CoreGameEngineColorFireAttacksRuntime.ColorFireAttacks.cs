#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineColorFireAttacksRuntime
{
    internal ColorFireAttackReceipt? FreezeColorFireAttackPolicy(int sourceSeat, CardActionContext? action)
    {
        if (action?.EffectiveKind != CardKind.FireAttack)
            return null;
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.RandomRevealColorFireAttack, CardKind.FireAttack).FirstOrDefault();
        return binding.Source is null ? null : new(new(binding.Source.SkillId, binding.Policy.Id, sourceSeat, binding.Source.SkillInstanceId), action.ActionId);
    }

    internal bool TryBeginColorFireAttackReveal(CardUseFrame pending)
    {
        if (pending.ColorFireAttack is not { } issued)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != pending.Id || pending.Action?.ActionId != issued.ActionId || pending.FireAttackSelection?.RevealedCardId is not null || issued.PaidCardId is not null)
            throw new InvalidOperationException("Random color Fire Attack lost its issued use.");
        var target = _coreMatchParticipantsPlayersAccess._players[CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending)];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !target.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0)
        {
            CompleteColorFireAttackWithoutDamage(pending);
            return true;
        }

        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
        var revealed = hand[_randomOperations._random.Next(hand.Count)];
        var receipt = issued with
        {
            TargetSeat = target.Seat,
            RevealedCardId = revealed.Id,
            RevealedSuit = revealed.Suit,
            RevealedIsRed = CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(target, revealed))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { ColorFireAttack = receipt });
        _coreNativeFireAttackOperations.ResolveFireAttackReveal((CardUseFrame)_resolutionStackOperations._resolutionStack.Last(), revealed.Id);
        return true;
    }

    internal IReadOnlyList<Card> ColorFireAttackCosts(CardUseFrame pending) => pending.ColorFireAttack is { RevealedIsRed: { } isRed } && _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].IsAlive ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat])).Where(card => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat], card)) == isRed).OrderBy(card => card.Id).ToArray() : [];
    internal bool TryBeginColorFireAttackDiscard(CardUseFrame pending, Card revealed)
    {
        if (pending.ColorFireAttack is not { } receipt)
            return false;
        if (receipt.RevealedCardId != revealed.Id || receipt.TargetSeat != CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending) || pending.FireAttackSelection?.RevealedCardId != revealed.Id || receipt.PaidCardId is not null)
            throw new InvalidOperationException("Color Fire Attack lost its frozen reveal.");
        var costs = ColorFireAttackCosts(pending);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || costs.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat!.Value].IsAlive)
        {
            CompleteColorFireAttackWithoutDamage(pending);
            return true;
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.FireAttackDiscard, source.Seat, "火攻随机展示已完成，弃置一张同颜色的手牌或装备牌，或放弃伤害。", costs.Select(card => card.Id).ToArray(), [], source.Seat, CardKind.FireAttack)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = receipt.TargetSeat,
            Choices = costs.Select(card => new PromptChoice(new($"fire-attack.discard.card-{(card.Id)}"), $"弃置【{(card.DisplayName)}】，造成火焰伤害。", [card.Id], [], new Dictionary<string, string> { ["response"] = "fire-attack-discard", ["action"] = "discard-same-color" })).Append(new PromptChoice(new("fire-attack.skip"), "放弃伤害", [], [], new Dictionary<string, string> { ["response"] = "fire-attack-skip", ["action"] = "skip-fire-attack" })).ToArray()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return true;
    }

    internal bool TryResolveColorFireAttackDiscard(CardUseFrame pending, int? selectedCardId)
    {
        if (pending.ColorFireAttack is not { } receipt)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != pending.Id || receipt.PaidCardId is not null || receipt.RevealedCardId != pending.FireAttackSelection?.RevealedCardId)
            throw new InvalidOperationException("Color Fire Attack payment is no longer current.");
        if (selectedCardId is null)
        {
            CompleteColorFireAttackWithoutDamage(pending);
            return true;
        }

        var cost = ColorFireAttackCosts(pending).SingleOrDefault(card => card.Id == selectedCardId) ?? throw new InvalidOperationException("Color Fire Attack requires one current same-color HE card.");
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var from = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, cost);
        var color = CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, cost))!.Value;
        receipt = receipt with
        {
            PaidCardId = cost.Id,
            PaidFrom = from,
            PaidIsRed = color,
            PaymentStartSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { ColorFireAttack = receipt });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(source.Seat, source.Seat, cost, from, CardMoveReasons.FireAttackDiscard);
        pending = (CardUseFrame)_resolutionStackOperations._resolutionStack.Single(frame => frame.Id == pending.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { ColorFireAttack = receipt with { PaymentEndSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ColorFireAttackPaidEvent(pending.Id, source.Seat, receipt.TargetSeat!.Value, color, from.Zone == CardZoneKind.Equipment));
        ContinueColorFireAttackPayment(pending.Id);
        return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 140, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(CardUseFrame), 120, typeof(NativeFrameRouteConditions), "ColorFireAttackPayment")]
    internal void ContinueColorFireAttackPayment(long useId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame pending || pending.Id != useId || pending.ColorFireAttack is not { PaidCardId: { } costId } receipt)
            throw new InvalidOperationException("Color Fire Attack lost its exact paid owning frame.");
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(useId, PostEventContinuation.ColorFireAttackPayment) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(useId))
            return;
        var targetSeat = receipt.TargetSeat ?? throw new InvalidOperationException("Paid color Fire Attack lost its frozen target.");
        var canDamage = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(useId, pending.SourceSeat, targetSeat, receipt.RevealedCardId!.Value, receipt.RevealedSuit!.Value, costId, canDamage));
        if (!canDamage)
        {
            CompleteColorFireAttackWithoutDamage(pending, publishFailure: false);
            return;
        }

        var effect = _coreNativeFireAttackOperations.GetFireAttackEffectCard(pending);
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { FireAttackSelection = null, ColorFireAttack = new(receipt.Source, receipt.ActionId) });
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, pending.SourceSeat, targetSeat, (_characterZhouYuMouOperations.IsMouZyZeroFireUse(useId) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroFireAttackUse(useId)) ? null : effect, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(useId));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void CompleteColorFireAttackWithoutDamage(CardUseFrame pending, bool publishFailure = true)
    {
        var receipt = pending.ColorFireAttack!;
        if (publishFailure && receipt.RevealedCardId is { } revealed)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, pending.SourceSeat, receipt.TargetSeat!.Value, revealed, receipt.RevealedSuit!.Value, null, false));
        var effect = _coreNativeFireAttackOperations.GetFireAttackEffectCard(pending);
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { FireAttackSelection = null, ColorFireAttack = new(receipt.Source, receipt.ActionId) });
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.Id, effect);
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.Id, effect, pending.CardKind);
    }

    internal bool TryResolveAiColorFireAttack(CardUseFrame pending, PendingDecision decision)
    {
        if (pending.ColorFireAttack is not { RevealedIsRed: { } red } || decision.Kind != DecisionKind.FireAttackDiscard)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var(id, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseColorFireAttackDiscard(_corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat), CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending), red, decision.ValidCardIds, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeFireAttackOperations.ResolveFireAttackDiscard(pending, id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
