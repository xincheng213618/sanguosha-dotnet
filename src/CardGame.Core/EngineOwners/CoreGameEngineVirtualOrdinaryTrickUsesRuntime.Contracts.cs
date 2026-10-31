#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreVirtualOrdinaryTrickUsesCoreActualTurnDamageClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiExtraEffectTargetSubset(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreOverflowTargetCancellationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOverflowTargetCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterZhaoYunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactMobileShiCuifengProgram(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenDaCuixinTrick(long id);
    bool IsShenDaCuixinUse(long id);
    global::CardGame.Core.Card ReadShenDaCuixinAppearance(long id, global::CardGame.Core.CardAppearanceReference a);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreZhaoguMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use);
    void ReturnZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuaManSouyingCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterDengAiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidShiDaVirtualSnatchUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnShiDaVirtualSnatch(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopFinishedCardUse(long frameId);
    void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreVirtualOrdinaryTrickUsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDinghanCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterChengYuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDanchiDrawTwo(long id);
    global::CardGame.Core.Card ReadMouDanchiAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterZhangYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidShiZyVirtualDuelUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnShiZyVirtualDuel(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterLiuLiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterGuoJiaHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    bool HasPendingProgramBoundCards { get; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJjEffectSubset(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterLiuYongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ReadMouDcUseAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterGuoJiaMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnShenGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreVirtualOrdinaryTrickUsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool TryReturnAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> NextActualUseOrdinaryTrickOptions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ordinary, global::CardGame.Core.Suit? suit, bool? color, global::CardGame.Core.Suit? shieldSuit, bool excludeOwner);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellArrivalProgram(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduChoulueVirtualOrdinaryProgram(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuXunVirtualParent(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.SkillProgramEffect MouLuXunVirtualInstruction(global::CardGame.Core.ProgramSkillFrame f);
    bool ReturnMouLuXunNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreVirtualOrdinaryTrickUsesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreVirtualOrdinaryTrickUsesCoreHellKingProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellRandomAssaultBranch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreVirtualOrdinaryTrickUsesCharacterXunYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXyVirtualUse(global::CardGame.Core.CardUseFrame use);
    bool ReturnMouXyVirtualUse(global::CardGame.Core.CardUseFrame use);
}
