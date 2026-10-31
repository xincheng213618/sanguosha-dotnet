#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeLegalActionsRuntime
{
    internal IReadOnlyList<LegalAction> GetLegalActions() => GetHumanLegalActions();
    internal IReadOnlyList<LegalAction> GetHumanLegalActions()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } pending || pending.PlayerSeat != _coreMatchDefinitionOptionsAccess._options.HumanSeat)
        {
            return[];
        }

        return pending.Kind == DecisionKind.PlayCard ? BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat]) : pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash ? _coreNationalGeneralRevealOperations.BuildNationalRevealActions(_coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat]) : [];
    }

    internal IReadOnlyList<LegalAction> BuildLegalActions(CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = null) => _coreHealthQueriesOperations.HappyQiaoActionQueryUnion(() => BuildLegalActionsCore(actor, includeProgramActions, selectedPhysicalCardId, _characterCaoAngMouOperations.MouCaQueryOriginalCard(actor, selectedPhysicalCardId) ?? _characterWeiYanHappyGodOperations.HappyWeiYanBoneReturnCard(actor, selectedPhysicalCardId) ?? _characterZhangHuanAnniversaryOperations.AnniversaryZhangHuanQueryOriginalCard(actor, selectedPhysicalCardId) ?? _characterZhouFeiBoundaryMobileOperations.FindBoundaryZhouFeiUseCard(actor, selectedPhysicalCardId)));
    internal IReadOnlyList<LegalAction> BuildLegalActionsCore(CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, Card? nativeOriginalCard, bool ignoreForcedHandSelection = false)
    {
        using var queryScope = _coreActionQueriesOperations.BeginActionQuery();
        var actions = new List<LegalAction>();
        if (!actor.IsAlive || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && !_characterHuangQuanOlOperations.IsJianjiExactCardQuery(actor, selectedPhysicalCardId)) && !_characterHuBanOlOperations.IsHuiyunExactHandQuery(actor, selectedPhysicalCardId) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !_characterDuanJiongOlOperations.IsDuanJiongSaoguDiscardQuery(actor, selectedPhysicalCardId) && !_characterYaDanOlOperations.IsQingyaDiscardQuery(actor, selectedPhysicalCardId) && !_characterGuoYouZhiMouMobileOperations.Value.IsMouGyzExactCardQuery(actor, selectedPhysicalCardId) && !_characterJiangWeiMouMobileOperations.IsMouJwSlashQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor) && !_characterZhouYuMouOperations.IsMouZyExactCardQuery(actor, selectedPhysicalCardId) && !_characterJiangJiMouOperations.IsMouJjExactCardQuery(actor, selectedPhysicalCardId) && !_characterCaoAngMouOperations.IsMouCaExactCardQuery(actor, selectedPhysicalCardId) && !_characterJuShouMouOperations.IsMouJsExactCardQuery(actor, selectedPhysicalCardId) && !_characterJiaXuMouOperations.IsMouJxExactCardQuery(actor, selectedPhysicalCardId) && !_characterWeiYanHappyGodOperations.IsHappyWeiYanBoneReturnQuery(actor, selectedPhysicalCardId) && !_characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId) && !_characterBaiLingYunAnniversaryOperations.IsAnniversaryBaiLingYunExactCardQuery(actor, selectedPhysicalCardId) && !_characterSunHuanAnniversaryOperations.Value.IsAnniversarySunHuanExactCardQuery(actor, selectedPhysicalCardId) && !_characterZhangHuanAnniversaryOperations.IsAnniversaryZhangHuanExactCardQuery(actor, selectedPhysicalCardId) && !_characterGuoYuanMobileShiOperations.IsShiGyHandQuery(actor, selectedPhysicalCardId) && !_characterZhouFeiBoundaryMobileOperations.IsBoundaryZhouFeiUseQuery(actor, selectedPhysicalCardId))
        {
            return actions;
        }

        bool IncludesPhysicalCard(Card card) => selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId;
        if (selectedPhysicalCardId is null or 0)
        {
            actions.AddRange(_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessPlayActions(actor));
            actions.AddRange(_coreTieredRoundZeroUsesOperations.TieredRoundZeroPlayActions(actor));
            actions.AddRange(_coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicPlayActions(actor));
            actions.AddRange(_coreChainedStateBasicConversionsOperations.ChainedStateBasicPlayActions(actor));
        }

        if (selectedPhysicalCardId is null && _coreNativeDamageHandlesOperations.Value.IsNationalWarMode)
        {
            actions.AddRange(_coreNationalGeneralRevealOperations.BuildNationalRevealActions(actor));
        }

        // An original public-pile query is a separate one-entity producer pool.
        // It never changes GetPlayableCards or any hand/equipment cost source.
        var allPhysicalPlayableCards = nativeOriginalCard is null ? _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor) : [nativeOriginalCard];
        var physicalPlayableCards = allPhysicalPlayableCards.Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)).ToArray();
        // Narrow only action producers. Whole-hand rules and the Slash gate
        // still read the complete pools, including unselected bonus materials.
        var physicalCardCandidates = selectedPhysicalCardId is null ? physicalPlayableCards : physicalPlayableCards.Where(IncludesPhysicalCard).ToArray();
        IReadOnlyList<Card> allPhysicalCardCandidates = selectedPhysicalCardId is null ? allPhysicalPlayableCards : allPhysicalPlayableCards.Where(IncludesPhysicalCard).ToArray();
        var playableCards = physicalCardCandidates.Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)).ToArray();
        var slashLimit = _coreRuleQueriesOperations.GetSlashUseLimit(actor);
        if (CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use)) && (_coreWushuangSkillsBOperations.HasWushuangBianyuPotential(actor) || _characterGuanYuWumiaoOperations.HasWuyouPotentialSlash(actor) || (_characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(actor) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(actor)) || physicalPlayableCards.Any(card => _characterSunYuanOlOperations.HasDongxinEntitySlashQuota(actor, CardKind.Slash, [card.Id])) || physicalPlayableCards.Any(card => _characterHuangQuanMobileShiOperations.ShiQuanGushiUnlimitedCard(actor, CardKind.Slash, [card.Id])) || _characterYaDanOlOperations.IsQingyaDiscardQuery(actor, selectedPhysicalCardId) || _characterFuRongOlOperations.IsFuRongDiscardQuery(actor, selectedPhysicalCardId) || _characterDuanJiongOlOperations.IsDuanJiongSaoguDiscardQuery(actor, selectedPhysicalCardId) || _characterHuangQuanOlOperations.IsJianjiExactCardQuery(actor, selectedPhysicalCardId) || _characterGuoYouZhiMouMobileOperations.Value.IsMouGyzExactCardQuery(actor, selectedPhysicalCardId) || _characterJiangWeiMouMobileOperations.IsMouJwSlashQuery(actor) || (_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) || _characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor) || _characterZhouYuMouOperations.IsMouZyExactCardQuery(actor, selectedPhysicalCardId) || _characterJiaXuMouOperations.IsMouJxExactCardQuery(actor, selectedPhysicalCardId) || _characterWeiYanHappyGodOperations.IsHappyWeiYanBoneReturnQuery(actor, selectedPhysicalCardId) || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId) || _characterBaiLingYunAnniversaryOperations.IsAnniversaryBaiLingYunExactCardQuery(actor, selectedPhysicalCardId) || _characterSunHuanAnniversaryOperations.Value.IsAnniversarySunHuanExactCardQuery(actor, selectedPhysicalCardId) || _characterZhangHuanAnniversaryOperations.IsAnniversaryZhangHuanExactCardQuery(actor, selectedPhysicalCardId) || _characterGuoYuanMobileShiOperations.IsShiGyOffTurnQuery(actor, selectedPhysicalCardId) || _characterZhouFeiBoundaryMobileOperations.IsBoundaryZhouFeiUseQuery(actor, selectedPhysicalCardId) || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _characterJiangJiMouOperations.IsMouJjExactCardQuery(actor, selectedPhysicalCardId) || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat) && _characterJuShouMouOperations.IsMouJsExactCardQuery(actor, selectedPhysicalCardId) || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat) && _characterCaoAngMouOperations.IsMouCaExactCardQuery(actor, selectedPhysicalCardId) || _characterHuBanOlOperations.IsHuiyunExactHandQuery(actor, selectedPhysicalCardId) || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) || _coreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess._slashCountThisTurn < slashLimit || _coreDirectedCardPolicyQueriesOperations.HasSlashAllowanceForAnyTarget(actor) || _characterSunJunMobileShiOperations.Value.HasShiSunJunFirstSuitAllowance(actor) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianPotential(actor.Seat) || _coreJudgedRankSlashPoliciesOperations.CurrentJudgedRankSlashPolicy(actor.Seat)is not null || physicalPlayableCards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(actor, forResponse: false)).Any(card => _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Slash, [card.Id])) || physicalPlayableCards.Any(card => _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).Any(match => _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, match.Source)))))
        {
            foreach (var transformed in physicalCardCandidates.SelectMany(card => _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).Where(match => CoreNativeCardClassificationsRuntime.IsSlashCard(match.Identity.OutputKind)).Select(match => (Card: card, Match: match))))
            {
                var card = transformed.Card;
                var conversionSource = transformed.Match.Source;
                foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants(actor, card, transformed.Match.Identity.OutputKind, conversionSource))
                {
                    var effectiveKind = variant.EffectiveKind;
                    IReadOnlyList<CardConversionSource>? additionalConversions = variant.AdditionalConversionSource is { } additional ? [additional] : null;
                    var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, conversionSource, effectiveKind: effectiveKind);
                    var physicalName = CardCatalog.Get(card.Kind).DisplayName;
                    var effectiveName = CardCatalog.Get(effectiveKind).DisplayName;
                    foreach (var target in targets)
                    {
                        actions.Add(new LegalAction(LegalActionKind.Slash, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(conversionSource, variant.AdditionalConversionSource is { } chained ? $"将【{(physicalName)}】视为【杀】，再发动【{(_coreProgramCardUseRulesOperations.DescribeAdditionalConversion(chained))}】改为【{(effectiveName)}】对 {(target.Name)} 使用" : variant.UsesZhuqueFan ? $"将【{(physicalName)}】视为【杀】，再以【朱雀羽扇】改为【火杀】对 {(target.Name)} 使用" : $"将【{(physicalName)}】视为【{(effectiveName)}】对 {(target.Name)} 使用"), PlayedCardKind: effectiveKind) { ConversionSource = conversionSource, AdditionalConversionSources = additionalConversions });
                    }

                    _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, card, targets, effectiveName, effectiveKind, conversionSource, additionalConversions);
                    _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, effectiveName, effectiveKind, conversionSource, additionalConversions);
                }
            }

            foreach (var slash in playableCards.Where(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind)))
            {
                foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants2(actor, slash.Kind))
                {
                    var effectiveKind = variant.EffectiveKind;
                    var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, slash, effectiveKind: effectiveKind);
                    var slashName = CardCatalog.Get(effectiveKind).DisplayName;
                    CardKind? playedCardKind = effectiveKind == slash.Kind ? null : effectiveKind;
                    foreach (var target in targets)
                    {
                        actions.Add(new LegalAction(LegalActionKind.Slash, slash.Id, target.Seat, variant.UsesZhuqueFan ? $"发动【朱雀羽扇】，将【杀】改为【火杀】对 {(target.Name)} 使用" : $"对 {(target.Name)} 使用【{(slashName)}】", PlayedCardKind: playedCardKind));
                    }

                    _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, slash, targets, slashName, playedCardKind);
                    _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, slash, targets, slashName, playedCardKind);
                }
            }

            // Every physical Slash conversion uses the same variant and target producers,
            // including a subsequent conversion of its declared elemental identity.
            var conversionCards = playableCards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor))
                .Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor))
                .Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(actor, forResponse: false))
                .Where(IncludesPhysicalCard).DistinctBy(card => card.Id)
                .Where(card => !_coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(actor.Seat, card.Id));
            foreach (var converted in conversionCards)
            {
                var sources = CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds
                    .SelectMany(kind => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, kind, forResponse: false)
                        .Select(source => (Kind: kind, Source: source)))
                    .DistinctBy(item => item.Source);
                foreach (var (kind, conversionSource) in sources)
                    foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants(actor, converted, kind, conversionSource))
                    {
                        IReadOnlyList<CardConversionSource>? additionalConversions = variant.AdditionalConversionSource is { } additional ? [additional] : null;
                        var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, converted, conversionSource, effectiveKind: variant.EffectiveKind);
                        var effectiveName = CardCatalog.Get(variant.EffectiveKind).DisplayName;
                        foreach (var target in targets)
                        {
                            var description = variant.AdditionalConversionSource is { } chained
                                ? $"将【{converted.DisplayName}】转换后，再发动【{_coreProgramCardUseRulesOperations.DescribeAdditionalConversion(chained)}】改为【{effectiveName}】对 {target.Name} 使用"
                                : variant.UsesZhuqueFan
                                    ? $"将【{converted.DisplayName}】当作【杀】，再以【朱雀羽扇】改为【火杀】对 {target.Name} 使用"
                                    : $"将【{converted.DisplayName}】当作【{effectiveName}】对 {target.Name} 使用";
                            actions.Add(new LegalAction(LegalActionKind.Slash, converted.Id, target.Seat,
                                _coreCardConversionsOperations.DescribeConversion(conversionSource, description), PlayedCardKind: variant.EffectiveKind)
                                { ConversionSource = conversionSource, AdditionalConversionSources = additionalConversions });
                        }
                        if (_corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(card => card.Id == converted.Id))
                            _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, converted, targets, effectiveName, variant.EffectiveKind, conversionSource, additionalConversions);
                        _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, converted, targets, effectiveName, variant.EffectiveKind, conversionSource, additionalConversions);
                    }
            }

            if (selectedPhysicalCardId is null && _coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(actor))
            {
                var groupTargets = _coreZhangbaSelectionsOperations.Value.GetZhangbaSlashGroupTargets(actor);
                var targets = groupTargets.Select(group => group.TargetSeat).Distinct().Order().ToArray();
                if (targets.Length > 0)
                {
                    actions.Add(new LegalAction(LegalActionKind.UseEquipmentEffect, null, null, "发动【丈八蛇矛】，将两张手牌当【杀】使用", MinCardCount: 2, MaxCardCount: 2, MinTargetCount: 1, MaxTargetCount: 1, EquipmentKind: CardKind.ZhangbaSerpentSpear) { SelectableCardIds = groupTargets.SelectMany(group => group.CardIds).Distinct().Order().ToArray(), SelectableTargetSeats = targets, SelectableCardGroupTargets = groupTargets });
                }
            }
        }
        else if (CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use)))
        {
            _coreCardConversionsOperations.AddNuzhanUnlimitedTrickSlashActions(actions, actor, playableCards);
        }

        if (_coreTreatedWoundedOperations.IsRuleWounded(actor))
        {
            foreach (var converted in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Where(IncludesPhysicalCard))
                foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Peach, false))
                    actions.Add(new LegalAction(LegalActionKind.Peach, converted.Id, actor.Seat, _coreCardConversionsOperations.DescribeConversion(conversion, "当作【桃】对自己使用"), PlayedCardKind: CardKind.Peach) { ConversionSource = conversion });
            foreach (var peach in playableCards.Where(card => card.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(actor, card)))
            {
                actions.Add(new LegalAction(LegalActionKind.Peach, peach.Id, actor.Seat, "对自己使用【桃】"));
            }
        }

        foreach (var duel in playableCards.Where(card => card.Kind == CardKind.Duel))
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Duel, duel.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, duel)))))
            {
                actions.Add(new LegalAction(LegalActionKind.Duel, duel.Id, target.Seat, $"对 {(target.Name)} 使用【决斗】"));
            }
        }

        foreach (var converted in playableCards.Where(card => card.Kind != CardKind.Duel))
        {
            foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Duel, forResponse: false))
            {
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Duel, converted.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, converted, conversionSource))))))
                {
                    actions.Add(new LegalAction(LegalActionKind.Duel, converted.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(conversionSource, $"将【{(converted.DisplayName)}】当作【决斗】对 {(target.Name)} 使用"), PlayedCardKind: CardKind.Duel) { ConversionSource = conversionSource });
                }
            }
        }

        _coreProgramMultiTargetDuelsOperations.AddProgramMultiTargetDuelActions(actions, actor);
        _coreHulaoCardsOperations.Value.AddHulaoAllianceFeastActions(actions, actor, playableCards);
        _coreShenwuCardsOperations.AddShenwuTrickActions(actions, actor, playableCards);
        _coreMantianGuohaiOperations.AddMantianGuohaiActions(actions, actor, playableCards);
        _coreHellSoulCardsOperations.AddHellSoulActions(actions, actor, playableCards);
        _coreWenheCardsOperations.AddWenheCardActions(actions, actor, playableCards);
        _coreGuanduCardsOperations.AddGuanduCardActions(actions, actor, playableCards);
        _coreDragonBoatCardsOperations.AddDragonBoatCardActions(actions, actor, playableCards);
        foreach (var drawTwo in playableCards.Where(card => card.Kind == CardKind.DrawTwo))
        {
            actions.Add(new LegalAction(LegalActionKind.DrawTwo, drawTwo.Id, null, "使用【无中生有】摸两张牌"));
        }

        foreach (var assault in playableCards.Where(card => card.Kind == CardKind.BarbarianAssault))
        {
            if (_coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(actor, CardKind.BarbarianAssault))
                actions.Add(new LegalAction(LegalActionKind.BarbarianAssault, assault.Id, null, "使用【南蛮入侵】"));
        }

        foreach (var arrowBarrage in playableCards.Where(card => card.Kind == CardKind.ArrowBarrage))
        {
            if (_coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(actor, CardKind.ArrowBarrage))
                actions.Add(new LegalAction(LegalActionKind.ArrowBarrage, arrowBarrage.Id, null, "使用【万箭齐发】"));
        }

        foreach (var peachGarden in playableCards.Where(card => card.Kind == CardKind.PeachGarden))
        {
            if (_coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(actor, CardKind.PeachGarden))
                actions.Add(new LegalAction(LegalActionKind.PeachGarden, peachGarden.Id, null, "使用【桃园结义】"));
        }

        foreach (var fiveGrains in playableCards.Where(card => card.Kind == CardKind.FiveGrains))
        {
            if (_coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(actor, CardKind.FiveGrains))
                actions.Add(new LegalAction(LegalActionKind.FiveGrains, fiveGrains.Id, null, "使用【五谷丰登】"));
        }

        foreach (var indulgence in playableCards.Where(card => card.Kind == CardKind.Indulgence))
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !player.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(player, CardKind.Indulgence)))
            {
                actions.Add(new LegalAction(LegalActionKind.Indulgence, indulgence.Id, target.Seat, $"对 {(target.Name)} 使用【乐不思蜀】"));
            }
        }

        {
            foreach (var converted in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Where(IncludesPhysicalCard).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)).Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)).Where(card => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Indulgence, forResponse: false).Count != 0))
            {
                var physicalName = CardCatalog.Get(converted.Kind).DisplayName;
                foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Indulgence, forResponse: false))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !player.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(player, CardKind.Indulgence)))
                    {
                        actions.Add(new LegalAction(LegalActionKind.Indulgence, converted.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(conversionSource, $"将【{(physicalName)}】当作【乐不思蜀】对 {(target.Name)} 使用"), PlayedCardKind: CardKind.Indulgence) { ConversionSource = conversionSource });
                    }
            }
        }

        foreach (var supplyShortage in playableCards.Where(card => card.Kind == CardKind.SupplyShortage))
        {
            var ignoresDistance = _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.SupplyShortage) || _characterFuRongOlOperations.HasFuRongDiscardDistance(actor, CardKind.SupplyShortage, [supplyShortage.Id]) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(actor, supplyShortage) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.SupplyShortage, [supplyShortage.Id]);
            var distanceLimit = _coreRuleQueriesOperations.GetCardUseDistanceLimit(actor, CardKind.SupplyShortage);
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !player.JudgmentAreaAbolished && ((_characterGuanYuRuntimeOperations.GuanFirstRedDistance(actor, supplyShortage) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, supplyShortage) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, player, CardKind.SupplyShortage, physicalCardIds: [supplyShortage.Id]) || ignoresDistance || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, player.Seat) <= distanceLimit)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.SupplyShortage) && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(player, CardKind.SupplyShortage)))
            {
                actions.Add(new LegalAction(LegalActionKind.SupplyShortage, supplyShortage.Id, target.Seat, $"对 {(target.Name)} 使用【兵粮寸断】"));
            }
        }

        {
            var distanceLimit = _coreRuleQueriesOperations.GetCardUseDistanceLimit(actor, CardKind.SupplyShortage);
            var convertedCards = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Where(IncludesPhysicalCard).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)).Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)).Where(card => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.SupplyShortage, forResponse: false).Count != 0);
            foreach (var converted in convertedCards)
            {
                var physicalName = CardCatalog.Get(converted.Kind).DisplayName;
                foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.SupplyShortage, forResponse: false))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !player.JudgmentAreaAbolished && (_characterGuanYuRuntimeOperations.GuanFirstRedDistance(actor, converted) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, converted) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, player, CardKind.SupplyShortage, excludedEquipmentId: converted.Id, physicalCardIds: [converted.Id]) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.SupplyShortage, [converted.Id], conversionSource) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.SupplyShortage) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(actor, converted) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, player.Seat) <= distanceLimit) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.SupplyShortage) && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(player, CardKind.SupplyShortage)))
                    {
                        actions.Add(new LegalAction(LegalActionKind.SupplyShortage, converted.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(conversionSource, $"将【{(physicalName)}】当作【兵粮寸断】对 {(target.Name)} 使用"), PlayedCardKind: CardKind.SupplyShortage) { ConversionSource = conversionSource });
                    }
            }
        }

        foreach (var lightning in playableCards.Where(card => card.Kind == CardKind.Lightning))
        {
            if (!actor.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(actor, CardKind.Lightning))
            {
                actions.Add(new LegalAction(LegalActionKind.Lightning, lightning.Id, actor.Seat, "对自己使用【闪电】"));
            }
        }

        var ironChainTargets = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).OrderBy(player => player.Seat).ToArray();
        {
            foreach (var ironChain in allPhysicalCardCandidates.Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card) && (card.Kind == CardKind.IronChain || _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(h => h.Id == card.Id) && (_coreModeCardRecastsOperations.Value.CanModeHandEquipmentRecast(card) || _coreHellDizangProgramsOperations.CanHellDitingMountRecast(actor, card)))))
            {
                actions.Add(new LegalAction(LegalActionKind.Recast, ironChain.Id, null, $"重铸【{(ironChain.DisplayName)}】，摸一张牌"));
            }
        }

        foreach (var ironChain in playableCards.Where(card => card.Kind == CardKind.IronChain))
            _coreProgramIronChainTargetsOperations.AddProgramIronChainUseActions(actions, actor, ironChain, ironChainTargets);
        foreach (var converted in allPhysicalCardCandidates.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(card => card.Id).Where(IncludesPhysicalCard).Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card) && card.Kind != CardKind.IronChain))
        {
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.IronChain, forResponse: false))
            {
                actions.Add(new LegalAction(LegalActionKind.Recast, converted.Id, null, $"将【{(converted.DisplayName)}】当【铁索连环】重铸并摸一张牌", PlayedCardKind: CardKind.IronChain) { ConversionSource = source });
                if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, converted))
                    continue;
                _coreProgramIronChainTargetsOperations.AddProgramIronChainUseActions(actions, actor, converted, ironChainTargets, CardKind.IronChain, source);
            }
        }

        if (actor.HasHpValue && (!actor.HasAlcoholEffect || _characterFuRongOlOperations.IsFuRongDiscardQuery(actor, selectedPhysicalCardId) || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId)))
        {
            foreach (var alcohol in playableCards.Where(card => card.Kind == CardKind.Alcohol && (_characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Alcohol, [card.Id]) || _coreBoundDiscardSlashBenefitsOperations.HasTurnAlcoholSuitAllowance(actor, card) || _characterFuRongOlOperations.IsFuRongDiscardQuery(actor, card.Id) || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, selectedPhysicalCardId) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || (_coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor)) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [card.Id]))))
            {
                actions.Add(new LegalAction(LegalActionKind.Alcohol, alcohol.Id, null, "使用【酒】，本回合下一张杀伤害+1"));
            }

            foreach (var card in playableCards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(card => card.Id).Where(IncludesPhysicalCard).Where(card => _coreBoundDiscardSlashBenefitsOperations.HasTurnAlcoholSuitAllowance(actor) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Alcohol, [card.Id]) || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, selectedPhysicalCardId) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || (_coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor)) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [card.Id])))
                foreach (var conversion in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Alcohol, forResponse: false).Where(conversion => _coreBoundDiscardSlashBenefitsOperations.HasTurnAlcoholSuitAllowance(actor, card, conversion) || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(actor, selectedPhysicalCardId) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, selectedPhysicalCardId) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [card.Id]) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Alcohol, [card.Id], conversion)))
                    actions.Add(new LegalAction(LegalActionKind.Alcohol, card.Id, null, _coreCardConversionsOperations.DescribeConversion(conversion, $"将【{(card.DisplayName)}】当作【酒】使用"), PlayedCardKind: CardKind.Alcohol) { ConversionSource = conversion });
        }

        foreach (var equipment in playableCards.Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && actor.EquipmentSlotCapacity(_coreFieldEquipmentOperations.FieldEquipmentSlot(card)) > 0))
        {
            var definition = _coreEquipmentContentOperations.EquipmentRules.Get(equipment.Kind);
            actions.Add(new LegalAction(LegalActionKind.Equip, equipment.Id, null, $"装备【{(definition.DisplayName)}】至{(EquipmentCatalog.GetSlotName(definition.Slot))}槽"));
        }

        foreach (var dismantlement in playableCards.Where(card => card.Kind == CardKind.Dismantlement))
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player)))
            {
                _coreNativeMultiTargetActionsOperations.AddTargetCardActions(actions, actor.Seat, LegalActionKind.Dismantlement, dismantlement, target, "过河拆桥");
            }
        }

        {
            var qixiCards = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Where(IncludesPhysicalCard).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)).Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)).Where(card => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Dismantlement, forResponse: false).Count != 0);
            foreach (var converted in qixiCards)
            {
                foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Dismantlement, forResponse: false))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player)))
                    {
                        _coreNativeMultiTargetActionsOperations.AddTargetCardActions(actions, actor.Seat, LegalActionKind.Dismantlement, converted, target, "过河拆桥", CardKind.Dismantlement, conversionSource);
                    }
            }
        }

        foreach (var snatch in playableCards.Where(card => card.Kind == CardKind.Snatch))
        {
            var ignoresDistance = _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.Snatch) || _characterFuRongOlOperations.HasFuRongDiscardDistance(actor, CardKind.Snatch, [snatch.Id]) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(actor, snatch) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Snatch, [snatch.Id]);
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && (_characterGuanYuRuntimeOperations.GuanFirstRedDistance(actor, snatch) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, snatch) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, player, CardKind.Snatch, physicalCardIds: [snatch.Id]) || ignoresDistance || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, player.Seat) == 1) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.Snatch) && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player)))
            {
                _coreNativeMultiTargetActionsOperations.AddTargetCardActions(actions, actor.Seat, LegalActionKind.Snatch, snatch, target, "顺手牵羊");
            }
        }

        {
            var jixiCandidates = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_corePhysicalOwnedCardQueriesOperations.GetAuthority(actor)).Where(IncludesPhysicalCard).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)).Where(card => _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Snatch, forResponse: false).Count != 0);
            foreach (var converted in jixiCandidates)
                foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Snatch, forResponse: false))
                {
                    var ignoresDistance = _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.Snatch) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(actor, converted);
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && (_characterGuanYuRuntimeOperations.GuanFirstRedDistance(actor, converted) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, converted) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, player, CardKind.Snatch, excludedEquipmentId: converted.Id, physicalCardIds: [converted.Id]) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Snatch, [converted.Id], conversionSource) || ignoresDistance || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, player.Seat) == 1) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.Snatch) && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(player)))
                    {
                        _coreNativeMultiTargetActionsOperations.AddTargetCardActions(actions, actor.Seat, LegalActionKind.Snatch, converted, target, "顺手牵羊", CardKind.Snatch, conversionSource);
                    }
                }
        }

        if (_coreNativeDamageHandlesOperations.Value.UsesFormalBorrowedSword)
        {
            foreach (var borrowedSword in playableCards.Where(card => card.Kind == CardKind.BorrowedSword))
            {
                foreach (var weaponOwner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.BorrowedSword) && _coreNativeBorrowedSwordOperations.GetWeapon(player)is not null))
                {
                    foreach (var slashTarget in _coreMatchParticipantsPlayersAccess._players.Where(player => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(weaponOwner, player)))
                    {
                        actions.Add(new LegalAction(LegalActionKind.BorrowedSword, borrowedSword.Id, null, $"令 {(weaponOwner.Name)} 对 {(slashTarget.Name)} 使用【杀】，否则获得其武器", TargetSeats: [weaponOwner.Seat, slashTarget.Seat]));
                    }
                }
            }
        }

        _coreUnexpectedAssaultActionsOperations.Value.AddUnexpectedAssaultActions(actor, playableCards, actions);
        foreach (var fireAttack in playableCards.Where(card => card.Kind == CardKind.FireAttack))
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Any(card => player.Seat != actor.Seat || card.Id != fireAttack.Id)))
            {
                actions.Add(new LegalAction(LegalActionKind.FireAttack, fireAttack.Id, target.Seat, $"对 {(target.Name)} 使用【火攻】"));
            }
        }

        foreach (var converted in playableCards.Where(card => card.Kind != CardKind.FireAttack))
        {
            foreach (var conversionSource in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.FireAttack, forResponse: false))
            {
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Any(card => player.Seat != actor.Seat || card.Id != converted.Id)))
                {
                    actions.Add(new LegalAction(LegalActionKind.FireAttack, converted.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(conversionSource, $"将【{(converted.DisplayName)}】当作【火攻】对 {(target.Name)} 使用"), PlayedCardKind: CardKind.FireAttack) { ConversionSource = conversionSource });
                }
            }
        }

        if (selectedPhysicalCardId is null && _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx && !_coreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count > 0 && _characterHuCheErMouOperations.KongwuEffectiveEquipment(actor).Any(card => card.Kind == CardKind.WoodenOx))
        {
            actions.Add(new LegalAction(LegalActionKind.UseEquipmentEffect, null, null, "发动【木牛流马】，将一张手牌扣置为“粮”，并可移动此装备", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 0, MaxTargetCount: 1, EquipmentKind: CardKind.WoodenOx) { SelectableCardIds = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Select(card => card.Id).Order().ToArray(), SelectableTargetSeats = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && !player.EquipmentAreaAbolished && _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).All(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) != EquipmentSlot.Treasure)).Select(player => player.Seat).Order().ToArray() });
        }

        _coreProgramActionModulesOperations.ProgramActions.Augment(ProgramCardActionPhase.BeforeProgram, actions, actor, allPhysicalCardCandidates, selectedPhysicalCardId);
        if (selectedPhysicalCardId is null)
        {
            if (includeProgramActions)
                actions.AddRange(_coreSkillProgramsOperations.BuildProgramActions(actor));
            actions.AddRange(_coreProgramActionModulesOperations.ProgramActions.Build(ProgramActionSourcePhase.Cardless, actor));
        }

        _coreProgramActionModulesOperations.ProgramActions.Augment(ProgramCardActionPhase.AfterProgram, actions, actor, allPhysicalCardCandidates, selectedPhysicalCardId);
        if (selectedPhysicalCardId is null)
            actions.Add(new LegalAction(LegalActionKind.EndPlay, null, null, "结束出牌"));
        var physicalKinds = allPhysicalPlayableCards.ToDictionary(card => card.Id, card => card.Kind);
        var physicalSuits = allPhysicalPlayableCards.ToDictionary(card => card.Id, card => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card));
        var slashTraits = new Dictionary<LegalAction, PublicSlashTraits>();
        GameSnapshot? slashTraitsView = null;
        var permittedActions = actions.Where(action => selectedPhysicalCardId is null || action.CardId == selectedPhysicalCardId).Where(action =>
        {
            var effectiveKind = action.PlayedCardKind ?? (action.CardId is { } cardId && physicalKinds.TryGetValue(cardId, out var physicalKind) ? physicalKind : (CardKind? )null);
            if (effectiveKind is null)
                return true;
            var targets = action.TargetSeats.Count > 0 ? action.TargetSeats : action.TargetSeat is { } targetSeat ? [targetSeat] : _coreDyingWindowSkillPoliciesOperations.HasBlackTrickTargetPolicy && action.Kind == LegalActionKind.DrawTwo ? [actor.Seat] : [];
            var declaredSuit = action.CardId is { } actionCardId && physicalSuits.TryGetValue(actionCardId, out var physicalSuit) ? physicalSuit : (action.Kind == LegalActionKind.IronChain || _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)is { } paidColorCard && _corePaidColorConversionsOperations.IsPaidColorEquipmentDuelAction(actor, action, paidColorCard)) && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId)is { } equipmentConversion && _cardZonesOperations._cardZones.GetLocation(equipmentConversion.Id) == CardLocation.Equipment(actor.Seat) ? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, equipmentConversion) : _characterMaChaoAnniversaryGodOperations.FindShouliFieldCard(actor, action.CardId)is { } fieldMount && _characterMaChaoAnniversaryGodOperations.IsAcceptedShouliMaterial(actor, fieldMount, action.ConversionSource, false) ? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, fieldMount) : _coreLastZoneConversionsOperations.FindLastZoneJudgmentConversionCard(actor, action.CardId ?? -1)is { } judgmentConversion ? _coreNativeCardAppearanceOperations.EffectiveSuit(actor, judgmentConversion) : (Suit? )null;
            var declaredColor = action.CardId is { } colorCardId && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, colorCardId)is { } colorCard ? CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, colorCard, action.ConversionSource))) : null;
            if (_characterWuJingOlOperations.HasWuJingLiubing && action.Kind == LegalActionKind.Slash && action.CardId is { } liubingId && _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, liubingId)is { } liubingCard)
            {
                var conversions = (action.ConversionSource is { } primary ? new[]
                {
                    primary
                }

                : Array.Empty<CardConversionSource>()).Concat(action.AdditionalConversionSources ?? []).ToArray();
                declaredSuit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actor.Seat, liubingCard, effectiveKind.Value, [liubingId], conversions, declaredSuit);
                declaredColor = CoreGameEnginePublicPileColorsRuntime.SuitColor(declaredSuit);
            }

            if (action.Kind == LegalActionKind.Slash && action.CardId is> 0)
            {
                if (declaredColor == false && _coreDirectedCardPolicyQueriesOperations.HasYuanchouCardPolicy)
                    slashTraitsView ??= _corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat);
                slashTraits[action] = _corePublicSlashTraitsOperations.CreatePublicSlashTraits(actor, action, effectiveKind.Value, declaredColor, slashTraitsView);
            }

            if (action.Kind != LegalActionKind.Recast && action.MaxCardCount <= 1 && declaredSuit is { } useSuit && _coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(actor.Seat, useSuit))
                return false;
            if (action.Kind != LegalActionKind.Recast && action.MaxCardCount <= 1 && _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, action.CardId is { } cuiCardId ? [cuiCardId] : [], action.ConversionSource is { } cuiConversion && _corePlayCardHistoryOperations.ViewAsRule(cuiConversion)?.InheritPreviousPlaySuit == true ? _corePlayCardHistoryOperations.PreviousPlaySuitForInheritance(actor.Seat) ?? declaredSuit : declaredSuit))
                return false;
            // Global tricks drop individually shielded targets instead of the whole
            // use: a black Nanman still resolves against everyone a Curtain bearer
            // does not protect, and only loses its action when no target remains.
            if (action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains)
                return _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, targets).Any(target => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target, effectiveKind.Value) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], effectiveKind.Value, declaredSuit, declaredColor));
            return _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, targets).All(target => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target, effectiveKind.Value) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], effectiveKind.Value, declaredSuit, declaredColor));
        });
        var projectedPermittedActions = permittedActions.Select(action => slashTraits.TryGetValue(action, out var traits) ? action with { SlashTraits = traits } : action);
        var result = _characterLuBuOlMagicOperations.FilterMoGangquanActions(actor, _coreDeckEquipmentProgramsOperations.FilterBeneficiarySuitShieldActions(actor, _coreCardUseModulesOperations.FilterTurnCardUseRestrictions(actor, projectedPermittedActions, nativeOriginalCard)));
        if (nativeOriginalCard is not null)
            return result;
        var originalPileActions = _coreNativePublicPileOriginalCardsOperations.NativePublicPileOriginalPlayActions(actor, selectedPhysicalCardId);
        var allActions = originalPileActions.Count == 0 ? result : result.Concat(originalPileActions).ToArray();
        var projectedActions = _characterLuBuMobileGodOperations.ProjectMobileWumouActions(actor, allActions, selectedPhysicalCardId);
        // Direct equipment placement is not a card use and does not inherit use-only prohibitions or declarations.
        var withHuxinPlacement = projectedActions.Concat(_coreHuxinMirrorOperations.HuxinMirrorPlacementActions(actor, allPhysicalCardCandidates)).ToArray();
        return ignoreForcedHandSelection ? withHuxinPlacement : _programPipelineHooksOperations._programPipelineHooks.FilterPlayPhaseActions(actor, withHuxinPlacement);
    }
}
