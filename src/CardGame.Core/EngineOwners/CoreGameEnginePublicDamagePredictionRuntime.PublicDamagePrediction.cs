#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicDamagePredictionRuntime
{
    internal bool HasPredictionHpPolicy(CharacterState owner, SkillProgramCardPolicyKind kind, PlayerSkillContext context, IReadOnlySet<string>? visibleSkillIds = null) => owner.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(kind).Any(binding => (visibleSkillIds is null || visibleSkillIds.Contains(binding.Source.SkillId)) && binding.Policy.CardKinds.Count == 0 && binding.Policy.RequiredCardKinds.Count == 0 && (binding.Policy.OwnerRole is null || _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, binding.Source.SkillId, binding.Source.SkillInstanceId, binding.Policy.OwnerRole.Value)) && binding.Policy.Condition.Evaluate(context));
    internal PlayerSkillContext CreateAiSkillContext(CharacterState player, int viewerSeat, GameSnapshot? snapshot = null)
    {
        var context = _coreNativeSkillContextQueriesOperations.CreateSkillContext(player);
        if (player.Seat == viewerSeat)
            return context;
        snapshot ??= _corePlayerViewProjectionOperations.CreateSnapshot(viewerSeat);
        var visible = VisiblePredictionSkillIds(snapshot.Players.Single(target => target.Seat == player.Seat));
        return context with
        {
            IncomingDamageCap = _corePassiveDamageCapsOperations.GetIncomingFixedDamageCap(player, visible),
            DamageBecomesHpLoss = HasPredictionHpPolicy(player, SkillProgramCardPolicyKind.DamageBecomesHpLoss, context, visible),
            OtherParticipantDamageBecomesHpLoss = HasPredictionHpPolicy(player, SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss, context, visible)
        };
    }

    internal PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, LegalAction action, int targetSeat)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(viewerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat))
            throw new ArgumentOutOfRangeException("viewerSeat");
        return PredictPublicActionDamage(_corePlayerViewProjectionOperations.CreateSnapshot(viewerSeat), viewerSeat, action, targetSeat);
    }

    internal PublicDamagePrediction PredictPublicActionDamage(GameSnapshot view, int viewerSeat, LegalAction action, int targetSeat)
    {
        var self = view.Players.Single(player => player.Seat == viewerSeat);
        // An unchanged physical Fire/Thunder Slash has no PlayedCardKind
        // override. Resolve only material already visible in the viewer's own
        // zones; an arbitrary foreign ID must never probe the zone registry.
        var ownMaterial = action.PlayedCardKind is null && action.Kind == LegalActionKind.Slash && action.CardId is { } materialId ? self.Hand.Concat(self.WoodenOxGrain ?? []).Concat(self.Equipment).Concat(self.AuthorityCards ?? []).FirstOrDefault(card => card.Id == materialId) : null;
        var nativeSlashKind = ownMaterial is { Kind: var printedKind } && CoreNativeCardClassificationsRuntime.IsSlashCard(printedKind) ? printedKind : CardKind.Slash;
        var kind = action.PlayedCardKind ?? action.Kind switch
        {
            LegalActionKind.Slash => nativeSlashKind,
            LegalActionKind.Duel => CardKind.Duel,
            LegalActionKind.BarbarianAssault => CardKind.BarbarianAssault,
            LegalActionKind.ArrowBarrage => CardKind.ArrowBarrage,
            LegalActionKind.FireAttack => CardKind.FireAttack,
            _ => (CardKind? )null
        };
        if (kind is not { } damageKind || !(CoreNativeCardClassificationsRuntime.IsSlashCard(damageKind) || damageKind is CardKind.Duel or CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.FireAttack))
        {
            var harmlessTarget = view.Players.Single(player => player.Seat == targetSeat);
            return new(harmlessTarget.Hp, harmlessTarget.Armor, 0, 0, 0, harmlessTarget.Hp, harmlessTarget.Armor, harmlessTarget.MaxHp);
        }

        var ownHand = self.Hand;
        var paidHand = action.CardId is { } id && ownHand.Any(card => card.Id == id) ? 1 : 0;
        var amount = kind is { } card && CoreNativeCardClassificationsRuntime.IsSlashCard(card) ? checked(1 + (_coreMatchParticipantsPlayersAccess._players[viewerSeat].HasAlcoholEffect ? Math.Max(1, _coreMatchParticipantsPlayersAccess._players[viewerSeat].AlcoholDamageBonus) : 0)) : 1;
        amount = checked(amount + _coreShanheLeizhenOperations.ShanheLeizhenPublicActionBonus(viewerSeat, kind));
        if (damageKind == CardKind.Duel && _coreShanheS5ShuangxiongOperations.TryGetShanheS5ShuangxiongPublicDuelBaseDamage(viewerSeat, action, out var scaled))
            amount = scaled;
        amount = checked(amount + _characterChengPuBoundaryMobileOperations.BoundaryChengPuPublicActionBonus(viewerSeat, action, targetSeat));
        var prediction = PredictVisibleDamage(view, viewerSeat, targetSeat, kind is null ? 0 : amount, kind, kind is not null, false, false, false, paidHand, _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[viewerSeat]), prospectiveSuit: _coreHeartSlashDamageOperations.PublicProposedSlashSuit(view, viewerSeat, action, damageKind));
        return _coreWushuangPublicPredictionOperations.AttachWushuangPublicCombatPrediction(view, viewerSeat, action, targetSeat, amount, damageKind, paidHand, prediction);
    }

    internal PublicDamagePrediction PredictPublicIncomingDamage(GameEngine.IDamageAttempt attack, int viewerSeat, int? sourceSeat = null, int? targetSeat = null, int? damageAmount = null)
    {
        var source = sourceSeat ?? attack.SourceSeat;
        var target = targetSeat ?? attack.TargetSeat;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(viewerSeat);
        var amount = damageAmount ?? attack.DamageAmount;
        var cardUse = attack is NativeOwners.CardAttackHandle && !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage && attack.CardUserSeat == source;
        var legacy = PredictVisibleDamage(view, source, target, amount, attack.EffectiveCardKind, cardUse, attack.IsSourceLess, attack.IsChainPropagation, attack.DamageAmountFinalized, 0, attack.IgnoresArmor, _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack);
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || view.Players.Count != 2 || source == target)
            return legacy;
        // The incoming use is already declared. Its public receipt freezes the
        // suit and use ordinal; never inspect an opponent's unplayed materials.
        var use = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatUseFactEvent>().LastOrDefault(fact => fact.CardUseFrameId == attack.ResolutionId);
        var payment = PredictVisibleDamage(view, source, target, amount, attack.EffectiveCardKind, cardUse, attack.IsSourceLess, attack.IsChainPropagation, attack.DamageAmountFinalized, 0, attack.IgnoresArmor, _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack, wushuangTactics: true, prospectiveSuit: use?.Suit ?? Suit.None);
        return legacy with
        {
            Wushuang = new(payment, null, false, !attack.IsSourceLess && view.Players.Single(player => player.Seat == source).WushuangTacticIds?.Contains(2210) == true)
        };
    }

    internal PublicDamagePrediction PredictVisibleDamage(GameSnapshot view, int sourceSeat, int targetSeat, int amount, CardKind? kind, bool cardUse, bool sourceLess, bool chain, bool finalized, int paidHandCards, bool ignoresEquipmentArmor, DamageNature? damageNature = null, GameEngine.IDamageAttempt? issuedAttack = null, bool wushuangTactics = false, Suit prospectiveSuit = Suit.None, int additionalIncomingSegments = 0, int priorTargetSegments = 0)
    {
        var target = view.Players.Single(player => player.Seat == targetSeat);
        var source = view.Players.Single(player => player.Seat == sourceSeat);
        var sourceState = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        var targetState = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var sourceVisible = VisiblePredictionSkillIds(source);
        var targetVisible = VisiblePredictionSkillIds(target);
        var fire = damageNature == DamageNature.Fire || damageNature is null && (kind is CardKind.FireSlash or CardKind.FireAttack || !sourceLess && (sourceVisible.Contains("shenwu:zhaohuo") && _coreShenwuNpcProgramsOperations.ShenwuEnabled(sourceState, "shenwu:zhaohuo") || _coreAdvancedWeaponsOperations.HasWeaponAbility(sourceState, CardKind.CrimsonSoulZither)));
        if (!finalized && fire && targetVisible.Contains("shenwu:zhaohuo") && _coreShenwuNpcProgramsOperations.ShenwuEnabled(targetState, "shenwu:zhaohuo"))
            return PublicDamagePrediction.Calculate(target.Hp, target.Armor, 0, maxHp: target.MaxHp);
        bool HasVisiblePolicy(CharacterState player, HashSet<string> visible, SkillProgramCardPolicyKind policy) => player.IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(player, policy).Any(binding => visible.Contains(binding.Source.SkillId));
        var cardLayer = amount;
        var additional = 0;
        if (!finalized)
            additional = checked(additional + CoreGameEngineQibaoDaoRuntime.QiBaoDaoDamageBonus(cardUse && kind is { } slash && CoreNativeCardClassificationsRuntime.IsSlashCard(slash), sourceLess, chain, _coreAdvancedWeaponsOperations.HasWeaponAbility(sourceState, CardKind.QiBaoDao), !target.HpUndefined, target.Hp, target.MaxHp));
        if (!finalized && !sourceLess && source.IsAlive)
        {
            if (cardUse && !chain && kind is { } card)
                cardLayer = checked(cardLayer + _turnCardUseEffectsOperations._turnCardUseEffects.GetDamageModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, sourceSeat, sourceSeat, card, false).Sum(modifier => modifier.Amount));
            foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(sourceState).ProgramInstances.Where(instance => sourceVisible.Contains(instance.SkillId) && _coreShanheSchoolsOperations.IsShanheSchoolSkillQualified(sourceSeat, instance.SkillId)))
                foreach (var modifier in instance.Program.DamageModifiers)
                {
                    if (modifier.CapsToAmount)
                        continue;
                    var eligible = modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource ? modifier.Condition is SkillProgramDamageModifierCondition.SourcePreviousRoundNoDamage or SkillProgramDamageModifierCondition.SourcePreviousRoundDamageBelowThree ? _coreShanheBadaoOperations.MatchesShanheBadaoDamageModifier(modifier, sourceSeat, chain) : modifier.Condition == SkillProgramDamageModifierCondition.DirectSkillDamage ? issuedAttack is { } skillAttack && _coreDirectSkillDamageOperations.IsDirectSkillDamage(skillAttack) : CoreGameEnginePassiveRulePoliciesRuntime.MatchesPerOccurrenceSourceDamageModifier(modifier, !source.HpUndefined, source.Hp, Math.Max(0, source.HandCount - paidHandCards), modifier.Condition == SkillProgramDamageModifierCondition.SourceToTargetDistanceGreaterThan && sourceSeat != targetSeat ? _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, targetSeat) : 0, _coreActualRoundProgressRoundNumberAccess._roundNumber) : modifier.SourceScope == SkillProgramDamageModifierSourceScope.OwnerUsed && cardUse && !chain && kind is { } effective && ProgramCardKindMatch.MatchesDamageModifier(modifier, effective) && (modifier.Condition switch
                    {
                        SkillProgramDamageModifierCondition.Always => true,
                        SkillProgramDamageModifierCondition.OwnerUsedFromRound => _coreActualRoundProgressRoundNumberAccess._roundNumber >= modifier.MinimumRound,
                        SkillProgramDamageModifierCondition.OwnerUsedHeartSlash => issuedAttack is null ? prospectiveSuit == Suit.Heart : _coreHeartSlashDamageOperations.MatchesOwnerUsedHeartSlash(issuedAttack),
                        SkillProgramDamageModifierCondition.SourceHandBelowLimit => Math.Max(0, source.HandCount - paidHandCards) < modifier.SourceHandCountLimit,
                        SkillProgramDamageModifierCondition.SourceNotFewerHandAndEquipmentThanTarget => target.HandCount <= Math.Max(0, source.HandCount - paidHandCards) && target.Equipment.Count <= source.Equipment.Count,
                        SkillProgramDamageModifierCondition.SourceOutsideTargetAttackRange => !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(targetSeat, sourceSeat),
                        SkillProgramDamageModifierCondition.OwnerRoundSlashUseOrdinalWithinLimit or SkillProgramDamageModifierCondition.OwnerActualTurnElementalSlashUseOrdinalWithinLimit or SkillProgramDamageModifierCondition.OwnerGameSlashUseOrdinalMultipleOfThree => _coreSlashUseOrdinalsOperations.MatchesSlashUseOrdinalModifier(modifier, issuedAttack, sourceSeat, effective),
                        _ => false
                    });
                    if (!eligible)
                        continue;
                    if (cardUse && modifier.DamageLayer == SkillProgramDamageLayer.CardDamage)
                        cardLayer = checked(cardLayer + _coreShanheSchoolLateOperations.ProgramDamageModifierAmount(modifier));
                    else
                        additional = checked(additional + _coreShanheSchoolLateOperations.ProgramDamageModifierAmount(modifier));
                }
        }

        if (!finalized && issuedAttack is { } originalAttack)
            cardLayer = checked(cardLayer + _characterPanZhangMaZhongBoundaryMobileOperations.BoundaryAnjianDamage(originalAttack) + _characterChengPuBoundaryMobileOperations.BoundaryChengPuLihuoDamage(originalAttack) + _characterHuangZhongBoundaryMobileOperations.BoundaryHuangZhongCardDamage(originalAttack) + _characterZhangFeiBoundaryMobileOperations.BoundaryZhangFeiLiyongCardDamage(originalAttack));
        if (!finalized)
            additional = checked(additional + _coreShanheS5QuyiOperations.ShanheS5JiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards) + _characterQuYiClassicOperations.ClassicJiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards));
        if (!finalized && !sourceLess && !chain)
            additional = checked(additional + _coreShanheSourceShennuOperations.ShanheShennuPublicDamageBonus(view, sourceSeat, sourceLess, chain, issuedAttack));
        if (!finalized && issuedAttack is null && cardUse && !sourceLess && !chain)
            additional = checked(additional + _coreWushuangAuditCombatOperations.WangongPublicProposedBonus(view, sourceSeat, kind));
        if (!finalized && (issuedAttack is not null || cardUse))
            additional = checked(additional + _coreShanheWoxinOperations.WoxinPublicBonus(view, sourceSeat, sourceLess, chain, issuedAttack));
        if (wushuangTactics && !finalized && !sourceLess && !chain)
            additional = checked(additional + _coreWushuangPublicPredictionOperations.WushuangPublicNativeDamageBonus(view, sourceSeat, targetSeat, kind, prospectiveSuit, additionalIncomingSegments, priorTargetSegments, issuedAttack?.ResolutionId));
        cardLayer = Math.Max(0, cardLayer);
        if (!sourceLess && (HasVisiblePolicy(sourceState, sourceVisible, SkillProgramCardPolicyKind.DamageBecomesHpLoss) || sourceSeat != targetSeat && (HasVisiblePolicy(sourceState, sourceVisible, SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss) || HasVisiblePolicy(targetState, targetVisible, SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss))))
            return PublicDamagePrediction.Calculate(target.Hp, target.Armor, cardLayer, PublicDamagePaymentKind.HpLoss, target.MaxHp);
        if (!sourceLess && source.IsAlive && cardUse && !chain && !finalized && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 1 && kind is { } replacementKind && !target.HpUndefined && _coreProgramCardPoliciesOperations.CardPolicies(sourceState, SkillProgramCardPolicyKind.FirstRoundCardDamageBecomesMaximumHpLoss, replacementKind).Any(binding => sourceVisible.Contains(binding.Source.SkillId) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(sourceSeat, binding.Source.SkillId, CoreGameEngineCardDamageMaximumHpReplacementRuntime.MaximumHpReplacementUsage(binding.Policy.Id), SkillUsageScope.Round) == 0))
            return PublicDamagePrediction.Calculate(target.Hp, target.Armor, cardLayer, PublicDamagePaymentKind.MaximumHpLoss, target.MaxHp);
        var full = Math.Max(0, checked(cardLayer + additional));
        var fixedWushuang = wushuangTactics && !sourceLess && source.WushuangTacticIds?.Contains(2210) == true;
        if (fixedWushuang)
            full = 2;
        if (!fixedWushuang && !finalized && _coreNativeDamageHandlesOperations.UsesFormalSilverLion && (chain || !ignoresEquipmentArmor) && _coreNativeEquipmentAndFactionQueriesOperations.HasSilverLion(targetState))
            full = Math.Min(1, full);
        if (!fixedWushuang && !finalized && _corePassiveDamageCapsOperations.FindPassiveProgramDamageCap(targetState, sourceLess ? null : sourceSeat, targetVisible)is { } damageCap)
            full = Math.Min(damageCap.Cap, full);
        var bypass = _coreDuanhunOperations.DuanhunBypassesNativeDamageArmor(sourceSeat, sourceLess, sourceVisible) || sourceSeat != targetSeat && _coreNativeDamageArmorPoliciesOperations.HasNativeDamageArmorPolicy(sourceSeat, sourceLess, SkillProgramCardPolicyKind.BypassOtherParticipantArmor, sourceVisible);
        var armorMultiplier = _coreNativeDamageArmorPoliciesOperations.HasNativeDamageArmorPolicy(sourceSeat, sourceLess, SkillProgramCardPolicyKind.DoubleArmorDamage, sourceVisible) ? 2 : 1;
        return PublicDamagePrediction.Calculate(target.Hp, target.Armor, full, maxHp: target.MaxHp, bypassArmor: bypass, armorDamageMultiplier: armorMultiplier);
    }

    internal static HashSet<string> VisiblePredictionSkillIds(PlayerSnapshot player) => RuntimeSkills.RuntimeRules_Core_PublicDamagePrediction.VisiblePredictionSkillIds(player);
    internal Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasSourceDistanceDamageModifiers && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasSourceHpDamageModifiers && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasSourceRoundDamageModifiers)
            return null;
        // One detached viewer projection is shared by all candidate estimates.
        // No skill IDs or predicates are added to a player-facing snapshot.
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(viewerSeat);
        var visibleSkills = view.Players.ToDictionary(player => player.Seat, VisiblePredictionSkillIds);
        return (sourceSeat, targetSeat) =>
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
                return 0;
            var modifiers = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).ProgramInstances.Where(instance => sourceSeat == viewerSeat || visibleSkills[sourceSeat].Contains(instance.SkillId)).SelectMany(instance => instance.Program.DamageModifiers).Where(modifier => modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource && modifier.Condition is SkillProgramDamageModifierCondition.SourceToTargetDistanceGreaterThan or SkillProgramDamageModifierCondition.SourceHpAtMost or SkillProgramDamageModifierCondition.SourceRoundIsOdd or SkillProgramDamageModifierCondition.SourceRoundIsEven).ToArray();
            var distance = sourceSeat != targetSeat && modifiers.Any(modifier => modifier.Condition == SkillProgramDamageModifierCondition.SourceToTargetDistanceGreaterThan) ? _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, targetSeat) : 0;
            var source = view.Players[sourceSeat];
            return modifiers.Where(modifier => CoreGameEnginePassiveRulePoliciesRuntime.MatchesPerOccurrenceSourceDamageModifier(modifier, !source.HpUndefined, source.Hp, 0, distance, _coreActualRoundProgressRoundNumberAccess._roundNumber)).Sum(modifier => modifier.Amount);
        };
    }
}
