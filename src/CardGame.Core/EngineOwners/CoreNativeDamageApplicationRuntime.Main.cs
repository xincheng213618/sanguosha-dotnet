#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDamageApplicationRuntime
{
    internal bool ApplyAttackDamage(GameEngine.IDamageAttempt attack)
    {
        if (attack is NativeOwners.CardAttackHandle responseAttack && _characterSimaYiMobileGodOperations.TryBeginMobileSimaRenjieFailedAttack(responseAttack))
            return true;
        _characterCaoAngMouOperations.PrepareMouCaDamageSource(attack);
        if (_characterXinPiOlOperations.IsXinPiRemainingCardDamageIneffective(attack))
            return false;
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(attack.ResolutionId, new(RecoveryPaidCardUseKind.AttackDamage, attack.SourceSeat)))
            return true;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!target.IsAlive)
        {
            return false;
        }

        if (!attack.IsSourceLess && source.IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(source, SkillProgramCardPolicyKind.DamageBecomesHpLoss))
            return _coreHealthReplacementProgramsOperations.ApplyAttackAsHpLoss(attack);
        if (_coreOtherParticipantHealthReplacementOperations.TryGetOtherParticipantHpReplacement(attack, out var replacementSource))
        {
            if (_coreHealthQueriesOperations.TryBeginHappyQiaoAttackHpLossChoice(attack))
                return true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OtherParticipantDamageReplacedEvent(attack.ResolutionId, replacementSource, attack.SourceSeat, target.Seat, _coreCardDamageMaximumHpReplacementOperations.CalculateCardDamageLayer(attack), _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack.IsChainPropagation));
            return _coreHealthReplacementProgramsOperations.ApplyAttackAsHpLoss(attack);
        }

        if (_coreCardDamageMaximumHpReplacementOperations.TryReplaceCardDamageWithMaximumHpLoss(attack, out var replacementPending))
            return replacementPending;
        if (!attack.IsChainPropagation && (_coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(attack.ResolutionId, target.Seat) || _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(attack.ResolutionId, CurrentCardEnhancement.IgnoreArmor)))
            attack.SetIgnoresArmor(true);
        if (!attack.DamageAmountFinalized && attack is NativeOwners.CardAttackHandle && (_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.DamageBecomesHpLoss) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.FirstRoundCardDamageBecomesMaximumHpLoss)))
            _ = _coreCardDamageMaximumHpReplacementOperations.CalculateCardDamageLayer(attack);
        if (_characterMangYaChangAnniversaryOperations.TryBeginAnniversaryJiedaoSourceWindow(attack))
            return true;
        if (_characterCaiYangAnniversaryOperations.TryBeginAnniversaryCaiYangJiaofengSourceWindow(attack))
            return true;
        var nature = _coreNativeCardClassificationsOperations.GetDamageNature(attack);
        var amount = _coreNativeDamagePoliciesOperations.FinalizeAttackDamageAmount(attack);
        if (amount <= 0)
            return false;
        if (nature != DamageNature.Thunder && target.Markers.GetValueOrDefault(PlayerMarkerKind.Mist) > 0)
        {
            _coreMatchLogOperations.AddLog("DamagePrevented", $"{(target.Name)} 的大雾防止了伤害。", attack.SourceSeat, target.Seat);
            return false;
        }

        var beforeWindow = _coreProgramDamageHooksOperations.DamageHooks.Intercept(ProgramDamagePhase.BeforeProgramWindow, attack, amount, nature);
        if (beforeWindow != ProgramDamageInterception.Continue)
            return beforeWindow == ProgramDamageInterception.Paused;
        if (_coreDamagePreventionProgramsOperations.TryBeginBeforeDamageProgramWindowForAttack(attack, amount, nature))
        {
            return true;
        }

        if (_coreColdDamageOperations.TryBeginColdDamageReplacement(attack, amount, nature))
            return true;
        if (attack is NativeOwners.CardAttackHandle iceSwordAttack && _coreNativeIceSwordOperations.TryBeginIceSwordChoice(iceSwordAttack, amount))
        {
            return true;
        }

        if (attack is NativeOwners.CardAttackHandle qilinBowAttack && _coreNativeQilinBowOperations.TryBeginQilinBowChoice(qilinBowAttack))
        {
            return true;
        }

        if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForDamage(attack, amount))
            return true;
        var afterEquipment = _coreProgramDamageHooksOperations.DamageHooks.Intercept(ProgramDamagePhase.AfterEquipmentChoices, attack, amount, nature);
        if (afterEquipment != ProgramDamageInterception.Continue)
            return afterEquipment == ProgramDamageInterception.Paused;
        amount = attack.DamageAmount;
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoDamageChoice(attack))
            return true;
        var qiaoHealthOwner = _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target.Seat) ? _resolutionStackOperations._resolutionStack.Last().Id : (long? )null;
        if (!attack.IsChainPropagation && (nature is DamageNature.Fire or DamageNature.Thunder or DamageNature.Cold) && target.IsChained)
        {
            var propagatedTargets = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != target.Seat && player.IsChained).Select(player => player.Seat).ToArray();
            var chainMembers = new[]
            {
                target.Seat
            }.Concat(propagatedTargets).ToArray();
            _coreRecipientScopedDamageOperations.CaptureRecipientScopedDamageBase(attack, propagatedTargets);
            attack.SetChainedTargets(propagatedTargets);
            foreach (var chainSeat in chainMembers)
            {
                _coreMatchParticipantsPlayersAccess._players[chainSeat].IsChained = _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(_coreMatchParticipantsPlayersAccess._players[chainSeat], _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[chainSeat], SkillProgramCardPolicyKind.ForceChained));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(attack.ResolutionId, source.Seat, chainSeat, IsChained: false));
            }

            if (propagatedTargets.Length > 0)
            {
                _coreMatchLogOperations.AddLog("ChainDamage", $"{(target.Name)} 受到{(CoreNativeCardClassificationsRuntime.GetDamageNatureLabel(nature))}伤害，向 {(propagatedTargets.Length)} 名连环角色传导同量伤害。", source.Seat, target.Seat);
            }
        }

        var damageFrameId = _coreNativeDamageFramesOperations.BeginDamage(attack.ResolutionId, source.Seat, target.Seat, amount, nature);
        var awaitingDying = false;
        var awaitingDamageTrigger = false;
        try
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageRequestedEvent(damageFrameId, source.Seat, target.Seat, amount, attack.EffectiveCardKind, nature) { SourceLess = attack.IsSourceLess });
            attack.CaptureDamageDistance(source.Seat == target.Seat ? 0 : _coreNativeRuleQueriesOperations.GetCombatDistance(source.Seat, target.Seat));
            if (amount > 0 && attack.PendingRedBladeDamageBonus)
            {
                _skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(source.Seat, "equipment:red-blood-blade", "farthest-damage", SkillUsageScope.Turn, 1);
                attack.PendingRedBladeDamageBonus = false;
            }

            var armorPayment = _coreDamageArmorPaymentsOperations.PrepareDamageArmorPayment(damageFrameId, attack);
            var healthBefore = target.Hp;
            _characterXuRongOlOperations.CaptureXuRongActualDamageHp(damageFrameId, attack, target.Hp, amount);
            if (armorPayment is not null)
            {
                target.Armor = armorPayment.ArmorAfter;
                target.Hp = armorPayment.HpAfter;
            }
            else
                target.Hp = checked(target.Hp - amount);
            if (qiaoHealthOwner is { } qiaoParent)
                _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(qiaoParent, target.Seat, HpChangeKind.Damage, healthBefore, target.Hp);
            attack.MarkDamageApplied();
            _coreCompletedUndamagedTargetRevealOperations.RecordCompletedUndamagedTargetDamage(attack, damageFrameId, amount);
            _characterMaXiuMaTieOlOperations.RecordKenshangDamage(attack, damageFrameId, amount);
            if (amount > 0)
                _coreRevealRecastProgramsOperations.RecordCompletedCardDamageParticipant(attack.ResolutionId, target.Seat);
            var useIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame is CardUseFrame use && use.Id == attack.ResolutionId);
            long? sourceCardUseResolutionId = null;
            if (useIndex >= 0 && _resolutionStackOperations._resolutionStack[useIndex] is CardUseFrame damageUse)
            {
                sourceCardUseResolutionId = damageUse.Id;
                _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[useIndex].Id, damageUse with { CausedDamage = true });
            }

            if (!attack.IsSourceLess && source.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play)
                _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess._playPhaseDamageDealtByCurrentPlayer += amount;
            if (!attack.IsSourceLess)
                _coreProgramPersistentCombatPoliciesOperations.AccumulateProgramPlayDamageHandLimit(source.Seat, amount);
            var natureLabel = CoreNativeCardClassificationsRuntime.GetDamageNatureLabel(nature);
            _coreMatchLogOperations.AddLog("Damage", attack.IsSourceLess ? $"{(target.Name)} 受到 {(amount)} 点无来源{(natureLabel)}伤害，剩余 {(target.Hp)} 点体力。" : $"{(target.Name)} 受到 {(source.Name)} 造成的 {(amount)} 点{(natureLabel)}伤害，剩余 {(target.Hp)} 点体力。", attack.IsSourceLess ? null : source.Seat, target.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageAppliedEvent(source.Seat, target.Seat, amount, target.Hp, nature) { SourceLess = attack.IsSourceLess, ArmorPayment = armorPayment, SourceCardUseResolutionId = sourceCardUseResolutionId });
            if (amount > 0 && _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(attack.ResolutionId, CurrentCardEnhancement.DrawAfterDamage))
            {
                var enhancementOwnerSeat = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Id == attack.ResolutionId).EnhancementOwnerSeat ?? source.Seat;
                var enhancementOwner = _coreMatchParticipantsPlayersAccess._players[enhancementOwnerSeat];
                if (enhancementOwner.IsAlive)
                    _corePhysicalDrawFacadesOperations.DrawCards(enhancementOwner, 1, log: true);
            }

            if (!attack.IsSourceLess)
                _characterLuMengMouOperations.ApplyHengyeGrowth(damageFrameId, source, target, amount);
            if (_coreProgramDamageHooksOperations.DamageHooks.TryBeginApplied(attack, damageFrameId))
            {
                awaitingDamageTrigger = true;
            }
            else if (_coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damageFrameId, SkillProgramTriggerWindow.DamageAppliedBeforeDying))
            {
                awaitingDamageTrigger = true;
            }
            else if (target.Hp <= 0)
            {
                _coreNativeDyingOperations.BeginDying(attack, damageFrameId, target, source);
                // Buqu may finish this dying continuation synchronously.
                awaitingDying = true;
            }
            else
            {
                awaitingDamageTrigger = _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damageFrameId, SkillProgramTriggerWindow.AfterDamageApplied);
            }

            if (!awaitingDying && !awaitingDamageTrigger)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AfterDamageEvent(damageFrameId, source.Seat, target.Seat, amount, target.Hp, nature) { SourceLess = attack.IsSourceLess });
            }
        }
        finally
        {
            if (!awaitingDying && !awaitingDamageTrigger)
            {
                _coreRuntimeOperations.PopResolutionFrame(damageFrameId, ResolutionFrameKind.Damage);
            }
        }

        return awaitingDying || awaitingDamageTrigger;
    }
}
