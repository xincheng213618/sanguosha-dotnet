#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreSkillProgramsCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public void AssertProvenanceClaim(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertProvenanceClaim(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime))]
internal sealed class CoreSkillProgramsCoreComparedBlackSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreComparedBlackSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _targetFactory = targetFactory;
    public void AssertComparedBlackSlashReceipt(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertComparedBlackSlashReceipt(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime))]
internal sealed class CoreSkillProgramsCoreShanheS5XuncanAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreShanheS5XuncanOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> _targetFactory = targetFactory;
    public void AssertShanheS5XuncanNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertShanheS5XuncanNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CoreSkillProgramsCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => this._targetFactory().HasSkillRoleQualification(owner, skillId, instance, role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreSkillProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreSkillProgramsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime))]
internal sealed class CoreSkillProgramsCorePrepDiscardReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePrepDiscardReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> _targetFactory = targetFactory;
    public void AssertPrepDiscardReceipts(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertPrepDiscardReceipts(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime))]
internal sealed class CoreSkillProgramsCoreProvenanceVirtualAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProvenanceVirtualAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> _targetFactory = targetFactory;
    public void AssertProvenanceAlcohol(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertProvenanceAlcohol(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime))]
internal sealed class CoreSkillProgramsCoreFireTargetBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFireTargetBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> _targetFactory = targetFactory;
    public void AssertFireTargetBenefit(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertFireTargetBenefit(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreSkillProgramsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false) => this._targetFactory().CurrentHappyQiaoPool(seat, parentFrameId, healthWrite);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreSkillProgramsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed class CoreSkillProgramsCoreSlashSuitDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreSlashSuitDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _targetFactory = targetFactory;
    public void AssertProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramSlashSuitDiscard(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreQuotaTopProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreQuotaTopProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _targetFactory = targetFactory;
    public void AssertQuotaTop(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertQuotaTop(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime))]
internal sealed class CoreSkillProgramsCoreConfiguredConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreConfiguredConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> _targetFactory = targetFactory;
    public void AssertConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertConfiguredCardDeclaration(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterWangChangMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterWangChangMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiWangChangChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => this._targetFactory().IsMobileShiWangChangChoiceResult(f, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterManChongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterManChongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryManChongChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsBoundaryManChongChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed class CoreSkillProgramsCoreRoundOnceCardlessNativeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRoundOnceCardlessNativeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _targetFactory = targetFactory;
    public void AssertRoundOnceCardlessNativeResponses() => this._targetFactory().AssertRoundOnceCardlessNativeResponses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreEquipmentSlotGroupProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreEquipmentSlotGroupProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLegalityParticipant CreateProgramLegalityParticipant(int seat) => this._targetFactory().CreateProgramLegalityParticipant(seat);
    public bool CanBePindianTarget(int sourceSeat, int targetSeat) => this._targetFactory().CanBePindianTarget(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterChenQunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterChenQunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiChenQunFaenChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsMobileShiChenQunFaenChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreSkillProgramsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool IsRuleWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsRuleWounded(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreDyingEntryProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDyingEntryProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying) => this._targetFactory().TryBeginDyingEntryProgramWindow(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime))]
internal sealed class CoreSkillProgramsCoreNamedDefenseAndPublicDraftAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNamedDefenseAndPublicDraftOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> _targetFactory = targetFactory;
    public void AssertNamedDefenseAndPublicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertNamedDefenseAndPublicDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime))]
internal sealed class CoreSkillProgramsCharacterHanFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHanFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime> _targetFactory = targetFactory;
    public bool HasPaidShuziInitialSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidShuziInitialSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime))]
internal sealed class CoreSkillProgramsCoreUnnullifiableOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreUnnullifiableOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public void AssertOrdinaryTrickCannotNullify(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertOrdinaryTrickCannotNullify(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreSkillProgramsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectedGlobalCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramSelectedGlobalCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome UseProgramSelectedCardsAsGlobal(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().UseProgramSelectedCardsAsGlobal(frame, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreSkillProgramsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasDiscardableHeBy(int actorSeat, int ownerSeat) => this._targetFactory().HasDiscardableHeBy(actorSeat, ownerSeat);
    public bool HasForeignDiscardCapability { get => this._targetFactory().HasForeignDiscardCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime))]
internal sealed class CoreSkillProgramsCoreAssistedPhysicalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAssistedPhysicalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> _targetFactory = targetFactory;
    public void AssertAssistedPhysicalCardDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertAssistedPhysicalCardDrafts(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreSkillProgramsCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime))]
internal sealed class CoreSkillProgramsCharacterZhangXunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhangXunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasExactAnniversaryZhangXunFollowupSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasExactAnniversaryZhangXunFollowupSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime))]
internal sealed class CoreSkillProgramsCharacterPeiYuanShaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterPeiYuanShaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> _targetFactory = targetFactory;
    public bool HasIssuedFulueRequest(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasIssuedFulueRequest(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime))]
