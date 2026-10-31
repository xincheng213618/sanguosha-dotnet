#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardUseCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareZongluanSlashAction(global::CardGame.Core.CardActionContext action);
    void BindZongluanSlashUse(long id);
}

internal interface ICoreNativeCardUseCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueMouZhaJiangUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreNativeCardUseCharacterZhangXiuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouZhangXiuAnniversarySlashAction(global::CardGame.Core.CardActionContext action);
    void BindMouZhangXiuAnniversarySlashUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindLeiluanNativeUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindJianjiNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreNonBasicSlashProfileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueNonBasicSlashPolicy(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindZhangYanSujiUse(long id);
}

internal interface ICoreNativeCardUseCharacterDuanJiongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindDuanJiongSaoguNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareChainedStateBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ChainedStateBasicPayment? payment);
    void IssueChainedStateBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChainedStateBasicPayment? payment);
}

internal interface ICoreNativeCardUseCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindRoundOnceCardlessProgramRequestUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareZonghuAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ZonghuPaidOrigin? p);
    void IssueZonghuUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ZonghuPaidOrigin? p);
}

internal interface ICoreNativeCardUseCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind);
}

internal interface ICoreNativeCardUseCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueProvenanceUsePolicy(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeCardUseCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashPolicyForColor(int actor, global::CardGame.Core.CardKind kind, bool? color);
}

internal interface ICoreNativeCardUseCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareWeijieAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.WeijiePaidOrigin? p);
    void IssueWeijieUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WeijiePaidOrigin? p);
}

internal interface ICoreNativeCardUseCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareHuanpeiAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.HuanpeiPaidOrigin? p);
    void IssueHuanpeiUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HuanpeiPaidOrigin? p);
    void BindWenyiNativeUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMoQuanyuQuota(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterZhangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindZhangJiNativeUse(long useId);
}

internal interface ICoreNativeCardUseCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouZuifengAlcoholUse(global::CardGame.Core.CardUseFrame supplied);
    void IssueMouGuquUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanFirstRedAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNativeCardUseCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindBoundaryLiuFengNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindQingyaNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueBeifenUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouLiuChengAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> MoGangquanNativeTargets(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<int> original);
}

internal interface ICoreNativeCardUseCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueHuiyunUse(long id, global::CardGame.Core.CardActionContext? a);
    void BindHuiyunFollowupUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindSuyiEquipmentUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterCaoAngMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouCaNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindWuJingHejiUse(long id);
}

internal interface ICoreNativeCardUseCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueWeiwanUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginMouSmSecretUse(long useId, long eventStart, long logStart);
}

internal interface ICoreNativeCardUseCharacterPeiYuanShaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueFulueCounterUse(long useId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouDcAction(global::CardGame.Core.CardActionContext action);
    void BindMouDcUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindChanshuangSlashUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterJiaXuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouJxNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindSayingSlashUse(global::CardGame.Core.CardUseFrame supplied);
    void BindDongxinEntityUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareDiouAction(global::CardGame.Core.CardActionContext action);
    void IssueDiouUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareDrawFundedDistinctBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment);
    void IssueDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment);
}

internal interface ICoreNativeCardUseCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouJsNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindGuoSiAcceptedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouZyNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouXuHuangAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeCardUseCharacterChengYuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouDanchiAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCoreGiftedSlashUseRewardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueGiftedSlashUseBenefit(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ColorFireAttackReceipt? FreezeColorFireAttackPolicy(int sourceSeat, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareShenDaCuixinAction(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreNativeCardUseCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeCardUseCoreNoDamageSkillDisableOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueNoDamageSkillDisablePolicy(long frameId);
}

internal interface ICoreNativeCardUseCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment? TransferDeclarationToCardUse(long useId);
}

internal interface ICoreNativeCardUseCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueRoundGainedUseQualifications(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueShownEntityUseBenefits(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCharacterFuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindFuRongNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouWenChouAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouHuCheErSlashAction(global::CardGame.Core.CardActionContext action);
    void BindMouHuCheErSlashUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareYouqueSlashAction(global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.CardActionContext PrepareXieweiAction(global::CardGame.Core.CardActionContext action);
    void BindYouqueSlashUse(global::CardGame.Core.CardUseFrame supplied);
    void PrepareXieweiNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueWuyouUse(global::CardGame.Core.CardUseFrame use);
    void IssueJuewuUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNativeCardUseCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouLixianNativeUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindQigongNativeUse(long id);
}

internal interface ICoreNativeCardUseCharacterHuLieMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouHuLieSlashAction(global::CardGame.Core.CardActionContext action);
    void BindMouHuLieSlashUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCoreSourceDingxiNativeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindSourceDingxiNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareMouXqJueyanAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeCardUseCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareNativePublicPileUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterChenLinMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouChenLinNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFirstActualPlayUseDistance(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeCardUseCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }
}

internal interface ICoreNativeCardUseCharacterWangYunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindWangYunNativeUse(long useId);
}

internal interface ICoreNativeCardUseCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueGrantedPhaseEntityUseDistance(long frameId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCharacterWangGuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueMiuyanUse(long useId, global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreNativeCardUseCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext PrepareWuLuKangKegouSlashAction(global::CardGame.Core.CardActionContext action);
    void BindWuLuKangKegouSlashUse(global::CardGame.Core.CardUseFrame supplied);
}

internal interface ICoreNativeCardUseCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueKenshangUse(long id, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNativeCardUseCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeCardUseCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueSiruiUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext? CaptureCardUseAction(global::CardGame.Core.Card card, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.CardConversionSource? explicitConversion = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversions = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false);
}

internal interface ICoreNativeCardUseCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindMouJjNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindKeBiNengPingduanNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindCaoChunNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueTieredRoundConversionUse(global::CardGame.Core.CardUseFrame use);
}
