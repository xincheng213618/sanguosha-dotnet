#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecoveryReplacementCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyQiaoAnyWounded(int seat);
    int HappyQiaoRecoveryHpBefore(global::CardGame.Core.RecoveryReplacementFrame frame);
    bool TryBeginHappyQiaoRecoveryRequestChoice(global::CardGame.Core.RecoveryReplacementFrame frame);
    bool TryBeginHappyQiaoRecoveryChoice(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.PreparedRecoveryApplication application);
    bool HasHappyQiaoHealth(int seat);
    void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after);
    void ReleaseHappyQiaoHealthSelection(long parentFrameId);
}

internal interface ICoreRecoveryReplacementCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecoveryReplacementCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreRecoveryReplacementCoreActualTurnLegacyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishActualTurnLegacyDyingAlcohol(long useId, long programParentId);
}

internal interface ICoreRecoveryReplacementCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRecoveryAmountSupport(int targetSeat);
}

internal interface ICoreRecoveryReplacementCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ObservesPaidCardUseChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreRecoveryReplacementCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDragonBoatCardEffect(long id);
}

internal interface ICoreRecoveryReplacementCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueTurnAfterHengye(global::CardGame.Core.CharacterState current);
}

internal interface ICoreRecoveryReplacementCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRecoveryReplacementPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICoreRecoveryReplacementCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId);
    void ResumeDrawFundedDistinctBasicRecovery(global::CardGame.Core.RecoveryReplacementFrame completed);
    bool IsDrawFundedDistinctBasicRecoveryParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryReplacementFrame child);
}

internal interface ICoreRecoveryReplacementCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePolicyCounterspellPayment(long windowId);
}

internal interface ICoreRecoveryReplacementCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecoveryReplacementCoreBoundDyingAlcoholReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBoundDyingAlcoholPayment(long useId, int recovered, long? recoveryAttemptId = default);
}

internal interface ICoreRecoveryReplacementCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreRecoveryReplacementCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreRecoveryReplacementCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWumiaoFlood(long id);
}

internal interface ICoreRecoveryReplacementCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreRecoveryReplacementCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSkillInvocationHealthChild(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery);
}

internal interface ICoreRecoveryReplacementCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreRecoveryReplacementCoreRecoveryAmountBonusesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOwnRecoveryAmountSupport(int targetSeat);
    global::CardGame.Core.RecoveryAmountBonusReceipt? PrepareOwnRecoveryAmountBonus(global::CardGame.Core.PreparedRecoveryApplication application);
    void AssertRecoveryAmountBonusReceipts();
    int SupportedRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount);
}

internal interface ICoreRecoveryReplacementCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNativeDrawTwo(long id);
}

internal interface ICoreRecoveryReplacementResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecoveryReplacementCoreNativePublicPileEquipmentChildrenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RecoveryPaidCardUseContinuation FreezeNativePublicPileEquipmentChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
    bool HasNativePublicPileEquipmentTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreRecoveryReplacementCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueColorFireAttackPayment(long useId);
}

internal interface ICoreRecoveryReplacementCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
    bool TryMarkProgramUseCommitted(long frameId);
}

internal interface ICoreRecoveryReplacementCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeFrame(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreRecoveryReplacementCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreRecoveryReplacementCoreNativeRecoveryFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount);
}

internal interface ICoreRecoveryReplacementCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
    void PopFinishedCardUse(long frameId);
}

internal interface ICoreRecoveryReplacementCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardSupplyCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child);
    bool IsCardSupplyCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecoveryReplacementCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreRecoveryReplacementCoreResponseCompletionHealthOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardResponseCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child);
    bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreRecoveryReplacementCoreNativeGroupRecoveryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGroupRecoveryTarget();
}

internal interface ICoreRecoveryReplacementCoreHuxinMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueHuxinMirrorPlacement(long id);
}

internal interface ICoreRecoveryReplacementCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRecoveryReplacementCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreRecoveryReplacementKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreRecoveryReplacementCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreRecoveryReplacementCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreRecoveryReplacementCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWenheCardEffect(long id);
}

internal interface ICoreRecoveryReplacementCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreRecoveryReplacementCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishVirtualBasicUse(long id);
}

internal interface ICoreRecoveryReplacementCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRecoveryReplacementCoreProgramRecoveryReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramRecoveryRequest(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryAttempt attempt);
}

internal interface ICoreRecoveryReplacementCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSoleIncomingRangeDraw(long id);
}

internal interface ICoreRecoveryReplacementCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreRecoveryReplacementKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreRecoveryReplacementCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueFriendDuelResponse(long id);
}

internal interface ICoreRecoveryReplacementCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuanduCardEffect(long id);
}

internal interface ICoreRecoveryReplacementCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRecoveryReplacementCharacterSunQuanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId, int sourceSeat, int targetSeat, global::CardGame.Core.RecoveryAttemptCompletion completion, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> ordinary);
    bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.RecoveryReplacementCandidate candidate);
}

internal interface ICoreRecoveryReplacementCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreRecoveryReplacementCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreRecoveryReplacementCoreMouLuMengOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalMouLuMeng { get; }
}

internal interface ICoreRecoveryReplacementCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRecoveryReplacementCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetHengyeGrowth(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRecoveryReplacementCoreFactionRequestCostRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePaidFactionRequestCost(long producerId);
    void AssertPaidFactionRequestCostRecoveries();
}

internal interface ICoreRecoveryReplacementCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreRecoveryReplacementCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreRecoveryReplacementCoreEquipmentRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueEquipmentRecast(long frameId);
}

internal interface ICoreRecoveryReplacementCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreRecoveryReplacementContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecoveryReplacementKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreRecoveryReplacementCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreRecoveryReplacementCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}