internal sealed class CoreSkillProgramsCoreCompletedUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCompletedUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> _targetFactory = targetFactory;
    public void AssertCompletedUsePayment(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertCompletedUsePayment(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreSkillProgramsCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime))]
internal sealed class CoreSkillProgramsCoreAppliedDamageBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAppliedDamageBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> _targetFactory = targetFactory;
    public string? DistinctTurnTargetUsage(global::CardGame.Core.SkillProgramActivation activation) => this._targetFactory().DistinctTurnTargetUsage(activation);
    public bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => this._targetFactory().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime))]
internal sealed class CoreSkillProgramsCoreShanheS5ZhangliangAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreShanheS5ZhangliangOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> _targetFactory = targetFactory;
    public bool HasExactOwnTurnEnemyHandGainSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactOwnTurnEnemyHandGainSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterXiahouShangMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterXiahouShangMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasExactMobileShiTanfengParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactMobileShiTanfengParticipantSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime))]
internal sealed class CoreSkillProgramsCoreNearestLegalSlashRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNearestLegalSlashRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> _targetFactory = targetFactory;
    public bool IsValidNearestSlashProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> instructions) => this._targetFactory().IsValidNearestSlashProgramSelection(frame, effect, instructions);
    public bool IsNearestFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsNearestFactionChoiceResult(frame, binding);
    public void AssertNearestLegalSlashState(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertNearestLegalSlashState(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime))]
internal sealed class CoreSkillProgramsCharacterHeQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHeQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CaptureEquipmentSuitProgramSource(frame);
    public bool ValidateEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateEquipmentSuitProgramSource(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed class CoreSkillProgramsCoreForeignContestAidObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreForeignContestAidObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _targetFactory = targetFactory;
    public void AssertForeignContestAidPrograms() => this._targetFactory().AssertForeignContestAidPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramRevealsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramRevealsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> _targetFactory = targetFactory;
    public void AssertProgramRevealCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramRevealCardSelection(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreSkillProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreSkillProgramsCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public void AssertAlternatingSuitTop(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertAlternatingSuitTop(f, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime))]
internal sealed class CoreSkillProgramsCoreSharedPhaseSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreSharedPhaseSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> _targetFactory = targetFactory;
    public bool TryExecuteSharedProgramActivation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryExecuteSharedProgramActivation(actor, action, cards, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreSkillProgramsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime))]
internal sealed class CoreSkillProgramsCorePhaseHandSeizureReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePhaseHandSeizureReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureReceiptsRuntime> _targetFactory = targetFactory;
    public void AssertPhaseHandSeizure(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertPhaseHandSeizure(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreActionCategoryGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreActionCategoryGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> _targetFactory = targetFactory;
    public void AssertDifferentActionCategoryGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertDifferentActionCategoryGift(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime))]
internal sealed class CoreSkillProgramsCoreSequentialDiscardReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreSequentialDiscardReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidSequentialDiscard(f);
    public void AssertSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertSequentialDiscard(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed class CoreSkillProgramsCharacterYangBiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterYangBiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasExactAnniversaryYangBiaoParticipantSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasExactAnniversaryYangBiaoParticipantSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreConvertingGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreConvertingGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _targetFactory = targetFactory;
    public void AssertConvertingGift(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertConvertingGift(f, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreSkillProgramsCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public void AssertHuangQuanNativeUses() => this._targetFactory().AssertHuangQuanNativeUses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreSkillProgramsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public void AssertTieredRoundConversionUses() => this._targetFactory().AssertTieredRoundConversionUses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreDiscardBudgetAndFactionGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDiscardBudgetAndFactionGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> _targetFactory = targetFactory;
    public void AssertBudgetGiftDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertBudgetGiftDraft(f, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramCompoundCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramCompoundCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> _targetFactory = targetFactory;
    public void AssertProgramCompoundCardTarget(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramCompoundCardTarget(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime))]
internal sealed class CoreSkillProgramsCharacterZhangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> _targetFactory = targetFactory;
    public bool ValidZhangJiNativeUse(global::CardGame.Core.CardUseFrame u) => this._targetFactory().ValidZhangJiNativeUse(u);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreSkillProgramsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreSkillProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default) => this._targetFactory().GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference);
    public bool IsProgramTargetEligible(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, int targetSeat, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default) => this._targetFactory().IsProgramTargetEligible(ownerSeat, targetKind, zones, targetSeat, marker, windowContext, actorReference);
    public global::CardGame.Core.ProgramAiPublicContext CreateProgramAiPublicContext(global::CardGame.Core.CharacterState owner) => this._targetFactory().CreateProgramAiPublicContext(owner);
    public bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying) => this._targetFactory().TryBeginMandatorySelfDyingProgram(dying);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime))]
