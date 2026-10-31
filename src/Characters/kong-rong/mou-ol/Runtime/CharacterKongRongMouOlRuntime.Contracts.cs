#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterKongRongMouOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterKongRongMouOlCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginChainedStateBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need);
}

internal interface ICharacterKongRongMouOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterKongRongMouOlCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterKongRongMouOlCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterKongRongMouOlCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterKongRongMouOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterKongRongMouOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterKongRongMouOlCorePrepPoisonOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, global::System.Action<int, int, int>? afterPayment = default);
}

internal interface ICharacterKongRongMouOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterKongRongMouOlCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterKongRongMouOlCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICharacterKongRongMouOlCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterKongRongMouOlCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterKongRongMouOlCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICharacterKongRongMouOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterKongRongMouOlCoreDamagePreventionProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PreventProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program);
}

internal interface ICharacterKongRongMouOlCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICharacterKongRongMouOlMouKongRongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouKongRongRuntime Get();
}

internal interface ICharacterKongRongMouOlCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICharacterKongRongMouOlCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEquipmentSlotNamedSkill(string skill);
    bool TryBeginEquipmentSlotNamedPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterKongRongMouOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterKongRongMouOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterKongRongMouOlCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDrawFundedDistinctBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need);
}

internal interface ICharacterKongRongMouOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterKongRongMouOlCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterKongRongMouOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICharacterKongRongMouOlCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider);
}

internal interface ICharacterKongRongMouOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterKongRongMouOlCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}
