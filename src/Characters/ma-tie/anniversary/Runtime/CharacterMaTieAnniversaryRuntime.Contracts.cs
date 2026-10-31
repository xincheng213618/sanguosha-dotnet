#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterMaTieAnniversaryContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterMaTieAnniversaryCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterMaTieAnniversaryCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false);
}

internal interface ICharacterMaTieAnniversaryCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDrawFundedDistinctBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need);
}

internal interface ICharacterMaTieAnniversaryCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICharacterMaTieAnniversaryCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MaTieNativeEdge(int i);
    bool MaTieFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool MaTieSuffix(int root);
}

internal interface ICharacterMaTieAnniversaryCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterMaTieAnniversaryCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterMaTieAnniversaryCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision TousuiProgramNativeDecision(global::CardGame.Core.PendingDecision p);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default);
}

internal interface ICharacterMaTieAnniversaryCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterMaTieAnniversaryCorePrepPoisonOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, global::System.Action<int, int, int>? afterPayment = default);
}

internal interface ICharacterMaTieAnniversaryCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterMaTieAnniversaryCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterMaTieAnniversaryCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterMaTieAnniversaryCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterMaTieAnniversaryCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterMaTieAnniversaryResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterMaTieAnniversaryCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision KenshangProgramNativeDecision(global::CardGame.Core.PendingDecision p);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default);
}

internal interface ICharacterMaTieAnniversaryCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICharacterMaTieAnniversaryCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? BoundaryLiuFengNativeParent(global::CardGame.Core.CardUseFrame use);
    global::CardGame.Core.ProgramSkillFrame? BoundaryLiuFengReturnedProgramParent(long parentId, global::CardGame.Core.CardUseFrame use);
    bool BoundaryLiuFengRequestChild(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterMaTieAnniversaryCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision RoundOnceCardlessProgramNativeDecision(global::CardGame.Core.PendingDecision p);
}

internal interface ICharacterMaTieAnniversaryCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterMaTieAnniversaryCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICharacterMaTieAnniversaryCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AssistedPhysicalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterMaTieAnniversaryResolutionStackOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack();
}

internal interface ICharacterMaTieAnniversaryCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterMaTieAnniversaryCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterMaTieAnniversaryCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasValidRequestedDeckBasicMaterial(global::CardGame.Core.ResolutionFrame owner);
    global::CardGame.Core.ResolutionFrame? RequestedDeckBasicInvariantTop();
    global::CardGame.Core.PendingDecision? RequestedDeckBasicInvariantDecision();
    bool RequestedDeckBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICharacterMaTieAnniversaryCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat);
}

internal interface ICharacterMaTieAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
}

internal interface ICharacterMaTieAnniversaryCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICharacterMaTieAnniversaryCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICharacterMaTieAnniversaryCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICharacterMaTieAnniversaryCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, global::CardGame.Core.SkillProgramTriggerFacts facts);
}

internal interface ICharacterMaTieAnniversaryCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEquipmentSlotNamedSkill(string skill);
    bool TryBeginEquipmentSlotNamedPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterMaTieAnniversaryCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterMaTieAnniversaryCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetFactionProviderSeats(int ownerSeat, string factionId);
}

internal interface ICharacterMaTieAnniversaryCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterMaTieAnniversaryCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterMaTieAnniversaryCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterMaTieAnniversaryCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterMaTieAnniversaryCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterMaTieAnniversaryCoreRecoveryReplacementDamageRidesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterMaTieAnniversaryCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterMaTieAnniversaryCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterMaTieAnniversaryCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICharacterMaTieAnniversaryCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICharacterMaTieAnniversaryCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterMaTieAnniversaryCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterMaTieAnniversaryCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICharacterMaTieAnniversaryCoreAssistedSlashPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> Cards, global::CardGame.Core.CardKind Kind, global::CardGame.Core.CardConversionSource? Conversion) ReadAssistedSlashPayment(global::CardGame.Core.CharacterState actor, global::CardGame.Core.PromptChoice selected);
}

internal interface ICharacterMaTieAnniversaryCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICharacterMaTieAnniversaryCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICharacterMaTieAnniversaryCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterMaTieAnniversaryCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginChainedStateBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need);
}

internal interface ICharacterMaTieAnniversaryCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterMaTieAnniversaryCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICharacterMaTieAnniversaryCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICharacterMaTieAnniversaryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterMaTieAnniversaryCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterMaTieAnniversaryCoreProgramRecoveryReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramRecoveryReplacementReceiptMatches(global::CardGame.Core.ProgramSkillFrame owner, global::CardGame.Core.RecoveryReplacementFrame recovery, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
}

internal interface ICharacterMaTieAnniversaryCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICharacterMaTieAnniversaryKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, global::CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, global::CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, global::CardGame.Core.CardKind requiredKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, global::CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface ICharacterMaTieAnniversaryCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}