internal sealed class CoreSkillProgramsCoreZhaoguMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreZhaoguMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> _targetFactory = targetFactory;
    public void AssertZhaoguMirror(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paused) => this._targetFactory().AssertZhaoguMirror(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreSkillProgramsCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default) => this._targetFactory().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreSkillProgramsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public void AssertMouZhijueProgram(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertMouZhijueProgram(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramChoiceProofsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramChoiceProofsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime> _targetFactory = targetFactory;
    public bool HasDeclaredProgramChoiceProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().HasDeclaredProgramChoiceProof(frame, plan, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime))]
internal sealed class CoreSkillProgramsCoreRedDiscardRecoveryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRedDiscardRecoveryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> _targetFactory = targetFactory;
    public void AssertRedDiscardRecoveryDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertRedDiscardRecoveryDraft(f, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed class CoreSkillProgramsCoreFixedRecipientBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFixedRecipientBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _targetFactory = targetFactory;
    public bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidFixedRecipientReceipt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreSkillProgramsCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public void AssertHandComparison(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op) => this._targetFactory().AssertHandComparison(f, op);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreRevealRecastProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRevealRecastProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> _targetFactory = targetFactory;
    public void AssertPairedHandRevealChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertPairedHandRevealChoice(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed class CoreSkillProgramsCoreNativeEquipmentUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeEquipmentUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _targetFactory = targetFactory;
    public void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId) => this._targetFactory().CompleteEquipmentUse(source, equipment, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime))]
internal sealed class CoreSkillProgramsCoreAdjacentDiscardStorageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAdjacentDiscardStorageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> _targetFactory = targetFactory;
    public void AssertPendingAdjacentDiscardOrigins() => this._targetFactory().AssertPendingAdjacentDiscardOrigins();
    public void AssertAdjacentDiscardStorage(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertAdjacentDiscardStorage(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterBuLianShiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterBuLianShiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool ValidBoundaryBuLianShiAnxuLeastSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidBoundaryBuLianShiAnxuLeastSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterCaoZhenBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterCaoZhenBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundarySidiChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsBoundarySidiChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime))]
internal sealed class CoreSkillProgramsCharacterHuangQuanMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHuangQuanMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> _targetFactory = targetFactory;
    public global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default) => this._targetFactory().NativeGiveCompletion(producer, giver, recipient, ids, before, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreSkillProgramsCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public void AssertTieredRoundResponseUses() => this._targetFactory().AssertTieredRoundResponseUses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CoreSkillProgramsCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasWeaponAbility(player, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterXiahouShiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterXiahouShiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasPaidMouXiahouShiSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidMouXiahouShiSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed class CoreSkillProgramsCharacterXiaoQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterXiaoQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _targetFactory = targetFactory;
    public bool HasPaidWeiwanSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidWeiwanSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreCappedHandRefreshProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCappedHandRefreshProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> _targetFactory = targetFactory;
    public void AssertCappedHandRefresh(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertCappedHandRefresh(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime))]
internal sealed class CoreSkillProgramsCoreOffTurnUsedCardObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOffTurnUsedCardObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _targetFactory = targetFactory;
    public void AssertOffTurnUsedCardGiftObservers() => this._targetFactory().AssertOffTurnUsedCardGiftObservers();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime))]
internal sealed class CoreSkillProgramsCharacterLiuHongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLiuHongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> _targetFactory = targetFactory;
    public void AssertYujuePending(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertYujuePending(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterZhuZhiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhuZhiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasExactBoundaryAnguoRescueTarget(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactBoundaryAnguoRescueTarget(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime))]
internal sealed class CoreSkillProgramsCharacterZhuZhiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhuZhiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryZhuZhiChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsAnniversaryZhuZhiChoiceResult(f, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreSkillProgramsCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByWuZishengMovement(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterGuYongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterGuYongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryGuYongChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsBoundaryGuYongChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramOwnedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramOwnedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _targetFactory = targetFactory;
    public void AssertProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramOwnedCardSelection(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreSkillProgramsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => this._targetFactory().FieldEquipmentSlot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime))]
internal sealed class CoreSkillProgramsCoreUniqueHpTargetAnnouncementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreUniqueHpTargetAnnouncementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> _targetFactory = targetFactory;
    public void AssertUniqueHpTargetAnnouncements() => this._targetFactory().AssertUniqueHpTargetAnnouncements();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime))]
