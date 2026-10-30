using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

Console.OutputEncoding = new System.Text.UTF8Encoding(false);

if (args.Contains("--filter", StringComparer.OrdinalIgnoreCase))
{
    Console.Error.WriteLine("Use --filter=<name>; an incomplete filter must not start the full suite.");
    return 2;
}

if (ProgramToolCommands.TryRun(args, out var programToolExitCode))
    return programToolExitCode;

if (args.FirstOrDefault() == "--ai-inspect")
{
    TacticalAiSimulation.Inspect(int.Parse(args[1]), int.Parse(args[2]), args[3]);
    return 0;
}

if (args.FirstOrDefault() == "--ai-batch")
{
    TacticalAiSimulation.Run(args[1]);
    return 0;
}

if (args.FirstOrDefault() == "--ai-duel")
{
    if (args.Length is < 2 or > 8 || args.Skip(2).Take(4).Any(value => !int.TryParse(value, out _)))
    {
        Console.Error.WriteLine("Usage: --ai-duel <output.json> [pairs] [baseline-policy] [candidate-policy] [first-pair] [mixed|wushuang-smoke|wushuang] [wushuang-pool-snapshot.json]");
        return 2;
    }
    try
    {
        return TacticalAiDuelEvaluation.Run(args[1],
            args.Length > 2 ? int.Parse(args[2]) : 12,
            args.Length > 3 ? int.Parse(args[3]) : args.Length > 6 && args[6].StartsWith("wushuang", StringComparison.OrdinalIgnoreCase)
                ? AiDecisionPolicy.RebelCoordinationVersion : AiDecisionPolicy.CooperativeCategoryGiftVersion,
            args.Length > 4 ? int.Parse(args[4]) : null,
            args.Length > 5 ? int.Parse(args[5]) : 0,
            args.Length > 6 ? args[6] : "mixed", args.Length > 7 ? args[7] : null);
    }
    catch (ArgumentException exception)
    {
        Console.Error.WriteLine(exception.Message);
        return 2;
    }
}

if (args.FirstOrDefault() == "--ai-identity")
{
    if (args.Length is < 2 or > 9 || args.Skip(2).Take(4).Any(value => !int.TryParse(value, out _)) ||
        args.Length > 7 && !args[7].Equals("all", StringComparison.OrdinalIgnoreCase) && !int.TryParse(args[7], out _))
    {
        Console.Error.WriteLine("Usage: --ai-identity <output.json> [seeds] [baseline-policy] [candidate-policy] [first-seed] [standard|active|classic] [5|8|all] [all|control|LordAndLoyalists|Rebels|Renegade]");
        return 2;
    }
    try
    {
        return IdentityAiEvaluation.Run(args[1],
            args.Length > 2 ? int.Parse(args[2]) : 12,
            args.Length > 3 ? int.Parse(args[3]) : AiDecisionPolicy.TacticalPlanningVersion,
            args.Length > 4 ? int.Parse(args[4]) : null,
            args.Length > 5 ? int.Parse(args[5]) : 0,
            args.Length > 6 ? args[6] : "standard",
            args.Length > 7 && !args[7].Equals("all", StringComparison.OrdinalIgnoreCase) ? int.Parse(args[7]) : null,
            args.Length > 8 ? args[8] : null);
    }
    catch (ArgumentException exception)
    {
        Console.Error.WriteLine(exception.Message);
        return 2;
    }
}

var unknownArguments = args.Where(argument =>
    !argument.StartsWith("--filter=", StringComparison.OrdinalIgnoreCase) &&
    !argument.Equals("--verbose", StringComparison.OrdinalIgnoreCase)).ToArray();
if (unknownArguments.Length > 0)
{
    Console.Error.WriteLine("Unknown Core check argument; use one or more --filter=<name> and optional --verbose.");
    return 2;
}

