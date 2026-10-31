#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientContestsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSharedPindian(long parentId, global::CardGame.Core.SkillPromptPresentation presentation, int sourceSeat, int? opponentSeat = default, int? sourceCardId = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility programResultVisibility = (global::CardGame.Core.SkillProgramCardSetVisibility)1, global::CardGame.Core.PindianOwnedPileSource? ownedPileSource = default);
}

internal interface ICoreRecipientContestsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRecipientContestsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreRecipientContestsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecipientContestsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreRecipientContestsCoreUniqueHpPeerProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientContestUnissuedSource(global::CardGame.Core.ProgramSkillFrame f);
    long RecipientContestSequence { get; }

    global::CardGame.Core.CardConversionSource RecipientContestSource(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.SkillProgramStepOutcome BeginUniqueHpPeer(global::CardGame.Core.ProgramSkillFrame supplied);
}

internal interface ICoreRecipientContestsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreRecipientContestsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreRecipientContestsCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRecipientContestsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientContestsCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? original = default);
}

internal interface ICoreRecipientContestsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreRecipientContestsCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind);
}

internal interface ICoreRecipientContestsCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseForeignTurnContestSlash(int actor, int target);
}

internal interface ICoreRecipientContestsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreRecipientContestsCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreRecipientContestsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRecipientContestsCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBePindianTarget(int sourceSeat, int targetSeat);
}

internal interface ICoreRecipientContestsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreRecipientContestsCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreRecipientContestsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreRecipientContestsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkProgramUseCommitted(long frameId);
}

internal interface ICoreRecipientContestsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreRecipientContestsCoreRecipientContestReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecipientContestPrograms(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientContestsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecipientContestsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreRecipientContestsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecipientContestsCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRecipientContestsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreRecipientContestsCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRecipientContestsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRecipientContestsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRecipientContestsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreRecipientContestsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreRecipientContestsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRecipientContestsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreRecipientContestsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }
}