internal sealed class CoreSkillProgramsCoreShenwuExpandedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreShenwuExpandedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasExactShenwuYanyuParticipant(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactShenwuYanyuParticipant(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime))]
internal sealed class CoreSkillProgramsCoreCurrentTurnHandExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCurrentTurnHandExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnHandExchangeRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnHandExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => this._targetFactory().IsCurrentTurnHandExchangeMovement(frame, effect, pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreLostHpOwnedGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreLostHpOwnedGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? LostHpOwnedGiftObserverRoot() => this._targetFactory().LostHpOwnedGiftObserverRoot();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CoreSkillProgramsCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public bool TryBeginEquipmentTargetPrograms(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long frameId) => this._targetFactory().TryBeginEquipmentTargetPrograms(source, equipment, frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreSkillProgramsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreSkillProgramsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool HasPaidWeifuInitialSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidWeifuInitialSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed class CoreSkillProgramsCoreOwnedDeathBenefitReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOwnedDeathBenefitReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _targetFactory = targetFactory;
    public bool ExactOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ExactOwnedDeathBenefitReturn(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime))]
internal sealed class CoreSkillProgramsCoreHpConditionalSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHpConditionalSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureHpConditionalProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CaptureHpConditionalProgramSource(frame);
    public bool CanContinueHpConditionalProgram(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CanContinueHpConditionalProgram(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterZhangRenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhangRenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasExactShiZrFuanStrikeTarget(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactShiZrFuanStrikeTarget(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreDeckProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDeckProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> _targetFactory = targetFactory;
    public void AssertDeckProgramDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertDeckProgramDrafts(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramCompletionRuntime))]
internal sealed class CoreSkillProgramsProgramCompletionAdapter(global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsProgramCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCompletionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreSkillProgramsCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramNextActualUseAdjustment? FreezeNextActualUseProgramSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().FreezeNextActualUseProgramSelection(owner, plan, targets);
    public void PrepareNextActualUseSelectedRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().PrepareNextActualUseSelectedRecovery(frame, selection);
    public global::CardGame.Core.SkillProgramStepOutcome BeginNextActualUseSelectedSlash(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().BeginNextActualUseSelectedSlash(frame, selection);
    public bool HasExactNextActualUseAdjustedProgramReturn(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasExactNextActualUseAdjustedProgramReturn(use);
    public bool HasExactNextActualUseProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().HasExactNextActualUseProgramSelection(frame, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterCaoPiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterCaoPiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryCaoPiChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => this._targetFactory().IsBoundaryCaoPiChoiceResult(f, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramLostHpChainedPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramLostHpChainedPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> _targetFactory = targetFactory;
    public void AssertLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertLostHpChainedPayment(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreTriggeredCardProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTriggeredCardProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> _targetFactory = targetFactory;
    public void AssertProgramEquipmentColorDiscardChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().AssertProgramEquipmentColorDiscardChoice(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target) => this._targetFactory().CanActivateTargetPhaseLedger(owner, skill, usage, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime))]
internal sealed class CoreSkillProgramsCoreSlashTargetBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreSlashTargetBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> _targetFactory = targetFactory;
    public void AssertSlashTargetBenefitProgram(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertSlashTargetBenefitProgram(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDrawThresholdAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDrawThresholdOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public bool HasExactNativeDrawThresholdSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactNativeDrawThresholdSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HappyQiaoActionQueryUnion(global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction>> query) => this._targetFactory().HappyQiaoActionQueryUnion(query);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreCompletedCardGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCompletedCardGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> _targetFactory = targetFactory;
    public bool IsValidCompletedGiftTargetSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsValidCompletedGiftTargetSelection(frame);
    public void AssertCompletedCardGiftDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertCompletedCardGiftDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramDiscardChallengesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramDiscardChallengesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> _targetFactory = targetFactory;
    public void ValidateProgramDiscardChallengeState(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateProgramDiscardChallengeState(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterChenDaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterChenDaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiChenDaoChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => this._targetFactory().IsMobileShiChenDaoChoiceResult(f, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreSkillProgramsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreSkillProgramsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime))]
internal sealed class CoreSkillProgramsCoreAppliedDamageBenefitObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreAppliedDamageBenefitObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime> _targetFactory = targetFactory;
    public void AssertAppliedDamageBenefitReceipt(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertAppliedDamageBenefitReceipt(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime))]
internal sealed class CoreSkillProgramsCharacterZhaoYunOlGod2016Adapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhaoYunOlGod2016Operations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime> _targetFactory = targetFactory;
    public void IssueHpScaledProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation a) => this._targetFactory().IssueHpScaledProgramSelection(f, a);
    public bool HpScaledProgramSelectionIsExact(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HpScaledProgramSelectionIsExact(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreSkillProgramsCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreSkillProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreSkillProgramsCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public bool TracksNativeLogicalDiscards { get => this._targetFactory().TracksNativeLogicalDiscards; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreSkillProgramsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertCurrentSlashFirePolicy(use);
    public void AssertCurrentSlashFireDraft(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertCurrentSlashFireDraft(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime))]
internal sealed class CoreSkillProgramsCharacterWangCanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterWangCanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _targetFactory = targetFactory;
    public bool IsDenglouNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsDenglouNativeUse(use);
    public void AssertDenglouNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertDenglouNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime))]
internal sealed class CoreSkillProgramsCharacterLeiBoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLeiBoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeiBoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryLeiBoFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => this._targetFactory().IsAnniversaryLeiBoFactionChoiceResult(f, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDamageCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDamageCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CompleteDamageAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime))]
internal sealed class CoreSkillProgramsCoreEquipmentPairPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreEquipmentPairPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsValidEquipmentPairProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, bool awaitingSelection) => this._targetFactory().IsValidEquipmentPairProgramSelection(frame, awaitingSelection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreParticipantHandProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreParticipantHandProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> _targetFactory = targetFactory;
    public void ValidateParticipantHandDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().ValidateParticipantHandDrafts(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime))]
internal sealed class CoreSkillProgramsCharacterHuaTuoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHuaTuoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasExactHappyHuaTuoParticipantSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasExactHappyHuaTuoParticipantSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime))]
internal sealed class CoreSkillProgramsCoreDistinctFactionDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDistinctFactionDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> _targetFactory = targetFactory;
    public void AssertDistinctFactionDiscardDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertDistinctFactionDiscardDraft(f, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime))]