// Existing card-effect fixtures retain automatic discards to isolate their scenarios.
// Default/manual discard validation and complete-match coverage live in ManualDiscardChecks.
var tests = new (string Name, Action Body)[]
{
    ("Wushuang balance: opening quality and mode exclusions preserve native rules cold", WushuangModeChecks.OpeningQualityAndBalanceExclusionsPreserveNativeRulesCold),
    ("Feng Lin Kuai AI: private Shenshi observation and exact gift cold", FengLinKuaiChecks.ShenshiAiRoutesPrivateObservationAndExactGiftCold),
    ("Duan Jiong: purchased equipment keeps exact discarded payment and child cold", WushuangModeChecks.SaoguPurchasedEquipmentKeepsOriginalDiscardAndPaidChildCold),
    ("Wushuang mode: Yinbing mixed storage exact destroyed equipment cold", WushuangModeChecks.YinbingMixedStorageKeepsDestroyedShopPaymentCold),
    ("Zhiyan: moved draw does not reveal equip or charge former recipient cold", ZhiyanAvailableDrawChecks.DrawMovedByRealChildDoesNotRevealEquipOrChargeItsFormerRecipientCold),
    ("Bound filter: available-source opt-in retains strict default", ZhiyanAvailableDrawChecks.FilterOptInPreservesStrictDefaultAndRejectsInvalidFlag),
    ("Wushuang AI refinement: draw before spending ordinary Slash", WushuangAiDrawPreparationChecks.DrawBeforeSpendingTheOnlyNonLethalSlashWithoutGuessingNewCards),
    ("Wushuang AI refinement: draw keeps lethal wine equipment and reserve", WushuangAiDrawPreparationChecks.DrawPreparationPreservesPublicLethalWineAndEquipmentLethal),
    ("Wushuang AI refinement: deferred shop debt and reliable slots", WushuangAiShopChecks.DeferredPhaseShopPricesActualRecipientDebtAndKeepsReliableSlots),
    ("Wushuang AI refinement: public deferred targets and native payment cold", WushuangModeChecks.DeferredPhaseHintsDriveActualTargetsAndNativePaymentCold),
    ("Shi Tai Shi Ci: Zhenfeng recovery returns through actual health child cold", ShiTaiShiCiChecks.ZhenfengRecoveryPaysOnceAndReturnsAfterHealthChildCold),
    ("Fu Rong terminal: lethal use and real Leiji retaliation finish paid ancestor cold", FuRongTerminalReturnChecks.LethalUseAndRealLeijiRetaliationFinishPaidAncestorCold),
    ("Attributed markers: acquired Qiangxi source-less payment preserves owner rewards and sourced hit cold", SourceLessAttributedMarkerChecks.AcquiredQiangxiSourceLessPaymentSkipsSourceMarkersButPreservesOwnerRewardsAndHitCold),
    ("Shi Zhou Yu: Chiyun fire returns to original paid frame cold", ZhouYuChecks.ChiyunFireOptionReturnsToOriginalPaidFrameAfterCold),
    ("Wushuang mode: Qixi exact destroyed shop material finish cold", WushuangModeChecks.PurchasedBlackEquipmentQixiFinishesItsExactDestroyedPaymentCold),
    ("Wushuang AI loss review: judgment beneficiaries, outcomes and payment", WushuangAiShopChecks.LossReviewJudgmentTracksBeneficiarySingleOutcomeAndPayment),
    ("Wushuang AI loss review: choice controller, eligibility and deferred debt", WushuangAiShopChecks.LossReviewChoicesUseControllerEligibilityAndNetDeferredDebt),
    ("Wushuang AI loss review: singleton chain and current acquired synergy", WushuangAiLossReviewBattleChecks.CurrentDuelRecastsEmptyChainButPreservesOwnedSynergyAndLegacyThoughts),
    ("Wushuang AI loss review: independent Guding lethal preparation", WushuangAiLossReviewBattleChecks.CurrentDuelPreparesIndependentGudingLethalAndRejectsUnsupportedPlans),
    ("Wushuang mode: foreign Zuoxing native fixed seven maximum payment cold", WushuangModeChecks.HappyZuoxingForeignRecipientPaysNativeFixedSevenMaximumCold),
    ("Mobile Shen Jiang Wei: Shenpei exact nested dying and original entry cold", MobileShenJiangWeiChecks.ShenpeiThunderOwnsNestedDyingAndReturnsOriginalEntryCold),
    ("Shen Cao Pi: Yinzhi Spear complete material pair cold", CardGame.Core.Tests.ShenCaoPiChecks.YinzhiZhangbaUsesOnlyPublishedCompleteNewMaterialPairCold),
    ("Wushuang mode: max HP base yields to real Jigong and expires cold", WushuangModeChecks.MaxHpBaseYieldsToRealJigongTurnDamageAndExpiresCold),
    ("Wushuang mode: Xianniang real shop destruction receipt cold", WushuangModeChecks.XianniangCountsOnlyReceivedCardsWhenNativeShopEquipmentIsDestroyedCold),
    ("Wushuang mode: Suishi exact destroyed shop material return cold", WushuangModeChecks.SuishiReturnsItsExactDestroyedNativeShopMaterialCold),

    ("Wushuang mode: Suji exact native shop equipment payment cold", WushuangModeChecks.SujiPaysExactNativeShopEquipmentAndReturnsOnceCold),
    ("Mobile Shen Cao Cao: Guixin exact phantom gain and destruction tail cold", MobileShenCaoCaoChecks.GuixinKeepsRealForeignPhantomGainAndExactDestructionTailCold),
    ("Yang Fu: lethal native Slash completes exact paid ancestor cold", YangFuChecks.SuyiLethalNativeSlashCompletesExactPaidAncestorCold),
    ("Wushuang mode: Huomo Dodge exact destroyed equipment response cold", WushuangModeChecks.HuomoDodgeRetainsExactDestroyedEquipmentResponsePaymentCold),
    ("Wushuang mode: Jiqiao exact destroyed shop equipment cold", WushuangModeChecks.JiqiaoCountsExactDestroyedShopEquipmentAndCompletesRevealCold),
    ("Wushuang mode: selected equipment return exact destruction cold", WushuangModeChecks.SelectedEquipmentReturnStopsAfterExactNativeDestructionCold),
    ("Wushuang skills A: Jinzhi response excludes Funan discard cost cold", WushuangOfficialSkillsAChecks.JinzhiVirtualResponseCannotBeTakenAsFunanDiscardMaterialCold),
    ("Ordinary Xun Chen: Fenglve current pindian gift pending and cold", OrdinaryXunChenChecks.FenglveGiftsItsExactCurrentPindianCardPendingAndCold),
    ("Wushuang skills B: Dingan exact second Slash nested damage cold", WushuangSkillsBChecks.DinganDamagesDuringExactSecondSlashAndReturnsCold),
    ("Anniversary Yan Xiang: exhausted true use quota filters ending plans cold", AnniversaryYanXiangChecks.EndingNativePlansRespectExhaustedTrueUseQuotaCold),
    ("Zhang Yan: converted counterspell returns before independent Suji cold", ZhangYanChecks.ConvertedCounterspellReturnsBeforeIndependentSujiUseCold),
    ("Happy Shen Da Xiao Qiao: undefined Wushuang HP and scalar limit cold", CardGame.Core.Tests.HappyShenDaXiaoQiaoChecks.WushuangFactsPreserveUndefinedSisterHpAndRejectScalarLimitCold),

    ("Shi Guo Yuan: frozen comparison permanent limit and latest record cold", ShiGuoYuanChecks.FrozenComparisonPersistentLimitAndLatestRecordCold),
    ("Shi Guo Yuan: completed damage use and original hand no distance cold", ShiGuoYuanChecks.CompletedDamageUseOwnDamageFactAndOriginalHandNoDistanceCold),
    ("Shi Guo Yuan: ordered repeated source discard children and skill loss cold", ShiGuoYuanChecks.OrderedRepeatedSourceDiscardPaidChildrenAndSuccessfulSkillLossCold),
    ("Shi Xin Xian Ying: own private view atomic discard suit quota and self Qingshi cold", ShiXinXianYingChecks.OwnPrivateViewAtomicDiscardSuitQuotaAndSelfQingshiCold),
    ("Shi Xin Xian Ying: shared activator private view missing suit and actual camp discard cold", ShiXinXianYingChecks.SharedActivatorPrivateViewRealMissingSuitAndActualCampDiscardCold),
    ("Shi Huan Jie: atomic exchange paid source loss derived skills and turn expiry cold", ShiHuanJieChecks.AtomicHandExchangePaidSourceLossDerivedSkillsAndActualTurnExpiryCold),
    ("Shi Huan Jie: limited all hand discard shuffle ordered four draws cold", ShiHuanJieChecks.LimitedAllHandDiscardRealShuffleOrderedFourDrawsAndPaidRecoveryCold),
    ("Shi Zhao Yun: lifetime hand names, shared round zero quota, unlimited basics and fixed-four cold recovery", ShiZhaoYunChecks.LifetimeHandNamesCardlessRoundQuotaUnlimitedBasicsAndFixedFourCold),
    ("Shi Zhao Yun: actual-turn played Slashes, bidirectional native Duel and round limit", ShiZhaoYunChecks.ActualTurnAllPlayersPlayedSlashesDynamicBidirectionalDuelAndRoundLimit),
    ("Shi Yu Ji: Fuji gift marks empowered first Slash and marked Dodge response", MobileShiYuJiChecks.FujiGiftMarksEmpoweredFirstSlashAndMarkedDodge),
    ("Shi Yu Ji: Fuji marked Slash Duel response and empower expiry", MobileShiYuJiChecks.FujiMarkedSlashDuelResponseAndEmpowerExpiry),
    ("Shi Yu Ji: Daozhuan own-turn exhaust blocks responses until round restore", MobileShiYuJiChecks.DaozhuanOwnTurnExhaustBlocksResponsesUntilRoundRestore),
    ("Shi Yu Ji: Daozhuan round name ledger and per-turn total off-turn", MobileShiYuJiChecks.DaozhuanRoundNameLedgerAndPerTurnTotalOffTurn),
    ("Shi Cheng Pu: Duzuo grants fire Slash from piles and skips its own grants", ShiChengPuChecks.DuzuoGrantsFireSlashFromPilesAndExcludesOwnGrants),
    ("Shi Cheng Pu: Bihan discards hand down to hp and reduces Slash damage", ShiChengPuChecks.BihanDiscardsToHpAndReducesSlashDamage),
    ("Shi Lu Yusheng: Runwei color grant debt refund same-turn exclusion and phase-end cold", ShiLuYuShengChecks.RunweiGrantsColorDebtRefundAndExcludesGainedTargets),
    ("Shi Lu Yusheng: Shuanghuai prevents damage and settles same-different memory", ShiLuYuShengChecks.ShuanghuaiPreventsDamageAndSettlesSameDifferentMemory),
    ("Shi Lu Su: real foreign hand use two refills conversion and next-turn expiry cold", ShiLuSuChecks.ActualForeignHandUseTwoRefillsAndNextTurnExpiryCold),
    ("Shi Lu Su: foreign Dodge preserves actor provider and paid response cold", ShiLuSuChecks.ActualForeignHandDodgeResponsePreservesActorProviderCold),
    ("Shi Lu Su: self exchange multi discard and paid child cold", ShiLuSuChecks.DimengSelfExchangeMultiDiscardAndPaidChildCold),
    ("Shi He Qi: field entries next-mark reward native Duel target quota and paid draw cold", ShiHeQiChecks.FieldEntriesNextMarkRewardNativeDuelTargetQuotaAndPaidDrawCold),
    ("Shi He Qi: issued global Yingji cleanup uses physical move after source loss cold", ShiHeQiChecks.IssuedGlobalYingjiCleanupUsesPhysicalMoveAfterSourceLossCold),
    ("Shi Sun Chen: different suits private simultaneous gifts and damage quota cold", ShiSunChenChecks.DistinctCostPrivateCommitmentsNativeGiftsAndDamageQuotaCold),
    ("Shi Sun Chen: completed category chains native draw and forced fire cold", ShiSunChenChecks.CompletedCurrentCategoryChainsDrawAndForcedMomentumPaidChildCold),
    ("Shi Cao Zhen: ordered real forecasts color hand discard and obtain cold", ShiCaoZhenChecks.NativeOrderedForecastColorHandDiscardAndObtainCold),
    ("Shi Cao Zhen: issued end after source loss recipients and damage cold", ShiCaoZhenChecks.IssuedEndAfterLossThreeRecipientsAndRemoveDamageCold),
    ("Shi Sun Jun: frozen suit cost other damage and actual phase cap cold", ShiSunJunChecks.XiongtuFrozenSuitCostOtherDamageAndPhaseCapCold),
    ("Shi Sun Jun: first suit Slash and Alcohol preserve ordinary quota", ShiSunJunChecks.XianshuaiFirstSuitSlashAndAlcoholKeepOrdinaryQuota),
    ("Shi Xiao Qiao: exact copy source preserves independent instance and state", ShiXiaoQiaoChecks.ExactCopySourcePreservesIndependentInstanceAndState),
    ("Shi Xiao Qiao: current acquired limited skill and self loss native draw cold", ShiXiaoQiaoChecks.CurrentAcquiredLimitedSkillAndSelfLossDrawCold),
    ("Shi Zhang Yan: sealed path actual discards prevention recovery and exact obtain cold", ShiZhangYanChecks.FeijingSealedPathOrderedDiscardsPreventRecoverAndExactObtainCold),
    ("Shi Zhang Yan: completed sole Slash native Duel and original return cold", ShiZhangYanChecks.XiaogeCompletedSoleSlashIssuesRealNativeDuelAndReturnsCold),
    ("Shi Deng Ai: full capacity entity losses threshold rewards and paid cold", ShiDengAiChecks.FullCapacityPerEntityLossAndThresholdSpendPaidChildCold),
    ("Shi Deng Ai: actual turn end paid multi target native Snatch cold", ShiDengAiChecks.ActualTurnEndDiscardAndOneDistanceFreeMultiTargetSnatchCold),
    ("Shi Xiahou Shang: preselected both independent targets native damage and paid cold recovery", ShiXiahouShangChecks.PreselectedBothIndependentTargetsNativeDamageAndPaidColdRecovery),
    ("Shi Xiahou Shang: cancellation zero one two self and live hand comparison", ShiXiahouShangChecks.CancellationZeroOneTwoSelfAndLiveHandComparison),
    ("Shi Xiahou Shang: post damage HP comparison self target and nested owner health change", ShiXiahouShangChecks.PostDamageHpComparisonSelfTargetAndNestedOwnerHealthChange),
    ("Shi Wei Yan: paid phase quotas Yinzhan mission and backwater cold", ShiWeiYanChecks.PaidPhaseQuotasYinzhanMissionAndBackwaterCold),
    ("Shi Wei Yan: dying mission failure keeps paid parent and Kunfen cold", ShiWeiYanChecks.DyingMissionFailureKeepsPaidParentAndKunfenCold),
    ("Shi Wei Yan: Kuanggu draw threshold returns original Slash cold", ShiWeiYanChecks.KuangguDrawThresholdReturnsToOriginalSlashCold),
    ("Shi Wei Yan: draw-phase threshold kill Kuanggu and mission cold", ShiWeiYanChecks.DrawPhaseThresholdKillAllowsKuangguAndMissionCold),
    ("Shi Chen Qun: native Faen opposite debt paid discard cold and played Slash exclusion", ShiChenQunChecks.NativeSelfBenefitsOppositeDebtPaidDiscardColdAndPurePlayedSlashes),
    ("Shi Tian Feng: gift ending ties opaque discard and next real Dodge cold", ShiTianFengChecks.GiftEndingTiesOpaqueDiscardAndNextRealDodgeCold),
    ("Shi Tian Feng: shared turn cap increasing backwater and dying choice cold", ShiTianFengChecks.SharedTurnCapIncreasingBackwaterAndDyingChoiceCold),
    ("Shi Wang Chang: dying population native draws reward and paid child cold", ShiWangChangChecks.KaijiDyingPopulationNativeDrawsRewardAndPaidChildCold),
    ("Shi Wang Chang: hidden top deck exact use turn and round name limits cold", ShiWangChangChecks.ShepanHiddenTopDeckExactUseTurnAndRoundNameLimitsCold),
    ("Shi Chen Zhi: paid HE extra turn lease and dying recovery cold", ShiChenZhiChecks.ExtraTurnDiscardChildrenDyingRecoveryAndSameRoundLimitCold),
    ("Shi Chen Zhi: global first discard and used or played Slash filter cold", ShiChenZhiChecks.GlobalFirstDiscardAndUsedOrPlayedSlashTargetFilterCold),
    ("Shi Chen Qun: Dodge use Faen actual loss ties and Dingpin round quota", ShiChenQunChecks.DodgeUseTriggersFaenAndDingpinCountsActualLossesTiesAndRoundQuota),
    ("Shi Dong Zhao: opening brain gain paid child foreign extra start cold", ShiDongZhaoChecks.OpeningBrainGainPaidChildAndForeignExtraStartCold),
    ("Shi Dong Zhao: distinct tricks cannot qualify and damage draw once", ShiDongZhaoChecks.DistinctTricksDoNotQualifyAndDamageDrawsOnce),
    ("Shi Chen Dao: selected entity distance no response and other target ban cold", ShiChenDaoChecks.SelectedEntityDistanceNoResponseAndOnlyOtherTargetBanCold),
    ("Shi Chen Dao: normal uses and response completion owners cold", ShiChenDaoChecks.NormalUseAndIssuedCounterspellHaveSeparateCompletionOwnersCold),
    ("Shi Chen Dao: preparation frozen X damage cap and issued ending cold", ShiChenDaoChecks.PreparationFrozenXDamageOccurrenceCapAndIssuedEndingCold),
    ("Shi Huang Zu: hand damage distance color response and use exclusion cold", ShiHuangZuChecks.HandDamageDistanceColorResponseAndUseDiscardExclusionCold),
    ("Shi Huang Zu: original slash phase judgment recast reward children cold", ShiHuangZuChecks.RandomOriginalSlashCurrentPhaseJudgmentRecastAndRewardChildrenCold),
    ("Mantian Guohai: real HEJ acquisitions before ordered gifts cold", MantianGuohaiChecks.RealTwoTargetHEJAcquisitionsThenOrderedGiftsCold),
    ("Mantian Guohai: per target nullification transferred holder and hand limit cold", MantianGuohaiChecks.PerTargetNullificationTransferredHolderAndHandLimitCold),
    ("Multiplayer: human seats prompts privacy and cold replay", MultiplayerChecks.MultipleHumansWaitRespondAndReplay),
    ("LAN rooms: HTTP players AI isolation retries and match completion", LanRoomChecks.HttpPlayersAiAndIsolation),
    ("LAN rooms: HTTP first round permissions retries and eight seats", LanRoomChecks.HttpFirstRound),
    ("Bagua response: failed judgment hand Dodge and cold replay", BaguaResponseChecks.FailedJudgmentReturnsToDodgeAndColdReplay),
    ("Bagua response: decline armor and red judgment retain hand Dodge", BaguaResponseChecks.DeclineAndRedJudgmentKeepHandDodgeOptional),
    ("Wushuang shop: mixed goods no mounts and ephemeral equipment", WushuangModeChecks.MixedShopNoMountsAndEphemeralEquipment),
    ("Wushuang mode: native shop growth acquired skills replacement and replay", WushuangModeChecks.NativeShopGrowthAcquiredSkillsReplacementAndReplay),
    ("Wushuang mode: explicit clock expires paid child and cold replay", WushuangModeChecks.ExplicitClockExpiresPaidChildAndColdReplay),
    ("Wushuang mode: survival limits compare current HP cold", WushuangModeChecks.SurvivalLimitsCompareCurrentHpWithoutRatioOrCards),
    ("Private color Duel: terminal attack returns to paid parent cold", BoundaryZhangChunHuaChecks.TerminalDuelReturnsToItsPaidParentOnceCold),
    ("Wushuang mode: Jinzhi shop equipment native payment and discard cold", WushuangModeChecks.JinzhiShopEquipmentPaysOneLogicalDiscardAndReturnsNativeUseAndResponseCold),
    ("Wushuang mode: category refund shop equipment payment cold", WushuangModeChecks.CategoryRefundCountsNativeShopEquipmentInItsExactThreeCategoryPaymentCold),
    ("Wushuang mode: Wang Yun shop equipment exact native use cold", WushuangModeChecks.WangYunJingongShopEquipmentPaysExactNativeUseAndReturnsCold),
    ("Wushuang mode: Quanmou shop equipment exact give and no false gain cold", WushuangModeChecks.QuanmouShopEquipmentSettlesExactGiveWithoutRecipientGainCold),
    ("Wushuang mode: destroyed equipment Nullification and delayed placement cold", WushuangModeChecks.PurchasedEquipmentNullificationAndDelayedPlacementKeepExactDestroyedPaymentCold),
    ("Wushuang mode: public pile shop equipment exact destroyed payment cold", WushuangModeChecks.PublicPileStoragePaysDestroyedShopEquipmentWithoutInventingPileCardsCold),
    ("Wushuang mode: Bingjie target shop equipment actual discard cold", WushuangModeChecks.BingjieTargetPaysActualShopEquipmentDestructionOnceCold),
    ("Happy Shen Lu Su: native shop equipment Dingzhou payments cold", CardGame.Core.Tests.HappyShenLuSuShopMovementChecks.RealShopEquipmentPaysBothDingzhouBranchesOnceCold),
    ("Wushuang AI shop: emergency opening and hand synergy", WushuangAiShopChecks.EmergencyOpeningAndHandSynergyUseExecutableBenefits),
    ("Wushuang AI shop: replacements and redundant capabilities", WushuangAiShopChecks.FullSlotsReplaceWeakSkillsAndRejectRedundantCapabilities),
    ("Wushuang AI shop: bounded refresh and savings", WushuangAiShopChecks.RefreshAndSavingsHaveFiniteProgressBounds),
    ("Wushuang AI shop: paid search follows missing build and purchase budget", WushuangAiShopChecks.PaidRefreshFollowsMissingBuildComponentsAndPreservesPurchaseBudget),
    ("Wushuang AI shop: locked upgrades wait for real tokens and expire", WushuangAiShopChecks.UsefulUnaffordableUpgradesLockBeforeSearchAndExpireWithoutProgress),
    ("Wushuang AI shop: dying windows discount unreliable income", WushuangAiShopChecks.DyingWindowsDoNotOutbidReliableDrawAsRoutineIncome),
    ("Wushuang AI shop: current skills change recovery and burst purchases", WushuangAiShopChecks.SameOwnerNewAndReplacedSkillsRecomputeRecoveryAndBurstPurchases),
    ("Wushuang AI shop: replacement prices current burst materials", WushuangAiShopChecks.ReplacementNetValueRetainsCurrentIndependentBurstMaterials),
    ("Wushuang AI shop: DrawCount uses native marginal value and replacement cost", WushuangAiShopChecks.DrawCountSetAndAddUseNativeMarginalValueAndReplacementCost),
    ("Wushuang AI shop: hand limit uses marginal capacity and replacement cost", WushuangAiShopChecks.HandLimitSetUsesActualMarginalCapacityAndReplacementCost),
    ("Wushuang material: purchased equipment Jianying Alcohol completes exact destroyed cost cold", WushuangModeChecks.PurchasedEquipmentJianyingAlcoholFinishesExactDestroyedMaterialCold),
    ("Anniversary Ma Tie: lethal Preparation Slash returns without starting retired phases cold", AnniversaryMaTieChecks.LethalPreparationSlashReturnsWithoutStartingRetiredPhasesCold),
    ("Duan Jiong: lethal discarded Slash completes exact paid ancestor cold", DuanJiongChecks.LethalDiscardSlashCompletesOriginalPaidAncestorCold),
    ("Wushuang AI shop: own published affordable deterministic choices", WushuangAiShopChecks.DecisionsUseOnlyOwnPublishedAffordableOffersAndAreDeterministic),
    ("Wushuang AI shop: public pressure and remaining time", WushuangAiShopChecks.PublicPressureFavorsDefenseButLatePurchasesStillSeekUsefulProgress),
    ("Wushuang AI shop: native defense fixed damage and drunken fist", WushuangAiNativeShopChecks.NativeDefenseFixedDamageAndDrunkenFistHaveConditionalPurchaseValue),
    ("Wushuang AI shop: native quotas slots and original replacement", WushuangAiNativeShopChecks.NativeQuotaSlotsAndOriginalReplacementRespectExistingCapacity),
    ("Wushuang AI shop: native multiple purchases and paid child cold", WushuangAiShopChecks.NativeAiMultiplePurchasesResumeThePaidAcquisitionParentCold),
    ("Wushuang AI combat: fixed damage and drunken Slash cold", WushuangAiCombatChecks.FixedDamageKeepsWineUnlessDrunkenSlashCannotBeResistedCold),
    ("Wushuang AI combat: thorns self defeat and lethal cold", WushuangAiCombatChecks.ThornsAvoidsSelfDefeatButDoesNotInventRetaliationAfterDeathCold),
    ("Wushuang AI combat: finite reduction public receipts cold", WushuangAiCombatChecks.FiniteReductionUsesPublicReceiptsAndIndependentSlashBudgetCold),
    ("Wushuang AI combat: incoming tactics keep unsafe injury defense cold", WushuangAiCombatChecks.NativeIncomingTacticsKeepUnsafeInjuryDefenseAndLegacyFieldsCold),
    ("Wushuang AI combat: nullified trick claims once finished physical card cold", WushuangAiCombatChecks.NullifiedTrickClaimsItsOnceFinishedPhysicalMaterialCold),
    ("Wushuang AI build: visible skills and native changes cold", WushuangBuildProfileChecks.VisibleRealGeneralsAndNativeSkillChangesUpdateTheBuildCold),
    ("Wushuang AI build: safe injury responds to current skills", WushuangAiBuildBattleChecks.CurrentInjuryBuildChangesSafeResponseAndKeepsDangerousAndLegacyDefense),
    ("Wushuang AI build: current Slash burst uses real materials", WushuangAiBuildBattleChecks.CurrentSlashBuildUsesSupportedBurstWithoutInventingWineMaterialsOrIgnoringRetaliation),
    ("Wushuang skills B: OfficialFuqiNonTargetResponseAndGuanchaoTrueUse", WushuangSkillsBChecks.OfficialFuqiNonTargetResponseAndGuanchaoTrueUse),
    ("Wushuang skills B: OfficialGuzhengAndDuwuVersionDifferences", WushuangSkillsBChecks.OfficialGuzhengAndDuwuVersionDifferences),
    ("Wushuang skills A: YashiSuppressesItsActualSelfSourceUntilTheNextStart", WushuangOfficialSkillsAChecks.YashiSuppressesItsActualSelfSourceUntilTheNextStart),
    ("Wushuang skills A: TiaoxinExpandsOnlyAfterItsRequestedSlashFailsToDamage", WushuangOfficialSkillsAChecks.TiaoxinExpandsOnlyAfterItsRequestedSlashFailsToDamage),
    ("Wushuang skills A: OfficialAuditKeepsSelfGiftsPublicEquipmentCostsAndPhaseLimits", WushuangOfficialSkillsAChecks.OfficialAuditKeepsSelfGiftsPublicEquipmentCostsAndPhaseLimits),
    ("Wushuang skills A: OriginalYijiAndGuanxuKeepPrivatePhysicalEntitiesAcrossColdReplay", WushuangOfficialSkillsAChecks.OriginalYijiAndGuanxuKeepPrivatePhysicalEntitiesAcrossColdReplay),
    ("Wushuang tactic regression: exempt hand limit skips manual and automatic discard", WushuangModeChecks.HandLimitExemptTacticsSkipBothManualAndAutomaticDiscard),
    ("Wushuang tactic regression: Bowen tiers obtain real tricks at each own turn start", WushuangModeChecks.BowenTiersObtainRealTricksAtEachOwnTurnStart),
    ("Wushuang skills A: ChoufaRewritesOnlyFrozenHandEntitiesWithoutInspectingTheirNames", WushuangOfficialSkillsAChecks.ChoufaRewritesOnlyFrozenHandEntitiesWithoutInspectingTheirNames),
    ("Wushuang skills A: TairanRepaysActualRecoveredHpAndOnlyItsStillHeldDrawnCards", WushuangOfficialSkillsAChecks.TairanRepaysActualRecoveredHpAndOnlyItsStillHeldDrawnCards),
    ("Wushuang skills A: LingrenBindsItsDamageToTheGuessedUseAndCiweiCancelsOnlySecondUse", WushuangOfficialSkillsAChecks.LingrenBindsItsDamageToTheGuessedUseAndCiweiCancelsOnlySecondUse),
    ("Wushuang skills A: JinzhiPaysIncreasingSameColorCountsAndDrawsBeforeNativeBasicEffects", WushuangOfficialSkillsAChecks.JinzhiPaysIncreasingSameColorCountsAndDrawsBeforeNativeBasicEffects),
    ("Wushuang combat: NoArmorFixedDamageAndFiniteReduction", WushuangCombatChecks.NoArmorFixedDamageAndFiniteReduction),
    ("Wushuang combat: DrawTwoAndAdditionalDismantlementKeepNativePaymentsCold", WushuangCombatChecks.DrawTwoAndAdditionalDismantlementKeepNativePaymentsCold),
    ("Wushuang combat: RecastAndActualDrawThresholdResumeTheirPaidInstruction", WushuangCombatChecks.RecastAndActualDrawThresholdResumeTheirPaidInstruction),
    ("Wushuang combat: LightningJudgmentsUseNativeReplacementAndSourcelessDamage", WushuangCombatChecks.LightningJudgmentsUseNativeReplacementAndSourcelessDamage),
    ("Wushuang combat: DyingExitDamageWaitsForItsNativePaidChildCold", WushuangCombatChecks.DyingExitDamageWaitsForItsNativePaidChildCold),
    ("Wushuang combat: native draw threshold damage preserves suspended before-damage cold", WushuangCombatChecks.NativeDrawThresholdDamageKeepsSuspendedBeforeDamageCold),
    ("Wushuang combat: finalized damage response judgment keeps exact materials cold", WushuangCombatChecks.FinalizedDamageResponseJudgmentReplacesItsOwnMaterialCold),
    ("Wushuang combat: paid Tianming native Draw2 keeps original Slash cold", WushuangCombatChecks.ActualTargetPaidTianmingDrawThresholdReturnsOriginalSlashCold),
    ("Wushuang audit combat: JieyingClaimsOnlyAfterNativeTurnEndedAndResumesGainChildCold", WushuangAuditCombatChecks.JieyingClaimsOnlyAfterNativeTurnEndedAndResumesGainChildCold),
    ("Wushuang audit combat: WangongKeepsLastActualBasicAcrossTurnsAndCapturesAcceptedSlash", WushuangAuditCombatChecks.WangongKeepsLastActualBasicAcrossTurnsAndCapturesAcceptedSlash),
    ("Wushuang pools: OfficialPoolsPricesVersionsAndAcquisitionRecovery", WushuangModeChecks.OfficialPoolsPricesVersionsAndAcquisitionRecovery),
    ("Wushuang pools: FreeRefreshCapacityAndOriginalReplacement", WushuangModeChecks.FreeRefreshCapacityAndOriginalReplacement),
    ("Wushuang pools: OriginalQimouAndHunZiVersions", WushuangModeChecks.OriginalQimouAndHunZiVersions),
    ("Wushuang pools: TacticQuotaFloorsAndEmptySlotsUseActualRules", WushuangModeChecks.TacticQuotaFloorsAndEmptySlotsUseActualRules),
    ("Wushuang skills B: WeiyiYinglueAndZhendanNativeRules", WushuangSkillsBChecks.WeiyiYinglueAndZhendanNativeRules),
    ("Wushuang skills B: LiantaoAndXiaojieNativeUsesReplay", WushuangSkillsBChecks.LiantaoAndXiaojieNativeUsesReplay),
    ("Wushuang skills B: BianyuIdentityAndChunlaoAnyHpLoss", WushuangSkillsBChecks.BianyuIdentityAndChunlaoAnyHpLoss),
    ("Wushuang skills B: DinganSecondNameAndLargestHandDiscard", WushuangSkillsBChecks.DinganSecondNameAndLargestHandDiscard),
    ("Wushuang skills B: OfficialJiushiActiveAlcoholAndDamageFace", WushuangSkillsBChecks.OfficialJiushiActiveAlcoholAndDamageFace),
    ("Wushuang skills B: OfficialQiangzhiDingcuoAndLonghunRules", WushuangSkillsBChecks.OfficialQiangzhiDingcuoAndLonghunRules),
    ("Wushuang skills B: OfficialZengouFirstDodgeMultiTargetAndDyingCost", WushuangSkillsBChecks.OfficialZengouFirstDodgeMultiTargetAndDyingCost),
    ("Guandu mode: frozen roster private rotating draft tier five military deck", GuanduModeChecks.RealPackageFreezesRosterRotatesPrivateDraftAndDealsTierFiveMilitaryDeck),
    ("Guandu mode: scoped options and historical checkpoint shape", GuanduModeChecks.GuanduOptionsRemainScopedAndOldCheckpointShapeStaysUnchanged),
    ("Guandu mode: enemy hand friendly penalty and native commander victory", GuanduModeChecks.NativeKillsTransferEnemyHandPunishFriendlyKillsAndDeferCommanderVictory),
    ("Guandu mode: real deck exhaustion finishes native draw children", GuanduModeChecks.ActualDeckExhaustionWaitsForDrawChildrenAndAwardsAiArmy),
    ("Guandu events: counterspell Use and played Slash keep distinct global count", GuanduEventChecks.TenthUseCountsNativeCounterspellButNotPlayedSlash),
    ("Guandu events: tenth global use native replay and target legality", GuanduEventChecks.TenthUsesGlobalNativeCounterAndRevalidatesOriginalTarget),
    ("Guandu events: tenth BorrowedSword preserves owner and paired source victim", GuanduEventChecks.TenthBorrowedSwordRechecksOnlyWeaponOwnersAndKeepsThePairedSourceVictim),
    ("Guandu events: Wuchao fire underdog and recipient damage base", GuanduEventChecks.WuchaoAndUnderdogUseActualDamageAndRecipientScopes),
    ("Guandu events: grain shortage normal draw and once per use reward", GuanduEventChecks.GrainShortageReplacesNormalCountAndRewardsUseOnce),
    ("Guandu events: real turn start duel or health loss and cold resume", GuanduEventChecks.TurnStartDuelAndHpLossRetainRealSourceAndCold),
    ("Guandu events: hold line round allowance next discard debt extra turn", GuanduEventChecks.HoldLineTracksRoundDebtAcrossGenuineExtraTurn),
    ("Guandu events: standoff actual round five first slash damage", GuanduEventChecks.StandoffSwitchesAtRoundFiveAndOnlyFirstSlash),
    ("Guandu events: slow advance responses skipped and next actual draw", GuanduEventChecks.SlowAdvanceTracksResponsesSkippedDrawAndNextActualDraw),
    ("Guandu events: arrogance independent health and hand maxima", GuanduEventChecks.ArroganceChecksHandMaximumAfterItsRealDiscard),
    ("Guandu chariot: Wusheng response destroyed invoice and cold native completion", GuanduChariotChecks.RedChariotWushengResponseKeepsSingleDestroyedInvoiceAndTypedCompletion),
    ("Guandu chariot: Wusheng use and actual discard destroy one entity", GuanduChariotChecks.RedChariotWushengUseAndRealDiscardPhaseDestroyOnce),
    ("Guandu cards: recovery chains and exact recast", GuanduCardChecks.ReinforcementsAndUnitedEffortKeepNativeRecoveryChainsAndRecast),
    ("Guandu generals: private Shicai and live faction qualification cold", GuanduGeneralChecks.ShicaiPrivateTopAndFushiLivingFactionQualificationSurviveColdReplay),
    ("Guandu generals: foreign normal draw interception and exact payment cold", GuanduGeneralChecks.JieliangPaysBeforeForeignNormalDrawAndUsesActualTurnHandLimit),
    ("Guandu generals: grain collapse and incoming Gangzhi native boundaries", GuanduGeneralChecks.GrainCollapseAndIncomingOnlyGangzhiUseNativeDrawAndHpReplacement),
    ("Guandu generals: generated chariot both slots and departure destruction", GuanduGeneralChecks.ThunderChariotGeneratesRealEquipmentDiscardsBothSlotsAndDestroysOnDeparture),
    ("Guandu generals: Choulue native Duel FireAttack and cold children", GuanduGeneralChecks.ChoulueGiftIssuesRealVirtualDuelAndFireAttackAcrossColdChildren),
    ("Guandu generals: Yuanlve entity and Moushi actual next play receipts", GuanduGeneralChecks.YuanlveExactGiftAndMoushiNextActualPlayUseNativeDrawReceipts),
    ("Guandu generals: off turn Fuyuan and owner death Zhongjie native returns", GuanduGeneralChecks.FuyuanOffTurnNativeUseAndZhongjieOwnerDeathKeepTheirPaidChildren),
    ("Guandu generals: Suishi real dying and faction death boundaries", GuanduGeneralChecks.SuishiUsesTrueDyingEntryAndFactionDeathWithoutConfusingTheArmy),
    ("Guandu generals: Xiying exact turn paid refusal blocks use and response", GuanduGeneralChecks.XiyingPaidRefusalBlocksTrueUseAndResponseForOnlyTheActualTurn),
    ("Dragon boat activity: opt in team only fixed draft and cold restore", DragonBoatGeneralChecks.ActualPackageDraftIsOptInAndSharedWithOnlyTheTeammate),
    ("Dragon boat activity: treasure cap friendly damage loss and extra turn boundary", DragonBoatGeneralChecks.TreasurePaymentsCapAndTwoActualRoundsIncludeExtraTurns),
    ("Dragon boat generals: real consecutive targets without ordinary quota", DragonBoatGeneralChecks.LianpianUsesRealConsecutiveTargetsWithoutTheOrdinaryOlQuota),
    ("Dragon boat generals: actual damage points and exact Jianji card cold", DragonBoatGeneralChecks.DianhuCountsRealDamagePointsAndJianjiKeepsItsNativeExactCard),
    ("Dragon boat generals: threshold grants and team Wenji Tunjiang cold", DragonBoatGeneralChecks.XingzhaoSharesThresholdBenefitsAndWenjiTunjiangUseTeamRules),
    ("Dragon boat cards: full basic families retain arity and named Peach", DragonBoatCardChecks.FullBasicFamiliesKeepMaterialArityWhileNamedPeachStaysPeach),
    ("Wenhe activity: real eight seats private draft and cold restore", WenhePackageChecks.RealOptInOfficialRosterPrivateDraftAndColdRestore),
    ("Wenhe mode grants: template replacement and ordinary same-name instances", WenheRuleGrantChecks.ModeInstancesSurviveTemplateReplacementWithoutProtectingOrdinarySameName),
    ("Wenhe mode grants: native suppression death curse and cold rules", WenheRuleGrantChecks.NativeSuppressionAndDeathCursePreserveModeRulesNirvanaAndChampionMark),
    ("Wenhe rules: first round all actors nearest Slash and cold prompt", WenheRuleChecks.FirstRoundChaosIncludesOwnerUsesNearestPhysicalSlashAndColdPrompt),
    ("Wenhe rules: private champion mark native draw once and cold replay", WenheRuleChecks.ChampionMarkNativeDrawPrivateUntilUseOnceAndColdReplay),
    ("Wenhe rules: native death children reward then sole survivor", WenheRuleChecks.KillRewardRaisesOnlyMaximumDrawsThreeAndFinalDeathChildPrecedesWinner),
    ("Wenhe rounds: famine host dies without issuing the rule twice", WenheRoundRuleChecks.FamineIssuesOnceWhenItsHostActuallyDiesAtOldRoundEnd),
    ("Wenhe rounds: equipment replacement finishes children before next tied actor", WenheRoundRuleChecks.EquipmentReplacementCompletesOneParticipantChildBeforeTheNextTie),
    ("Wenhe rounds: dead actual owner retains only mode equipment ending", WenheRoundRuleChecks.DeadActualTurnOwnerStillRunsOnlyTheModeEquipmentEnding),
    ("Wenhe rounds: bounty doubles real reward without healing", WenheRoundRuleChecks.BountyKillUsesDoubleRealRewardWithoutHealing),
    ("Wenhe rounds: last stand rescue draw and extra turn paid once", WenheRoundRuleChecks.LastStandRescueDrawAndExtraActualTurnArePaidOnceEach),
    ("Wenhe rounds: sweep source damage chain propagation and ordinary loss", WenheRoundRuleChecks.SweepAddsAtTheSourceOnceWhileChainAndHpLossStayExact),
    ("Wenhe rounds: all dead famine waits for frozen children before draw", WenheRoundRuleChecks.FamineAllDeadCompletesAsDrawAfterItsFrozenDeathChildren),
    ("Wenhe cards: star shift native health children and counterspell", WenheCardChecks.StarShiftUsesRealSettingsChildrenAndOneCounterspell),
    ("Wenhe poison: serial public loss hidden transfers and cold return", WenhePoisonChecks.PublicDeparturesAreSerialNativeLossesAndHiddenTransfersStayOpaque),
    ("Wenhe poison: physical conversion pays loss before Slash and dying", WenhePoisonChecks.ConvertedPhysicalPoisonPaysBeforeItsRealSlashAndCanEnterDying),
    ("Wenhe poison: fire attack waits for public poison cost before damage", WenhePoisonChecks.FireAttackWaitsForItsPublicPoisonCostBeforeDamage),
    ("Wenhe AI: four published tricks score survival views without self target crash", WenheAiChecks.FourPublishedTricksScorePublicSurvivalViewsWithoutSelfTargetCrash),
    ("Hell activity: five tiers dormant auxiliary private draft checkpoint", HellActivityChecks.RealPackageFiveTiersDormantAuxiliaryRotatedDraftAndCheckpoint),
    ("Hell activity: four stages await deaths and unlock hidden boss", HellActivityChecks.FourStagesWaitForDeathChildrenAndRealMengpoUnlocksHiddenBoss),
    ("Hell activity: living Mengpo keeps ordinary final boss and victory", HellActivityChecks.LivingMengpoDoesNotUnlockHiddenBossOrBlockFinalVictory),
    ("Hell Mengpo: real robe blocks all soul targets before payment", HellMengpoChecks.RealRobeExcludesEverySoulTargetWithoutPublishingAnEmptyUse),
    ("Hell ordinary tricks: Qice permission preserves other names and materials", HellOrdinaryTrickChecks.QicePermissionFilterKeepsOtherNamesAndUnpaidMaterials),
    ("Hell ordinary tricks: added soul all hand one zero materials native restoration", HellOrdinaryTrickChecks.AddedSoulNamePreservesAllHandOneZeroMaterialsAndNativeSkillRestoration),
    ("Hell tier: Chongsheng keeps owned zones limited ledger and cold draw", HellTierPrivilegeChecks.ChongshengPreservesHandEquipmentAndLimitedLedgerAcrossColdDraw),
    ("Hell tier: actual equipment late fifth tier assault native cold", HellTierPrivilegeChecks.TierEquipmentAndLateFifthTierAssaultUseRealNativeCardsOnce),
    ("Hell kings: death children recalculate highest health", HellKingChecks.DeathEffectsRecalculateExtremeHpAndPreserveNativeSubtrees),
    ("Hell Dizang: actual distance fire thresholds draw children cold", HellDizangChecks.ActualDistanceFireDamageThresholdAndWuliangDrawChildrenCold),
    ("Hell Dizang: native judgment any suit rank choices cold", HellDizangChecks.FinalJudgmentAnySuitRankChoiceAndColdReplay),
    ("Hell Dizang: abolished mounts native recast draw cold", HellDizangChecks.AbolishedMountSlotsNativeRecastPaymentDrawAndCold),
    ("Hell Mengpo: pre-discard distinct suits native movement", HellMengpoChecks.ShiyouRedirectsHandLimitAndProgramDiscardsBeforeEntry),
    ("Hell Mengpo: exact lost skill restoration native soul card", HellMengpoChecks.WanghunRestoresOnlyItsOwnSkillsAndDestroysSoulWithColdChildren),
    ("Hell Mengpo: actual enemy turn category use response ban", HellMengpoChecks.WangshiBansUseAndResponseCategoryForTheActualEnemyTurn),
    ("Shanhe campaign: frozen content routes and nested views", ShanheCampaignChecks.ContentRoutesAndPublishedNestedListsAreFrozen),
    ("Shanhe exit: map choice battle reward and cold journal", ShanheCampaignChecks.EndExpeditionFromMapChoiceBattleAndRewardReplaysCold),
    ("Shanhe chapters: first two red tactics and skills then direct final settlement", ShanheChapterRewardChecks.FirstTwoBossesGrantNativeTacticsAndSkillsThenFinalVictorySettlesDirectly),
    ("Shanhe chapters: epic tactic card and skill pools and bounded nested definitions", ShanheChapterRewardChecks.CurrentChapterPoolsAreEpicAndNestedDefinitionsRemainBounded),
    ("Shanhe legendary tactics: real Spade quota armor and repeated reflections", ShanheLegendaryTacticBuildChecks.UnlimitedSpadesOpeningArmorAndEveryDamageReflectionsUseNativeFlows),
    ("Shanhe economy: skills category surcharge actual payments and cold", ShanheCampaignChecks.SkillShopEscalationUsesActualPaymentAndResetsAtNextShopCold),
    ("Shanhe economy: native victory interest bargain recycling and five refreshes cold", ShanheCampaignChecks.EconomyVictoryInterestDiscountRefundAndFiveRefreshesUseRealCommandsCold),
    ("Shanhe economy: stock expansion and ordinary one refresh remain conditional cold", ShanheCampaignChecks.EconomyStockExpansionAndOrdinarySingleRefreshRemainConditionalCold),
    ("Shanhe market overlord: actual purchases all categories and discounts", ShanheMarketOverlordChecks.ActualOwnershipSuppressesAllCategorySurchargesBeforeDiscount),
    ("Shanhe starting wealth: opening only inventory and rejected late grants", ShanheStartingWealthChecks.StartingWealthRemainsEquipOnlyAndRejectsMidExpeditionGrants),
    ("Shanhe wealth tactics: strict balances native opening draw and hand limit", ShanheWealthTacticChecks.StrictBalancesAndActualOwnershipQualifyNativeOpeningEffects),
    ("Shanhe wealth tactics: card skill reflection and chain damage", ShanheWealthTacticChecks.QualifiedDamageIncludesCardsDirectSkillsAndRealReflection),
    ("Shanhe wealth tactics: spending requalifies the next actual encounter", ShanheWealthTacticChecks.SpendingRequalifiesAllHeldSourcesForTheNextEncounter),
    ("Shanhe draw tactics: normal phase own ending skill draw and discard", ShanheDrawTacticChecks.AdditiveNormalDrawAndOwnEndingKeepNativeDiscardAndSkillDraw),
    ("Shanhe draw tactics: every other public ally draws two more", ShanheDrawTacticChecks.TeamDrawAddsTwoForEveryOtherPublicAllyOnly),
    ("Shanhe preparation: actual game starting entities and one opening", ShanhePreparationTacticChecks.ActualGameStartingDrawsStackAfterInitialDealAndNeverRepeatWithoutOwnership),
    ("Shanhe logistics: negative initial deal zero floor and normal draw", ShanheLogisticsTacticChecks.StackedInitialPenaltiesClampBeforeDealAndKeepNormalDrawBonuses),
    ("Shanhe logistics: negative modifier loader limits", ShanheLogisticsTacticChecks.NegativeInitialModifiersRejectConditionalForeignAndDynamicDefinitions),
    ("Shanhe elite rewards forward: published quality categories and compensation", ShanheEliteRewardChecks.PublishedEliteDraftsKeepRareCategoriesCompensationAndPromotionsForward),
    ("Shanhe reward categories: owned counts frozen type and actual claim", ShanheRewardCategoryChecks.CategoryChoiceCountsFollowOwnedTacticsAndStayFrozen),
    ("Shanhe reward economy: grouped non tactic declines preserve pending groups cold", ShanheCampaignChecks.GroupedNonTacticDeclinesResolveOnlyTheirGroupAndReplayCold),
    ("Shanhe reward economy: signed income ordinary and boss bases once cold", ShanheCampaignChecks.SignedIncomeModifiersApplyToOrdinaryAndBossBasesOnceCold),
    ("Shanhe bonus rewards: elite ownership primary claim and no duplication", ShanheBonusRewardTacticChecks.EliteVictoryFreezesOwnedRareBonusAfterPrimaryRewardOnlyOnce),
    ("Shanhe battle quality: boss exact bonus terminal actual twenty percent", ShanheBattleQualityChecks.RealBossAndTwentyPercentWinsPreserveExactBonusTerminalAndOwnershipFreeze),
    ("Shanhe boss opening: typed battle kinds and actual initial entities", ShanheBossOpeningTacticChecks.OwnedSourcesUseBattleKindAndNativePhysicalDeal),
    ("Shanhe boss opening: native second phase preserves dealt hand", ShanheBossOpeningTacticChecks.NativeSecondFormKeepsTheAlreadyDealtPlayerHand),
    ("Shanhe persistent market: full production catalogue and registry", ShanheCycleLoadoutChecks.PersistentMarketUsesTheActualFullCatalogueAndBattleRegistry),
    ("Shanhe low HP damage: native loss recovery cards and skill damage", ShanheLowHpDamageChecks.NativeHpThresholdTracksLossRecoveryCardsAndSkillDamage),
    ("Shanhe low HP damage: loader and undefined HP boundaries", ShanheLowHpDamageChecks.SourceHpThresholdLoaderAndUndefinedHp),
    ("Shanhe round recovery: opening minimum clamp and new battle baseline", ShanheRoundRecoveryChecks.XuezhanOpeningFloorsAtOneClampsHpAndNewBattleUsesItsOwnBaseline),
    ("Shanhe round recovery: native recovery once per real round", ShanheRoundRecoveryChecks.XuezhanRecoversAtMostTwoOncePerRealRound),
    ("Shanhe incoming reduction: zeroed segments round quota and reset", ShanheIncomingReductionChecks.RoundBudgetCountsZeroedSegmentsAndResetsAfterThree),
    ("Shanhe incoming reduction: elemental chain independent recipient budgets", ShanheIncomingReductionChecks.ElementalChainRecipientsReduceWithIndependentOwnerBudgets),
    ("Shanhe shop availability: verified skills real purchase and research gate", ShanheShopAvailabilityChecks.VerifiedUnmappedSkillsReachTheActualShopWithoutUnlockingResearchSkills),
    ("Shanhe Dingcuo: self damage actual colors and shared turn allowance", ShanheDingcuoChecks.NativeSelfDamageUsesActualDrawColorsAndOneSharedTurnAllowance),
    ("Shanhe Guanwei: real payment gates draw and inserted Play", ShanheGuanweiPaymentChecks.EmptyPaymentCancelsAndPaidMovementReturnsBeforeExtraPlay),
    ("Shanhe Guanchao: true response use phase ledger and own turn scope", ShanheGuanchaoChecks.TrueResponseUseKeepsThePhaseRankLedgerAndWholeOwnTurnScope),
    ("Shanhe schools: owned exact tiers and distinct identities", ShanheSchoolChecks.CompletionRequiresOwnedExactRaritiesAndDistinctAcquisitionSources),
    ("Shanhe school draw: initial and paid native refill", ShanheSchoolDrawChecks.InitialAndPaidCompletionRefillWithRealDrawsAndNativeChildren),
    ("Shanhe school draw: unowned program cannot activate", ShanheSchoolDrawChecks.UnownedManualGrantCannotActivateTheDrawSchool),
    ("Shanhe school draw: depleted deck returns", ShanheSchoolDrawChecks.DepletedDeckRefillsOnlyAvailableMaterialsAndReturns),
    ("Shanhe school draw: shared hand count selected cap and zero deficit", ShanheSchoolDrawChecks.SharedHandCountDrawKeepsTheSelectedMaximumCapAndZeroDeficit),
    ("Shanhe school slash: production six component recipe", ShanheSchoolSlashChecks.ProductionSixComponentRecipeDispatchesItsDerivedSchoolProgram),
    ("Shanhe school vital: native source damage and chain layer", ShanheSchoolVitalChecks.OwnedDamageSchoolUsesNativeCardsSkillsAndChainLayer),
    ("Shanhe school vital: preparation adjustment and health child", ShanheSchoolVitalChecks.PreparationSchoolAdjustsHpWithoutRecoveryAndDrainsHealthChild),
    ("Shanhe school vital: first dying per actual turn", ShanheSchoolVitalChecks.DyingSchoolRecoversToOneOnlyOncePerActualTurn),
    ("Shanhe school trick: single paid use and Fengzi effects", ShanheSchoolTrickChecks.OnePaidUseRepeatsNativeDamageEffectsAndCoexistsWithFengzi),
    ("Shanhe third Slash: dodged virtual and cross round declarations", ShanheThirdSlashChecks.NativeThirdSlashCountsDodgedVirtualAndCrossRoundDeclarations),
    ("Shanhe Miaoshou: distant Snatch keeps real native payment and Slash range", ShanheMiaoshouChecks.RealSnatchReachesDistantHandWithoutExtendingSlash),
    ("Shanhe remaining schools: production full five-member recipes", ShanheRemainingSchoolStockChecks.ProductionFiveMemberRecipesDispatchThroughTheNativeBattleFactory),
    ("Shanhe school late: global rounds first declarations misses and chain", ShanheSchoolLateChecks.GlobalRoundsFirstDeclarationsMissesAndChainsUseOneNativeCardBonus),
    ("Shanhe school late: paid completion keeps the current round ledger", ShanheSchoolLateChecks.PaidCompletionKeepsTheCurrentRoundLedgerAndUnearnedCopiesStayInactive),
    ("Shanhe late round stock: native damage and Slash quota thresholds", ShanheLateRoundTacticChecks.DamageAndSlashQuotaChangeOnlyAtTheirActualGlobalRoundThresholds),
    ("Shanhe late round stock: complete sources native draw and Slash thresholds", ShanheLateRoundTacticChecks.CompleteLateRoundStockStacksNativeDrawsAndRoundTwoSlashQuota),
    ("Shanhe team health: native health adjustment preserves wounded allies", ShanheTeamHealthChecks.ActualAlliedHealthBonusPreservesWoundsAndBypassesRecoveryModifiers),
    ("Shanhe school team: initial rank floor wounds and actual initial cards", ShanheSchoolTeamChecks.InitialSharingUsesTheRankFloorPreservesWoundsAndDealsRealOpeningCards),
    ("Shanhe school team: paid rank initializes allies once after native children", ShanheSchoolTeamChecks.PaidCompletionInitializesLivingAlliesOnceAndReturnsAfterNativeChildren),
    ("Shanhe school team: independent expedition rebirth", ShanheSchoolTeamChecks.FifthRankAlliesAndPlayerHaveIndependentExpeditionRebirth),
    ("Shanhe school team: unowned program and ordinary team isolation", ShanheSchoolTeamChecks.UnownedManualGrantAndOrdinaryTeamsCannotReceiveRankPrivileges),
    ("Shanhe repeated entry: actual copies preserve allied armor draw and school counts", ShanheRepeatedEntryChecks.EarnedRepeatableCopiesKeepArmorAndDrawAtBattleEntry),
    ("Shanhe wine tactics: paid additive quota actual wine Slash and later turns", ShanheWineTacticsChecks.PaidWineTiersAddRealQuotaAndRenewOnTheNextActualTurn),
    ("Shanhe wine tactics: actual dying Wine leaves ordinary quota available", ShanheWineTacticsChecks.DyingPhysicalWineDoesNotConsumeTheOrdinaryWineBudget),
    ("Shanhe Zuiquan: sober complete Slash family and native Wine", ShanheZuiquanChecks.CompleteSlashFamilyBypassesDodgesWithoutWineAndKeepsNativeAlcohol),
    ("Shanhe Guandao: paid Diamond Slash distance and converted materials", ShanheGuandaoChecks.PaidDiamondSlashRangeIncludesNativeElementsAndConvertedCards),
    ("Shanhe low HP draw: actual health thresholds and native draw windows", ShanheLowHpDrawChecks.CurrentHealthThresholdsStackOnlyDuringTheOwnersRealDrawPhase),
    ("Shanhe opening boss stage: native opening damage preserves unfinished startup", ShanheBossOpeningStageChecks.RealOpeningDrawDamageTransformsWithoutRetiringTheUnfinishedStartup),
    ("Shanhe opening boss stage: actual turn interruption still starts the boss turn", ShanheBossOpeningStageChecks.ActualTurnLethalStillRetiresThatTurnAndStartsTheBossNaturalTurn),
    ("Shanhe Heart Blade: paid native Heart suit predictions and chain", ShanheHeartBladeChecks.LivePurchaseUsesFrozenNativeSuitAndChainInheritsOnce),
    ("Shanhe Ershengsan: real counterspell and repeated paid effects", ShanheErshengsanChecks.NullificationAndFengziRetainOriginalPaidUseAndEffectQuantity),
    ("Shanhe Dual Blade: dodged unowned Slash before live acquisition", ShanheDualBladeChecks.DodgedUnownedSlashConsumesRoundBeforeLivePurchase),
    ("Shanhe hand limit stock: paid exemptions preserve partial discard", ShanheHandLimitStockChecks.PaidExemptionsRefreshTheSamePartlyPaidDiscard),
    ("Shanhe double steal: real counterspell and independent Fengzi rounds", ShanheDoubleStealChecks.RealCounterspellAndPaidFengziKeepIndependentEffectRounds),
    ("Shanhe double response: frozen target Dodges and completed response child", ShanheDoubleResponseChecks.ArrowsFreezeEachIssuedTargetAndResumeTwoPaidDodgesAfterTheirChild),
    ("Shanhe trick response: paid enemy scope and independent counterspell chain", ShanheTrickResponseChecks.PaidYinyangKeepsAlliesAndCounterspellChainOutsideEnemyScope),
    ("Shanhe shop stock: five base items and owned expansion", ShanheShopQualityChecks.BaseFiveSlotsAndOwnedExpansionUseRealCategoryStock),
    ("Shanhe persistent market: chapter shelf survives ordinary map choices", ShanhePersistentMarketChecks.ChapterShelfSurvivesMapChoicesAndOnlyResetsAcrossChapters),
    ("Shanhe persistent market: frozen battle seed isolated wallet and health quotes", ShanhePersistentMarketChecks.BattleMarketStartsFromFrozenStateWithoutCampaignHistoryOrDoublePayment),
    ("Shanhe live market: paid skills tactics cards and original play prompt", ShanheLiveMarketChecks.PaidSkillTacticAndCardsApplyNowAndKeepTheOriginalPrompt),
    ("Shanhe live market: purchased card joins the suspended Slash response", ShanheLiveMarketChecks.ACardBoughtDuringSlashResponseJoinsThatExactResponse),
    ("Shanhe live market: negative health purchases keep the original dying", ShanheLiveMarketChecks.DyingPurchasesUseRealNegativeHealthAndReturnTheOriginalRescue),
    ("Shanhe live market: entry wealth survives purchases and refreshes", ShanheLiveMarketChecks.EntryWealthQualificationSurvivesPurchasesAndRefreshes),
    ("Native draw threshold: mandatory owner hand gain definition", NativeDrawThresholdDefinitionChecks.ThresholdRequiresMandatoryOwnerHandGainAndPositiveCount),
    ("Shanhe draw threshold: Niepan dying draw returns to original damage", ShanheDrawThresholdChecks.NiepanDyingDrawCrossesOneThresholdAndResumesOriginalDamage),
    ("Shanhe draw threshold: native paid draws insert damage and return cold", ShanheDrawThresholdChecks.NativePaidDrawsInsertThresholdDamageAndReturnToOriginalDamageCold),
    ("Shanhe native rule repair: acquired Longdan play healing and dying rescue", ShanheLongdanRuleChecks.AcquiredAlcoholPeachHealsDuringPlayAndStillRescuesDying),
    ("Shanhe native rule repair: acquired Weimu black self tricks and red uses", ShanheWeimuRuleChecks.AcquiredWeimuCoversBlackSelfTricksWithoutBlockingRedCardsOrOtherTargets),
    ("Shanhe recast tactics: paid HE group returns after recovery child", ShanheRecastTacticChecks.CompletedSlashRecastsHandAndEquipmentAfterNativeRecoveryChild),
    ("Shanhe recast tactics: purchased tiers and outside-turn response", ShanheRecastTacticChecks.PurchasedTiersRecastDodgedSlashAndRainDrawsAfterOutsideTurnResponse),
    ("Bound group recast: owner material counts zones and old single window", BoundCardsRecastDefinitionChecks.GroupsRequireOneOrTwoOwnedHeMaterialsAndKeepSingleCardWindow),
    ("Applied damage reflection: owner segment definition contract", AppliedDamageReflectionDefinitionChecks.ReflectionRequiresAnOwnerAppliedSegmentAndUnconditionalEffect),
    ("Shanhe applied damage reflection: separate real segments and outside-turn qualification", ShanheAppliedDamageReflectionChecks.OutOfTurnAppliedSegmentsReflectWithoutAggregatingHpLossOrSmallDamage),
    ("Shanhe applied damage reflection: final Fire segment and native armor", ShanheAppliedDamageReflectionChecks.ReflectionCopiesFrozenFinalNatureAndUsesFreshSkillDamageWithNativeArmor),
    ("Deck card gain: native filter shortfall and movement child return", DeckCardGainChecks.NativePileSelectionShortfallAndGainChildReturnKeepOneReceipt),
    ("Shanhe Fuyi: AOE supply native use and exhausted matching pool", ShanheFuyiTacticChecks.AoeSupplyObtainsAndUsesActualEntitiesUntilTheMatchingDeckIsExhausted),
    ("Shanhe Bowen tactics: three tiers native trick gains own turn only", ShanheBowenTacticChecks.ThreeTiersGainTricksAtOwnActualTurnStartsSeparatelyFromDraw),
    ("Shanhe hand stock: actual discard capacity paid Slash threshold and Cold regression", ShanheHandStockTacticChecks.NativeDiscardCapacityAndPaidColdSlashThresholdUseActualCards),
    ("Shanhe response stock: actual paid Nullification seals the response chain", ShanheResponseStockTacticChecks.PaidAbsoluteNullificationEndsItsNativeResponseChain),
    ("Shanhe cycle tactics: exact completed uses outside play and actual turn reset", ShanheCycleTacticChecks.ActualFifthSixthSeventhUsesDrawAndResetBeyondPlayPhase),
    ("Shanhe sustain opening: each damage point draws while HP loss stays separate", ShanheSustainOpeningTacticChecks.ReceivedDamagePointsDrawRealCardsWhileHpLossStaysSeparate),
    ("Shanhe boss stages: paid lethal Slash transitions and second form cold", ShanheBossStageChecks.LethalSlashRetiresPaidMaterialAndSecondFormCold),
    ("Shanhe boss stages: own-turn retaliation retires original turn cold", ShanheBossStageChecks.BossTurnRetaliationRetiresOriginalTurnCold),
    ("Shanhe boss stages: native zero maximum transforms before direct death cold", ShanheBossStageChecks.ZeroMaximumNativeReplacementStillTransformsCold),
    ("Shanhe chapter progression: native three-chapter rosters", ShanheChapterProgressionChecks.ThreeChapterRecipesStartNativeBattlesWithinEightSeats),
    ("Shanhe chapter progression: White Horse screenshot boss preview and native stats", ShanheWhiteHorseBossChecks.ChapterTwoScreenshotBossKeepsPreviewRosterAndNativeStats),
    ("Shanhe chapter progression: Hebei screenshot boss preview and native stats", ShanheHebeiScreenshotBossChecks.ChapterTwoRandomBossUsesObservedPanelAndExistingShuangxiong),
    ("Shanhe confident Lvbu: allied actual-turn Slash quota and foreign-turn cold", ShanheConfidentLvbuChecks.ActualAlliedSlashHitsGrantOneQuotaToActorAndExcludeOtherTurnsCold),
    ("Shanhe Fengxian: observed first form native second-stage equipment and cold", ShanheFengxianBossChecks.ObservedFirstFormTransitionsWithNativeEquipmentAndRecoversCold),
    ("Shanhe Dong clan: screenshot Dong Zhuo to shield Dong Min native cold", ShanheDongClanBossChecks.ScreenshotDongZhuoTransitionsToShieldDongMinWithoutCuanchaoCold),
    ("Shanhe elite Handang: real empty-slot equipment and own dodged Slash cold", ShanheEliteHandangChecks.EmptySlotUsesRealDeckEquipmentAndMengwuDrawsAfterOwnDodgeCold),
    ("Shanhe elite Chunlao: stores more than twenty native Slashes", ShanheEliteChunlaoChecks.StoresMoreThanTwentyRealSlashesWithoutAnArtificialCap),
    ("Shanhe third-chapter elite: complete roster loot and onward routes cold", ShanheThirdChapterEliteChecks.CompleteEliteRosterEarnsLootThenRejoinsFinalRoutesCold),
    ("Shanhe avatar pool: source Zuoci draws ordinary avatars with fixed seats", ShanheAvatarPoolChecks.PublishedZuociDrawsOrdinaryAvatarsWithoutChangingFixedSeats),
    ("Shanhe native skills: Mou Lumeng first gift repeated fire and growth", ShanheNativeSkillFlowChecks.MouLumengFirstGiftRepeatedFireAndGrowthUseActualCampaign),
    ("Shanhe native skills: bought Luanji and Yuce native payments recovery", ShanheNativeSkillFlowChecks.BoughtLuanjiAndYuceRunNativePaymentsAndRecovery),
    ("Shanhe skill stock: purchased Qixi conversion and Wushuang responses", ShanheSkillStockUseChecks.PurchasedQixiAndWushuangUsePhysicalConversionAndTwoDodges),
    ("Shanhe skill stock: purchased Enyuan Wangxi point payments and card gains", ShanheSkillStockChecks.BoughtEnyuanAndWangxiUsePerPointPaymentsAndRealCardGains),
    ("Shanhe skill refill stock: purchased Xunxun and Feedback", ShanheSkillRefillStockChecks.PurchasedXunxunAndFeedbackKeepNativeDeckOrderAndCardGain),
    ("Program capped refill: loader shape hashes and public AI", ProgramCappedRefillChecks.LoaderAndPublicAiHonorFixedCappedRefillGoals),
    ("Program capped refill: native target cap excess hand and existing goals", ProgramCappedRefillChecks.NativeRefillCapsCurrentMaxHpAndPreservesExcessHands),
    ("Shanhe capped refill skills: bought Jieming per-point allied five-card cap", ShanheCappedRefillSkillChecks.PurchasedJiemingOffersEachDamagePointAndCapsAlliedRefillAtFive),
    ("Shanhe Fencheng skill stock: bought three skills native NPC discard and fire", ShanheFenchengSkillStockChecks.BoughtThreeSkillsEscalateShopPricesAndFenchengUsesNativeNpcDiscardAndFire),
    ("Shanhe Niepan skill stock: bought self-dying rescue resumes lethal parent", ShanheNiepanSkillStockChecks.PurchasedNiepanClearsRealOwnedCardsAndResumesLethalDamageParentAlive),
    ("Shanhe Beige skill source: bought equipment cost Club judgment source discard", ShanheBeigeSkillChecks.PurchasedBeigePaysOnlyEquipmentBeforeClubJudgmentAndSourceNpcDiscardsTwo),
    ("Shanhe retaliation skill stock: bought Tuifeng per-point Feng and actual Slash quota", ShanheRetaliationSkillStockChecks.PurchasedTuifengStoresEachDamagePointAndCashOutAddsRealSlashUses),
    ("Shanhe shop refresh: free quota stacks before independent paid allowance", ShanheShopRefreshChecks.FreeRefreshesStackBeforeMembershipPaidAllowanceAndNeedNoCopper),
    ("Shanhe shop refresh: mid-shop gain and next-shop counter reset", ShanheShopRefreshChecks.MidShopAcquisitionAddsFreeUsesAndNextShopResetsEveryCounter),
    ("Shanhe unlimited refresh: resource school completion and twenty-five paid uses", ShanheUnlimitedRefreshChecks.CompletingResourceSchoolUnlocksMoreThanTwentyPaidRefreshes),
    ("Shanhe unlimited refresh: free priority finite membership and wallet", ShanheUnlimitedRefreshChecks.UnlimitedRefreshesKeepFreePriorityAndFiniteMembershipBoundaries),
    ("Shanhe unlimited refresh: real components stock restock and quality", ShanheUnlimitedRefreshChecks.RealEconomyComponentsParticipateInRestockAndQualityPromotion),
    ("Shanhe shop restock: perk purchase skill replacement and unique stock", ShanheShopRestockChecks.BuyingRestockAndReplacingSkillsAreAtomicAndHonorUniqueStock),
    ("Shanhe shop quality: zero forced promotion category tiers aliases", ShanheShopQualityChecks.ZeroAndForcedPromotionsKeepBaselineCategoryIsolationNearestTierAndAliases),
    ("Shanhe shop quality: promoted restock own prices free paid refresh", ShanheShopQualityChecks.PromotedRestockAndFreePaidRefreshKeepOwnPricesSupplyAndAllowances),
    ("Shanhe shop quality: actual twenty percent frozen quotes rejected queries", ShanheShopQualityChecks.ActualTwentyPercentSourcesKeepFixedSuccessFailureQuotesAndRejectedQueriesPure),
    ("Shanhe shop quality: chapter profiles exclude horses and favor later red stock", ShanheShopQualityChecks.PublishedChapterWeightsExcludeHorsesAndFavorLaterRedStock),
    ("Shanhe shop quality: chapter profiles refresh restock replay and battle markets", ShanheShopQualityChecks.ChapterWeightsApplyToRefreshRestockRecoveryAndBattleMarkets),
    ("Shanhe shop quality: balanced white exclusion Wolong schools and market migration", ShanheShopQualityChecks.BalancedWhiteExclusionWolongSchoolsAndMarketMigration),
    ("Shanhe Wangong: previous actual card paid distance quota damage", ShanheWangongChecks.PreviousActualCardControlsPaidSlashDistanceQuotaAndDamage),
    ("Shanhe Wangong: previous basic survives actual turn boundary", ShanheWangongChecks.LastActualBasicSurvivesTurnBoundaryWithoutKeepingAlcoholBonus),
    ("Shanhe player Fuqi: nearby non-targets and delayed cards", ShanhePlayerFuqiChecks.NearbyNonTargetsAndDelayedCardsRespectDistance),
    ("Shanhe player Fuqi: counterspell latest actual user distance", ShanhePlayerFuqiChecks.CounterspellUsesLatestActualUserAndDistance),
    ("Shanhe build tactics: purchased armor tiers actual opening and physique", ShanheBuildTacticChecks.PurchasedTiersEnterNativeOpeningArmorAndPhysique),
    ("Shanhe Qianlong: frozen capacity printed rank and seat isolation", ShanheQianlongChecks.FrozenCapacityIgnoresPrintedTacticAndRankPrivilegesAndOtherSeats),
    ("Shanhe Qianlong: paid live skills replacement slots and Slash ledger", ShanheQianlongChecks.LivePurchasesReplacementAndUnlockedSlotsRecomputeWithoutResettingPaidSlashes),
    ("Shanhe multi-target tricks: paid chain independent counters and recast", ShanheMultiTargetTrickChecks.PaidHengjiangUsesOneChainWithIndependentCountersAndKeepsRecast),
    ("Shanhe multi-target tricks: paid Duel native physical responses", ShanheMultiTargetTrickChecks.PaidDantiaowangKeepsOneDuelAndEachTargetsPhysicalSlashResponse),
    ("Shanhe multi-target tricks: converted Duel material and native responses", ShanheMultiTargetTrickChecks.PaidDantiaowangKeepsConvertedMaterialAndNativeDuelResponses),
    ("Shanhe Xuyan: paid reveal cost native public children and retained entity", ShanheMultiTargetTrickChecks.PaidXuyanRevealsSameSuitPaymentAndReturnsFromNativeRevealChildren),
    ("Shanhe allied tricks: ordinary identity keeps hidden roles private", ShanheAlliedTrickChecks.OrdinaryIdentityDoesNotInferAlliesFromHiddenRolesOrSharedFaction),
    ("Shanhe Yongzhan: actual rescues only enemies repeated and fatal exclusion", ShanheYongzhanChecks.RealRescueHitsOnlyEnemiesFromTheSurvivorAndRepeatsOnTheNextRescue),
    ("Shanhe Qiaoqu Haoduo: borrowed physical Slash owner and single chain bonus", ShanheQiaoquHaoduoChecks.BorrowedSwordKeepsTrickOwnerBonusOnPhysicalSlashAndOneChainPropagation),
    ("Shanhe target visibility: paid opaque slots exact entities and visible AI", ShanheTargetHandVisibilityChecks.PaidVisibilityRefreshesIssuedOpaqueSlotsAndSelectsTheExactPhysicalCard),
    ("Shanhe dismantlement stock: paid limits and optional native stopping", ShanheDismantlementStockChecks.PaidTiersKeepAbsoluteLimitsAndAllowStoppingAtOneOrTwo),
    ("Shanhe Badao: previous actual round damage native skills reflection chain prediction", ShanheBadaoChecks.PreviousRoundActualDamageControlsCardsSkillsReflectionAndChainPrediction),
    ("Shanhe steady hand: maximum health base bonuses and native discard", ShanheSteadyHandChecks.MaximumHpBasePreservesBonusesThroughRealHealthChangesAndNativeDiscard),
    ("Shanhe gambler: paid ranges deterministic income and pure quotes", ShanheGamblerChecks.PaidRangesKeepOneDeterministicIncomeAndLeaveQueriesPure),
    ("Shanhe gambler: income rounding interest and separate copper awards", ShanheGamblerChecks.PaidIncomeOrderRoundsBeforeInterestAndDoesNotTouchOtherGold),
    ("Shanhe gambler: fixed inclusive endpoints and zero income interest", ShanheGamblerChecks.FixedEndpointPurchasesIncludeZeroIncomeWithPrincipalInterest),
    ("Shanhe counterspell claim: converted materials and ordinary mode isolation", ShanheCounterspellClaimChecks.MultiMaterialConversionClaimsEveryOriginalAndOrdinaryModeStaysIsolated),
    ("Shanhe damage draw tactics: native Slash trick phase and Duel recovery", ShanheDamageDrawTacticChecks.SlashTrickPhaseAllowancesAndDuelRecoveryUseNativeDamage),
    ("Shanhe damage draw tactics: heavy hit actual amount and public enemies", ShanheDamageDrawTacticChecks.HeavyHitUsesSingleAppliedDamageAndPublicEnemyRelation),
    ("Shanhe direct skill damage: Qiangxi Fanci and actual card origins", ShanheDirectSkillDamageChecks.QiangxiFanciAndRealCardUsesRetainTheirDamageOrigins),
    ("Shanhe armor break: other participants bypass and self armor", ShanheArmorBreakTacticChecks.BypassAffectsOtherParticipantsWhileSelfDamageStillPaysArmor),
    ("Shanhe armor break: double armor overflow priority and sourceless", ShanheArmorBreakTacticChecks.DoubleArmorPreservesNativeOverflowAndBypassPriorityAndSourcelessPayment),
    ("Shanhe Woxin: outside hits prevention and public prediction", ShanheWoxinChecks.OutsideHitsAccumulateAndPreventedDamagePreservesCredits),
    ("Shanhe Woxin: nested outside-turn reflections consume distinct credits", ShanheWoxinChecks.ReflectionsConsumeNewCreditsWithinTheOriginalOutsideTurn),
    ("Shanhe market: random stock category prices refresh and cold replay", ShanheMarketChecks.RandomStockCategoryPricesRefreshAndColdReplay),
    ("Shanhe shop stock: purchases native equipment and cards", ShanheShopStockChecks.PurchasesNativeEquipmentAndCards),
    ("Shanhe Qibao: actual weapon four Slashes health armor and prediction", ShanheQibaoChecks.ManualEquipmentAllSlashKindsMatchHealthArmorAndPublicPrediction),
    ("Shanhe Qibao: skill damage and chain recipients keep native amount", ShanheQibaoChecks.DirectSkillDamageAndChainRecipientsDoNotAcquireAnotherWeaponBonus),
    ("Shanhe Baibi: four Slashes obtain actual hidden hand entities", ShanheBaibiChecks.ActualEquippedWeaponObtainsRealHandCardAfterAllFourSlashKinds),
    ("Shanhe special equipment: Yaodao paid Cold Slash judgment redirect", ShanheSpecialEquipmentChecks.YaodaoColdSlashUsesPaidNativeJudgmentAndActualRedirect),
    ("Shanhe special equipment: living target Set captured count and loader bounds", ShanheSpecialEquipmentChecks.OtherLivingTargetSetUsesCapturedCountAndRejectsWrongDefinitions),
    ("Shanhe Longhun: two black compulsory discard", ShanheLonghunChecks.TwoBlackResponsesCompulsorilyDiscardCurrentTurnCard),
    ("Shanhe Yiji: bought skill private distribution per actual damage point", ShanheYijiChecks.BoughtSkillDistributesPrivateTopCardsPerActualDamagePoint),
    ("Shanhe inheritance: frozen initial rare skill once and cold replay", ShanheOpeningSkillChecks.InitialThreeChoiceSkillIsFrozenUnlockedOnceAndColdReplayed),
    ("Shanhe inheritance: verified rare pool and old definition fingerprints", ShanheOpeningSkillChecks.VerifiedRareCatalogAndOldDefinitionFingerprintsRemainValid),
    ("Shanhe market: insufficient funds small pools and legacy fixed stock", ShanheMarketChecks.InsufficientFundsSmallPoolsAndLegacyFixedStock),
    ("Shanhe roster: ordinary expanded general real first battle cold replay", ShanhePlayerRosterChecks.OrdinaryRosterStartsRealExpandedGeneralBattleAndRestoresCold),
    ("Shanhe roster: frozen pools current defaults and forged pool rejection", ShanhePlayerRosterChecks.SavedRostersFreezeLegacyCompatibilityAndRejectForgedPools),
    ("Shanhe campaign: story shop skill replacement atomic cold journal", ShanheCampaignChecks.StoryShopSkillReplacementAndColdJournalStayAtomic),
    ("Shanhe campaign: real completed battles gate rewards and full story replay", ShanheCampaignChecks.RealCompletedBattlesGateRewardsAndReplayTheWholeStory),
    ("Shanhe campaign: seeded entry encounter and content fingerprint", ShanheCampaignChecks.SeededEntryEncounterAndContentFingerprintRemainStable),
    ("Shanhe campaign: retry shuffle resources and earlier victory rejection cold", ShanheCampaignChecks.RetriedEncountersIssueNewShuffleAndRejectEarlierVictoriesCold),
    ("Shanhe opening: automatic roster private rerolls retain fixed resources cold", ShanheCampaignChecks.NoninteractiveOpeningRerollsKeepFixedResourcesCold),
    ("Shanhe opening: chapter pools two locations preview challenge and inheritance", ShanheOpeningBattleChecks.ChapterPoolsTwoLocationsPreviewChallengeAndInheritanceAreDeterministic),
    ("Shanhe opening: both branches grant rewards and rejoin progress cold", ShanheCampaignChecks.BothOpeningBranchesGrantRewardsAndRejoinProgressCold),
    ("Shanhe stories: random recruitment variants skill offers and cold recovery", ShanheStoryEventChecks.RandomStoriesRecruitmentAndSkillOffersRecoverCold),
    ("Shanhe rewards: balanced future rewards migration and legacy cold replay", ShanheBalancedJourneyMigrationChecks.FutureRewardsUpgradeAtSafeBoundaryAndReplayOldChoices),
    ("Shanhe stories: balanced categories rare tactics legacy upgrade and cold replay", ShanheBalancedStoryChecks.CategoriesRareTacticsLegacyUpgradeAndColdReplay),
    ("Shanhe Caosong companion: native name-length draws lost-HP discard and cold", ShanheCaosongCompanionChecks.NativeCardDamageDrawsByNameAndDiscardsLostHpCold),
    ("Shanhe Chilian: opening body global domain strict hands and cold", ShanheChilianCompanionChecks.OpeningBodyAndGlobalLiangyinUseActualMovementCold),
    ("Shanhe Chilian: chained damage Jieming storage Liangyin frozen cursor cold", ShanheChilianCompanionChecks.ChainedDamageJiemingStorageAndLiangyinRetainFrozenCursorCold),
    ("Shanhe Caorui companion: native Luanxin growth foreign discard and next draw cold", ShanheCaoruiCompanionChecks.NativeGrowthForeignDiscardAndActualNextDrawRecoverCold),
    ("Shanhe companion: equipped opening native roster preview and cold replay", ShanheCompanionChecks.EquippedOpeningCompanionNativeRosterPreviewAndColdReplay),
    ("Shanhe Manzhu AI: actual enemy Dodge HP Zhaxiang first response combo", ShanheManzhuAiChecks.EnemyDodgeAiPaysHpChainsZhaxiangAndKeepsFirstResponseCursor),
    ("Huang Zhong Tianchong forward: armored native Slash Fengshang dying entry and return", AnniversaryHuangZhongTianchongChecks.ArmoredNativeSlashSecondBodyTianchongReturnsThroughFengshang),
    ("Shanhe Manzhu AI: last HP range and converted Duel boundaries", ShanheManzhuAiChecks.AiPreservesLastHpRangeAndConvertedDuelBoundaries),
    ("Shanhe companion: dual opening fixed faces remaining seats and cold replay", ShanheCompanionChecks.BothOpeningTacticsAccumulateFixedHandsAndRespectRemainingSeatsCold),
    ("Shanhe companion: complete source pools and recruitment native starts", ShanheExpandedCompanionChecks.SourceOpeningPoolsAndRecruitmentEntriesStartNativeBattles),
    ("Shanhe companion: Shunxue refills at play and ending actual hand limit", ShanheAdditionalRecruitmentBChecks.ShunxueRefillsAtPlayAndEndingWithActualHandLimit),
    ("Shanhe companion: Duanzi abolishes enemy slots with allied equipment and cold", ShanheAdditionalRecruitmentBChecks.DuanziAbolishesAllEnemySlotsAndKeepsAlliedEquipmentCold),
    ("Shanhe companion: Guangao accepted targets even-hand nullification and cold", ShanheGuangaoChecks.LegalExtraTargetsAndEvenHandNullificationRecoverCold),
    ("Shanhe companion: Yingxiang physical gift use and round-limited Qingjue cold", ShanheYingxiangChecks.PhysicalGiftUseAndRoundLimitedQingjueResumeCold),
    ("Shanhe companion: Aling Niepan restores states and draws before recovery", ShanheAlingChecks.DyingNiepanRestoresBothStatesAndDrawsBeforeRecovery),
    ("Shanhe Xiasheng: paid hand colors native draw legal targets and cold", ShanheXiashengChecks.PaidHandColorsDrawAndLegalExtraTargetsRecoverCold),
    ("Native draw threshold: native operation draw returns committed card cold", ShanheXiashengChecks.NativeDrawThresholdReturnsToCommittedCardCold),
    ("Shanhe guide: Buwu actual allied damage friendly prevention and cold", ShanheGuideSkillsChecks.BuwuUsesActualAlliedDamageAndPreventsFriendlyDamageCold),
    ("Shanhe guide: Shuanghun maximum refill living ally and cold", ShanheGuideSkillsChecks.ShuanghunRecoversMaximumRefillsMaximumAndRestoresOnlyWithLivingAllyCold),
    ("Shanhe terrain: forest true use cap response and next play phase cold", ShanheTerrainChecks.ForestTenthUseKeepsResponseAndResetsForNextActualPlayPhaseCold),
    ("Shanhe terrain: numeric policies reject malformed static shapes", ShanheTerrainChecks.TerrainNumericPoliciesRejectMalformedStaticShapes),
    ("Shanhe Mulu: four followers scoped revival state children and cold", ShanheMuluChecks.FourFollowersReviveOnlyDeadSummonerElephantsWithStateChildCold),
    ("Shanhe guide integration: stock inheritance terrain Mulu and cold", ShanheGuideIntegrationChecks.GuideStockInheritanceTerrainAndMuluEnterNativeBattlesCold),
    ("Shanhe source difficulty: verbatim tiers and independent identity", ShanheSourceDifficultyCatalogChecks.SourceTiersKeepVerbatimModifiersAndIndependentIdentity),
    ("Shanhe source growth: official composition rounding and seed order", ShanheSourceEnemyGrowthChecks.ExactSourceCompositionRoundingAndSeedOrder),
    ("Shanhe source runtime: preview armor damage conditional registry and cold", ShanheSourceDifficultyRuntimeChecks.FrozenPreviewActualArmorDamageAndConditionalBossRegistryCold),
    ("Shanhe Chanrang edict: first gains independent owners real payment and cold", ShanheChanrangEdictChecks.FirstGainIndependentOwnersForeignPayerAndEquipmentPaymentCold),
    ("OL SP Liu Bei 664: brew quota grant thunder adjustment and cold", OlSpLiuBei664Checks.RealBrewQuotaLimitedGrantAndThunderAdjustmentResumeCold),
    ("Shanhe source routes: explicit graph potential pools and immutable identities", ShanheSourceRouteCatalogChecks.ExplicitSourceGraphAndPotentialPoolsStayImmutable),
    ("Shanhe source Mou Dong Zhuo map: real source city local route and physical opening", ShanheSourceMouDongZhuoMapChecks.ActualSourceCityAndRecommendedRouteLoadCompleteNativeBossAndGear),
    ("Shanhe source Veteran: real all-hand Zhanjue Lianying and original Duel", ShanheSourceVeteranChecks.AllHandZhanjuePaysOnceReturnsLianyingAndCompletesOriginalDuel),
    ("Shanhe source Wuhuan rider: real delayed-trick Jixi Snatch and distance", ShanheSourceWuhuanRiderChecks.JixiPaysADelayedTrickEntityForOneNativeSnatchAndKeepsItsDistanceRules),
    ("Shanhe source Warrior: post-reveal gain and source damage expiry", ShanheSourceWarriorChecks.LuoyiAcceptsMixedEntitiesAndItsSourceBonusExpiresAtTheNextOwnerTurn),
    ("Shanhe source ordinary troops: source pools and both recommended entrypoints", ShanheSourceOrdinaryTroopChecks.SourcePoolsAndBothRecommendedEntrypointsRetainCompleteTieredRecipes),
    ("Shanhe source ordinary troops: new preference preview replay and legacy mixed pools", ShanheSourceOrdinaryTroopChecks.NewSourcePreferencePreservesPreviewReplayAndHistoricalMixedPools),
    ("Shanhe source scene cities: reference geography effective battles and cold", ShanheSourceOrdinaryTroopChecks.SceneCityBindingsKeepReferenceGeographyAndEffectiveBattlesConsistentCold),
    ("Shanhe source ordinary troops: real preview move opening projection", ShanheSourceOrdinaryTroopChecks.SourceAndLocalPreviewMoveAndStartUseOneProjectionAndRealOpeningEntities),
    ("Shanhe source mixed troops: complete member recipes and both recommended routes", ShanheSourceMixedTroopChecks.CompleteMixedSourceRecipesAndBothRecommendedRoutes),
    ("Shanhe source mixed troops: real opening grain and independent skill draws", ShanheSourceMixedTroopChecks.MixedPreviewMoveAndNativeOpeningKeepMembersAndSeparateSkillDraws),
    ("Shanhe source Jielue: native regional gifts return before HP loss", ShanheSourceJielueChecks.ActualRegionsReturnThroughMovementChildrenBeforeHealthLossAndEmptyTargetsStillCharge),
    ("Shanhe source demon archer: actual Youyan phases and Saogu ending payment", ShanheSourceDemonArcherChecks.GongqiAndActualDiscardTriggerYouyanButSaoguEndingReturnsWithoutIt),
    ("Shanhe source Weizhong: nonempty least hand draw and original Bingjie return", ShanheSourceMaRiDiChecks.ActualMaximumHpChangeFreezesLeastHandDrawAndReturnsThroughBingjie),
    ("Shanhe source war elephant: trick range Slash damage and turnover protection", ShanheSourceWarElephantNativeChecks.RealTrickRangeAllSlashDamageAndTurnoverProtection),
    ("Shanhe source war elephant: first large damage counter returns original attack", ShanheSourceWarElephantNativeChecks.FirstLargeDamagePerActualTurnCounterSlashReturnsOriginalAttack),
    ("Shanhe source Naman: completed physical and converted Slash materials return to parent", ShanheSourceNamanChecks.ActualCompletedSlashClaimsPhysicalAndConvertedMaterialsAndReturnsToParent),
    ("Shanhe source Zhaoran: public Play hand suit rewards and phase reset", ShanheSourceZhaoranNativeChecks.ActualPlayPublicHandSuitQuotaDrawDiscardAndPhaseReset),
    ("Shanhe source armory: actual slot payments Tuxing and exact treasure Jizhi", ShanheSourceArmoryCommanderChecks.ActualSlotPaymentsKeepTuxingRecipientAndGrantExactTreasureJizhi),
    ("Shanhe source Liu Sandao: actual enemy kill draw awakening weapons and ice Slash", ShanheSourceLiuSandaoChecks.ActualEnemyKillsFeedNormalDrawAndDeadAllyAwakeningReturnsIceSlash),
    ("Shanhe source Liu Sandao: non-Wusheng conversions preserve Nuzhan material rules", ShanheSourceLiuSandaoChecks.NonWushengConversionsKeepNuzhanMaterialsAndNaturalSlashUntouched),
    ("Shanhe source Jiangdong trio: actual Lihuo first damage and Chunlao judgment draw", ShanheSourceJiangdongTrioChecks.Source3160FirstDamagePaymentAnd3161DiscarderWineJudgmentDraw),
    ("Shanhe source Qingjian: repeated batches original cards actual give children and Draw owner", ShanheSourceQingjianNativeChecks.RepeatedBatchesOriginalEntitiesRealGiveChildrenAndDrawOwnerQualification),
    ("Shanhe source Qingjian: earlier gain child leaves only remaining original cards", ShanheSourceQingjianNativeChecks.EarlierGainChildRemovalLeavesOnlyTheRemainingOriginalBatchCards),
    ("Shanhe source Cao Song: enemy HP and paid maximum child before negative HP rescue", ShanheSourceCaoSongTrioChecks.Source665EnemyHpAnd1602PaidMaximumChildBeforeNegativeHpRescue),
    ("Shanhe source Tiepao: actual material return directional distance and owner turn expiry", ShanheSourceTiepaoNativeChecks.ActualMaterialRetaliationReturnsAndDirectionalDistanceExpiresAtOwnerStart),
    ("Shanhe source Duanjia: real armor placement and existing empty-slot capacity", ShanheSourceDuanjiaChecks.MissingArmorCreatesNativeEntityOutsideDrawPileAndPreservesSlots),
    ("Shanhe source Weizhu: real hand payment before ally protection", ShanheSourceWeizhuNativeChecks.OtherTeamDamageWaitsForActualHandPaymentAndNeverProtectsSelfOrEnemy),
    ("Shanhe source Liu Yan: team distance and limited actual pindian directional Duel", ShanheSourceLiuYanTrioChecks.Source1161TeamDistanceAnd4055LimitedActualPindianDirectionalDuel),
    ("Shanhe source Zirun: native preparation draws wait for children and current equipment", ShanheSourceZirunNativeChecks.ActualPreparationDrawsWaitForChildrenAndReadEachRecipientsCurrentEquipment),
    ("Shanhe source Beigong: original Dingxi entities and real Huojie judgments", ShanheSourceBeigongNativeChecks.OriginalDingxiEntitiesReturnAndHuojieContinuesPastPreventedLightningUntilActualDamage),
    ("Shanhe source Xingxia: real red payments and two-round spacing", ShanheSourceXingxiaNativeChecks.OwnPlayStartWaitsForRealRedHandAndEquipmentPaymentsAndTwoRoundSpacing),
    ("Shanhe source grain: actual fire loss and equipment sharing with true teammates", ShanheSourceGrainChecks.ActualFireDamageConsumesSourceGrainAndSushouSharesEquippedCardsOnlyWithTrueTeammates),
    ("Shanhe source White Horse: paid Xiying native bans and actual ending reward", ShanheSourceWhiteHorseChecks.PaidXiyingDiscardsBansNativeUseAndResponseAndRewardsAtActualEnding),
    ("Shanhe source Qianxi: real mount distance and original judgment hand prohibition", ShanheSourceQianxiChecks.RealHorseDistanceJudgmentRestrictsTheChosenTargetsBlackHand),
    ("Shanhe source agriculture: actual response loss and shared original judgment", ShanheSourceAgricultureOfficerChecks.ActualOffTurnDodgeLossJudgesOnceAndSharesTheOriginalJudgmentWithTiandu),
    ("Shanhe source phases: Jushou after discard and native equipment use", ShanheSourcePhaseChecks.JushouRunsAfterHandLimitDiscardAndUsesTheOriginalEquipmentAtTurnEnding),
    ("Shanhe source Keshou: real same-colour HE cost damage and faction judgment", ShanheSourceKeshouChecks.SameColourHePaymentRedJudgmentAndCurrentFactionGateUseTheOriginalDamageFrame),
    ("Shanhe source Xun Yue: native shown card and exact Play phase lifetime", ShanheSourceXunYueChecks.QiangzhiRevealsOneOriginalHandCardAndOnlyDrawsWithinItsSelectedPlayPhase),
    ("Shanhe source Yidu: sole target reveal and unchanged OL multi-target", ShanheSourceYiduChecks.SoleTargetCompletesNativeRevealWhileOlRetainsMultipleTargets),
    ("Shanhe Huxin Mirror: actual armor payment and finalized damage eligibility", ShanheHuxinMirrorChecks.ActualArmorPaymentPreventsLargeOrLethalDamageAndHonoursNativeArmorBypass),
    ("Shanhe source executioner: both real wagers and multi-target Slash settlement", ShanheSourceExecutionerChecks.HuanfuPaysBothNativeWagersAndSettlesTheSameMultiTargetSlash),
    ("Shanhe Zhaogu Mirror: ordinary trick native draw and counterspell return", ShanheZhaoguMirrorChecks.RevealedOrdinaryTrickUsesRealDrawAndPaidCounterspellBranches),
    ("Shanhe source Mou Dong Zhuo: real five-card cost maximum HP and dying allocation wine", ShanheSourceMouDongZhuoChecks.FiveSuitPaymentMaximumHpAndUnrestrictedDyingAllocationUseOriginalChildrenAndWine),
    ("Shanhe Duanhun: real source damage self damage and armor equipment", ShanheDuanhunChecks.RealWeaponBypassesOtherAndSelfArmorWhileLeavingArmorEquipmentEffective),
    ("Shanhe Kunlun mirror: real damage names and equipment lifetime", ShanheKunlunMirrorChecks.FirstRealTrickLearnsItsNameAndTreasureMemorySurvivesRemovalAndDifferentEntity),
    ("Shanhe returning reinforcements: both entries local tiers and terrain routes", ShanheReturningReinforcementChecks.BothLocalEntriesSelectCompleteThreeTierProfilesAndCarryThemIntoTerrainRoutes),
    ("Shanhe source garrisons: native preview move and physical opening", ShanheSourceGarrisonChecks.NativePreviewMoveAndStartKeepTheChosenRosterAndPhysicalOpening),
    ("Shanhe wolf battles: source chapters elite growth and reward planning", ShanheWolfBattleChecks.PublishedSourceChaptersUseEliteGrowthAndActualRareRewardPools),
    ("Shanhe battle: NPC opening equipment native entities and skills", ShanheBattleChecks.NpcOpeningEquipmentInstallsEntitiesAndRunsNativeSkills),
    ("Shanhe source boss tiers: native forms enemy-scaled Shenwei and own-play Shenji", ShanheSourceBossTierChecks.Source2644NativeFormsCountPublicEnemiesAndRestrictShenjiToOwnPlay),
    ("Shanhe source stages: exact tier forms and cumulative equipment", ShanheSourceStageEquipmentChecks.SourceThresholdsResolveExactFormsAndCumulativeStageEquipment),
    ("Shanhe source Zhangyan: medium map lethal stage and native turn", ShanheSourceZhangYanStageChecks.Source2653MediumRouteLethalTransitionStartsTheRealZhangYanTurn),
    ("Shanhe source Zhangyan: higher cumulative skills physical armor and native stage", ShanheSourceZhangYanHighStageChecks.HigherSource2653KeepsCumulativeSkillsAndRealSanshouAcrossItsNativeStage),
    ("Shanhe source Dongdang: native opening entry interrupts and starts boss first", ShanheSourceDongdangChecks.ActualOpeningEntryConsumesItsNativeInstanceAndStartsBossFirst),
    ("Shanhe source Pojun: whole-hand hold and actual turn return", ShanheSourcePojunChecks.NativeSlashHoldsEveryHandCardDrainsMovementAndReturnsAtRealTurnEnd),
    ("Shanhe source Quhu: original opponent capped damage and native contest", ShanheSourceQuhuChecks.WinningOpponentDealsItsCappedMaximumHpAndNonWinsRetainOriginalDamageSource),
    ("Shanhe source Hunzi: native ending recovery and awakened maximum", ShanheSourceSunCeChecks.EndingRecoveryRequestsTwoAndRespectsTheActualAwakenedMaximum),
    ("Shanhe source reinforcements: effective initial Dongdang and first native turn", ShanheSourceReinforcementChecks.InitialDongdangUsesItsEffectiveOwnerBeforeTheFirstNaturalTurn),
    ("Shanhe black light armor: native multi-target immunity and actual equipment", ShanheBlackLightArmorChecks.ActualArmorNullifiesOnlyItsOwnMultiTargetSlashDamageAndBlackTrickEffects),
    ("Shanhe Gouxiang armor: matching actual Dodge responses and physical armor effectiveness", ShanheGouXiangArmorChecks.ActualMatchingDodgeStealsOncePerResponseAndHonoursArmorEffectiveness),
    ("Shanhe source Shennu stage: actual high map native transformation and gear", ShanheSourceShenNuStageChecks.ActualSource2687EntersItsTwentyHpShenNuStageWithCumulativeGear),
    ("Shanhe source Shennu: actual damage source HP draws and owned anger settlement", ShanheSourceShennuChecks.ActualDamageEarnsOneNuDrawsRealSourceHpAndOnlyCompletedOutgoingDamageClears),
    ("Shanhe Sanshou armor: physical grant effectiveness removal and printed skill", ShanheSanshouArmorChecks.PhysicalArmorGrantPreventsOnlyWhileEffectiveAndLeavesPrintedSanshouAlone),
    ("Shanhe Sanshou market: source price paid hand card and native equip during play", ShanheSanshouMarketChecks.PaidSourceArmorJoinsCurrentHandThenEquipsThroughItsNativeUse),
    ("Shanhe source Biri: fire prevention atomic recovery and child returns", ShanheSourceBiriChecks.FirePreventionUsesAtomicRecoveryResultBeforeHealthAndDrawChildren),
    ("Shanhe source Yanyu: locked exact red black judgments and three limit", ShanheSourceYanyuChecks.LockedRedAndBlackUseExactJudgmentsAndStopAtThree),
    ("Shanhe mobilization: dead allies revive at true new rounds and act naturally", ShanheMobilizationChecks.DeadAlliesReviveAtRealNewRoundAndRetainNaturalTurnsWithoutCards),
    ("Shanhe source campaign: three chapters native stages city receipts and cold", ShanheSource2026Checks.ThreeSourceChaptersNativeStagesCityReceiptsAndColdRecovery),
    ("Shanhe chapter route: six mainline steps native cities and boss exits", ShanheCampaignChecks.SourceChaptersPlaceTenStopsAndKeepTheirNativeEnemyCities),
    ("Shanhe definition reuse: frozen keys external validation and source templates", ShanheCampaignChecks.FinalDefinitionsReuseFrozenKeysAndKeepExternalValidation),
    ("Shanhe chapter mainline: six steps market gate optional battles and cold", ShanheCampaignChecks.SixMainlineStepsGateMarketAndLeaveOptionalBattlesSkippableCold),
    ("Shanhe daily travel: extra victories tier upgrade and cold chapters", ShanheCampaignChecks.DailyTravelExtraBattlesPayOnceUpgradeAndRecoverAcrossChapters),
    ("Shanhe daily travel: market upgrade reconciles frozen rewards cold", ShanheCampaignChecks.MarketTravelUpgradeExhaustsFrozenRewardsWithoutBlockingTheRouteCold),
    ("Shanhe guide tasks: typed source evidence bounded progress and cold", ShanheGuideTaskProgressChecks.TypedSourceEvidenceAccumulatesWithoutLocalDifficultyOrDuplicateCredit),
    ("Shanhe campaign: independent tactic grants alias rejection and cold setup", ShanheCampaignChecks.TacticsUseIndependentGrantsAndRejectAliases),
    ("Shanhe campaign: repeated acquisition identities hashes boss groups and all instances cold", ShanheRepeatableTacticChecks.CampaignRepeatedAcquisitionsRespectIdentityHashAndKeepAllInstancesCold),
    ("Shanhe campaign: decline frozen rewards exact gold once and cold journal", ShanheCampaignChecks.DecliningPublishedRewardsPaysOnceAndReplaysWithoutReroll),
    ("Shanhe codex: initial tactics frozen grants validation and cold replay", ShanheCodexLoadoutChecks.InitialTacticsValidateFreezeGrantAndReplayWithoutChangingContent),
    ("Shanhe codex: locked skill equipment pools and forged commands", ShanheCodexLoadoutChecks.LockedSkillsAndEquipmentStayOutOfAllRewardPathsAndForgedCommands),
    ("Shanhe codex: old options unrestricted cold checkpoint replay", ShanheCodexLoadoutChecks.OldOptionsRemainUnrestrictedThroughColdCheckpointReplay),
    ("Shanhe codex content: editions checkpoint pool and native equipment", ShanheCodexContentChecks.EditionsCheckpointChoiceAndSpecialEquipmentStayPlayable),
    ("Shanhe tactic: simultaneous ranks and opponent AI use same bonus cold", ShanheTacticPindianChecks.SimultaneousRanksAndOpponentAiUseTheSameBonusCold),
    ("Shanhe S5 Zhangliao: planned draw smaller hands native Pojun and cold return", ShanheS5ZhangliaoChecks.PlannedDrawTakesThreeSmallerHandsThenRealPojunReturnsCold),
    ("Shanhe Lingdong: Tiandu claim movement frozen suit and replacement bypass cold", ShanheLingdongChecks.NativeTianduKeepsClaimedEntityWhileReplacementsBypassAndRealDamageJudgesCold),
    ("Shanhe Huangkong: planned draw Yingzi team aura real Tuxi and Hongde cold", ShanheHuangkongChecks.PlannedDrawDoublesAfterYingziAndTeamAuraBeforeRealTuxiCold),
    ("Shanhe Huangkong: doubled discard quota actual payment and child return cold", ShanheHuangkongChecks.DoubledDiscardQuotaPaysFourWithoutChangingHandLimitAndResumesChildrenCold),
    ("Shanhe Huangkong: counted exemptions protected cards skipped phases and NPC quota", ShanheHuangkongChecks.DiscardExemptionsProtectedCardsSkippedPhaseAndNpcUseSameQuota),
    ("Shanhe S5 Dong Zhuo: two scenario owners per point Armor recovery and claim cold", ShanheS5DongZhuoChecks.TwoQualifiedScenarioBaonueOwnersJudgeEachDamagePointThroughFullArmorCold),
    ("Shanhe god faction: actual god general fallback public views native override and Baonue cold", ShanheGodFactionChecks.RealGodGeneralUsesScenarioFallbackForPublicFactionAndNativeBaonueCold),
    ("Shanhe god faction: null input hash session propagation and tampered battle rejection", ShanheGodFactionChecks.DefaultGodFactionPolicyFreezesHashesPassesSessionAndRejectsTamperedBattle),
    ("Shanhe team Slash: paid use holder death loss and future quota recheck cold", ShanheTeamSlashChecks.PaidSlashSurvivesHolderDeathOrLossWhileFutureQuotaRechecksCold),
    ("Shanhe Dong Min: fixed cap before Armor public prediction and replacements cold", ShanheS5DongMinChecks.NativeFixedCapPrecedesArmorAndPublicPredictionCold),
    ("Shanhe Dong Min: elemental recipients source-less and live caps cold", ShanheS5DongMinChecks.ElementalChainAndLiveCapSourcesRecheckWithoutInflatingLowerDamageCold),
    ("Shanhe Cuanchao: all recipients extra rounds and delayed placement cold", ShanheDongMinCuanchaoChecks.WholeUseStateCancelsAllRecipientsRoundsAndDelayedPlacementCold),
    ("Shanhe Far Strike: native program final distance armor public prediction and cold", ShanheFarStrikeChecks.NativeAndProgramDamageUseFinalDirectedDistanceArmorAndPublicPredictionCold),
    ("Shanhe Liyu: whole regions actual gain receipts printed categories and cold", ShanheLiyuChecks.WholeRegionsUseActualObtainReceiptsAndPrintedCategoryCold),
    ("Shanhe Biyue: empty Hand live limit native gain children and cold", ShanheBiyueChecks.EmptyHandRefillsLiveLimitAndNativeGainChildrenCold),
    ("Shanhe Lijian: real ordered Duel use windows and physical receipts cold", ShanheLijianChecks.RealOrderedDuelRunsUseWindowsAndPaysOriginalEntitiesCold),
    ("Shanhe S5 Roulin: any gender physical double responses and maximum policy cold", ShanheS5RoulinChecks.AnyGenderBidirectionalSlashResponsesUseTwoDistinctDodgesAndMaximumPolicyCold),
    ("Shanhe S5 Zaiqi: replacement eligibility codex purchase and conditional recipes cold", ShanheS5ZaiqiChecks.FullHpAndDeclinedReplacementKeepNormalDrawAndVerifiedCodexReward),
    ("Shanhe S5 Kanpo: purchased HE native second layer and fresh target windows cold", ShanheS5KanpoChecks.PurchasedHeAndNativeCounterspellsKeepSecondLayerAndFreshTargetWindowsCold),
    ("Shanhe Jinnang Ji: replacement gains versus real draw phase skill draws cold", ShanheJinnangJiChecks.ReplacementGainsDoNotCountButRealDrawPhaseSkillDrawsDoCold),
    ("Shanhe Jinnang Ji: multiple real phases turn end and scoped schema cold", ShanheJinnangJiChecks.MultipleRealDrawPhasesAccumulateAndTurnEndClearsWithScopedSchemaCold),
    ("Shanhe Gaichao: real single enemy source health draw and conditional packages cold", ShanheFengyiEncounterChecks.ActualGaichaoSingleEnemyRecipeKeepsSourceHealthDrawAndConditionalPackagesCold),
    ("Shanhe Duoduo Yishan: captured eligibility late sources and paid children cold", ShanheDuoduoYishanChecks.CapturedDrawEligibilityRejectsLateSourcesAndPreservesPaidChildrenCold),
    ("Shanhe Yaoli: six versions real Peach recovery ordinals and actual turns cold", ShanheYaoliChecks.RecoveryCallsStackSixVersionsAndRealPeachesResetAtActualTurnsCold),
    ("Shanhe Yaoli: live sources paid children and zero recovery cold", ShanheYaoliChecks.LiveSourcesAndPaidChildrenPreserveEligibilityAndZeroRecoveryCold),
    ("Shanhe Yaoli: Zaiqi Kuanggu blood blade and recovery draw one payment cold", ShanheYaoliChecks.ZaiqiKuangguBloodBladeAndRecoveryDrawUseOneModifiedPaymentCold),
    ("Shanhe Yaoli: redirected recovery and Yizheng final recipient once cold", ShanheYaoliChecks.RedirectedRecoveryAndYizhengSupportApplyFinalRecipientPoliciesOnceCold),
    ("Shanhe Dong Ji: black locks exact dynamic discard or draw cold", ShanheDongJiChecks.BlackGiftLocksExcludePaymentAndExactDynamicDiscardOrDrawCold),
    ("Shanhe Dong Ji: frozen HP equipment children and paid source invalidation cold", ShanheDongJiChecks.FrozenRecipientHpSurvivesEquipmentChildrenAndPaidSourceInvalidationCold),
    ("Shanhe Dong Ji: actual source pair encounter and edition isolation cold", ShanheDongJiChecks.ActualDongshiEncounterUsesSourcePairAndIsolatesEditionsCold),
    ("Shanhe first Slash: declarations misses responses conversions and virtual uses cold", ShanheFirstSlashChecks.DeclarationsCountMissResponsesConversionsAndVirtualUsesCold),
    ("Shanhe Chengxiong: native elemental propagation and live source loss cold", ShanheChengxiongChecks.NativeElementalPropagationRechecksEachRecipientAndLiveSourceLossCold),
    ("Shanhe Caoxing: continuous HE topdeck native Slash responses and damage markers cold", ShanheCaoxingChecks.ContinuousHeartHeTopdeckUsesRealSlashCostsResponsesAndDamageMarkersCold),
    ("Liushi effect contract: rejects unpaid foreign and wrong-window definitions", LiushiEffectValidationChecks.RejectsUnpaidForeignAndWrongWindowDefinitions),
    ("Shanhe Shidu: paid source loss target death and isolated actual recipes cold", ShanheShiduChecks.PaidObtainSourceLossTargetDeathAndActualRecipesStayIsolatedCold),
    ("Shanhe Shihun: direct native Slash current skill pool distinct grants and cold", ShanheShihunChecks.DirectSlashAcquiresCurrentEligibleSkillOncePerDamageAndReplaysCold),
    ("Shanhe Shihun: chain prevention replacements source loss and persistent grants cold", ShanheShihunChecks.DodgedChainedAndReplacedDamageDoNotAcquireSkillsAndPaidSourceLossKeepsGrant),
    ("Shanhe Jiaozi: Shidu live hand leadership damage Armor and half return cold", ShanheShihunChecks.ShiduHandLeadershipChangesJiaoziDamageBeforeAndAfterHalfReturnCold),
    ("Shanhe Huaxiong: paired health Duel and actual isolated recipes cold", ShanheHuaxiongChecks.PairedYaowuDuelInteractionsAndActualRecipesStayIsolatedCold),
    ("Shanhe Taishici: sole incoming repeats before live range draw cold", ShanheTaishiciChecks.IncomingSoleCardRepeatsBeforeLiveRangeDrawAndReplaysCold),
    ("Mou Taishi Ci OL: fixed one native Duel initial population capped permanent growth", CardGame.Core.Tests.MouTaishiCiOlChecks.OneActualDuelOrInitialPopulationCappedPermanentGrowth),
    ("Mou Lu Xun Mobile: physical Wenhe Chaos participant effect and exact parent cold return", CardGame.Core.Tests.MouLuXunMobileChecks.RealWenheChaosParticipantRecordsAndReturnsItsPhysicalParent),
    ("Mobile Shen Cao Cao: Guixin chosen areas per point and paid child cold", MobileShenCaoCaoChecks.GuixinSelectsAreasPerDamagePointAndColdReturnsOnce),
    ("Mobile Shen Lu Bu: ending damage card gain original paid frame cold", CardGame.Core.Tests.MobileShenLuBuChecks.EndingDamageCardGainKeepsOriginalPaidFrameCold),
    ("Mobile Shen Sima Yi: unanswered native windows round cap and faction awakening cold", CardGame.Core.Tests.MobileShenSimaYiChecks.UnansweredNativeWindowsRoundCapAndFactionAwakeningCold),
    ("OL Shen Zhen Ji: odd native dying death repeats and survivor stops cold", CardGame.Core.Tests.OlShenZhenJiChecks.OddNativeDyingDeathRepeatsAndSurvivorStopsCold),
    ("OL Shen Cao Pi: two actual awakenings obtain Chu permanent skill and cold", CardGame.Core.Tests.OlShenCaoPiChecks.TwoActualAwakeningsObtainChuAndGrantPermanentSkillCold),
    ("anniversary Sun Quan private Ming native copy and random surge cold returns once", AnniversaryShenSunQuanChecks.PrivateMingNativeCopyAndRandomSurgeColdReturnsOnce),
    ("OL2016 Shen Zhao Yun: live HP costs native equipment use response cold recovery", CardGame.Core.Tests.Ol2016ShenZhaoYunChecks.LiveHpCostsNativeEquipmentUseResponseAndColdRecovery),
    ("OL2016 Shen Sima Yi: actual Discard phase original Jilue paid draw and Lianpo cold", CardGame.Core.Tests.Ol2016ShenSimaYiChecks.ActualPhaseDiscardsOriginalJiluePaidDrawAndLianpoCold),
    ("OL Shen Cao Cao: priority regions fallback random damage point cold", CardGame.Core.Tests.OlShenCaoCaoChecks.PriorityRegionsFallbackRandomAndDamagePointCold),
    ("Anniversary Shen Zhang Liao Cuixi paid cold and temporary equipment phases", AnniversaryShenZhangLiaoChecks.CuixiPaidColdAndTemporarySlotsUseActualPhases),
    ("Mobile Shen Ma Chao: real nature markers Jimie both Yuli recharge cold", CardGame.Core.Tests.MobileShenMaChaoChecks.RealNatureMarkerJimieAndBothYuliRechargeCold),
    ("Mobile Shen Jiang Wei: Tiantao Shenpei Huitian real native returns cold", MobileShenJiangWeiChecks.TiantaoShenpeiAndHuitianUseRealNativeReturns),
    ("Shanhe Caocao Guixin: independent damage points random areas and turnover cold", ShanheCaocaoGuixinChecks.DamagePointsChooseIndependentNonemptyAreasAndTurnOverCold),
    ("Shanhe Yujin: frozen preparation printed types and exact owner payment cold", ShanheYujinChecks.FrozenPreparationDiscardsUsePrintedNonEquipmentAndExactOwnerPaymentCold),
    ("Shanhe Yujin: immediate native draw gain children and paid source loss cold", ShanheYujinChecks.ImmediateTargetDrawUsesNativeAttemptsGainChildrenAndPaidSourceLossCold),
    ("Shanhe Yujin: legacy ending timing and isolated actual recipes cold", ShanheYujinChecks.LegacyEndingTimingAndActualRecipesStayIsolatedCold),
    ("Shanhe Yudu: final preparation judgment poison loss before native Niepan cold", ShanheYuduChecks.FinalPreparationJudgmentRemovesOrPaysPoisonBeforeNativeNiepanCold),
    ("Shanhe Liru: actual single enemy recipe conditional registry and recovery", ShanheLiruEncounterChecks.ActualSingleEnemyRecipeAndConditionalRegistryRecover),
    ("Shanhe encounters: source recipes conditional packages and battle recovery", ShanheEncounterChecks.ActualEncounterRecipesAndConditionalPackagesRecoverCold),
    ("Shanhe S5 Sima Yi: four Ren actual damage and phase discards original Jilue cold", CardGame.Core.Tests.ShanheS5SimayiChecks.FourRenPerActualPointAndPhaseDiscardPayOriginalJilueColdWithoutAwakeningCost),
    ("Shanhe S5 Qiangxi: actual round scaling native costs Ninge and cold", ShanheS5QiangxiChecks.ActualRoundScalingTwoPhaseCostsNingeAndCold),
    ("Shanhe S5 Xiahou: Shensu actual phase costs frozen count other source and cold", ShanheS5XiahouPairChecks.ShensuThreeNativeCostsFrozenActualSkipsAndOtherSourceDedupCold),
    ("Shanhe S5 Zuoci: three initial two exchanged and native damage libraries cold", CardGame.Core.Tests.ShanheS5ZuociHuatuoChecks.ThreeInitialTwoExchangedAndThreePerActualDamagePointUseNativeLibraryCold),
    ("Shanhe S5 Zuoci: exhausted pool zero exchange preserves displayed avatar cold", CardGame.Core.Tests.ShanheS5ZuociHuatuoChecks.ExhaustedAvatarPoolKeepsDisplayedCardAndAllowsZeroExchangeCold),
    ("Shanhe S5 Cao Wei: Jianxiong native lost HP draw original claim and cold", ShanheS5CaoWeiPioneersChecks.JianxiongLostHpDrawBeforeOriginalClaimAndCardlessCold),
    ("Shanhe S5 Cao Wei: Hujia actual Used Played chooser quota and cold", ShanheS5CaoWeiPioneersChecks.HujiaActualUsedPlayedForeignChooserSharedTurnQuotaAndCold),
    ("Shanhe S5 Zhangxiu: native areas paid children and repeated abolished judgment cold", ShanheS5ZhangxiuChecks.XiongluanRealAreaCostsRecoveryChildrenAndAlreadyAbolishedJudgmentCold),
    ("Shanhe S5 Xingtian: native weapon cost all response bans armor and expiry cold", ShanheXingtianChecks.FixedHandRealEquipWeaponCostAndAllResponsesSealOtherSourceArmorUntilActualTurnEndCold),
    ("Shanhe S5 Xingtian: sealed target no repeated offer other target and next turn cold", ShanheXingtianChecks.RetainedAxeDoesNotReofferSealedTargetButOtherTargetsAndNextTurnRemainLegalCold),
    ("Shanhe S5 Xingtian forward: sealed target no repeated offer other target and next turn", ShanheXingtianChecks.RetainedAxeDoesNotReofferSealedTargetButOtherTargetsAndNextTurnRemainLegalForward),
    ("Shanhe Xingtian forward: self target excluded and other target retained", ShanheXingtianChecks.SelfTargetUseNeverOffersEitherAxeAndOtherTargetsStillResolve),
    ("Shanhe S5 Xingtian: original special axe retains hand-only and directed armor contract", ShanheXingtianChecks.ExistingSpecialAxeStillOnlyBlocksHandAndHolderArmorBypass),
    ("Shanhe NPC opening: random four plus fixed equipment native cold", ShanheNpcStartingCardChecks.RandomFourPlusFixedEquipmentOrdinaryLifecycleAndRecipeCold),
    ("Shanhe S5 Li Jue: every actual prepare positive random damage and paid child cold", ShanheS5LiJueChecks.EveryActualPreparePositiveRandomDamageAndPaidChildCold),
    ("Shanhe Zhanhun Dao: real equipped native and cardless damage with cold health child", ShanheZhanhunDaoChecks.RealEquippedWeaponPreventsNativeAndCardlessDamageWithColdHealthChild),
    ("Shanhe Zhanhun Dao: actual self source qualifies but source less does not", ShanheZhanhunDaoChecks.ActualSelfSourceQualifiesButSourceLessDamageDoesNot),
    ("Shanhe S5 Dongwu veterans: nine-card cap and native wine cold", ShanheS5DongwuVeteransChecks.NineCardCapRechecksEachDiscardAndRefillsAfterNativeWineCold),
    ("Shanhe S5 Moushi: actual next Play paid gift and native draw children cold", ShanheS5MoushiChecks.MoushiActualNextPlayKeepsPaidGiftAndNativeDrawChildrenCold),
    ("Shanhe S5 Ma siblings: whole-color Tieqi actual judgment HE payment and native Dodge cold", ShanheS5MaSiblingsChecks.ColorAllCostJudgmentSuppressionAndNativeDodgeCold),
    ("YuWei actual turn: preparation native uses qualified double Shiyuan and cold children", YuweiActualTurnChecks.PrepareNativeUsesDoubleQualifiedShiyuanAndColdChildren),
    ("Native Cold: physical Slash existing family double Dodge bonus and elemental chain", NativeColdDamageChecks.PhysicalSlashUsesOldFamilyDoubleDodgeBonusAndElementalChain),
    ("Native Cold: sequential replacement paid Silver Lion recovery and cold return", NativeColdDamageChecks.SequentialReplacementWaitsForPaidSilverLionRecoveryAndRestores),
    ("Anqi Feiren: real equipment random suppression target quota expiry and cold", AnqiFeirenChecks.EquippedNativeDamageRandomSkillQuotaTargetTurnExpiryAndCold),
    ("Shanhe S5 Yimie: frozen X native cost damage and recovery children cold", ShanheS5YimieChecks.FrozenXNativeCostDamageAndRecoveryChildrenKeepOriginalFramesCold),
    ("Shanhe S5 Yimie: prevention and nonpositive X preserve cost without negative effects", ShanheS5YimieChecks.PreventionAndNonPositiveXKeepPaidCostWithoutNegativeDamageOrRecovery),
    ("Shanhe S5 Xu Rong: native equipment rejudge claim legal redirect and red outcome cold", ShanheS5XuRongChecks.WeaponNativeEquipRejudgeClaimLegalRandomTargetAndRedOutcomeCold),
    ("Shanhe S5 Xu Rong: Shajue frozen negative entry pool and native causal claim cold", ShanheS5XuRongChecks.ShajueFrozenNegativeEntryGatesBothPoolAndCausalClaimCold),
    ("Shanhe S5 Xuncan: both native Cold directions and living Fenchai cold", ShanheS5XuncanCaoshiChecks.YunshenBothNativeColdDirectionsAndLivingFenchaiRecoverCold),
    ("Shanhe Yuanshu: Tongji actual range hand threshold native virtual Slash cold", ShanheS5YuanshuChecks.TongjiActualRangeHandThresholdAndNativeVirtualSlashRecoverCold),
    ("Shanhe Taoyuan: repeatable Tishen actual recovery receipt cold", ShanheS5TaoyuanChecks.RepeatableTishenDrawsActualRecoveryAcrossPaidChildCold),
    ("Shanhe Gongsun: native range three area discard before Dodge paid cold", ShanheS5GongsunChecks.NativeSlashRangeThreeAreaDiscardBeforeDodgeAndPaidCold),
    ("Shanhe Gongsun: repeated ally rescue paid cold and qualification", ShanheS5GongsunChecks.JiuzhuActualRepeatedAllyRescuePaidColdAndQualification),
    ("Shanhe Zhanghe: uncapped persistent Whirl and large native draw cold", ShanheS5ZhangheChecks.UncappedPersistentPileAndLargeNativeUseDrawResumeCold),
    ("Shanhe Quyi: distant Duel response and actual counterspell user cold", ShanheS5QuyiChecks.FuqiDistantDuelAndActualCounterspellUserRecoverCold),
    ("Shanhe S5 Shuangxiong: round native Duel", ShanheS5YanliangWenchouChecks.ExtraDrawRoundCountNativeDuelAndNextRoundReset),
    ("Shanhe S5 Shuangxiong: back damage and paid cold", ShanheS5YanliangWenchouChecks.PaidChildColdNativeBackDamageAndCausalEndingClaim),
    ("Shanhe Zhuling: three conditions native Slash frozen draw and original target penalty cold", ShanheZhulingChecks.ThreeConditionsNativeSlashFrozenDrawAndOriginalTargetPenaltyCold),
    ("Shanhe Zhuling: all source damage native cardless source-less HP loss and NPC flow cold", ShanheZhulingChecks.BaoliNativeAndCardlessSourceDamageExcludeSourceLessAndHpLossAndNpcUsesJixian),
    ("Shanhe Yuanshao: same color pairs and optional native Arrow targets cold", ShanheS5YuanshaoChecks.SameColorPairsAndOptionalNativeArrowTargetsRecoverCold),
    ("Shanhe Xuchu: frozen HP reveal native Slash Duel and owner expiry cold", ShanheXuchuChecks.HpRevealChoiceNativeSlashDuelAndOwnerExpiryCold),
    ("Shanhe Xuchu: revealed cards decline substitution and normal draw cold", ShanheXuchuChecks.RevealedCardsCanDeclineReplacementAndStillDrawNormallyCold),
    ("Shanhe Xuchu: NPC revealed options compare actual cards and normal draw cold", ShanheXuchuChecks.NpcRevealedChoiceWeighsActualCardsAgainstNormalDrawCold),
    ("Shanhe Zhangliang: native fire prevention and round rain cold", ShanheZhangliangChecks.NativeFirePreventionAndRoundDamageRecoverCold),
    ("Shanhe rank: native optional rebirth recovery then five gained cards cold", ShanheRankRebirthChecks.NativeOptionalRebirthRecoversThenGainsFiveWithoutClearingStateCold),
    ("Shanhe equipment forward: direct opening equipment empty deck and native theft", ShanheStartingResourceChecks.OpeningEquipmentDeckAndStolenWeaponStayNativeForward),
    ("Shanhe resources: flag flows across battles and legacy JSON stays unchanged", ShanheCampaignChecks.StartingResourceFlagFlowsAcrossBattlesAndKeepsLegacyJson),
    ("Shanhe resources: native use conversion response equipment and delayed exit cold", ShanheStartingResourceChecks.NewResourcesUseConversionDodgeAndEquipmentExitCold),
    ("Shanhe resources: claimed originals and legacy physical cards stay in circulation", ShanheStartingResourceChecks.ClaimedResourcesAndLegacyRecipeRetainTheirPhysicalCards),
    ("Shanhe resources: supplied Guhuo binds new use owner and failed declarations discard cold", ShanheStartingResourceDeclarationChecks.SuppliedGuhuoTracksItsNewUseOwnerAndFailedDeclarationStaysDiscardedCold),
    ("Shanhe rank: bonuses freeze once and legacy recipes stay unchanged", ShanheCampaignChecks.GeneralRankBonusesFreezeOnceAndLegacyRecipesStayUnchanged),
    ("Shanhe rank: current printed health fifth rank and explicit growth cold", ShanheCampaignChecks.CurrentRankHealthStartsFromPrintedHpAndRetainsExplicitGrowthCold),
    ("Shanhe rank: rebirth usage survives two battles victory defeat and cold", ShanheCampaignChecks.FifthRankRebirthUsageSurvivesTwoBattlesAndColdVictoryOrDefeat),
    ("Shanhe S5 Jiuchi: black hand unlimited Wine real Armor permanent exchange cold", ShanheS5JiuchiChecks.BlackHandWineUnlimitedUsesAndRealArmorDamagePermanentlySwapCold),
    ("Permanent Alcohol exchange: strict parser and registry reference boundaries", AlcoholSlashSkillReplacementValidationChecks.LoaderAndRegistryRejectInvalidPermanentAlcoholReplacementContracts),
    ("Shanhe Armor: full partial damage counter health delta and native Kuanggu cold", ShanheArmorChecks.FullAndPartialArmorKeepDamageCounterAndRealHealthDeltaCold),
    ("Shanhe tactics: random recovery damage native Kuanggu children cold", ShanheTacticReactionChecks.RandomRecoveryDamageFreezesItsRecipientAndComposesWithNativeRecovery),
    ("Shanhe original skills: Shenxing Lianying last hand batch cold", ShanheOriginalSkillChecks.ShenxingLastHandBatchCompletesLianyingBeforeNativeDrawCold),
    ("Shanhe recovery: paid dying Peach blood blade original dying cold", ShanheDamageReplacementChecks.PaidDyingPeachRecoveryStartsBloodBladeAndReturnsToOriginalDyingCold),
    ("Shanhe companion: fixed opening frozen faces native Fudao cold", ShanheBossChecks.FixedCompanionOpeningReplacesRandomBaseFreezesFacesAndReplaysBeforeNativeFudao),
    ("Shanhe observer: third party real Duel exclusions lost and stacked instances cold", ShanheObservedTacticChecks.ThirdPartyNeihongObservesActualDuelsExcludesOtherDamageAndLosesNativeInstancesCold),
    ("Shanhe observer: fire chain frozen base and Silver Lion cold", ShanheObservedTacticChecks.ObservedFireChainRechecksFrozenOriginBaseAndSilverLionCold),
    ("Shanhe observer: unknown public teams preserve identity privacy cold", ShanheObservedTacticChecks.UnknownPublicTeamsDoNotInferHiddenIdentityOrFactionCold),
    ("Shanhe trick ordinal: converted repeated AOE original receipt cold", ShanheTrickOrdinalTacticChecks.ConvertedRepeatedAoeKeepsOriginalOrdinalAcrossOwnCounterspellCold),
    ("Shanhe team tactics: living public allies native Slash debits and skill loss cold", ShanheTeamTacticChecks.JibanRechecksLivingPublicAlliesNativeSlashDebitsAndSkillLossCold),
    ("Shanhe team draw aura: independent copies other public allies phase-only cold", ShanheTeamTacticChecks.TeamDrawAuraStacksForOtherPublicAlliesOnlyDuringActualDrawPhasesCold),
    ("Shanhe team draw aura: source death loss before issue and frozen paid request cold", ShanheTeamTacticChecks.TeamDrawAuraRechecksBeforeIssueAndKeepsIssuedRequestsFrozenCold),
    ("Shanhe team tactics: real opening Draw2 native Hongde gain child cold", ShanheTeamTacticChecks.TeamOpeningUsesRealDrawBatchesWithNativeHongdeCold),
    ("Shanhe team tactics: fixed Fenghuang faces actual draws native Fudao cold", ShanheTeamTacticChecks.FixedFenghuangOpeningKeepsFacesBeforeTeamDrawAndNativeFudaoCold),
    ("Shanhe passives: delayed outcome inverts once without rewriting cards cold", ShanhePassiveTacticChecks.DelayedOutcomeInvertsOnceAcrossRealSourcesWithoutRewritingCardsCold),
    ("Shanhe battle: dynamic rosters private views team AI cold replay", ShanheBattleChecks.DynamicRostersPrivateViewsAndTeamAiColdReplay),
    ("Shanhe battle: invalid recipes external collection isolation", ShanheBattleChecks.RejectInvalidRecipesAndFreezeExternalCollections),
    ("Hulao alliance feast: native per-target nullification and cold choice", HulaoCardChecks.AllianceFeastRetainsNativePerTargetNullificationAndColdChoice),
    ("Shenwu activity: real shared draft private rotated seats and replay", ShenwuActivityChecks.RealSharedDraftPrivacyRotatedSeatsEquipmentAndColdReplay),
    ("Shenwu activity: initial recipe permanent half health and frozen metadata", ShenwuActivityChecks.ExactInitialRecipePermanentHalfHealthAndFrozenMetadata),
    ("Shenwu activity: leader victory waits for death children and extinction", ShenwuActivityChecks.LeaderVictoryWaitsForDeathChildrenAndAlliedExtinctionLoses),
    ("Shenwu NPC: native damage policies replacement draw and turnover", ShenwuNpcChecks.NativeDamagePoliciesReplacementDrawAndTurnover),
    ("Shenwu NPC: damage source and recovery participants draw once", ShenwuNpcChecks.DamageSourceAndRecoveryParticipantsDrawExactlyOnce),
    ("Shenwu NPC: Mingwan later targets and actual ended turn Yandu", ShenwuNpcChecks.MingwanRestrictsLaterUsesAndYanduUsesActualEndedTurn),
    ("Shenwu NPC: Luanchang actual turn start end native AOE parents", ShenwuNpcChecks.LuanchangOwnStartAndActualEndKeepNativeAoeParents),
    ("Shenwu NPC: Futai Peach policy and native coalition death supply", ShenwuNpcChecks.FutaiPeachPolicyAndNativeCoalitionDeathBenefits),
    ("Shenwu expanded: Sipu first two responses and real enemy tiers cold", ShenwuNpcChecks.ExpandedSipuAndRealTierInitialization),
    ("Shenwu cards: native requests payments and cold returns", ShenwuCardChecks.OrdinaryTricksRetainNativeRequestsPaymentsAndColdReturns),
    ("Shenwu equipment: original entities and native effects", ShenwuCardChecks.ExclusiveEquipmentUsesOriginalEntitiesAndNativeEffects),
    ("Nian NPC: real draw replacement dormant targets and Slash payments", NianNpcChecks.BaseDrawRandomReplacementDormancyAndNativeSlash),
    ("Nian activity: real package rotated draft tier five death children and replay", NianPackageChecks.RealPackageRotatedDraftTierFiveDeathChildrenAndColdReplay),
    ("Nian activity: round bags preserve state across extras death skips and replay", NianActivityChecks.RoundBagsPreserveNativeStateAcrossExtrasDeathsAndCold),
    ("Nian clock: expiry retires paid physical Slash children and replay", NianActivityChecks.NativeClockExpiresPaidSlashChildWithoutRepaymentAndCold),
    ("Nian NPC: private wise keeps and actual loss batches", NianNpcChecks.RuizhiPrivateKeepsAndJingjueRealMixedLossBatches),
    ("Nian NPC: actual skipped phases and outside turn per point draws", NianNpcChecks.ActualSkipLedgerAndOutsideTurnPerPointDamageRecovery),
    ("Nian revival: shared three uses and real dead ally restoration", NianRevivalChecks.SharedRevivalRestoresRealDeadAllyThreeTimes),
    ("Nian revival: AI restores human with cold draw and limited usage", NianRevivalChecks.AiRevivesHumanWithColdDrawAndPreservesLimitedUsage),
    ("Nian clock: expiry retires paid revival children without refund", NianRevivalChecks.TimeoutCancelsPaidRevivalDrawWithoutRefundOrReplay),
    ("Nian clock: expiry disposes only the current judgment entity", NianRevivalChecks.TimeoutDisposesOnlyActiveJudgmentEntity),
    ("Nian revival: native draw attempt returns before physical cards", NianRevivalChecks.RevivalWaitsForNativeDrawAttemptBeforePhysicalCards),
    ("Cixiong multi target: staged choices current Slash cursor private cold", CixiongMultiTargetChecks.StagedChoicesKeepTheCurrentNativeSlashTargetAndColdReplay),
    ("Hulao special equipment: native effects and setup entities", HulaoCardChecks.EquipmentGrantsRespectNativeEffectsAndSetupEntities),
    ("Hulao rules: armor prevents whole large damage and coalition death supply", HulaoRuleChecks.AozhanPreventsWholeLargeDamageAndUnitedSuppliesSurvivors),
    ("Hulao rules: real tier five compatible setup and ordinary skill replay", HulaoRuleChecks.RealHulaoSetupTierFiveCompatibleRosterAndKurouColdReplay),
    ("Hulao rules: Xiuluo pays owned discard before real judgment", HulaoRuleChecks.XiuluoPaysNativeOwnedDiscardBeforeRemovingRealJudgment),
    ("Hulao rules: Shenqu physical Peach and Jiwu turn expiry", HulaoRuleChecks.ShenquUsesPhysicalPeachAndJiwuSkillsExpireAtActualTurnEnd),
    ("Hulao transition: paid damage interrupts preserves zones and replay", HulaoTransitionChecks.PaidDamageInterruptsAtFourOrZeroPreservesZonesAndColdReplay),
    ("Hulao transition: real reshuffle interrupts and second waits for turn end", HulaoTransitionChecks.RealDrawShuffleInterruptsThenSecondShuffleWaitsForActualTurnEnd),
    ("Hulao transition: allied and final boss deaths finish children", HulaoTransitionChecks.AlliedAndFinalBossDeathsFinishChildrenBeforeVictory),
    ("Ghost rules: Enyuan hearts per damage and actual recovery points", GhostRuleChecks.EnyuanRequiresHeartsPerDamageAndRewardsActualRecoveryPoints),
    ("Ghost rules: death source discard preserves native zones and replay", GhostRuleChecks.GhostDeathDiscardsTheKillerThroughNativeZones),
    ("Ghost rules: judgment discard and red black consequences use actual cards", GhostRuleChecks.GhostJudgmentDiscardAndRedBlackConsequencesUseActualCards),
    ("Shanhe Guanping companion: mandatory Moyan paid Longyin native return cold", GhostRuleChecks.MandatoryGuanpingMoyanReturnsToPaidLongyinCold),
    ("Jiange activity: five-tier players scaled NPCs private faction roster and replay", JiangeActivityChecks.SetupTiersTeamsPrivateRosterAndReplay),
    ("Jiange activity: Fuyou repeats one red trick use and replays", JiangeActivityChecks.RedTrickRepeatsOnePhysicalUseAndReplays),
    ("Jiange activity: native NPC enemy effects and limited rescue replay", JiangeActivityChecks.NpcEffectsAndLimitedRescueUseRealCommands),
    ("native logical discard invocations preserve whole mixed and cross-prompt costs", NativeLogicalDiscardChecks.WholeLogicalDiscardsKeepCrossPromptAndMixedMaterialInvoicesCold),
    ("native effective faction changes preserve avatar declarations and setup boundaries", NativeFactionChecks.AvatarAcquisitionRedeclarationSourceLossAndSetupCold),
    ("Meng Da: GoudeWholeInvocationHistoryAndOptionMasksCold", MengDaChecks.GoudeWholeInvocationHistoryAndOptionMasksCold),
    ("Cai Mao: whole reveal decline HP privacy and Miuyan replay", CaiMaoChecks.WholeRevealDeclineHpLimitPrivacyAndMiuyanCold),
    ("actual phase end: six actual phases typed cold returns skip and restoration", ActualPhaseEndChecks.SixActualPhaseEndsKeepTypedColdReturnsAndSkipRestorationBoundaries),
    ("Ya Dan: next actual phase original card native uses and expiry replay", YaDanChecks.QingyaNextActualPhaseOriginalCardNativeUsesAndExpiryCold),
    ("Cao Teng: permanent hand growth linked skills and own turn expiry replay", CaoTengChecks.PermanentHandGrowthLinkedAndGrantOwnTurnExpiryCold),
    ("Ma Yuan Yi: published Bing AI exact public pile material cold", MaYuanYiChecks.PublishedBingAiMaterialUsesItsExactPublicPileCold),
    ("Liu Ba: private bottom slots atomic exchange", LiuBaChecks.CoinEndingPrivateBottomSlotsAtomicExchangeCold),
    ("Bao Xin: real effective Hand pile random ring and cold", BaoXinChecks.MutaoRealEffectiveHandPileRandomRingAndCold),
    ("Cui Yan: pre-skill history actual end and range cold", CuiYanChecks.ZhijiePreSkillHistoryActualEndAndRangeCold),
    ("Cui Yan faction supply: requester suit and native Liubing projection cold", CuiYanFactionSupplyChecks.RequesterSuitRestrictionAndNativeLiubingProjectionCold),
    ("Native recast invocation: single and whole group are distinct", NativeRecastInvocationChecks.NativeSingleAndWholeGroupAreDistinct),
    ("Native recast invocation: two participants independent and cold", NativeRecastInvocationChecks.TwoParticipantsOwnIndependentFrozenInvocationsCold),
    ("Native recast invocation: paid loss death and zero draw cold", NativeRecastInvocationChecks.PaidLossAndDeathKeepZeroDrawCompletionCold),
    ("Native recast invocation: equipment recovery and Ox spill cold", NativeRecastInvocationChecks.EquipmentRecoveryAndOxSpillAreNotExtraRecastsCold),
    ("Li Yi: private same choices recast Lion and Ox cold", LiYiChecks.ChanshuangPrivateSameChoicesRecastLionOxCold),
    ("Sun Yuan: native obtain reply and round range cold", SunYuanChecks.SayingNativeObtainReplyAndRoundRangeCold),
    ("Liu Zhang: true use request families and played exclusions cold", LiuZhangChecks.ZonghuTrueUseRequestFamiliesAndPlayedExclusionsCold),
    ("Saying native supply: zero suppliers preserve foreign actor and native return cold", Batch49NativeSupplyChecks.SayingZeroSuppliersKeepForeignActorAndNativeReturnsCold),
    ("Batch51 native boundary: dynamic Leiluan admission and legacy fingerprints", Batch51NativeBoundaryChecks.LeiluanPolicyKeepsDynamicAdmissionAndLegacyFingerprints),
    ("Kong Shu: true responses played and pre skill round names cold", KongShuChecks.LeiluanTrueResponsePlayedAndPreSkillRoundNamesCold),
    ("Ma Cheng: native extra targets and whole Slash cold", MaChengChecks.ChenglieTargetsNativeMultiAndZeroUsesCold),
    ("Sun Hong: actual normal draw phase replacement cold", SunHongChecks.ZenrunActualNormalDrawPhaseReplacementCold),
    ("Native draw attempt: private decision and actual result cold", NativeDrawAttemptChecks.ProgramDrawWaitsForPrivateDecisionAndBindsOnlyActualCardsCold),
    ("Native draw attempt: sequential recipients and actual gain observers cold", NativeDrawAttemptChecks.SelectedRecipientsWaitForEachActualDrawAndObserverCold),
    ("Native draw attempt: zero request and absent capability", NativeDrawAttemptChecks.ZeroRequestAndAbsentCapabilityKeepSynchronousDrawContract),
    ("Native draw attempt: actual normal extra and skipped phases cold", NativeDrawAttemptChecks.ActualNormalAndExtraDrawWaitForExactAttemptCold),
    ("Native card draw: DrawTwo attempts actual gain children and repeated effects cold", NativeDrawCardEffectChecks.NativeDrawTwoWaitsForAttemptAndActualGainChildrenCold),
    ("Native card draw: Zenrun replaces DrawTwo without forged draws cold", NativeDrawCardEffectChecks.ZenrunReplacesNativeDrawTwoWithoutForgingDrawMaterialsCold),
    ("Native card draw: unnullifiable repeated DrawTwo effect identity cold", NativeDrawCardEffectChecks.UnnullifiableRepeatedNativeDrawTwoKeepsIssuedEffectIdentityCold),
    ("Guan Hai: two real Hand baits true Use and played boundaries cold", GuanHaiChecks.XieweiAtomicHandBaitsTrueUsesAndPlayedBoundariesCold),
    ("Native discard projection: fixed and variable collections retain native payment cold", NativeCollectedDiscardProjectionChecks.FixedAndVariableNativeCollectionsPublishRemainingBoundsAndFinish),
    ("Native discard projection: Cao Xian adjustment and target batch cold", NativeCollectedDiscardProjectionChecks.CaoXianAdjustmentAndTargetDiscardPreserveNativeCollectionCold),
    ("Native additional discard: Jianzhuan whole fixed set and separate recast cold", NativeAdditionalDiscardChecks.JianzhuanFixedDiscardProjectsWholeSetAndKeepsRecastSeparateCold),
    ("Native additional discard: Li Yi two card group and paid child cold", NativeAdditionalSelfDiscardChecks.LiYiTwoCardGroupSelectionAndPaidChildCold),
    ("Native additional discard: Yang Feng self group and foreign opaque cold", NativeAdditionalSelfDiscardChecks.YangFengSelfGroupKeepsForeignOpaqueAndColdPayment),
    ("Native additional discard: Bijing whole discard paid batch cold", NativeAdditionalSelfDiscardChecks.BijingBuffersWholeDiscardAndResumesPaidBatchCold),
    ("Xie Ling Yu: paid children replacement source loss death and privacy cold", XieLingYuChecks.XinyouPaidChildrenReplacementSourceLossDeathAndInputPrivacyCold),
    ("Li Feng: native responses played quota and faction supply cold", LiFengChecks.ShuliangNativeResponsesPlayedQuotaAndFactionSupplyCold),
    ("Ordinary Cai Fu Ren: actual turn use targets and single Qieting choice cold", CardGame.Core.Tests.OrdinaryCaiFuRenChecks.QietingActualTurnUseTargetsAndSingleChoiceCold),
    ("Huan Shu Jun: Peach and gifted equipment use restoration children cold", CardGame.Core.Tests.HuanShuJunChecks.WenyiPeachAndGiftedEquipmentUseRestoreChildrenCold),
    ("Zhao Zhong: original Hand gift independent next chain leases cold", CardGame.Core.Tests.ZhaoZhongChecks.DiciOriginalHandGiftIndependentNextChainLeasesCold),
    ("Tao Qian: per recipient native draw replacement paid rights cold", CardGame.Core.Tests.TaoQianChecks.WenrenPerRecipientNativeDrawReplacementPaidRightsCold),
    ("Le Da Qiao Zixi: final targets responses source rights and input privacy cold", CardGame.Core.Tests.LeDaQiaoZixiChecks.ZixiFinalTargetsResponsesSourceRightsAndInputPrivacyCold),
    ("Le Xiao Qiao Qiqin: preparation whole shared discard pile cold", CardGame.Core.Tests.LeXiaoQiaoQiqinChecks.QiqinPreparationObtainsWholeSharedDiscardPileCold),
    ("Le Xiao Qiao Weiwan: directed all Use distance quota bans and expiry cold", CardGame.Core.Tests.LeXiaoQiaoWeiwanChecks.WeiwanDirectedAllUseDistanceCountBanAndTurnExpiryCold),
    ("Le Cai Wen Ji: whole marked loss missing suits and native children cold", CardGame.Core.Tests.LeCaiWenJiChecks.BeifenWholeMarkedLossMissingSuitObtainAndNativeChildrenCold),
    ("Wu Guan Yu Juewu: flood ordered nullification and native children cold", CardGame.Core.Tests.WuGuanYuJuewuChecks.WaterOriginalOrderNullificationAndNativeChildrenCold),
    ("Wu Guan Yu Wuyou: later phase names native responses and supplied use cold", CardGame.Core.Tests.WuGuanYuWuyouChecks.WuyouLaterPhaseNamesNativeResponsesAndProvidedUseCold),
    ("Wu Guan Yu Yixian: all field equipment native children and original owners cold", CardGame.Core.Tests.WuGuanYuYixianChecks.FieldEquipmentAtomicOriginalOwnersLionOxAndCompensationCold),
    ("Le Zou Shi: opening entities dynamic non-locked suppression restore cold", CardGame.Core.Tests.LeZouShiChecks.YunzhengOpeningEntitiesDynamicNonLockedSuppressionAndRestoreCold),
    ("Le Diao Chan: retained show native basic trick delayed equipment cold", CardGame.Core.Tests.LeDiaoChanChecks.DiouRetainedShowNativeBasicTrickDelayedAndEquipmentCold),
    ("Wu Zhang Fei: true Use material ranks private sampling cold", CardGame.Core.Tests.WuZhangFeiChecks.TrueUseMaterialRanksPrivateSamplingAndCold),
    ("Wu Lu Kang: Dixian successful guess effective rank native quota lifetime cold", CardGame.Core.Tests.WuLuKangDixianChecks.SuccessfulGuessEffectiveRankNativeQuotaAndLifetimeCold),
    ("Mou Xiahou Shi Mobile: Yanyu real discard actual Play quota whole turn late inserted support cold", CardGame.Core.Tests.MouXiahouShiYanyuChecks.ActualDiscardPhaseQuotaWholeTurnInsertedSupportCold),
    ("Mou Xiahou Shi Mobile: Qiaoshi native recovery replacement paid children source loss cold", CardGame.Core.Tests.MouXiahouShiQiaoshiChecks.NativeRecoveryReplacementPaidChildrenSourceLossAndCold),
    ("Le Zhuge Guo: native reorder and same Use effects", CardGame.Core.Tests.LeZhugeGuoChecks.NativeReorderAndSameUseEffects),
    ("Mou Zhen Ji Mobile: seated reveal color movement hand limit cold", CardGame.Core.Tests.MouZhenJiMobileChecks.SeatedRevealColorMovementHandLimitCold),
    ("Mou Yuan Shao Mobile: any pair native Arrow responses independent actual turn caps and Lord targets", CardGame.Core.Tests.MouYuanShaoMobileChecks.AnyPairNativeArrowResponsesIndependentTurnCapsAndLordTargets),
    ("Mou Zhu Rong Mobile: native designation contest deferred damage and outside Assault gift", CardGame.Core.Tests.MouZhuRongMobileChecks.NativeDesignationContestDeferredDamageAndOutsideAssaultGift),
    ("OL Cheng Ji Cheng Cui: general weapon actual destroyed bottom payment cold", CardGame.Core.Tests.OlChengJiChengCuiChecks.TousuiGeneralWeaponPaysItsNativeDestroyedMaterialOnceCold),
    ("Mou Da Qiao Mobile: native Guose mode budget removal and Heart Liuli issued Play", CardGame.Core.Tests.MouDaQiaoMobileChecks.NativeGuoseModeBudgetRemovalAndHeartLiuliIssuedPlay),
    ("Mou Huang Yueying Mobile: native Jizhi exact exemption and Qicai three received entities", CardGame.Core.Tests.MouHuangYueyingMobileChecks.NativeJizhiExactExemptionAndQicaiThreeReceivedEntities),
    ("OL Mo Lu Bu: native opening Qi Gangquan reciprocal Kuang and death return", CardGame.Core.Tests.OlMoLuBuChecks.NativeOpeningQiGangquanReciprocalKuangAndDeathReturn),
    ("OL Mo Sima Yi: actual damage records marked aliases learning and round Slash return", CardGame.Core.Tests.OlMoSimaYiChecks.ActualDamageRecordsMarkedAliasesLearningAndRoundSlashReturn),
    ("Mou Cheng Yu OL: stacked targets and actual next-Use native rewards Cold", CardGame.Core.Tests.MouChengYuOlChecks.StackedTargetsAndActualNextUseNativeRewardsCold),
    ("OL Mo Zhang Fei: mandatory identity mixed native targets and counterspell Cold", CardGame.Core.Tests.OlMoZhangFeiChecks.MandatoryIdentityMixedNativeTargetsAndCounterspellCold),
    ("OL Mo Zhang Fei forward: cumulative Slash Duel armor both directions and once", CardGame.Core.Tests.OlMoZhangFeiChecks.CumulativeSlashAndDuelDamageCountsArmorBothDirectionsAndEntersOnceForward),
    ("Mou Han Dang Mobile: native color responses and range aid", CardGame.Core.Tests.MouHanDangMobileChecks.NativeColorResponsesAndRangeAid),
    ("Mou Pang Tong Mobile: ordered native Niepan upgrade all target discard cold", CardGame.Core.Tests.MouPangTongMobileChecks.OrderedNativeNiepanUpgradeAndAllTargetDiscardCold),
    ("Mou Meng Huo Mobile: native attribution charge phase and paid recipient cold", CardGame.Core.Tests.MouMengHuoMobileChecks.NativeAttributionChargePhaseAndPaidRecipientCold),
    ("Mou Zhou Yu Mobile: frozen Yingzi and actual covered guess cold", CardGame.Core.Tests.MouZhouYuMobileChecks.FrozenYingziAndActualCoveredGuessCold),
    ("Mou Lu Zhi: private Ren and completed categories", CardGame.Core.Tests.MouLuZhiMobileChecks.PrivateRenVariableCostAndCompletedCategories),
    ("Mou Xiahou Dun: native retaliation and non-Use distribution", CardGame.Core.Tests.MouXiahouDunMobileChecks.NativeRetaliationAndNonUseDistribution),
    ("Mo Diao Chan: native cohorts and affected Use completion", CardGame.Core.Tests.OlMoDiaoChanChecks.NativeCohortAndAffectedUseCompletion),
    ("Mou Diao Chan: native ring and actual Ending draw", CardGame.Core.Tests.MouDiaoChanMobileChecks.NativeRingAndEndingDraw),
    ("Mou Fa Zheng: native marks random gain and Enyuan", CardGame.Core.Tests.MouFaZhengMobileChecks.NativeMarksRandomGainAndEnyuan),
    ("SP Sun Shangxiang OL: actual recovery help and Fanxiang", CardGame.Core.Tests.SpSunShangxiangOlChecks.ActualRecoveryHelpAndFanxiang),
    ("Mou Sun Jian OL: first HP and independent Hulie", CardGame.Core.Tests.MouSunJianOlChecks.FirstHpAndIndependentHulie),
    ("Mou Zhang Xiu OL: actual phases and previous true Use", CardGame.Core.Tests.MouZhangXiuOlChecks.ActualPhasesAndPreviousTrueUse),
    ("Mou Wen Chou OL: native counts and first all-Hand", CardGame.Core.Tests.MouWenChouOlChecks.NativeCountsAndFirstAllHand),
    ("Mou Deng Ai OL: native Gu exchange and Jiewan", CardGame.Core.Tests.MouDengAiOlChecks.NativeGuExchangeAndJiewan),
    ("OL Mo Cao Cao: native equipment and Bachao", CardGame.Core.Tests.OlMoCaoCaoChecks.NativeEquipmentAndBachao),
    ("Mou Zhang He Mobile: actual skipped phases and Liaoshi", CardGame.Core.Tests.MouZhangHeMobileChecks.NativeSkippedPhasesAndLiaoshi),
    ("Mou Zhu Ran Mobile: sealed payments and Heyuan", CardGame.Core.Tests.MouZhuRanMobileChecks.NativeSealedPaymentsAndHeyuan),
    ("Mou Gongsun Zan OL: equal range native Slash and next actual Play expiry Cold", CardGame.Core.Tests.MouGongsunZanOlChecks.EqualRangeNativeSlashAndNextActualPlayExpiryCold),
    ("Mou Jiang Wei Mobile: real requested Slash gift charge awakening and Beifa Cold", CardGame.Core.Tests.MouJiangWeiMobileChecks.RealRequestedSlashGiftChargeAwakeningAndBeifaCold),
    ("Mou Lu Zhi OL: real red pressure counter and round draw Cold", CardGame.Core.Tests.MouLuZhiOlChecks.RealRedPressureCounterAndRoundDrawCold),
    ("Mou Hua Xiong OL: native committed pair and persistent directional tickets Cold", CardGame.Core.Tests.MouHuaXiongOlChecks.NativeCommittedPairMutualUseAndPersistentDirectionalTicketsCold),
    ("Mou Dong Zhao OL: real damage gift and native suit-loss obligation Cold", CardGame.Core.Tests.MouDongZhaoOlChecks.RealDamageGiftAndNativeSuitLossObligationCold),
    ("Mou Gongsun Zan Mobile: real Hu original Played and Hand conversions Round charge Cold", CardGame.Core.Tests.MouGongsunZanMobileChecks.RealHuOriginalPlayedAndHandConversionsWithRoundChargeCold),
    ("Mou Xiao Qiao OL: real public Hand reveal returns and retained native uses Cold", CardGame.Core.Tests.MouXiaoQiaoOlChecks.RealMiluoRetainedUsesAndExternalRevealReturnCold),
    ("Mou Wang Ling: real Ending gifts five-name awakening and duplicate retention Cold", CardGame.Core.Tests.MouWangLingChecks.ActualEndingGiftsFiveNamesAwakenAndRetainDuplicatesCold),
    ("Mou Liu Xie: native all-Hand adjustments and ordered rewards Cold", CardGame.Core.Tests.MouLiuXieChecks.NativeAllHandAdjustmentsAndOrderedRewardsCold),
    ("Mou Xu Sheng: native color-run benefits and mutual opposing-Hand choices Cold", CardGame.Core.Tests.MouXuShengChecks.NativeColorRunBenefitsAndMutualOpposingHandChoicesCold),
    ("Round ending: old round movement children and actual turn identity cold", RoundEndingChecks.OldRoundMovementChildrenAndColdKeepActualTurnIdentity),
    ("Round ending: pending closing extras skipped turns and population cold", RoundEndingChecks.PendingAndClosingExtraTurnsSkippedTurnsAndPopulationCold),
    ("Round ending: BeiMiHu prelude follows old round cold", RoundEndingChecks.BeiMiHuPreludeStartsOnlyAfterOldRoundEndingCold),
    ("Round ending: legacy round holds return before old round closes cold", RoundEndingChecks.LegacyRoundHoldsReturnBeforeOldRoundEndsCold),
    ("committed use contracts preserve immutable registration and response-use validation", ProgramCommittedUseContractChecks.RegistrationRejectsAmbiguousAndMutableContracts),
    ("program selection transforms requery facts without mutating sources", ProgramSelectionTransformChecks.CandidateTransformsRequeryFactsAndPreserveReadOnlySources),
    ("program selection relations preserve ordered pairs and card targets", ProgramSelectionTransformChecks.CorrelatedSelectionsPreserveOrderedPairsAndCardTargets),
    ("program selection ordering reads current ranks and preserves source order", ProgramSelectionTransformChecks.PindianOrderingUsesCurrentFactsWithoutMutatingInputs),
    ("program selection validation and target resolvers preserve ordered errors and isolated queries", ProgramSelectionValidationChecks.RegisteredValidationAndTargetResolversPreserveBoundaries),
    ("program selection registration preserves ordered stateless modules and failures", ProgramSelectionRegistrationChecks.CatalogRegistrationPreservesOrderFilteringAndFailures),
    ("program selection claimed sources preserve card and target stages", ProgramSelectionRegistrationChecks.ClaimedSourcesKeepCardAndTargetStagesSeparate),
    ("program selection sources requery private facts and freeze inputs", ProgramSelectionRegistrationChecks.ConvertedAndAllHandSourcesRequeryPrivateFacts),
    ("program action registration preserves source and command boundaries", ProgramActionRegistrationChecks.RegistrationAndDispatchPreserveBoundaries),
    ("program capability registration isolates adapters and preserves typed host compatibility", ProgramCapabilityChecks.RegistrationAndIsolationPreserveBoundaries),
    ("program runtime modules share match instances and reject engine dependencies", ProgramRuntimeModuleChecks.IndependentFactoriesShareMatchInstancesAndRejectEngineDependencies),
    ("program insertions: scheduler ownership composes and suspends only original dying", ProgramInsertionChecks.SchedulerOwnershipComposesAndOnlySuspendsOriginalDying),
    ("Mode native pipeline: optional registrations preserve native ordering and per-match ownership", ModeNativePipelineChecks.OptionalRegistrationsShareNativeOrderingAndFreezePerMatch),
    ("Mode presentation: wording visibility and required feature profiles", ModePresentationChecks.WordingVisibilityAndRequiredContractsBelongToProfile),
    ("Mode commands: reveal validation and lazy native host", ModePresentationChecks.RevealCommandRetainsPromptOrderAndLazyNativeHost),
    ("Mode setup: Hulao equipment slot kind and entity random order", ModeInitialEquipmentPlanChecks.HulaoSelectionKeepsNativeSlotsAndKindThenEntityRandomOrder),
    ("Mode configuration: initial validation ordering and exact factory defaults", ModeConfigurationChecks.InitialOptionsKeepOriginalOrderAndFactoryOverrides),
    ("Mode setup: choice metadata family removal and faction precedence", ModePresentationChecks.GeneralChoicesFamiliesAndFactionPrecedenceRemainModeOwned),
    ("Mode composition: independent setup and profile overrides", ModeSetupCompositionChecks.SetupAndProfileOverridesRemainIndependent),
    ("Mode damage pipeline: native priority and per-match rule isolation", ModeDamagePipelineChecks.AdditiveRulesPreserveNativeOrderAndMatchIsolation),
    ("Mode assembly: every registered mode cold construction and optional ports", ModeEntryAssemblyChecks.EveryRegisteredModeComposesWithoutLiveHostAccess),
    ("Mode cards: activity recast and independent basic play families", ModeCardRecastChecks.ActivityRecastAndBasicFamiliesKeepIndependentBoundaries),
    ("Mode activity policy: tier eligibility and allied damage cap ownership", ModeActivityPolicyChecks.TierEligibilityAndAlliedDamageCapsBelongToInstalledFeatures),
    ("Mode allegiance: identity faction privacy and duel relation boundaries", ModeAllegianceChecks.StandardPoliciesKeepPublicIdentityFactionAndDuelBoundaries),
    ("Mode allegiance: exact profile drives native faction and target queries", ModeAllegianceChecks.ExactProfileControlsNativeFactionAndTargetQueriesWithoutChangingMatchState),
    ("Mode setup composition: exact override preserves independent features and rejects conflicts", ModeSetupCompositionChecks.ExactSetupReplacesOnlyDefaultAndConflictsStayExplicit),
    ("Mode setup: frozen private drafts use shared deterministic random", ModeSetupPlanChecks.FrozenPrivateDraftsUseSharedDeterministicRandom),
    ("Mode setup: pure names selected equipment and primary dual health", ModeSetupPlanChecks.PureCardNamesSelectedEquipmentAndPrimaryDualHealth),
    ("Mode extensions: ordered native and external rules", ModeFlowExtensionChecks.NativeAndExternalRulesShareOrderedPipeline),
    ("Mode extensions: ordered runtime continuations", ModeFlowExtensionChecks.NativeAndExternalRuntimeHooksShareOrderedScheduler),
    ("Mode extensions: controlled hosts and wildcard factories", ModeFlowExtensionChecks.ControlledHostsAndWildcardFactoryQueries),
    ("Mode rules: frozen opening dependencies and deterministic initiative", ModeRuleSetChecks.FrozenOpeningDependenciesAndDeterministicInitiative),
    ("Mode rules: death objectives preserve encounter and ranking boundaries", ModeRuleSetChecks.DeathObjectivesPreserveEncounterAndRankingBoundaries),
    ("Mode rules: card projection keeps source identity and mode-local names", ModeRuleSetChecks.CardProjectionKeepsSourceIdentityAndModeLocalNames),
    ("Mode equipment: optional packages and generated dependencies", EquipmentContentChecks.OptionalPackagesAndGeneratedDependencies),
    ("Mode equipment: exact entity source lifetime", EquipmentContentChecks.PurchasedEquipmentLifetimeUsesExactEntitySource),
    ("Mode equipment: explicit match definitions and cold replay", EquipmentContentChecks.ExplicitMatchDefinitionsOverrideOnlyLegacyFallback),
    ("Equipment registration: frozen composition and fingerprint", EquipmentRegistrationChecks.FrozenCompositionAndCompleteFingerprint),
    ("Equipment registration: package dependencies and invalid references", EquipmentRegistrationChecks.PackageDependenciesAndInvalidReferences),
    ("Mode runtime: optional observers are not bound in other matches", MatchModeRuntimeChecks.ScopedObserversAreNotBoundInOtherMatches),
    ("Mode runtime: Wushuang owns optional progress isolation and cold recovery", WushuangModeChecks.OptionalFeatureOwnsMatchProgressAndRecovery),
    ("Mode feature factories: frozen registration and legacy hash", ModeFeatureFactoryChecks.ExplicitFactoriesFreezeMetadataAndPreserveContentHash),
    ("Mode feature factories: invalid registration and hosts", ModeFeatureFactoryChecks.FactoriesRejectDuplicateUnknownAndUnsupportedHosts),
    ("program rule observer registration preserves ordered recursive boundaries", ProgramRuleObserverChecks.RegistrationPreservesOrderReentrancyAndIsolation),
    ("program rule observer registration rejects invalid metadata", ProgramRuleObserverChecks.InvalidRegistrationsFailBeforeDispatch),
    ("program legality registration preserves ordered capability gates", ProgramLegalityChecks.RegistrationAndDispatchPreserveBoundaries),
    ("Table order: moved participant ring preserves damage and judgment trigger priorities", TableOrderChecks.TriggerPrioritiesUseMovedParticipantOrder),
    ("Table order: native move changes distance cold replay and next normal turn", TableOrderChecks.NativeMoveChangesDistanceAndNextNormalTurn),
    ("program interaction registration preserves dispatch boundaries", ProgramInteractionChecks.RegistrationAndDispatchPreserveBoundaries),
    ("program interaction AI preserves published choices and conditional policies", ProgramInteractionChecks.AiSelectionPreservesPublishedOrderAndConditionalPolicies),
    ("Identity concealment: UndefinedHpRejectsNativeRecoveryAndMaxHpNumberCalculation", IdentityConcealmentChecks.UndefinedHpRejectsNativeRecoveryAndMaxHpNumberCalculation),
    ("Identity concealment: GameStartingHpLossAppearanceDoesNotOfferOffTurnGaoling", IdentityConcealmentChecks.GameStartingHpLossAppearanceDoesNotOfferOffTurnGaoling),
    ("Identity concealment: NativeGroupRecoveryAndSilverLionRemovalRejectUndefinedHp", IdentityConcealmentChecks.NativeGroupRecoveryAndSilverLionRemovalRejectUndefinedHp),
    ("Identity concealment: SetupEveryViewerHasUndefinedHpAndForeignTemplatePrivacy", IdentityConcealmentChecks.SetupEveryViewerHasUndefinedHpAndForeignTemplatePrivacy),
    ("Identity concealment: ActualOwnTurnAppearanceRestoresLordInitialBonusWithoutOffTurnGaoling", IdentityConcealmentChecks.ActualOwnTurnAppearanceRestoresLordInitialBonusWithoutOffTurnGaoling),
    ("Identity concealment: NativeSlashAppearanceGaolingRecoveryChildColdAndLaterDamage", IdentityConcealmentChecks.NativeSlashAppearanceGaolingRecoveryChildColdAndLaterDamage),
    ("Identity concealment: NativeProgramHpLossAndDamageReplacementRespectUndefinedHp", IdentityConcealmentChecks.NativeProgramHpLossAndDamageReplacementRespectUndefinedHp),
    ("Identity concealment: PaidGaolingRecoveryChildCanKillAppearingOwnerAndReturnOnce", IdentityConcealmentChecks.PaidGaolingRecoveryChildCanKillAppearingOwnerAndReturnOnce),
    ("Frozen hand roster: ActualFrozenRosterChooserOrderPrivacyAndMandatoryReward", FrozenHandRosterChecks.ActualFrozenRosterChooserOrderPrivacyAndMandatoryReward),
    ("Private whirl: PrivateFiveDepositsColdViewsAndResidualDrawCount", PrivateWhirlChecks.PrivateFiveDepositsColdViewsAndResidualDrawCount),
    ("Damage target budget: actual self source-less and two-point occurrences", DamageTargetBudgetChecks.ActualDamageOccurrencesSelfSourceLessAndTwoPoints),
    ("Damage zones and adjacent red target: each zone privacy once and turn expiry", DamageZoneAndAdjacentTargetChecks.ActualEachZonePrivateSelectionOnceAndTurnExpiry),
    ("Targeted trick hand HP: actual ending adjustment reward gift and cold", TargetedTrickHandHpChecks.ActualEndingDrawDiscardSelfAndOptionalGiftColdReturn),
    ("Phase start card control: payment privacy directed distance and actual expiry", PhaseStartCardControlChecks.MeibuPaymentPrivacyDirectedDistanceAndActualTurnExpiry),
    ("Card response completion cost: typed health kind actual recovery", CardResponseCompletionCostChecks.TypedHealthKindMatchesActualResponseCostRecovery),
    ("Triggered card conversion: real hand equipment materials and cold replay", TriggeredCardConversionChecks.BlackHandAndEquipmentUseRealMaterialsAndColdReplay),
    ("program runtime hook registration preserves phase order and per-game isolation", ProgramRuntimeHookChecks.OrderedPhasesAndMatchIsolation),
    ("program runtime hook registration rejects invalid metadata", ProgramRuntimeHookChecks.InvalidMetadataRejected),
    ("program runtime proofs preserve typed admission order and dying groups", ProgramRuntimeProofChecks.OrderedTypedAdmissionAndDyingGroups),
    ("program continuation registration preserves ordered typed returns", ProgramContinuationChecks.OrderedRegistrationPreservesReturnBoundaries),
    ("native return protocol preserves ordered preparation and isolation", ProgramNativeReturnChecks.OrderedNativeBoundariesPreservePreparationAndIsolation),
    ("native return protocol rejects invalid metadata", ProgramNativeReturnChecks.InvalidNativeReturnMetadataFailsBeforeDispatch),
    ("program window returns preserve typed parent guards and registration", ProgramWindowReturnChecks.RegistrationAndTypedParentGuardsPreserveBoundaries),
    ("typed trigger window driver preserves candidate and closing boundaries", ProgramTriggerWindowDriverChecks.TypedWindowDriverPreservesCandidateAndClosingBoundaries),
    ("trigger window protocol preserves ordered skip and typed returns", TriggerWindowProtocolChecks.OrderedSkipAndTypedReturnsPreserveBoundaries),
    ("Observer subtree: exact paid edges, pending choice and once-only cold return", PairedHandRecastChecks.ObserverSubtreeRejectsUnrelatedEdgesAndResumesExactlyOnce),
    ("frame-owned choices preserve multi-step bindings privacy and replay", ProgramChoiceDraftChecks.MultiStepBindingsPrivacyAndReplay),
    ("Damage nature: fire chain normal thunder HP loss and cold replay", DamageNaturePreventionChecks.ActualFireAndChainPreventedNormalThunderAndHpLossPreserved),
    ("Actor hand limit penalty: real uses stack and change actual discard", ActorHandLimitPenaltyChecks.RealUsesStackAndChangeActualDiscard),
    ("Play phase temporary skills: independent sources share state and preserve permanent grants", PlayPhaseTemporarySkillChecks.MultiplePhaseGrantSourcesShareInstanceStateQuotaAndPreservePermanentSource),
    ("Last zone conversions: equipment Nullification Silver Lion and movement children", LastZoneConversionChecks.LastEquipmentNullificationOwnsSilverLionRecoveryAndMovementChildren),
    ("Card response completion: native Zhangba pair and cold return", CardResponseCompletionNativeChecks.NativeZhangbaDuelResponseCompletesTwoEntitiesOnceAndColdReturns),
    ("Native discard classification: exact paid producers exclude adjacent gain and cleanup", NativeDiscardClassificationChecks.ExactPaidProducersExcludeNeighborReasons),
    ("Phase name prediction: secret actual phase, escalating round cost and native settlement", PhaseNamePredictionChecks.SecretExactPhaseAndSettlement),
    ("Phase name prediction: lethal settlement cold return skips dead actor phases", PhaseNamePredictionChecks.LethalSettlementReturnsColdChildWithoutDeadActorPhases),
    ("Last damage source reciprocity: latest actual source, native HE payment and physical regrant memory", LastDamageSourceReciprocityChecks.LatestSourceAndPhysicalRegrantKeepExactNativeReciprocity),
    ("Unexpected Assault: opaque reveal native damage counterspell and conversion lifetime", UnexpectedAssaultChecks.OpaqueRevealDamageNullificationAndConversionLock),
    ("Activation-count awakening: actual refunded activations preparation quota and source loss", GameActivationAwakeningChecks.ActualRefundedActivationsQualifyPreparationOnceAndSurviveSourceLoss),
    ("Category discard refund: distinct payment phase refund and actual turn target ban", DrawDiscardCategoryRefundChecks.DistinctPaymentRefundsPhaseOnlyAndBansOriginalActualTurnTarget),
    ("Draw-funded distinct basic: qualification ledger and paid children", DrawFundedDistinctBasicUseChecks.QualificationLedgerAndPaidChildren),
    ("Draw-funded distinct basic: native Dodge Dying and program returns", DrawFundedDistinctBasicResponseChecks.NativeDodgeDyingAndProgramReturns),
    ("Ending damage marker: frozen actual damage settles once", TurnEndDamageMarkerChecks.FrozenActualDamageAddsOnceAcrossChildrenAndColdReplay),
    ("Draw advice and designated extra target: frozen draw qualifications and HE payment", DrawAdviceAndDesignatedExtraTargetChecks.DrawAdviceFreezesBothQualificationsAfterRealDrawAndPaysHeOnce),
    ("Public pile cash out: damage points hand equipment and decline", PublicPileCashOutChecks.DamagePointsStoreHandOrEquipmentAndDeclineWithColdReturn),
    ("Actual target prompt retains the AI-owned attack", ActualUseTargetPauseChecks.PacedAiTargetPromptCommitsColdAndReturnsOnce),
    ("Initial hand modifiers: owner maximum HP controls the real deal and dynamic hand limit", InitialHandModifierChecks.ActualDealAndDynamicLimitPreserveRoleAndReplay),
    ("Virtual ordinary trick composition frozen branches and public AI", VirtualOrdinaryTrickCompositionChecks.FrozenBranchesAndParticipantResourcesAreValidated),
    ("Ending Pindian win loss tie actual virtual tricks and cold return", PhaseEndingVirtualTrickChecks.EndingPindianWinLossTieUseExactVirtualTricksAndColdReturn),
    ("Actual turn use ordinal: frozen declaration order and true uses", ActualTurnUseOrdinalChecks.FrozenDeclarationOrderCountsOnlyActualUsesAndColdRestores),
    ("Actual second use foreign grants share state and expire on end", ActualTurnTemporarySkillChecks.ActualSecondUseForeignGrantsShareStateAndExpireOnEnd),
    ("Actual legacy Wine synchronous HP and completion child return", ActualTurnLegacyAlcoholChecks.SynchronousLegacyWineWaitsForHpAndCompletionChildren),
    ("Actual hand gains and category gifts: ActualHandGainsPreserveBatchIdentityAndExcludeSelfRecursion", ActualHandGainAndCategoryGiftChecks.ActualHandGainsPreserveBatchIdentityAndExcludeSelfRecursion),
    ("skill program public pile preparation whole fill paid child", PublicPilePreparationChecks.WholePileFillRetainsPaidChild),
    ("Phase hand seizure: LihunRealCostFlipTakeAndFrozenHpReturnColdReplay", PhaseHandSeizureChecks.LihunRealCostFlipTakeAndFrozenHpReturnColdReplay),
    ("skill program activation BooleanState nested gates and current instance isolation", ProgramActivationBooleanStateChecks.NestedGatesUseTheCurrentOwnerSkillInstance),
    ("public equipment half count draw resumes once", EquipmentHalfDrawChecks.PublicEquipmentCountDrawsOnceAndResumes),
    ("Shanhe Li Ru no-cost trick or all-hand paid child", ShanheLiruSkillChecks.NoCostTrickOrWholeHandKeepsPaidMovementChild),
    ("Shanhe Li Ru chosen-start hand difference fire and two-round quota", ShanheLiruSkillChecks.StartingSeatDifferenceFireEmptyHandAndTwoRoundQuota),
    ("Ordinary SP Cai Wen Ji original Dying four suits private cost cold replay", DyingSuitsAndEndingHistoryChecks.ChenqingOriginalDyingFourSuitsPrivateCostAndColdReplay),
    ("Skill changes: opening damage dying upgrades replacements and cold", SkillChangeOpeningChecks.DamageAndDyingChangesBeforeFirstRoundCold),
    ("Skill changes: shared cap state grants and paid child cold", SkillChangeOpeningChecks.SharedCapStateAndAcquisitionAfterPaidChildCold),
    ("Skill changes: shared source rejects foreign owner instance and cursor", SkillChangeOpeningChecks.SharedChangeRejectsForeignOwnerInstanceAndCursor),
    ("Phase hand extra turn native public declaration paid selected beneficiary", PhaseHandExtraTurnAiChecks.NativePublicDeclarationPaysAndExecutesSelectedBeneficiaryExtraTurn),
    ("Turn kind hand exemption survives source suppression actual turn expiry", TurnHandLimitKindExemptionChecks.IssuedKindPolicySurvivesSourceSuppressionUntilActualTurnEnd),
    ("Recovery replacement paid Qixi stops after observer victory", RecoveryPaidWinnerChecks.SilverLionPaidQixiStopsAfterObserverVictory),
    ("program conversion polarity shared state and replay", ProgramConversionPolarityChecks.SharedStateAndReplay),
    ("Recorded suit Slash: recording reveal damage cold replay", RecordedSuitSlashChecks.RecordingRevealDamageAndReplay),

    ("after turn ended return movement prelude ordering", AfterTurnEndedChecks.ReturnMovementPreludeBeforeWindow),
    ("committed events see the accepted command", EngineFaultChecks.CommittedObserversSeeTheAcceptedCommand),
    ("command projection preparation failure preserves prior commit", CommandSessionChecks.ProjectionFailurePreservesPriorCommit),
    ("command delivery failure preserves visible commit", CommandSessionChecks.DeliveryFailurePreservesVisibleCommit),
    ("command observer exception preserves commit and result", CommandSessionChecks.ObserverExceptionPreservesCommitAndResult),
    ("prepared snapshot rejects observer collection mutation", CommandSessionChecks.ObserverMutationCannotChangePreparedSnapshot),
    ("committed event collections reject observer mutation", CommandSessionChecks.EventCollectionObserversCannotRewriteCommittedHistory),
    ("round-start programs include foreign owners and cold returns", RoundStartingParticipantChecks.ForeignOwnerReceivesRoundStartAndColdReturns),
    ("selected gift continues after recipient death", CardMovementProgramChecks.SelectedGiftContinuesAfterRecipientDeathAndReplays),
    ("internal failure stops the session and preserves its trusted prefix", EngineFaultChecks.InternalFailureStopsTheSessionAndPreservesTheLastPrefix),
    ("draw and recover compile typed instructions", TypedProgramInstructionChecks.DrawAndRecoverCompileDistinctAmountSources),
    ("commands share prepared projection across result and reentry", PreparedCommandProjectionChecks.PublishedResultAndReentrantRejectionSharePreparedView),
    ("long-hand action queries retain exact Spear materials across growing history", LongHandActionQueryChecks.SpearQueriesIgnoreUnrelatedHistoryAndRefreshMaterials),
    ("declaration payment lookups preserve exact receipt precedence", DeclarationPaymentLookupChecks.ReverseLookupsPreserveOriginalReceiptPrecedence),
    ("ordinary card snapshot reuse preserves appearance privacy and replay", SnapshotProjectionReuseChecks.OrdinaryCardReusePreservesAppearancePrivacyAndReplay),
    ("program state projection preserves empty and live policy JSON", ProgramStateProjectionChecks.EmptyAndLivePoliciesMatchOriginalSnapshotJson),
    ("active effect snapshots preserve real duration ownership freezing and replay", ActivePlayerEffectChecks.LiveDurationOwnershipAndFreezing),
    ("public pile projection preserves empty and live queries", PublicPileProjectionChecks.EmptyAndLiveSourcesPreserveOriginalQueriesAndFreeze),
    ("response use completion Dodge timing parent and replay", ResponseUseCompletionChecks.DodgeCompletesAfterCostAndResumesSlashOnce),
    ("response use completion counterspell chain and replay", ResponseUseCompletionChecks.CounterspellCompletesAfterCostAndResumesChainOnce),
    ("response use completion counterspell dying rescue exact parent", ResponseUseCompletionChecks.CounterspellCompletionDyingRescueRetainsExactParent),
    ("response use completion Dodge dying rescue exact parent", ResponseUseCompletionChecks.DodgeCompletionDyingRescueRetainsExactParent),
    ("response use completion excludes group response and wrong loader window", ResponseUseCompletionChecks.GroupDodgeStaysResponseOnlyAndLoaderRejectsWrongWindow),
    ("shared post-event recipient batch counts and reason filters", SharedPostEventChecks.TransferFiltersAndBatchCountsUseTheRecipient),
    ("shared use lifecycle nests rescue inside a suspended card window", CardUseLifecycleChecks.NestedRescueRetainsTheOuterUseWindow),
    ("Taoluan AI uses public ally inference and private gift choices", TaoluanCooperationAiChecks.PublicInferenceAndPrivateGiftChoices),
    ("Program trigger resource contracts", ProgramTriggerResourceChecks.TriggerWindowAndFrozenSuitContracts),
    ("turn-ending Xiaoguo Tianxiang game over stops later observers", TurnEndingGameOverChecks.XiaoguoTianxiangVictoryStopsLaterObservers),
    ("Zhang Xiu Xiongluan abolishes areas and blocks hand without armor bypass", ResponseAndSkillSuppressionChecks.XiongluanBlocksHandButDoesNotIgnoreArmor),
    ("phase exchange mixed zones atomicity and replay", ProgramActivationLimitChecks.MixedZonesAtomicityAndReplay),
    ("owned-card set private draft movement and replay", ProgramOwnedCardsChecks.PrivateDraftBatchMovementAndReplay),
    ("owned-card selection bounds remain private and replay", ProgramOwnedCardsChecks.VariableBoundsStayPrivateAndReplay),
    ("owned-card set shortfall empty sources and invalidation", ProgramOwnedCardsChecks.ShortfallEmptyAndInvalidatedDraft),
    ("Program choice definitions validate choices, payments and presentation hashes", ProgramChoiceChecks.DefinitionsValidateChoicesPaymentsAndPresentationHash),
    ("completed Slash program window follows finished card use and replays", CardUseCompletedChecks.FinishedSlashOpensReplayableProgramWindow),
    ("card-action judgment windows resume and replay", ProgramCardJudgmentWindowChecks.AllCardActionWindowsStartPublicJudgmentsAndReplay),
    ("composition kernel resource graph safety", ProgramCompositionDefinitionChecks.ResourceGraphsRejectAliasingLeaksAndMissingInputs),
    ("composition kernel malformed nodes", ProgramCompositionDefinitionChecks.MalformedNodesFailBeforeExecution),
    ("composition kernel boundary capabilities", ProgramCompositionContextChecks.SharedWindowsAcceptCommonNodesAndRejectMissingContexts),
    ("composition kernel target-set consumption", ProgramCompositionContextChecks.TargetSetIsConsumedOnce),
    ("execution plans freeze instructions and reject ambiguous bindings", ProgramExecutionPlanChecks.PlansFreezeInstructionsAndRejectAmbiguousBindings),
    ("rule query reduction is input-order independent", RuleQueryReducerChecks.IsIndependentOfInputOrderAndRejectsOnlyWinningSetConflicts),
    ("rule query final mode adjustment preserves frozen provenance", RuleQueryReducerChecks.FinalModeAdjustmentPreservesFrozenProvenance),
    ("rule query reduction rejects invalid mutable inputs", RuleQueryReducerChecks.RejectsInvalidInputsAndFreezesOutput),
    ("rule query distance runs both directions before clamping", RuleQueryReducerChecks.DirectionalDistanceRunsBothStagesBeforeClamping),
    ("rule query actual-round use distance requires captured outgoing count", RuleQueryReducerChecks.ActualRoundUseDistanceRequiresCapturedCountAndOutgoingAddition),
    ("rule query registry rejects future Set conflicts", RuleQueryReducerChecks.StaticSetConflictsAreRejectedBeforePlay),
    ("character skills retain separate sources and stable ownership", CharacterSkillSetChecks.GrantsRetainSourcesAndStableOwnership),
    ("character skills reject conflicting grants without partial state", CharacterSkillSetChecks.InvalidGrantsAndConflictsLeaveStateUnchanged),
    ("current turn nonlocked suppression host lifecycle audit", CurrentTurnSkillSuppressionChecks.HostLifecycleQualificationAudit),
    ("module catalog contracts preserve validation order and failure identity", ProgramModuleContractChecks.CatalogValidationKeepsOrderAndFailureIdentity),
    ("program card legality preserves ordered stages and live queries", ProgramCardLegalityChecks.OrderedStagesAndLiveQueries),
    ("program card-use payments preserve ordered proofs and match isolation", ProgramCardUsePaymentChecks.OrderedPaymentProofsAndMatchIsolation),
    ("program damage hooks preserve actual order side effects and short circuits", ProgramDamageHookChecks.ActualDamagePhasesKeepOrderAndShortCircuit),
    ("module distance rules preserve ordered arithmetic and native query boundaries", ProgramDistanceRuleChecks.OrderedArithmeticAndNativeDistanceQueries),
    ("program target rules preserve ordered boundaries and live queries", ProgramTargetRuleChecks.OrderedBoundariesAndLiveQueries),
    ("program target windows preserve ordered subscriptions and exact target identity", ProgramTargetWindowProtocolChecks.OrderedWindowSubscriptionsAndExactTargetIdentity),
    ("program view contributors preserve ordered visibility and Processing privacy", ProgramViewContributorChecks.OrderedVisibilityContributionsAndProcessingPrivacy),
    ("program phase-used projections preserve ordered fold and match isolation", ProgramPhaseUsedProjectionChecks.OrderedFoldAndMatchIsolation),
    ("program fact requirements preserve entry sources and catalog isolation", ProgramFactRequirementChecks.CatalogDemandPreservesEntrySourcesAndIsolation),
    ("module operation continuation preserves dead-target receipt and host boundaries", ProgramModuleContractChecks.DeadTargetPolicyKeepsReceiptAndHostBoundaries),
    ("skill executor resumes child resolution without repeating paid effects", SkillProgramExecutorChecks.SuspendedChildResumesWithoutRepeatingPaidEffect),
    ("skill executor separates owner-death and lost-skill continuation rights", SkillProgramExecutorChecks.ContinuationPermissionsKeepDeathAndSkillLossSeparate),
    ("skill executor preserves continuation metadata and immutable definition checks", SkillProgramExecutorChecks.ContinuationMetadataAndDefinitionValidationRemainAuthoritative),
    ("skill executor keeps lost-skill continuation behind its paid child", SkillProgramExecutorChecks.SourceLossContinuationWaitsForPaidChildExactlyOnce),
    ("skill executor cancels remaining effects after an invalid selected cost", SkillProgramExecutorChecks.InvalidSelectedCostCancelsRemainingEffectsAtomically),
    ("skill executor rejects changed programs and missing or duplicate handlers", SkillProgramExecutorChecks.RejectsChangedProgramsUnknownHandlersAndDuplicates),
    ("active Program contracts reject invalid selections and preserve target order", SkillProgramExecutorChecks.ActiveActivationContractsRejectInvalidDefinitionsAndPreserveOrder),
    ("card subset selector enumerates every legal subset once", CardSubsetSelectorChecks.EnumeratesEveryLegalSubsetExactlyOnce),
    ("card subset selector rejects oversized sources and freezes choices", CardSubsetSelectorChecks.RejectsOversizedOrAmbiguousSourcesAndFreezesChoices),
    ("lifecycle programs reject live card bindings across inserted phases", ProgramLifecycleChecks.RejectsCardBindingsAcrossDetachedPhaseBoundary),
    ("lifecycle programs cancel missing conditional bindings without leaks", ProgramLifecycleChecks.MissingConditionalBindingsCancelWithoutLeakingCards),
    ("cancelled lifecycle programs preserve parent attack cards", ProgramLifecycleChecks.CancelledDamageProgramsDoNotCleanupParentAttackCards),
    ("card movement programs compose atomic per-card and per-batch triggers with replay", CardMovementProgramChecks.AtomicBatchRunsPerCardAndPerBatchAndReplays),
    ("nested card movement batches retain immediate parent identity", CardMovementProgramChecks.NestedBatchesRetainImmediateParentIdentity),
    ("damage programs claim select gift draw and replay through generic choices", DamageProgramChecks.GenericDamageChoicesClaimSelectGiftDrawAndReplay),
    ("damage cursor typed observers require frozen candidate and parent chain", DamageCursorProjectionChecks.TypedObserversRequireExactFrozenCandidateAndParentChain),
    ("schema-19 additive draw adjustments, explicit draws and damage grants compose and replay", DrawPhaseCompositionChecks.AdditiveAdjustmentDrawAndDamageGrantComposeAndReplay),
    ("schema-19 reveal partitions, recovery and explicit draws conserve cards and replay", DrawPhaseCompositionChecks.RevealPartitionRecoveryAndExtraDrawConserveCardsAndReplay),
    ("schema-19 draw-phase validation rejects unsafe data-flow graphs", DrawPhaseCompositionChecks.ValidatorRejectsUnsafeGraphs),
    ("schema-20 turn policies are typed idempotent and expire together", DrawPolicyProgramChecks.TurnPoliciesAreTypedIdempotentAndExpireTogether),
    ("structured skill metadata normalizes explicitly and fingerprints content", SkillMetadataChecks.TagsNormalizeAndFingerprint),
    ("printed Lord skills follow identity and rules 96 replay ownership boundaries", SkillOwnershipChecks.PrintedLordSkillsFollowIdentityAndReplayBoundary),
    ("skill program v2 trigger definitions validate and freeze", SkillProgramTriggerDefinitionChecks.Run),
    ("skill program card triggers require exact conversion sources and replay", SkillProgramCardTriggerChecks.Run),
    ("skill program multi-target triggers wait for every Liuli redirection", SkillProgramTargetOrderChecks.Run),
    ("skill program final judgment triggers resume recovery, drawing and replay", SkillProgramJudgmentTriggerChecks.WindowAndReplay),
    ("skill program judgment replacement commits both old-card destinations and replays", SkillProgramJudgmentReplacementChecks.WindowDestinationsAndReplay),
    ("skill program judgment damage resumes through dying, death and replay", SkillProgramJudgmentDamageChecks.TargetDamageDyingAndReplay),
    ("skill program direct Dodge and Lightning judgments resume and replay", SkillProgramStartedJudgmentChecks.DirectDodgeAndLightningReplay),
    ("skill program contributions bind owner filters, phase ledger and replay", SkillProgramContributionChecks.CrossOwnerFiltersLedgerAndReplay),
    ("skill program selected judgment subjects damage and replay", SkillProgramSelectedJudgmentChecks.SelectedSubjectDamageAndReplay),
    ("schema-46 viewAs source zones validate and isolate hand from equipment", SkillProgramViewAsZoneChecks.DefinitionAndZoneIsolation),
    ("mandatory card identity suppresses native use, ignores Slash distance and replays", SkillProgramCardIdentityChecks.MandatoryIdentityDistanceAndReplay),
    ("turn card-use effects union order consume and expire by action semantics", CardUseModuleEffectChecks.TurnStateUsesActionSemanticsStableOrderAndExpiration),
    ("active programs suspend into Pindian and branch from the frozen result", PindianModuleChecks.ActiveProgramPindianSuspendsConditionsAndReplays),
    ("Private reserve: ending skill order payments expiry and replay", PrivateReserveOrderChecks.EndSkillsChooseOrderPayOnceAndReplay),
    ("turn-ending boundary orders Jushou Jujian Biyue and replays", TurnEndingBoundaryChecks.OrdersJushouJujianBiyueAndReplays),
    ("public Nightmare markers count each Wuhun damage point before dying and replay", PublicMarkerChecks.WuhunDamageOrderAndReplay),
    ("Xingshang claims nested direct death cleanup cards and replays", PublicMarkerChecks.XingshangClaimsNestedDeathCleanupAndReplay),
    ("recast is an independent validated card movement and replays exactly", RecastChecks.CommandAndReplay),
    ("viewer snapshot hides private roles and hands", SnapshotHidesSecrets),
    ("viewer snapshot hides seed and other players' decisions", SnapshotHidesEngineSecrets),
    ("viewer cannot mutate the engine pending decision", SnapshotDecisionIsDefensive),
    ("commands publish a revision and reject stale input atomically", CommandRevisionBoundary),
    ("play commands use one exact published card and target choice", CommandPlayUsesExactChoice),
    ("prompt answers validate prompt, choice, actor and revision", CommandPromptAnswerBoundary),
    ("command reentry is a typed rejection", CommandReentryIsTyped),
    ("winner rules cover all three camps", WinnerRules),
    ("manual discards reject invalid subsets without mutation", ManualDiscardChecks.Rejections),
    ("manual discard checkpoints and commands replay exactly", ManualDiscardChecks.Replay),
    ("manual discards accept partial payments and restore the remaining quota", ManualDiscardChecks.PartialPaymentsRestoreRemainingQuota),
    ("manual discards preserve doubled quota across partial payments cold", ShanheHuangkongChecks.PartialDiscardKeepsFrozenDoubledQuotaCold),
    ("standard deck content is deterministic and balanced", StandardDeckContent),
    ("standard package builds an immutable isolated registry", StandardContentRegistryBuilds),
    ("skill program loader separates canonical gameplay from presentation", SkillProgramChecks.LoaderCanonicalizationAndPresentationIsolation),
    ("skill program loader rejects malformed and unsupported definitions", SkillProgramChecks.LoaderRejectsMalformedUnsupportedDefinitions),
    ("program expression conditions validate types and use three-valued reads", ProgramExpressionChecks.LoaderAndEvaluation),
    ("program expression conditions gate mobile Fuli's flip on unique highest HP", ProgramExpressionChecks.FuliFlipsOnlyAsUniqueHighestHp),
    ("program amount expression: loader hash typed facts and AI", ProgramAmountExpressionChecks.LoaderHashAndTypedEvaluation),
    ("program amount expression: live quantities unknown negative and native health", ProgramAmountExpressionChecks.LiveAmountsSkipUnknownAndApplyHealth),
    ("program amount expression: paid draw child changes hand and cold return once", ProgramAmountExpressionChecks.PaidDrawChildrenResumeColdWithoutReevaluation),
    ("program amount expression: paid health children change facts and cold return once", ProgramAmountExpressionChecks.PaidHealthChildrenResumeColdWithoutReevaluation),
    ("program amount expression: independent HP query keeps original pool and cold return once", CardGame.Core.Tests.HappyShenDaXiaoQiaoChecks.ExpressionQuantitySelectsItsOriginalHealthPoolAndReturnsColdOnce),
    ("program amount expression: captured numbers and empty result bindings stay cold", ProgramAmountExpressionChecks.CapturedNumbersAndZeroCardBindingsRemainAvailableAndCold),
    ("program amount expression: AI cancellation preserves prefix and skips later benefits", ProgramAmountExpressionChecks.AiCancellationPreservesPaidPrefixAndSkipFallback),
    ("frozen expression facts: real recovery draw children and cold comparison", FrozenFactionExpressionChecks.FrozenRecoveryFactsSurviveRealChildrenAndColdReturn),
    ("public pile bindings: selected source frozen suits and paid children cold", BoundPublicPileDistributionChecks.SelectedSourceFrozenSuitsAndPaidChildrenReplayExactly),
    ("configured active sequences reject forged input and replay exact moves", SkillProgramChecks.ConfiguredActiveSequenceIsAtomicAndReplayable),
    ("program gameplay hashes control checkpoint compatibility independently of presentation", SkillProgramChecks.ProgramHashControlsCheckpointCompatibility),
    ("program dying checkpoints cancel a selected card spent on rescue", SkillProgramChecks.ProgramPausedDyingAndConsumedSelection),
    ("physical deck recipes preserve exact suit, rank and content hashing", PhysicalDeckRecipeChecks.ExactSuitRankValidationAndHashing),
    ("classic identity applies base HP, multiple skills and legacy replay boundaries", ClassicGeneralChecks.SetupHealthAndReplay),
    ("classic Borrowed Sword transfers weapons or nests a real Slash and replays", BorrowedSwordChecks.TransferSlashAndReplay),
    ("classic Stone Axe pays an exact two-card cost and resumes Slash damage", StoneAxeChecks.ExactCostDamageAndReplay),
    ("classic Qinglong opens a same-target follow-up Slash and replays", QinglongCrescentBladeChecks.SameTargetFollowupAndReplay),
    ("classic Ice Sword sequentially discards target cards and prevents Slash damage", IceSwordChecks.SequentialDiscardPreventsDamageAndReplays),
    ("classic Ice Sword: protected remaining hand ends paid prevention cold", IceSwordChecks.ProtectedHandAfterOneEquipmentDiscardFinishesNativePreventionCold),
    ("classic Pang De discards an opaque hand card or public equipment through Mengjin", MengjinChecks.HiddenHandPublicEquipmentChoiceAndReplay),
    ("classic Zhuque Fan converts ordinary Slash to Fire Slash and preserves owner choice", ZhuqueFanChecks.FireConversionChainFactionSlashAndLegacyBoundary),
    ("classic Silver Lion caps damage and recovers after leaving equipment", SilverLionChecks.DamageCapRemovalRecoveryAndLegacyBoundary),
    ("classic Wooden Ox stores private playable grain and replays", WoodenOxChecks.StoresPrivatePlayableGrainAndReplays),
    ("classic FactionSlash privately supplies Duel and Barbarian Slash responses and replays", ClassicGeneralChecks.FormalFactionSlashResponseFlow),
    ("content registry rejects duplicate ids and bad references", ContentRegistryValidation),
    ("interactive setup exposes private deterministic general choices", InteractiveSetupPipeline),
    ("Opening customization: private roster and initial hand rerolls replay", OpeningCustomizationChecks.PrivateRosterAndOpeningHandReplay),
    ("Opening customization: public Lord then nonzero human and private AI", OpeningCustomizationChecks.NonLordSelectsAfterPublicLordBeforePrivateAi),
    ("Opening customization: nonzero team and national human before private AI", OpeningCustomizationChecks.TeamAndNationalHumanChooseBeforePrivateAi),
    ("identity dual generals preserve selection health privacy skills and replay", IdentityDualGeneralChecks.SelectionHealthPrivacyAndReplay),
    ("identity role general candidates freeze private choices and replay", IdentitySetupChecks.RoleCandidatesPrivacyAndReplay),
    ("identity general variants keep candidate families private and replay exact editions", IdentitySetupChecks.GeneralVariantsPrivacyAndReplay),
    ("identity draft preserves fixed fair private families restricted AI swaps and exact replay", WeightedGeneralDraftChecks.PrivacyDiversityAndReplay),
    ("identity draft AI matches public Lord defenses roles and faction cooperation", GeneralDraftAiChecks.MatchupsAndPrivacy),
    ("recorded general selection preserves scoring and strict fingerprints", RecordedGeneralSelectionChecks.RecordedScoringAndFingerprintsRemainStrict),
    ("identity draft AI prefers actual ordinary edition upgrades and safer Lords", GeneralDraftAiChecks.OrdinaryEditionStrengthAndLordSafety),
    ("identity draft AI evaluates ordinary roster rules without legacy edition priors", GeneralDraftAiChecks.OrdinaryRosterRuleCoverage),
    ("identity draft AI prices Lord benefit recipients costs and awakening", GeneralDraftAiChecks.LordBenefitRecipientsAndCosts),
    ("identity draft separates five and eight player exact edition selection rosters", IdentityGeneralPoolChecks.ModePoolsAndSelectionBoundaries),
    ("Custom generals: visual compiler immutable extension and validation", CustomGeneralContentChecks.CompilerFreezesValidatesAndExtendsExistingContent),
    ("Custom generals: native active triggered effects and paid child cold replay", CustomGeneralContentChecks.ExistingSkillAndVisualProgramsUseNativeChildrenAndColdReplay),
    ("general rules deduplication removes redundant definitions and keeps distinct rules", IdentityGeneralPoolChecks.EquivalentRulesUseOneRosterEntry),
    ("identity draft keeps excluded editions in native avatar libraries and freezes eligibility", IdentityGeneralPoolChecks.ExcludedEditionsRemainRealAvatarsAndMetadataIsFrozen),
    ("identity Rebel kill reward draws exactly three cards and replays once", IdentitySetupChecks.RebelKillDrawsThree),
    ("AI support draw accepts optional empty target sets", TacticalAiChecks.SupportDrawAllowsPublishedEmptyTargetSets),
    ("Identity belief AI: public quotas and directed evidence", IdentityBeliefAiChecks.PublicCompositionAndDirectedEvidenceConditionJointBeliefs),
    ("Identity belief AI: revealed roles and uncertain endgame", IdentityBeliefAiChecks.PublicRoleRevelationRevisesEvidenceWithoutInventingEndgameRoles),
    ("Identity belief AI: reversible evidence and fallback", IdentityBeliefAiChecks.EvidenceCanReverseAndMissingCompositionRemainsUncertain),
    ("Identity belief AI: real snapshots publish public composition", IdentityBeliefAiChecks.NativeSnapshotsPublishOnlyPublicCompositionAndViewerRoles),
    ("Identity belief AI: participant fork preserves independent observations", IdentityBeliefAiChecks.DetachedParticipantBeliefsPreserveHistoryWithoutSharingObservations),
    ("Identity belief AI: Rebels pressure uncertain targets and protect likely allies", IdentityBeliefAiChecks.RebelsPressureUnresolvedTargetsWithoutAttackingLikelyAllies),
    ("Identity strategy AI: Rebels concentrate on Lord", IdentityStrategyAiChecks.RebelsConcentrateOnLordWithoutRevivingBlockedAttacks),
    ("Identity strategy AI: Lord safety and Loyalist rescue reserve", IdentityStrategyAiChecks.LordSafetyAndLoyalistRescueReserve),
    ("Identity strategy AI: Renegade balances camps and final duel", IdentityStrategyAiChecks.RenegadeBalancesPublicCampsAndTransitionsToFinalDuel),
    ("Identity strategy AI: rescue beliefs and weaker camp", IdentityStrategyAiChecks.RescueBeliefsProtectLordAndPreserveWeakSide),
    ("Identity strategy AI: public views and other modes", IdentityStrategyAiChecks.IdentityObjectivesKeepPublicInputsAndOtherModesStable),
    ("Identity strategy AI: nullification and rescue share camp objectives", IdentityStrategyAiChecks.IdentityNullificationAndRescueMatchCampObjectives),
    ("Rebel strategy AI: effective Lord damage and exposure", RebelStrategyAiChecks.LordPressurePricesEffectiveDamageAndExposure),
    ("Rebel pressure AI: objective exposure preserves attack value and safety", RebelStrategyAiChecks.PressureDiscountsOnlyNewObjectiveAndKeepsRealDamageSafety),
    ("Rebel coordination AI: uncertain neighbour evidence and Lord pressure", RebelStrategyAiChecks.CoordinationWaitsForEvidenceWithoutSuppressingLordOrKnownEnemies),
    ("Rebel strategy AI: group pressure protects allies and winning attacks", RebelStrategyAiChecks.GroupPressureProtectsAlliesUnlessItCanFinishLord),
    ("Rebel strategy AI: side focus and native preparation safety", RebelStrategyAiChecks.SideFocusAndPreparationKeepNativeSafetyRejections),
    ("Rebel strategy AI: confident rescue and self reserve", RebelStrategyAiChecks.RescueReserveUsesConfidenceWithoutAbandoningKnownAllies),
    ("Tactical planning AI: defense removal before a published Slash", ShortPlanningAiChecks.RemovesDefenseBeforeAvailableSlashAndKeepsLegacyPriority),
    ("Tactical planning AI: preserve Vine fire and Silver Lion healing", ShortPlanningAiChecks.PreservesVineFireAndSilverLionHealing),
    ("Tactical planning AI: select fire and red Slash against public armor", ShortPlanningAiChecks.SelectsFireAndRedSlashAgainstPublicArmor),
    ("Tactical planning AI: independent legal materials and immutable views", ShortPlanningAiChecks.OnlyPlansIndependentPublishedAttacksAndNeverMutatesInput),
    ("Tactical planning AI: reserve rescue and skill response materials", SkillAwareCardValueChecks.ReservesLastRescueAndSkillResponseMaterials),
    ("Tactical planning AI: spare responses and constrained skill payments", SkillAwareCardValueChecks.PricesSpareResponsesAndKeepsSelectionConstraints),
    ("Tactical planning AI: convertible defense and historical reserve", SkillAwareCardValueChecks.PreservesConvertibleDefenseWithoutRevivingRejectedActions),
    ("Tactical planning AI: Duel readiness and public response risk", SkillAwareCardValueChecks.DuelReadinessUsesRemainingResponsesAndPublicRisk),
    ("Tactical planning AI: wine uses final attack score", TacticalPlanningIntegrationChecks.WineUsesFinalAttackScoreAndKeepsHistoricalPolicy),
    ("Tactical planning AI: native Liubing traits pure and cold", TacticalPlanningIntegrationChecks.NativeLiubingTraitsArePureAndOverridePrintedSlashColor),
    ("Tactical planning AI: native Yuanchou traits preserve visibility", TacticalPlanningIntegrationChecks.NativeYuanchouTraitsRespectOwnerAndTargetVisibility),
    ("Ganglie source guard: source-less Lightning keeps judgment and cold", SourceLessGanglieChecks.LightningKeepsJudgmentButSkipsMissingSourceAndColdRestores),
    ("Ganglie source guard: sourced punishment and condition shape", SourceLessGanglieChecks.SourcedJudgmentStillPunishesOriginalParticipantAndConditionShapeIsValidated),
    ("AI policy versions validate and replay deterministically", TacticalAiChecks.PolicyReplay),
    ("AI published materials: foreign Xiansi native payment cold", AiPublishedMaterialChecks.ForeignXiansiMaterialsUsePublishedOwnerPileAndPayNativeSlashCold),
    ("AI published materials: round once cardless Slash native quota cold", AiPublishedMaterialChecks.RoundOnceCardlessSlashScoresWithoutPhysicalMaterialAndReturnsCold),
    ("targeted tricks finish when the target spends its last card on Nullification", TargetLossChecks.LastNullification),
    ("hand guidance is private, read-only and agrees with legal actions", HandGuidanceChecks.ReadOnlyAndPrivate),
    ("accepted command journals serialize and replay deterministically", CommandJournalReplay),
    ("command checkpoints restore a paused private prompt deterministically", CheckpointRestore),
    ("command checkpoints reject same-version content drift", CheckpointRejectsSameVersionContentDrift),
    ("initial deal registers every physical card in one zone", InitialDealCardZones),
    ("invalid single and batch moves are atomic", InvalidCardMovesAreAtomic),
    ("Slash and Dodge pass through the Processing zone", SlashAndDodgeProcessing),
    ("card inventory is conserved at every public boundary", CardInventoryConservation),
    ("observers run only after the public operation commits", ObserversRunPostCommit),
    ("typed host events are committed, ordered and deterministic", TypedEventStream),
    ("slash resolution exposes a serializable frame stack", ResolutionFrameStack),
    ("duel alternates Slash responses through a typed window", DuelResponseFlow),
    ("Nullification resolves a private multi-layer trick window", NullificationFlow),
    ("PeachGarden resolves a paused multi-target recovery", PeachGardenFlow),
    ("FiveGrains reveals public cards with private draft prompts", FiveGrainsFlow),
    ("target-card prompts expose opaque slots and replay without hidden identities", TargetCardChecks.OpaqueSlotFlow),
    ("target-card preselection survives native nullification and replay without second picker", TargetCardChecks.PreselectedSlotSurvivesNullificationAndReplay),
    ("FireAttack reveals privately then resolves typed fire damage", FireAttackFlow),
    ("dying response can pause and recover with a private Peach", DyingResponseFlow),
    ("throwing observers are isolated after commit", ObserverFailuresAreIsolated),
    ("synchronous observers cannot advance the engine reentrantly", ReentrantAdvanceIsRejected),
    ("an unknown phase fails fast", UnknownPhaseFailsFast),
    ("snapshot is JSON serializable", SnapshotSerialization),
    ("Boundary Zhang Fei cancellation next real Use freeze", BoundaryZhangFeiChecks.CancellationAndUseFreeze),
    ("Mobile boundary Cao Cao: native claim children precede draw and cardless damage cold", BoundaryCaoCaoMobileChecks.NativeClaimChildrenPrecedeDrawAndCardlessDamageStillDrawsCold),
};

