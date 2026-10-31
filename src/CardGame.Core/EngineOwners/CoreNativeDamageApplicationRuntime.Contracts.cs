#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDamageApplicationCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreNativeDamageApplicationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeDamageApplicationCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat);
}

internal interface ICoreNativeDamageApplicationCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeDamageApplicationCoreDamagePreventionProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBeforeDamageProgramWindowForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature);
}

internal interface ICoreNativeDamageApplicationCoreNativeIceSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIceSwordChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount);
}

internal interface ICoreNativeDamageApplicationCoreHealthReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackAsHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FinalizeAttackDamageAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinPiRemainingCardDamageIneffective(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreNativeDamageApplicationCoreNativeDamageFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginDamage(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.DamageNature nature = (global::CardGame.Core.DamageNature)0);
}

internal interface ICoreNativeDamageApplicationCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyHengyeGrowth(long damageFrameId, global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int amount);
}

internal interface ICoreNativeDamageApplicationCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreNativeDamageApplicationCoreOtherParticipantHealthReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetOtherParticipantHpReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, out global::CardGame.Core.CardUseEffectSource source);
}

internal interface ICoreNativeDamageApplicationCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreNativeDamageApplicationCharacterCaoAngMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareMouCaDamageSource(global::CardGame.Core.GameEngine.IDamageAttempt attempt);
}

internal interface ICoreNativeDamageApplicationCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResolveEnteringChain(global::CardGame.Core.CharacterState player, bool requested);
}

internal interface ICoreNativeDamageApplicationCharacterMangYaChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAnniversaryJiedaoSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeDamageApplicationCoreRevealRecastProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCompletedCardDamageParticipant(long cardUseFrameId, int targetSeat);
}

internal interface ICoreNativeDamageApplicationCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState killer);
}

internal interface ICoreNativeDamageApplicationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeDamageApplicationCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeDamageApplicationCoreProgramPersistentCombatPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AccumulateProgramPlayDamageHandLimit(int sourceSeat, int amount);
}

internal interface ICoreNativeDamageApplicationCoreDamageArmorPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageArmorPaymentReceipt? PrepareDamageArmorPayment(long frameId, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageApplicationCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    bool TryReplaceCardDamageWithMaximumHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack, out bool pending);
}

internal interface ICoreNativeDamageApplicationCoreRecipientScopedDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreNativeDamageApplicationCoreColdDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginColdDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature);
}

internal interface ICoreNativeDamageApplicationCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyQiaoDamageChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    bool HasHappyQiaoHealth(int seat);
    void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after);
}

internal interface ICoreNativeDamageApplicationCharacterXuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureXuRongActualDamageHp(long damageFrameId, global::CardGame.Core.GameEngine.IDamageAttempt attack, int hpBefore, int actualAmount);
}

internal interface ICoreNativeDamageApplicationCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileSimaRenjieFailedAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeDamageApplicationSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreNativeDamageApplicationCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag);
}

internal interface ICoreNativeDamageApplicationCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreNativeDamageApplicationCoreProgramDamageHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreNativeDamageApplicationCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount);
}

internal interface ICoreNativeDamageApplicationCoreCompletedUndamagedTargetRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCompletedUndamagedTargetDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, int amount);
}

internal interface ICoreNativeDamageApplicationCharacterCaiYangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt a);
}

internal interface ICoreNativeDamageApplicationCoreNativeQilinBowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQilinBowChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeDamageApplicationCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordKenshangDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageId, int amount);
}