internal sealed class CoreSkillProgramsCoreProgramTargetCommitAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramTargetCommitOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> _targetFactory = targetFactory;
    public void AssertProgramTargetDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramTargetDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreStrategicProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreStrategicProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _targetFactory = targetFactory;
    public void AssertStrategicProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertStrategicProgramSelection(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime))]
internal sealed class CoreSkillProgramsCorePopulationTopReorderProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePopulationTopReorderProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> _targetFactory = targetFactory;
    public void ValidatePopulationTopReorder(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidatePopulationTopReorder(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreSkillProgramsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public void RecordBianFuRenOwnedTransferProducer(global::CardGame.Core.CardMovementRecord movement, int recipientSeat, long producerId) => this._targetFactory().RecordBianFuRenOwnedTransferProducer(movement, recipientSeat, producerId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime))]
internal sealed class CoreSkillProgramsCoreActualPhaseEndsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreActualPhaseEndsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _targetFactory = targetFactory;
    public bool HasActualPhaseEndBoundaryFrame() => this._targetFactory().HasActualPhaseEndBoundaryFrame();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed class CoreSkillProgramsCoreRoundPricedPileAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRoundPricedPileAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _targetFactory = targetFactory;
    public void AssertRoundPileAlcoholUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertRoundPileAlcoholUse(use);
    public void AssertRoundPileAlcohol(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertRoundPileAlcohol(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterChengPuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterChengPuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiChengPuChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsMobileShiChengPuChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreSkillProgramsCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramMarkerPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramMarkerPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> _targetFactory = targetFactory;
    public void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId) => this._targetFactory().PayProgramMarkerCost(owner, cost, skillId, bindingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreFixedSlashAndDeclaredDeckProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFixedSlashAndDeclaredDeckProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime> _targetFactory = targetFactory;
    public void AssertFixedSlashAndDeclaredDeckDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertFixedSlashAndDeclaredDeckDrafts(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime))]
internal sealed class CoreSkillProgramsCoreRecipientContestReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRecipientContestReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> _targetFactory = targetFactory;
    public void AssertPindianWinnerUses() => this._targetFactory().AssertPindianWinnerUses();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime))]
internal sealed class CoreSkillProgramsCharacterDianWeiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterDianWeiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasExactHappyDianWeiElaiSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasExactHappyDianWeiElaiSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreSkillProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreSkillProgramsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime))]
internal sealed class CoreSkillProgramsCoreProgramAttackRangeAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramAttackRangeAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> _targetFactory = targetFactory;
    public void AssertProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramAttackRangeAid(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreSkillProgramsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public bool HasPaidLeZhouFeiXianshuSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasPaidLeZhouFeiXianshuSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDrawThresholdDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDrawThresholdDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> _targetFactory = targetFactory;
    public bool TryDescribeNativeDrawThresholdDyingDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? source, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSource, out global::CardGame.Core.NativeDrawThresholdDyingOrigin? origin) => this._targetFactory().TryDescribeNativeDrawThresholdDyingDamage(frame, target, amount, source, nature, sourceLess, explicitSource, out origin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed class CoreSkillProgramsCharacterLiuYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLiuYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _targetFactory = targetFactory;
    public bool HasPaidLiuYanInitialSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidLiuYanInitialSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreExactTopReorderProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreExactTopReorderProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> _targetFactory = targetFactory;
    public void ValidateExactTopReorder(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateExactTopReorder(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreDeferredBenefitsProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDeferredBenefitsProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> _targetFactory = targetFactory;
    public void ValidateFactionRecoveryDraft(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateFactionRecoveryDraft(frame);
    public void ValidateWeaponDamageDraft(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateWeaponDamageDraft(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreSkillProgramsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime))]
internal sealed class CoreSkillProgramsCoreHpLossSlashMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHpLossSlashMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HpLossSlashSelection? FreezeHpLossSlashSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().FreezeHpLossSlashSelection(owner, plan, targets);
    public global::CardGame.Core.SkillProgramStepOutcome BeginHpLossSelectedSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().BeginHpLossSelectedSlash(f, selection);
    public bool HasExactHpLossSlashSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().HasExactHpLossSlashSelection(f, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreSkillProgramsCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public void AssertCurrentCardEnhancementDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertCurrentCardEnhancementDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime))]
internal sealed class CoreSkillProgramsCoreTurnDefaultStatsValidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTurnDefaultStatsValidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> _targetFactory = targetFactory;
    public void AssertTurnDefaultStatState() => this._targetFactory().AssertTurnDefaultStatState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime))]