var nameFilters = args.Where(argument => argument.StartsWith("--filter=", StringComparison.OrdinalIgnoreCase))
    .Select(argument => argument["--filter=".Length..].Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
if (nameFilters.Length > 0)
{
    if (nameFilters.Any(value => value.Length == 0))
    {
        Console.Error.WriteLine("A Core check filter cannot be empty.");
        return 2;
    }
    foreach (var value in nameFilters)
    {
        if (!tests.Any(test => test.Name.Contains(value, StringComparison.OrdinalIgnoreCase)))
        {
            Console.Error.WriteLine($"No Core checks matched filter '{value}'.");
            return 2;
        }
    }
    tests = tests.Where(test => nameFilters.Any(value => test.Name.Contains(value, StringComparison.OrdinalIgnoreCase))).ToArray();
}

var passed = 0;
var failed = 0;
const int skipped = 0;
foreach (var (name, body) in tests)
{
    var started = System.Diagnostics.Stopwatch.GetTimestamp();
    try
    {
        body();
        passed++;
        Console.WriteLine($"[PASS] {name}");
    }
    catch (Exception exception)
    {
        failed++;
        Console.WriteLine($"[FAIL] {name}");
        Console.WriteLine(exception);
    }
    finally
    {
        if (args.Contains("--verbose", StringComparer.OrdinalIgnoreCase))
            Console.WriteLine($"[TIME] {name}: {System.Diagnostics.Stopwatch.GetElapsedTime(started).TotalMilliseconds.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)} ms");
    }
}

Console.WriteLine();
Console.WriteLine($"{passed} passed, {failed} failed, {skipped} skipped ({tests.Length} total).");
return failed == 0 ? 0 : 1;

static void SnapshotHidesSecrets()
{
    var game = GameEngine.CreateStandard(new GameOptions { UseInteractiveDiscard = false, Seed = 13, HumanSeat = 0, HumanRole = Role.Lord }, StandardContentRegistry.Create());
    var snapshot = game.State;
    var self = snapshot.Players.Single(player => player.Seat == 0);
    var hiddenOpponent = snapshot.Players.First(player => player.Seat != 0 && player.Role is null);

    Equal(Role.Lord, self.Role);
    Equal(4, self.Hand.Count);
    Equal(0, hiddenOpponent.Hand.Count);
    Equal(4, hiddenOpponent.HandCount);
}

static void SnapshotHidesEngineSecrets()
{
    var game = GameEngine.CreateStandard(new GameOptions { UseInteractiveDiscard = false, Seed = 413, HumanSeat = 0, HumanRole = Role.Lord }, StandardContentRegistry.Create());
    game.DriveStart();

    Equal(413, game.Seed);
    Equal<int?>(null, game.State.Seed);
    Equal<int?>(413, game.CreateSnapshot(0, revealAll: true).Seed);
    NotNull(game.State.PendingDecision);
    Equal<PendingDecision?>(null, game.CreateSnapshot(1).PendingDecision);
    Equal<PendingDecision?>(null, game.CreateSnapshot(1, revealAll: true).PendingDecision);
    True(game.Log.All(entry => !entry.Message.Contains("413", StringComparison.Ordinal)));
}

static void SnapshotDecisionIsDefensive()
{
    GameEngine? selectedGame = null;
    for (var seed = 1; seed <= 64 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord
        }, StandardContentRegistry.Create());
        game.DriveStart();
        if (game.PendingDecision is { ValidCardIds.Count: > 0, ValidTargetSeats.Count: > 0 })
        {
            selectedGame = game;
        }
    }

    NotNull(selectedGame);
    var exposed = selectedGame!.State.PendingDecision!;
    var originalCard = exposed.ValidCardIds[0];
    var originalTarget = exposed.ValidTargetSeats[0];
    True(exposed.ValidCardIds is not int[]);
    True(exposed.ValidTargetSeats is not int[]);

    var copiedCards = exposed.ValidCardIds.ToArray();
    var copiedTargets = exposed.ValidTargetSeats.ToArray();
    copiedCards[0] = int.MaxValue;
    copiedTargets[0] = int.MaxValue;

    Equal(originalCard, selectedGame.PendingDecision!.ValidCardIds[0]);
    Equal(originalTarget, selectedGame.PendingDecision!.ValidTargetSeats[0]);
}

