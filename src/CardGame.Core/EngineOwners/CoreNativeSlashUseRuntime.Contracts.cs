#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeSlashUseCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeSlashUseCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default);
}

internal interface ICoreNativeSlashUseCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeSlashUseCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSlashUseCoreRedAdditionalTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextCardTargetAdjustment(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeSlashUseCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeSlashUseCoreForeignSelectedCardSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashSource(long? parentId, int actor, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int physicalOwner, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? materials, bool countsTowardLimit);
    void BindSelectedForeignCardSlashUse(long useId, long? parentId);
}

internal interface ICoreNativeSlashUseCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouLiuChengLateSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryMarkProgramUseCommitted(long frameId);
}

internal interface ICoreNativeSlashUseCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeSlashUseCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoGangquanMultiSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.CharacterState MoGangquanFirstTarget(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState fallback, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNativeSlashUseCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FreezeNativeSlashTargetDuelPlan(global::CardGame.Core.CardUseFrame use);
    bool TryBeginNativeSlashTargetDuel(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id);
}

internal interface ICoreNativeSlashUseCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
    bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat);
}

internal interface ICoreNativeSlashUseCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellRemenImmunity(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeSlashUseCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouSmPrivateWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.MouSmPrivateCall call);
}

internal interface ICoreNativeSlashUseCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeSlashUseCoreNextSlashProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FreezeNextSlashDamage(long frameId);
}

internal interface ICoreNativeSlashUseCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseZhangbaSerpentSpear(global::CardGame.Core.CharacterState actor);
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeSlashUseCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetPenalties(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreNativeSlashUseCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDrawFundedDistinctBasicDodge(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeSlashUseCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag);
}

internal interface ICoreNativeSlashUseCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default);
}

internal interface ICoreNativeSlashUseCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeSlashUseCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeSlashUseCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiMultiTargetUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeSlashUseCoreShenwuEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryApplyShenwuQimen(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeSlashUseCharacterBaiLingYunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryBaiLingYunSlashSource(long? programId, int source, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int provider, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physical, bool counts);
}

internal interface ICoreNativeSlashUseCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
    bool HasIssuedPlayPhaseUseBan(int seat);
}

internal interface ICoreNativeSlashUseCharacterDuKuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDuKuiFanyinSlashSource(long? programId, int source, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int provider, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physical, bool counts);
}

internal interface ICoreNativeSlashUseCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenZfAttackIsColorless(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }
}

internal interface ICoreNativeSlashUseCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetFangtianEffectiveTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, int index);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardAttackState>? PreparedTargetAttacks(long id);
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
    bool TryBeginProgramSlashStage(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.ProgramCardContinuation continuation);
    bool PrepareProgramSlashTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool TryBeginProgramCardUseBeforeTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreSlashTargetBenefitWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreNativeSlashUseCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboUnrespondable(long resolutionId);
}

internal interface ICoreNativeSlashUseCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeShortRangeSlashResponseRequirement(long attackId, int source, int target, global::CardGame.Core.CardKind kind, int ordinary);
}

internal interface ICoreNativeSlashUseCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int responderSeat, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeSlashUseCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeSlashUseCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeSlashUseCoreNativeWeaponResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCixiongDoubleSwordsChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreActorHandLimitTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAcceptedSlashActorHandLimitTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreNativeSlashUseCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeSlashUseCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeSlashUseCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeSlashUseCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreNativeSlashUseCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuanpeiRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeSlashUseCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string PublicDeclarationDescription(global::CardGame.Core.Card card, string ordinaryDescription);
}

internal interface ICoreNativeSlashUseCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TousuiRequiredDodge(long useId, int ordinary);
}

internal interface ICoreNativeSlashUseCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectProgramFireSlashConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::CardGame.Core.GameEngine.NuzhanModifiers GetNuzhanModifiers(long frameId, global::CardGame.Core.CharacterState source);
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreNativeSlashUseCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
    bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
    bool IsSuitSlashResponseProhibited(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
    int GetRequiredResponseCount(global::CardGame.Core.CharacterState skillOwner, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind);
    bool HasBlackSlashBarrier(global::CardGame.Core.CharacterState player);
    bool HasArmor(global::CardGame.Core.CharacterState player);
    bool HasTengjia(global::CardGame.Core.CharacterState player);
    bool HasBagua(global::CardGame.Core.CharacterState player);
    bool CanRequestFactionDefense(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeSlashUseCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreNativeSlashUseCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLsHandAccess(int actor, int id);
}

internal interface ICoreNativeSlashUseCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PaySlashUseMaterials(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int providerSeat, global::CardGame.Core.CardLocation? sourceLocation);
}

internal interface ICoreNativeSlashUseResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeSlashUseCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ActualUseTargetReturnKind kind);
}

internal interface ICoreNativeSlashUseCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICoreNativeSlashUseCharacterHaoMengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHaoMengNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeSlashUseCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreNativeSlashUseCoreComparedBlackSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyComparedBlackSlashPolicies(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZonghuRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeSlashUseKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, global::CardGame.Core.CardConversionSource? conversionSource = default);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreNativeSlashUseCharacterLiuQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenjiResponseForbidden(long useId, int responderSeat);
}

internal interface ICoreNativeSlashUseCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesProgramCardTargetCount(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card physicalCard, global::CardGame.Core.CardKind effectiveKind, int targetCount);
}

internal interface ICoreNativeSlashUseCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangDrunkenSlashUnrespondable(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCharacterJiangWeiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileJwXinghunSlashSource(long? parentId, int actor, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int physicalOwner, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? materials, bool countsTowardLimit);
    void BindMobileJwXinghunSlashUse(long useId, long? parentId);
}

internal interface ICoreNativeSlashUseCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChainedStateBasicDodge(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeSlashUseCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreNativeSlashUseCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalFangtianHalberd { get; }

    bool UsesFormalTengjia { get; }
}

internal interface ICoreNativeSlashUseCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreNativeSlashUseCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProvidedCardSupplyCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request);
}

internal interface ICoreNativeSlashUseCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeSlashUseCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICoreNativeSlashUseCoreConvertedSlashResponseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasConvertedSlashSameColorResponseRestriction(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool? ConvertedSlashSameColorResponseColor(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeSlashUseCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeSlashUseKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreNativeSlashUseCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreNativeSlashUseCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeSlashUseCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ProgramDodgeResponseChoices(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeSlashUseCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyMoQuanyuAttackPolicy(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateCardTargetCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId = default);
}

internal interface ICoreNativeSlashUseCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueExtraCardEffectFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CaptureExtraCardEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeSlashUseCharacterYuanShaoMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouYuanShaoLateSlashTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}
