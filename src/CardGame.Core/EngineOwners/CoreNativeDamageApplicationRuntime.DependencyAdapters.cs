#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreNativeDamageApplicationCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().TryPauseRecoveryPaidCardUse(useId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat) => this._targetFactory().HasDirectedCardArmorBypass(resolutionId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreDamagePreventionProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreDamagePreventionProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginBeforeDamageProgramWindowForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => this._targetFactory().TryBeginBeforeDamageProgramWindowForAttack(attack, amount, nature);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeIceSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeIceSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> _targetFactory = targetFactory;
    public bool TryBeginIceSwordChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => this._targetFactory().TryBeginIceSwordChoice(attack, preventedDamageAmount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreHealthReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreHealthReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackAsHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackAsHpLoss(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public int FinalizeAttackDamageAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().FinalizeAttackDamageAmount(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterXinPiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterXinPiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _targetFactory = targetFactory;
    public bool IsXinPiRemainingCardDamageIneffective(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().IsXinPiRemainingCardDamageIneffective(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeDamageFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeDamageFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageFramesRuntime> _targetFactory = targetFactory;
    public long BeginDamage(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.DamageNature nature = (global::CardGame.Core.DamageNature)0) => this._targetFactory().BeginDamage(parentFrameId, sourceSeat, targetSeat, amount, nature);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public void ApplyHengyeGrowth(long damageFrameId, global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int amount) => this._targetFactory().ApplyHengyeGrowth(damageFrameId, source, target, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeDamageApplicationCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime))]
internal sealed class CoreNativeDamageApplicationCoreOtherParticipantHealthReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreOtherParticipantHealthReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> _targetFactory = targetFactory;
    public bool TryGetOtherParticipantHpReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, out global::CardGame.Core.CardUseEffectSource source) => this._targetFactory().TryGetOtherParticipantHpReplacement(attack, out source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreNativeDamageApplicationCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterCaoAngMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterCaoAngMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _targetFactory = targetFactory;
    public void PrepareMouCaDamageSource(global::CardGame.Core.GameEngine.IDamageAttempt attempt) => this._targetFactory().PrepareMouCaDamageSource(attempt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().TryBeginHappyQiaoAttackHpLossChoice(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreEquipmentSlotGroupProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreEquipmentSlotGroupProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _targetFactory = targetFactory;
    public bool ResolveEnteringChain(global::CardGame.Core.CharacterState player, bool requested) => this._targetFactory().ResolveEnteringChain(player, requested);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterMangYaChangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterMangYaChangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryBeginAnniversaryJiedaoSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().TryBeginAnniversaryJiedaoSourceWindow(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreRevealRecastProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreRevealRecastProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCompletedCardDamageParticipant(long cardUseFrameId, int targetSeat) => this._targetFactory().RecordCompletedCardDamageParticipant(cardUseFrameId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState killer) => this._targetFactory().BeginDying(attack, damageFrameId, victim, killer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeDamageApplicationCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreProgramPersistentCombatPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreProgramPersistentCombatPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> _targetFactory = targetFactory;
    public void AccumulateProgramPlayDamageHandLimit(int sourceSeat, int amount) => this._targetFactory().AccumulateProgramPlayDamageHandLimit(sourceSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreDamageArmorPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreDamageArmorPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageArmorPaymentReceipt? PrepareDamageArmorPayment(long frameId, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().PrepareDamageArmorPayment(frameId, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreNativeDamageApplicationCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CalculateCardDamageLayer(attack);
    public bool TryReplaceCardDamageWithMaximumHpLoss(global::CardGame.Core.GameEngine.IDamageAttempt attack, out bool pending) => this._targetFactory().TryReplaceCardDamageWithMaximumHpLoss(attack, out pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime))]
internal sealed class CoreNativeDamageApplicationCoreRecipientScopedDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreRecipientScopedDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> _targetFactory = targetFactory;
    public void CaptureRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().CaptureRecipientScopedDamageBase(attack, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime))]
internal sealed class CoreNativeDamageApplicationCoreColdDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreColdDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> _targetFactory = targetFactory;
    public bool TryBeginColdDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => this._targetFactory().TryBeginColdDamageReplacement(attack, amount, nature);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyQiaoDamageChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().TryBeginHappyQiaoDamageChoice(attack);
    public bool HasHappyQiaoHealth(int seat) => this._targetFactory().HasHappyQiaoHealth(seat);
    public void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after) => this._targetFactory().CompleteHappyQiaoHealthPayment(parentFrameId, targetSeat, kind, before, after);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterXuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterXuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> _targetFactory = targetFactory;
    public void CaptureXuRongActualDamageHp(long damageFrameId, global::CardGame.Core.GameEngine.IDamageAttempt attack, int hpBefore, int actualAmount) => this._targetFactory().CaptureXuRongActualDamageHp(damageFrameId, attack, hpBefore, actualAmount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginMobileSimaRenjieFailedAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryBeginMobileSimaRenjieFailedAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag) => this._targetFactory().HasCurrentCardEnhancement(frameId, flag);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeDamageWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeDamageWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().TryBeginDamageTriggerWindow(attack, damageFrameId, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime))]
internal sealed class CoreNativeDamageApplicationCoreProgramDamageHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreProgramDamageHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreNativeDamageApplicationCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => this._targetFactory().TryBeginIdentityAppearanceForDamage(attack, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime))]
internal sealed class CoreNativeDamageApplicationCoreCompletedUndamagedTargetRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreCompletedUndamagedTargetRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _targetFactory = targetFactory;
    public void RecordCompletedUndamagedTargetDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, int amount) => this._targetFactory().RecordCompletedUndamagedTargetDamage(attack, damageFrameId, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterCaiYangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterCaiYangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryBeginAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt a) => this._targetFactory().TryBeginAnniversaryCaiYangJiaofengSourceWindow(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeQilinBowAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCoreNativeQilinBowOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _targetFactory = targetFactory;
    public bool TryBeginQilinBowChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryBeginQilinBowChoice(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreNativeDamageApplicationCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageApplicationCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public void RecordKenshangDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageId, int amount) => this._targetFactory().RecordKenshangDamage(attack, damageId, amount);
}
