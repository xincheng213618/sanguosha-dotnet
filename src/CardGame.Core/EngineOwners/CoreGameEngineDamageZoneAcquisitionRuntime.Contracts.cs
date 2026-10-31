#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageZoneAcquisitionCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetAdjacentRedTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool AdjacentRedTargetEligible(global::CardGame.Core.CardUseFrame use, int owner);
    global::CardGame.Core.SkillProgramStepOutcome BeginAdjacentRedTarget(global::CardGame.Core.ProgramSkillFrame supplied);
}

internal interface ICoreDamageZoneAcquisitionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreDamageZoneAcquisitionCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreDamageZoneAcquisitionCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreDamageZoneAcquisitionCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreDamageZoneAcquisitionCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreDamageZoneAcquisitionCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDamageZoneAcquisitionCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreDamageZoneAcquisitionCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreDamageZoneAcquisitionCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreDamageZoneAcquisitionCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamageZoneAcquisitionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageZoneAcquisitionCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreDamageZoneAcquisitionCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramTurnCardTargetRestriction(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int targetSeat);
}

internal interface ICoreDamageZoneAcquisitionKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreDamageZoneAcquisitionContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageZoneAcquisitionCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICoreDamageZoneAcquisitionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreDamageZoneAcquisitionKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface ICoreDamageZoneAcquisitionCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreDamageZoneAcquisitionCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default);
}

internal interface ICoreDamageZoneAcquisitionCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreDamageZoneAcquisitionCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDamageZoneAcquisitionCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreDamageZoneAcquisitionCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDamageZoneAcquisitionCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}
