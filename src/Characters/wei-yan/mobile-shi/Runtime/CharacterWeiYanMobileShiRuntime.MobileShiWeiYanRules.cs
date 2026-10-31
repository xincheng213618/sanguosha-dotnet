#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanMobileShiRuntime
{
    internal bool HasMobileShiZhuangshiDistance(int seat, long? existingUse = null)
    {
        if (existingUse is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action is { } action)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Any(e => e.UseId == id && e.ActionId == action.ActionId && e.OwnerSeat == seat && e.DistanceAndNoResponse);
        return ShiWyPhaseGrant(seat)is { } grant && ShiWyPhaseUses(seat, grant.TurnNumber, grant.PhaseInstanceId) < grant.DiscardedCards;
    }

    internal bool HasMobileShiZhuangshiQuota(int seat) => ShiWyPhaseGrant(seat)is { } grant && ShiWyPhaseUses(seat, grant.TurnNumber, grant.PhaseInstanceId) < grant.LostHp;
    internal CardConversionSource? MobileShiZhuangshiCounterspellSource(CharacterState owner, IReadOnlyList<int> physicalCardIds) => HasMobileShiZhuangshiDistance(owner.Seat) ? ShiWyPhaseGrant(owner.Seat)?.Source : null;
    internal bool IsMobileShiZhuangshiUnrespondable(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } action } && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Any(e => e.UseId == id && e.ActionId == action.ActionId && e.DistanceAndNoResponse);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 47100)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 47100)]
    internal bool IsIssuedMobileShiZhuangshiNoCount(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } action } && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Any(e => e.UseId == id && e.ActionId == action.ActionId && e.OwnerSeat == actor && e.NoCount);
    internal int MobileShiWeiYanPhaseSlashBonus(int seat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat ? _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiKuangguSlashGrantedEvent>().Count(e => e.OwnerSeat == seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : 0;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 47100)]
    internal bool MobileShiZhuangshiCardDistance(ProgramCardDistanceQuery q) => HasMobileShiZhuangshiDistance(q.Actor.Seat, q.CardUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 47100)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 47100)]
    internal bool MobileShiZhuangshiSlashDistance(ProgramSlashDistanceQuery q) => HasMobileShiZhuangshiDistance(q.Actor.Seat, q.ExistingUseFrameId);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 47100)]
    internal bool MobileShiZhuangshiSlashQuota(ProgramSlashQuotaQuery q) => HasMobileShiZhuangshiQuota(q.Actor.Seat);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 47100)]
    internal void CaptureMobileShiWeiYanUse(IGameEvent payload)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use && use.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && use.Action is { Type: CardActionType.Use } action)
        {
            var grant = ShiWyPhaseGrant(use.SourceSeat);
            if (grant is null || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Any(e => e.ActionId == action.ActionId))
                return;
            var ordinal = ShiWyPhaseUses(use.SourceSeat, grant.TurnNumber, grant.PhaseInstanceId) + 1;
            var noCount = ordinal <= grant.LostHp;
            if (noCount)
            {
                _coreCardUseLifecycleOperations.TryMarkUnlimitedUse(use.Id);
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { QuotaOnlyUseLimitExemption = true });
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiWeiYanUseEvent(use.Id, action.ActionId, use.SourceSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, ordinal, ordinal <= grant.DiscardedCards, noCount));
        }
        else if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } response)
        {
            var a = response.Action;
            if (a.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || a.ProviderSeat != a.ActorSeat || a.RequesterSeat is not null || !(a.EffectiveKind == CardKind.Nullification || a.EffectiveKind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], a.EffectiveKind)) || ShiWyPhaseGrant(a.ActorSeat)is not { } grant || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWeiYanUseEvent>().Any(e => e.ActionId == a.ActionId))
                return;
            var ordinal = ShiWyPhaseUses(a.ActorSeat, grant.TurnNumber, grant.PhaseInstanceId) + 1;
            var owner = a.EffectiveKind == CardKind.Nullification ? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id ?? 0 : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId ?? 0;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiWeiYanUseEvent(owner, a.ActionId, a.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, ordinal, ordinal <= grant.DiscardedCards, ordinal <= grant.LostHp));
        }
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 47100)]
    internal int MobileShiYinzhanDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || attack.CardUserSeat != attack.SourceSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Duel) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreNativeSkillContextQueriesOperations.HasProgramSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], ShiWyYinzhan) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { CardKind: CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash })
            return 0;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var hp = source.HasHpValue && target.HasHpValue && source.Hp <= target.Hp;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(source).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiYinzhanQualifiedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, attack.ResolutionId, source.Seat, target.Seat, hp, hand));
        return !attack.IsChainPropagation && hp ? 1 : 0;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 47101)]
    internal void CaptureMobileShiYinzhanApplied(DamageAppliedEvent damage)
    {
        if (damage.SourceCardUseResolutionId is not { } useId || damage.Amount <= 0 || damage.SourceLess)
            return;
        var qualification = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanQualifiedEvent>().LastOrDefault(e => e.UseId == useId && e.OwnerSeat == damage.SourceSeat && e.TargetSeat == damage.TargetSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiYinzhanAppliedEvent>().Any(a => a.QualificationId == e.Id));
        if (qualification is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiYinzhanAppliedEvent(qualification.Id));
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiWeiYanPublicStates(CharacterState owner, string skillId)
    {
        var states = new List<ProgramPublicRuleStateSnapshot>();
        if (skillId == ShiWyZhuangshi && ShiWyPhaseGrant(owner.Seat)is { } grant)
        {
            var instance = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.FirstOrDefault(i => i.SkillId == skillId);
            if (instance is not null)
            {
                var used = ShiWyPhaseUses(owner.Seat, grant.TurnNumber, grant.PhaseInstanceId);
                states.Add(new(instance.SkillInstanceId, "zhuangshi-discard", ProgramPublicRuleStateKind.ActivationLimit, grant.DiscardedCards, Math.Min(used, grant.DiscardedCards), SkillUsageScope.Phase));
                states.Add(new(instance.SkillInstanceId, "zhuangshi-hp", ProgramPublicRuleStateKind.ActivationLimit, grant.LostHp, Math.Min(used, grant.LostHp), SkillUsageScope.Phase));
            }
        }

        return Array.AsReadOnly(states.ToArray());
    }
}