internal sealed class CoreSkillProgramsCoreLiejieSourceDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreLiejieSourceDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> _targetFactory = targetFactory;
    public void AssertLiejieSourceDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertLiejieSourceDiscard(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime))]
internal sealed class CoreSkillProgramsCorePublicPersistentPilesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePublicPersistentPilesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> _targetFactory = targetFactory;
    public void AssertPublicPileDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertPublicPileDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramPrivateDamageOfferOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramPrivateDamageOfferOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> _targetFactory = targetFactory;
    public void AssertPrivateOfferDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertPrivateOfferDrafts(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreSkillProgramsCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsProgramWaitingForSkillInvocation(global::CardGame.Core.ProgramSkillFrame program) => this._targetFactory().IsProgramWaitingForSkillInvocation(program);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime))]
internal sealed class CoreSkillProgramsCoreOrderedPrintedSkillLossObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOrderedPrintedSkillLossObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> _targetFactory = targetFactory;
    public void AssertOrderedPrintedSkillLossState() => this._targetFactory().AssertOrderedPrintedSkillLossState();
    public void AssertOrderedPrintedSkillLossObservers() => this._targetFactory().AssertOrderedPrintedSkillLossObservers();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterZhangNiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterZhangNiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryWurongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => this._targetFactory().IsBoundaryWurongChoiceResult(f, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime))]
internal sealed class CoreSkillProgramsCorePindianAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> _targetFactory = targetFactory;
    public void BeginSharedPindian(long parentId, global::CardGame.Core.SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility programResultVisibility = (global::CardGame.Core.SkillProgramCardSetVisibility)1, global::CardGame.Core.PindianOwnedPileSource? ownedPileSource = default) => this._targetFactory().BeginSharedPindian(parentId, presentation, sourceSeat, opponentSeat, sourceCardId, programResultBind, programResultVisibility, ownedPileSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionValidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramSelectionValidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionValidationRegistry ProgramSelectionValidators { get => this._targetFactory().ProgramSelectionValidators; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime))]
internal sealed class CoreSkillProgramsCoreDeferredCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDeferredCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> _targetFactory = targetFactory;
    public void AssertParticipantTopView(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertParticipantTopView(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime))]
internal sealed class CoreSkillProgramsCoreHalfHandPhaseDebtReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHalfHandPhaseDebtReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPhaseHandDebtPayment(f);
    public void AssertHalfHandPhaseDebt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertHalfHandPhaseDebt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreSkillProgramsCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public void AssertHistoricalEndingUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertHistoricalEndingUse(use);
    public bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByDyingSuits(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreSkillProgramsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool IsRoundOnceCardlessBasicUse(long id) => this._targetFactory().IsRoundOnceCardlessBasicUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime))]
internal sealed class CoreSkillProgramsCoreProgramVirtualSlashOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramVirtualSlashOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> _targetFactory = targetFactory;
    public void AssertProgramVirtualSlashOffer(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().AssertProgramVirtualSlashOffer(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreSkillProgramsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
    public bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default) => this._targetFactory().HasTurnCardTargetRestriction(actorSeat, restriction, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreSkillProgramsCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public bool HasPaidZixiPlacement(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidZixiPlacement(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime))]
internal sealed class CoreSkillProgramsCoreKuangfuReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreKuangfuReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> _targetFactory = targetFactory;
    public bool HasExactKuangfuProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().HasExactKuangfuProgramSelection(f, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterChenQunBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterChenQunBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasPaidBoundaryChenQunInitialSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasPaidBoundaryChenQunInitialSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime))]
