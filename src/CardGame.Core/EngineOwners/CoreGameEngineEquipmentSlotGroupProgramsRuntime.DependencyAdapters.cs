#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreProgramEquipmentRecastRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreProgramEquipmentRecastRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> _targetFactory = targetFactory;
    public bool TryPauseProgramEquipmentRecast(global::CardGame.Core.ProgramSkillFrame paidFrame, global::CardGame.Core.Card card) => this._targetFactory().TryPauseProgramEquipmentRecast(paidFrame, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner) => this._targetFactory().CanWuLuKangDiscardForPindian(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreProgramSkillChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreProgramSkillChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _targetFactory = targetFactory;
    public void ReplaceProgramSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> lost, global::System.Collections.Generic.IReadOnlyList<string> acquired, global::CardGame.Core.ProgramSkillSourceRemoval removal, bool acquireFirst, bool announceAwakening = false) => this._targetFactory().ReplaceProgramSkills(frame, lost, acquired, removal, acquireFirst, announceAwakening);
    public void CommitProgramSkillChangeFact(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.IGameEvent fact) => this._targetFactory().CommitProgramSkillChangeFact(frame, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreNativeRecastInvocationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreNativeRecastInvocationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame BeginFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, int? participantCursor = default) => this._targetFactory().BeginFrameNativeRecastInvocation(frame, executorSeat, originalPrimaryIds, participantCursor);
    public global::CardGame.Core.ProgramSkillFrame CompleteFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int actualDrawCount) => this._targetFactory().CompleteFrameNativeRecastInvocation(frame, actualDrawCount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public void SetEquipmentSlotCapacity(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot, int capacity) => this._targetFactory().SetEquipmentSlotCapacity(player, slot, capacity);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramTargetSelectionRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsProgramTargetSelectionAdapter(global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEquipmentSlotGroupProgramsProgramTargetSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetSelectionRuntime Get() => this._targetFactory();
}
