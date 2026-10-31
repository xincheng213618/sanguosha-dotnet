#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardConversionsRuntime
{
    internal const string SpGuanYuWushengSkillId = "sp:guan-yu-wusheng";
    internal const string NuzhanSkillId = "sp:nuzhan";
    internal bool IgnoresSpGuanYuWushengDistance(CharacterState player, Card card) => _coreNationalSkillsOperations.HasRuntimeSkill(player, SpGuanYuWushengSkillId) && card.Suit == Suit.Diamond;
    internal GameEngine.NuzhanModifiers GetNuzhanModifiers(long frameId, CharacterState source)
    {
        if (!_coreNationalSkillsOperations.HasRuntimeSkill(source, NuzhanSkillId))
            return default;
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == frameId).Action;
        if (action is null || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.PhysicalCards.Count != 1 || !action.ConversionChain.Any(conversion => conversion.OwnerSeat == source.Seat))
        {
            return default;
        }

        var physical = action.PhysicalCards[0];
        var modifiers = new GameEngine.NuzhanModifiers(IgnoresSlashLimit: CardCatalog.Get(physical.CardKind).CategoryName == "锦囊牌", DamageBonus: _coreEquipmentContentOperations.EquipmentRules.IsEquipment(physical.CardKind) ? 1 : 0, PhysicalCardId: physical.CardId);
        if (!modifiers.IgnoresSlashLimit && modifiers.DamageBonus == 0)
            return default;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NuzhanAppliedEvent(frameId, source.Seat, modifiers.PhysicalCardId, modifiers.IgnoresSlashLimit, modifiers.DamageBonus));
        _coreMatchLogOperations.AddLog("SkillTriggered", modifiers.IgnoresSlashLimit ? $"{(source.Name)} 的【怒斩】令此【杀】不计入出牌阶段次数。" : $"{(source.Name)} 的【怒斩】令此【杀】的伤害值+1。", source.Seat);
        return modifiers;
    }

    internal void AddNuzhanUnlimitedTrickSlashActions(ICollection<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playableCards)
    {
        if (!_coreNationalSkillsOperations.HasRuntimeSkill(actor, NuzhanSkillId))
            return;
        foreach (var converted in playableCards.Where(card => CardCatalog.Get(card.Kind).CategoryName == "锦囊牌"))
        {
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, converted, CardKind.Slash, forResponse: false))
            {
                var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, converted, source, ignoresSlashLimit: true);
                foreach (var target in targets)
                {
                    actions.Add(new LegalAction(LegalActionKind.Slash, converted.Id, target.Seat, DescribeConversion(source, $"将【{(converted.DisplayName)}】当作【杀】对 {(target.Name)} 使用（【怒斩】不计次数）"), PlayedCardKind: CardKind.Slash) { ConversionSource = source });
                }

                _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, converted, targets, "杀", CardKind.Slash, source);
                _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, converted, targets, "杀", CardKind.Slash, source);
            }
        }
    }

    internal string DescribeConversion(CardConversionSource? source, string description)
    {
        if (source is null)
            return description;
        if (_corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null)
            description = System.Text.RegularExpressions.Regex.Replace(description, "将【[^】]*】(?:当作|当|改为)【[^】]*】", "声明【" + CardCatalog.Get(_corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind).DisplayName + "】");
        return $"【{(_contentRegistryOperations._contentRegistry!.Skills[source.SkillId].Name)}】{(description)}";
    }

    internal IReadOnlyList<GameEngine.ProgramCardIdentityMatch> FindProgramCardIdentityMatches(CharacterState owner, Card card)
    {
        var identityLocation = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var identityHand = identityLocation == CardLocation.Hand(owner.Seat);
        var identityGrain = identityLocation == CardLocation.WoodenOxGrain(owner.Seat);
        var identityHu = _characterGongsunZanMouMobileOperations.IsMouGszHuCard(owner, card.Id);
        if (!identityHand && !identityGrain && !identityHu && !_coreAlcoholIdentityPoliciesOperations.IsAlcoholIdentityOwnedLocation(owner, card))
            return[];
        if (identityHand && _characterPangTongAnniversaryGodOperations.ShenPangTongLuansuoIdentity(owner, card)is { } luansuo)
            return[luansuo];
        if (identityHand && _characterGuanYuWumiaoOperations.WuyouIdentity(owner, card)is { } wuyou)
            return[wuyou];
        if (identityHand && _coreGiftedSlashUseRewardsOperations.GetGiftedSlashIdentityMatch(owner, card)is { } gifted)
            return[gifted];
        if (identityHand && _characterWangGuanOlOperations.ShiluIdentity(owner, card)is { } shilu)
            return[shilu];
        if (identityHand && _characterGuanYuRuntimeOperations.WeilinIdentity(owner, card)is { } weilin)
            return[weilin];
        if (identityHand && _coreWushuangOriginalSkillRulesOperations.WushuangChoufaIdentity(owner, card)is { } choufa)
            return[choufa];
        if (identityHand && _coreWushuangSkillsBOperations.WushuangBianyuIdentity(owner, card)is { } bianyu)
            return[bianyu];
        var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(owner);
        if (bindings.CardIdentityPrograms.Count == 0)
            return[];
        var inputKind = _coreWenhePoisonOperations.EffectiveConversionInputKind(owner, card);
        var context = _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
        return bindings.ProgramInstances.Where(instance => instance.Program.CardIdentities.Count != 0 && instance.SkillId != "anniversary:luansuo").SelectMany(instance => instance.Program.CardIdentities.Where(identity => ((identityHand || identityGrain) && identity.Zones.Contains(identityLocation.Zone) || identityHu && identity.Zones.Contains(CardZoneKind.Hand) || instance.Program.CardPolicies.Any(p => p.Kind == SkillProgramCardPolicyKind.AlcoholKingIdentityRank) && identity.InputKinds.SequenceEqual([CardKind.Alcohol]) && identity.OutputKind == CardKind.Slash && identity.Zones.Contains(CardZoneKind.Hand)) && identity.Condition.Evaluate(context) && (identity.InputKinds.Count == 0 || ProgramCardKindMatch.Contains(identity.InputKinds, inputKind)) && (identity.InputSuits.Count == 0 || identity.InputSuits.Contains(card.Suit))).Select(identity => new GameEngine.ProgramCardIdentityMatch(instance.Program, identity, new CardConversionSource(instance.SkillId, identity.Id, owner.Seat, instance.SkillInstanceId)))).OrderBy(match => match.Source.SkillId, StringComparer.Ordinal).ThenBy(match => match.Source.BindingId, StringComparer.Ordinal).ThenBy(match => match.Source.SkillInstanceId, StringComparer.Ordinal).ToArray();
    }

    internal IReadOnlyList<CardConversionSource> GetProgramCardIdentitySources(CharacterState owner, Card card, CardKind effectiveKind, bool forResponse)
    {
        if (effectiveKind == CardKind.Alcohol && _characterHouChengAnniversaryOperations.IsAnniversaryHouChengWineCard(card.Id))
            return _characterHouChengAnniversaryOperations.AnniversaryHouChengWineSources(owner, card, effectiveKind, forResponse, dyingUse: forResponse && _coreDyingCursorRuntimeOperations.ActiveDying?.VictimSeat == owner.Seat);
        var matches = _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card);
        if (matches.Count == 0)
            return[];
        return matches.Where(match => match.Identity.OutputKind == effectiveKind || forResponse && CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, effectiveKind) || !forResponse && match.Identity.OutputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind)).Select(match => match.Source).ToArray();
    }

    internal bool HasProgramCardIdentity(CharacterState owner, Card card) => _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card).Count != 0;
    internal bool IgnoresProgramSlashDistance(CharacterState owner, CardConversionSource? source)
    {
        if (source is null || source.OwnerSeat != owner.Seat)
            return false;
        if (_coreProgramDistanceRulesOperations.DistanceRules.HasConvertedSlashExemption(new(owner, source)))
            return true;
        var context = _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetNumericModifiers(SkillRuleQuery.SlashDistanceLimit).Any(binding => binding.Source.SkillId == source.SkillId && binding.Source.SkillInstanceId == source.SkillInstanceId && binding.Modifier is { } modifier && modifier.Query == SkillRuleQuery.SlashDistanceLimit && modifier.Operation == SkillRuleOperation.Unlimited && modifier.SourceCardIdentityId == source.BindingId && modifier.Condition.Evaluate(context));
    }

    internal bool IgnoresProgramSlashLimit(CharacterState owner, CardConversionSource? source) => _characterMaChaoAnniversaryGodOperations.ShouliIgnoresSlashLimit(owner, source) || _characterMiHengLeOperations.SiruiIgnoresSlashLimit(owner, source) || _characterSunQuanAnniversaryGodOperations.IsAnniversaryChouxiSource(owner, source) || source is not null && source.OwnerSeat == owner.Seat && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetNumericModifiers(SkillRuleQuery.SlashLimit).Any(binding => binding.Source.SkillId == source.SkillId && binding.Source.SkillInstanceId == source.SkillInstanceId && binding.Modifier.Operation == SkillRuleOperation.Unlimited && binding.Modifier.SourceCardIdentityId == source.BindingId && binding.Modifier.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)));
    internal bool HasUnlimitedStaticSlashDistance(CharacterState owner) => _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetNumericModifiers(SkillRuleQuery.SlashDistanceLimit).Any(binding => binding.Modifier is { } modifier && modifier.SourceCardIdentityId is null && modifier.Query == SkillRuleQuery.SlashDistanceLimit && modifier.Operation == SkillRuleOperation.Unlimited && modifier.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)));
    internal bool IgnoresSlashUseDistance(CharacterState actor) => _coreCardUseModulesOperations.HasUnlimitedTurnRuleModifier(actor.Seat, SkillRuleQuery.SlashDistanceLimit) || HasUnlimitedStaticSlashDistance(actor);
    internal IReadOnlyList<CardConversionSource> FindProgramViewAsConversions(CharacterState owner, Card card, CardKind outputKind, bool forResponse, bool dyingUse = false, int? requestedUseTarget = null)
    {
        if (requestedUseTarget is { } requested && (forResponse || dyingUse || outputKind != CardKind.Peach || !_coreWenyiPeachSuppliesOperations.IsWenyiPeachSupplyQuery(owner, requested)))
            return[];
        if (_coreCardDeclarationsOperations.UnclaimedDeclarationPayment(owner.Seat, card.Id)is { } paid && (paid.DeclaredKind == outputKind || forResponse && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(paid.DeclaredKind)))
            return[paid.Source];
        var houChengWine = _characterHouChengAnniversaryOperations.AnniversaryHouChengWineSources(owner, card, outputKind, forResponse, dyingUse);
        if (houChengWine.Count != 0)
            return houChengWine;
        var fieldMounts = _characterMaChaoAnniversaryGodOperations.ShouliFieldConversions(owner, card, outputKind, forResponse, dyingUse);
        if (fieldMounts.Count != 0)
            return fieldMounts;
        var registered = _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetSingleCardConversions(outputKind);
        var hasTurnConversions = _turnCardUseEffectsOperations._turnCardUseEffects.Conversions.Count > 0;
        if (registered.Count == 0 && !hasTurnConversions)
            return[];
        if (_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(owner.Seat, card.Id))
            return[];
        var wuyouJuewuIdentity = _characterGuanYuWumiaoOperations.HasWuGuanYuJuewuCapability && _characterGuanYuWumiaoOperations.WuyouIdentity(owner, card)is not null;
        if (HasProgramCardIdentity(owner, card) && !wuyouJuewuIdentity)
        {
            return[];
        }

        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (location == CardLocation.WoodenOxGrain(owner.Seat) && _characterHuCheErMouOperations.MouHuCheErEquipmentDisabled(owner.Seat))
            return[];
        var zone = location == CardLocation.Hand(owner.Seat) || _characterGongsunZanMouMobileOperations.IsMouGszHuCard(owner, card.Id) || _characterLuSuMobileShiOperations.IsShiLsHandAccess2(owner.Seat, card.Id) ? CardZoneKind.Hand : location == CardLocation.Equipment(owner.Seat) ? CardZoneKind.Equipment : location == CardLocation.Authority(owner.Seat) ? CardZoneKind.Authority : location == CardLocation.WoodenOxGrain(owner.Seat) ? CardZoneKind.WoodenOxGrain : location == CardLocation.Judgment(owner.Seat) ? CardZoneKind.Judgment : location.Zone == CardZoneKind.PublicPersistentPile && location.OwnerSeat == owner.Seat ? CardZoneKind.PublicPersistentPile : (CardZoneKind? )null;
        if (zone is null || _coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(owner.Seat, outputKind, [card.Id], forResponse && !dyingUse))
            return[];
        var inputKind = _coreWenhePoisonOperations.EffectiveConversionInputKind(owner, card);
        PlayerSkillContext? context = null;
        var configured = registered.Count == 0 ? Array.Empty<CardConversionSource>() : registered.Where(item =>
        {
            var(instance, rule) = item;
            return (!wuyouJuewuIdentity || instance.SkillId == "wumiao:juewu" && rule.RankTwoDamageUse) && instance.SkillId != "wushuang:skill-3252" && rule.KenshangUse != true && !(rule.VariableInputCount && rule.CostDestination == SkillProgramCardDestination.DrawPileBottom) && CharacterZhaoYunOlGod2016Runtime.EffectiveProgramViewAsInputCount(owner, rule) == 1 && (rule.OutputKind == outputKind || _coreProgramCardLegalityOperations.CardLegality.AllowsNativeSlashAlias(new(owner, instance, rule, outputKind, forResponse)) || instance.SkillId == "le:fule" && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _characterCaoZhiLeOperations.FuleSlashAliasAllowed(owner, forResponse) || rule.EqualNameLengthUse && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _characterMiHengLeOperations.SiruiSlashAliasAllowed(owner, forResponse) || rule.RankTwoDamageUse && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _characterGuanYuWumiaoOperations.JuewuSlashAliasAllowed(owner, forResponse) || forResponse && rule.ExtendedUse && outputKind == CardKind.Slash && rule.OutputKind == CardKind.FireSlash || forResponse && rule.DeclarationValidation is not null && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind)) && rule.SourceZones.Contains(zone.Value) && _characterHuJinDingOlOperations.MatchesZhongshenRoundGain(owner, card, rule) && (zone != CardZoneKind.Judgment || rule.LastInSourceZone == true) && (requestedUseTarget is not null ? rule.ForResponse || rule.ForPlay : dyingUse ? rule.ForResponse || rule.UseOnly && rule.ForPlay : forResponse ? rule.ForResponse && (!rule.UseOnly || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, outputKind) || rule.RankTwoDamageUse && _characterGuanYuWumiaoOperations.IsJuewuTrueSlashUse(owner) || rule.EqualNameLengthUse && _characterMiHengLeOperations.IsSiruiTrueResponseUse(owner, rule.OutputKind, dyingUse, requestedUseTarget) || (rule.ConversionStateId is not null || rule.UnusedOutputNameThisGame || rule.LastInSourceZone == true) && outputKind == CardKind.Nullification) : rule.ForPlay) && (rule.DeclarationValidation is null || _coreCardDeclarationsOperations.CanDeclareCard(owner, instance.SkillId)) && !_coreProgramCardLegalityOperations.CardLegality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.NamedConversionBan, new(owner, card, instance, rule, forResponse, dyingUse, requestedUseTarget)) && (rule.TieredRoundConversion is null || !card.IsGeneralWeapon) && !_coreProgramCardLegalityOperations.CardLegality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.ConfiguredConversionBan, new(owner, card, instance, rule, forResponse, dyingUse, requestedUseTarget)) && (!rule.UnusedOutputThisTurn || !_coreAlternativeViewAsCostsOperations.HasProgramUsedBasicCardThisTurn(owner.Seat, outputKind)) && (inputKind != outputKind || rule.InheritPreviousPlaySuit || rule.ConversionStateId is not null || rule.AllowSameKind) && !_coreProgramCardLegalityOperations.CardLegality.IsConfiguredConversionForbidden(ProgramCardLegalityStage.ConversionUsageBan, new(owner, card, instance, rule, forResponse, dyingUse, requestedUseTarget)) && (rule.Condition.Kind == SkillProgramConditionKind.Always || rule.Condition.Evaluate(context ??= _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner))) && (rule.InputKinds.Count == 0 || ProgramCardKindMatch.Contains(rule.InputKinds, inputKind)) && (rule.InputCategories.Count == 0 || rule.InputCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(inputKind, category))) && (rule.InputSuits.Count == 0 || rule.InputSuits.Contains(rule.UseEffectiveInputSuit == true ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit));
        }).Select(item => new CardConversionSource(item.Source.SkillId, item.Rule.Id, owner.Seat, item.Source.SkillInstanceId)).Where(source => !_coreProgramCardLegalityOperations.CardLegality.IsConversionSourceForbidden(ProgramCardLegalityStage.DyingConversionBan, new(owner, card, source, forResponse, dyingUse))).Where(source => !_coreProgramCardLegalityOperations.CardLegality.IsConversionSourceForbidden(ProgramCardLegalityStage.ConversionSourceBan, new(owner, card, source, forResponse, dyingUse))).ToArray();
        var turnScoped = wuyouJuewuIdentity || !hasTurnConversions || forResponse || zone != CardZoneKind.Hand ? Array.Empty<CardConversionSource>() : _turnCardUseEffectsOperations._turnCardUseEffects.GetConversions(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, owner.Seat, outputKind, CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card))).Select(item => new CardConversionSource(item.Source.SkillId, $"{(item.Source.BindingId)}.turn-{(item.EffectIndex)}", item.Source.OwnerSeat, item.Source.SkillInstanceId)).ToArray();
        var paidColor = hasTurnConversions && !wuyouJuewuIdentity ? _corePaidColorConversionsOperations.GetPaidColorTurnDuelConversions(owner, card, outputKind, forResponse, zone.Value) : Array.Empty<CardConversionSource>();
        if (configured.Length == 0 && turnScoped.Length == 0 && paidColor.Count == 0)
            return[];
        return configured.Concat(turnScoped).Concat(paidColor).Where(source => _coreProgramCardLegalityOperations.IsProgramViewAsAllowed(owner, outputKind, [card.Id], source, forResponse, dyingUse, requestedUseTarget)).Distinct().OrderBy(source => source.SkillId, StringComparer.Ordinal).ThenBy(source => source.BindingId, StringComparer.Ordinal).ThenBy(source => source.SkillInstanceId, StringComparer.Ordinal).ToArray();
    }

    internal bool IsDirectProgramFireSlashConversion(CharacterState owner, Card card, CardConversionSource? source) => source is not null && (_coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.FireSlash, forResponse: false).Contains(source) || GetProgramCardIdentitySources(owner, card, CardKind.FireSlash, forResponse: false).Contains(source));
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(CharacterState owner, CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = null)
    {
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(owner.Seat, outputKind))
            return[];
        if (!owner.IsAlive || requestedUseTarget is { } requested && (forResponse || outputKind != CardKind.Peach || !_coreWenyiPeachSuppliesOperations.IsWenyiPeachSupplyQuery(owner, requested)))
            return[];
        var bindings = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances;
        var leiluan = _characterKongShuOlOperations.LeiluanSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition);
        var jinzhi = _coreWushuangJinzhiOperations.WushuangJinzhiSelections(owner, outputKind, forResponse);
        if (leiluan.Count == 0 && jinzhi.Count == 0 && !bindings.Any(instance => instance.Program.ViewAs.Any(rule => CharacterZhaoYunOlGod2016Runtime.EffectiveProgramViewAsInputCount(owner, rule) > 1 && !rule.VariableInputCount)))
            return[];
        var context = _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
        var dyingUse = forResponse && outputKind is CardKind.Peach or CardKind.Alcohol && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == owner.Seat;
        var eligibleHand = _characterLuSuMobileShiOperations.AppendShiLsPlayableCards(owner, _corePhysicalOwnedCardQueriesOperations.GetHand(owner)).Concat(_characterGongsunZanMouMobileOperations.MouGszHuCards(owner)).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Concat(_characterHuCheErMouOperations.KongwuUsableWoodenOxGrain(owner)).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) && !HasProgramCardIdentity(owner, card)).OrderBy(card => card.Id).ToArray();
        var selections = new List<ProgramMultiCardViewAsSelection>();
        foreach (var instance in bindings.Where(instance => instance.Program.ViewAs.Count != 0))
            foreach (var rule in instance.Program.ViewAs.Where(rule => CharacterZhaoYunOlGod2016Runtime.EffectiveProgramViewAsInputCount(owner, rule) > 1 && !rule.VariableInputCount && (rule.OutputKind == outputKind || forResponse && rule.ExtendedUse && outputKind == CardKind.Slash && rule.OutputKind == CardKind.FireSlash || !forResponse && rule.RoundDistinctBasicUse is not null && outputKind == CardKind.Slash && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind)) && (requestedUseTarget is not null ? rule.ForResponse || rule.ForPlay : forResponse ? rule.ForResponse || rule.RoundDistinctBasicUse is not null && dyingUse && rule.ForPlay : rule.ForPlay) && (rule.RoundDistinctBasicUse is not null || !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, outputKind, forResponse ? CardActionType.Response : CardActionType.Use)) && (rule.XieweiBaitBasicUse is null || _characterGuanHaiOlOperations.XieweiNativeDirection(owner, outputKind, forResponse)) && _coreRoundDistinctBasicUseOperations.CanUseRoundDistinctBasicConversion(owner, instance, rule, forResponse, dyingUse) && rule.Condition.Evaluate(context)))
            {
                var candidates = eligibleHand.Where(card => (rule.SourceZones.Contains(_cardZonesOperations._cardZones.GetLocation(card.Id).Zone) || _characterGongsunZanMouMobileOperations.MouGszHuHandSource(owner, card, rule.SourceZones) || rule.RoundDistinctBasicUse is not null && _coreRoundDistinctBasicUseOperations.IsRoundDistinctBasicMaterial(owner, card)) && (rule.InputKinds.Count == 0 || ProgramCardKindMatch.Contains(rule.InputKinds, _coreWenhePoisonOperations.EffectiveConversionInputKind(owner, card))) && (rule.InputSuits.Count == 0 || rule.InputSuits.Contains(rule.UseEffectiveInputSuit == true ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit))).ToArray();
                var inputCount = CharacterZhaoYunOlGod2016Runtime.EffectiveProgramViewAsInputCount(owner, rule);
                if (candidates.Length < inputCount)
                    continue;
                var source = new CardConversionSource(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId);
                foreach (var cards in EnumerateCardCombinations(candidates, inputCount))
                    if ((!rule.SameSuit || cards.Select(card => rule.UseEffectiveInputSuit == true ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit).Distinct().Count() == 1) && _coreRoundDistinctBasicUseOperations.MatchesRoundDistinctBasicMaterials(owner, rule, cards))
                        selections.Add(new(cards, source, rule.RoundDistinctBasicUse is null ? outputKind : rule.OutputKind));
            }

        return selections.Concat(leiluan).Concat(jinzhi).Where(item => _coreProgramCardLegalityOperations.IsProgramViewAsAllowed(owner, outputKind, item.Cards.Select(c => c.Id).ToArray(), item.Source, forResponse, dyingUse, requestedUseTarget)).Where(item => _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(item) || !forResponse || _coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(owner, item.Cards, outputKind)).Where(item => _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(item) || !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(owner.Seat, outputKind, item.Cards.Select(c => c.Id), forResponse && !dyingUse)).Where(item => _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(item) || !forResponse || !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(owner, outputKind, item.Cards)).Where(item => _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(item) || forResponse || ignoreSuitUseProhibition || !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, item.Cards.Select(c => c.Id).ToArray())).OrderBy(item => item.Source.SkillId, StringComparer.Ordinal).ThenBy(item => item.Source.BindingId, StringComparer.Ordinal).ThenBy(item => string.Join(',', item.Cards.Select(card => card.Id)), StringComparer.Ordinal).ToArray();
    }

    internal static IReadOnlyList<IReadOnlyList<Card>> EnumerateCardCombinations(IReadOnlyList<Card> cards, int count) => RuntimeSkills.RuntimeRules_Core_CardConversions.EnumerateCardCombinations(cards, count);
    internal ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(CharacterState owner, IReadOnlyList<int> cardIds, CardKind outputKind, bool forResponse, CardConversionSource source)
    {
        if (cardIds.Count == 0 || cardIds.Distinct().Count() != cardIds.Count)
            return null;
        var ordered = cardIds.Order().ToArray();
        var variable = FindProgramVariableMultiCardViewAsSelection(owner, ordered, outputKind, forResponse, source);
        if (variable is not null)
            return variable;
        return GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse).FirstOrDefault(candidate => candidate.Source == source && candidate.Cards.Select(card => card.Id).Order().SequenceEqual(ordered));
    }

    internal ProgramMultiCardViewAsSelection? FindProgramVariableMultiCardViewAsSelection(CharacterState owner, int[] orderedCardIds, CardKind outputKind, bool forResponse, CardConversionSource source)
    {
        if (!owner.IsAlive || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, outputKind, forResponse ? CardActionType.Response : CardActionType.Use))
            return null;
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.FirstOrDefault(item => item.SkillId == source.SkillId && item.SkillInstanceId == source.SkillInstanceId);
        if (instance is null)
            return null;
        var rule = instance.Program.ViewAs.FirstOrDefault(item => item.Id == source.BindingId && item.VariableInputCount && item.OutputKind == outputKind && (forResponse ? item.ForResponse : item.ForPlay));
        if (rule is null || rule.KenshangUse == true && orderedCardIds.Length < 2 || !rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)))
            return null;
        var cards = new List<Card>();
        foreach (var cardId in orderedCardIds)
        {
            var location = _cardZonesOperations._cardZones.GetLocation(cardId);
            if (location.OwnerSeat != owner.Seat && !_characterLuSuMobileShiOperations.IsShiLsHandAccess2(owner.Seat, cardId))
                return null;
            var card = _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(item => item.Id == cardId);
            if (card is null || !(rule.SourceZones.Contains(location.Zone) || _characterGongsunZanMouMobileOperations.MouGszHuHandSource(owner, card, rule.SourceZones) || _characterChengJiChengCuiOlOperations.IsTousuiVariableSource(owner, rule, card, location)) || _coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) || HasProgramCardIdentity(owner, card) || rule.InputKinds.Count != 0 && !ProgramCardKindMatch.Contains(rule.InputKinds, _coreWenhePoisonOperations.EffectiveConversionInputKind(owner, card)) || rule.InputSuits.Count != 0 && !rule.InputSuits.Contains(rule.UseEffectiveInputSuit == true ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit))
                return null;
            cards.Add(card);
        }

        if (forResponse ? _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbiddenByExistingPolicies(owner.Seat, orderedCardIds) || _characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(owner, outputKind, cards) : _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, orderedCardIds))
            return null;
        if (_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(owner.Seat, outputKind, orderedCardIds, forResponse) || !_coreProgramCardLegalityOperations.IsProgramViewAsAllowed(owner, outputKind, orderedCardIds, source, forResponse, forResponse && outputKind is CardKind.Peach or CardKind.Alcohol && _coreDyingCursorRuntimeOperations.ActiveDying?.ResponderSeat == owner.Seat))
            return null;
        if (rule.AllHandCardsOfSelectedSuit && !_coreFriendTrioOperations.ValidWholeHandSuitConversion(owner, cards, source))
            return null;
        return new(cards, source, outputKind);
    }

    internal string ProgramConversionName(CardConversionSource source) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Name ?? source.SkillId;
    internal void ResolveProgramMultiCardSlash(CharacterState source, CharacterState target, ProgramMultiCardViewAsSelection selection, NativeOwners.BorrowedSwordHandle? borrowedSword = null, bool enforceOwnTurnSlashLimit = true)
    {
        var effectiveKind = _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)is { RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null } ? selection.OutputKind : CardKind.Slash;
        var current = FindProgramMultiCardViewAsSelection(source, selection.Cards.Select(card => card.Id).ToArray(), effectiveKind, forResponse: false, selection.Source);
        if (current is null || !(borrowedSword is null ? _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit: _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(source, current), effectiveColor: _coreGuanHaiBridgesOperations.ProgramMultiCardColor(source, current), physicalCardIds: current.Cards.Select(c => c.Id).ToArray()) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(source, target, effectiveKind, physicalSuit: _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(source, current), allowAnyPhysicalSuit: false, effectiveColor: _coreGuanHaiBridgesOperations.ProgramMultiCardColor(source, current), physicalCardIds: current.Cards.Select(c => c.Id).ToArray())))
            throw new InvalidOperationException("The configured multi-card Slash is no longer legal.");
        _coreNativeSlashUseOperations.ResolveSlashCore(source, target, _characterGuanHaiOlOperations.IsXieweiSelection(current) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(current) ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(source, current.Cards[0], current.Source) : current.Cards[0], effectiveKind, source.Seat, borrowedSword: borrowedSword, physicalCards: current.Cards, countsTowardSlashLimit: enforceOwnTurnSlashLimit, conversionSource: current.Source);
    }

    internal CardActionContext MoveProgramMultiCardResponse(CharacterState responder, ProgramMultiCardViewAsSelection selection, long resolutionId, int responseTargetSeat, int? actorSeat = null)
    {
        var current = FindProgramMultiCardViewAsSelection(responder, selection.Cards.Select(card => card.Id).ToArray(), selection.OutputKind, forResponse: true, selection.Source) ?? throw new InvalidOperationException("The configured multi-card response is no longer legal.");
        var costs = current.Cards.Select(card => new CardActionCost(card.Id, card.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, card))).ToArray();
        _characterZhangFeiWumiaoOperations.PrepareWuZhangFeiResponseRanks(responder, selection.OutputKind, current.Cards, current.Source, actorSeat);
        _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(responder, selection.OutputKind, current.Cards);
        _characterGongsunZanMouMobileOperations.PrepareMouGszHuResponse(responder, current.Cards, selection.OutputKind, current.Source, actorSeat);
        _characterKongShuOlOperations.PrepareLeiluanResponsePayment(responder, current, resolutionId);
        _characterGuanHaiOlOperations.PrepareXieweiResponsePayment(responder, current, resolutionId, actorSeat);
        var completedResponseUseSuit = _characterMaChaoAnniversaryGodOperations.HasHengwuCapability || actorSeat is null || actorSeat == responder.Seat ? _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(responder, current.Cards, selection.OutputKind) : null;
        var xieweiPayment = _characterGuanHaiOlOperations.IsXieweiSelection(current);
        if (xieweiPayment && !_characterGuanHaiOlOperations.TryPayXieweiMaterials(current.Cards, responder.Seat, null, CardMoveReasons.Respond))
            throw new InvalidOperationException("The configured Xiewei response has no exact issued public-pile payment.");
        var leiluanAtomicPayment = _corePlayCardHistoryOperations.ViewAsRule(current.Source)?.LeiluanBasicUse is not null || _characterXunXuAnniversaryOperations.IsAnniversaryXunXuBoqiaSource(current.Source);
        if (leiluanAtomicPayment && !xieweiPayment)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(current.Cards.Select(c => c.Id).ToArray(), CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
        foreach (var card in current.Cards)
        {
            if (!leiluanAtomicPayment && !xieweiPayment)
                _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(card.Id, responder.Seat, responseTargetSeat, selection.OutputKind));
        }

        var actionActorSeat = actorSeat ?? responder.Seat;
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id == resolutionId);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, parent?.Action?.ActionId, CardActionType.Response, actionActorSeat, responder.Seat, actionActorSeat == responder.Seat ? null : actionActorSeat, responder.Seat, responseTargetSeat, selection.OutputKind, [], costs, [selection.Source], effectiveSuit: xieweiPayment ? Suit.None : completedResponseUseSuit, effectiveRank: xieweiPayment ? 0 : null, effectiveIsRed: xieweiPayment ? null : _coreActionDiscardColorsOperations.CaptureActionColor(costs, completedResponseUseSuit)));
        _characterZhangFeiWumiaoOperations.BindWuZhangFeiResponseRanks(action);
        if ((action.ActorSeat == action.ProviderSeat || _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action)) && action.RequesterSeat is null && (selection.OutputKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(responder, selection.OutputKind)))
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _characterKongShuOlOperations.BindLeiluanResponsePayment(resolutionId, action);
        _characterGuanHaiOlOperations.BindXieweiResponsePayment(resolutionId, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        _coreOffTurnUsedCardsOperations.CaptureOffTurnResponseUseOrigin(resolutionId, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(resolutionId, selection.Source.SkillId, selection.Source.BindingId, responder.Seat, Array.AsReadOnly(current.Cards.Select(card => card.Id).ToArray()), selection.OutputKind, IsUse: false, [responseTargetSeat]));
        return action;
    }

    internal void FinishProgramMultiCardResponse(ProgramMultiCardViewAsSelection selection)
    {
        if (_characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuResponseMaterials(selection))
            return;
        foreach (var card in selection.Cards)
            if (!_characterGuanHaiOlOperations.IsXieweiResponseRemovedMaterial(selection, card.Id) && !(_coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) && _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Processing))
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
    }

    internal void ResolveDuelProgramMultiCardResponse(NativeOwners.DuelHandle duel, CharacterState responder, ProgramMultiCardViewAsSelection selection)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveDuel, duel) || responder.Seat != duel.ResponderSeat)
            throw new InvalidOperationException("The configured Duel response is not current.");
        var responseAction = MoveProgramMultiCardResponse(responder, selection, duel.ResolutionId, duel.OpponentSeat);
        _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} 发动【{(ProgramConversionName(selection.Source))}】，将 {(selection.Cards.Count)} 张手牌当【杀】应战【决斗】。", responder.Seat, duel.OpponentSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel.ResolutionId, responder.Seat, UsedSlash: true, SlashCardId: selection.Cards[0].Id, ResponseCardKind: CardKind.Slash));
        if (_coreResponseEntityExchangeOperations.HasResponseEntityExchangeObservers() && _coreCardActionsOperations.TryBeginProgramCardWindow(duel.Attack, responseAction, SkillProgramTriggerWindow.CardResponseAccepted, [duel.OpponentSeat], ProgramCardContinuation.DuelSlash))
            return;
        if (_coreResponseCompletionOperations.HasCardResponseCompletedObserver(responseAction, ProgramCardContinuation.DuelSlash))
        {
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(duel.Attack, responseAction, ProgramCardContinuation.DuelSlash);
            return;
        }

        FinishProgramMultiCardResponse(selection);
        _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, responder.Seat);
    }

    internal void ResolveGroupProgramMultiCardResponse(NativeOwners.GroupCardHandle group, CharacterState responder, ProgramMultiCardViewAsSelection selection)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack || group.RequiredCardKind != selection.OutputKind || group.CurrentAttack is not { } attack || responder.Seat != attack.TargetSeat)
            throw new InvalidOperationException("The configured group response is not current.");
        var responseAction = MoveProgramMultiCardResponse(responder, selection, group.ResolutionId, group.SourceSeat);
        _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} 发动【{(ProgramConversionName(selection.Source))}】，将 {(selection.Cards.Count)} 张手牌当【杀】响应【{(group.Card.DisplayName)}】。", responder.Seat, group.SourceSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Slash, responder.Seat, UsedResponse: true, ResponseCardId: selection.Cards[0].Id, ResponseCardKind: CardKind.Slash));
        if (_coreResponseEntityExchangeOperations.HasResponseEntityExchangeObservers() && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, responseAction, SkillProgramTriggerWindow.CardResponseAccepted, [group.SourceSeat], ProgramCardContinuation.GroupResponse))
            return;
        if (_coreResponseCompletionOperations.HasCardResponseCompletedObserver(responseAction, ProgramCardContinuation.GroupResponse))
        {
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, responseAction, ProgramCardContinuation.GroupResponse);
            return;
        }

        FinishProgramMultiCardResponse(selection);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal IReadOnlyList<Card> FindSlashUseCards(CharacterState owner, bool ignoreSuitUseProhibition = false)
    {
        if (CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.All(kind => _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, kind, CardActionType.Use)))
            return[];
        var cards = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(owner).Where(card =>
        {
            if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card))
                return false;
            var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card);
            return identities.Count != 0 ? identities.Any(match => CoreNativeCardClassificationsRuntime.IsSlashCard(match.Identity.OutputKind)) : CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Slash, forResponse: false).Count != 0;
        });
        cards = cards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(owner)).Where(card => _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Slash, forResponse: false).Count != 0));
        cards = cards.Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(owner, forResponse: false));
        cards = cards.Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(owner));
        return cards.Where(card => !_coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(owner.Seat, card.Id) && !_characterZhugeLiangWumiaoOperations.IsWuZglCopyBlocked(owner.Seat, card.Id)).Where(card => ignoreSuitUseProhibition || !_coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(owner.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, _characterWuJingOlOperations.WuJingLiubingUseCard(owner, card, card.Kind)))).DistinctBy(card => card.Id).ToArray();
    }

    internal static bool IsFactionSlashUse(NativeOwners.FactionCardRequestHandle pending) => pending.IsProgramSkillUse || pending.IsBorrowedSwordUse || pending.IsQinglongCrescentBladeUse;
    internal IReadOnlyList<Card> GetFactionSlashSlashCards(NativeOwners.FactionCardRequestHandle pending, CharacterState provider)
    {
        var cards = (pending.IsAssistedProgramUse ? _coreAssistedFactionSlashOperations.GetAssistedFactionSlashCards(pending, provider).Select(variant => variant.Card).DistinctBy(card => card.Id).ToArray() : IsFactionSlashUse(pending) ? _coreActionQueriesOperations.GetSlashUseCards(provider, ignoreSuitUseProhibition: true) : _coreNativeResponseMaterialQueriesOperations.GetResponseCards(provider, CardKind.Slash)).Where(card => (!IsFactionSlashUse(pending) || !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(pending.OwnerSeat, [card.Id])) && _coreTurnRedSlashBenefitsOperations.IsRedSlashProviderPaymentLegal(pending, GetFactionSlashEffectiveSlashKind(pending, provider, card), [card])).ToArray();
        if (!_characterCuiYanOlOperations.HasCuiYanCapability || !IsFactionSlashUse(pending) || pending.IsAssistedProgramUse)
            return cards;
        return cards.Where(card => _coreCuiYanNativeQueriesOperations.HasCuiYanAllowedFactionSlashVariant(pending, provider, card)).ToArray();
    }

    internal CardKind GetFactionSlashEffectiveSlashKind(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, Card card)
    {
        if (_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(provider, card) && !_coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(provider, card, card.Kind, false))
            return CardKind.Slash;
        var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(provider, card).FirstOrDefault();
        if (identity is not null)
            return identity.Identity.OutputKind;
        return IsFactionSlashUse(pending) ? CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) ? card.Kind : CardKind.Slash : _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(provider, card, CardKind.Slash);
    }

    internal static void AddConversionParameters(IDictionary<string, string> parameters, CardConversionSource source) => RuntimeSkills.RuntimeRules_Core_CardConversions.AddConversionParameters(parameters, source);
    internal IEnumerable<PromptChoice> CreateConversionChoiceVariants(CharacterState owner, Card card, CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, IReadOnlyList<int> cards, IReadOnlyList<int> targets, IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false)
    {
        var hasIdentity = HasProgramCardIdentity(owner, card);
        var identitySources = GetProgramCardIdentitySources(owner, card, effectiveKind, forResponse);
        var programSources = _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, effectiveKind, forResponse, dyingUse);
        if (!forResponse && effectiveKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(owner) && _coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(owner, card))
            programSources = programSources.Concat(_coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Slash, forResponse: false)).Distinct().ToArray();
        programSources = programSources.Where(source => _characterMiHengLeOperations.SiruiNativeConversionMatchesOutput(owner, source, effectiveKind) && _characterGuanYuWumiaoOperations.JuewuNativeConversionMatchesOutput(owner, source, effectiveKind) && _characterCaoZhiLeOperations.FuleNativeConversionMatchesOutput(owner, source, effectiveKind) && !_coreProgramCardLegalityOperations.CardLegality.IsNativeSlashOutputForbidden(new(owner, card, effectiveKind, source))).ToArray();
        var originalPile = _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(owner, card, effectiveKind, forResponse);
        var includeUnspecified = !_coreWenhePoisonOperations.IsWenhePhysicalPoison(owner, card) && !hasIdentity && !_coreLastZoneConversionsOperations.IsLastZoneJudgmentCard(owner, card) && (!_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(owner, card) || originalPile) && (card.Kind == effectiveKind || programSources.Count == 0) && _characterMiHengLeOperations.SiruiAllowsUnspecifiedForcedSlash(owner, card, effectiveKind) && _characterCaoZhiLeOperations.FuleAllowsUnspecifiedForcedSlash(owner, card, effectiveKind) && !_coreProgramCardLegalityOperations.CardLegality.IsUnspecifiedNativeSlashForbidden(new(owner, card, effectiveKind));
        var sources = new List<CardConversionSource?>();
        if (includeUnspecified)
            sources.Add(null);
        if (hasIdentity)
        {
            sources.AddRange(identitySources);
            if (_characterGuanYuWumiaoOperations.WuyouIdentity(owner, card)is not null)
                sources.AddRange(programSources.Where(_characterGuanYuWumiaoOperations.IsJuewuSource));
        }
        else
        {
            sources.AddRange(programSources);
        }

        for (var index = 0; index < sources.Count; index++)
        {
            var source = sources[index];
            if (!_characterHanDangMouMobileOperations.IsMouHanDangPublishedResponseAllowed(owner, card, source, forResponse, dyingUse))
                continue;
            var parameters = new Dictionary<string, string>(baseParameters);
            var sourceKind = source is not null && (_characterMiHengLeOperations.IsSiruiSource(source) || _characterGuanYuWumiaoOperations.IsJuewuSource(source)) && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind) ? _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind : effectiveKind;
            if (sourceKind != effectiveKind)
            {
                if (parameters.ContainsKey("response-card-kind"))
                    parameters["response-card-kind"] = sourceKind.ToString();
                if (parameters.ContainsKey("effective-kind"))
                    parameters["effective-kind"] = sourceKind.ToString();
            }

            if (source is null && originalPile)
                parameters["native-original-public-pile"] = "true";
            if (source is not null)
                AddConversionParameters(parameters, source);
            var suffix = source is null ? string.Empty : $".conversion-{(index)}";
            if (source is not null && _characterMiHengLeOperations.IsSiruiSource(source) && _characterMiHengLeOperations.SiruiExplicitForcedSlashFrontier(owner))
                suffix += ".sirui-" + sourceKind;
            var choiceDescription = sourceKind == effectiveKind ? description : description.Replace("【杀】", "【" + CardCatalog.Get(sourceKind).DisplayName + "】", StringComparison.Ordinal);
            if (source is not null && _characterMiHengLeOperations.IsSiruiSource(source) && CoreNativeCardClassificationsRuntime.IsSlashCard(sourceKind) && _characterMiHengLeOperations.SiruiExplicitForcedSlashFrontier(owner) && targets is [var targetSeat])
            {
                var convertedCardDescription = $"将【{(card.DisplayName)}】当作【{(CardCatalog.Get(sourceKind).DisplayName)}】使用";
                if (baseParameters.GetValueOrDefault("action") == "qinglong-slash")
                    choiceDescription = $"{(convertedCardDescription)}，继续攻击 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。";
                else if (baseParameters.GetValueOrDefault("response") == "borrowed-sword-slash")
                    choiceDescription = $"{(convertedCardDescription)}攻击 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)}。";
            }

            yield return new PromptChoice(new ChoiceId(baseChoiceId + suffix), DescribeConversion(source, choiceDescription), cards, targets, parameters);
        }
    }

    internal static bool TryReadConversionSource(IReadOnlyDictionary<string, string> parameters, out CardConversionSource? source)
    {
        source = null;
        if (!parameters.TryGetValue("conversion-skill-id", out var skillId) || !parameters.TryGetValue("conversion-binding-id", out var bindingId) || !parameters.TryGetValue("conversion-owner-seat", out var ownerSeatText) || !int.TryParse(ownerSeatText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var ownerSeat) || !parameters.TryGetValue("conversion-instance-id", out var instanceId))
        {
            return false;
        }

        source = new CardConversionSource(skillId, bindingId, ownerSeat, instanceId);
        return true;
    }

    internal static CardConversionSource RequireConversionSource(PromptChoice choice) => TryReadConversionSource(choice.Parameters, out var source) && source is not null ? source : throw new InvalidOperationException("The configured conversion choice lost its source identity.");
    internal void CaptureSelectedResponseConversion(PromptChoice choice)
    {
        if (choice.Cards.Count != 1 || !choice.Parameters.ContainsKey("response-card-kind"))
            return;
        if (TryReadConversionSource(choice.Parameters, out var source))
        {
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = source;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = true;
            return;
        }

        var startsCardAction = choice.Parameters.TryGetValue("response", out var response) && response is "dodge" or "slash" or "faction-defense-dodge" or "faction-slash-slash" or "borrowed-sword-slash" || choice.Parameters.GetValueOrDefault("action") == "qinglong-slash";
        if (startsCardAction && choice.Cards.Count == 1)
        {
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = null;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = true;
        }
    }

    internal void CaptureAiCardResponseChoice(PendingDecision decision, Card card, CardKind effectiveKind)
    {
        var choice = decision.Choices.FirstOrDefault(candidate => candidate.Cards is [var cardId] && cardId == card.Id && candidate.Parameters.GetValueOrDefault("response-card-kind") == effectiveKind.ToString()) ?? throw new InvalidOperationException("The AI response has no published card choice.");
        CaptureSelectedResponseConversion(choice);
    }

    internal CardConversionSource? GetSelectedResponseConversion(CharacterState provider, Card responseCard, CardKind effectiveKind)
    {
        var hasIdentity = HasProgramCardIdentity(provider, responseCard);
        var identitySources = GetProgramCardIdentitySources(provider, responseCard, effectiveKind, forResponse: true);
        var candidates = hasIdentity ? identitySources.Concat(_characterGuanYuWumiaoOperations.WuyouIdentity(provider, responseCard)is not null ? _coreActionQueriesOperations.GetProgramViewAsConversions(provider, responseCard, effectiveKind, forResponse: true).Where(_characterGuanYuWumiaoOperations.IsJuewuSource) : []).Distinct().ToArray() : _coreActionQueriesOperations.GetProgramViewAsConversions(provider, responseCard, effectiveKind, forResponse: true, dyingUse: _coreDyingCursorRuntimeOperations.ActiveDying is not null && effectiveKind is CardKind.Peach or CardKind.Alcohol).ToArray();
        var selected = _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        var selectedChoice = _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = false;
        if (selectedChoice)
        {
            if (selected is null && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(provider, responseCard) && !hasIdentity && !_coreLastZoneConversionsOperations.IsLastZoneJudgmentCard(provider, responseCard) && (!_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(provider, responseCard) || _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(provider, responseCard, effectiveKind, true)) && CoreNativeCardClassificationsRuntime.IsNativeResponseCard(responseCard, effectiveKind) && _characterHanDangMouMobileOperations.IsMouHanDangNativeResponseAllowed(provider, responseCard, _coreDyingCursorRuntimeOperations.ActiveDying is not null && effectiveKind is CardKind.Peach or CardKind.Alcohol))
                return null;
            if (selected is not null && candidates.Contains(selected))
            {
                _coreOwnPublicPileConversionsOperations.ValidateOwnPublicPileConversionCommit(selected, provider, [responseCard], forResponse: true, effectiveKind: effectiveKind);
                return selected;
            }

            throw new InvalidOperationException("The selected response conversion is no longer legal.");
        }

        throw new InvalidOperationException("A card response requires its published source choice.");
    }

    internal void SelectUseConversion(LegalAction action)
    {
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion = action.ConversionSource;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice = true;
    }

    internal CardConversionSource? GetSelectedUseConversion(CharacterState actor, CharacterState provider, Card card, CardKind effectiveKind)
    {
        if (_characterLuSuMobileShiOperations.IsShiLsHandAccess2(actor.Seat, card.Id))
            provider = actor;
        var hasIdentity = HasProgramCardIdentity(provider, card);
        var fanConvertsSlash = effectiveKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor);
        var identitySources = GetProgramCardIdentitySources(provider, card, effectiveKind, forResponse: false);
        var candidates = hasIdentity ? identitySources.Concat(_characterGuanYuWumiaoOperations.WuyouIdentity(provider, card)is not null ? _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, effectiveKind, forResponse: false).Concat(fanConvertsSlash ? _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, CardKind.Slash, forResponse: false) : []).Where(_characterGuanYuWumiaoOperations.IsJuewuSource) : []).Distinct().ToArray() : _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, effectiveKind, forResponse: false).Concat(fanConvertsSlash ? _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, CardKind.Slash, forResponse: false) : []).Distinct().ToArray();
        var selected = _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice ? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion : _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        var selectedChoice = _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice || _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = false;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice = false;
        if (selected is not null)
        {
            return candidates.Contains(selected) ? selected : throw new InvalidOperationException("The selected use conversion is no longer legal.");
        }

        if (!_coreWenhePoisonOperations.IsWenhePhysicalPoison(provider, card) && !hasIdentity && !_coreLastZoneConversionsOperations.IsLastZoneJudgmentCard(provider, card) && (!_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(provider, card) || _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(provider, card, effectiveKind, false) || _characterPangTongFriendOperations.IsFriendYangmingOriginalUseChoice(provider, card, effectiveKind)) && (card.Kind == effectiveKind || fanConvertsSlash && card.Kind == CardKind.Slash) && (selectedChoice || candidates.Length == 0))
            return null;
        throw new InvalidOperationException("A converted card use requires its published source choice.");
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 230)]
    internal bool DistancePhysicalSlash230IgnoresProgramSlashDistance(ProgramSlashDistanceQuery q) => IgnoresProgramSlashDistance(q.Actor, q.Conversion);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 240)]
    internal bool DistancePhysicalSlash240IgnoresSpGuanYuWushengDistance(ProgramSlashDistanceQuery q) => IgnoresSpGuanYuWushengDistance(q.Actor, q.PhysicalCard!);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 250)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 110)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 90)]
    internal bool DistancePhysicalSlash250IgnoresSlashUseDistance(ProgramSlashDistanceQuery q) => IgnoresSlashUseDistance(q.Actor);
}