static void CommandRevisionBoundary()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 901,
        HumanSeat = 0,
        HumanRole = Role.Lord
    }, StandardContentRegistry.Create());

    Equal(0L, game.Revision);
    Equal(0L, game.State.Revision);

    var started = game.Submit(new StartGameCommand(ExpectedRevision: 0));
    True(started.Accepted);
    Equal<CommandError?>(null, started.Error);
    Equal(1L, started.Revision);
    Equal(started.Revision, started.State.Revision);
    NotNull(started.PendingDecision);
    True(started.PendingDecision!.PromptId.IsValid);
    Equal(started.Revision, started.PendingDecision.Revision);
    True(started.PendingDecision.Choices.Count > 0);
    Equal(
        started.PendingDecision.Choices.Count,
        started.PendingDecision.Choices.Select(choice => choice.Id).Distinct().Count());

    var stateBeforeStale = SnapshotJson.Serialize(game.State);
    var logCountBeforeStale = game.Log.Count;
    var movementCountBeforeStale = game.CardMovements.Count;
    var stale = game.Submit(new EndPlayPhaseCommand(
        ActorSeat: 0,
        ExpectedRevision: 0,
        PromptId: started.PendingDecision.PromptId));

    False(stale.Accepted);
    Equal(CommandErrorCode.StaleRevision, stale.Error!.Code);
    Equal(1L, game.Revision);
    Equal(stateBeforeStale, SnapshotJson.Serialize(game.State));
    Equal(logCountBeforeStale, game.Log.Count);
    Equal(movementCountBeforeStale, game.CardMovements.Count);
}

