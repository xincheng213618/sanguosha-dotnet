#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeferredCardsRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 10)]
    internal CardSnapshot[] GetPrivatelyViewedCards(int viewerSeat)
    {
        var topCards = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Select(frame => frame.ParticipantTopView).OfType<ProgramParticipantTopView>().Where(view => view.Seat == viewerSeat).SelectMany(view => view.CardIds).Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id)).Select(_coreSnapshotValueProjectionOperations.ToSnapshot);
        return topCards.Concat(_characterMaChengOlOperations.ChengliePrivateCards(viewerSeat)).Concat(_characterTangTangOlOperations.Value.TangTangPrivatelyViewedCards(viewerSeat)).Concat(_characterWangCanOlOperations.DenglouPrivateCards(viewerSeat)).Concat(_characterFanChouOlOperations.Value.FanChouPrivateDeckCards(viewerSeat)).Concat(_characterSimaHuiOlOperations.SimaHuiPrivatelyViewedCards(viewerSeat).Select(_coreSnapshotValueProjectionOperations.ToSnapshot)).Concat(_characterZhugeLiangMouOperations.MouZhitianPrivateCards(viewerSeat)).Concat(_coreDelegatedJudgmentChoicesOperations.Value.DelegatedJudgmentPrivateCards(viewerSeat)).Concat(_corePrivateCardOffersOperations.DeferredPrivateOfferViewedCards(viewerSeat)).Concat(_coreInspectedHandSlashesOperations.Value.InspectedHandPrivateCards(viewerSeat)).Concat(_coreRequestedDeckBasicsOperations.RequestedDeckBasicPrivateCards(viewerSeat)).Concat(_coreProgramPrivateDamageOfferOperationsOperations.GetOfferedPrivatelyViewedCards(viewerSeat)).Concat(_coreQuotaTopProgramsOperations.GetQuotaPrivatelyViewedCards(viewerSeat)).Concat(_coreConvertingGiftsOperations.GetConvertingGiftPrivatelyViewedCards(viewerSeat)).Concat(_coreExactTopReorderProgramsOperations.Value.GetExactTopPrivatelyViewedCards(viewerSeat)).Concat(_corePopulationTopReorderProgramsOperations.Value.GetPopulationTopPrivatelyViewedCards(viewerSeat)).Concat(_coreParticipantHandProgramsOperations.GetParticipantPrivatelyViewedCards(viewerSeat)).Concat(_characterZhugeLiangWumiaoOperations.WuZglPrivateTopCards(viewerSeat)).DistinctBy(card => card.Id).ToArray();
    }

    internal IReadOnlyList<Card> GetDiscardEligibleHand(CharacterState owner)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => !_coreWushuangTacticRulesOperations.IsWushuangHandLimitExempt(owner, card) && card.Kind != CardKind.MantianGuohai && !_coreShenwuExpandedPoliciesOperations.IsShenwuHonglianHandExempt(owner, card) && !_characterDongBaiOlOperations.IsXiaHuiHandLimitExempt(owner.Seat, card) && !_characterTaoQianOlOperations.IsZhaohuoHandLimitExempt(owner.Seat, card.Id) && !_characterXieLingYuOlOperations.IsXinyouHandLimitExempt(owner.Seat, card.Id) && !_characterMiHengLeOperations.IsJiguHandLimitExempt(owner, card.Id) && !_characterZhouFeiLeOperations.IsLingkongHandLimitExempt(owner, card.Id) && !_characterCaiWenJiLeOperations.IsShuangjiaHandLimitExempt(owner, card.Id) && !_characterZouShiLeOperations.IsYunzhengHandLimitExempt(owner, card.Id) && !_characterDiaoChanLeOperations.IsTanbanHandLimitExempt(owner, card.Id) && !_characterZhugeGuoLeOperations.IsLeXidiHandLimitExempt(owner, card.Id) && !_characterCaiYongLeOperations.IsJiaoweiHandLimitExempt(owner, card.Id) && !_characterCaoZhiLeOperations.IsFuleHandLimitExempt(owner, card.Id) && !_characterXiaoQiaoLeOperations.IsQiqinHandLimitExempt(owner, card.Id) && !_characterSunQuanOlMagicOperations.IsMoTianenHandLimitExempt(owner, card.Id) && !_characterLuXunWumiaoOperations.IsWuXiongmuHandLimitExempt(owner, card.Id) && !_characterHuangYueyingMouMobileOperations.IsMouHyJizhiHandLimitExempt(owner, card.Id) && !_characterSimaYiOlMagicOperations.IsMoGuifuHandLimitExempt(owner, card.Id) && !_characterJuShouMouOperations.IsMouJsHandLimitExempt(owner.Seat, card.Id) && !_characterChenLinMouOperations.IsMouChenLinHandLimitExempt(owner, card.Id) && !_characterZuoCiHappyGodOperations.IsHappyZuoCiHandLimitExempt(owner, card.Id) && !_characterLiuLiAnniversaryOperations.IsAnniversaryLiuLiHandLimitExempt(owner.Seat, card) && !_characterCheZhouAnniversaryOperations.Value.IsAnniversaryCheZhouHandExempt(owner, card)).ToArray();
        var exactIds = _turnCardUseEffectsOperations._turnCardUseEffects.GetHandLimitExemptCardIds(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, owner.Seat);
        var exemptKinds = _coreProgramHandLimitKindExemptionsOperations.Value.GetCurrentHandLimitExemptKinds(owner);
        if (!_coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.IgnoreTurnObtainedHandCardsForDiscard))
            return exactIds.Count == 0 && exemptKinds.Count == 0 ? hand : hand.Where(card => !exactIds.Contains(card.Id) && !_coreAlcoholIdentityPoliciesOperations.IsEffectiveHandKindExempt(owner, card, exemptKinds)).ToArray();
        var obtained = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).OfType<CardMovedEvent>().Where(item => item.To == CardLocation.Hand(owner.Seat) && item.From != item.To).Select(item => item.CardId).ToHashSet();
        return hand.Where(card => !obtained.Contains(card.Id) && !exactIds.Contains(card.Id) && !_coreAlcoholIdentityPoliciesOperations.IsEffectiveHandKindExempt(owner, card, exemptKinds)).ToArray();
    }

    internal void ConsumeSkippedNextTurnDrawBenefits(CharacterState current)
    {
        if (current.IsFaceDown)
            _coreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers.RemoveAll(item => item.TargetSeat == current.Seat && item.Query == SkillRuleQuery.DrawCount);
    }

    SkillProgramStepOutcome IDeferredCardProgramHost.ViewTopCardsAndObtainMatchingCards(ProgramSkillFrame frame, int seat, int amount, IReadOnlyList<SkillProgramCardCategory> categories) => BeginParticipantTopView(frame, seat, amount);
    SkillProgramStepOutcome IDeferredCardProgramHost.DepositBoundCardsUntilNextTurn(ProgramSkillFrame frame, string bind) => DepositDeferredPile(frame, bind);
    SkillProgramStepOutcome IDeferredCardProgramHost.ObtainDeferredPile(ProgramSkillFrame frame) => ObtainDeferredPile(frame);
    void IDeferredCardProgramHost.RewardDeferredProviders(ProgramSkillFrame frame) => RewardDeferredProviders(frame);
    internal SkillProgramStepOutcome BeginParticipantTopView(ProgramSkillFrame frame, int seat, int amount)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || seat == frame.OwnerSeat || frame.ParticipantTopView is not null)
            throw new InvalidOperationException("A private top-card view requires a living other participant and no active view.");
        var cards = new List<Card>();
        for (var index = 0; index < amount; index++)
            if (_corePhysicalDrawFacadesOperations.DrawOneToProcessing(new CardMoveReason("skill-program.private-top.view"))is { } card)
                cards.Add(card);
            else
                break;
        if (cards.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ParticipantTopView = new(seat) { CardIds = cards.Select(card => card.Id).ToArray() } });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choice = new PromptChoice(new ChoiceId($"participant-top.frame-{(frame.Id)}.confirm"), string.Join("、", cards.Select(card => card.DisplayName + "（" + card.RankText + "）")), cards.Select(card => card.Id).ToArray(), [], new Dictionary<string, string> { ["program-action"] = "participant-top-view", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "观看牌堆顶的牌并确认。", cards.Select(card => card.Id).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = [choice],
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 观看牌堆顶", skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("participant-top-view")]
    internal void ResolveParticipantTopView(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Private top view lost its frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.ViewTopCardsAndObtainMatchingCards || frame.ParticipantTopView is not { } view || frame.SelectedTargetSeats.Single() != view.Seat || !selected.Cards.SequenceEqual(view.CardIds) || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || view.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing))
            throw new InvalidOperationException("Private top-card confirmation does not match the frozen participant view.");
        var cards = view.CardIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id)).ToArray();
        var gained = cards.Where(card => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(card.Kind, effect.CardCategories)).ToArray();
        var remaining = cards.Except(gained).ToArray();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ParticipantTopView = null, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        // Return non-matching cards first, in their original top-first order. Gain
        // observers therefore cannot draw the unreconciled processing remainder.
        _corePhysicalMovementFacadesOperations.MoveCards(remaining, CardLocation.Processing, CardLocation.DrawPile, new("skill-program.private-top.return"));
        _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(remaining.Select(card => card.Id).ToArray());
        _corePhysicalMovementFacadesOperations.MoveCards(gained, CardLocation.Processing, CardLocation.Hand(view.Seat), new("skill-program.private-top.obtain"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal SkillProgramStepOutcome DepositDeferredPile(ProgramSkillFrame frame, string bind)
    {
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        var provider = frame.SelectedTargetSeats.Single();
        if (provider == frame.OwnerSeat || set.CardIds.Count > 3 || set.CardIds.Where((id, index) => set.SourceLocations[index].OwnerSeat != provider || set.SourceLocations[index].Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(id) != set.SourceLocations[index]).Any())
            throw new InvalidOperationException("Deferred pile deposit must use the provider's actual selected hand/equipment cards.");
        if (set.CardIds.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(set.CardIds, new(CardZoneKind.PublicDeferredPile, frame.OwnerSeat), new("skill-program.deferred-pile.deposit"));
        var realIds = set.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == new CardLocation(CardZoneKind.PublicDeferredPile, frame.OwnerSeat)).ToArray();
        if (realIds.Length > 0)
        {
            var deposit = new DeferredPublicPileDeposit(frame.Id, frame.OwnerSeat, provider, frame.SkillId, frame.SkillInstanceId, realIds, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber);
            _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.Add(deposit);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredPublicPileDepositedEvent(deposit));
        }

        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ObtainDeferredPile(ProgramSkillFrame frame)
    {
        var deposits = _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.Where(item => item.OwnerSeat == frame.OwnerSeat && item.SkillId == frame.SkillId && item.CreatedTurn < _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber).ToArray();
        if (deposits.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var pile = new CardLocation(CardZoneKind.PublicDeferredPile, frame.OwnerSeat);
        var rewards = new List<ProgramDeferredProviderReward>();
        foreach (var deposit in deposits)
        {
            _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.Remove(deposit);
            var ids = deposit.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == pile).ToArray();
            rewards.Add(new(deposit.ProviderSeat, ids.Length));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredPublicPileObtainedEvent(frame.OwnerSeat, deposit.ProviderSeat, ids));
        }

        var actual = deposits.SelectMany(item => item.CardIds).Distinct().Where(id => _cardZonesOperations._cardZones.GetLocation(id) == pile).ToArray();
        frame = frame with
        {
            DeferredProviderRewards = Array.AsReadOnly(rewards.ToArray())
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        if (actual.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(actual.Select(id => _cardZonesOperations._cardZones.CardsAt(pile).Single(card => card.Id == id)).ToArray(), pile, CardLocation.Hand(frame.OwnerSeat), new("skill-program.deferred-pile.obtain"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RewardDeferredProviders(ProgramSkillFrame frame)
    {
        if (frame.DeferredProviderRewards is not { } rewards)
            return;
        // Consume before drawing: a nested window may suspend, but cannot pay again.
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeferredProviderRewards = null });
        foreach (var reward in rewards)
            if (_coreMatchParticipantsPlayersAccess._players[reward.Seat].IsAlive && reward.Count > 0)
                _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, reward.Seat, reward.Count, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.deferred-pile.provider-reward"));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 60)]
    internal void CleanupLostDeferredPileSources()
    {
        foreach (var deposit in _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.Where(item => !_coreMatchParticipantsPlayersAccess._players[item.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[item.OwnerSeat].SkillGrants.Grants.Any(grant => grant.SkillId == item.SkillId && grant.SkillInstanceId == item.SkillInstanceId)).ToArray())
        {
            DiscardDeferredDeposit(deposit);
        }
    }

    internal void DiscardDeferredDeposit(DeferredPublicPileDeposit deposit)
    {
        _coreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits.Remove(deposit);
        var pile = new CardLocation(CardZoneKind.PublicDeferredPile, deposit.OwnerSeat);
        var cards = deposit.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == pile).Select(id => _cardZonesOperations._cardZones.CardsAt(pile).Single(card => card.Id == id)).ToArray();
        _corePhysicalMovementFacadesOperations.MoveCards(cards, pile, CardLocation.DiscardPile, new("skill-program.deferred-pile.invalid-source"));
    }
}
