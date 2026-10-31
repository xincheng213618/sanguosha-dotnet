#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEquipmentSlotGroupProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreEquipmentSlotGroupProgramsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreProgramEquipmentRecastRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPauseProgramEquipmentRecast(global::CardGame.Core.ProgramSkillFrame paidFrame, global::CardGame.Core.Card card);
}

internal interface ICoreEquipmentSlotGroupProgramsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceProgramSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> lost, global::System.Collections.Generic.IReadOnlyList<string> acquired, global::CardGame.Core.ProgramSkillSourceRemoval removal, bool acquireFirst, bool announceAwakening = false);
    void CommitProgramSkillChangeFact(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.IGameEvent fact);
}

internal interface ICoreEquipmentSlotGroupProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreNativeRecastInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame BeginFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, int? participantCursor = default);
    global::CardGame.Core.ProgramSkillFrame CompleteFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int actualDrawCount);
}

internal interface ICoreEquipmentSlotGroupProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreEquipmentSlotGroupProgramsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetEquipmentSlotCapacity(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot, int capacity);
}

internal interface ICoreEquipmentSlotGroupProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreEquipmentSlotGroupProgramsProgramTargetSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetSelectionRuntime Get();
}
