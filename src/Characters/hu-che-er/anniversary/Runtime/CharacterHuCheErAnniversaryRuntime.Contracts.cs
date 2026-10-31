#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterHuCheErAnniversaryCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterHuCheErAnniversaryCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICharacterHuCheErAnniversaryCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuCheErAnniversaryCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYanPuDyingExitEdge(int index);
}

internal interface ICharacterHuCheErAnniversaryCharacterHuCheErAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuCheErFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool HuCheErObserverEdge(int i);
}

internal interface ICharacterHuCheErAnniversaryCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterHuCheErAnniversaryResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterHuCheErAnniversaryCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterHuCheErAnniversaryCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterHuCheErAnniversaryCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterHuCheErAnniversaryCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterHuCheErAnniversaryCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterHuCheErAnniversaryCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuCheErAnniversaryCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterHuCheErAnniversaryCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateHappyWeiYanGainRequestSettlement(global::CardGame.Core.HappyWeiYanGainRequestSettlement settlement, long producerFrameId, global::System.Collections.Generic.IReadOnlyList<int> intendedIds, global::CardGame.Core.CardLocation requestedTo, long before);
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICharacterHuCheErAnniversaryCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterHuCheErAnniversaryCoreActualHandGainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ActualHandGainTurnOpen(int owner);
    int[] MatchingActualHandGainIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location);
}

internal interface ICharacterHuCheErAnniversaryCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuCheErAnniversaryCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICharacterHuCheErAnniversaryCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterHuCheErAnniversaryCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterHuCheErAnniversaryCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterHuCheErAnniversaryCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterHuCheErAnniversaryCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterHuCheErAnniversaryCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICharacterHuCheErAnniversaryCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterHuCheErAnniversaryKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterHuCheErAnniversaryCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICharacterHuCheErAnniversaryCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterHuCheErAnniversaryCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICharacterHuCheErAnniversaryCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false);
}

internal interface ICharacterHuCheErAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameNameHandObserverEdge(int child);
}

internal interface ICharacterHuCheErAnniversaryCoreOutsidePhaseDrawDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof);
}

internal interface ICharacterHuCheErAnniversaryContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
