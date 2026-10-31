#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterZhuLingOlCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterZhuLingOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterZhuLingOlCoreCardRecoveryModifiersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramCardRecoveryModifier(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, int amount, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}

internal interface ICharacterZhuLingOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterZhuLingOlCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterZhuLingOlCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICharacterZhuLingOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterZhuLingOlCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICharacterZhuLingOlCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterZhuLingOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICharacterZhuLingOlCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId, bool value);
}

internal interface ICharacterZhuLingOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterZhuLingOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterZhuLingOlCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId);
}

internal interface ICharacterZhuLingOlCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramTurnCardDamageModifier(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, int amount, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration, global::CardGame.Core.SkillProgramDamageModifierSourceScope sourceScope);
    void GrantProgramTurnRuleModifier(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.SkillRuleOperation operation, int amount, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::CardGame.Core.PlayerMarkerKind? amountFromMarker = default, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}

internal interface ICharacterZhuLingOlCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId);
}

internal interface ICharacterZhuLingOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICharacterZhuLingOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterZhuLingOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterZhuLingOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterZhuLingOlCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default);
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICharacterZhuLingOlCoreTurnHandLimitKindExemptionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramTurnHandLimitCardKindExemption(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}

internal interface ICharacterZhuLingOlCoreProgramBooleanStateExpirationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ScheduleProgramBooleanStateExpiration(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> stateIds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}
