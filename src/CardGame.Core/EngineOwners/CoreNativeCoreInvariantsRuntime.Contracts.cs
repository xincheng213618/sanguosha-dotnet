#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCoreInvariantsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertZonghuRequests();
    global::CardGame.Core.PendingDecision? ZonghuInvariantDecision(global::CardGame.Core.PendingDecision? ordinary);
    global::CardGame.Core.ResolutionFrame? ZonghuInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary);
    bool HasZonghuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasZonghuNativeRequestContinuation(long owner);
    bool IsZonghuProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterMuShunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryMuShunPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreDelegatedJudgmentChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDelegatedJudgment(global::CardGame.Core.SkillProgramTrigger trigger);
    bool IsDelegatedJudgmentPromptValid(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.PendingDecision? p);
}

internal interface ICoreNativeCoreInvariantsCharacterGaoXiangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryGaoXiangPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeDelayedAttackChild(long judgmentId);
    bool IsOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame j);
    bool IsOriginalUseLightningAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterHuZhaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHuZhaoPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterWangChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryWangChangPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMantianGuohaiState();
    bool HasMantianGuohaiPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeCoreInvariantsCoreFinalizedJudgmentDamageChildrenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFinalizedJudgmentDamageChildren();
}

internal interface ICoreNativeCoreInvariantsCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterZhangHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryZhangHuanPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreCompletedGiftQinglongRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetCompletedGiftRecipientSlashRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window, out global::CardGame.Core.CardUseFrame child);
}

internal interface ICoreNativeCoreInvariantsCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreNativeCoreInvariantsCoreIdentityAppearanceInvariantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIdentityAppearanceAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeCoreInvariantsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiResponsePending();
    bool IsAiNullificationPending();
}

internal interface ICoreNativeCoreInvariantsCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRoundGainedUseQualifications(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreRecipientScopedDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecipientScopedDamageStates();
}

internal interface ICoreNativeCoreInvariantsCharacterMangYaChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryJiedaoPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileZhitiDuelContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasMobileZhitiContestPrompt();
    bool IsMobileZhitiContestSubtree(global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreNativeCoreInvariantsCoreDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDiscardPromptInvariant();
    bool HasManualDiscardBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCharacterWangYiMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDeckTopJudgmentReplacementPromptValid(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.PendingDecision? decision);
}

internal interface ICoreNativeCoreInvariantsCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? MouLiuChengNativeAncestorProcessing(long useId);
    bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
    bool IsMouLiuChengAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterSunHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversarySunHuanPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreShanheS5XuncanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheS5YunshenUse(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreNativeIceSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertIceSwordLogicalDiscard(global::CardGame.Core.NativeOwners.IceSwordHandle pending);
}

internal interface ICoreNativeCoreInvariantsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetResponseExchangeMovementDyingCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame? window, out int[] costs);
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
    bool HasHuiyunCompletedUseObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryXunXuPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreEquipmentRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasEquipmentRecastNativeDrawBoundary();
}

internal interface ICoreNativeCoreInvariantsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? KenshangNativeOriginalDecision(long ownerId);
    void AssertKenshangRequests();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeCoreInvariantsCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuAnGuoLiyongDuelUse(long id);
    bool IsWuAnGuoLiyongAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreCardEffectProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCardEffectProgramState();
}

internal interface ICoreNativeCoreInvariantsCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWoLongFengChuBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowed);
    bool HasWoLongFengChuUseObserver(long id);
    bool HasWoLongFengChuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeCoreInvariantsCorePaidColorDamageClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPaidColorDamageClaims();
}

internal interface ICoreNativeCoreInvariantsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDeferredTurnEndBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiMultiTargetAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsCoreActualPhaseEndsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualPhaseEndBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCharacterLiuLiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryLiuLiPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHpLossMaterialSlashes();
    bool IsExactHpLossMaterialSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreSourceCurseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSourceCurseState();
}

