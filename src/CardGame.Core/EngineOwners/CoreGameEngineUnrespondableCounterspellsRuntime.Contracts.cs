#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreUnrespondableCounterspellsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreUnrespondableCounterspellsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreUnrespondableCounterspellsCoreCommittedResponseUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreUnrespondableCounterspellsCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? MouZhaJiangCounterspellSource(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreUnrespondableCounterspellsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreUnrespondableCounterspellsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreUnrespondableCounterspellsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedMouXqResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreUnrespondableCounterspellsCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreUnrespondableCounterspellsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreUnrespondableCounterspellsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreUnrespondableCounterspellsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreUnrespondableCounterspellsCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedTieredRoundZeroResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreUnrespondableCounterspellsCharacterChenDaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? MobileShiWanglieCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreUnrespondableCounterspellsCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void TryFreezeProvenanceCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds, ref global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreUnrespondableCounterspellsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreUnrespondableCounterspellsCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? MobileShiZhuangshiCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds);
}

internal interface ICoreUnrespondableCounterspellsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreUnrespondableCounterspellsCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreUnrespondableCounterspellsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreUnrespondableCounterspellsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreUnrespondableCounterspellsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreUnrespondableCounterspellsCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreUnrespondableCounterspellsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreUnrespondableCounterspellsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreUnrespondableCounterspellsCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue);
}

internal interface ICoreUnrespondableCounterspellsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreUnrespondableCounterspellsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreUnrespondableCounterspellsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreUnrespondableCounterspellsCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreUnrespondableCounterspellsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreUnrespondableCounterspellsCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreUnrespondableCounterspellsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreUnrespondableCounterspellsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreUnrespondableCounterspellsCharacterSimaShiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? MouSimaShiNoResponseSource(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreUnrespondableCounterspellsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
}

internal interface ICoreUnrespondableCounterspellsCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreUnrespondableCounterspellsCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreUnrespondableCounterspellsCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}