static void CommandPlayUsesExactChoice()
{
    GameEngine? selectedGame = null;
    PromptChoice? selectedChoice = null;
    LegalAction? selectedAction = null;
    for (var seed = 1; seed <= 64 && selectedChoice is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord
        }, StandardContentRegistry.Create());
        var started = game.Submit(new StartGameCommand());
        var action = game.GetHumanLegalActions().FirstOrDefault(candidate =>
            candidate.CardId is not null && candidate.TargetSeats.Count == 1 &&
            candidate.ConversionSource is null && candidate.AdditionalConversionSources is null);
        var choice = action is null ? null : started.PendingDecision?.Choices.FirstOrDefault(candidate =>
            candidate.Cards.SequenceEqual([action.CardId!.Value]) &&
            candidate.Targets.SequenceEqual(action.TargetSeats) &&
            !candidate.Parameters.ContainsKey("conversion-skill-id") &&
            candidate.Parameters.GetValueOrDefault("played-card-kind") == action.PlayedCardKind?.ToString());
        if (started.Accepted && choice is not null && action is not null)
        {
            selectedGame = game;
            selectedChoice = choice;
            selectedAction = action;
        }
    }

    NotNull(selectedGame);
    NotNull(selectedChoice);
    NotNull(selectedAction);
    var pending = selectedGame!.PendingDecision!;
    var before = SnapshotJson.Serialize(selectedGame.State);
    var revisionBefore = selectedGame.Revision;
    var card = selectedChoice!.Cards.Single();
    var target = selectedChoice.Targets.Single();

    var malformed = selectedGame.Submit(new PlayCardCommand(
        ActorSeat: 0,
        CardId: card,
        TargetSeats: [int.MaxValue],
        ExpectedRevision: revisionBefore,
        PromptId: pending.PromptId,
        PlayedCardKind: selectedAction!.PlayedCardKind,
        TargetCardId: selectedAction.TargetCardId));
    if (malformed.Accepted) throw new InvalidOperationException("An unpublished target was accepted.");
    Equal(CommandErrorCode.InvalidTarget, malformed.Error!.Code);
    Equal(revisionBefore, selectedGame.Revision);
    Equal(before, SnapshotJson.Serialize(selectedGame.State));

    var accepted = selectedGame.Submit(new PlayCardCommand(
        ActorSeat: 0,
        CardId: card,
        TargetSeats: [target],
        ExpectedRevision: revisionBefore,
        PromptId: pending.PromptId,
        PlayedCardKind: selectedAction!.PlayedCardKind,
        TargetCardId: selectedAction.TargetCardId));
    if (!accepted.Accepted) throw new InvalidOperationException(
        accepted.Error?.Message ?? "The exact published play action was rejected.");
    Equal(revisionBefore + 1, selectedGame.Revision);
    ResolveNullificationWindowForTest(selectedGame);
    if (!selectedGame!.Events.Any(item => item.Payload is CardUseDeclaredEvent declared &&
            declared.CardId == card))
        throw new InvalidOperationException("The accepted exact choice did not declare its physical card use.");
}