internal interface ICoreNativeCoreInvariantsCharacterDianWeiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAssertHappyDianWeiDeathRemoval(global::CardGame.Core.DeathFrame death);
    bool HasHappyDianWeiGuardPrompt();
}

internal interface ICoreNativeCoreInvariantsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPindianInvariant();
}

internal interface ICoreNativeCoreInvariantsCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertYingboInvariant();
    global::CardGame.Core.NativeOwners.YingboGiftHandle? ActiveYingboGift { get; set; }
}

internal interface ICoreNativeCoreInvariantsCoreWushuangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangIntermissionBoundary();
}

internal interface ICoreNativeCoreInvariantsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeCoreInvariantsCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageCursorProjection ResolveDamageCursor(global::CardGame.Core.DamageTriggerWindowFrame window);
    global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false);
    bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsCharacterPangTongHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyPangTongPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryMaTiePrompt();
}

internal interface ICoreNativeCoreInvariantsCoreGiftedSlashUseRewardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertGiftedSlashUseBenefit(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDragonBoatCardState();
    bool HasDragonBoatDiscardPrompt();
    bool HasDragonBoatCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool IsDragonBoatZeroAgainstCurrentAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreFactionRequestCostRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request);
}

internal interface ICoreNativeCoreInvariantsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertYintianDamageStates();
}

internal interface ICoreNativeCoreInvariantsCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? TousuiNativeOriginalDecision(long ownerId);
    void AssertTousuiRequests();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeCoreInvariantsCoreSlashTargetPenaltyObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetPenaltyUseObserver(long useId);
    void AssertTargetPenaltyWindows();
}

internal interface ICoreNativeCoreInvariantsCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFriendEventFrames();
    bool HasFriendDuelResponseObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreDamageJudgmentSuitPaymentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDamageJudgmentSuitPaymentRide(long judgment);
}

internal interface ICoreNativeCoreInvariantsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterHeYanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHeYanPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreJudgmentOutcomeClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactJudgmentOutcomeClaimDisposition(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeCoreInvariantsCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetPolicyCounterspellPaymentRide(global::CardGame.Core.NullificationWindowFrame window, out int[] rescueCosts);
}

internal interface ICoreNativeCoreInvariantsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeCaoZhiWenlanRescueContinuation(long dyingId);
    bool HasLeCaoZhiWenlanUseContinuation(long nativeId);
}

internal interface ICoreNativeCoreInvariantsCharacterGuoZhaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryGuoZhaoPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreRoundPricedPileAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNativeCoreInvariantsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? MouXqNativeAncestorProcessing(long id);
    bool HasMouXqJueyanResponseContinuation(long parentId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long parentId, bool qinglong);
    bool IsMouXqJueyanAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreGuanduChariotResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreNativeCoreInvariantsCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShenwuCardState();
    bool HasShenwuTrickPrompt();
    bool IsShenwuTransplantAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPostEventProgramInvariants();
}

internal interface ICoreNativeCoreInvariantsCharacterLeiBoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryLeiBoPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterCheZhouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCheZhouPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreYaodaoChitongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactYaodaoChitongJudgmentSubtree(long judgmentId);
}

internal interface ICoreNativeCoreInvariantsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMoQuanyuSlashUses();
}

internal interface ICoreNativeCoreInvariantsCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendRecastDuelUse(long id);
}

internal interface ICoreNativeCoreInvariantsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNativeCoreInvariantsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFinalTargetSlashReceipts();
}

internal interface ICoreNativeCoreInvariantsCharacterHouChengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHouChengPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecoveryReplacementProgramDying(long? requiredPaidUseId = default);
}

internal interface ICoreNativeCoreInvariantsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertXieweiPayments(global::CardGame.Core.CardUseFrame use);
    bool IsXieweiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCoreRoundOnceCardlessReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundOnceCardlessProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNativeCardEffectDraw(global::CardGame.Core.CardUseFrame use);
    bool HasNativeCardEffectDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? MouZyNativeAncestorProcessing(long id);
    bool IsMouZyZeroFireAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder);
}

internal interface ICoreNativeCoreInvariantsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCoreUnexpectedAssaultOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertUnexpectedAssaultReceipts();
}

internal interface ICoreNativeCoreInvariantsCharacterLuXunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeGroupEffectAttackObserver(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeCoreInvariantsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertKemingEchoFrames();
    bool HasKemingCixiongContinuation(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending);
    bool HasKemingEchoPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreRecordedSuitSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecordedSuitSlashReceipts();
}

internal interface ICoreNativeCoreInvariantsCharacterLuZhiMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouLzDirectUseContinuation(long id);
}

internal interface ICoreNativeCoreInvariantsCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateAssistedFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreNativeCoreInvariantsCharacterJiangWeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenJwAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreActualDiscardRecoveryObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertActualEquipmentOrDiscardPrograms();
}

internal interface ICoreNativeCoreInvariantsCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNativeEquipmentInvocations();
    global::CardGame.Core.GameEngine.NativeEquipmentSuspensionProjection? FindNativeEquipmentInvocation(long attackId);
    bool HasFactionDefenseEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense);
    bool HasQinglongFactionEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request);
    bool HasNativeEquipmentInvocation(long attackId);
}

internal interface ICoreNativeCoreInvariantsCoreColdDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertColdDamageReplacementFrames();
    bool HasColdDamageReplacement(long attackId);
    bool HasColdDamagePrompt();
}

internal interface ICoreNativeCoreInvariantsCoreOwnedDamagePointJudgmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOwnedDamagePointJudgmentRide(long judgmentFrameId);
}

internal interface ICoreNativeCoreInvariantsCoreSlashTargetBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashTargetBenefitUseObserver(long useId);
    void AssertSlashTargetBenefitWindows();
}

internal interface ICoreNativeCoreInvariantsCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFeiYiEffectRounds();
}

internal interface ICoreNativeCoreInvariantsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertLeChengyanEffects();
    bool HasLeChengyanBorrowedPrompt();
}

internal interface ICoreNativeCoreInvariantsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterQiaoRuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryQiaoRuiPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleIncomingRangeDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsSuppressedCardMovedEventBatchIdsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Stack<long> _suppressedCardMovedEventBatchIds { get; }
}

internal interface ICoreNativeCoreInvariantsCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor();
    global::CardGame.Core.TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor();
}

internal interface ICoreNativeCoreInvariantsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactHappyBonePlacement(global::CardGame.Core.Card card, int holder);
}

internal interface ICoreNativeCoreInvariantsCoreInspectedHandSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertInspectedHandSlashes();
}

internal interface ICoreNativeCoreInvariantsCoreJudgmentOutcomeDamageEdgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactJudgmentOutcomeSubtree(long judgmentId);
    bool HasExactBeforeDamageJudgmentSubtree(long windowId);
}

internal interface ICoreNativeCoreInvariantsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHappyWeiYanRetainedGainSettlements(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeCoreInvariantsCoreShanheNullifiedClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetShanheNullifiedClaimRide(global::CardGame.Core.NullificationWindowFrame window, out int[] childProcessingCosts);
}

internal interface ICoreNativeCoreInvariantsCoreProgramShanheSourceWarElephantOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateShanheWarElephantCounterParentProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramDodgeResponseContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeCoreInvariantsCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativeSlashTargetDuelZeroUse(long useId);
    bool IsNativeSlashTargetDuelAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeCoreInvariantsCoreNativeStoneAxeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetStoneAxeDiscardCards(global::CardGame.Core.CharacterState source);
}

internal interface ICoreNativeCoreInvariantsCoreKenshangPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterSharedGameEngineOlMoRumoProofsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOlMoRumoStates();
}

internal interface ICoreNativeCoreInvariantsCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramVirtualOrdinaryTricks();
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
    bool IsProgramVirtualAreaAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterYuanYinAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryYuanYinPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? DiouNativeAncestorProcessing(long useId);
    bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
    bool IsDiouAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterLvYiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[]? ShiLySongyanAncestorProcessing(long id);
}

internal interface ICoreNativeCoreInvariantsCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLvKuangLvXiangCardObserver(long originalUseId);
    bool IsQigongCompletedAttackProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSequentialTrickTargets();
}

internal interface ICoreNativeCoreInvariantsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool IsQinLangXianyingBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiLsPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWuLuKangWindows();
    bool IsWuLuKangPairSubtree(global::CardGame.Core.PindianFrame pair);
}

internal interface ICoreNativeCoreInvariantsCoreHulaoCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHulaoCardState();
    bool HasHulaoAllianceFeastPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNativeCoreInvariantsCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDamageTargetDuels();
    bool IsDamageTargetDuelUse(long id);
    global::System.Collections.Generic.IReadOnlyList<int> DamageTargetDuelOuterProcessing(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreSharedSlashOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSharedSlashUseReturns();
}

internal interface ICoreNativeCoreInvariantsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalStoneAxe { get; }

    bool UsesFormalQinglongCrescentBlade { get; }

    bool UsesFormalIceSword { get; }

    bool UsesFormalQilinBow { get; }

    bool UsesFormalCixiongDoubleSwords { get; }

    global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
    global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame);
    global::CardGame.Core.Card? GetDelayedJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
    bool UsesFormalZhuqueFan { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterFuRongFuQianAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryFuRongFuQianPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreProgramStateCardContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCharacterStateCardUseContinuation();
}

internal interface ICoreNativeCoreInvariantsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardsMovedProgramBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeathFrame GetCurrentDeathFrame(long expectedId);
    void AssertKillDiedProgramInvariant();
    void AssertOwnerDiedProgramInvariant();
}

internal interface ICoreNativeCoreInvariantsCoreLostHpOwnedGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertLostHpOwnedGifts();
}

internal interface ICoreNativeCoreInvariantsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeCaiYongFeibaiRescueContinuation(long dyingId);
    bool HasLeCaiYongFeibaiUseContinuation(long nativeId);
}

internal interface ICoreNativeCoreInvariantsCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundEndingBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCoreFactionRequestPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFactionRequestCostPrompt(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending);
}

internal interface ICoreNativeCoreInvariantsCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? ShenDaCuixinAncestorProcessing(long id);
    bool HasShenDaSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
    bool IsShenDaCuixinAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCorePaidOwnTargetObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualUseTargetObserver(long useId);
    void AssertActualUseTargetPrograms();
}

internal interface ICoreNativeCoreInvariantsCorePaidHpLossProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPaidHpLossModifiers();
}

internal interface ICoreNativeCoreInvariantsCoreEquipmentDonationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertEquipmentDonationPrograms();
}

internal interface ICoreNativeCoreInvariantsCoreDynamicDiscardDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDynamicDiscardDamageAttackObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeCoreInvariantsCoreNativeSkillInvocationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeFactionSkillInvocation(long attackId);
    bool HasFactionDefenseSkillInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense);
    bool HasFactionSlashSkillInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request);
}

internal interface ICoreNativeCoreInvariantsCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeiMiHuTurnPreludeBoundary();
}

internal interface ICoreNativeCoreInvariantsCharacterZhangHengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryZhangHengPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreConditionalDiscardDuelReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertConditionalDiscardDuels();
    bool IsConditionalDiscardDuelUse(long id);
}

internal interface ICoreNativeCoreInvariantsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWenheCardState();
    bool HasWenheCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool IsWenheChaosAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCorePlacedEquipmentBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPlacedEquipmentBenefits();
}

internal interface ICoreNativeCoreInvariantsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWeijieRequests();
    global::CardGame.Core.PendingDecision? WeijieInvariantDecision(global::CardGame.Core.PendingDecision? ordinary);
    global::CardGame.Core.ResolutionFrame? WeijieInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary);
    bool HasWeijieQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasWeijieNativeRequestContinuation(long owner);
    bool IsWeijieProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertExtraCardEffectReceipts();
}

