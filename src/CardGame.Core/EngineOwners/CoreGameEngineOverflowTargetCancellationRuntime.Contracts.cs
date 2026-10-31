#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOverflowTargetCancellationCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreOverflowTargetCancellationCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBeiMiHuTargetMutation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreOverflowTargetCancellationCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOverflowTargetCancellation(global::CardGame.Core.ProgramSkillFrame frame);
    bool ValidOverflowTargetCancellationReceipt(global::CardGame.Core.ProgramSkillFrame frame);
    bool IsOverflowTargetCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OverflowUseTargetsCanceledEvent fact, global::System.Collections.Generic.IReadOnlyList<int> beforeTargets);
}

internal interface ICoreOverflowTargetCancellationCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreOverflowTargetCancellationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreOverflowTargetCancellationCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuaManSouyingCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreOverflowTargetCancellationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOverflowTargetCancellationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreOverflowTargetCancellationCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICoreOverflowTargetCancellationCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreOverflowTargetCancellationCharacterYuanTanYuanShangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNeifaTrickTargetCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreOverflowTargetCancellationCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreOverflowTargetCancellationCoreCompletedCategoryUseProofOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed);
}

internal interface ICoreOverflowTargetCancellationCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreOverflowTargetCancellationCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreOverflowTargetCancellationCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreOverflowTargetCancellationCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreOverflowTargetCancellationCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreOverflowTargetCancellationCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDinghanCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCuiLieCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreOverflowTargetCancellationCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreOverflowTargetCancellationCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreOverflowTargetCancellationCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreOverflowTargetCancellationCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreOverflowTargetCancellationCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreOverflowTargetCancellationCoreRoundGainedTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundGainedTrickTargetCancellation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreOverflowTargetCancellationCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetHandLimit(global::CardGame.Core.CharacterState player);
}

internal interface ICoreOverflowTargetCancellationCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreOverflowTargetCancellationContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreOverflowTargetCancellationCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card EntityAtCurrentLocation(int id);
}

internal interface ICoreOverflowTargetCancellationCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}
