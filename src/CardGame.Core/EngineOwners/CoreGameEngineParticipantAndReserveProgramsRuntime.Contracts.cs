#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreParticipantAndReserveProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreParticipantAndReserveProgramsCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome RequestLegalSlashByNearestActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int hpAmount, bool trickEffectReturned = false);
}

internal interface ICoreParticipantAndReserveProgramsCharacterTengFangLanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidLuochongDyingContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreParticipantAndReserveProgramsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreParticipantAndReserveProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreParticipantAndReserveProgramsCorePaidOwnTargetObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidOwnTargetMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requireCost);
}

internal interface ICoreParticipantAndReserveProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreParticipantAndReserveProgramsCoreLostHpOwnedGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame root);
}

internal interface ICoreParticipantAndReserveProgramsCoreEquipmentDonationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidAllEquipmentDonation(global::CardGame.Core.ProgramSkillFrame f);
    bool IsValidActualEndedEquipment(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLingkongInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreParticipantAndReserveProgramsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableOwnedHand(global::CardGame.Core.CharacterState owner);
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreParticipantAndReserveProgramsCharacterHanDangBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreParticipantAndReserveProgramsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureMoLuBuInitialQi(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreParticipantAndReserveProgramsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLeXidiOpening(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w);
}

internal interface ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TargetPenaltyStillMayIssue(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidTargetPenaltyPayment(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame FreezeFuleInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIdentityAppearanceForProgramHpLoss(long frameId, string skillId, int targetSeat, int amount);
}

internal interface ICoreParticipantAndReserveProgramsCoreSlashTargetBenefitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SlashBenefitProgramParentMatches(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureTanbanInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreParticipantAndReserveProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidTargetPenaltyReward(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreParticipantAndReserveProgramsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreParticipantAndReserveProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreParticipantAndReserveProgramsCorePrepDiscardReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPrepDiscard(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByNearest(global::CardGame.Core.ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false);
}

internal interface ICoreParticipantAndReserveProgramsCharacterZhouYuLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureGuYinOpeningOrigins();
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureGuYinInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreParticipantAndReserveProgramsCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidStrategicDamageBatchContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreParticipantAndReserveProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreParticipantAndReserveProgramsCharacterYangWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidZhuihuanContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused);
}

internal interface ICoreParticipantAndReserveProgramsCoreWenheChaosOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactWenheChaos(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreParticipantAndReserveProgramsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
    bool EnsureDrawPile();
}

internal interface ICoreParticipantAndReserveProgramsCorePrepDiscardEndingPromisesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPrepDiscardEndingDraw(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCharacterZouShiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureYunzhengInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId);
}

internal interface ICoreParticipantAndReserveProgramsCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureQiqinInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind);
}

internal interface ICoreParticipantAndReserveProgramsCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureShuangjiaInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
    void BeginProgramSkillDying(long parentFrameId, global::CardGame.Core.CharacterState victim);
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreParticipantAndReserveProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreParticipantAndReserveProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreParticipantAndReserveProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreParticipantAndReserveProgramsCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SlashTargetPenaltyProgramMatches(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreParticipantAndReserveProgramsCoreProgramMarkerPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId);
}

internal interface ICoreParticipantAndReserveProgramsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureJiaoweiInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCharacterZhangFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBoundaryZhangFeiLiyongPenalty(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreParticipantAndReserveProgramsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureJiguInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreParticipantAndReserveProgramsCharacterCaiFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedOrdinaryCaiFuRenXianzhou(global::CardGame.Core.ProgramSkillFrame f);
    bool CanContinueIssuedOrdinaryCaiFuRen(global::CardGame.Core.ProgramSkillFrame f);
}