internal interface ICoreNativeCoreInvariantsCoreJudgmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramJudgmentWindowState();
    bool IsProgramJudgmentPromptValid(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame);
    bool IsAiProgramJudgmentPending();
}

internal interface ICoreNativeCoreInvariantsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXuHuangDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AssertCardDeclarationInvariant();
}

internal interface ICoreNativeCoreInvariantsCoreNativeMaterialSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactNativeMaterialSlashExpansion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuZglRescueContinuation(long dyingId);
    bool HasWuZglUseContinuation(long nativeId);
}

internal interface ICoreNativeCoreInvariantsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsMouLuSuPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreNativeCoreInvariantsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertGuanduCardState();
    bool HasGuanduCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get; set; }

    global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get; set; }

    global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get; set; }

    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get; set; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }
}

internal interface ICoreNativeCoreInvariantsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMouBingcaiExtraEffectRounds();
    global::System.Collections.Generic.IReadOnlyList<int>? MouLixianNativeAttackAncestorProcessing(long useId);
    bool IsMouLixianRemovedMaterial(long useId, int cardId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateMouLixianDamageChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCorePrivateOfferObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPrivateOfferPrograms();
}

internal interface ICoreNativeCoreInvariantsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool HasMouSmPrivateContinuation(long useId);
    bool IsMouSmPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreNativeCoreInvariantsCoreSoleTargetProtectionObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetQuarantinedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterLiuBeiSpOl664Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOlSpZhujiuDyingRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNativeCoreInvariantsCoreSignedDamagePaymentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSignedDamagePaymentAttackObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeCoreInvariantsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHappyQiaoDamageHealth();
    void AssertHappyQiaoCardMaximumHealth();
    void AssertHappyQiaoHealthFrames();
    bool HasHappyQiaoHealthPrompt();
    bool HasHappyQiaoDamageHealthContinuation(long attackId);
    bool HasHappyQiaoCardMaximumHealthContinuation(long attackId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateHappyQiaoDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateHappyQiaoCardMaximumHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCorePindianRevealWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPindianRevealWindows();
}

internal interface ICoreNativeCoreInvariantsCorePhaseHandSeizureReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPhaseHandSeizurePrograms();
}

internal interface ICoreNativeCoreInvariantsCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenZfJudgmentGainDisposition(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeCoreInvariantsCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterLuShiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOlLuShiLeijieAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreDualColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDualColorDuels();
    bool IsDualColorDuelUse(long id);
}

internal interface ICoreNativeCoreInvariantsCharacterHuangQuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiQuanDuelUse(long id);
}

internal interface ICoreNativeCoreInvariantsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardAttackSuspendedByProgramDamage { get; }

    void AssertProgramAttackState();
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }

    bool HasDamageTriggerForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeCoreInvariantsCoreWenheChaosOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWenheChaosPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactNextActualUseMaterialSlash(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetCompletedNullificationDyingCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame? window, out int[] costs);
}

internal interface ICoreNativeCoreInvariantsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidZeroMaterialAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool IsTieredRoundZeroBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreGrantedEntityDistanceInvariantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertGrantedEntityDistancePolicies();
}

internal interface ICoreNativeCoreInvariantsCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRequestedDeckBasicMaterials();
    global::CardGame.Core.PendingDecision? RequestedDeckBasicInvariantDecision();
    global::CardGame.Core.ResolutionFrame? RequestedDeckBasicInvariantTop();
}

internal interface ICoreNativeCoreInvariantsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeCoreInvariantsCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasForeignActualTurnStartBoundary();
}

internal interface ICoreNativeCoreInvariantsCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWuXiongmuDamageStates();
}

internal interface ICoreNativeCoreInvariantsCoreOwnTrickDrawTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOwnTrickDrawOfferProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreShanheLeizhenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShanheLeizhenUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeCoreInvariantsCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertColorFireAttackReceipts();
}

internal interface ICoreNativeCoreInvariantsCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSelectedForeignCardSlashes();
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCoreOwnedDeathBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOwnedDeathBenefitReturns();
}

internal interface ICoreNativeCoreInvariantsCharacterHaoPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhenyingDuelUse(long id);
    bool IsZhenyingDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryYangBiaoPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHellSoulCardState();
    bool HasHellSoulPrompt();
    bool IsHellDestroyedSoulMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsActiveCardMovementBatchIdsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Stack<long> _activeCardMovementBatchIds { get; }
}

internal interface ICoreNativeCoreInvariantsCoreRecipientConsequencesObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecipientConsequencesSubtrees();
}

internal interface ICoreNativeCoreInvariantsCoreNoDamageSkillDisableOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNoDamageSkillDisablePolicy(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeCoreInvariantsCoreForeignPublicPileSlashesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignPublicPileSlashUse(long resolutionId);
}

internal interface ICoreNativeCoreInvariantsCharacterBaoSanNiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertChosenAssaults();
    bool IsChosenAssaultAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreDrawPhaseObligationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDrawPhaseObligationBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterXiahouHuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryXiahouHuiPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterDiaoChanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyDiaoChanPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramSkillState();
}

internal interface ICoreNativeCoreInvariantsCharacterLuMengMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AssertPreEntryDiscardPolicyChoice(global::CardGame.Core.PendingDecision pending);
    bool HasNativeDiscardEntryBoundary();
}

internal interface ICoreNativeCoreInvariantsCharacterQiaoZhouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQiaoZhouPreparationBoundaryFrame();
}

internal interface ICoreNativeCoreInvariantsCoreWenyiDamageHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWenyiDamageHealth();
    bool HasWenyiDamageHealthContinuation(long attackFrameId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateWenyiDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertHuanpeiRequests();
    global::CardGame.Core.PendingDecision? HuanpeiInvariantDecision(global::CardGame.Core.PendingDecision? ordinary);
    global::CardGame.Core.ResolutionFrame? HuanpeiInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary);
    bool HasHuanpeiBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
    bool HasHuanpeiQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasHuanpeiNativeRequestContinuation(long parentId);
    bool IsHuanpeiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDouyuWindows();
    bool HasBiguoPindianContinuation();
    bool IsDouyuPairSubtree(global::CardGame.Core.PindianFrame pair);
}

internal interface ICoreNativeCoreInvariantsCharacterZhangYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiZySuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWumiaoFlood(global::CardGame.Core.CardUseFrame use);
    bool HasWumiaoFloodDiscardPrompt();
    bool HasWumiaoFloodProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWenhePoisonFireAttackPrompt();
    bool IsCurrentWenhePoisonDying();
    bool HasExactWenhePoisonResponseContinuation();
}

internal interface ICoreNativeCoreInvariantsCharacterHanSongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHanSongPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterBaiLingYunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryBaiLingYunPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterDuKuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDuKuiPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileSimaRenjieNativeSubtree(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreDamageArmorPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDamageArmorPayments();
}

internal interface ICoreNativeCoreInvariantsCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShownEntityTurnPrograms();
}

internal interface ICoreNativeCoreInvariantsCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasXinPiCardObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreDelayedPlacementTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreNativeCoreInvariantsCoreShanheS5YimieObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheS5YimieRecoveryContinuation(long attackId);
}

internal interface ICoreNativeCoreInvariantsCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSkillInvocations();
    bool HasExactBoundarySkillInvocation();
    bool IsExactSkillInvocationDying(global::CardGame.Core.DyingFrame? dying);
}

internal interface ICoreNativeCoreInvariantsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreNativeCoreInvariantsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramCardWindowState();
    global::CardGame.Core.NativeOwners.CardAttackHandle? ProgramCardAttack { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterAHuiNanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAHuiNanJuemanAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterHuCheErAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHuCheErPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterZhangWenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryZhangWenPrompt();
}

internal interface ICoreNativeCoreInvariantsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeCoreInvariantsCharacterHeQiMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileHeQiPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCuiLiePrompt();
}

internal interface ICoreNativeCoreInvariantsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreNativeCoreInvariantsCoreProgramViewContributorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramViewContributors ViewContributors { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterChengYuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? MouDanchiAncestorProcessing(long id);
    bool HasMouDanchiSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsMouDanchiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertTianChouNativeUses();
    bool IsTianChouShandaoAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSelectedActorDuels();
    bool IsSelectedActorDuelUse(long id);
}

internal interface ICoreNativeCoreInvariantsCoreProgramVirtualDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNativeCoreInvariantsCharacterFengXiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryFengXiPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreShanheS5YimieOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShanheS5YimieRecoveryFrames();
    bool HasShanheS5YimieRecovery(long attackId);
}

internal interface ICoreNativeCoreInvariantsCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouWenChouDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterYanXiangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryYanXiangPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeCoreInvariantsCharacterMaMiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryMaMiaoPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreNativeCoreInvariantsCharacterXuJingAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryXuJingPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterWuFuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryWuFuPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieCompletedUseObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCharacterHeJinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHeJinMouzhuDuelUse(long id);
    bool IsHeJinMouzhuAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] CurrentPindianProcessingIds(global::CardGame.Core.PindianFrame frame);
}

internal interface ICoreNativeCoreInvariantsCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId);
    bool TryValidateProgramAlternativeCostAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing, out bool valid);
}

internal interface ICoreNativeCoreInvariantsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AssertDyingSuitsSubtree();
    bool HasDyingSuitsSuspendedAttack(long attackId);
}

internal interface ICoreNativeCoreInvariantsCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhangFenPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.SkillProgram Program, global::CardGame.Core.SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(global::CardGame.Core.JudgmentTriggerCandidate candidate);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false);
    bool IsAiProgramJudgmentReplacementPending();
}

internal interface ICoreNativeCoreInvariantsCoreNativeQilinBowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetQilinBowTargetMounts(global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeCoreInvariantsCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetQinglongCrescentBladeSlashCards(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QinglongPhysicalSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> slashes);
    bool CanRequestQinglongCrescentBladeFactionSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
}

internal interface ICoreNativeCoreInvariantsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuZhangFeiUseContinuation(long nativeUseId);
}

internal interface ICoreNativeCoreInvariantsCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyQiaoNativeDamageChoice(long attackId);
}

internal interface ICoreNativeCoreInvariantsCoreDrawFundedDistinctBasicObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertDrawFundedDistinctBasicFramesAndUses();
    global::CardGame.Core.PendingDecision? DrawFundedDistinctBasicInvariantDecision(global::CardGame.Core.PendingDecision? nativeDecision);
    global::CardGame.Core.ResolutionFrame? DrawFundedDistinctBasicInvariantTop(global::CardGame.Core.ResolutionFrame? nativeTop);
    global::CardGame.Core.DrawFundedDistinctBasicFrame? DrawFundedDistinctBasicObserverRoot(long? requestedId = default);
    bool HasDrawFundedDistinctBasicObserver(long cardUseId);
}

internal interface ICoreNativeCoreInvariantsCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyZuoCiPrompt();
}

internal interface ICoreNativeCoreInvariantsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramLifecycleBoundaryFrame();
    bool HasTurnEndingBoundaryFrame();
    bool HasPlayPhaseStartingBoundaryFrame();
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
    bool HasPendingProgramBoundCards { get; }

    bool IsProgramProcessingConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertQizheng(global::CardGame.Core.CardUseFrame use);
    bool HasQizhengPrompt();
    bool HasQizhengProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool HasQizhengResponseContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool IsVirtualQizhengAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecoveryReplacementInvariants();
    bool HasRecoveryPaidCardUseObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreNativeCoreInvariantsCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiGyPrompt();
    bool HasShiGyCompletedUseObserver(long id);
}

internal interface ICoreNativeCoreInvariantsCoreNonBasicSlashProfileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNonBasicSlashPolicies();
}

internal interface ICoreNativeCoreInvariantsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
    void AssertCurrentUsePhysicalClaims();
}

internal interface ICoreNativeCoreInvariantsCharacterZhangXunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryZhangXunPrompt();
}

internal interface ICoreNativeCoreInvariantsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcZeroAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreNativeCoreInvariantsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreNativeCoreInvariantsCoreMultiplayerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHumanControlledSeat(int seat);
}

internal interface ICoreNativeCoreInvariantsCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJueshengDuelUse(long id);
    bool IsJueshengDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreEquipmentPairDyingCardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactDyingOwnedCardRide(long dyingId);
}

internal interface ICoreNativeCoreInvariantsCoreExtraEffectForeignSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AssertExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PendingDecision? decision, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreNativeCoreInvariantsCharacterLiuYongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryLiuYongPrompt();
    bool IsAnniversaryLiuYongSlashConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreKuangfuReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertKuangfuPrograms();
}

internal interface ICoreNativeCoreInvariantsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong);
}

internal interface ICoreNativeCoreInvariantsCharacterHaoMengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHaoMengPrompt();
    bool IsHaoMengMaterialSlashAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMaximumHpDamageReplacementReceipts();
}

internal interface ICoreNativeCoreInvariantsCoreFixedDistanceOneDebtOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertFixedDistanceOnePolicies();
}

internal interface ICoreNativeCoreInvariantsCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFirstSlashDuelLegacyUseObserver(long id);
    void AssertFirstSlashDuelLegacyWindows();
}

internal interface ICoreNativeCoreInvariantsCoreOtherDyingRecoveryRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactOtherDyingRecoveryRide(long dyingId);
}

internal interface ICoreNativeCoreInvariantsCoreCardActionContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardActionPindianContinuation();
}

internal interface ICoreNativeCoreInvariantsCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? BoundaryLiuFengInvariantDecision(global::CardGame.Core.PendingDecision? ordinary);
    global::CardGame.Core.ResolutionFrame? BoundaryLiuFengInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary);
    void AssertBoundaryLiuFengRequests();
    bool HasBoundaryLiuFengPrompt();
    bool HasBoundaryLiuFengBorrowedSettlementRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle b);
    bool HasBoundaryLiuFengQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    bool HasBoundaryLiuFengNativeRequestContinuation(long ownerFrameId);
    bool IsBoundaryLiuFengNativeAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCharacterZhouCangBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryZhouCangCompletedUseObserver(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertQinglongInsertions();
    global::System.Collections.Generic.HashSet<int> QinglongAncestorProcessing(long attackId);
}

internal interface ICoreNativeCoreInvariantsCoreShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheMarketBoundary();
}

internal interface ICoreNativeCoreInvariantsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZeroEntityProcessingParent(long id, int cardId);
    bool IsZeroEntityUse(long id);
    bool IsMouZhijueZeroAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
    bool IsMouZhijueBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertChainedStateBasicFramesAndUses();
    global::CardGame.Core.PendingDecision? ChainedStateBasicInvariantDecision(global::CardGame.Core.PendingDecision? native);
    global::CardGame.Core.ResolutionFrame? ChainedStateBasicInvariantTop(global::CardGame.Core.ResolutionFrame? native);
    bool HasChainedStateBasicObserver(long id);
    bool IsChainedStateBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing);
}

internal interface ICoreNativeCoreInvariantsCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertJudgedRankSlashPolicies();
}

internal interface ICoreNativeCoreInvariantsCoreJoinedTrickDamageRewardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertJoinedTrickDamageBenefits(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCoreInvariantsCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentTurnOwnSkillSuppressions();
}

internal interface ICoreNativeCoreInvariantsCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuaManCardContinuation(long useId);
}

internal interface ICoreNativeCoreInvariantsCoreNativeDrawAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNativeDrawAttempts();
}

internal interface ICoreNativeCoreInvariantsCharacterXingDaoRongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryXingDaoRongPrompt();
}
