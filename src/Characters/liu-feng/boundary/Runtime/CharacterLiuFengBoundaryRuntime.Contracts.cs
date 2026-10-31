#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterLiuFengBoundaryKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICharacterLiuFengBoundaryCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterLiuFengBoundaryCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default);
}

internal interface ICharacterLiuFengBoundaryCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider);
}

internal interface ICharacterLiuFengBoundaryCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishAssistedPhysicalSlashPrompt(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterLiuFengBoundaryCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterLiuFengBoundaryCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICharacterLiuFengBoundaryCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteBorrowedSwordAfterSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
    void BeginBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false);
    bool CanRequestBorrowedSwordFactionSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterLiuFengBoundaryCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NearestLegalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterLiuFengBoundaryCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
}

internal interface ICharacterLiuFengBoundaryCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterLiuFengBoundaryResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack();
}

internal interface ICharacterLiuFengBoundaryCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICharacterLiuFengBoundaryCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterLiuFengBoundaryCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterLiuFengBoundaryCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRequest(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.CardConversionSource WuJingSource(global::CardGame.Core.ProgramSkillFrame f);
    void PublishWuJing(global::CardGame.Core.ProgramSkillFrame f, bool keepCurrentRequest = false);
}

internal interface ICharacterLiuFengBoundaryCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
    global::CardGame.Core.ProgramSkillWindowContext CreateCardActionProgramContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate);
}

internal interface ICharacterLiuFengBoundaryCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterLiuFengBoundaryCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PrepareAnniversaryMaTieSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actor, int provider, int target, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? conversion);
    void ContinueMaTie(long id);
}

internal interface ICharacterLiuFengBoundaryCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishQinglongCrescentBladeChoice(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, bool includeFactionSlash);
    bool CanRequestQinglongCrescentBladeFactionSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending);
    bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICharacterLiuFengBoundaryCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    void PrepareSayingSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversionSource, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
    bool ResumeSayingSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied);
    bool FinishSayingSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame supplied);
    void PublishSaying(global::CardGame.Core.ProgramSkillFrame supplied, bool keepCurrentRequest = false);
    bool TryGetSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICharacterLiuFengBoundaryCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterLiuFengBoundaryKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICharacterLiuFengBoundaryCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterLiuFengBoundaryCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}

internal interface ICharacterLiuFengBoundaryCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterLiuFengBoundaryCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterLiuFengBoundaryCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterLiuFengBoundaryCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterLiuFengBoundaryCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterLiuFengBoundaryResolutionStackOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterLiuFengBoundaryKernelCoreMainOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICharacterLiuFengBoundaryCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICharacterLiuFengBoundaryCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterLiuFengBoundaryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterLiuFengBoundaryCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICharacterLiuFengBoundaryCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterLiuFengBoundaryCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterLiuFengBoundaryCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICharacterLiuFengBoundaryCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterLiuFengBoundaryCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICharacterLiuFengBoundaryCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryMarkProgramUseCommitted(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterLiuFengBoundaryCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterLiuFengBoundaryCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterLiuFengBoundaryCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterLiuFengBoundaryCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICharacterLiuFengBoundaryCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterLiuFengBoundaryKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICharacterLiuFengBoundaryCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterLiuFengBoundaryCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterLiuFengBoundaryCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterLiuFengBoundaryCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICharacterLiuFengBoundaryCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICharacterLiuFengBoundaryCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterLiuFengBoundaryCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICharacterLiuFengBoundaryCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool ResumeCaoChun(long id);
    void PublishCaoChun(global::CardGame.Core.ProgramSkillFrame f);
    void CompleteCaoChunNativePayment(long id);
}

internal interface ICharacterLiuFengBoundaryCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTargetScopedZixiEffectRound(long id);
}

internal interface ICharacterLiuFengBoundaryCoreOriginalHandEntityObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OriginalHandBindingStarted(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICharacterLiuFengBoundaryCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICharacterLiuFengBoundaryContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterLiuFengBoundaryCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICharacterLiuFengBoundaryCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundOnceCardlessProgramActor(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op, int actor);
    int[] RoundOnceCardlessProgramTargets(global::CardGame.Core.ProgramSkillFrame f, int actor, global::CardGame.Core.SkillProgramEffectOp op);
}

internal interface ICharacterLiuFengBoundaryCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICharacterLiuFengBoundaryCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterLiuFengBoundaryCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update);
}

internal interface ICharacterLiuFengBoundaryCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }
}

internal interface ICharacterLiuFengBoundaryCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterLiuFengBoundaryCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterLiuFengBoundaryCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterLiuFengBoundaryCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    void PrepareChanshuangSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversionSource, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
    bool ResumeChanshuangSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied);
    bool FinishChanshuangSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame f);
    void PublishChanshuang(global::CardGame.Core.ProgramSkillFrame supplied);
    bool TryGetChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterLiuFengBoundaryCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishKeBiNeng(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidKeBiNeng(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterLiuFengBoundaryCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopFinishedCardUse(long frameId);
    void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterLiuFengBoundaryCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CommitProgramChoiceResult(long frameId, string bind, string optionId, int chooserSeat, string label);
    global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string resultBind, bool keepCurrentRequest = false);
    global::CardGame.Core.SkillProgramStepOutcome RequestProgramSlashByNearest(global::CardGame.Core.ProgramSkillFrame frame, int participantSeat, int hpAmount, bool keepCurrentRequest = false);
}

internal interface ICharacterLiuFengBoundaryCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterLiuFengBoundaryCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterLiuFengBoundaryCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICharacterLiuFengBoundaryCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterLiuFengBoundaryCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICharacterLiuFengBoundaryCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SuspendContinuationForQinglongFollowup(long outerResolutionId, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? followupRequest = default);
    bool RestoreContinuationAfterQinglongFollowup(long completedOwnerId);
    void RestoreFactionRequestAfterQinglongChoice(long ownerId);
}

internal interface ICharacterLiuFengBoundaryCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterLiuFengBoundaryCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterLiuFengBoundaryCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterLiuFengBoundaryCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BoundaryLiuFengNativeEdge(int child);
    bool BoundaryLiuFengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool BoundaryLiuFengSuffix(int i);
}

internal interface ICharacterLiuFengBoundaryCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterLiuFengBoundaryCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f);
    void RestoreQigongParentAttack(global::CardGame.Core.ProgramSkillFrame f);
    void PublishLvKuangLvXiang(global::CardGame.Core.ProgramSkillFrame f);
    void CompleteQigongNativePayment(long id);
}

internal interface ICharacterLiuFengBoundaryCoreFixedSlashAndDeclaredDeckProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResumeFixedSlashAndDeclaredDeck(long frameId);
    void PublishFixedSlashAndDeclaredDeckPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FixedTargetSlashChoices(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterLiuFengBoundaryCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}