static void CommandPromptAnswerBoundary()
{
    GameEngine? selectedGame = null;
    for (var seed = 1; seed <= 4_096 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 150
        }, StandardContentRegistry.Create());
        var result = game.Submit(new StartGameCommand());
        var steps = 0;
        while (result.Accepted && result.State.Status != EngineStatus.Completed && steps++ < 500)
        {
            if (result.State.Status == EngineStatus.AwaitingHumanResponse)
            {
                if (result.PendingDecision?.Kind == DecisionKind.RespondDodge)
                {
                    selectedGame = game;
                    break;
                }

                var responsePrompt = result.PendingDecision!;
                var declineResponse = responsePrompt.Choices.Last();
                result = game.Submit(new AnswerPromptCommand(
                    0,
                    responsePrompt.PromptId,
                    declineResponse.Id,
                    game.Revision));
                continue;
            }

            if (result.State.Status == EngineStatus.AwaitingHumanDying)
            {
                var dyingPrompt = result.PendingDecision!;
                var letDieChoice = dyingPrompt.Choices.Single(choice =>
                    choice.Parameters["response"] == "let-die");
                result = game.Submit(new AnswerPromptCommand(
                    0,
                    dyingPrompt.PromptId,
                    letDieChoice.Id,
                    game.Revision));
                continue;
            }

            if (result.State.Status == EngineStatus.AwaitingHumanCardSelection)
            {
                var harvestPrompt = result.PendingDecision!;
                var harvestChoice = harvestPrompt.Choices.First();
                result = game.Submit(new AnswerPromptCommand(
                    0,
                    harvestPrompt.PromptId,
                    harvestChoice.Id,
                    game.Revision));
                continue;
            }

            if (result.State.Status == EngineStatus.AwaitingHumanPlay)
            {
                var playPrompt = result.PendingDecision!;
                result = game.Submit(new EndPlayPhaseCommand(
                    ActorSeat: 0,
                    ExpectedRevision: game.Revision,
                    PromptId: playPrompt.PromptId));
            }
            else
            {
                result = game.Submit(new AdvanceCommand(game.Revision));
            }
        }
    }

    NotNull(selectedGame);
    var gameWithPrompt = selectedGame!;
    var prompt = gameWithPrompt.PendingDecision!;
    True(prompt.PromptId.IsValid);
    Equal(gameWithPrompt.Revision, prompt.Revision);
    True(prompt.Choices.Any(choice => choice.Parameters["response"] == "dodge"));
    var decline = prompt.Choices.Single(choice => choice.Parameters["response"] == "take-damage");
    var before = SnapshotJson.Serialize(gameWithPrompt.State);
    var revisionBefore = gameWithPrompt.Revision;

    var stale = gameWithPrompt.Submit(new AnswerPromptCommand(
        ActorSeat: 0,
        Prompt: prompt.PromptId,
        Choice: decline.Id,
        ExpectedRevision: revisionBefore - 1));
    False(stale.Accepted);
    Equal(CommandErrorCode.StaleRevision, stale.Error!.Code);
    Equal(revisionBefore, gameWithPrompt.Revision);
    Equal(before, SnapshotJson.Serialize(gameWithPrompt.State));

    var wrongPrompt = gameWithPrompt.Submit(new AnswerPromptCommand(
        ActorSeat: 0,
        Prompt: new PromptId(prompt.PromptId.Value + 1),
        Choice: decline.Id,
        ExpectedRevision: revisionBefore));
    False(wrongPrompt.Accepted);
    Equal(CommandErrorCode.InvalidPrompt, wrongPrompt.Error!.Code);
    Equal(before, SnapshotJson.Serialize(gameWithPrompt.State));

    var wrongChoice = gameWithPrompt.Submit(new AnswerPromptCommand(
        ActorSeat: 0,
        Prompt: prompt.PromptId,
        Choice: new ChoiceId("response.not-published"),
        ExpectedRevision: revisionBefore));
    False(wrongChoice.Accepted);
    Equal(CommandErrorCode.InvalidChoice, wrongChoice.Error!.Code);
    Equal(before, SnapshotJson.Serialize(gameWithPrompt.State));

    var accepted = gameWithPrompt.Submit(new AnswerPromptCommand(
        ActorSeat: 0,
        Prompt: prompt.PromptId,
        Choice: decline.Id,
        ExpectedRevision: revisionBefore));
    True(accepted.Accepted);
    Equal(revisionBefore + 1, gameWithPrompt.Revision);
    True(gameWithPrompt.PendingDecision is null ||
         gameWithPrompt.PendingDecision.PromptId != prompt.PromptId);
}

static void CommandReentryIsTyped()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 907,
        HumanSeat = 0,
        HumanRole = Role.Lord
    }, StandardContentRegistry.Create());
    CommandResult? reentry = null;
    game.LogAdded += _ => reentry ??= game.Submit(new AdvanceCommand(game.Revision));

    var result = game.Submit(new StartGameCommand());

    True(result.Accepted);
    NotNull(reentry);
    False(reentry!.Accepted);
    Equal(CommandErrorCode.ReentrantOperation, reentry.Error!.Code);
    Equal(result.Revision, game.Revision);
}

static void WinnerRules()
{
    Equal(Winner.None, GameRules.EvaluateWinner(
    [
        new(Role.Lord, true),
        new(Role.Loyalist, true),
        new(Role.Rebel, true),
        new(Role.Renegade, true)
    ]));

    Equal(Winner.LordAndLoyalists, GameRules.EvaluateWinner(
    [
        new(Role.Lord, true),
        new(Role.Loyalist, true),
        new(Role.Rebel, false),
        new(Role.Renegade, false)
    ]));

    Equal(Winner.Rebels, GameRules.EvaluateWinner(
    [
        new(Role.Lord, false),
        new(Role.Loyalist, false),
        new(Role.Rebel, true),
        new(Role.Renegade, true)
    ]));

    Equal(Winner.Renegade, GameRules.EvaluateWinner(
    [
        new(Role.Lord, false),
        new(Role.Loyalist, false),
        new(Role.Rebel, false),
        new(Role.Renegade, true)
    ]));
}

static void StandardDeckContent()
{
    var registry = StandardContentRegistry.Create();
    var definition = registry.GetDeck("standard:basic-demo");
    var game = GameEngine.CreateStandard(new GameOptions
    {
        Seed = 1,
        PlayerCount = 5,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        UseInteractiveSetup = false
    }, registry);
    var physical = game.CreateCardZoneDiagnostics();
    var expected = definition.Cards.ToDictionary(
        entry => registry.GetCard(entry.CardDefinitionId).LegacyKind!.Value,
        entry => entry.Count);
    var actual = physical.GroupBy(card => card.CardKind)
        .ToDictionary(group => group.Key, group => group.Count());

    Equal(4, definition.InitialHandSize);
    Equal(2, definition.DrawPerTurn);
    Equal(definition.Cards.Sum(entry => entry.Count), physical.Count);
    Equal(expected.Count, actual.Count);
    foreach (var (kind, count) in expected) Equal(count, actual[kind]);
    True(physical.Select(card => card.CardId).Order().SequenceEqual(
        Enumerable.Range(1, physical.Count)));
    True(game.CreateSnapshot(0, true).Players.All(player =>
        player.HandCount == definition.InitialHandSize));
}

static void StandardContentRegistryBuilds()
{
    var registry = StandardContentRegistry.Create();
    var secondRegistry = StandardContentRegistry.Create();

    Equal(1, registry.Packages.Count);
    Equal("standard", registry.Packages[0].Id);
    TrueWithMessage(!string.IsNullOrWhiteSpace(registry.ContentHash), "registry exposes a content hash");
    Equal(registry.ContentHash, secondRegistry.ContentHash);
    Equal(35, registry.Cards.Count);
    Equal(14, registry.Skills.Count);
    Equal(
        "装备至防具槽；成为普通/火/雷杀的直接目标时可选择公开判定，红色判定牌视为闪。",
        registry.GetCard("standard:bagua").Description);
    Equal("public-judgment-dodge", registry.GetCard("standard:bagua").AiTags!["response"]);
    Equal(
        "装备至武器槽；你使用杀时无视目标的防具。",
        registry.GetCard("standard:qinggang_sword").Description);
    Equal("ignore-armor", registry.GetCard("standard:qinggang_sword").AiTags!["modifier"]);
    Equal(14, registry.Generals.Count);
    Equal(1, registry.Decks.Count);
    Equal(2, registry.Modes.Count);
    Equal(CardKind.Slash, registry.GetCard("standard:slash").LegacyKind);
    Equal(CardKind.QizhengXiangsheng, registry.GetCard("mobile:qizheng-xiangsheng").LegacyKind);
    Equal(CardKind.DagongChe, registry.GetCard("anniversary:dagong-che-card").LegacyKind);
    Equal("mobile:mantian-guohai", registry.GetCard("mobile:mantian-guohai").Id);
    Equal(CardKind.MantianGuohai, registry.GetCard("mobile:mantian-guohai").LegacyKind);
    TrueWithMessage(registry.GetSkill("mobile:mantian-guohai-gift") is { Program: not null },
        "Mantian Guohai registers its native ordered-gift program alongside the card");
    Equal("standard:jianxiong", registry.Generals["standard:cao-cao"].SkillId);
    Equal("standard:wusheng", registry.Generals["standard:guan-yu"].SkillId);
    Equal("standard:longdan", registry.Generals["standard:zhao-yun"].SkillId);
    TrueWithMessage(registry.GetSkill("standard:yingzi") is
    { Program: not null },
        "current Yingzi uses the shared composition kernel");
    TrueWithMessage(registry.GetSkill("standard:yiji") is
    { Program: not null },
        "current Yiji uses the shared composition kernel");
    Equal("standard:yiji", registry.Generals["standard:guo-jia"].SkillId);
    TrueWithMessage(registry.GetSkill("standard:jieming") is
    { Program: not null },
        "current Jieming uses the shared composition kernel");
    Equal("standard:jieming", registry.Generals["standard:xun-yu"].SkillId);
    Equal("standard:yuanhu", registry.GetSkill("standard:yuanhu").Id);
    Equal("standard:yuanhu", registry.Generals["standard:demo-yuanhu"].SkillId);
    Equal("standard:ganglie", registry.GetSkill("standard:ganglie").Id);
    Equal("standard:ganglie", registry.Generals["standard:demo-ganglie"].SkillId);
    Equal("standard:guicai", registry.GetSkill("standard:guicai").Id);
    Equal("standard:guicai", registry.Generals["standard:demo-guicai"].SkillId);
    Equal(18, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:slash").Count);
    Equal(4, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:duel").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:draw_two").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:barbarian_assault").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:arrow_barrage").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:fire_attack").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:peach_garden").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:five_grains").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:dismantlement").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:snatch").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:fire_slash").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:thunder_slash").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:alcohol").Count);
    Equal(1, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:qinggang_sword").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:nullification").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:iron_chain").Count);
    Equal(2, registry.GetDeck("standard:basic-demo").Cards.Single(card =>
        card.CardDefinitionId == "standard:supply_shortage").Count);

    True(!ReferenceEquals(registry.Cards, secondRegistry.Cards));
    Throws<NotSupportedException>(() =>
        ((IDictionary<string, ContentCardDefinition>)registry.Cards).Add(
            "test:extra",
            new ContentCardDefinition("test:extra", "额外", "测试", "测试")));
}

static void ContentRegistryValidation()
{
    var skillless = new ContentGeneralDefinition("test:skillless", "无技能角色", "supporter", null);
    var skilllessRegistry = ContentRegistry.Build(new SyntheticPackage("skillless", builder => builder.AddGeneral(skillless)));
    TrueWithMessage(skilllessRegistry.Generals[skillless.Id].SkillIds.Count == 0 && skilllessRegistry.Skills.Count == 0,
        "A null primary represents zero printed skills without a synthetic skill definition.");
    Throws<ArgumentException>(() => ContentRegistry.Build(new SyntheticPackage("empty-primary",
        builder => builder.AddGeneral(skillless with { SkillId = " " }))));
    Throws<ArgumentException>(() => ContentRegistry.Build(new SyntheticPackage("orphaned-extra",
        builder => builder.AddGeneral(skillless with { AdditionalSkillIds = ["missing:skill"] }))));

    var duplicateId = new ContentCardDefinition(
        "test:duplicate", "重复", "测试", "测试");
    Throws<InvalidOperationException>(() => ContentRegistry.Build(
        new SyntheticPackage("one", builder => builder.AddCard(duplicateId)),
        new SyntheticPackage("two", builder => builder.AddCard(duplicateId))));

    Throws<InvalidOperationException>(() => ContentRegistry.Build(
        new SyntheticPackage("deck", builder => builder.AddDeck(new ContentDeckRecipe(
            "test:deck",
            "坏牌堆",
            1,
            1,
            [new ContentDeckCardCount("missing:card", 1)])))));

    Throws<InvalidOperationException>(() => ContentRegistry.Build(
        new SyntheticPackage(
            "consumer",
            _ => { },
            new PackageDependency("missing", new Version(1, 0, 0)))));

    Throws<InvalidOperationException>(() => ContentRegistry.Build(
        new SyntheticPackage(
            "cycle-a",
            _ => { },
            new PackageDependency("cycle-b", new Version(1, 0, 0))),
            new SyntheticPackage(
            "cycle-b",
            _ => { },
            new PackageDependency("cycle-a", new Version(1, 0, 0)))));

    Throws<InvalidOperationException>(() => ContentRegistry.Build(new SyntheticPackage(
        "identity-team-role",
        builder => builder.AddMode(new ContentModeDefinition(
            "identity:team-role",
            "错误身份局",
            2,
            2,
            new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1,
                [nameof(Role.TeamA)] = 1
            })))));

    Throws<InvalidOperationException>(() => ContentRegistry.Build(new SyntheticPackage(
        "national-missing-pool",
        builder => builder.AddMode(new ContentModeDefinition(
            "national:missing-pool",
            "缺少专属武将池的国战",
            4,
            4,
            new Dictionary<string, int>(),
            GeneralCandidateCount: 3,
            ModeKind: ContentModeKind.NationalWarLite,
            FactionCounts: new Dictionary<string, int>
            {
                ["wei"] = 2,
                ["shu"] = 2
            })))));

    var firstContent = ContentRegistry.Build(new SyntheticPackage(
        "hash",
        builder => builder.AddCard(new ContentCardDefinition(
            "hash:card",
            "甲",
            "测试",
            "定义一"))));
    var secondContent = ContentRegistry.Build(new SyntheticPackage(
        "hash",
        builder => builder.AddCard(new ContentCardDefinition(
            "hash:card",
            "乙",
            "测试",
            "定义二"))));
    TrueWithMessage(
        !string.Equals(firstContent.ContentHash, secondContent.ContentHash, StringComparison.Ordinal),
        "same-version package changes produce different content hashes");
}

