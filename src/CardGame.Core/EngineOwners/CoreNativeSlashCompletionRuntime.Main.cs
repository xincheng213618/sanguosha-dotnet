#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeSlashCompletionRuntime
{
    internal bool FinishAttack(NativeOwners.CardAttackHandle attack, bool allowYingboGift = true)
    {
        if (_coreProgramDamageHooksOperations.DamageHooks.TryComplete(ProgramDamagePhase.AttackCompletionPrelude, attack, out var preludePaused))
            return preludePaused;
        if (attack.IsProgramSkillDamage)
        {
            if (attack.Card is not null || attack.ProgramSkillFrameId != attack.ResolutionId)
                throw new InvalidOperationException("Configured active-program damage must retain its program frame and no physical card.");
            return false;
        }

        if (attack.IsProgramJudgmentDamage)
        {
            if (attack.Card is not null || attack.ProgramJudgmentFrameId is null)
            {
                throw new InvalidOperationException("Configured judgment damage must retain its program frame and no physical card.");
            }

            return false;
        }

        if (_coreProgramDamageHooksOperations.DamageHooks.TryComplete(ProgramDamagePhase.IssuedAttackCompletion, attack, out var issuedPaused))
            return issuedPaused;
        if (attack.Card is null && attack.EffectiveCardKind == CardKind.Duel && _coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(attack.ResolutionId))
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
            _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, CardKind.Duel));
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
                return true;
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
            return false;
        }

        if (attack.Card is null && (attack.EffectiveCardKind == CardKind.Slash || _coreNativeSlashTargetDuelOperations.IsNativeSlashTargetDuelAttack(attack) || _coreShanheS5XuncanOperations.IsShanheS5YunshenUse(attack.ResolutionId) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } fireUse && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(fireUse) || CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.VirtualBasicReturn is not null) && (attack.ProgramSkillCardUseFrameId is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.VirtualBasicReturn is not null || _coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(attack.ResolutionId)))
        {
            var virtualUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId);
            virtualUse = _coreActualHandGainProgramsOperations.NormalizeCompletedCategoryVirtualUse(virtualUse, attack.ProgramSkillCardUseFrameId);
            virtualUse = _coreActualTurnLegacyCompletionOperations.NormalizeActualTurnLegacyCompletedUse(virtualUse, attack.ProgramSkillCardUseFrameId);
            if (virtualUse.Action is { } virtualAction)
            {
                if (attack.ProgramSkillCardUseFrameId is { } programParentId && virtualUse.SharedSlashBenefit is null && virtualUse.ForeignTurnContestSlashReturn is null && virtualUse.PindianWinnerSlashReturn is null && virtualUse.InspectedHandSlashReturn is null && virtualUse.KuangfuSlashReturn is null && virtualUse.HeJinMouzhu is null && virtualUse.WuAnGuoLiyong is null && !_characterDianWeiMouOperations.IsMouDianWeiSlashUse(virtualUse.Id) && !_characterWuJingOlOperations.IsWuJingHejiZeroUse(virtualUse) && !_characterMengDaOlOperations.IsGoudeSlashUse(virtualUse.Id) && !_characterYaDanOlOperations.IsIssuedQingyaUse(virtualUse.Id) && !_characterYangFuOlOperations.IsSuyiSlashUse(virtualUse.Id) && !_characterLiYiOlOperations.HasIssuedChanshuangSlash(virtualUse.Id, virtualUse.SourceSeat) && !_characterWangChangAnniversaryOperations.Value.IsWangChangSlash(virtualUse) && !_coreShanheS5XuncanOperations.IsShanheS5YunshenUse(virtualUse.Id))
                {
                    var parentIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == programParentId);
                    if (parentIndex < 0 || _resolutionStackOperations._resolutionStack[parentIndex] is not ProgramSkillFrame parent)
                        throw new InvalidOperationException("The virtual Slash lost its selected-target program parent.");
                    if (parent.YangHong is null && parent.GaoXiang is null)
                    {
                        IReadOnlyList<int> primaryTarget = virtualUse.TargetSeats;
                        if (virtualUse.TargetSeats.Count != 1 && !_coreOriginalTargetAdditionsOperations.TryGetOriginalTargetVirtualSlashReturn(virtualUse, parent, out primaryTarget))
                            throw new InvalidOperationException("The virtual Slash lost its selected-target program parent.");
                        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { SelectedTargetSeats = primaryTarget });
                    }
                }

                _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.Completed);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(attack.ResolutionId, 0, virtualUse.CardKind));
                if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, virtualAction, SkillProgramTriggerWindow.CardUseCompleted, virtualUse.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
                    return true;
                _coreNativeCardUseCompletionOperations.PopFinishedCardUse(attack.ResolutionId);
                _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
                return false;
            }

            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.Completed);
            _coreRuntimeOperations.PopResolutionFrame(attack.ResolutionId, ResolutionFrameKind.CardUse);
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                if (_coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
                    _kernelCoreMainOperations.CompleteGame();
            }

            return false;
        }

        if (_coreOriginalUseLightningOperations.FinishOriginalUseLightningAttack(attack))
            return false;
        var attackCard = CoreNativeDamagePoliciesRuntime.RequireAttackCard(attack);
        var cardLocation = _cardZonesOperations._cardZones.GetLocation(attackCard.Id);
        if (attack.IsDelayedJudgmentDamage)
        {
            if (cardLocation == CardLocation.Judgment(attack.DelayedJudgmentSeat ?? attack.SourceSeat))
            {
                _corePhysicalMovementFacadesOperations.MoveCard(attackCard, cardLocation, CardLocation.DiscardPile, CardMoveReasons.DelayedCardFinish);
            }
            else if (cardLocation != CardLocation.DiscardPile && !_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial2(attackCard.Id, CardLocation.Judgment(attack.DelayedJudgmentSeat ?? attack.SourceSeat), cardLocation) && !_characterXuRongOlOperations.IsXuRongCausalEntityClaim(attack.ResolutionId, attackCard.Id) && !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(movement => movement.CardId == attackCard.Id && movement.From == CardLocation.Judgment(attack.DelayedJudgmentSeat ?? attack.SourceSeat) && movement.To == CardLocation.DiscardPile && movement.Reason == CardMoveReasons.DeathDiscard))
            {
                throw new InvalidOperationException($"A resolved Lightning left its judgment zone through an unsupported destination: {(cardLocation)}.");
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
            {
                _kernelCoreMainOperations.CompleteGame();
            }

            return false;
        }

        if (allowYingboGift && _characterLuMengMouOperations.TryBeginYingboGift(attack.ResolutionId, attack.CardUserSeat, attackCard, CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack), attack.PhysicalCards, YingboGiftContinuation.Attack, attack: attack))
        {
            return true;
        }

        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } materialUse)
            _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(materialUse);
        foreach (var physicalCard in attack.PhysicalCards)
        {
            if (_characterXinPiOlOperations.IsXinPiPhysicalClaim(attack.ResolutionId, physicalCard.Id) || _characterHuaManOlOperations.IsHuaManClaimedEntity(attack.ResolutionId, physicalCard.Id) || _coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(attack.ResolutionId, physicalCard.Id) || _characterXuRongOlOperations.IsXuRongCausalEntityClaim(attack.ResolutionId, physicalCard.Id))
                continue;
            if ((_coreHellSoulCardsOperations.IsHellDestroyedSoulMaterial(attack.ResolutionId, physicalCard.Id) || (_characterHuBanOlOperations.IsHuiyunOutsideGeneralWeaponMaterial(attack.ResolutionId, physicalCard.Id) || _characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(attack.ResolutionId, physicalCard.Id)) || (_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(attack.ResolutionId, physicalCard.Id) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(attack.ResolutionId, physicalCard.Id))) || (_coreForeignSelectedCardSlashReturnsOperations.IsSelectedForeignCardSlashRemovedMaterial(attack.ResolutionId, physicalCard.Id) || _characterZhangYanOlOperations.IsZhangYanSujiRemovedMaterial(attack.ResolutionId, physicalCard.Id) || ((_coreKenshangPaymentsOperations.IsKenshangRemovedMaterial(attack.ResolutionId, physicalCard.Id) || _characterGuanHaiOlOperations.IsXieweiRemovedMaterial(attack.ResolutionId, physicalCard.Id)) || (_characterWuJingOlOperations.IsWuJingHejiRemovedMaterial(attack.ResolutionId, physicalCard.Id) || _characterSunYuanOlOperations.IsSayingRemovedMaterial(attack.ResolutionId, physicalCard.Id)))))
                continue;
            var physicalLocation = _cardZonesOperations._cardZones.GetLocation(physicalCard.Id);
            if (physicalLocation == CardLocation.Processing)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(physicalCard, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
            }
            else if (physicalLocation.Zone is not (CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile))
            {
                throw new InvalidOperationException($"A resolved {(attack.EffectiveCardKind)} physical card left Processing through an unsupported destination: {(physicalLocation)}.");
            }
        }

        var completedAction = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).Action;
        _coreNativeCardUseCompletionOperations.QueueCardUseFinishedWithoutPop(attack.ResolutionId, attackCard, CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && completedAction is not null && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, completedAction, SkillProgramTriggerWindow.CardUseCompleted, completedAction.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            return true;
        _coreNativeCardUseCompletionOperations.PopFinishedCardUse(attack.ResolutionId);
        _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        return false;
    }
}
