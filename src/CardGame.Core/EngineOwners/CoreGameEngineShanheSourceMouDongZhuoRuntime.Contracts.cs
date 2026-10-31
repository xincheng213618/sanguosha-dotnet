#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSourceMouDongZhuoCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreShanheSourceMouDongZhuoCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreShanheSourceMouDongZhuoCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreShanheSourceMouDongZhuoCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheSourceMouDongZhuoCoreShanheSourceMouDongZhuoWineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeginShanheSourceMouWine(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreShanheSourceMouDongZhuoResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheSourceMouDongZhuoCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreShanheSourceMouDongZhuoCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreShanheSourceMouDongZhuoCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RemoveProgramExplicitSkillSources(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> grantIds);
}

internal interface ICoreShanheSourceMouDongZhuoContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheSourceMouDongZhuoCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreShanheSourceMouDongZhuoKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheSourceMouDongZhuoCoreShanheSourceMouDongZhuoZhibingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.ShanheSourceMouBlackUseEvent Use, int Ordinal)? SourceMouThreshold(int owner, global::CardGame.Core.ProgramSkillWindowContext context);
    void ApplySourceMouThreshold(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreShanheSourceMouDongZhuoCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreShanheSourceMouDongZhuoCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreShanheSourceMouDongZhuoCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void ChangeProgramParticipantMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int amount, string? sourceSkillId = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit GetProgramEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation = default);
}

internal interface ICoreShanheSourceMouDongZhuoCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryApplyHappyQiaoMaximumHp(long parentFrameId, int targetSeat, int newMaximum, string sourceId);
}

internal interface ICoreShanheSourceMouDongZhuoCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreShanheSourceMouDongZhuoCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramLogicalDiscard(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreShanheSourceMouDongZhuoCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangTactic(int seat, int id);
}

internal interface ICoreShanheSourceMouDongZhuoCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreShanheSourceMouDongZhuoCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheSourceMouDongZhuoCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreShanheSourceMouDongZhuoCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreShanheSourceMouDongZhuoCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreShanheSourceMouDongZhuoCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreShanheSourceMouDongZhuoCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? original = default);
}