static void InteractiveSetupPipeline()
{
    var options = new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 1_931,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        UseInteractiveSetup = true
    };
    var registry = StandardContentRegistry.Create();
    var left = GameEngine.CreateStandard(options, registry);
    var right = GameEngine.CreateStandard(options, StandardContentRegistry.Create());

    Equal(EngineStatus.NotStarted, left.State.Status);
    True(left.State.Players.All(player => player.HandCount == 0));
    True(left.State.Players.All(player => player.GeneralId == string.Empty));

    var started = left.Submit(new StartGameCommand());
    var rightStarted = right.Submit(new StartGameCommand());
    True(started.Accepted);
    True(rightStarted.Accepted);
    Equal(EngineStatus.AwaitingHumanGeneralSelection, started.State.Status);
    Equal(DecisionKind.SelectGeneral, started.PendingDecision!.Kind);
    Equal(3, started.PendingDecision.ValidContentIds.Count);
    True(started.PendingDecision.ValidContentIds.SequenceEqual(
        rightStarted.PendingDecision!.ValidContentIds));
    True(started.PendingDecision.Choices.All(choice => choice.ContentIds.Count == 1));
    True(started.PendingDecision.PromptId.IsValid);

    var otherView = left.CreateSnapshot(1);
    Equal<PendingDecision?>(null, otherView.PendingDecision);
    True(otherView.Players.All(player => player.GeneralId == string.Empty));
    var otherJson = SnapshotJson.Serialize(otherView);
    True(started.PendingDecision.ValidContentIds.All(
        candidate => !otherJson.Contains(candidate, StringComparison.Ordinal)));

    var beforeInvalid = left.Revision;
    var invalid = left.Submit(new SelectGeneralCommand(
        0,
        "standard:not-a-candidate",
        beforeInvalid,
        started.PendingDecision.PromptId));
    False(invalid.Accepted);
    Equal(CommandErrorCode.InvalidGeneral, invalid.Error!.Code);
    Equal(beforeInvalid, left.Revision);

    var choice = started.PendingDecision.Choices[0];
    var selected = left.Submit(new SelectGeneralCommand(
        0,
        choice.ContentIds.Single(),
        left.Revision,
        started.PendingDecision.PromptId));
    var rightSelected = right.Submit(new SelectGeneralCommand(
        0,
        rightStarted.PendingDecision!.Choices[0].ContentIds.Single(),
        right.Revision,
        rightStarted.PendingDecision.PromptId));

    True(selected.Accepted);
    True(rightSelected.Accepted);
    Equal(EngineStatus.AwaitingHumanPlay, selected.State.Status);
    True(selected.State.Players.Single(player => player.Seat == 0).HandCount >= 4);
    True(selected.State.Players.All(player => player.IsGeneralPublic));
    Equal(
        selected.State.Players.Count,
        selected.State.Players.Select(player => player.GeneralId)
            .Distinct(StringComparer.Ordinal)
            .Count());
    Equal(
        SnapshotJson.Serialize(left.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(right.CreateSnapshot(0, revealAll: true)));

    True(left.Events.Any(eventItem => eventItem.Payload is GeneralSelectionRequestedEvent));
    True(left.Events.Any(eventItem => eventItem.Payload is GeneralSelectedEvent));
    True(left.Events.Any(eventItem => eventItem.Payload is SetupCompletedEvent));
    var leftEvents = left.Events.Select(eventItem =>
        $"{eventItem.Sequence}|{eventItem.Payload.GetType().Name}|{eventItem.Payload}");
    var rightEvents = right.Events.Select(eventItem =>
        $"{eventItem.Sequence}|{eventItem.Payload.GetType().Name}|{eventItem.Payload}");
    True(leftEvents.SequenceEqual(rightEvents));
}

static string EventSignature(EventEnvelope eventItem) =>
    JsonSerializer.Serialize(new
    {
        eventItem.Id,
        eventItem.ParentId,
        eventItem.Sequence,
        eventItem.Revision,
        eventItem.CorrelationId,
        Payload = JsonSerializer.Serialize(eventItem.Payload, eventItem.Payload.GetType())
    });

static void CommandJournalReplay()
{
    var options = new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 808,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        UseInteractiveSetup = false,
        MaxTurns = 120
    };
    var original = GameEngine.CreateStandard(options, StandardContentRegistry.Create());
    var started = original.Submit(new StartGameCommand(original.Revision));
    TrueWithMessage(started.Accepted, "replay start accepted");
    var endedPlay = original.Submit(new EndPlayPhaseCommand(0, original.Revision));
    TrueWithMessage(endedPlay.Accepted, "replay end-play accepted");

    var journal = original.AcceptedCommands;
    Equal(2, journal.Count);
    var json = CommandJson.Serialize(journal);
    TrueWithMessage(json.Contains("\"$type\": \"start\"", StringComparison.Ordinal), "journal has command discriminator");
    var decoded = CommandJson.Deserialize(json);
    Equal(journal.Count, decoded.Count);

    var replayed = GameReplay.Replay(
        options,
        decoded,
        StandardContentRegistry.Create());
    Equal(SnapshotJson.Serialize(original.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(replayed.CreateSnapshot(0, revealAll: true)));
    TrueWithMessage(original.CardMovements.SequenceEqual(replayed.CardMovements), "replay preserves card movements");
    TrueWithMessage(
        original.Events.Select(EventSignature).SequenceEqual(replayed.Events.Select(EventSignature)),
        "replay preserves typed events");
    TrueWithMessage(
        original.AcceptedCommands.SequenceEqual(replayed.AcceptedCommands),
        "replay preserves accepted command journal");
}

static void CheckpointRestore()
{
    var options = new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 912,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        UseInteractiveSetup = true,
        MaxTurns = 120
    };
    var original = GameEngine.CreateStandard(options, StandardContentRegistry.Create());
    var started = original.Submit(new StartGameCommand(original.Revision));
    TrueWithMessage(started.Accepted, "checkpoint start accepted");
    var pending = started.PendingDecision;
    NotNull(pending);
    Equal(DecisionKind.SelectGeneral, pending!.Kind);

    var checkpoint = original.CreateCheckpoint();
    Equal(original.Revision, checkpoint.Revision);
    Equal(original.ModeId, checkpoint.ModeId);
    Equal(1, checkpoint.Commands.Count);
    TrueWithMessage(
        checkpoint.ContentPackages.SequenceEqual(["standard@" + StandardContentPackage.CurrentVersion]),
        "checkpoint records the content package signature");
    Equal(StandardContentRegistry.Create().ContentHash, checkpoint.ContentHash);

    var json = GameCheckpointJson.Serialize(checkpoint);
    TrueWithMessage(!JsonDocument.Parse(json).RootElement.TryGetProperty("RulesVersion", out _),
        "new checkpoints omit the removed rules epoch");
    TrueWithMessage(json.Contains("\"$type\": \"start\"", StringComparison.Ordinal),
        "checkpoint JSON retains command discriminators");
    var decoded = GameCheckpointJson.Deserialize(json);
    var restored = GameReplay.Restore(decoded, StandardContentRegistry.Create());

    Equal(
        SnapshotJson.Serialize(original.CreateSnapshot(0)),
        SnapshotJson.Serialize(restored.CreateSnapshot(0)));
    Equal(
        SnapshotJson.Serialize(original.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(restored.CreateSnapshot(0, revealAll: true)));
    Equal(JsonSerializer.Serialize(original.Log), JsonSerializer.Serialize(restored.Log));
    Equal(JsonSerializer.Serialize(original.AiThoughts), JsonSerializer.Serialize(restored.AiThoughts));
    Equal(JsonSerializer.Serialize(original.AiGeneralThoughts), JsonSerializer.Serialize(restored.AiGeneralThoughts));
    Equal(JsonSerializer.Serialize(original.CardMovements), JsonSerializer.Serialize(restored.CardMovements));
    TrueWithMessage(
        original.Events.Select(EventSignature).SequenceEqual(restored.Events.Select(EventSignature)),
        "checkpoint restore preserves typed events");
    TrueWithMessage(
        original.AcceptedCommands.SequenceEqual(restored.AcceptedCommands),
        "checkpoint restore preserves the command prefix");

    ThrowsFor<InvalidOperationException>(() => GameReplay.Restore(
        decoded with { ContentHash = "00BAD-CONTENT-HASH" },
        StandardContentRegistry.Create()), "a mismatched content hash");
    var legacyJson = System.Text.Json.Nodes.JsonNode.Parse(json)!.AsObject();
    legacyJson["RulesVersion"] = int.MaxValue;
    var legacyCheckpoint = GameCheckpointJson.Deserialize(legacyJson.ToJsonString());
    var legacyRestored = GameReplay.Restore(legacyCheckpoint, StandardContentRegistry.Create());
    Equal(SnapshotJson.Serialize(restored.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(legacyRestored.CreateSnapshot(0, revealAll: true)));
    TrueWithMessage(original.Events.Select(EventSignature).SequenceEqual(legacyRestored.Events.Select(EventSignature)) &&
        !JsonDocument.Parse(GameCheckpointJson.Serialize(legacyCheckpoint)).RootElement.TryGetProperty("RulesVersion", out _),
        "legacy JSON fields are ignored on read and omitted on write without changing replay");

    var selectedGeneralId = pending.ValidContentIds.First();
    var originalSelection = original.Submit(new SelectGeneralCommand(
        0,
        selectedGeneralId,
        original.Revision,
        pending.PromptId));
    var restoredSelection = restored.Submit(new SelectGeneralCommand(
        0,
        selectedGeneralId,
        restored.Revision,
        restored.PendingDecision!.PromptId));
    TrueWithMessage(originalSelection.Accepted && restoredSelection.Accepted,
        "the restored private prompt accepts the same command");
    Equal(
        SnapshotJson.Serialize(original.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(restored.CreateSnapshot(0, revealAll: true)));
    TrueWithMessage(
        original.Events.Select(EventSignature).SequenceEqual(restored.Events.Select(EventSignature)),
        "continued checkpoint state remains deterministic");

    ThrowsFor<ArgumentNullException>(() => GameEngine.CreateStandard(options, null!),
        "a null content registry at game creation");
    ThrowsFor<ArgumentNullException>(() => GameReplay.Replay(options, checkpoint.Commands, null!),
        "a null content registry at replay");
    ThrowsFor<ArgumentNullException>(() => GameReplay.Restore(decoded, null!),
        "a null content registry at restore");

    var commandRegistry = StandardContentRegistry.Create();
    var commandDriven = GameEngine.CreateStandard(
        new GameOptions { UseInteractiveDiscard = false, Seed = 913 },
        commandRegistry);
    _ = commandDriven.DriveStart();
    var commandCheckpoint = commandDriven.CreateCheckpoint();
    Equal(SnapshotJson.Serialize(commandDriven.CreateSnapshot(0, revealAll: true)),
        SnapshotJson.Serialize(GameReplay.Restore(commandCheckpoint, commandRegistry)
            .CreateSnapshot(0, revealAll: true)));
}

static void CheckpointRejectsSameVersionContentDrift()
{
    var registry = StandardContentRegistry.Create();
    var game = GameEngine.CreateStandard(
        new GameOptions
        {
            UseInteractiveDiscard = false,
            Seed = 914,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            UseInteractiveSetup = true
        },
        registry);
    var started = game.Submit(new StartGameCommand(game.Revision));
    TrueWithMessage(started.Accepted, "content-drift checkpoint start accepted");
    var checkpoint = game.CreateCheckpoint();

    var driftedRegistry = ContentRegistry.Build(new ModifiedStandardContentPackage());
    Equal(
        checkpoint.ContentPackages.Single(),
        $"{driftedRegistry.Packages.Single().Id}@{driftedRegistry.Packages.Single().Version}");
    TrueWithMessage(
        !string.Equals(checkpoint.ContentHash, driftedRegistry.ContentHash, StringComparison.Ordinal),
        "same-version standard content drift changes the registry hash");
    Throws<InvalidOperationException>(() => GameReplay.Restore(checkpoint, driftedRegistry));
}

static void InitialDealCardZones()
{
    var game = GameEngine.CreateStandard(new GameOptions { UseInteractiveDiscard = false, Seed = 211 }, StandardContentRegistry.Create());
    var cards = game.CreateCardZoneDiagnostics();

    Equal(90, cards.Count);
    Equal(90, cards.Select(card => card.CardId).Distinct().Count());
    Equal(58, cards.Count(card => card.Location == CardLocation.DrawPile));
    Equal(0, cards.Count(card => card.Location == CardLocation.Processing));
    Equal(0, cards.Count(card => card.Location == CardLocation.DiscardPile));

    for (var seat = 0; seat < 8; seat++)
    {
        Equal(4, cards.Count(card => card.Location == CardLocation.Hand(seat)));
    }

    Equal(32, game.CardMovements.Count);
    for (var index = 0; index < game.CardMovements.Count; index++)
    {
        var movement = game.CardMovements[index];
        Equal(index + 1, movement.Sequence);
        Equal(CardLocation.DrawPile, movement.From);
        Equal(CardLocation.Hand(index % 8), movement.To);
        Equal(CardMoveReasons.InitialDeal, movement.Reason);
    }
}

static void InvalidCardMovesAreAtomic()
{
    var store = new CardZoneStore(playerCount: 2);
    store.LoadInitialDeck(
    [
        new Card(1, CardKind.Slash, Suit.Spade, 7),
        new Card(2, CardKind.Dodge, Suit.Heart, 2),
        new Card(3, CardKind.Peach, Suit.Diamond, 3)
    ]);

    var beforeInvalidTarget = store.CreateDiagnostics();
    Throws<ArgumentOutOfRangeException>(() =>
        store.Move(1, CardLocation.DrawPile, CardLocation.Hand(99)));
    True(beforeInvalidTarget.SequenceEqual(store.CreateDiagnostics()));

    store.Move(1, CardLocation.DrawPile, CardLocation.Hand(0));
    var beforeInvalidBatch = store.CreateDiagnostics();
    Throws<InvalidOperationException>(() =>
        store.MoveMany([2, 1], CardLocation.DrawPile, CardLocation.Hand(1)));
    True(beforeInvalidBatch.SequenceEqual(store.CreateDiagnostics()));
    True(store.CardsAt(CardLocation.DrawPile) is not List<Card>);
    store.AssertInvariants(expectedCardCount: 3);
}

static void SlashAndDodgeProcessing()
{
    GameEngine? selectedGame = null;
    for (var seed = 1; seed <= 128 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 150
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        if (HasSkill(game, "standard:yuanhu"))
        {
            continue;
        }

        var decisions = 0;
        while (result.Status != EngineStatus.Completed && decisions++ < 400)
        {
            if (result.Status == EngineStatus.AwaitingHumanResponse)
            {
                var processingCards = game.CreateCardZoneDiagnostics()
                    .Where(card => card.Location == CardLocation.Processing)
                    .ToArray();
                if (result.PendingDecision is
                    { Kind: DecisionKind.RespondDodge, IncomingCard: CardKind.Slash } &&
                    processingCards.Length == 1 &&
                    processingCards[0].CardKind == CardKind.Slash)
                {
                    selectedGame = game;
                    break;
                }

                result = result.PendingDecision?.Kind == DecisionKind.RespondDodge
                    ? game.DriveHumanRespond(useDodge: false)
                    : game.DriveHumanRespondSlash(useSlash: false);
                continue;
            }

            if (result.Status == EngineStatus.AwaitingHumanDying)
            {
                result = game.DriveHumanRespondDying(usePeach: false);
                continue;
            }

            if (result.Status == EngineStatus.AwaitingHumanCardSelection)
            {
                result = ResolveFirstHarvestChoice(game);
                continue;
            }

            result = result.Status == EngineStatus.AwaitingHumanPlay
                ? game.DriveHumanEndPlay()
                : game.DriveAdvanceOneStep();
        }
    }

    NotNull(selectedGame);
    var pendingGame = selectedGame!;
    var before = pendingGame.State;
    Equal(1, before.ProcessingCardCount);
    var slash = pendingGame.CreateCardZoneDiagnostics()
        .Single(card => card.Location == CardLocation.Processing);
    Equal(CardKind.Slash, slash.CardKind);

    var dodgeId = before.Players.Single(player => player.Seat == 0).Hand
        .First(card => card.Kind == CardKind.Dodge).Id;
    var after = pendingGame.DriveHumanRespond(useDodge: true, advanceToHumanBoundary: false);
    Equal(0, after.State.ProcessingCardCount);
    AssertCardInventory(pendingGame);

    True(pendingGame.CardMovements.Any(movement =>
        movement.CardId == slash.CardId &&
        movement.From.Zone == CardZoneKind.Hand &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.Use));
    True(pendingGame.CardMovements.Any(movement =>
        movement.CardId == slash.CardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.UseFinished));
    True(pendingGame.CardMovements.Any(movement =>
        movement.CardId == dodgeId &&
        movement.From == CardLocation.Hand(0) &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.Respond));
    True(pendingGame.CardMovements.Any(movement =>
        movement.CardId == dodgeId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.ResponseFinished));
}

static void CardInventoryConservation()
{
    // Fixed, previously verified matches retain inventory assertions at every
    // boundary. Specific movement and nested-window cases have focused fixtures.
    foreach (var seed in new[] { 1, 7, 13 })
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 100
        }, StandardContentRegistry.Create());
        game.StateChanged += AssertPublishedCardTotal;

        var result = game.DriveStart();
        AssertCardInventory(game);
        var steps = 0;
        while (result.Status != EngineStatus.Completed && steps++ < 800)
        {
            if (game.PendingDecision?.Kind == DecisionKind.ProgramTrigger)
            {
                result = ResolveIncidentalProgramTrigger(game);
            }
            else if (result.Status == EngineStatus.AwaitingHumanPlay)
            {
                var action = game.GetHumanLegalActions()
                    .FirstOrDefault(candidate => candidate.Kind != LegalActionKind.EndPlay &&
                        candidate.CardId is not null);
                result = action is null
                    ? game.DriveHumanEndPlay(advanceToHumanBoundary: false)
                    : action.Kind == LegalActionKind.Recast
                        ? game.Submit(new RecastCardCommand(0, action.CardId!.Value, game.Revision, game.PendingDecision!.PromptId)).Result
                    : DrivePublishedPlayAction(game, action);
            }
            else if (result.Status == EngineStatus.AwaitingHumanResponse)
            {
                result = game.PendingDecision?.Kind == DecisionKind.RespondSlash
                    ? game.DriveHumanRespondSlash(useSlash: true, advanceToHumanBoundary: false)
                    : game.DriveHumanRespond(useDodge: true, advanceToHumanBoundary: false);
            }
            else if (result.Status == EngineStatus.AwaitingHumanDying)
            {
                result = game.DriveHumanRespondDying(usePeach: false, advanceToHumanBoundary: false);
            }
            else if (result.Status == EngineStatus.AwaitingHumanCardSelection)
            {
                result = ResolveFirstHarvestChoice(game);
            }
            else
            {
                result = game.DriveAdvanceOneStep();
            }

            AssertCardInventory(game);
        }

        TrueWithMessage(result.Status == EngineStatus.Completed && steps < 800,
            $"seed {seed} did not complete the inventory match within {steps} steps");
    }
}

static void ObserversRunPostCommit()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 331,
        HumanSeat = 0,
        HumanRole = Role.Lord
    }, StandardContentRegistry.Create());
    var observedStatuses = new List<EngineStatus>();
    var publishedSnapshots = new List<GameSnapshot>();
    game.LogAdded += _ => observedStatuses.Add(game.State.Status);
    game.CardMoved += _ => observedStatuses.Add(game.State.Status);
    game.StateChanged += snapshot => publishedSnapshots.Add(snapshot);

    var result = game.DriveStart();

    Equal(EngineStatus.AwaitingHumanPlay, result.Status);
    True(observedStatuses.Count > 0);
    True(observedStatuses.All(status => status == EngineStatus.AwaitingHumanPlay));
    Equal(1, publishedSnapshots.Count);
    Equal(SnapshotJson.Serialize(result.State), SnapshotJson.Serialize(publishedSnapshots[0]));
}

static void TypedEventStream()
{
    var options = new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 929,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        MaxTurns = 80
    };
    var left = GameEngine.CreateStandard(options, StandardContentRegistry.Create());
    var right = GameEngine.CreateStandard(options, StandardContentRegistry.Create());
    var observedStatuses = new List<EngineStatus>();
    left.EventCommitted += _ => observedStatuses.Add(left.State.Status);

    var leftResult = left.DriveStart();
    var rightResult = right.DriveStart();

    True(left.Events.Count > 0);
    True(left.Events[0].Payload is GameStartedEvent started && started.PlayerCount == 8);
    Equal(left.Events.Count, left.Events.Select(eventItem => eventItem.Id).Distinct().Count());
    True(left.Events.Select(eventItem => eventItem.Sequence)
        .SequenceEqual(Enumerable.Range(1, left.Events.Count).Select(value => (long)value)));
    True(left.Events.All(eventItem => eventItem.Revision == left.Revision));
    True(left.Events.Any(eventItem => eventItem.Payload is CardMovedEvent));
    True(left.Events.Any(eventItem => eventItem.Payload is TurnStartedEvent));
    True(observedStatuses.Count > 0);
    True(observedStatuses.All(status => status == leftResult.Status));
    True(!left.SerializeState().Contains("Events", StringComparison.Ordinal));

    var leftSignature = left.Events.Select(eventItem =>
        $"{eventItem.Sequence}|{eventItem.Payload.GetType().Name}|{eventItem.Payload}");
    var rightSignature = right.Events.Select(eventItem =>
        $"{eventItem.Sequence}|{eventItem.Payload.GetType().Name}|{eventItem.Payload}");
    True(leftSignature.SequenceEqual(rightSignature));
    Equal(leftResult.Revision, rightResult.Revision);
}

static void ResolutionFrameStack()
{
    GameEngine? selectedGame = null;
    EngineRunResult result = null!;
    for (var seed = 1; seed <= 128 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 150
        }, StandardContentRegistry.Create());
        result = game.DriveStart();
        if (HasSkill(game, "standard:yuanhu"))
        {
            continue;
        }

        var steps = 0;
        while (result.Status != EngineStatus.Completed && steps++ < 500)
        {
            if (game.PendingDecision?.Kind == DecisionKind.ProgramTrigger)
            {
                result = ResolveIncidentalProgramTrigger(game);
                continue;
            }
            if (result.Status == EngineStatus.AwaitingHumanResponse)
            {
                var processingCards = game.CreateCardZoneDiagnostics()
                    .Where(card => card.Location == CardLocation.Processing)
                    .ToArray();
                if (result.PendingDecision is
                    { Kind: DecisionKind.RespondDodge, IncomingCard: CardKind.Slash } &&
                    processingCards.Length == 1 &&
                    processingCards[0].CardKind == CardKind.Slash)
                {
                    selectedGame = game;
                    break;
                }

                result = result.PendingDecision?.Kind == DecisionKind.RespondDodge
                    ? game.DriveHumanRespond(useDodge: false)
                    : game.DriveHumanRespondSlash(useSlash: false);
                continue;
            }

            result = result.Status switch
            {
                EngineStatus.AwaitingHumanPlay => game.DriveHumanEndPlay(),
                EngineStatus.AwaitingHumanDying => game.DriveHumanRespondDying(usePeach: false),
                EngineStatus.AwaitingHumanCardSelection => ResolveFirstHarvestChoice(game),
                _ => game.DriveAdvanceOneStep()
            };
        }
    }

    NotNull(selectedGame);
    var gameWithResolution = selectedGame!;
    Equal(2, gameWithResolution.ResolutionStack.Count);
    RuntimeFrameStoreChecks.VerifyBoundaries(gameWithResolution);
    var originalResolutionId = gameWithResolution.ResolutionStack[0].Id;
    TrueWithMessage(gameWithResolution.ResolutionStack[0] is CardUseFrame cardUse &&
         cardUse.Step == ResolutionFrameStep.AwaitingResponse,
         $"card frame: {gameWithResolution.ResolutionStack[0]}");
    TrueWithMessage(gameWithResolution.ResolutionStack[1] is ResponseWindowFrame responseWindow &&
         responseWindow.ParentFrameId == gameWithResolution.ResolutionStack[0].Id,
         $"response frame: {gameWithResolution.ResolutionStack[1]}");
    var serializedStack = JsonSerializer.Serialize(gameWithResolution.ResolutionStack);
    TrueWithMessage(serializedStack.Contains("response-window", StringComparison.Ordinal), "response-window serialized");
    TrueWithMessage(!gameWithResolution.SerializeState().Contains("ResolutionStack", StringComparison.Ordinal), "frame stack omitted from public state");

    var prompt = gameWithResolution.PendingDecision!;
    var decline = prompt.Choices.Single(choice => choice.Parameters["response"] == "take-damage");
    var accepted = gameWithResolution.Submit(new AnswerPromptCommand(
        0,
        prompt.PromptId,
        decline.Id,
        gameWithResolution.Revision));
    TrueWithMessage(accepted.Accepted, "decline accepted");
    for (var step = 0; step < 32 &&
         gameWithResolution.ResolutionStack.Any(frame => frame.Id == originalResolutionId); step++)
    {
        if (gameWithResolution.PendingDecision?.Kind == DecisionKind.ProgramTrigger)
        {
            _ = ResolveIncidentalProgramTrigger(gameWithResolution);
        }
        else
        {
            _ = gameWithResolution.DriveAdvanceOneStep();
        }
    }
    TrueWithMessage(gameWithResolution.ResolutionStack.All(frame => frame.Id != originalResolutionId), "original resolution released");
    TrueWithMessage(gameWithResolution.Events.Any(eventItem => eventItem.Payload is CardUseDeclaredEvent), "card use declared");
    TrueWithMessage(gameWithResolution.Events.Any(eventItem => eventItem.Payload is DamageRequestedEvent), "damage requested");
    TrueWithMessage(gameWithResolution.Events.Any(eventItem => eventItem.Payload is AfterDamageEvent), "after damage");
    TrueWithMessage(gameWithResolution.Events.Any(eventItem => eventItem.Payload is CardUseFinishedEvent), "card use finished");
}

static void DuelResponseFlow()
{
    GameEngine? selectedGame = null;
    PendingDecision? duelPrompt = null;
    for (var seed = 1; seed <= 512 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 220
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        var steps = 0;
        while (result.Status != EngineStatus.Completed && steps++ < 2_500)
        {
            if (game.PendingDecision?.Kind == DecisionKind.ProgramTrigger)
            {
                result = ResolveIncidentalProgramTrigger(game);
                continue;
            }
            if (result.Status == EngineStatus.AwaitingHumanPlay)
            {
                var duel = game.GetHumanLegalActions()
                    .FirstOrDefault(action => action.Kind == LegalActionKind.Duel);
                result = duel is null
                    ? game.DriveHumanEndPlay(advanceToHumanBoundary: false)
                    : game.DriveHumanPlay(duel.CardId!.Value, duel.TargetSeat, advanceToHumanBoundary: false);
                continue;
            }

            if (result.Status == EngineStatus.AwaitingHumanResponse)
            {
                if (game.PendingDecision is
                    { Kind: DecisionKind.RespondSlash, IncomingCard: CardKind.Duel })
                {
                    selectedGame = game;
                    duelPrompt = game.PendingDecision;
                    break;
                }

                result = game.DriveHumanRespond(useDodge: false, advanceToHumanBoundary: false);
                continue;
            }

            result = result.Status == EngineStatus.AwaitingHumanDying
                ? game.DriveHumanRespondDying(usePeach: false, advanceToHumanBoundary: false)
                : result.Status == EngineStatus.AwaitingHumanCardSelection
                    ? ResolveFirstHarvestChoice(game)
                : game.DriveAdvanceOneStep();
        }
    }

    if (selectedGame is null || duelPrompt is null)
    {
        throw new InvalidOperationException("No deterministic Duel Slash-response prompt was found.");
    }

    var gameWithDuel = selectedGame!;
    var prompt = duelPrompt!;
    Equal(DecisionKind.RespondSlash, prompt.Kind);
    Equal(CardKind.Duel, prompt.IncomingCard);
    Equal(CardKind.Slash, prompt.RequiredCardKind);
    TrueWithMessage(prompt.Choices.Any(choice => choice.Parameters["response"] == "slash"), "duel Slash choice");
    TrueWithMessage(gameWithDuel.ResolutionStack[^1] is ResponseWindowFrame response &&
         response.IncomingCard == CardKind.Duel &&
         response.RequiredCardKind == CardKind.Slash, "duel response frame");
    var duelFrame = gameWithDuel.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardKind == CardKind.Duel);
    var otherViewer = gameWithDuel.CreateSnapshot(1);
    Equal<PendingDecision?>(null, otherViewer.PendingDecision);

    var beforeInvalid = gameWithDuel.SerializeState();
    var invalid = gameWithDuel.Submit(new AnswerPromptCommand(
        0,
        prompt.PromptId,
        new ChoiceId("respond.slash.fake"),
        gameWithDuel.Revision));
    TrueWithMessage(!invalid.Accepted, "invalid Duel response rejected");
    Equal(CommandErrorCode.InvalidChoice, invalid.Error!.Code);
    Equal(beforeInvalid, gameWithDuel.SerializeState());

    var slashChoice = prompt.Choices.First(choice => choice.Parameters["response"] == "slash");
    var accepted = gameWithDuel.Submit(new AnswerPromptCommand(
        0,
        prompt.PromptId,
        slashChoice.Id,
        gameWithDuel.Revision));
    TrueWithMessage(accepted.Accepted, "Duel Slash response accepted");
    TrueWithMessage(gameWithDuel.Events.Any(eventItem =>
        eventItem.Payload is DuelResponseEvent response &&
        response.ResolutionId == duelFrame.Id &&
        response.ResponderSeat == 0 &&
        response.UsedSlash), "Duel Slash response event");

    var resultAfterResponse = accepted.Result;
    var stepsAfterResponse = 0;
    while (!gameWithDuel.Events.Any(eventItem =>
               eventItem.Payload is CardUseFinishedEvent finished &&
               finished.ResolutionId == duelFrame.Id) &&
           resultAfterResponse.Status != EngineStatus.Completed &&
           stepsAfterResponse++ < 2_500)
    {
        if (resultAfterResponse.Status == EngineStatus.AwaitingHumanPlay)
        {
            resultAfterResponse = gameWithDuel.DriveHumanEndPlay(advanceToHumanBoundary: false);
        }
        else if (resultAfterResponse.Status == EngineStatus.AwaitingHumanResponse)
        {
            resultAfterResponse = gameWithDuel.PendingDecision?.Kind == DecisionKind.RespondSlash
                ? gameWithDuel.DriveHumanRespondSlash(useSlash: false, advanceToHumanBoundary: false)
                : gameWithDuel.DriveHumanRespond(useDodge: false, advanceToHumanBoundary: false);
        }
        else if (resultAfterResponse.Status == EngineStatus.AwaitingHumanDying)
        {
            resultAfterResponse = gameWithDuel.DriveHumanRespondDying(usePeach: false, advanceToHumanBoundary: false);
        }
        else if (resultAfterResponse.Status == EngineStatus.AwaitingHumanCardSelection)
        {
            resultAfterResponse = ResolveFirstHarvestChoice(gameWithDuel);
        }
        else
        {
            resultAfterResponse = gameWithDuel.DriveAdvanceOneStep();
        }
    }

    TrueWithMessage(gameWithDuel.Events.Any(eventItem =>
        eventItem.Payload is DuelResponseEvent response &&
        response.ResolutionId == duelFrame.Id &&
        !response.UsedSlash), "Duel pass event");
    TrueWithMessage(gameWithDuel.Events.Any(eventItem =>
        eventItem.Payload is DamageRequestedEvent damage &&
        damage.ResolutionId != 0 &&
        damage.SourceCard == CardKind.Duel), "Duel damage event");
    TrueWithMessage(gameWithDuel.Events.Any(eventItem =>
        eventItem.Payload is CardUseFinishedEvent finished &&
        finished.ResolutionId == duelFrame.Id &&
        finished.CardKind == CardKind.Duel), "Duel finished event");
    TrueWithMessage(
        gameWithDuel.ResolutionStack.All(frame => frame.Id != duelFrame.Id),
        $"Duel frame completed (count={gameWithDuel.ResolutionStack.Count}, status={gameWithDuel.State.Status}, pending={gameWithDuel.PendingDecision?.Kind})");
    Equal(
        CardLocation.DiscardPile,
        gameWithDuel.CreateCardZoneDiagnostics()
            .Single(card => card.CardId == duelFrame.CardId)
            .Location);
}

static void NullificationFlow()
{
    GameEngine? aiGame = null;
    for (var seed = 1; seed <= 128 && aiGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = -1,
            HumanRole = null,
            MaxTurns = 250
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        if (result.Status == EngineStatus.Completed &&
            game.Events.Any(eventItem => eventItem.Payload is NullificationRespondedEvent))
        {
            aiGame = game;
        }
    }

    NotNull(aiGame);
    var resolvedGame = aiGame!;
    var responses = resolvedGame.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<NullificationRespondedEvent>()
        .ToArray();
    True(responses.Length > 0);
    var resolvedEffects = resolvedGame.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<NullificationResolvedEvent>()
        .ToArray();
    True(resolvedEffects.Any(effect => effect.EffectNullified));
    foreach (var responseGroup in responses.GroupBy(response =>
                 (response.ResolutionId, response.EffectCardId, response.WindowFrameId)))
    {
        var ordered = responseGroup.OrderBy(response => response.ChainDepth).ToArray();
        var resolved = resolvedEffects.Single(effect =>
            effect.ResolutionId == responseGroup.Key.ResolutionId &&
            effect.EffectCardId == responseGroup.Key.EffectCardId && effect.WindowFrameId == responseGroup.Key.WindowFrameId);
        Equal(ordered.Length, resolved.ChainDepth);
        Equal(ordered.Length % 2 == 1, resolved.EffectNullified);
        True(ordered.Select(response => response.ChainDepth)
            .SequenceEqual(Enumerable.Range(1, ordered.Length)));
        foreach (var response in ordered)
        {
            True(resolvedGame.CardMovements.Any(movement =>
                movement.CardId == response.NullificationCardId &&
                movement.From == CardLocation.Hand(response.ResponderSeat) &&
                movement.To == CardLocation.Processing &&
                movement.Reason == CardMoveReasons.Nullification));
            True(resolvedGame.CardMovements.Any(movement =>
                movement.CardId == response.NullificationCardId &&
                movement.From == CardLocation.Processing &&
                movement.To == CardLocation.DiscardPile &&
                movement.Reason == CardMoveReasons.NullificationFinished));
        }
    }
    AssertCardInventory(resolvedGame);

    GameEngine? humanGame = null;
    PendingDecision? humanPrompt = null;
    for (var seed = 1; seed <= 8_192 && humanGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 180
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        if (result.Status != EngineStatus.AwaitingHumanPlay)
        {
            continue;
        }

        var action = game.GetHumanLegalActions().FirstOrDefault(candidate =>
            candidate.Kind is LegalActionKind.DrawTwo or
                LegalActionKind.BarbarianAssault or
                LegalActionKind.ArrowBarrage or
                LegalActionKind.PeachGarden or
                LegalActionKind.FiveGrains or
                LegalActionKind.Dismantlement or
                LegalActionKind.Snatch or
                LegalActionKind.FireAttack or
                LegalActionKind.Duel);
        if (action is null)
        {
            continue;
        }

        _ = game.DriveHumanPlay(
            action.CardId!.Value,
            action.TargetSeat,
            advanceToHumanBoundary: false,
            targetCardId: action.TargetCardId);
        for (var step = 0; step < 256; step++)
        {
            if (game.PendingDecision?.Kind == DecisionKind.Nullification)
            {
                humanGame = game;
                humanPrompt = game.PendingDecision;
                break;
            }

            if (!game.ResolutionStack.Any(frame => frame is NullificationWindowFrame))
            {
                break;
            }

            game.DriveAdvanceOneStep();
        }
    }

    NotNull(humanGame);
    NotNull(humanPrompt);
    var privateGame = humanGame!;
    var privatePrompt = humanPrompt!;
    Equal(DecisionKind.Nullification, privatePrompt.Kind);
    Equal(CardKind.Nullification, privatePrompt.RequiredCardKind);
    True(privatePrompt.ValidCardIds.Count > 0);
    True(privatePrompt.Choices.Any(choice =>
        choice.Parameters.GetValueOrDefault("response") == "nullification"));
    Equal<PendingDecision?>(null, privateGame.CreateSnapshot(1).PendingDecision);
    var window = privateGame.ResolutionStack.OfType<NullificationWindowFrame>().Single();
    Equal(privatePrompt.IncomingCard, window.EffectCardKind);
    Equal(privatePrompt.SourceSeat, window.SourceSeat);
    True(JsonSerializer.Serialize(privateGame.ResolutionStack)
        .Contains("nullification-window", StringComparison.Ordinal));

    var beforeInvalid = privateGame.SerializeState();
    var invalid = privateGame.Submit(new AnswerPromptCommand(
        0,
        privatePrompt.PromptId,
        new ChoiceId("nullification.fake"),
        privateGame.Revision));
    False(invalid.Accepted);
    Equal(CommandErrorCode.InvalidChoice, invalid.Error!.Code);
    Equal(beforeInvalid, privateGame.SerializeState());

    var useChoice = privatePrompt.Choices.First(choice =>
        choice.Parameters.GetValueOrDefault("response") == "nullification");
    var usedCardId = useChoice.Cards.Single();
    var accepted = privateGame.Submit(new AnswerPromptCommand(
        0,
        privatePrompt.PromptId,
        useChoice.Id,
        privateGame.Revision));
    True(accepted.Accepted);
    ResolveNullificationWindowForTest(privateGame);
    True(privateGame.Events.Any(eventItem =>
        eventItem.Payload is NullificationRespondedEvent response &&
        response.ResponderSeat == 0 &&
        response.NullificationCardId == usedCardId));
    False(SnapshotJson.Serialize(privateGame.CreateSnapshot(1))
        .Contains($"\"Id\": {usedCardId},", StringComparison.Ordinal));
    AssertCardInventory(privateGame);
}

