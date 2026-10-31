#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAdvancedLifecycleCoreAdvancedLifecycleChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PromptAdvancedSelection(long frameId);
}

internal interface ICoreAdvancedLifecycleCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreAdvancedLifecycleCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreAdvancedLifecycleSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreAdvancedLifecycleCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreAdvancedLifecycleCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreAdvancedLifecycleCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreAdvancedLifecycleRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICoreAdvancedLifecycleCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreAdvancedLifecycleCharacterGuoJiaMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenGuoJiaAwakeningCondition(int seat, string skill, string? binding = default);
}

internal interface ICoreAdvancedLifecycleCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreAdvancedLifecycleCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICoreAdvancedLifecycleCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> AdvancedOwnedSkillIds(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreAdvancedLifecycleObservedSkillGrantRevisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface ICoreAdvancedLifecycleCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreAdvancedLifecycleCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreAdvancedLifecycleCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreAdvancedLifecycleCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome ScheduleProgramPhase(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.TurnPhase phase, global::CardGame.Core.SkillProgramPhaseContinuation continuation, int? beneficiarySeat = default);
    void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default);
}

internal interface ICoreAdvancedLifecycleCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreAdvancedLifecycleResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreAdvancedLifecycleCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player);
}

internal interface ICoreAdvancedLifecycleCharacterLiuHongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTuxingAfterSlotAbolish(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount);
}

internal interface ICoreAdvancedLifecycleCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreAdvancedLifecycleCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PlaceAdvancedWeapon(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.CardKind kind);
    global::CardGame.Core.SkillProgramStepOutcome BeginWeaponReclaim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreAdvancedLifecycleKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreAdvancedLifecycleCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}
