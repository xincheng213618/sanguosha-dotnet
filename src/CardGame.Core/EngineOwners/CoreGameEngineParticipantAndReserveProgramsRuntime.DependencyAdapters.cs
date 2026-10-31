#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreNearestLegalSlashRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreNearestLegalSlashRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome RequestLegalSlashByNearestActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int hpAmount, bool trickEffectReturned = false) => this._targetFactory().RequestLegalSlashByNearestActor(frame, actorSeat, hpAmount, trickEffectReturned);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterTengFangLanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterTengFangLanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime> _targetFactory = targetFactory;
    public bool IsValidLuochongDyingContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().IsValidLuochongDyingContinuation(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePaidOwnTargetObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePaidOwnTargetObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _targetFactory = targetFactory;
    public bool PaidOwnTargetMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requireCost) => this._targetFactory().PaidOwnTargetMatches(root, parent, requireCost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreLostHpOwnedGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreLostHpOwnedGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _targetFactory = targetFactory;
    public bool IsValidLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame root) => this._targetFactory().IsValidLostHpOwnedGift(root);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreEquipmentDonationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreEquipmentDonationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationObserversRuntime> _targetFactory = targetFactory;
    public bool IsValidAllEquipmentDonation(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsValidAllEquipmentDonation(f);
    public bool IsValidActualEndedEquipment(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsValidActualEndedEquipment(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLingkongInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureLingkongInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableOwnedHand(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetSelfDiscardableOwnedHand(owner);
    public bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterHanDangBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterHanDangBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool ValidProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidProgramAttackRangeAid(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureMoLuBuInitialQi(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f) => this._targetFactory().CaptureMoLuBuInitialQi(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts) => this._targetFactory().CollectEligibleProgramTriggerCandidates(owner, window, facts);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLeXidiOpening(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w) => this._targetFactory().CaptureLeXidiOpening(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreSlashTargetPenaltyPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyPaymentsRuntime> _targetFactory = targetFactory;
    public bool TargetPenaltyStillMayIssue(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().TargetPenaltyStillMayIssue(f);
    public bool ValidTargetPenaltyPayment(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidTargetPenaltyPayment(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame FreezeFuleInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().FreezeFuleInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool TryBeginIdentityAppearanceForProgramHpLoss(long frameId, string skillId, int targetSeat, int amount) => this._targetFactory().TryBeginIdentityAppearanceForProgramHpLoss(frameId, skillId, targetSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreSlashTargetBenefitPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreSlashTargetBenefitPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> _targetFactory = targetFactory;
    public bool SlashBenefitProgramParentMatches(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SlashBenefitProgramParentMatches(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureTanbanInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureTanbanInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreSlashTargetPenaltyObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyObserversRuntime> _targetFactory = targetFactory;
    public bool ValidTargetPenaltyReward(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidTargetPenaltyReward(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePrepDiscardReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePrepDiscardReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardReceiptsRuntime> _targetFactory = targetFactory;
    public bool ValidPrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPrepDiscard(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreProgramSlashRequestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreProgramSlashRequestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByNearest(global::CardGame.Core.ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false) => this._targetFactory().RequestProgramSlashByNearest(frame, participantSeat, hpAmount, keepCurrentRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterZhouYuLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterZhouYuLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> _targetFactory = targetFactory;
    public void CaptureGuYinOpeningOrigins() => this._targetFactory().CaptureGuYinOpeningOrigins();
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureGuYinInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureGuYinInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreStrategicProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreStrategicProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _targetFactory = targetFactory;
    public bool IsValidStrategicDamageBatchContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().IsValidStrategicDamageBatchContinuation(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterYangWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterYangWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> _targetFactory = targetFactory;
    public bool IsValidZhuihuanContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().IsValidZhuihuanContinuation(frame, paused);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreWenheChaosAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreWenheChaosOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> _targetFactory = targetFactory;
    public bool IsExactWenheChaos(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsExactWenheChaos(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
    public bool EnsureDrawPile() => this._targetFactory().EnsureDrawPile();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePrepDiscardEndingPromisesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePrepDiscardEndingPromisesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _targetFactory = targetFactory;
    public bool ValidPrepDiscardEndingDraw(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidPrepDiscardEndingDraw(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterZouShiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterZouShiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureYunzhengInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureYunzhengInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId) => this._targetFactory().RecordBianFuRenForeignDiscardProducer(movement, actorSeat, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterXiaoQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterXiaoQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureQiqinInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureQiqinInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind) => this._targetFactory().RecordHpChange(parentFrameId, sourceSeat, targetSeat, before, after, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureShuangjiaInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureShuangjiaInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
    public void BeginProgramSkillDying(long parentFrameId, global::CardGame.Core.CharacterState victim) => this._targetFactory().BeginProgramSkillDying(parentFrameId, victim);
    public global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default) => this._targetFactory().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreSlashTargetPenaltyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetPenaltyWindowsRuntime> _targetFactory = targetFactory;
    public bool SlashTargetPenaltyProgramMatches(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().SlashTargetPenaltyProgramMatches(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCoreProgramMarkerPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCoreProgramMarkerPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> _targetFactory = targetFactory;
    public void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId) => this._targetFactory().PayProgramMarkerCost(owner, cost, skillId, bindingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureJiaoweiInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureJiaoweiInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterZhangFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterZhangFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool ValidBoundaryZhangFeiLiyongPenalty(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidBoundaryZhangFeiLiyongPenalty(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureJiguInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().CaptureJiguInitialEntities(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime))]
internal sealed class CoreParticipantAndReserveProgramsCharacterCaiFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantAndReserveProgramsCharacterCaiFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenOlRuntime> _targetFactory = targetFactory;
    public bool IsIssuedOrdinaryCaiFuRenXianzhou(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsIssuedOrdinaryCaiFuRenXianzhou(f);
    public bool CanContinueIssuedOrdinaryCaiFuRen(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CanContinueIssuedOrdinaryCaiFuRen(f);
}
