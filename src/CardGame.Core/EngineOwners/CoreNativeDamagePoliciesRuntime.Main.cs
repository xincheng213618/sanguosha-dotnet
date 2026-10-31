#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamagePoliciesRuntime
{
    internal DamageNature GetDamageNatureBeforeMobileMaChao(GameEngine.IDamageAttempt attack) => _characterMaChaoAnniversaryGodOperations.ShouliThunderNature(attack) ?? _modeRuntimeOperations._modeRuntime.TransformDamageNature(_coreShenwuEquipmentOperations.ShenwuDamageNature(attack, _characterLuMengMouOperations.IsYingboRepeated(attack.ResolutionId) ? DamageNature.Fire : attack.DamageNatureOverride ?? CoreNativeCardClassificationsRuntime.GetDamageNature(RequireAttackCardKind(attack))));
    internal static Card RequireAttackCard(NativeOwners.CardAttackHandle attack) => attack.Card ?? throw new InvalidOperationException("This card-resolution path requires a physical source card.");
    internal static CardKind RequireAttackCardKind(GameEngine.IDamageAttempt attack) => attack.EffectiveCardKind ?? throw new InvalidOperationException("This card-resolution path requires an effective source card kind.");
    internal int FinalizeAttackDamageAmount(GameEngine.IDamageAttempt attack)
    {
        if (attack.DamageAmountFinalized)
        {
            return attack.DamageAmount;
        }

        _characterDingYuanOlOperations.Value.CaptureAdoptedChildSlashDamageIssue(attack);
        var conversionBonus = attack is NativeOwners.CardAttackHandle converted ? _coreExtendedViewAsOperations.ProgramConversionDamageBonus(converted) : 0;
        var baseAmount = attack.DamageAmount + conversionBonus + (_coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire ? _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Gale) : 0);
        var turnDamageModifiers = attack.EffectiveCardKind is { } effectiveKind ? _turnCardUseEffectsOperations._turnCardUseEffects.GetDamageModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, attack.SourceSeat, attack.CardUserSeat, effectiveKind, attack.IsChainPropagation) : [];
        var programDamageModifiers = _coreProgramDamageHooksOperations.DamageHooks.AppendModifiers(attack, turnDamageModifiers.Select(modifier => (modifier.Source, modifier.Amount))).ToArray();
        var programDamageBonus = _coreProgramDamageHooksOperations.DamageHooks.AddAmounts(attack, programDamageModifiers.Sum(modifier => modifier.Amount));
        var receivesGudingBladeBonus = _coreNativeDamageHandlesOperations.Value.UsesFormalGudingBlade && !attack.IsChainPropagation && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]).Count == 0 && _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], CardKind.GudingBlade);
        var receivesYingboBonus = _characterLuMengMouOperations.IsYingboRepeated(attack.ResolutionId);
        var receivesTengjiaBonus = _coreNativeDamageHandlesOperations.Value.UsesFormalTengjia && _coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire && _coreNativeEquipmentAndFactionQueriesOperations.HasTengjia(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) && (attack.IsChainPropagation || !attack.IgnoresArmor);
        var silverLionCapsDamage = _coreNativeDamageHandlesOperations.Value.UsesFormalSilverLion && baseAmount + programDamageBonus + (receivesGudingBladeBonus ? 1 : 0) + (receivesYingboBonus ? 1 : 0) + (receivesTengjiaBonus ? 1 : 0) > 1 && _coreNativeEquipmentAndFactionQueriesOperations.HasSilverLion(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) && (attack.IsChainPropagation || !attack.IgnoresArmor);
        var passiveDamageCap = _corePassiveDamageCapsOperations.GetPassiveProgramDamageCap(attack);
        var damageBonus = programDamageBonus + conversionBonus + (_coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire ? _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Gale) : 0) + (receivesGudingBladeBonus ? 1 : 0) + (receivesYingboBonus ? 1 : 0) + (receivesTengjiaBonus ? 1 : 0);
        var damageBeforeZhuiLie = checked(attack.DamageAmount + damageBonus);
        var zhuiLieEscalationBonus = _coreFinalTargetSlashOperations.FinalTargetSlashEscalationBonus(attack, damageBeforeZhuiLie);
        if (_coreFinalTargetSlashOperations.HasFinalTargetSlashHpDamage(attack))
            silverLionCapsDamage = damageBeforeZhuiLie + zhuiLieEscalationBonus > 1 && _coreNativeEquipmentAndFactionQueriesOperations.HasSilverLion(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) && (attack.IsChainPropagation || !attack.IgnoresArmor);
        if (_coreWushuangCombatTacticsOperations.WushuangFixedDamage(attack))
        {
            damageBonus = 2 - attack.DamageAmount;
            zhuiLieEscalationBonus = 0;
            silverLionCapsDamage = false;
            passiveDamageCap = null;
        }

        var shefuReplacesDamage = _characterCheZhouAnniversaryOperations.Value.TryGetAnniversaryCheZhouDamageReplacement(attack, out var shefuDamage);
        if (shefuReplacesDamage)
        {
            damageBonus = shefuDamage - attack.DamageAmount;
            zhuiLieEscalationBonus = 0;
            silverLionCapsDamage = _coreNativeDamageHandlesOperations.Value.UsesFormalSilverLion && shefuDamage > 1 && _coreNativeEquipmentAndFactionQueriesOperations.HasSilverLion(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) && (attack.IsChainPropagation || !attack.IgnoresArmor);
        }

        attack.FinalizeDamageAmount(damageBonus + zhuiLieEscalationBonus, silverLionCapsDamage ? 1 : passiveDamageCap?.Cap);
        var runningAmount = baseAmount;
        foreach (var modifier in programDamageModifiers)
        {
            var modifiedAmount = checked(runningAmount + modifier.Amount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardDamageModifiedEvent(attack.ResolutionId, modifier.Source, attack.TargetSeat, attack.EffectiveCardKind, runningAmount, modifiedAmount));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[modifier.Source.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(modifier.Source.SkillId).Name)}】令本次伤害 {((modifier.Amount > 0 ? "+" : ""))}{(modifier.Amount)}。", modifier.Source.OwnerSeat, attack.TargetSeat);
            runningAmount = modifiedAmount;
        }

        if (receivesGudingBladeBonus)
        {
            var modifiedAmount = checked(runningAmount + 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GudingBladeDamageIncreasedEvent(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, RequireAttackCardKind(attack), runningAmount, modifiedAmount));
            _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Name)} 的【古锭刀】对空手的 {(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)} 生效，本次伤害 +1。", attack.SourceSeat, attack.TargetSeat);
            runningAmount = modifiedAmount;
        }

        if (receivesYingboBonus)
        {
            var modifiedAmount = checked(runningAmount + 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YingboDamageIncreasedEvent(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, RequireAttackCardKind(attack), runningAmount, modifiedAmount));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Name)} 的【英博】令本次伤害 +1。", attack.SourceSeat, attack.TargetSeat);
            runningAmount = modifiedAmount;
        }

        if (receivesTengjiaBonus)
        {
            var modifiedAmount = checked(runningAmount + 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TengjiaFireDamageIncreasedEvent(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind, runningAmount, modifiedAmount));
            _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)} 的【藤甲】令本次火焰伤害 +1。", attack.TargetSeat, attack.SourceSeat);
            runningAmount = modifiedAmount;
        }

        if (zhuiLieEscalationBonus != 0)
        {
            var modifiedAmount = checked(damageBeforeZhuiLie + zhuiLieEscalationBonus);
            var escalation = _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).FirstOrDefault(item => item.EscalateToTargetHp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTargetSlashDamageEscalatedEvent(attack.ResolutionId, escalation?.SkillId ?? "", attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind, damageBeforeZhuiLie, modifiedAmount));
            var escalationSkillName = escalation is { } raised ? _contentRegistryOperations._contentRegistry!.GetSkill(raised.SkillId).Name : "追猎";
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Name)} 的【{(escalationSkillName)}】令本次伤害变为 {(modifiedAmount)} 点。", attack.SourceSeat, attack.TargetSeat);
            runningAmount = modifiedAmount;
        }

        // Cap facts must use the complete amount, including native bonuses that
        // do not have an individual ProgramCardDamageModifiedEvent.
        runningAmount = checked(damageBeforeZhuiLie + zhuiLieEscalationBonus);
        if (shefuReplacesDamage)
        {
            _characterCheZhouAnniversaryOperations.Value.RecordAnniversaryCheZhouDamageReplacement(attack, runningAmount, shefuDamage);
            runningAmount = shefuDamage;
        }

        if (silverLionCapsDamage)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SilverLionDamageCappedEvent(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind, runningAmount, 1));
            _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)} 的【白银狮子】将本次伤害改为 1 点。", attack.TargetSeat, attack.SourceSeat);
            runningAmount = 1;
        }

        if (passiveDamageCap is { } damageCap && runningAmount > damageCap.Cap)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageCappedEvent(attack.ResolutionId, damageCap.Source, attack.IsSourceLess ? null : attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind, runningAmount, damageCap.Cap));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(damageCap.Source.SkillId).Name)}】将本次伤害改为 {(damageCap.Cap)} 点。", attack.TargetSeat, attack.SourceSeat);
        }

        return attack.DamageAmount;
    }

    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetPassiveProgramDamageModifiers(GameEngine.IDamageAttempt attack, bool cardDamageOnly = false)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive)
            return[];
        var sourceAndTarget = new[]
        {
            attack.SourceSeat,
            attack.TargetSeat
        }.Distinct();
        var owners = _corePassiveRulePoliciesOperations.HasPublicEnemyParticipantDamageModifiers ? _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => player.Seat) : sourceAndTarget;
        var borrowedOwner = _coreQiaoquHaoduoOperations.Value.BorrowedSwordSlashDamageModifierOwner(attack);
        if (borrowedOwner is { } borrower && _coreMatchParticipantsPlayersAccess._players[borrower].IsAlive)
            owners = owners.Append(borrower).Distinct();
        return owners.SelectMany(seat => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Where(instance => _coreShanheSchoolsOperations.IsShanheSchoolSkillQualified(seat, instance.SkillId)).SelectMany(instance => instance.Program.DamageModifiers.Select(modifier => (seat, instance, modifier)))).Where(item => !item.modifier.CapsToAmount && (!cardDamageOnly || item.modifier.DamageLayer == SkillProgramDamageLayer.CardDamage) && (item.modifier.SourceScope == SkillProgramDamageModifierSourceScope.BorrowedSwordUseOwner ? borrowedOwner == item.seat && _coreMatchParticipantsPlayersAccess._players[item.seat].IsAlive && attack.EffectiveCardKind is { } borrowedKind && ProgramCardKindMatch.MatchesDamageModifier(item.modifier, borrowedKind) : item.modifier.SourceScope == SkillProgramDamageModifierSourceScope.AllLivingSkillOwners ? item.modifier.Condition == SkillProgramDamageModifierCondition.BothParticipantsPublicEnemiesOfOwner && !attack.IsSourceLess && attack.SourceSeat != attack.TargetSeat && _coreTeamActivityProgramsOperations.PublicTeamEnemy(item.seat, _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]) && _coreTeamActivityProgramsOperations.PublicTeamEnemy(item.seat, _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) : item.modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageParticipant && item.modifier.Condition == SkillProgramDamageModifierCondition.OwnerMaximumHandCurrentRound ? (item.seat == attack.TargetSeat || !attack.IsSourceLess && item.seat == attack.SourceSeat) && _coreShanheS5QuyiOperations.IsShanheS5JiaoziQualified(item.seat) : _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive && (item.seat == attack.SourceSeat || item.seat == attack.TargetSeat) && (item.modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource ? item.seat == attack.SourceSeat && !attack.IsSourceLess && (item.modifier.Condition is SkillProgramDamageModifierCondition.SourcePreviousRoundNoDamage or SkillProgramDamageModifierCondition.SourcePreviousRoundDamageBelowThree ? _coreShanheBadaoOperations.MatchesShanheBadaoDamageModifier(item.modifier, attack.SourceSeat, attack.IsChainPropagation) : item.modifier.Condition == SkillProgramDamageModifierCondition.DirectSkillDamage ? _coreDirectSkillDamageOperations.Value.IsDirectSkillDamage(attack) : CoreGameEnginePassiveRulePoliciesRuntime.MatchesPerOccurrenceSourceDamageModifier(item.modifier, _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].HasHpValue, _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Hp, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).Count, item.modifier.Condition == SkillProgramDamageModifierCondition.SourceToTargetDistanceGreaterThan && attack.SourceSeat != attack.TargetSeat ? _coreNativeRuleQueriesOperations.GetCombatDistance(attack.SourceSeat, attack.TargetSeat) : 0, _coreActualRoundProgressRoundNumberAccess._roundNumber)) : item.modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageParticipant ? item.modifier.Condition switch
        {
            SkillProgramDamageModifierCondition.OwnerUniqueMaximumHand => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != item.seat).All(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[item.seat]).Count),
            SkillProgramDamageModifierCondition.ChainedFirePropagationOrigin => item.seat == attack.TargetSeat && !attack.IsChainPropagation && _coreNativeCardClassificationsOperations.GetDamageNature(attack) == DamageNature.Fire && _coreMatchParticipantsPlayersAccess._players[item.seat].IsChained && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != item.seat && player.IsChained),
            SkillProgramDamageModifierCondition.FaceStatesDiffer => attack.SourceSeat != attack.TargetSeat && _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsFaceDown != _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsFaceDown,
            SkillProgramDamageModifierCondition.DamageSourceAttackRangeBelowThree => item.seat == attack.TargetSeat && _coreNativeRuleQueriesOperations.GetAttackRange(attack.SourceSeat) < 3,
            SkillProgramDamageModifierCondition.DamageSourceAttackRangeAboveThree => item.seat == attack.TargetSeat && _coreNativeRuleQueriesOperations.GetAttackRange(attack.SourceSeat) > 3,
            _ => false
        } : item.seat == attack.SourceSeat && !attack.IsChainPropagation && !attack.IsDelayedJudgmentDamage && !attack.IsProgramJudgmentDamage && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && ProgramCardKindMatch.MatchesDamageModifier(item.modifier, kind) && (item.modifier.Condition switch
        {
            SkillProgramDamageModifierCondition.Always => true,
            SkillProgramDamageModifierCondition.OwnerUsedFromRound => _coreActualRoundProgressRoundNumberAccess._roundNumber >= item.modifier.MinimumRound,
            SkillProgramDamageModifierCondition.OwnerUsedHeartSlash => _coreHeartSlashDamageOperations.Value.MatchesOwnerUsedHeartSlash(attack),
            SkillProgramDamageModifierCondition.SourceOutsideTargetAttackRange => !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(attack.TargetSeat, attack.SourceSeat),
            SkillProgramDamageModifierCondition.SourceNotFewerHandAndEquipmentThanTarget => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).Count && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]).Count <= _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).Count,
            SkillProgramDamageModifierCondition.SourceHandBelowLimit => !attack.IsSourceLess && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).Count < item.modifier.SourceHandCountLimit,
            SkillProgramDamageModifierCondition.OwnerActualTurnTrickUseOrdinalWithinLimit => !attack.IsSourceLess && _coreWholeActualTurnUsesOperations.CapturedWholeActualTurnTrickUseOrdinal(attack)is { } ordinal && ordinal <= item.modifier.MaximumUseOrdinal,
            SkillProgramDamageModifierCondition.OwnerRoundSlashUseOrdinalWithinLimit or SkillProgramDamageModifierCondition.OwnerActualTurnElementalSlashUseOrdinalWithinLimit or SkillProgramDamageModifierCondition.OwnerGameSlashUseOrdinalMultipleOfThree => !attack.IsSourceLess && _coreSlashUseOrdinalsOperations.MatchesSlashUseOrdinalModifier(item.modifier, attack, attack.CardUserSeat, kind),
            _ => false
        })))).Where(item => _coreShanheSchoolLateOperations.ProgramDamageModifierAmount(item.modifier) != 0).SelectMany(item => Enumerable.Repeat((new CardUseEffectSource(item.instance.SkillId, item.modifier.Id, item.seat, item.instance.SkillInstanceId), _coreShanheSchoolLateOperations.ProgramDamageModifierAmount(item.modifier)), CharacterQuYiClassicRuntime.ClassicJiaoziDamageOccurrences(item.modifier, attack.SourceSeat, attack.TargetSeat, attack.IsSourceLess))).ToArray();
    }

    internal bool IsSlashProhibited(CharacterState target)
    {
        return IsCardTargetProhibited(target, CardKind.Slash);
    }

    internal bool IsCardTargetProhibited(CharacterState target, CardKind cardKind, Suit? suit = null, bool? effectiveIsRed = null) => CoreNativeCardClassificationsRuntime.IsDelayedCard(cardKind) && _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) || _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitTarget, cardKind) || suit is { } declaredSuit && _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.ProhibitTargetBySuit, cardKind).Any(item => item.Policy.InputSuit == declaredSuit) || _coreDyingWindowSkillPoliciesOperations.IsBlackTrickTargetProhibited(target, cardKind, effectiveIsRed);
    internal bool IsSlashProhibited(CharacterState source, CharacterState target, Card slashCard)
    {
        if (IsSlashProhibited(target))
        {
            return true;
        }

        return false;
    }

    internal static bool IsBlackCardUse(IReadOnlyList<Card> physicalCards) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsBlackCardUse(physicalCards);
    internal static bool CanYizhongNullify(IReadOnlyList<Card> physicalCards, bool hasArmor) => RuntimeSkills.RuntimeRules_Core_CommonRules.CanYizhongNullify(physicalCards, hasArmor);
    internal static bool IsTrickCardDamage(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsTrickCardDamage(kind);
    internal static bool CanWuyanPreventDamage(bool isClassicIdentityMode, CardKind kind, bool sourceHasWuyan, bool targetHasWuyan) => RuntimeSkills.RuntimeRules_Core_CommonRules.CanWuyanPreventDamage(isClassicIdentityMode, kind, sourceHasWuyan, targetHasWuyan);
    internal bool TryPreventWuyanDamage(GameEngine.IDamageAttempt attack, int amount)
    {
        if (attack.EffectiveCardKind is not { } cardKind)
        {
            return false;
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var sourcePolicy = _coreProgramCardPoliciesOperations.CardPolicies(source, SkillProgramCardPolicyKind.PreventTrickDamage, cardKind).FirstOrDefault().Source;
        var targetPolicy = _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.PreventTrickDamage, cardKind).FirstOrDefault().Source ?? _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.PreventIncomingTrickDamage, cardKind).FirstOrDefault().Source;
        if (sourcePolicy is null && targetPolicy is null)
        {
            return false;
        }

        var skillOwner = sourcePolicy is not null ? source : target;
        var skill = sourcePolicy ?? targetPolicy!;
        _coreMatchLogOperations.AddLog("DamagePrevented", $"{(skillOwner.Name)} 的【{(skill.Definition.Name)}】防止了【{(CardCatalog.Get(cardKind).DisplayName)}】造成的 {(amount)} 点伤害。", skillOwner.Seat, skillOwner.Seat == source.Seat ? target.Seat : source.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyanDamagePreventedEvent(attack.ResolutionId, source.Seat, target.Seat, cardKind, amount, skillOwner.Seat, skill.SkillId));
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeProgramWindow, 10)]
    internal ProgramDamageInterception InterceptWuyanDamage(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => TryPreventWuyanDamage(attack, amount) ? ProgramDamageInterception.Prevented : ProgramDamageInterception.Continue;
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 10)]
    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetActualPassiveProgramDamageModifiers(GameEngine.IDamageAttempt attack) => GetPassiveProgramDamageModifiers(attack);
}
