#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTieredRoundZeroResponsesCoreProgramTieredRoundConversionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TieredRoundTier(int ownerSeat, string stateId);
    void ConsumeTieredRoundConversion(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreTieredRoundZeroResponsesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreTieredRoundZeroResponsesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreTieredRoundZeroResponsesCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false);
}

internal interface ICoreTieredRoundZeroResponsesCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginEquipmentSlotNamedResponse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.PromptChoice choice, bool dyingUse);
}

internal interface ICoreTieredRoundZeroResponsesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat);
}

internal interface ICoreTieredRoundZeroResponsesCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? FreezeUnrespondableCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds);
    bool TryBeginPolicyCounterspellPayment(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource, global::System.Collections.Generic.IReadOnlyList<int> frozenMaterialIds);
}

internal interface ICoreTieredRoundZeroResponsesCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreTieredRoundZeroResponsesCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }
}

internal interface ICoreTieredRoundZeroResponsesCoreCommittedResponseUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreTieredRoundZeroResponsesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreTieredRoundZeroResponsesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreTieredRoundZeroResponsesCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICoreTieredRoundZeroResponsesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTieredRoundZeroResponsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreTieredRoundZeroResponsesCharacterChengYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouCyMatchingName(int actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreTieredRoundZeroResponsesSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreTieredRoundZeroResponsesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreTieredRoundZeroResponsesKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreTieredRoundZeroResponsesCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreTieredRoundZeroResponsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreTieredRoundZeroResponsesCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated);
}

internal interface ICoreTieredRoundZeroResponsesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreTieredRoundZeroResponsesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreTieredRoundZeroResponsesCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame PrepareNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending, int responder, int cardId);
    void BindNamedAmbushCounterspell(long id, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeDyingFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetDyingFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreTieredRoundZeroResponsesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreTieredRoundZeroResponsesCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreTieredRoundZeroResponsesCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHzPendingInvalidation(int actor);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreTieredRoundZeroResponsesCoreProgramAllHandBonusOperationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUsePeachToRescue(int responderSeat, int victimSeat);
}

internal interface ICoreTieredRoundZeroResponsesCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next);
}

internal interface ICoreTieredRoundZeroResponsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreTieredRoundZeroResponsesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreTieredRoundZeroResponsesCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action);
    bool HasResponseUseCompletionObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreTieredRoundZeroResponsesCoreActivityOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgram ActivityRuntimeProgram(global::CardGame.Core.SkillProgram original);
}

internal interface ICoreTieredRoundZeroResponsesCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreTieredRoundZeroResponsesCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreTieredRoundZeroResponsesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}
