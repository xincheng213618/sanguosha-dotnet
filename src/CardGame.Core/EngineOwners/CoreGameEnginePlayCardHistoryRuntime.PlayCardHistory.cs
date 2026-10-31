#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePlayCardHistoryRuntime
{
    internal bool TracksPlayCardHistory => _contentRegistryOperations._contentRegistry?.ProgramDependencies.TracksPlayCardHistory == true;

    internal IEnumerable<IGameEvent> EventsSinceLastBoundary(Func<IGameEvent, bool> boundary) => _coreProgramEventHistoryOperations.ProgramHistory.SinceLastBoundary(boundary);
    internal CardActionContext? PreviousPlayCard(int actorSeat, long beforeActionId = long.MaxValue)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actorSeat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat)
            return null;
        return EventsSinceLastBoundary(item => item is PhaseChangedEvent or TurnStartedEvent).OfType<CardUseAppearanceCapturedEvent>().Select(item => item.Action).Where(action => action.Type == CardActionType.Use && action.ActorSeat == actorSeat && action.ActionId < beforeActionId).DistinctBy(action => action.ActionId).MaxBy(action => action.ActionId);
    }

    internal Suit? CaptureUsedCardSuit(int actorSeat, IReadOnlyList<int> physicalIds, Card card)
    {
        if (physicalIds.Count == 0)
            return null;
        if (physicalIds.Count == 1)
            return _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actorSeat], card);
        var suits = physicalIds.Select(id => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actorSeat], _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(item => item.Id == id))).Distinct().ToArray();
        return suits.Length == 1 ? suits[0] : null;
    }

    internal bool MatchesPreviousPlayCard(CardActionContext action)
    {
        var previous = PreviousPlayCard(action.ActorSeat, action.ActionId);
        return previous is not null && (action.EffectiveSuit is { } suit && suit != Suit.None && previous.EffectiveSuit == suit || action.EffectiveRank is> 0 && previous.EffectiveRank == action.EffectiveRank);
    }

    internal Suit? PreviousPlaySuitForInheritance(int actorSeat) => PreviousPlayCard(actorSeat)?.EffectiveSuit is { } suit && suit != Suit.None ? suit : null;
    internal bool PreviousPlayCardIsBasic(CardActionContext action)
    {
        var previous = PreviousPlayCard(action.ActorSeat, action.ActionId);
        return previous is not null && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(previous.EffectiveKind, SkillProgramCardCategory.Basic);
    }

    internal bool? PreviousPlayCardIsBasicFor(int actorSeat) => PreviousPlayCard(actorSeat)is { } previous && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(previous.EffectiveKind, SkillProgramCardCategory.Basic);
    internal int DamageInstancesTakenThisTurn(int ownerSeat) => EventsSinceLastBoundary(item => item is TurnStartedEvent).OfType<DamageAppliedEvent>().Count(item => item.TargetSeat == ownerSeat && item.Amount > 0);
    internal string ViewAsUsageId(CardConversionSource source, SkillProgramViewAs rule) => $"view-as:{(rule.UsageGroup ?? rule.Id)}@{(source.SkillInstanceId)}";
    internal bool CanUsePhaseLimitedViewAs(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => rule.EqualNameLengthUse ? _characterMiHengLeOperations.CanUseSiruiPhase(owner, new(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId)) : rule.UsesPerPhase is not { } limit || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && owner.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, instance.SkillId, ViewAsUsageId(new(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId), rule), SkillUsageScope.Phase) < limit);
    internal SkillProgramViewAs? ViewAsRule(CardConversionSource source) => _contentRegistryOperations._contentRegistry?.Skills.GetValueOrDefault(source.SkillId)?.Program is { } program ? _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(program).ViewAs.SingleOrDefault(rule => rule.Id == source.BindingId) : null;
    internal void ConsumeProgramViewAsUsage(IReadOnlyList<CardConversionSource> sources)
    {
        foreach (var source in sources)
        {
            if (_characterMiHengLeOperations.IsSiruiSource(source))
                continue;
            _coreProgramTieredRoundConversionOperations.ConsumeTieredRoundConversion(source);
            _coreNamedUseConversionsOperations.ConsumeNamedUseConversion(source);
            _coreGuanduEventProgramsOperations.ConsumeGuanduRoundConversion(source);
            _coreActualTurnViewAsOperations.ConsumeActualTurnLimitedViewAs(source);
            if (ViewAsRule(source)is not { UsesPerPhase: { } limit } rule)
                continue;
            if (rule.ActivationUsageGroup is { } activationGroup)
            {
                var key = (source.OwnerSeat, source.SkillId, activationGroup);
                if (_coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) >= limit)
                    throw new InvalidOperationException("The shared activation allowance was consumed.");
                _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) + 1;
            }

            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(source.OwnerSeat, source.SkillId, ViewAsUsageId(source, rule), SkillUsageScope.Phase, limit))
                throw new InvalidOperationException("The view-as phase allowance was already consumed.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(source.OwnerSeat, source.SkillId, source.BindingId, SkillUsageScope.Phase, 1));
        }
    }

    internal Card ApplyProgramUseAppearance(CharacterState owner, Card card, CardConversionSource? source) => _characterZhangFeiAnniversaryGodOperations.IsShenZfIdentitySource(owner, card, source) ? card with
    {
        Suit = Suit.None
    }

    : _characterGuanYuWumiaoOperations.ApplyJuewuOwnedAppearance(owner, source is not null && (_characterGuanHaiOlOperations.IsXieweiSource(source) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSource(source)) ? card with { Suit = Suit.None, Rank = 0 } : source is not null && ViewAsRule(source)?.InheritPreviousPlaySuit == true && PreviousPlaySuitForInheritance(owner.Seat)is { } suit ? card with { Suit = suit } : card);
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 10)]
    internal void AddPhaseLimitedBasicCardActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playableCards, int? selectedPhysicalCardId = null)
    {
        if (!_coreNationalSkillsOperations.GetSkillBindingShard(actor).ViewAsPrograms.Any(program => program.ViewAs.Any(rule => rule.UsesPerPhase is not null || rule.UseOnly)))
            return;
        foreach (var card in playableCards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(card => card.Id).Where(card => selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)))
            foreach (var kind in new[]
            {
                CardKind.Slash,
                CardKind.FireSlash,
                CardKind.ThunderSlash,
                CardKind.ColdSlash,
                CardKind.Peach,
                CardKind.Alcohol
            }

            )
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, forResponse: false).Where(source => ViewAsRule(source)is { } rule && (rule.UsesPerPhase is not null || rule.UseOnly) && (!(rule.EqualNameLengthUse || rule.RankTwoDamageUse) || rule.OutputKind == kind)))
                {
                    if (actions.Any(action => action.CardId == card.Id && action.ConversionSource == source && action.PlayedCardKind == kind))
                        continue;
                    var usedCard = ApplyProgramUseAppearance(actor, card, source);
                    var name = CardCatalog.Get(kind).DisplayName;
                    if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
                    {
                        var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, usedCard, source, effectiveKind: kind);
                        foreach (var target in targets)
                            actions.Add(new LegalAction(LegalActionKind.Slash, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"将【{(card.DisplayName)}】当作【{(name)}】对 {(target.Name)} 使用"), PlayedCardKind: kind) { ConversionSource = source });
                        _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, usedCard, targets, name, kind, source);
                        _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, usedCard, targets, name, kind, source);
                    }
                    else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) || kind == CardKind.Alcohol && !actor.HasAlcoholEffect && (_coreBoundDiscardSlashBenefitsOperations.HasTurnAlcoholSuitAllowance(actor, card, source) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, kind, [card.Id], source) || _characterMiHengLeOperations.SiruiIgnoresAlcoholLimit(actor, source) || _characterSunQuanAnniversaryGodOperations.IsAnniversaryChouxiSource(actor, source) || !actor.UsedPlayPhaseAlcoholThisTurn || _characterWeiYanMobileShiOperations.HasMobileShiZhuangshiQuota(actor.Seat) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [card.Id])))
                        actions.Add(new LegalAction(kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, card.Id, kind == CardKind.Peach ? actor.Seat : null, _coreCardConversionsOperations.DescribeConversion(source, $"将【{(card.DisplayName)}】当作【{(name)}】使用"), PlayedCardKind: kind) { ConversionSource = source });
                }
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 10)]
    internal bool PhaseConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => !CanUsePhaseLimitedViewAs(query.Instance, query.Rule, query.Owner);
}