static void PeachGardenFlow()
{
    GameEngine? selectedGame = null;
    LegalAction? selectedGarden = null;
    CardUseFrame? groupFrame = null;
    var injuredSeat = -1;

    for (var seed = 1; seed <= 512 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 180
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        var garden = game.GetHumanLegalActions()
            .FirstOrDefault(candidate => candidate.Kind == LegalActionKind.PeachGarden);
        var slash = game.GetHumanLegalActions()
            .FirstOrDefault(candidate =>
                candidate.Kind == LegalActionKind.Slash &&
                candidate.TargetSeat is { } targetSeat &&
                !game.CreateSnapshot(targetSeat).Players.Single(player => player.Seat == targetSeat).Hand
                    .Any(card => card.Kind == CardKind.Dodge));
        if (result.Status != EngineStatus.AwaitingHumanPlay || garden is null || slash is null)
        {
            continue;
        }

        var targetSeatForTest = slash.TargetSeat!.Value;
        game.DriveHumanPlay(slash.CardId!.Value, targetSeatForTest, advanceToHumanBoundary: false);
        ResolveNullificationWindowForTest(game);
        var targetAfterSlash = game.State.Players.Single(player => player.Seat == targetSeatForTest);
        if (targetAfterSlash.Hp != targetAfterSlash.MaxHp - 1)
        {
            continue;
        }

        result = game.DriveAdvanceOneStep();
        garden = game.GetHumanLegalActions()
            .FirstOrDefault(candidate => candidate.Kind == LegalActionKind.PeachGarden);
        if (result.Status != EngineStatus.AwaitingHumanPlay || garden is null)
        {
            continue;
        }

        game.DriveHumanPlay(garden.CardId!.Value, null, advanceToHumanBoundary: false);
        ResolveNullificationWindowForTest(game);
        var currentFrame = game.ResolutionStack.OfType<CardUseFrame>()
            .SingleOrDefault(frame => frame.CardKind == CardKind.PeachGarden);
        if (currentFrame is not null &&
            currentFrame.TargetIndex == 0 &&
            currentFrame.TargetSeats.Count == 8 &&
            game.State.ProcessingCardCount == 1)
        {
            selectedGame = game;
            selectedGarden = garden;
            groupFrame = currentFrame;
            injuredSeat = targetSeatForTest;
        }
    }

    if (selectedGame is null || selectedGarden is null || groupFrame is null || injuredSeat < 0)
    {
        throw new InvalidOperationException("No deterministic PeachGarden recovery boundary was found.");
    }

    var gameWithGarden = selectedGame!;
    var frame = groupFrame!;
    Equal(8, frame.TargetSeats.Count);
    Equal(0, frame.TargetIndex);
    Equal(1, gameWithGarden.State.ProcessingCardCount);
    Equal<PendingDecision?>(null, gameWithGarden.State.PendingDecision);
    Equal<PendingDecision?>(null, gameWithGarden.CreateSnapshot(injuredSeat).PendingDecision);
    True(!gameWithGarden.SerializeState().Contains("ResolutionStack", StringComparison.Ordinal));
    TrueWithMessage(
        gameWithGarden.ResolutionStack[^1] is CardUseFrame cardUse &&
        cardUse.CardKind == CardKind.PeachGarden &&
        cardUse.TargetIndex == 0,
        "PeachGarden card-use frame is paused before the first target");

    var firstStep = gameWithGarden.DriveAdvanceOneStep();
    firstStep = ResolveNullificationWindowForTest(gameWithGarden, firstStep);
    if (gameWithGarden.ResolutionStack.OfType<CardUseFrame>().Single().TargetIndex == 0)
        firstStep = gameWithGarden.DriveAdvanceOneStep();
    Equal(EngineStatus.Running, firstStep.Status);
    TrueWithMessage(
        gameWithGarden.ResolutionStack.OfType<CardUseFrame>().Single().TargetIndex == 1,
        "PeachGarden advances one target per recovery step after that target's counterspell window");
    Equal(1, gameWithGarden.State.ProcessingCardCount);

    True(gameWithGarden.Events.Any(eventItem =>
        eventItem.Payload is GroupCardUsedEvent used &&
        used.ResolutionId == frame.Id &&
        used.CardKind == CardKind.PeachGarden &&
        used.TargetSeats.SequenceEqual(frame.TargetSeats)));

    var steps = 0;
    while (!gameWithGarden.Events.Any(eventItem =>
               eventItem.Payload is CardUseFinishedEvent finished &&
               finished.ResolutionId == frame.Id) &&
           steps++ < 100)
    {
        gameWithGarden.DriveAdvanceOneStep();
        ResolveNullificationWindowForTest(gameWithGarden);
    }

    TrueWithMessage(
        gameWithGarden.Events.Any(eventItem =>
            eventItem.Payload is CardUseFinishedEvent finished &&
            finished.ResolutionId == frame.Id &&
            finished.CardKind == CardKind.PeachGarden),
        "PeachGarden finished event");
    var recovered = gameWithGarden.State.Players.Single(player => player.Seat == injuredSeat);
    Equal(recovered.MaxHp, recovered.Hp);
    TrueWithMessage(
        gameWithGarden.Events.Any(eventItem =>
            eventItem.Payload is RecoveryAppliedEvent recovery &&
            recovery.SourceSeat == 0 &&
            recovery.TargetSeat == injuredSeat &&
            recovery.Amount == 1 &&
            recovery.RemainingHp == recovered.MaxHp),
        "PeachGarden recovery event");
    True(!gameWithGarden.Events.Any(eventItem =>
        eventItem.Payload is DamageRequestedEvent damage &&
        damage.SourceCard == CardKind.PeachGarden));
    True(gameWithGarden.CardMovements.Any(movement =>
        movement.CardId == frame.CardId &&
        movement.From == CardLocation.Hand(0) &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.Use));
    True(gameWithGarden.CardMovements.Any(movement =>
        movement.CardId == frame.CardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.UseFinished));
    Equal(0, gameWithGarden.ResolutionStack.Count);
    Equal(
        CardLocation.DiscardPile,
        gameWithGarden.CreateCardZoneDiagnostics()
            .Single(card => card.CardId == frame.CardId)
            .Location);
    AssertCardInventory(gameWithGarden);
}

static void FiveGrainsFlow()
{
    GameEngine? selectedGame = null;
    CardUseFrame? draftFrame = null;
    EngineRunResult? afterUse = null;
    for (var seed = 1; seed <= 512 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 180
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        var action = game.GetHumanLegalActions()
            .FirstOrDefault(candidate => candidate.Kind == LegalActionKind.FiveGrains);
        if (result.Status != EngineStatus.AwaitingHumanPlay || action is null)
        {
            continue;
        }

        result = game.DriveHumanPlay(action.CardId!.Value, null, advanceToHumanBoundary: false);
        result = ResolveNullificationWindowForTest(game, result);
        var candidateFrame = game.ResolutionStack.OfType<CardUseFrame>()
            .SingleOrDefault(candidate => candidate.CardKind == CardKind.FiveGrains);
        if (result.Status == EngineStatus.AwaitingHumanCardSelection &&
            candidateFrame is not null &&
            candidateFrame.TargetSeats.Count == 8 &&
            candidateFrame.TargetIndex == 0 &&
            result.State.PublicRevealedCards.Count == 8)
        {
            selectedGame = game;
            draftFrame = candidateFrame;
            afterUse = result;
        }
    }

    if (selectedGame is null || draftFrame is null || afterUse is null)
    {
        throw new InvalidOperationException("No deterministic FiveGrains public draft boundary was found.");
    }

    var gameWithDraft = selectedGame!;
    var frame = draftFrame!;
    var startedState = afterUse!.State;
    var prompt = gameWithDraft.PendingDecision!;
    Equal(DecisionKind.SelectHarvestCard, prompt.Kind);
    Equal(EngineStatus.AwaitingHumanCardSelection, startedState.Status);
    Equal(0, frame.TargetIndex);
    Equal(9, startedState.ProcessingCardCount);
    Equal(8, startedState.PublicRevealedCards.Count);
    Equal(8, prompt.ValidCardIds.Count);
    True(prompt.ValidCardIds.OrderBy(id => id)
        .SequenceEqual(startedState.PublicRevealedCards.Select(card => card.Id).OrderBy(id => id)));
    Equal(prompt.ValidCardIds.Count, prompt.Choices.Count);
    True(prompt.Choices.All(choice =>
        choice.Parameters.GetValueOrDefault("action") == "harvest-pick" &&
        choice.Cards.Count == 1 &&
        prompt.ValidCardIds.Contains(choice.Cards[0])));

    var otherViewer = gameWithDraft.CreateSnapshot(1);
    Equal<PendingDecision?>(null, otherViewer.PendingDecision);
    Equal(8, otherViewer.PublicRevealedCards.Count);
    True(otherViewer.Players.Single(player => player.Seat == 0).Hand.Count == 0);
    True(startedState.Players.Single(player => player.Seat == 0).Hand
        .Where(card => !prompt.ValidCardIds.Contains(card.Id))
        .All(card => otherViewer.Players.Single(player => player.Seat == 0).Hand.All(hidden => hidden.Id != card.Id)));
    True(gameWithDraft.SerializeState().Contains("PublicRevealedCards", StringComparison.Ordinal));
    True(!gameWithDraft.SerializeState().Contains("ResolutionStack", StringComparison.Ordinal));
    True(gameWithDraft.ResolutionStack[^1] is CardUseFrame cardUse &&
         cardUse.CardKind == CardKind.FiveGrains &&
         cardUse.TargetIndex == 0);
    True(gameWithDraft.Events.Any(eventItem =>
        eventItem.Payload is CardsRevealedEvent revealed &&
        revealed.ResolutionId == frame.Id &&
        revealed.Cards.Count == 8 &&
        revealed.Cards.Select(card => card.Id).SequenceEqual(startedState.PublicRevealedCards.Select(card => card.Id))));
    True(gameWithDraft.CardMovements.Any(movement =>
        movement.From == CardLocation.DrawPile &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.Reveal));

    var beforeInvalid = gameWithDraft.SerializeState();
    var invalid = gameWithDraft.Submit(new AnswerPromptCommand(
        0,
        prompt.PromptId,
        new ChoiceId("harvest.card-fake"),
        gameWithDraft.Revision));
    False(invalid.Accepted);
    Equal(CommandErrorCode.InvalidChoice, invalid.Error!.Code);
    Equal(beforeInvalid, gameWithDraft.SerializeState());

    var selectedCardId = prompt.Choices[0].Cards.Single();
    var afterHumanPick = gameWithDraft.DriveHumanSelectHarvestCard(
        selectedCardId,
        advanceToHumanBoundary: false);
    Equal(EngineStatus.Running, afterHumanPick.Status);
    Equal<PendingDecision?>(null, afterHumanPick.State.PendingDecision);
    Equal(7, afterHumanPick.State.PublicRevealedCards.Count);
    Equal(8, afterHumanPick.State.ProcessingCardCount);
    Equal(1, afterHumanPick.State.Players.Single(player => player.Seat == 0).Hand.Count(card => card.Id == selectedCardId));
    Equal(DecisionKind.SelectHarvestCard, gameWithDraft.CreateSnapshot(1).PendingDecision!.Kind);
    Equal<PendingDecision?>(null, gameWithDraft.CreateSnapshot(2).PendingDecision);

    var guard = 0;
    while (gameWithDraft.ResolutionStack.OfType<CardUseFrame>()
               .Any(cardUse => cardUse.Id == frame.Id) &&
           guard++ < 32)
    {
        True(gameWithDraft.State.Status == EngineStatus.Running);
        gameWithDraft.DriveAdvanceOneStep();
    }

    TrueWithMessage(guard < 32, "FiveGrains draft completes within one step per picker");
    TrueWithMessage(gameWithDraft.Events.Any(eventItem =>
        eventItem.Payload is CardUseFinishedEvent finished &&
        finished.ResolutionId == frame.Id &&
        finished.CardKind == CardKind.FiveGrains), "FiveGrains finished event");
    var selections = gameWithDraft.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<HarvestCardSelectedEvent>()
        .Where(selection => selection.ResolutionId == frame.Id)
        .ToArray();
    Equal(frame.TargetSeats.Count, selections.Length);
    True(selections.Select(selection => selection.PlayerSeat).SequenceEqual(frame.TargetSeats));
    True(selections.All(selection =>
        gameWithDraft.CreateCardZoneDiagnostics()
            .Single(card => card.CardId == selection.CardId)
            .Location == CardLocation.Hand(selection.PlayerSeat)));
    True(gameWithDraft.CardMovements.Any(movement =>
        movement.CardId == selectedCardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.Hand(0) &&
        movement.Reason == CardMoveReasons.HarvestPick));
    True(gameWithDraft.CardMovements.Any(movement =>
        movement.CardId == frame.CardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.UseFinished));
    Equal(0, gameWithDraft.State.PublicRevealedCards.Count);
    Equal(0, gameWithDraft.State.ProcessingCardCount);
    Equal(0, gameWithDraft.ResolutionStack.Count);
    AssertCardInventory(gameWithDraft);
}

static void FireAttackFlow()
{
    GameEngine? selectedGame = null;
    LegalAction? selectedAction = null;
    PendingDecision? revealPrompt = null;
    PendingDecision? discardPrompt = null;
    CardUseFrame? selectedFrame = null;
    for (var seed = 1; seed <= 8_192 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 180
        }, StandardContentRegistry.Create());
        var result = game.DriveStart();
        if (HasSkill(game, "standard:yuanhu"))
        {
            continue;
        }

        if (result.Status != EngineStatus.AwaitingHumanPlay)
        {
            continue;
        }

        var action = game.GetHumanLegalActions()
            .FirstOrDefault(candidate =>
                candidate.Kind == LegalActionKind.FireAttack &&
                candidate.TargetSeat is not null and not 0);
        if (action is null)
        {
            continue;
        }

        var targetSeat = action.TargetSeat!.Value;
        var targetSkills = game.CreateSnapshot(0, revealAll: true).Players
            .Single(player => player.Seat == targetSeat).Skills;
        if (targetSkills?.Any(skill => skill.ContentId is "standard:yiji" or "standard:jieming" or "standard:yuanhu") == true)
        {
            continue;
        }

        result = game.DriveHumanPlay(action.CardId!.Value, targetSeat, advanceToHumanBoundary: false);
        if (result.Status != EngineStatus.Running || game.PendingDecision is not null)
        {
            continue;
        }

        var targetView = game.CreateSnapshot(targetSeat);
        var candidateRevealPrompt = targetView.PendingDecision;
        if (candidateRevealPrompt is not { Kind: DecisionKind.FireAttackReveal } ||
            candidateRevealPrompt.ValidCardIds.Count == 0)
        {
            continue;
        }

        var sourceViewBeforeReveal = game.CreateSnapshot(0);
        Equal<PendingDecision?>(null, sourceViewBeforeReveal.PendingDecision);
        foreach (var cardId in candidateRevealPrompt.ValidCardIds)
        {
            False(SnapshotJson.Serialize(sourceViewBeforeReveal).Contains(
                $"\"Id\": {cardId},",
                StringComparison.Ordinal));
        }

        var candidateFrame = game.ResolutionStack.OfType<CardUseFrame>()
            .SingleOrDefault(frame => frame.CardKind == CardKind.FireAttack);
        if (candidateFrame is null)
        {
            continue;
        }

        result = game.DriveAdvanceOneStep();
        var candidateDiscardPrompt = game.PendingDecision;
        if (result.Status != EngineStatus.AwaitingHumanCardSelection ||
            candidateDiscardPrompt is not { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 0 } ||
            !candidateDiscardPrompt.Choices.Any(choice =>
                choice.Parameters.GetValueOrDefault("response") == "fire-attack-discard"))
        {
            continue;
        }

        if (game.State.PublicRevealedCards.Count != 1 ||
            game.State.ProcessingCardCount != 1)
        {
            continue;
        }

        selectedGame = game;
        selectedAction = action;
        revealPrompt = candidateRevealPrompt;
        discardPrompt = candidateDiscardPrompt;
        selectedFrame = candidateFrame;
    }

    if (selectedGame is null || selectedAction is null || revealPrompt is null ||
        discardPrompt is null || selectedFrame is null)
    {
        throw new InvalidOperationException("No deterministic FireAttack damage boundary was found.");
    }

    var gameWithFireAttack = selectedGame!;
    var actionToUse = selectedAction!;
    var frame = selectedFrame!;
    var targetSeatAtBoundary = actionToUse.TargetSeat!.Value;
    var revealedEvent = gameWithFireAttack.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<FireAttackCardRevealedEvent>()
        .Single(eventItem => eventItem.ResolutionId == frame.Id);
    var discardChoice = discardPrompt.Choices.First(choice =>
        choice.Parameters.GetValueOrDefault("response") == "fire-attack-discard");
    var matchingDiscardCardId = discardChoice.Cards.Single();
    var sourceBefore = gameWithFireAttack.CreateSnapshot(0, revealAll: true)
        .Players.Single(player => player.Seat == 0);
    var targetBefore = gameWithFireAttack.CreateSnapshot(0, revealAll: true)
        .Players.Single(player => player.Seat == targetSeatAtBoundary);
    var ordinaryViewer = gameWithFireAttack.CreateSnapshot(1);
    TrueWithMessage(ordinaryViewer.PublicRevealedCards.Count == 1, "FireAttack publishes the revealed card");
    TrueWithMessage(targetBefore.Hand.Any(card => card.Id == revealedEvent.CardId),
        "formal FireAttack keeps the revealed card in the target hand");
    TrueWithMessage(
        ordinaryViewer.Players.SelectMany(player => player.Hand).All(card => card.Id != matchingDiscardCardId),
        "source discard remains private in an ordinary viewer");

    var invalid = gameWithFireAttack.Submit(new AnswerPromptCommand(
        0,
        discardPrompt.PromptId,
        new ChoiceId("fire-attack.fake"),
        gameWithFireAttack.Revision));
    False(invalid.Accepted);
    Equal(CommandErrorCode.InvalidChoice, invalid.Error!.Code);

    var accepted = gameWithFireAttack.Submit(new AnswerPromptCommand(
        0,
        discardPrompt.PromptId,
        discardChoice.Id,
        gameWithFireAttack.Revision));
    TrueWithMessage(accepted.Accepted, "FireAttack discard choice accepted");
    accepted = gameWithFireAttack.Submit(new AdvanceCommand(gameWithFireAttack.Revision));
    TrueWithMessage(accepted.Accepted, "FireAttack discard continuation accepted");
    Equal(EngineStatus.AwaitingHumanPlay, accepted.State.Status);
    Equal(0, accepted.State.ProcessingCardCount);
    Equal(0, gameWithFireAttack.ResolutionStack.Count);
    Equal(0, accepted.State.PublicRevealedCards.Count);

    var resolved = gameWithFireAttack.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<FireAttackResolvedEvent>()
        .Single(eventItem => eventItem.ResolutionId == frame.Id);
    Equal(revealedEvent.CardId, resolved.RevealedCardId);
    Equal(revealedEvent.Suit, resolved.RevealedSuit);
    Equal(matchingDiscardCardId, resolved.MatchingDiscardCardId);
    True(resolved.CausedDamage);
    TrueWithMessage(gameWithFireAttack.Events.Any(eventItem =>
        eventItem.Payload is DamageRequestedEvent damage &&
        damage.SourceSeat == 0 &&
        damage.TargetSeat == targetSeatAtBoundary &&
        damage.Amount == 1 &&
        damage.SourceCard == CardKind.FireAttack &&
        damage.Nature == DamageNature.Fire), "FireAttack emits typed fire damage");
    var targetAfter = gameWithFireAttack.CreateSnapshot(0, revealAll: true)
        .Players.Single(player => player.Seat == targetSeatAtBoundary);
    Equal(targetBefore.Hp - 1, targetAfter.Hp);
    Equal(targetBefore.HandCount, targetAfter.HandCount);
    TrueWithMessage(targetAfter.Hand.Any(card => card.Id == revealedEvent.CardId),
        "formal FireAttack reveal remains in hand after damage");
    var sourceAfter = gameWithFireAttack.CreateSnapshot(0, revealAll: true)
        .Players.Single(player => player.Seat == 0);
    TrueWithMessage(
        sourceBefore.HandCount - 1 == sourceAfter.HandCount,
        $"FireAttack source hand expected {sourceBefore.HandCount - 1}, actual {sourceAfter.HandCount}, " +
        $"discard card {matchingDiscardCardId} location " +
        $"{gameWithFireAttack.CreateCardZoneDiagnostics().Single(card => card.CardId == matchingDiscardCardId).Location}");

    TrueWithMessage(gameWithFireAttack.CardMovements.Any(movement =>
        movement.CardId == actionToUse.CardId &&
        movement.From == CardLocation.Hand(0) &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.Use), "FireAttack effect enters Processing");
    False(gameWithFireAttack.CardMovements.Any(movement =>
        movement.CardId == revealedEvent.CardId &&
        movement.From == CardLocation.Hand(targetSeatAtBoundary) &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.FireAttackReveal));
    TrueWithMessage(gameWithFireAttack.CardMovements.Any(movement =>
        movement.CardId == matchingDiscardCardId &&
        movement.From == CardLocation.Hand(0) &&
        movement.To == CardLocation.Processing &&
        movement.Reason == CardMoveReasons.FireAttackDiscard), "FireAttack same-suit discard movement");
    False(gameWithFireAttack.CardMovements.Any(movement =>
        movement.CardId == revealedEvent.CardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.FireAttackFinished));
    TrueWithMessage(gameWithFireAttack.CardMovements.Any(movement =>
        movement.CardId == matchingDiscardCardId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.FireAttackDiscardFinished), "FireAttack source discard finishes");
    TrueWithMessage(gameWithFireAttack.Events.Any(eventItem =>
        eventItem.Payload is CardUseFinishedEvent finished &&
        finished.ResolutionId == frame.Id &&
        finished.CardKind == CardKind.FireAttack), "FireAttack finished event");
    TrueWithMessage(gameWithFireAttack.AiThoughts.Any(thought =>
        thought.Candidates.Any(candidate =>
            candidate.Action.Kind == LegalActionKind.FireAttackReveal &&
            candidate.Reason.Contains("自己的私有手牌", StringComparison.Ordinal))),
        "FireAttack reveal AI uses its private snapshot");
    AssertCardInventory(gameWithFireAttack);
}

static void DyingResponseFlow()
{
    GameEngine? selectedGame = null;
    PendingDecision? dyingPrompt = null;
    EngineRunResult result = null!;
    for (var seed = 1; seed <= 512 && selectedGame is null; seed++)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false,
            Seed = seed,
            HumanSeat = 0,
            HumanRole = Role.Lord,
            MaxTurns = 180
        }, StandardContentRegistry.Create());
        result = game.DriveStart();
        var steps = 0;
        while (result.Status != EngineStatus.Completed && steps++ < 2_000)
        {
            if (result.Status == EngineStatus.AwaitingHumanDying)
            {
                var prompt = game.PendingDecision;
                if (prompt is { Choices.Count: > 1 } &&
                    prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "peach"))
                {
                    selectedGame = game;
                    dyingPrompt = prompt;
                    break;
                }
            }

            result = result.Status switch
            {
                EngineStatus.AwaitingHumanPlay => game.DriveHumanEndPlay(advanceToHumanBoundary: false),
                EngineStatus.AwaitingHumanResponse => game.PendingDecision?.Kind == DecisionKind.RespondSlash
                    ? game.DriveHumanRespondSlash(useSlash: false, advanceToHumanBoundary: false)
                    : game.DriveHumanRespond(useDodge: false, advanceToHumanBoundary: false),
                EngineStatus.AwaitingHumanDying => game.DriveHumanRespondDying(usePeach: false, advanceToHumanBoundary: false),
                EngineStatus.AwaitingHumanCardSelection => ResolveFirstHarvestChoice(game),
                _ => game.DriveAdvanceOneStep()
            };
        }
    }

    if (selectedGame is null || dyingPrompt is null)
    {
        throw new InvalidOperationException("No human dying prompt with a Peach was found in the deterministic seed search.");
    }
    var gameWithDying = selectedGame!;
    var promptAtBoundary = dyingPrompt!;
    Equal(EngineStatus.AwaitingHumanDying, gameWithDying.State.Status);
    Equal(DecisionKind.RescueDying, promptAtBoundary.Kind);
    Equal(promptAtBoundary.TargetSeat, promptAtBoundary.SourceSeat);
    TrueWithMessage(promptAtBoundary.Choices.Any(choice => choice.Parameters["response"] == "peach"), "peach choice");
    TrueWithMessage(gameWithDying.ResolutionStack[^1] is DyingFrame dyingFrame &&
         gameWithDying.ResolutionStack[^2] is DamageFrame damageFrame &&
         dyingFrame.ParentFrameId == damageFrame.Id, "dying stack");
    var serializedDyingStack = JsonSerializer.Serialize(gameWithDying.ResolutionStack);
    TrueWithMessage(serializedDyingStack.Contains("dying", StringComparison.Ordinal), "dying stack serialization");

    var otherViewer = gameWithDying.CreateSnapshot(1);
    Equal<PendingDecision?>(null, otherViewer.PendingDecision);
    var hiddenPeachId = promptAtBoundary.Choices
        .First(choice => choice.Parameters["response"] == "peach")
        .Cards.Single();
    TrueWithMessage(otherViewer.Players.All(player => player.Hand.All(card => card.Id != hiddenPeachId)), "other viewer hides peach");

    var beforeInvalidChoice = gameWithDying.SerializeState();
    var invalidChoice = gameWithDying.Submit(new AnswerPromptCommand(
        gameWithDying.State.HumanSeat,
        promptAtBoundary.PromptId,
        new ChoiceId("dying.fake"),
        gameWithDying.Revision));
    TrueWithMessage(!invalidChoice.Accepted, "invalid dying choice rejected");
    Equal(CommandErrorCode.InvalidChoice, invalidChoice.Error!.Code);
    Equal(beforeInvalidChoice, gameWithDying.SerializeState());

    var originalRevision = gameWithDying.Revision;
    var peachChoice = promptAtBoundary.Choices
        .First(choice => choice.Parameters["response"] == "peach");
    var rescued = gameWithDying.Submit(new AnswerPromptCommand(
        gameWithDying.State.HumanSeat,
        promptAtBoundary.PromptId,
        peachChoice.Id,
        originalRevision));
    TrueWithMessage(rescued.Accepted, "peach command accepted");
    TrueWithMessage(gameWithDying.Events.Any(eventItem =>
        eventItem.Payload is DyingResponseEvent response &&
        response.UsedPeach && response.PeachCardId == hiddenPeachId), "dying peach event");
    TrueWithMessage(gameWithDying.Events.Any(eventItem =>
        eventItem.Payload is DyingResolvedEvent resolved && resolved.Survived), "dying survived event");
    TrueWithMessage(gameWithDying.ResolutionStack.All(frame =>
        frame is not DyingFrame dying || dying.VictimSeat != promptAtBoundary.TargetSeat),
        "completed dying frame removed");
    TrueWithMessage(gameWithDying.CardMovements.Any(movement =>
        movement.CardId == hiddenPeachId &&
        movement.From == CardLocation.Processing &&
        movement.To == CardLocation.DiscardPile &&
        movement.Reason == CardMoveReasons.UseFinished),
        "rescue Peach discarded through the recovery resolution");
    var recovery = gameWithDying.Events
        .Select(eventItem => eventItem.Payload)
        .OfType<RecoveryAppliedEvent>()
        .LastOrDefault(eventItem =>
            eventItem.TargetSeat == promptAtBoundary.TargetSeat &&
            eventItem.Amount == 1);
    NotNull(recovery);
    TrueWithMessage(recovery!.RemainingHp > 0,
        $"dying Peach recovery applied at hp={recovery.RemainingHp}");
}

static void ObserverFailuresAreIsolated()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        AdvanceAfterHumanCommands = false,
        UseInteractiveDiscard = false,
        Seed = 337,
        HumanSeat = 0,
        HumanRole = Role.Lord,
        MaxTurns = 100
    }, StandardContentRegistry.Create());
    var deliveredLogs = 0;
    var deliveredThoughts = 0;
    var deliveredStates = 0;
    var deliveredMovements = 0;

    game.LogAdded += _ => throw new InvalidOperationException("log observer failed");
    game.LogAdded += _ => deliveredLogs++;
    game.AiThoughtAdded += _ => throw new InvalidOperationException("thought observer failed");
    game.AiThoughtAdded += _ => deliveredThoughts++;
    game.StateChanged += _ => throw new InvalidOperationException("state observer failed");
    game.StateChanged += _ => deliveredStates++;
    game.CardMoved += _ => throw new InvalidOperationException("movement observer failed");
    game.CardMoved += _ => deliveredMovements++;

    var result = game.DriveStart();
    Equal(EngineStatus.AwaitingHumanPlay, result.Status);
    True(deliveredLogs > 0);
    True(deliveredStates > 0);
    True(deliveredMovements > 0);

    result = game.DriveHumanEndPlay(advanceToHumanBoundary: false);
    for (var step = 0; step < 8 && deliveredThoughts == 0; step++)
    {
        result = game.DriveAdvanceOneStep();
    }

    True(deliveredThoughts > 0);
    True(game.ObserverFailures.Any(failure => failure.NotificationType == nameof(game.LogAdded)));
    True(game.ObserverFailures.Any(failure => failure.NotificationType == nameof(game.AiThoughtAdded)));
    True(game.ObserverFailures.Any(failure => failure.NotificationType == nameof(game.StateChanged)));
    True(game.ObserverFailures.Any(failure => failure.NotificationType == nameof(game.CardMoved)));
    AssertCardInventory(game);
    True(result.Status != EngineStatus.NotStarted);
}

static void ReentrantAdvanceIsRejected()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        UseInteractiveDiscard = false,
        Seed = 71,
        HumanSeat = 0,
        HumanRole = Role.Lord
    }, StandardContentRegistry.Create());
    var rejected = false;
    game.LogAdded += entry =>
    {
        if (entry.Type != "CardsDrawn")
        {
            return;
        }

        rejected = game.Submit(new AdvanceCommand(game.Revision)).Error?.Code ==
            CommandErrorCode.ReentrantOperation;
    };

    var result = game.DriveStart();
    True(rejected);
    Equal(EngineStatus.AwaitingHumanPlay, result.Status);
    Equal(Winner.None, result.Winner);
    Equal(TurnPhase.Play, result.State.Phase);
}

static void UnknownPhaseFailsFast()
{
    var game = GameEngine.CreateStandard(new GameOptions
    {
        AdvanceAfterHumanCommands = false,
        UseInteractiveDiscard = false,
        Seed = 79,
        HumanSeat = 0,
        HumanRole = Role.Lord
    }, StandardContentRegistry.Create());
    game.DriveStart();
    game.DriveHumanEndPlay(advanceToHumanBoundary: false);

    var phaseField = typeof(GameEngine).GetField("_phase", BindingFlags.Instance | BindingFlags.NonPublic);
    NotNull(phaseField);
    phaseField!.SetValue(game, (TurnPhase)999);

    Throws<InvalidOperationException>(() => game.DriveAdvanceOneStep());
}

static void SnapshotSerialization()
{
    var game = GameEngine.CreateStandard(new GameOptions { UseInteractiveDiscard = false, Seed = 123 }, StandardContentRegistry.Create());
    using var json = JsonDocument.Parse(game.SerializeState());
    Equal("Lord", json.RootElement.GetProperty("Players")[0].GetProperty("Role").GetString());
    Equal(8, json.RootElement.GetProperty("Players").GetArrayLength());
}

static bool HasSkill(GameEngine game, string skill) =>
    game.CreateSnapshot(0, revealAll: true).Players.Any(player => player.Skills?.Any(entry => entry.ContentId == skill) == true);

static void AssertCardInventory(GameEngine game)
{
    var cards = game.CreateCardZoneDiagnostics();
    Equal(cards.Count, cards.Select(card => card.CardId).Distinct().Count());

    var snapshot = game.CreateSnapshot(0, revealAll: true);
    Equal(snapshot.DrawPileCount, cards.Count(card => card.Location == CardLocation.DrawPile));
    Equal(snapshot.DiscardPileCount, cards.Count(card => card.Location == CardLocation.DiscardPile));
    Equal(snapshot.ProcessingCardCount, cards.Count(card => card.Location == CardLocation.Processing));
    foreach (var player in snapshot.Players)
    {
        var handCards = cards
            .Where(card => card.Location == CardLocation.Hand(player.Seat))
            .Select(card => card.CardId)
            .OrderBy(id => id)
            .ToArray();
        var snapshotCards = player.Hand.Select(card => card.Id).OrderBy(id => id).ToArray();
        Equal(player.HandCount, handCards.Length);
        TrueWithMessage(
            handCards.SequenceEqual(snapshotCards),
            $"hand snapshot mismatch for seat {player.Seat}: zone=[{string.Join(',', handCards)}], snapshot=[{string.Join(',', snapshotCards)}]");

        var equipmentCards = cards
            .Where(card => card.Location == CardLocation.Equipment(player.Seat))
            .OrderBy(card => card.CardId)
            .ToArray();
        var snapshotEquipment = player.Equipment
            .OrderBy(card => card.Id)
            .ToArray();
        Equal(equipmentCards.Length, player.Equipment.Count);
        TrueWithMessage(
            equipmentCards.Select(card => card.CardId).SequenceEqual(snapshotEquipment.Select(card => card.Id)),
            $"equipment snapshot mismatch for seat {player.Seat}: zone=[{string.Join(',', equipmentCards.Select(card => card.CardId))}], snapshot=[{string.Join(',', snapshotEquipment.Select(card => card.Id))}]");
        TrueWithMessage(
            equipmentCards.All(card => EquipmentCatalog.IsEquipment(card.CardKind)),
            $"non-equipment card in equipment zone for seat {player.Seat}");
        TrueWithMessage(
            equipmentCards.Select(card => EquipmentCatalog.Get(card.CardKind).Slot).Distinct().Count() == equipmentCards.Length,
            $"duplicate equipment slot for seat {player.Seat}");

        var judgmentCards = cards
            .Where(card => card.Location == CardLocation.Judgment(player.Seat))
            .OrderBy(card => card.CardId)
            .ToArray();
        var snapshotJudgment = player.Judgment
            .OrderBy(card => card.Id)
            .ToArray();
        Equal(judgmentCards.Length, snapshotJudgment.Length);
        TrueWithMessage(
            judgmentCards.Select(card => card.CardId).SequenceEqual(snapshotJudgment.Select(card => card.Id)),
            $"judgment snapshot mismatch for seat {player.Seat}: zone=[{string.Join(',', judgmentCards.Select(card => card.CardId))}], snapshot=[{string.Join(',', snapshotJudgment.Select(card => card.Id))}]");
        TrueWithMessage(
            judgmentCards.All(card =>
                card.CardKind is CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning ||
                game.ResolutionStack.OfType<JudgmentFrame>()
                    .Any(frame => frame.CardId == card.CardId)),
            $"unexpected card in judgment zone for seat {player.Seat}");
    }

    if (snapshot.Status == EngineStatus.Completed)
    {
        Equal(0, snapshot.ProcessingCardCount);
    }
}

static EngineRunResult ResolveFirstHarvestChoice(GameEngine game)
{
    var prompt = game.PendingDecision ??
        throw new InvalidOperationException("The test expected a human FiveGrains prompt.");
    var choice = prompt.Choices.FirstOrDefault() ??
        throw new InvalidOperationException("The test expected at least one FiveGrains choice.");
    return prompt.Kind switch
    {
        DecisionKind.SelectTargetCard => game.DriveHumanSelectTargetCardSlot(
            int.Parse(choice.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture),
            advanceToHumanBoundary: false),
        DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard => game.DriveHumanSelectFireAttackCard(
            choice.Cards.Single(),
            advanceToHumanBoundary: false),
        _ => game.DriveHumanSelectHarvestCard(choice.Cards.Single(), advanceToHumanBoundary: false)
    };
}

static EngineRunResult ResolveIncidentalProgramTrigger(GameEngine game)
{
    var prompt = game.PendingDecision ??
        throw new InvalidOperationException("The test expected a published skill trigger.");
    Equal(DecisionKind.ProgramTrigger, prompt.Kind);
    var choice = prompt.Choices.FirstOrDefault(candidate =>
            candidate.Parameters.GetValueOrDefault("program-action") == "skip") ??
        prompt.Choices.FirstOrDefault(candidate =>
            candidate.Parameters.GetValueOrDefault("program-action") == "activate") ??
        prompt.Choices.FirstOrDefault() ??
        throw new InvalidOperationException("The published skill trigger has no executable choice.");
    var answer = game.Submit(new AnswerPromptCommand(
        prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    TrueWithMessage(answer.Accepted, "published skill trigger choice accepted");
    return answer.Result;
}

static EngineRunResult DrivePublishedPlayAction(GameEngine game, LegalAction action)
{
    var prompt = game.PendingDecision ??
        throw new InvalidOperationException("The test expected a published play prompt.");
    var accepted = game.Submit(new PlayCardCommand(
        prompt.PlayerSeat, action.CardId!.Value, action.TargetSeats,
        game.Revision, prompt.PromptId, action.PlayedCardKind, action.TargetCardId)
    {
        ConversionSource = action.ConversionSource,
        AdditionalConversionSources = action.AdditionalConversionSources,
    });
    TrueWithMessage(accepted.Accepted, "published exact play action accepted");
    return accepted.Result;
}

static EngineRunResult ResolveNullificationWindowForTest(
    GameEngine game,
    EngineRunResult? initial = null)
{
    var result = initial ?? new EngineRunResult(
        game.State.Status,
        game.State.Winner,
        game.State,
        game.PendingDecision,
        game.Revision);
    for (var step = 0;
         step < 256 && game.ResolutionStack.Any(frame =>
             frame is NullificationWindowFrame or TargetCardSelectionFrame);
         step++)
    {
        result = game.PendingDecision?.Kind switch
        {
            DecisionKind.Nullification => game.DriveHumanRespondNullification(
                useNullification: false,
                advanceToHumanBoundary: false),
            DecisionKind.SelectTargetCard => game.DriveHumanSelectTargetCardSlot(
                int.Parse(
                    game.PendingDecision.Choices.First().Parameters["slot-index"],
                    System.Globalization.CultureInfo.InvariantCulture),
                advanceToHumanBoundary: false),
            _ => game.DriveAdvanceOneStep()
        };
    }

    if (game.ResolutionStack.Any(frame =>
            frame is NullificationWindowFrame or TargetCardSelectionFrame))
    {
        throw new InvalidOperationException("The test Nullification window did not settle.");
    }

    return result;
}

static void AssertPublishedCardTotal(GameSnapshot snapshot)
{
    Equal(
        90,
        snapshot.DrawPileCount +
        snapshot.DiscardPileCount +
        snapshot.ProcessingCardCount +
        snapshot.Players.Sum(player =>
            player.HandCount + player.Equipment.Count + player.Judgment.Count));
}

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected {expected}, got {actual}.");
    }
}

static void True(bool condition)
{
    if (!condition)
    {
        throw new InvalidOperationException("Expected condition to be true.");
    }
}

static void TrueWithMessage(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException($"Expected condition to be true: {message}.");
    }
}

static void False(bool condition)
{
    if (condition)
    {
        throw new InvalidOperationException("Expected condition to be false.");
    }
}

static void NotNull(object? value)
{
    if (value is null)
    {
        throw new InvalidOperationException("Expected a non-null value.");
    }
}

static void Throws<TException>(Action action)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name} to be thrown.");
}

static void ThrowsFor<TException>(Action action, string expectation)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException(
        $"Expected {typeof(TException).Name} to be thrown for {expectation}.");
}

sealed class SyntheticPackage : IGameContentPackage
{
    private readonly Action<IContentRegistryBuilder> _register;

    public SyntheticPackage(
        string id,
        Action<IContentRegistryBuilder> register,
        params PackageDependency[] dependencies)
    {
        _register = register;
        Manifest = new PackageManifest(id, new Version(1, 0, 0), dependencies);
    }

    public PackageManifest Manifest { get; }

    public void Register(IContentRegistryBuilder builder) => _register(builder);
}


sealed class ModifiedStandardContentPackage : IGameContentPackage
{
    private readonly StandardContentPackage _inner = new();

    public PackageManifest Manifest => _inner.Manifest;

    public void Register(IContentRegistryBuilder builder) =>
        _inner.Register(new ModifiedStandardContentBuilder(builder));
}


sealed class ModifiedStandardContentBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
{
    public void AddModeFeatureFactory(IGameModeFeatureFactory factory) => inner.AddModeFeatureFactory(factory);
    public bool HasModeFeatureFactory(string id) => inner.HasModeFeatureFactory(id);
    public void AddEquipmentContent(EquipmentContentPackage package) => inner.AddEquipmentContent(package);

    public void AddCard(ContentCardDefinition definition) => inner.AddCard(
        definition.Id == "standard:slash"
            ? definition with { Description = $"{definition.Description}（同版本内容漂移）" }
            : definition);

    public void AddSkill(ContentSkillDefinition definition) => inner.AddSkill(definition);

    public void AddGeneral(ContentGeneralDefinition definition) => inner.AddGeneral(definition);

    public void AddDeck(ContentDeckRecipe definition) => inner.AddDeck(definition);

    public void AddMode(ContentModeDefinition definition) => inner.AddMode(definition);
}
