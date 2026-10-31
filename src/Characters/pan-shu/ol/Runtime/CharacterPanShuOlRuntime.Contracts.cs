#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterPanShuOlCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(string skillId, string stateId);
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICharacterPanShuOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICharacterPanShuOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterPanShuOlCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CompleteFrameNativeLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.ProgramSkillFrame AppendFrameNativeLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterPanShuOlCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default);
}

internal interface ICharacterPanShuOlCoreNativeRecoveryFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount);
}

internal interface ICharacterPanShuOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterPanShuOlCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId);
}

internal interface ICharacterPanShuOlCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterPanShuOlCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterPanShuOlCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICharacterPanShuOlCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICharacterPanShuOlCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterPanShuOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterPanShuOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(global::CardGame.Core.CharacterState owner, int movedCardCount, global::CardGame.Core.CardMovementSourceCount sourceCount, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.CardMovementTiming? timing = default, global::CardGame.Core.CardMovementBatchContext? nativeDrawBatch = default);
}

internal interface ICharacterPanShuOlCoreProgramTopReorderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramTopReorder(global::CardGame.Core.ProgramSkillFrame frame, int maximumCards, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, int? exactTopCount = default, global::CardGame.Core.ProgramPopulationThresholdCount? population = default, string? allBottomStateId = default, bool requireCurrentSeat = true);
}

internal interface ICharacterPanShuOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterPanShuOlCoreProgramMovementSourceCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICharacterPanShuOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterPanShuOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterPanShuOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterPanShuOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}
