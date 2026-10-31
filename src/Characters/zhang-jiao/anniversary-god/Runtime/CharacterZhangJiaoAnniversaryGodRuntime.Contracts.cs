#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterZhangJiaoAnniversaryGodCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICharacterZhangJiaoAnniversaryGodContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterZhangJiaoAnniversaryGodKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICharacterZhangJiaoAnniversaryGodCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
    global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICharacterZhangJiaoAnniversaryGodCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICharacterZhangJiaoAnniversaryGodCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterZhangJiaoAnniversaryGodCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterZhangJiaoAnniversaryGodRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterZhangJiaoAnniversaryGodResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterZhangJiaoAnniversaryGodCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveAllCards(global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreDamagePreventionProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PreventProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program);
}

internal interface ICharacterZhangJiaoAnniversaryGodCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}
