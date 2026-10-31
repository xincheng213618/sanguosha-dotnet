#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardUseRuntime
{
    internal long BeginCardUse(Card card, int sourceSeat, IReadOnlyList<int> targetSeats, CardKind? playedCardKind = null, bool ignoresArmor = false, IReadOnlyList<int>? physicalCardIds = null, CardConversionSource? conversionSource = null, IReadOnlyList<CardConversionSource>? additionalConversionSources = null, IReadOnlyList<int>? designatedTargetSeats = null, bool isTrueZhangbaSlash = false, ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = null, ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = null, ProgramDualColorDuelOrigin? dualColorDuelOrigin = null, ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = null, ShiQuanDuelOrigin? shiQuanDuelOrigin = null, ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = null, DrawFundedDistinctBasicPayment? drawFundedPayment = null, ChainedStateBasicPayment? chainedStatePayment = null, FriendRecastDuelOrigin? friendRecastDuel = null, ChosenAssaultReturn? chosenAssault = null, ZhenyingDuelOrigin? zhenyingDuel = null, HeJinMouzhuUseOrigin? heJinMouzhu = null, WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = null, TianChouShandaoUseOrigin? tianChouShandao = null, QinLangXianyingUseOrigin? qinLangXianying = null, AHuiNanJuemanUseOrigin? aHuiNanJueman = null, LiuYanLimuUseOrigin? liuYanLimu = null, JueshengDuelOrigin? jueshengDuel = null, QuHuangNishouUseOrigin? quHuangNishou = null, WeijiePaidOrigin? weijieOrigin = null, ZonghuPaidOrigin? zonghuOrigin = null, HuanpeiPaidOrigin? huanpeiOrigin = null)
    {
        var mouSmEventStart = checked(_coreCommandJournalStorageEventSequencePendingEventsAccess._eventSequence + _coreCommandJournalStorageEventSequencePendingEventsAccess._pendingEvents.Count + 1L);
        var mouSmLogStart = _coreGameLogLogSequenceAccess._logSequence;
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var adjustedTargets = _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats;
        _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = null;
        var moGangquanSource = conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        targetSeats = _characterLuBuOlMagicOperations.MoGangquanNativeTargets(sourceSeat, card, playedCardKind ?? card.Kind, moGangquanSource, targetSeats);
        if (CharacterLuBuOlMagicRuntime.IsMoGangquanSource(moGangquanSource))
            designatedTargetSeats = targetSeats;
        targetSeats = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(playedCardKind ?? card.Kind, sourceSeat, targetSeats);
        var targets = Array.AsReadOnly((adjustedTargets ?? targetSeats).ToArray());
        if (adjustedTargets is not null)
        {
            if (_coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._selectedRedAdditionalTargetSource is { } redSource)
            {
                _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._redAdditionalTargetGrants.Remove(sourceSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RedAdditionalTargetsConsumedEvent(sourceSeat, redSource, adjustedTargets));
                _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._selectedRedAdditionalTargetSource = null;
            }
        }

        var designated = (Array.AsReadOnly((designatedTargetSeats ?? targets).Where(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Distinct().ToArray()));
        var effectiveCardKind = playedCardKind ?? card.Kind;
        var physicalIds = Array.AsReadOnly((physicalCardIds ?? [card.Id]).ToArray());
        var actionContext = _coreCardActionsOperations.CaptureCardUseAction(card, sourceSeat, targets, effectiveCardKind, physicalIds, conversionSource, additionalConversionSources, designated, isTrueZhangbaSlash);
        if (actionContext is not null)
            actionContext = _coreChainedStateBasicReturnsOperations.PrepareChainedStateBasicAction(_coreDrawFundedDistinctBasicUsesOperations.PrepareDrawFundedDistinctBasicAction(actionContext, drawFundedPayment), chainedStatePayment);
        if (aHuiNanJueman is not null || qinLangXianying is not null || tianChouShandao is not null || wuAnGuoLiyong is not null || heJinMouzhu is not null || zhenyingDuel is not null || chosenAssault is not null || selectedActorDuelOrigin is not null || damageTargetDuelOrigin is not null || dualColorDuelOrigin is not null || conditionalDiscardDuelOrigin is not null || shiQuanDuelOrigin is not null || virtualOrdinaryTrickOrigin is not null || friendRecastDuel is not null)
            actionContext = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(actionContext!.ActionId, actionContext.ParentActionId, CardActionType.Use, sourceSeat, sourceSeat, null, null, null, aHuiNanJueman is null && qinLangXianying is null && tianChouShandao is null && virtualOrdinaryTrickOrigin is null && chosenAssault is null && heJinMouzhu is null ? CardKind.Duel : effectiveCardKind, targets, [], [], designated, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: virtualOrdinaryTrickOrigin?.SkillId == "shanhe:zhaogu-mirror-6018" ? null : false));
        if (actionContext is not null)
            actionContext = _characterChengYuMouOlOperations.PrepareMouDanchiAction(actionContext);
        if (actionContext is not null)
            actionContext = _characterDengAiAnniversaryGodOperations.PrepareShenDaCuixinAction(actionContext);
        if (actionContext is not null)
            actionContext = _characterHuanShuJunOlOperations.PrepareHuanpeiAction(_characterLiuZhangOlOperations.PrepareZonghuAction(_characterGuoTuOlOperations.PrepareWeijieAction(actionContext, weijieOrigin), zonghuOrigin), huanpeiOrigin);
        if (actionContext is not null)
            actionContext = _characterZhangXiuMouOperations.PrepareMouZhangXiuAnniversarySlashAction(actionContext);
        if (actionContext is not null)
            actionContext = _characterHuLieMouOperations.PrepareMouHuLieSlashAction(actionContext);
        if (actionContext is not null)
            actionContext = _characterHuCheErMouOperations.PrepareMouHuCheErSlashAction(_characterLuKangWumiaoOperations.PrepareWuLuKangKegouSlashAction(_characterTaoQianOlOperations.PrepareZongluanSlashAction(_characterGuanHaiOlOperations.PrepareYouqueSlashAction(_characterGuanHaiOlOperations.PrepareXieweiAction(actionContext)))));
        if (actionContext is not null)
            actionContext = _characterWenChouMouOlOperations.PrepareMouWenChouAction(_characterXuHuangMouMobileOperations.PrepareMouXuHuangAction(_characterLiuChengMouMobileOperations.PrepareMouLiuChengAction(_characterXiaoQiaoMouOlOperations.PrepareMouXqJueyanAction(_characterDiaoChanLeOperations.PrepareDiouAction(actionContext)))));
        if (actionContext is not null)
            actionContext = _characterDongChengMouOperations.PrepareMouDcAction(actionContext);
        var colorFireAttack = _coreColorFireAttacksOperations.FreezeColorFireAttackPolicy(sourceSeat, actionContext);
        var firstUseDistance = actionContext is not null && _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(_coreMatchParticipantsPlayersAccess._players[sourceSeat]);
        if (actionContext is not null)
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(actionContext);
        _coreFriendEventsOperations.CaptureFriendMinimumHandUse(actionContext);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(sourceSeat, effectiveCardKind);
        var declarationPayment = _coreCardDeclarationsOperations.TransferDeclarationToCardUse(resolutionId);
        var turnRedSlashBenefit = actionContext is { Type: CardActionType.Use, EffectiveIsRed: true } && _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashPolicyForColor(sourceSeat, effectiveCardKind, actionContext.EffectiveIsRed);
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, sourceSeat, card.Id, effectiveCardKind, targets, IgnoresArmor: ignoresArmor, PhysicalCardIds: physicalIds) { ZonghuOrigin = zonghuOrigin, QuHuangNishou = quHuangNishou, JueshengDuel = jueshengDuel, LiuYanLimu = liuYanLimu, AHuiNanJueman = aHuiNanJueman, QinLangXianying = qinLangXianying, TianChouShandao = tianChouShandao, WuAnGuoLiyong = wuAnGuoLiyong, HeJinMouzhu = heJinMouzhu, ZhenyingDuel = zhenyingDuel, ChosenAssault = chosenAssault, FriendRecastDuel = friendRecastDuel, SelectedActorDuelOrigin = selectedActorDuelOrigin, VirtualOrdinaryTrickOrigin = virtualOrdinaryTrickOrigin is null ? null : virtualOrdinaryTrickOrigin with { CardActionId = actionContext!.ActionId }, DamageTargetDuelOrigin = damageTargetDuelOrigin, DualColorDuelOrigin = dualColorDuelOrigin, ConditionalDiscardDuelOrigin = conditionalDiscardDuelOrigin, ShiQuanDuelOrigin = shiQuanDuelOrigin, Enhancements = turnRedSlashBenefit ? CurrentCardEnhancement.Uncancelable : CurrentCardEnhancement.None, TurnRedSlashSkipsDodge = turnRedSlashBenefit, AcceptedDeclarationPayment = declarationPayment, Action = actionContext, TargetsAdjusted = adjustedTargets is not null, FirstOwnPlayUseDistanceUnlimited = firstUseDistance, GuanFirstRedDistance = _characterGuanYuRuntimeOperations.GuanFirstRedAvailable(_coreMatchParticipantsPlayersAccess._players[sourceSeat], actionContext?.EffectiveSuit), ColorFireAttack = colorFireAttack });
        _characterLiuFengBoundaryOperations.BindBoundaryLiuFengNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterSimaYiMouOperations.BeginMouSmSecretUse(resolutionId, mouSmEventStart, mouSmLogStart);
        _characterSunQuanOlMagicOperations.CaptureMoQuanyuQuota(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterCaiWenJiLeOperations.IssueBeifenUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuangGaiMouMobileOperations.IssueMouZhaJiangUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterZhouYuOlMouOperations.BindMouZuifengAlcoholUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterXiaoQiaoLeOperations.IssueWeiwanUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterZhouYuOlMouOperations.IssueMouGuquUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterDiaoChanLeOperations.IssueDiouUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterGuanYuWumiaoOperations.IssueWuyouUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterMiHengLeOperations.IssueSiruiUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterGuanYuWumiaoOperations.IssueJuewuUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterGuoTuOlOperations.IssueWeijieUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!, weijieOrigin);
        _characterLiuZhangOlOperations.IssueZonghuUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!, zonghuOrigin);
        _characterHuanShuJunOlOperations.IssueHuanpeiUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!, huanpeiOrigin);
        _characterHuanShuJunOlOperations.BindWenyiNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterKongShuOlOperations.BindLeiluanNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterGuanHaiOlOperations.BindYouqueSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterLuKangWumiaoOperations.BindWuLuKangKegouSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuCheErMouOperations.BindMouHuCheErSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterZhangXiuMouOperations.BindMouZhangXiuAnniversarySlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterDongChengMouOperations.BindMouDcUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterChenLinMouOperations.BindMouChenLinNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _coreSourceDingxiNativeOperations.BindSourceDingxiNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuLieMouOperations.BindMouHuLieSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterTaoQianOlOperations.BindZongluanSlashUse(resolutionId);
        _characterGuanHaiOlOperations.PrepareXieweiNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _coreRoundOnceCardlessProgramRequestsOperations.BindRoundOnceCardlessProgramRequestUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        if (colorFireAttack is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ColorFireAttackPolicyIssuedEvent(resolutionId, colorFireAttack.ActionId, colorFireAttack.Source));
        _coreTieredRoundZeroUsesOperations.IssueTieredRoundConversionUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _coreNoDamageSkillDisableOperations.Value.IssueNoDamageSkillDisablePolicy(resolutionId);
        _coreDrawFundedDistinctBasicUsesOperations.IssueDrawFundedDistinctBasicUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!, drawFundedPayment);
        _coreChainedStateBasicReturnsOperations.IssueChainedStateBasicUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!, chainedStatePayment);
        _coreProvenanceCardPoliciesOperations.IssueProvenanceUsePolicy(resolutionId, actionContext);
        _coreNonBasicSlashProfileOperations.IssueNonBasicSlashPolicy(resolutionId, actionContext);
        _coreRoundGainedSourceUsesOperations.IssueRoundGainedUseQualifications(resolutionId, actionContext);
        _coreShownEntityTurnPoliciesOperations.IssueShownEntityUseBenefits(resolutionId, actionContext);
        _coreGiftedSlashUseRewardsOperations.Value.IssueGiftedSlashUseBenefit(resolutionId, actionContext);
        _coreGrantedEntityPhasesOperations.IssueGrantedPhaseEntityUseDistance(resolutionId, actionContext);
        _characterWangGuanOlOperations.IssueMiuyanUse(resolutionId, actionContext);
        _characterMaXiuMaTieOlOperations.IssueKenshangUse(resolutionId, actionContext!);
        _characterPeiYuanShaoOlOperations.IssueFulueCounterUse(resolutionId, actionContext!);
        _characterWuJingOlOperations.BindWuJingHejiUse(resolutionId);
        _characterLiYiOlOperations.BindChanshuangSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuangYueyingOlMouOperations.BindMouLixianNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterSunYuanOlOperations.BindSayingSlashUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterSunYuanOlOperations.BindDongxinEntityUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterYangFuOlOperations.BindSuyiEquipmentUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuBanOlOperations.IssueHuiyunUse(resolutionId, actionContext);
        _characterHuBanOlOperations.BindHuiyunFollowupUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, card.Id, effectiveCardKind, sourceSeat, IgnoresArmor: ignoresArmor));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory && actionContext is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(actionContext));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, targets));
        _characterLuMengMouOperations.RecordYingboCardUse(resolutionId, sourceSeat, effectiveCardKind);
        _characterWangYunOlOperations.BindWangYunNativeUse(resolutionId);
        _characterZhangJiOlOperations.Value.BindZhangJiNativeUse(resolutionId);
        _characterGuoSiOlOperations.BindGuoSiAcceptedUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterHuangQuanOlOperations.BindJianjiNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterZhouYuMouOperations.BindMouZyNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterJiangJiMouOperations.BindMouJjNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterCaoAngMouOperations.BindMouCaNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterJuShouMouOperations.BindMouJsNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterJiaXuMouOperations.BindMouJxNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterCaoChunOlOperations.BindCaoChunNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterLvKuangLvXiangOlOperations.BindQigongNativeUse(resolutionId);
        _characterZhangYanOlOperations.BindZhangYanSujiUse(resolutionId);
        _characterFuRongOlOperations.BindFuRongNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterYaDanOlOperations.BindQingyaNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterDuanJiongOlOperations.BindDuanJiongSaoguNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        _characterKeBiNengOlOperations.BindKeBiNengPingduanNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)!);
        return resolutionId;
    }
}
