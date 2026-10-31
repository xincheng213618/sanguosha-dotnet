#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEquipmentRecastsCoreNativeRecastInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = default, int? instructionIndex = default, int? participantCursor = default, global::CardGame.Core.CardMoveReason? reason = default);
    void CompleteNativeRecastInvocation(global::CardGame.Core.NativeRecastInvocationDraft? draft, int actualDrawCount);
}

internal interface ICoreEquipmentRecastsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreEquipmentRecastsCoreHellDizangProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
    bool TryContinueEquipmentRecastNativeDraw(global::CardGame.Core.EquipmentRecastFrame supplied);
}

internal interface ICoreEquipmentRecastsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreEquipmentRecastsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreEquipmentRecastsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreEquipmentRecastsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreEquipmentRecastsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreEquipmentRecastsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreEquipmentRecastsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreEquipmentRecastsCoreModeCardRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanModeHandEquipmentRecast(global::CardGame.Core.Card card);
}

internal interface ICoreEquipmentRecastsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreEquipmentRecastsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreEquipmentRecastsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}
