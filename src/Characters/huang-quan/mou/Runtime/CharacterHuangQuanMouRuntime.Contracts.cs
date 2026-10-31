#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterHuangQuanMouCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICharacterHuangQuanMouCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterHuangQuanMouResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterHuangQuanMouCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICharacterHuangQuanMouCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMoQuanyuExtraRound(long id);
}

internal interface ICharacterHuangQuanMouCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterHuangQuanMouCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICharacterHuangQuanMouCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICharacterHuangQuanMouCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterHuangQuanMouCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanMouCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterHuangQuanMouContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterHuangQuanMouCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterHuangQuanMouKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICharacterHuangQuanMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuangQuanMouCoreSoleIncomingEffectRepeatOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedSoleIncomingEffect(long id);
}

internal interface ICharacterHuangQuanMouCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICharacterHuangQuanMouCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingSequentialTrickTargets(long frameId);
}

internal interface ICharacterHuangQuanMouCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedFeiYiExtraEffect(long id);
    global::CardGame.Core.ExtraCardEffectState? NativeExtraEffectState(global::CardGame.Core.CardUseFrame use);
}

internal interface ICharacterHuangQuanMouBoundaryYiJiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.BoundaryYiJiRuntime Get();
}

internal interface ICharacterHuangQuanMouCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateHappyWeiYanGainRequestSettlement(global::CardGame.Core.HappyWeiYanGainRequestSettlement settlement, long producerFrameId, global::System.Collections.Generic.IReadOnlyList<int> intendedIds, global::CardGame.Core.CardLocation requestedTo, long before);
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICharacterHuangQuanMouCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterHuangQuanMouCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterHuangQuanMouCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICharacterHuangQuanMouCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICharacterHuangQuanMouCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMouBingcaiExtraEffect(long id);
}

internal interface ICharacterHuangQuanMouCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingAdjustedSimpleTargets(long frameId);
}

internal interface ICharacterHuangQuanMouMouHuangQuanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouHuangQuanRuntime Get();
}

internal interface ICharacterHuangQuanMouCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterHuangQuanMouCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICharacterHuangQuanMouKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterHuangQuanMouCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterHuangQuanMouCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICharacterHuangQuanMouCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICharacterHuangQuanMouCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanMouCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashDefense(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterHuangQuanMouCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanMouCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterHuangQuanMouCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterHuangQuanMouCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterHuangQuanMouCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedZixiExtraEffect(long id);
}

internal interface ICharacterHuangQuanMouCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterHuangQuanMouCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use);
}