internal sealed class CoreSkillProgramsCharacterLuYuShengMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLuYuShengMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiLuYuShengChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsMobileShiLuYuShengChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterBuLianShiMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterBuLianShiMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime> _targetFactory = targetFactory;
    public bool IsMobileBuLianShiAnxuEqualityResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsMobileBuLianShiAnxuEqualityResult(f, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed class CoreSkillProgramsCoreRecipientCategoryMarkObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRecipientCategoryMarkObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _targetFactory = targetFactory;
    public void AssertRecipientCategoryMarkState() => this._targetFactory().AssertRecipientCategoryMarkState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat) => this._targetFactory().BuildDyingResponderSeats(victimSeat);
    public void ExposeHumanDyingPrompt() => this._targetFactory().ExposeHumanDyingPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterHuaXiongMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterHuaXiongMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouHuaXiongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsMouHuaXiongChoiceResult(f, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentAttack(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime))]
internal sealed class CoreSkillProgramsCoreDyingOwnedCardSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDyingOwnedCardSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> _targetFactory = targetFactory;
    public bool IsValidDyingOwnedCardProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, bool awaitingSelection) => this._targetFactory().IsValidDyingOwnedCardProgramSelection(frame, awaitingSelection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime))]
internal sealed class CoreSkillProgramsCoreNamedCardAcquisitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNamedCardAcquisitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> _targetFactory = targetFactory;
    public void AssertNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertNamedCardAcquisition(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreSkillProgramsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public bool IsExactDeferredChild(global::CardGame.Core.ProgramSkillFrame child) => this._targetFactory().IsExactDeferredChild(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreSkillProgramsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool WuLuKangActivationTargetHasPindianSource(global::CardGame.Core.ProgramExecutionPlan plan, global::CardGame.Core.CharacterState target) => this._targetFactory().WuLuKangActivationTargetHasPindianSource(plan, target);
    public bool WuLuKangCanOmitPindianSource(int owner, global::CardGame.Core.SkillProgramActivation activation) => this._targetFactory().WuLuKangCanOmitPindianSource(owner, activation);
    public global::CardGame.Core.ProgramSkillFrame CaptureWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation activation) => this._targetFactory().CaptureWuLuKangPindianSelection(f, activation);
    public void IssueWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IssueWuLuKangPindianSelection(f);
    public bool HasWuLuKangPindianSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation a) => this._targetFactory().HasWuLuKangPindianSelection(f, a);
    public bool HasWuLuKangPindianInput(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasWuLuKangPindianInput(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime))]
internal sealed class CoreSkillProgramsCharacterKongShuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterKongShuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _targetFactory = targetFactory;
    public void AssertLeiluanPayments(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertLeiluanPayments(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreSkillProgramsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CoreSkillProgramsCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreSkillProgramsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public void AssertResponseEntityExchange(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertResponseEntityExchange(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreSkillProgramsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsXieweiSelection(selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterLingTongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLingTongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool ValidBoundaryLingTongEquipmentSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidBoundaryLingTongEquipmentSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed class CoreSkillProgramsCoreFriendTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFriendTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _targetFactory = targetFactory;
    public bool FriendXuanjianIgnoresDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().FriendXuanjianIgnoresDistance(owner, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreParticipantAndReserveProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreParticipantAndReserveProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> _targetFactory = targetFactory;
    public void AssertParticipantReserveDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertParticipantReserveDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime))]
internal sealed class CoreSkillProgramsCharacterLuMengHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterLuMengHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasExactHappyGongxinParticipantSelection(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasExactHappyGongxinParticipantSelection(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreSkillProgramsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime))]
internal sealed class CoreSkillProgramsCoreGuanduGeneralNativeCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreGuanduGeneralNativeCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> _targetFactory = targetFactory;
    public bool IsGuanduChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsGuanduChoiceResult(f, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime))]
internal sealed class CoreSkillProgramsCoreFinalTargetGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFinalTargetGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> _targetFactory = targetFactory;
    public void AssertFinalTargetGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().AssertFinalTargetGift(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime))]
internal sealed class CoreSkillProgramsCoreRecoveryDamageEdgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRecoveryDamageEdgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> _targetFactory = targetFactory;
    public bool TryDescribeRecoveryDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSourceSeat, out global::CardGame.Core.RecoveryDamageOrigin? origin, out long? damageWindowId) => this._targetFactory().TryDescribeRecoveryDamage(frame, target, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out origin, out damageWindowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CoreSkillProgramsCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramHoldCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramHoldCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> _targetFactory = targetFactory;
    public void AssertProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramHoldCardSelection(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime))]
internal sealed class CoreSkillProgramsCoreOriginalHandEntityObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOriginalHandEntityObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> _targetFactory = targetFactory;
    public void AssertOriginalHandEntityState() => this._targetFactory().AssertOriginalHandEntityState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed class CoreSkillProgramsCoreVirtualBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreVirtualBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _targetFactory = targetFactory;
    public void AssertVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().AssertVirtualBasicDraft(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime))]
