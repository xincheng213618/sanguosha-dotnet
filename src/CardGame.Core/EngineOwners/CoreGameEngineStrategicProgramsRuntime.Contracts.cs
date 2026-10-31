#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreStrategicProgramsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreStrategicProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreStrategicProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreStrategicProgramsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreStrategicProgramsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreStrategicProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreStrategicProgramsCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
    global::CardGame.Core.SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(string skillId, string stateId);
}

internal interface ICoreStrategicProgramsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreStrategicProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreStrategicProgramsCoreNamedAcquisitionObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLostHpTargetSelection(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreStrategicProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreStrategicProgramsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreStrategicProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat);
}

internal interface ICoreStrategicProgramsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default);
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICoreStrategicProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreStrategicProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreStrategicProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreStrategicProgramsCharacterCaoRenBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDiscardHandOrUseEquipment(global::CardGame.Core.ProgramSkillFrame frame, int cardId);
    void ResolveMoveFieldEquipment(global::CardGame.Core.ProgramSkillFrame frame, int cardId);
}

internal interface ICoreStrategicProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreStrategicProgramsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreStrategicProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreStrategicProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreStrategicProgramsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId);
}

internal interface ICoreStrategicProgramsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}
