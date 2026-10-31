#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public void AssertZonghuRequests() => this._targetFactory().AssertZonghuRequests();
    public global::CardGame.Core.PendingDecision? ZonghuInvariantDecision(global::CardGame.Core.PendingDecision? ordinary) => this._targetFactory().ZonghuInvariantDecision(ordinary);
    public global::CardGame.Core.ResolutionFrame? ZonghuInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary) => this._targetFactory().ZonghuInvariantTop(ordinary);
    public bool HasZonghuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasZonghuQinglongPaymentRide(attack);
    public bool HasZonghuNativeRequestContinuation(long owner) => this._targetFactory().HasZonghuNativeRequestContinuation(owner);
    public bool IsZonghuProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsZonghuProgramRequestAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMuShunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMuShunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMuShunAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryMuShunPrompt() => this._targetFactory().HasAnniversaryMuShunPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDelegatedJudgmentChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDelegatedJudgmentChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _targetFactory = targetFactory;
    public bool IsDelegatedJudgment(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsDelegatedJudgment(trigger);
    public bool IsDelegatedJudgmentPromptValid(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.PendingDecision? p) => this._targetFactory().IsDelegatedJudgmentPromptValid(j, p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGaoXiangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGaoXiangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryGaoXiangPrompt() => this._targetFactory().HasAnniversaryGaoXiangPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public bool HasNativeDelayedAttackChild(long judgmentId) => this._targetFactory().HasNativeDelayedAttackChild(judgmentId);
    public bool IsOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().IsOriginalUseLightningJudgment(j);
    public bool IsOriginalUseLightningAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsOriginalUseLightningAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuZhaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuZhaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHuZhaoPrompt() => this._targetFactory().HasAnniversaryHuZhaoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWangChangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWangChangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryWangChangPrompt() => this._targetFactory().HasAnniversaryWangChangPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public void AssertMantianGuohaiState() => this._targetFactory().AssertMantianGuohaiState();
    public bool HasMantianGuohaiPrompt() => this._targetFactory().HasMantianGuohaiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => this._targetFactory().FieldEquipmentSlot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFinalizedJudgmentDamageChildrenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFinalizedJudgmentDamageChildrenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> _targetFactory = targetFactory;
    public void AssertFinalizedJudgmentDamageChildren() => this._targetFactory().AssertFinalizedJudgmentDamageChildren();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryZhangHuanPrompt() => this._targetFactory().HasAnniversaryZhangHuanPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCompletedGiftQinglongRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCompletedGiftQinglongRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> _targetFactory = targetFactory;
    public bool TryGetCompletedGiftRecipientSlashRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window, out global::CardGame.Core.CardUseFrame child) => this._targetFactory().TryGetCompletedGiftRecipientSlashRide(window, out child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreIdentityAppearanceInvariantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreIdentityAppearanceInvariantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityAppearanceInvariantsRuntime> _targetFactory = targetFactory;
    public bool HasIdentityAppearanceAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasIdentityAppearanceAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public bool IsAiResponsePending() => this._targetFactory().IsAiResponsePending();
    public bool IsAiNullificationPending() => this._targetFactory().IsAiNullificationPending();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public void AssertRoundGainedUseQualifications(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertRoundGainedUseQualifications(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRecipientScopedDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRecipientScopedDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> _targetFactory = targetFactory;
    public void AssertRecipientScopedDamageStates() => this._targetFactory().AssertRecipientScopedDamageStates();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMangYaChangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMangYaChangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryJiedaoPrompt() => this._targetFactory().HasAnniversaryJiedaoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasMobileZhitiDuelContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasMobileZhitiDuelContinuation(attack);
    public bool HasMobileZhitiContestPrompt() => this._targetFactory().HasMobileZhitiContestPrompt();
    public bool IsMobileZhitiContestSubtree(global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().IsMobileZhitiContestSubtree(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> _targetFactory = targetFactory;
    public void AssertDiscardPromptInvariant() => this._targetFactory().AssertDiscardPromptInvariant();
    public bool HasManualDiscardBoundaryFrame() => this._targetFactory().HasManualDiscardBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWangYiMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWangYiMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> _targetFactory = targetFactory;
    public bool IsDeckTopJudgmentReplacementPromptValid(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.PendingDecision? decision) => this._targetFactory().IsDeckTopJudgmentReplacementPromptValid(judgment, decision);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? MouLiuChengNativeAncestorProcessing(long useId) => this._targetFactory().MouLiuChengNativeAncestorProcessing(useId);
    public bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsMouLiuChengSuspendedCompletedAttack(original, requiredWindowId);
    public bool IsMouLiuChengAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouLiuChengAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSunHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSunHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversarySunHuanPrompt() => this._targetFactory().HasAnniversarySunHuanPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheS5XuncanAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheS5XuncanOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> _targetFactory = targetFactory;
    public bool IsShanheS5YunshenUse(long useId) => this._targetFactory().IsShanheS5YunshenUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeIceSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeIceSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> _targetFactory = targetFactory;
    public void AssertIceSwordLogicalDiscard(global::CardGame.Core.NativeOwners.IceSwordHandle pending) => this._targetFactory().AssertIceSwordLogicalDiscard(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool TryGetResponseExchangeMovementDyingCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame? window, out int[] costs) => this._targetFactory().TryGetResponseExchangeMovementDyingCosts(window, out costs);
    public bool IsExchangedCardClaim(long actionId, int cardId) => this._targetFactory().IsExchangedCardClaim(actionId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuBanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuBanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _targetFactory = targetFactory;
    public bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsHuiyunOutsideGeneralWeaponMaterial(useId, cardId);
    public bool HasHuiyunCompletedUseObserver(long useId) => this._targetFactory().HasHuiyunCompletedUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXunXuAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXunXuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryXunXuPrompt() => this._targetFactory().HasAnniversaryXunXuPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEquipmentRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEquipmentRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _targetFactory = targetFactory;
    public bool HasEquipmentRecastNativeDrawBoundary() => this._targetFactory().HasEquipmentRecastNativeDrawBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? KenshangNativeOriginalDecision(long ownerId) => this._targetFactory().KenshangNativeOriginalDecision(ownerId);
    public void AssertKenshangRequests() => this._targetFactory().AssertKenshangRequests();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => this._targetFactory().KenshangQinglongSlashChoices(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWuAnGuoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWuAnGuoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> _targetFactory = targetFactory;
    public bool IsWuAnGuoLiyongDuelUse(long id) => this._targetFactory().IsWuAnGuoLiyongDuelUse(id);
    public bool IsWuAnGuoLiyongAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsWuAnGuoLiyongAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardEffectProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardEffectProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _targetFactory = targetFactory;
    public void AssertCardEffectProgramState() => this._targetFactory().AssertCardEffectProgramState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWoLongFengChuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWoLongFengChuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _targetFactory = targetFactory;
    public bool HasWoLongFengChuBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowed) => this._targetFactory().HasWoLongFengChuBorrowedPaymentRide(borrowed);
    public bool HasWoLongFengChuUseObserver(long id) => this._targetFactory().HasWoLongFengChuUseObserver(id);
    public bool HasWoLongFengChuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasWoLongFengChuQinglongPaymentRide(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePaidColorDamageClaimObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePaidColorDamageClaimObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> _targetFactory = targetFactory;
    public void AssertPaidColorDamageClaims() => this._targetFactory().AssertPaidColorDamageClaims();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public bool HasDeferredTurnEndBoundaryFrame() => this._targetFactory().HasDeferredTurnEndBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGuanHaiBridgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGuanHaiBridgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _targetFactory = targetFactory;
    public bool IsXieweiMultiTargetAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsXieweiMultiTargetAttack(pending, attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGroupTargetNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGroupTargetNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> _targetFactory = targetFactory;
    public bool IsGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().IsGroupTargetNullification(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreActualPhaseEndsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreActualPhaseEndsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _targetFactory = targetFactory;
    public bool HasActualPhaseEndBoundaryFrame() => this._targetFactory().HasActualPhaseEndBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuLiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuLiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryLiuLiPrompt() => this._targetFactory().HasAnniversaryLiuLiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHpLossSlashMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHpLossSlashMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> _targetFactory = targetFactory;
    public void AssertHpLossMaterialSlashes() => this._targetFactory().AssertHpLossMaterialSlashes();
    public bool IsExactHpLossMaterialSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsExactHpLossMaterialSlash(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSourceCurseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSourceCurseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime> _targetFactory = targetFactory;
    public void AssertSourceCurseState() => this._targetFactory().AssertSourceCurseState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDianWeiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDianWeiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryAssertHappyDianWeiDeathRemoval(global::CardGame.Core.DeathFrame death) => this._targetFactory().TryAssertHappyDianWeiDeathRemoval(death);
    public bool HasHappyDianWeiGuardPrompt() => this._targetFactory().HasHappyDianWeiGuardPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePindianAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> _targetFactory = targetFactory;
    public void AssertPindianInvariant() => this._targetFactory().AssertPindianInvariant();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public void AssertYingboInvariant() => this._targetFactory().AssertYingboInvariant();
    public global::CardGame.Core.NativeOwners.YingboGiftHandle? ActiveYingboGift { get => this._targetFactory().ActiveYingboGift; set => this._targetFactory().ActiveYingboGift = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreWushuangAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreWushuangOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> _targetFactory = targetFactory;
    public bool HasWushuangIntermissionBoundary() => this._targetFactory().HasWushuangIntermissionBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageObserverCursorQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageObserverCursorQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageCursorProjection ResolveDamageCursor(global::CardGame.Core.DamageTriggerWindowFrame window) => this._targetFactory().ResolveDamageCursor(window);
    public global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => this._targetFactory().DamageCursorEffectiveTop(includeNestedObservers);
    public bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window) => this._targetFactory().DamageProgramMatchesWindow(root, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterPangTongHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterPangTongHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyPangTongPrompt() => this._targetFactory().HasHappyPangTongPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMaTieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMaTieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaTieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryMaTiePrompt() => this._targetFactory().HasAnniversaryMaTiePrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGiftedSlashUseRewardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGiftedSlashUseRewardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> _targetFactory = targetFactory;
    public void AssertGiftedSlashUseBenefit(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertGiftedSlashUseBenefit(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void AssertDragonBoatCardState() => this._targetFactory().AssertDragonBoatCardState();
    public bool HasDragonBoatDiscardPrompt() => this._targetFactory().HasDragonBoatDiscardPrompt();
    public bool HasDragonBoatCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasDragonBoatCardEffectProcessing(processing);
    public bool IsDragonBoatZeroAgainstCurrentAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsDragonBoatZeroAgainstCurrentAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFactionRequestCostRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFactionRequestCostRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _targetFactory = targetFactory;
    public bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request) => this._targetFactory().HasPaidFactionRequestCostRecovery(request);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().HasNullificationEffectOwner(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public void AssertYintianDamageStates() => this._targetFactory().AssertYintianDamageStates();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? TousuiNativeOriginalDecision(long ownerId) => this._targetFactory().TousuiNativeOriginalDecision(ownerId);
    public void AssertTousuiRequests() => this._targetFactory().AssertTousuiRequests();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => this._targetFactory().TousuiQinglongSlashChoices(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSlashTargetPenaltyObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSlashTargetPenaltyObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> _targetFactory = targetFactory;
    public bool HasTargetPenaltyUseObserver(long useId) => this._targetFactory().HasTargetPenaltyUseObserver(useId);
    public void AssertTargetPenaltyWindows() => this._targetFactory().AssertTargetPenaltyWindows();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public void AssertFriendEventFrames() => this._targetFactory().AssertFriendEventFrames();
    public bool HasFriendDuelResponseObserver(long useId) => this._targetFactory().HasFriendDuelResponseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageJudgmentSuitPaymentObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageJudgmentSuitPaymentObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> _targetFactory = targetFactory;
    public bool HasDamageJudgmentSuitPaymentRide(long judgment) => this._targetFactory().HasDamageJudgmentSuitPaymentRide(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGuanduEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGuanduEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => this._targetFactory().IsGuanduDestroyedChariotMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHeYanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHeYanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHeYanPrompt() => this._targetFactory().HasAnniversaryHeYanPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgmentOutcomeClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgmentOutcomeClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> _targetFactory = targetFactory;
    public bool HasExactJudgmentOutcomeClaimDisposition(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().HasExactJudgmentOutcomeClaimDisposition(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool TryGetPolicyCounterspellPaymentRide(global::CardGame.Core.NullificationWindowFrame window, out int[] rescueCosts) => this._targetFactory().TryGetPolicyCounterspellPaymentRide(window, out rescueCosts);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeCaoZhiWenlanRescueContinuation(long dyingId) => this._targetFactory().HasLeCaoZhiWenlanRescueContinuation(dyingId);
    public bool HasLeCaoZhiWenlanUseContinuation(long nativeId) => this._targetFactory().HasLeCaoZhiWenlanUseContinuation(nativeId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGuoZhaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGuoZhaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryGuoZhaoPrompt() => this._targetFactory().HasAnniversaryGuoZhaoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRoundPricedPileAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRoundPricedPileAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _targetFactory = targetFactory;
    public bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsRoundPricedPileAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? MouXqNativeAncestorProcessing(long id) => this._targetFactory().MouXqNativeAncestorProcessing(id);
    public bool HasMouXqJueyanResponseContinuation(long parentId) => this._targetFactory().HasMouXqJueyanResponseContinuation(parentId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long parentId, bool qinglong) => this._targetFactory().MouXqForcedSlashChoices(actor, target, parentId, qinglong);
    public bool IsMouXqJueyanAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouXqJueyanAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGuanduChariotResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGuanduChariotResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default) => this._targetFactory().GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public void AssertShenwuCardState() => this._targetFactory().AssertShenwuCardState();
    public bool HasShenwuTrickPrompt() => this._targetFactory().HasShenwuTrickPrompt();
    public bool IsShenwuTransplantAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsShenwuTransplantAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public void AssertPostEventProgramInvariants() => this._targetFactory().AssertPostEventProgramInvariants();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLeiBoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLeiBoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryLeiBoPrompt() => this._targetFactory().HasAnniversaryLeiBoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterCheZhouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterCheZhouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryCheZhouPrompt() => this._targetFactory().HasAnniversaryCheZhouPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreYaodaoChitongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreYaodaoChitongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> _targetFactory = targetFactory;
    public bool HasExactYaodaoChitongJudgmentSubtree(long judgmentId) => this._targetFactory().HasExactYaodaoChitongJudgmentSubtree(judgmentId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public void AssertMoQuanyuSlashUses() => this._targetFactory().AssertMoQuanyuSlashUses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFriendEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFriendEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _targetFactory = targetFactory;
    public bool IsFriendRecastDuelUse(long id) => this._targetFactory().IsFriendRecastDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public void AssertFinalTargetSlashReceipts() => this._targetFactory().AssertFinalTargetSlashReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHouChengAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHouChengAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHouChengPrompt() => this._targetFactory().HasAnniversaryHouChengPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRecoveryReplacementDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRecoveryReplacementDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsRecoveryReplacementProgramDying(long? requiredPaidUseId = default) => this._targetFactory().IsRecoveryReplacementProgramDying(requiredPaidUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public void AssertXieweiPayments(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertXieweiPayments(use);
    public bool IsXieweiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsXieweiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRoundOnceCardlessReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRoundOnceCardlessReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> _targetFactory = targetFactory;
    public bool IsRoundOnceCardlessProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsRoundOnceCardlessProgramRequestAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public void AssertNativeCardEffectDraw(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertNativeCardEffectDraw(use);
    public bool HasNativeCardEffectDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasNativeCardEffectDrawProcessing(processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhouYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhouYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? MouZyNativeAncestorProcessing(long id) => this._targetFactory().MouZyNativeAncestorProcessing(id);
    public bool IsMouZyZeroFireAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouZyZeroFireAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetNullificationCards(responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool IsWuJingHejiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsWuJingHejiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool IsZhangYanSujiRemovedMaterial(long useId, int cardId) => this._targetFactory().IsZhangYanSujiRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreUnexpectedAssaultAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreUnexpectedAssaultOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _targetFactory = targetFactory;
    public void AssertUnexpectedAssaultReceipts() => this._targetFactory().AssertUnexpectedAssaultReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuXunMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuXunMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasNativeGroupEffectAttackObserver(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasNativeGroupEffectAttackObserver(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void AssertKemingEchoFrames() => this._targetFactory().AssertKemingEchoFrames();
    public bool HasKemingCixiongContinuation(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending) => this._targetFactory().HasKemingCixiongContinuation(pending);
    public bool HasKemingEchoPrompt() => this._targetFactory().HasKemingEchoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRecordedSuitSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRecordedSuitSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> _targetFactory = targetFactory;
    public void AssertRecordedSuitSlashReceipts() => this._targetFactory().AssertRecordedSuitSlashReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuZhiMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuZhiMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouLzDirectUseContinuation(long id) => this._targetFactory().HasMouLzDirectUseContinuation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreAssistedFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreAssistedFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _targetFactory = targetFactory;
    public void ValidateAssistedFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateAssistedFactionSlashParent(pending, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterJiangWeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterJiangWeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsShenJwAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsShenJwAttackConsistent(a, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreActualDiscardRecoveryObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreActualDiscardRecoveryObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> _targetFactory = targetFactory;
    public void AssertActualEquipmentOrDiscardPrograms() => this._targetFactory().AssertActualEquipmentOrDiscardPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeEquipmentInvocationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeEquipmentInvocationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> _targetFactory = targetFactory;
    public void AssertNativeEquipmentInvocations() => this._targetFactory().AssertNativeEquipmentInvocations();
    public global::CardGame.Core.GameEngine.NativeEquipmentSuspensionProjection? FindNativeEquipmentInvocation(long attackId) => this._targetFactory().FindNativeEquipmentInvocation(attackId);
    public bool HasFactionDefenseEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense) => this._targetFactory().HasFactionDefenseEquipmentInvocation(defense);
    public bool HasQinglongFactionEquipmentInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => this._targetFactory().HasQinglongFactionEquipmentInvocation(request);
    public bool HasNativeEquipmentInvocation(long attackId) => this._targetFactory().HasNativeEquipmentInvocation(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreColdDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreColdDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> _targetFactory = targetFactory;
    public void AssertColdDamageReplacementFrames() => this._targetFactory().AssertColdDamageReplacementFrames();
    public bool HasColdDamageReplacement(long attackId) => this._targetFactory().HasColdDamageReplacement(attackId);
    public bool HasColdDamagePrompt() => this._targetFactory().HasColdDamagePrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreOwnedDamagePointJudgmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreOwnedDamagePointJudgmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _targetFactory = targetFactory;
    public bool HasOwnedDamagePointJudgmentRide(long judgmentFrameId) => this._targetFactory().HasOwnedDamagePointJudgmentRide(judgmentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSlashTargetBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSlashTargetBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> _targetFactory = targetFactory;
    public bool HasSlashTargetBenefitUseObserver(long useId) => this._targetFactory().HasSlashTargetBenefitUseObserver(useId);
    public void AssertSlashTargetBenefitWindows() => this._targetFactory().AssertSlashTargetBenefitWindows();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public void AssertFeiYiEffectRounds() => this._targetFactory().AssertFeiYiEffectRounds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public void AssertLeChengyanEffects() => this._targetFactory().AssertLeChengyanEffects();
    public bool HasLeChengyanBorrowedPrompt() => this._targetFactory().HasLeChengyanBorrowedPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool IsSayingRemovedMaterial(long useId, int cardId) => this._targetFactory().IsSayingRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterQiaoRuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterQiaoRuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryQiaoRuiPrompt() => this._targetFactory().HasAnniversaryQiaoRuiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public bool HasSoleIncomingRangeDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasSoleIncomingRangeDrawProcessing(processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreTypedRecoveryDamageCursorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreTypedRecoveryDamageCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => this._targetFactory().ProjectTypedResponseCompletionHealthCursor();
    public global::CardGame.Core.TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor() => this._targetFactory().ProjectTypedCommittedSlashPaymentCursor();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasExactHappyBonePlacement(global::CardGame.Core.Card card, int holder) => this._targetFactory().HasExactHappyBonePlacement(card, holder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreInspectedHandSlashReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreInspectedHandSlashReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> _targetFactory = targetFactory;
    public void AssertInspectedHandSlashes() => this._targetFactory().AssertInspectedHandSlashes();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgmentOutcomeDamageEdgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgmentOutcomeDamageEdgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> _targetFactory = targetFactory;
    public bool HasExactJudgmentOutcomeSubtree(long judgmentId) => this._targetFactory().HasExactJudgmentOutcomeSubtree(judgmentId);
    public bool HasExactBeforeDamageJudgmentSubtree(long windowId) => this._targetFactory().HasExactBeforeDamageJudgmentSubtree(windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public void AssertHappyWeiYanRetainedGainSettlements(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().AssertHappyWeiYanRetainedGainSettlements(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheNullifiedClaimObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheNullifiedClaimObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> _targetFactory = targetFactory;
    public bool TryGetShanheNullifiedClaimRide(global::CardGame.Core.NullificationWindowFrame window, out int[] childProcessingCosts) => this._targetFactory().TryGetShanheNullifiedClaimRide(window, out childProcessingCosts);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramShanheSourceWarElephantAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramShanheSourceWarElephantOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateShanheWarElephantCounterParentProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().SeparateShanheWarElephantCounterParentProcessing(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool HasProgramDodgeResponseContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasProgramDodgeResponseContinuation(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeSlashTargetDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeSlashTargetDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> _targetFactory = targetFactory;
    public bool IsNativeSlashTargetDuelZeroUse(long useId) => this._targetFactory().IsNativeSlashTargetDuelZeroUse(useId);
    public bool IsNativeSlashTargetDuelAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().IsNativeSlashTargetDuelAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeStoneAxeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeStoneAxeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetStoneAxeDiscardCards(global::CardGame.Core.CharacterState source) => this._targetFactory().GetStoneAxeDiscardCards(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreKenshangPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreKenshangPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsKenshangRemovedMaterial(long useId, int cardId) => this._targetFactory().IsKenshangRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSharedGameEngineOlMoRumoProofsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSharedGameEngineOlMoRumoProofsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> _targetFactory = targetFactory;
    public void AssertOlMoRumoStates() => this._targetFactory().AssertOlMoRumoStates();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreVirtualOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreVirtualOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public void AssertProgramVirtualOrdinaryTricks() => this._targetFactory().AssertProgramVirtualOrdinaryTricks();
    public bool IsProgramVirtualOrdinaryTrickUse(long frameId) => this._targetFactory().IsProgramVirtualOrdinaryTrickUse(frameId);
    public bool IsProgramVirtualAreaAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsProgramVirtualAreaAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterYuanYinAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterYuanYinAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryYuanYinPrompt() => this._targetFactory().HasAnniversaryYuanYinPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? DiouNativeAncestorProcessing(long useId) => this._targetFactory().DiouNativeAncestorProcessing(useId);
    public bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsDiouSuspendedCompletedAttack(original, requiredWindowId);
    public bool IsDiouAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsDiouAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLvYiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLvYiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> _targetFactory = targetFactory;
    public int[]? ShiLySongyanAncestorProcessing(long id) => this._targetFactory().ShiLySongyanAncestorProcessing(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public bool HasLvKuangLvXiangCardObserver(long originalUseId) => this._targetFactory().HasLvKuangLvXiangCardObserver(originalUseId);
    public bool IsQigongCompletedAttackProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsQigongCompletedAttackProcessing(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSequentialTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSequentialTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _targetFactory = targetFactory;
    public void AssertSequentialTrickTargets() => this._targetFactory().AssertSequentialTrickTargets();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool IsQinLangXianyingAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsQinLangXianyingAttackConsistent(attack, processing);
    public bool IsQinLangXianyingBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsQinLangXianyingBorrowedAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiLsPrompt() => this._targetFactory().HasShiLsPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public void AssertWuLuKangWindows() => this._targetFactory().AssertWuLuKangWindows();
    public bool IsWuLuKangPairSubtree(global::CardGame.Core.PindianFrame pair) => this._targetFactory().IsWuLuKangPairSubtree(pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHulaoCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHulaoCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> _targetFactory = targetFactory;
    public void AssertHulaoCardState() => this._targetFactory().AssertHulaoCardState();
    public bool HasHulaoAllianceFeastPrompt() => this._targetFactory().HasHulaoAllianceFeastPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageTargetObtainProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageTargetObtainProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _targetFactory = targetFactory;
    public void AssertDamageTargetDuels() => this._targetFactory().AssertDamageTargetDuels();
    public bool IsDamageTargetDuelUse(long id) => this._targetFactory().IsDamageTargetDuelUse(id);
    public global::System.Collections.Generic.IReadOnlyList<int> DamageTargetDuelOuterProcessing(long useId) => this._targetFactory().DamageTargetDuelOuterProcessing(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSharedSlashOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSharedSlashOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedSlashOffersRuntime> _targetFactory = targetFactory;
    public void AssertSharedSlashUseReturns() => this._targetFactory().AssertSharedSlashUseReturns();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalStoneAxe { get => this._targetFactory().UsesFormalStoneAxe; }
    public bool UsesFormalQinglongCrescentBlade { get => this._targetFactory().UsesFormalQinglongCrescentBlade; }
    public bool UsesFormalIceSword { get => this._targetFactory().UsesFormalIceSword; }
    public bool UsesFormalQilinBow { get => this._targetFactory().UsesFormalQilinBow; }
    public bool UsesFormalCixiongDoubleSwords { get => this._targetFactory().UsesFormalCixiongDoubleSwords; }

    public global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentAttack(frame);
    public global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => this._targetFactory().GetDamageTriggerAttack(frame);
    public global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame) => this._targetFactory().GetDyingAttack(frame);
    public global::CardGame.Core.Card? GetDelayedJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetDelayedJudgmentCard(frame);
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
    public bool UsesFormalZhuqueFan { get => this._targetFactory().UsesFormalZhuqueFan; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterFuRongFuQianAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterFuRongFuQianAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryFuRongFuQianPrompt() => this._targetFactory().HasAnniversaryFuRongFuQianPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramStateCardContinuationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramStateCardContinuationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime> _targetFactory = targetFactory;
    public bool HasCharacterStateCardUseContinuation() => this._targetFactory().HasCharacterStateCardUseContinuation();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool HasCardsMovedProgramBoundaryFrame() => this._targetFactory().HasCardsMovedProgramBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidResponseCompletionWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDeathProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDeathProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeathFrame GetCurrentDeathFrame(long expectedId) => this._targetFactory().GetCurrentDeathFrame(expectedId);
    public void AssertKillDiedProgramInvariant() => this._targetFactory().AssertKillDiedProgramInvariant();
    public void AssertOwnerDiedProgramInvariant() => this._targetFactory().AssertOwnerDiedProgramInvariant();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreLostHpOwnedGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreLostHpOwnedGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _targetFactory = targetFactory;
    public void AssertLostHpOwnedGifts() => this._targetFactory().AssertLostHpOwnedGifts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public bool HasLeCaiYongFeibaiRescueContinuation(long dyingId) => this._targetFactory().HasLeCaiYongFeibaiRescueContinuation(dyingId);
    public bool HasLeCaiYongFeibaiUseContinuation(long nativeId) => this._targetFactory().HasLeCaiYongFeibaiUseContinuation(nativeId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public bool HasRoundEndingBoundaryFrame() => this._targetFactory().HasRoundEndingBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFactionRequestPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFactionRequestPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsFactionRequestCostPrompt(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => this._targetFactory().IsFactionRequestCostPrompt(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? ShenDaCuixinAncestorProcessing(long id) => this._targetFactory().ShenDaCuixinAncestorProcessing(id);
    public bool HasShenDaSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().HasShenDaSuspendedCompletedParent(w);
    public bool IsShenDaCuixinAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsShenDaCuixinAttackConsistent(a, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePaidOwnTargetObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePaidOwnTargetObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _targetFactory = targetFactory;
    public bool HasActualUseTargetObserver(long useId) => this._targetFactory().HasActualUseTargetObserver(useId);
    public void AssertActualUseTargetPrograms() => this._targetFactory().AssertActualUseTargetPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePaidHpLossProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePaidHpLossProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> _targetFactory = targetFactory;
    public void AssertPaidHpLossModifiers() => this._targetFactory().AssertPaidHpLossModifiers();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEquipmentDonationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEquipmentDonationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> _targetFactory = targetFactory;
    public void AssertEquipmentDonationPrograms() => this._targetFactory().AssertEquipmentDonationPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDynamicDiscardDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDynamicDiscardDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HasDynamicDiscardDamageAttackObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasDynamicDiscardDamageAttackObserver(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeSkillInvocationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeSkillInvocationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillInvocationObserversRuntime> _targetFactory = targetFactory;
    public bool HasNativeFactionSkillInvocation(long attackId) => this._targetFactory().HasNativeFactionSkillInvocation(attackId);
    public bool HasFactionDefenseSkillInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense) => this._targetFactory().HasFactionDefenseSkillInvocation(defense);
    public bool HasFactionSlashSkillInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => this._targetFactory().HasFactionSlashSkillInvocation(request);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool HasBeiMiHuTurnPreludeBoundary() => this._targetFactory().HasBeiMiHuTurnPreludeBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangHengAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangHengAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryZhangHengPrompt() => this._targetFactory().HasAnniversaryZhangHengPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreConditionalDiscardDuelReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreConditionalDiscardDuelReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> _targetFactory = targetFactory;
    public void AssertConditionalDiscardDuels() => this._targetFactory().AssertConditionalDiscardDuels();
    public bool IsConditionalDiscardDuelUse(long id) => this._targetFactory().IsConditionalDiscardDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public void AssertWenheCardState() => this._targetFactory().AssertWenheCardState();
    public bool HasWenheCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasWenheCardEffectProcessing(processing);
    public bool IsWenheChaosAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsWenheChaosAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePlacedEquipmentBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePlacedEquipmentBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlacedEquipmentBenefitObserversRuntime> _targetFactory = targetFactory;
    public void AssertPlacedEquipmentBenefits() => this._targetFactory().AssertPlacedEquipmentBenefits();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public void AssertWeijieRequests() => this._targetFactory().AssertWeijieRequests();
    public global::CardGame.Core.PendingDecision? WeijieInvariantDecision(global::CardGame.Core.PendingDecision? ordinary) => this._targetFactory().WeijieInvariantDecision(ordinary);
    public global::CardGame.Core.ResolutionFrame? WeijieInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary) => this._targetFactory().WeijieInvariantTop(ordinary);
    public bool HasWeijieQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasWeijieQinglongPaymentRide(attack);
    public bool HasWeijieNativeRequestContinuation(long owner) => this._targetFactory().HasWeijieNativeRequestContinuation(owner);
    public bool IsWeijieProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsWeijieProgramRequestAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreExtraCardEffectRoundsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreExtraCardEffectRoundsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _targetFactory = targetFactory;
    public void AssertExtraCardEffectReceipts() => this._targetFactory().AssertExtraCardEffectReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _targetFactory = targetFactory;
    public void AssertProgramJudgmentWindowState() => this._targetFactory().AssertProgramJudgmentWindowState();
    public bool IsProgramJudgmentPromptValid(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame) => this._targetFactory().IsProgramJudgmentPromptValid(frame);
    public bool IsAiProgramJudgmentPending() => this._targetFactory().IsAiProgramJudgmentPending();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXuHuangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXuHuangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouXuHuangDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouXuHuangDuelAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public bool AssertCardDeclarationInvariant() => this._targetFactory().AssertCardDeclarationInvariant();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeMaterialSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeMaterialSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime> _targetFactory = targetFactory;
    public bool IsExactNativeMaterialSlashExpansion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsExactNativeMaterialSlashExpansion(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuZglRescueContinuation(long dyingId) => this._targetFactory().HasWuZglRescueContinuation(dyingId);
    public bool HasWuZglUseContinuation(long nativeId) => this._targetFactory().HasWuZglUseContinuation(nativeId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool ValidMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidMouLuSuPlayedWindow(window);
    public bool IsMouLuSuPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsMouLuSuPlayedCostFinished(w, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void AssertGuanduCardState() => this._targetFactory().AssertGuanduCardState();
    public bool HasGuanduCardEffectProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasGuanduCardEffectProcessing(processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get => this._targetFactory().ActiveQinglongFollowup; set => this._targetFactory().ActiveQinglongFollowup = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get => this._targetFactory().ActiveStoneAxe; set => this._targetFactory().ActiveStoneAxe = value; }
    public global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get => this._targetFactory().ActiveIceSword; set => this._targetFactory().ActiveIceSword = value; }
    public global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get => this._targetFactory().ActiveQilinBow; set => this._targetFactory().ActiveQilinBow = value; }
    public global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get => this._targetFactory().ActiveCixiongDoubleSwords; set => this._targetFactory().ActiveCixiongDoubleSwords = value; }
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => this._targetFactory().ActiveFangtianHalberd; set => this._targetFactory().ActiveFangtianHalberd = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public void AssertMouBingcaiExtraEffectRounds() => this._targetFactory().AssertMouBingcaiExtraEffectRounds();
    public global::System.Collections.Generic.IReadOnlyList<int>? MouLixianNativeAttackAncestorProcessing(long useId) => this._targetFactory().MouLixianNativeAttackAncestorProcessing(useId);
    public bool IsMouLixianRemovedMaterial(long useId, int cardId) => this._targetFactory().IsMouLixianRemovedMaterial(useId, cardId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateMouLixianDamageChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().SeparateMouLixianDamageChildProcessing(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePrivateOfferObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePrivateOfferObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferObserversRuntime> _targetFactory = targetFactory;
    public void AssertPrivateOfferPrograms() => this._targetFactory().AssertPrivateOfferPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public bool ValidMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ValidMouSmPlayedWindow(window);
    public bool HasMouSmPrivateContinuation(long useId) => this._targetFactory().HasMouSmPrivateContinuation(useId);
    public bool IsMouSmPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsMouSmPlayedCostFinished(w, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSoleTargetProtectionObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSoleTargetProtectionObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> _targetFactory = targetFactory;
    public bool IsSoleTargetQuarantinedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsSoleTargetQuarantinedAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuBeiSpOl664Runtime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuBeiSpOl664Adapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBeiSpOl664Runtime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuBeiSpOl664Operations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBeiSpOl664Runtime> _targetFactory = targetFactory;
    public bool IsOlSpZhujiuDyingRide(int index, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOlSpZhujiuDyingRide(index, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSignedDamagePaymentObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSignedDamagePaymentObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> _targetFactory = targetFactory;
    public bool HasSignedDamagePaymentAttackObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasSignedDamagePaymentAttackObserver(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public void AssertHappyQiaoDamageHealth() => this._targetFactory().AssertHappyQiaoDamageHealth();
    public void AssertHappyQiaoCardMaximumHealth() => this._targetFactory().AssertHappyQiaoCardMaximumHealth();
    public void AssertHappyQiaoHealthFrames() => this._targetFactory().AssertHappyQiaoHealthFrames();
    public bool HasHappyQiaoHealthPrompt() => this._targetFactory().HasHappyQiaoHealthPrompt();
    public bool HasHappyQiaoDamageHealthContinuation(long attackId) => this._targetFactory().HasHappyQiaoDamageHealthContinuation(attackId);
    public bool HasHappyQiaoCardMaximumHealthContinuation(long attackId) => this._targetFactory().HasHappyQiaoCardMaximumHealthContinuation(attackId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateHappyQiaoDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().SeparateHappyQiaoDamageHealthChildProcessing(attack, processing);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateHappyQiaoCardMaximumHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().SeparateHappyQiaoCardMaximumHealthChildProcessing(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePindianRevealWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePindianRevealWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _targetFactory = targetFactory;
    public void AssertPindianRevealWindows() => this._targetFactory().AssertPindianRevealWindows();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePhaseHandSeizureReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePhaseHandSeizureReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> _targetFactory = targetFactory;
    public void AssertPhaseHandSeizurePrograms() => this._targetFactory().AssertPhaseHandSeizurePrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasShenZfJudgmentGainDisposition(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().HasShenZfJudgmentGainDisposition(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDrawFundedDistinctBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDrawFundedDistinctBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> _targetFactory = targetFactory;
    public bool IsDrawFundedDistinctBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsDrawFundedDistinctBasicAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuShiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuShiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> _targetFactory = targetFactory;
    public bool IsOlLuShiLeijieAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle a, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsOlLuShiLeijieAttackConsistent(a, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsCurrentSlashFireChangedUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDualColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDualColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public void AssertDualColorDuels() => this._targetFactory().AssertDualColorDuels();
    public bool IsDualColorDuelUse(long id) => this._targetFactory().IsDualColorDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuangQuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuangQuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiQuanDuelUse(long id) => this._targetFactory().IsShiQuanDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public bool IsCardAttackSuspendedByProgramDamage { get => this._targetFactory().IsCardAttackSuspendedByProgramDamage; }

    public void AssertProgramAttackState() => this._targetFactory().AssertProgramAttackState();
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }

    public bool HasDamageTriggerForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasDamageTriggerForAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreWenheChaosAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreWenheChaosOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> _targetFactory = targetFactory;
    public bool HasWenheChaosPrompt() => this._targetFactory().HasWenheChaosPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public bool IsExactNextActualUseMaterialSlash(global::CardGame.Core.NativeOwners.FangtianHalberdHandle multi, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsExactNextActualUseMaterialSlash(multi, attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public bool TryGetCompletedNullificationDyingCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame? window, out int[] costs) => this._targetFactory().TryGetCompletedNullificationDyingCosts(window, out costs);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool IsPaidZeroMaterialAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsPaidZeroMaterialAttackConsistent(attack, processing);
    public bool IsTieredRoundZeroBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsTieredRoundZeroBorrowedAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreGrantedEntityDistanceInvariantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreGrantedEntityDistanceInvariantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityDistanceInvariantsRuntime> _targetFactory = targetFactory;
    public void AssertGrantedEntityDistancePolicies() => this._targetFactory().AssertGrantedEntityDistancePolicies();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRequestedDeckBasicOwnershipAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRequestedDeckBasicOwnershipOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> _targetFactory = targetFactory;
    public void AssertRequestedDeckBasicMaterials() => this._targetFactory().AssertRequestedDeckBasicMaterials();
    public global::CardGame.Core.PendingDecision? RequestedDeckBasicInvariantDecision() => this._targetFactory().RequestedDeckBasicInvariantDecision();
    public global::CardGame.Core.ResolutionFrame? RequestedDeckBasicInvariantTop() => this._targetFactory().RequestedDeckBasicInvariantTop();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasWeaponAbility(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public bool HasForeignActualTurnStartBoundary() => this._targetFactory().HasForeignActualTurnStartBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public void AssertWuXiongmuDamageStates() => this._targetFactory().AssertWuXiongmuDamageStates();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreOwnTrickDrawTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreOwnTrickDrawTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> _targetFactory = targetFactory;
    public bool HasOwnTrickDrawOfferProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasOwnTrickDrawOfferProcessing(processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheLeizhenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheLeizhenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> _targetFactory = targetFactory;
    public void AssertShanheLeizhenUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertShanheLeizhenUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong) => this._targetFactory().TieredRoundZeroForcedSlashChoices(actor, target, originalId, qinglong);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public void AssertColorFireAttackReceipts() => this._targetFactory().AssertColorFireAttackReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreForeignSelectedCardSlashReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreForeignSelectedCardSlashReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> _targetFactory = targetFactory;
    public void AssertSelectedForeignCardSlashes() => this._targetFactory().AssertSelectedForeignCardSlashes();
    public bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => this._targetFactory().IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreOwnedDeathBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreOwnedDeathBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> _targetFactory = targetFactory;
    public void AssertOwnedDeathBenefitReturns() => this._targetFactory().AssertOwnedDeathBenefitReturns();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHaoPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHaoPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> _targetFactory = targetFactory;
    public bool IsZhenyingDuelUse(long id) => this._targetFactory().IsZhenyingDuelUse(id);
    public bool IsZhenyingDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsZhenyingDuelAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterYangBiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterYangBiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryYangBiaoPrompt() => this._targetFactory().HasAnniversaryYangBiaoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void AssertHellSoulCardState() => this._targetFactory().AssertHellSoulCardState();
    public bool HasHellSoulPrompt() => this._targetFactory().HasHellSoulPrompt();
    public bool IsHellDestroyedSoulMaterial(long useId, int cardId) => this._targetFactory().IsHellDestroyedSoulMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRecipientConsequencesObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRecipientConsequencesObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> _targetFactory = targetFactory;
    public void AssertRecipientConsequencesSubtrees() => this._targetFactory().AssertRecipientConsequencesSubtrees();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNoDamageSkillDisableAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNoDamageSkillDisableOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> _targetFactory = targetFactory;
    public void AssertNoDamageSkillDisablePolicy(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertNoDamageSkillDisablePolicy(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool HasZhuqueFan(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasZhuqueFan(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreForeignPublicPileSlashesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreForeignPublicPileSlashesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> _targetFactory = targetFactory;
    public bool IsForeignPublicPileSlashUse(long resolutionId) => this._targetFactory().IsForeignPublicPileSlashUse(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterBaoSanNiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterBaoSanNiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> _targetFactory = targetFactory;
    public void AssertChosenAssaults() => this._targetFactory().AssertChosenAssaults();
    public bool IsChosenAssaultAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsChosenAssaultAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDrawPhaseObligationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDrawPhaseObligationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> _targetFactory = targetFactory;
    public bool HasDrawPhaseObligationBoundaryFrame() => this._targetFactory().HasDrawPhaseObligationBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXiahouHuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXiahouHuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryXiahouHuiPrompt() => this._targetFactory().HasAnniversaryXiahouHuiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDiaoChanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDiaoChanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyDiaoChanPrompt() => this._targetFactory().HasHappyDiaoChanPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void AssertProgramSkillState() => this._targetFactory().AssertProgramSkillState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLuMengMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLuMengMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> _targetFactory = targetFactory;
    public bool AssertPreEntryDiscardPolicyChoice(global::CardGame.Core.PendingDecision pending) => this._targetFactory().AssertPreEntryDiscardPolicyChoice(pending);
    public bool HasNativeDiscardEntryBoundary() => this._targetFactory().HasNativeDiscardEntryBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterQiaoZhouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterQiaoZhouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> _targetFactory = targetFactory;
    public bool HasQiaoZhouPreparationBoundaryFrame() => this._targetFactory().HasQiaoZhouPreparationBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreWenyiDamageHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreWenyiDamageHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> _targetFactory = targetFactory;
    public void AssertWenyiDamageHealth() => this._targetFactory().AssertWenyiDamageHealth();
    public bool HasWenyiDamageHealthContinuation(long attackFrameId) => this._targetFactory().HasWenyiDamageHealthContinuation(attackFrameId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateWenyiDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().SeparateWenyiDamageHealthChildProcessing(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public void AssertHuanpeiRequests() => this._targetFactory().AssertHuanpeiRequests();
    public global::CardGame.Core.PendingDecision? HuanpeiInvariantDecision(global::CardGame.Core.PendingDecision? ordinary) => this._targetFactory().HuanpeiInvariantDecision(ordinary);
    public global::CardGame.Core.ResolutionFrame? HuanpeiInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary) => this._targetFactory().HuanpeiInvariantTop(ordinary);
    public bool HasHuanpeiBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => this._targetFactory().HasHuanpeiBorrowedPaymentRide(pending);
    public bool HasHuanpeiQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasHuanpeiQinglongPaymentRide(attack);
    public bool HasHuanpeiNativeRequestContinuation(long parentId) => this._targetFactory().HasHuanpeiNativeRequestContinuation(parentId);
    public bool IsHuanpeiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsHuanpeiAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public void AssertDouyuWindows() => this._targetFactory().AssertDouyuWindows();
    public bool HasBiguoPindianContinuation() => this._targetFactory().HasBiguoPindianContinuation();
    public bool IsDouyuPairSubtree(global::CardGame.Core.PindianFrame pair) => this._targetFactory().IsDouyuPairSubtree(pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiZySuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasShiZySuspendedCompletedParent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public void AssertWumiaoFlood(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertWumiaoFlood(use);
    public bool HasWumiaoFloodDiscardPrompt() => this._targetFactory().HasWumiaoFloodDiscardPrompt();
    public bool HasWumiaoFloodProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasWumiaoFloodProcessing(processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool HasWenhePoisonFireAttackPrompt() => this._targetFactory().HasWenhePoisonFireAttackPrompt();
    public bool IsCurrentWenhePoisonDying() => this._targetFactory().IsCurrentWenhePoisonDying();
    public bool HasExactWenhePoisonResponseContinuation() => this._targetFactory().HasExactWenhePoisonResponseContinuation();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHanSongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHanSongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHanSongPrompt() => this._targetFactory().HasAnniversaryHanSongPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterBaiLingYunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterBaiLingYunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryBaiLingYunPrompt() => this._targetFactory().HasAnniversaryBaiLingYunPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDuKuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDuKuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasDuKuiPrompt() => this._targetFactory().HasDuKuiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasMobileSimaRenjieNativeSubtree(long useId) => this._targetFactory().HasMobileSimaRenjieNativeSubtree(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageArmorPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageArmorPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _targetFactory = targetFactory;
    public void AssertDamageArmorPayments() => this._targetFactory().AssertDamageArmorPayments();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShownEntityTurnPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShownEntityTurnPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _targetFactory = targetFactory;
    public void AssertShownEntityTurnPrograms() => this._targetFactory().AssertShownEntityTurnPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXinPiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXinPiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _targetFactory = targetFactory;
    public bool HasXinPiCardObserver(long useId) => this._targetFactory().HasXinPiCardObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDelayedPlacementTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDelayedPlacementTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _targetFactory = targetFactory;
    public bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsExactFinalizedDelayedPlacementWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheS5YimieObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheS5YimieObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieObserversRuntime> _targetFactory = targetFactory;
    public bool HasShanheS5YimieRecoveryContinuation(long attackId) => this._targetFactory().HasShanheS5YimieRecoveryContinuation(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void AssertSkillInvocations() => this._targetFactory().AssertSkillInvocations();
    public bool HasExactBoundarySkillInvocation() => this._targetFactory().HasExactBoundarySkillInvocation();
    public bool IsExactSkillInvocationDying(global::CardGame.Core.DyingFrame? dying) => this._targetFactory().IsExactSkillInvocationDying(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public void AssertProgramCardWindowState() => this._targetFactory().AssertProgramCardWindowState();
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ProgramCardAttack { get => this._targetFactory().ProgramCardAttack; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterAHuiNanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterAHuiNanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _targetFactory = targetFactory;
    public bool IsAHuiNanJuemanAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsAHuiNanJuemanAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuCheErAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuCheErAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHuCheErPrompt() => this._targetFactory().HasAnniversaryHuCheErPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangWenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangWenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryZhangWenPrompt() => this._targetFactory().HasAnniversaryZhangWenPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHeQiMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHeQiMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime> _targetFactory = targetFactory;
    public bool HasMobileHeQiPrompt() => this._targetFactory().HasMobileHeQiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryCuiLiePrompt() => this._targetFactory().HasAnniversaryCuiLiePrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramViewContributorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramViewContributorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramViewContributorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramViewContributors ViewContributors { get => this._targetFactory().ViewContributors; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int>? MouDanchiAncestorProcessing(long id) => this._targetFactory().MouDanchiAncestorProcessing(id);
    public bool HasMouDanchiSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().HasMouDanchiSuspendedCompletedParent(window);
    public bool IsMouDanchiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouDanchiAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterTianChouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterTianChouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> _targetFactory = targetFactory;
    public void AssertTianChouNativeUses() => this._targetFactory().AssertTianChouNativeUses();
    public bool IsTianChouShandaoAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsTianChouShandaoAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public void AssertSelectedActorDuels() => this._targetFactory().AssertSelectedActorDuels();
    public bool IsSelectedActorDuelUse(long id) => this._targetFactory().IsSelectedActorDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramVirtualDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramVirtualDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> _targetFactory = targetFactory;
    public bool IsProgramVirtualDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsProgramVirtualDuelAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterFengXiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterFengXiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryFengXiPrompt() => this._targetFactory().HasAnniversaryFengXiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheS5YimieAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheS5YimieOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> _targetFactory = targetFactory;
    public void AssertShanheS5YimieRecoveryFrames() => this._targetFactory().AssertShanheS5YimieRecoveryFrames();
    public bool HasShanheS5YimieRecovery(long attackId) => this._targetFactory().HasShanheS5YimieRecovery(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWenChouMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWenChouMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouWenChouDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouWenChouDuelAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterYanXiangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterYanXiangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryYanXiangPrompt() => this._targetFactory().HasAnniversaryYanXiangPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMaMiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMaMiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryMaMiaoPrompt() => this._targetFactory().HasAnniversaryMaMiaoPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXuJingAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXuJingAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryXuJingPrompt() => this._targetFactory().HasAnniversaryXuJingPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterWuFuAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterWuFuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryWuFuPrompt() => this._targetFactory().HasAnniversaryWuFuPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieCompletedUseObserver(long useId) => this._targetFactory().HasChenglieCompletedUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHeJinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHeJinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _targetFactory = targetFactory;
    public bool IsHeJinMouzhuDuelUse(long id) => this._targetFactory().IsHeJinMouzhuDuelUse(id);
    public bool IsHeJinMouzhuAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsHeJinMouzhuAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed class CoreNativeCoreInvariantsCorePindianPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCorePindianPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> _targetFactory = targetFactory;
    public int[] CurrentPindianProcessingIds(global::CardGame.Core.PindianFrame frame) => this._targetFactory().CurrentPindianProcessingIds(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => this._targetFactory().IsProgramAlternativeCost(action, cardId);
    public bool TryValidateProgramAlternativeCostAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing, out bool valid) => this._targetFactory().TryValidateProgramAlternativeCostAttack(attack, processing, out valid);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsEphemeralCardDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool AssertDyingSuitsSubtree() => this._targetFactory().AssertDyingSuitsSubtree();
    public bool HasDyingSuitsSuspendedAttack(long attackId) => this._targetFactory().HasDyingSuitsSuspendedAttack(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangFenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangFenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasZhangFenPrompt() => this._targetFactory().HasZhangFenPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.SkillProgram Program, global::CardGame.Core.SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(global::CardGame.Core.JudgmentTriggerCandidate candidate) => this._targetFactory().GetProgramJudgmentReplacement(candidate);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false) => this._targetFactory().GetProgramJudgmentReplacementCards(owner, trigger, judgmentSubjectSeat, judgmentReason, ignoreCurrentColor);
    public bool IsAiProgramJudgmentReplacementPending() => this._targetFactory().IsAiProgramJudgmentReplacementPending();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeQilinBowAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeQilinBowOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetQilinBowTargetMounts(global::CardGame.Core.CharacterState target) => this._targetFactory().GetQilinBowTargetMounts(target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetQinglongCrescentBladeSlashCards(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => this._targetFactory().GetQinglongCrescentBladeSlashCards(source, target);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QinglongPhysicalSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> slashes) => this._targetFactory().QinglongPhysicalSlashChoices(pending, slashes);
    public bool CanRequestQinglongCrescentBladeFactionSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => this._targetFactory().CanRequestQinglongCrescentBladeFactionSlash(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuZhangFeiUseContinuation(long nativeUseId) => this._targetFactory().HasWuZhangFeiUseContinuation(nativeUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public bool HasHappyQiaoNativeDamageChoice(long attackId) => this._targetFactory().HasHappyQiaoNativeDamageChoice(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreDrawFundedDistinctBasicObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreDrawFundedDistinctBasicObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> _targetFactory = targetFactory;
    public void AssertDrawFundedDistinctBasicFramesAndUses() => this._targetFactory().AssertDrawFundedDistinctBasicFramesAndUses();
    public global::CardGame.Core.PendingDecision? DrawFundedDistinctBasicInvariantDecision(global::CardGame.Core.PendingDecision? nativeDecision) => this._targetFactory().DrawFundedDistinctBasicInvariantDecision(nativeDecision);
    public global::CardGame.Core.ResolutionFrame? DrawFundedDistinctBasicInvariantTop(global::CardGame.Core.ResolutionFrame? nativeTop) => this._targetFactory().DrawFundedDistinctBasicInvariantTop(nativeTop);
    public global::CardGame.Core.DrawFundedDistinctBasicFrame? DrawFundedDistinctBasicObserverRoot(long? requestedId = default) => this._targetFactory().DrawFundedDistinctBasicObserverRoot(requestedId);
    public bool HasDrawFundedDistinctBasicObserver(long cardUseId) => this._targetFactory().HasDrawFundedDistinctBasicObserver(cardUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyZuoCiPrompt() => this._targetFactory().HasHappyZuoCiPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool HasProgramLifecycleBoundaryFrame() => this._targetFactory().HasProgramLifecycleBoundaryFrame();
    public bool HasTurnEndingBoundaryFrame() => this._targetFactory().HasTurnEndingBoundaryFrame();
    public bool HasPlayPhaseStartingBoundaryFrame() => this._targetFactory().HasPlayPhaseStartingBoundaryFrame();
    public bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame) => this._targetFactory().IsValidProgramJudgmentContinuation(pending, frame);
    public bool HasPendingProgramBoundCards { get => this._targetFactory().HasPendingProgramBoundCards; }

    public bool IsProgramProcessingConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsProgramProcessingConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public void AssertQizheng(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertQizheng(use);
    public bool HasQizhengPrompt() => this._targetFactory().HasQizhengPrompt();
    public bool HasQizhengProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().HasQizhengProcessing(processing);
    public bool HasQizhengResponseContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasQizhengResponseContinuation(attack);
    public bool IsVirtualQizhengAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsVirtualQizhengAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public void AssertRecoveryReplacementInvariants() => this._targetFactory().AssertRecoveryReplacementInvariants();
    public bool HasRecoveryPaidCardUseObserver(long useId) => this._targetFactory().HasRecoveryPaidCardUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasShiGyPrompt() => this._targetFactory().HasShiGyPrompt();
    public bool HasShiGyCompletedUseObserver(long id) => this._targetFactory().HasShiGyCompletedUseObserver(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNonBasicSlashProfileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNonBasicSlashProfileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _targetFactory = targetFactory;
    public void AssertNonBasicSlashPolicies() => this._targetFactory().AssertNonBasicSlashPolicies();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsClaimedUseCardEntity(long frameId, int cardId) => this._targetFactory().IsClaimedUseCardEntity(frameId, cardId);
    public void AssertCurrentUsePhysicalClaims() => this._targetFactory().AssertCurrentUsePhysicalClaims();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhangXunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhangXunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryZhangXunPrompt() => this._targetFactory().HasAnniversaryZhangXunPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool IsMouDcZeroAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouDcZeroAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().CurrentJudgmentCandidate(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreMultiplayerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreMultiplayerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> _targetFactory = targetFactory;
    public bool IsHumanControlledSeat(int seat) => this._targetFactory().IsHumanControlledSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterFanJiangZhangDaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterFanJiangZhangDaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> _targetFactory = targetFactory;
    public bool IsJueshengDuelUse(long id) => this._targetFactory().IsJueshengDuelUse(id);
    public bool IsJueshengDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsJueshengDuelAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreEquipmentPairDyingCardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreEquipmentPairDyingCardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairDyingCardObserversRuntime> _targetFactory = targetFactory;
    public bool IsExactDyingOwnedCardRide(long dyingId) => this._targetFactory().IsExactDyingOwnedCardRide(dyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreExtraEffectForeignSelectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreExtraEffectForeignSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> _targetFactory = targetFactory;
    public bool AssertExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PendingDecision? decision, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().AssertExtraForeignSelection(pending, decision, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuYongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuYongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryLiuYongPrompt() => this._targetFactory().HasAnniversaryLiuYongPrompt();
    public bool IsAnniversaryLiuYongSlashConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsAnniversaryLiuYongSlashConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreKuangfuReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreKuangfuReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> _targetFactory = targetFactory;
    public void AssertKuangfuPrograms() => this._targetFactory().AssertKuangfuPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong) => this._targetFactory().RoundOnceCardlessForcedSlashChoices(actor, target, originalId, qinglong);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHaoMengAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHaoMengAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHaoMengPrompt() => this._targetFactory().HasAnniversaryHaoMengPrompt();
    public bool IsHaoMengMaterialSlashAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsHaoMengMaterialSlashAttack(pending, attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public void AssertMaximumHpDamageReplacementReceipts() => this._targetFactory().AssertMaximumHpDamageReplacementReceipts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFixedDistanceOneDebtAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFixedDistanceOneDebtOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _targetFactory = targetFactory;
    public void AssertFixedDistanceOnePolicies() => this._targetFactory().AssertFixedDistanceOnePolicies();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreFirstSlashDuelLegacyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreFirstSlashDuelLegacyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _targetFactory = targetFactory;
    public bool HasFirstSlashDuelLegacyUseObserver(long id) => this._targetFactory().HasFirstSlashDuelLegacyUseObserver(id);
    public void AssertFirstSlashDuelLegacyWindows() => this._targetFactory().AssertFirstSlashDuelLegacyWindows();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreOtherDyingRecoveryRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreOtherDyingRecoveryRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherDyingRecoveryRidesRuntime> _targetFactory = targetFactory;
    public bool IsExactOtherDyingRecoveryRide(long dyingId) => this._targetFactory().IsExactOtherDyingRecoveryRide(dyingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreCardActionContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreCardActionContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionContestsRuntime> _targetFactory = targetFactory;
    public bool HasCardActionPindianContinuation() => this._targetFactory().HasCardActionPindianContinuation();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? BoundaryLiuFengInvariantDecision(global::CardGame.Core.PendingDecision? ordinary) => this._targetFactory().BoundaryLiuFengInvariantDecision(ordinary);
    public global::CardGame.Core.ResolutionFrame? BoundaryLiuFengInvariantTop(global::CardGame.Core.ResolutionFrame? ordinary) => this._targetFactory().BoundaryLiuFengInvariantTop(ordinary);
    public void AssertBoundaryLiuFengRequests() => this._targetFactory().AssertBoundaryLiuFengRequests();
    public bool HasBoundaryLiuFengPrompt() => this._targetFactory().HasBoundaryLiuFengPrompt();
    public bool HasBoundaryLiuFengBorrowedSettlementRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle b) => this._targetFactory().HasBoundaryLiuFengBorrowedSettlementRide(b);
    public bool HasBoundaryLiuFengQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().HasBoundaryLiuFengQinglongPaymentRide(attack);
    public bool HasBoundaryLiuFengNativeRequestContinuation(long ownerFrameId) => this._targetFactory().HasBoundaryLiuFengNativeRequestContinuation(ownerFrameId);
    public bool IsBoundaryLiuFengNativeAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsBoundaryLiuFengNativeAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhouCangBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhouCangBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryZhouCangCompletedUseObserver(long useId) => this._targetFactory().HasBoundaryZhouCangCompletedUseObserver(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreQinglongInsertionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreQinglongInsertionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> _targetFactory = targetFactory;
    public void AssertQinglongInsertions() => this._targetFactory().AssertQinglongInsertions();
    public global::System.Collections.Generic.HashSet<int> QinglongAncestorProcessing(long attackId) => this._targetFactory().QinglongAncestorProcessing(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreShanheMarketAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreShanheMarketOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime> _targetFactory = targetFactory;
    public bool HasShanheMarketBoundary() => this._targetFactory().HasShanheMarketBoundary();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool IsZeroEntityProcessingParent(long id, int cardId) => this._targetFactory().IsZeroEntityProcessingParent(id, cardId);
    public bool IsZeroEntityUse(long id) => this._targetFactory().IsZeroEntityUse(id);
    public bool IsMouZhijueZeroAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouZhijueZeroAttackConsistent(attack, processing);
    public bool IsMouZhijueBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsMouZhijueBorrowedAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreChainedStateBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreChainedStateBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _targetFactory = targetFactory;
    public void AssertChainedStateBasicFramesAndUses() => this._targetFactory().AssertChainedStateBasicFramesAndUses();
    public global::CardGame.Core.PendingDecision? ChainedStateBasicInvariantDecision(global::CardGame.Core.PendingDecision? native) => this._targetFactory().ChainedStateBasicInvariantDecision(native);
    public global::CardGame.Core.ResolutionFrame? ChainedStateBasicInvariantTop(global::CardGame.Core.ResolutionFrame? native) => this._targetFactory().ChainedStateBasicInvariantTop(native);
    public bool HasChainedStateBasicObserver(long id) => this._targetFactory().HasChainedStateBasicObserver(id);
    public bool IsChainedStateBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => this._targetFactory().IsChainedStateBasicAttackConsistent(attack, processing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJudgedRankSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJudgedRankSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> _targetFactory = targetFactory;
    public void AssertJudgedRankSlashPolicies() => this._targetFactory().AssertJudgedRankSlashPolicies();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreJoinedTrickDamageRewardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreJoinedTrickDamageRewardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> _targetFactory = targetFactory;
    public void AssertJoinedTrickDamageBenefits(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertJoinedTrickDamageBenefits(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public void AssertCurrentTurnOwnSkillSuppressions() => this._targetFactory().AssertCurrentTurnOwnSkillSuppressions();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool HasHuaManCardContinuation(long useId) => this._targetFactory().HasHuaManCardContinuation(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime))]
internal sealed class CoreNativeCoreInvariantsCoreNativeDrawAttemptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCoreNativeDrawAttemptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> _targetFactory = targetFactory;
    public void AssertNativeDrawAttempts() => this._targetFactory().AssertNativeDrawAttempts();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime))]
internal sealed class CoreNativeCoreInvariantsCharacterXingDaoRongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCharacterXingDaoRongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryXingDaoRongPrompt() => this._targetFactory().HasAnniversaryXingDaoRongPrompt();
}