internal sealed class CoreSkillProgramsCoreRecipientCategoryMarkContinuationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRecipientCategoryMarkContinuationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByRecipientCategoryMark(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime))]
internal sealed class CoreSkillProgramsCoreRoundDistinctBasicUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRoundDistinctBasicUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> _targetFactory = targetFactory;
    public void AssertRoundDistinctBasicUses(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertRoundDistinctBasicUses(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime))]
internal sealed class CoreSkillProgramsCoreProgramHandControlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramHandControlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime> _targetFactory = targetFactory;
    public void AssertProgramHandControlDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramHandControlDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed class CoreSkillProgramsCoreGuanHaiBridgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreGuanHaiBridgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardSuit(owner, selection);
    public bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardColor(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime))]
internal sealed class CoreSkillProgramsCharacterWangYunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterWangYunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> _targetFactory = targetFactory;
    public bool ValidWangYunNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ValidWangYunNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreSkillProgramsCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public void AssertCaoChunNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertCaoChunNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreHellKingProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreHellKingProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> _targetFactory = targetFactory;
    public bool IsHellKingChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsHellKingChoiceResult(frame, binding);
    public bool HasExactHellKingParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactHellKingParticipantSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed class CoreSkillProgramsCoreOwnedExactCountAndNamedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOwnedExactCountAndNamedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _targetFactory = targetFactory;
    public void AssertProgramNamedBoundTargetSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramNamedBoundTargetSelection(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreSkillProgramsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreShenwuNpcProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreShenwuNpcProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> _targetFactory = targetFactory;
    public bool IsShenwuChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsShenwuChoiceResult(frame, binding);
    public bool HasExactShenwuBoundParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().HasExactShenwuBoundParticipantSelection(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime))]
internal sealed class CoreSkillProgramsCharacterSunLuBanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterSunLuBanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundarySunLuBanChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => this._targetFactory().IsBoundarySunLuBanChoiceResult(frame, binding);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime))]
internal sealed class CoreSkillProgramsCoreProgramOwnedCardDistributionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramOwnedCardDistributionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> _targetFactory = targetFactory;
    public void AssertProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertProgramOwnedCardDistribution(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime))]
internal sealed class CoreSkillProgramsCoreProgramDiscardTopPlacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramDiscardTopPlacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> _targetFactory = targetFactory;
    public void AssertProgramDiscardTopPlacement(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertProgramDiscardTopPlacement(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreTeamActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTeamActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool IsExactPublicTeamTargetSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect selection, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().IsExactPublicTeamTargetSelection(frame, selection, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreDiamondDelayedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreDiamondDelayedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> _targetFactory = targetFactory;
    public void AssertDiamondDelayed(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paused) => this._targetFactory().AssertDiamondDelayed(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed class CoreSkillProgramsCoreRoundOnceCardlessProgramRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreRoundOnceCardlessProgramRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _targetFactory = targetFactory;
    public void AssertRoundOnceCardlessProgramRequests() => this._targetFactory().AssertRoundOnceCardlessProgramRequests();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime))]
internal sealed class CoreSkillProgramsCoreOriginalTargetAdditionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreOriginalTargetAdditionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _targetFactory = targetFactory;
    public void AssertOriginalTargetAdditionDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().AssertOriginalTargetAdditionDraft(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime))]
internal sealed class CoreSkillProgramsCoreProgramDamageInsertionRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramDamageInsertionRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageInsertionRuntime DamageInsertions { get => this._targetFactory().DamageInsertions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed class CoreSkillProgramsCoreProgramCategoryTargetLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramCategoryTargetLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _targetFactory = targetFactory;
    public bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, cardIds, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime))]
internal sealed class CoreSkillProgramsCoreFinalTargetTurnCountsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreFinalTargetTurnCountsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> _targetFactory = targetFactory;
    public void AssertNamedTurnFlow(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().AssertNamedTurnFlow(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime))]
internal sealed class CoreSkillProgramsCoreGhostActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreGhostActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool IsValidRedJudgmentFireSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, bool awaiting) => this._targetFactory().IsValidRedJudgmentFireSelection(frame, effect, awaiting);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreSkillProgramsCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public bool IsGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsGuoSiNativeUse(use);
    public void AssertGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertGuoSiNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed class CoreSkillProgramsCoreProgramPersistentStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramPersistentStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _targetFactory = targetFactory;
    public bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId) => this._targetFactory().GetProgramBooleanState(ownerSeat, skillId, skillInstanceId, stateId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreSkillProgramsCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public void AssertCappedConversionBenefits() => this._targetFactory().AssertCappedConversionBenefits();
    public bool IsCappedConversionAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMovementContinuation pending) => this._targetFactory().IsCappedConversionAwaitedMovement(frame, pending);
}
