#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOwnedDrawPhaseRuntimePortsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreNativeDrawThresholdOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksNativeDrawThresholds { get; }
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCharacterZhuHuanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksBoundaryZhuHuanPhaseSkips { get; }
}

internal interface ICoreOwnedDrawPhaseRuntimePortsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCharacterChengGongYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeChengGongYingDraw(int actorSeat);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreShanheSourceChunlaoJudgment3161Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteShanheSourceChunlaoJudgmentDraw(global::CardGame.Core.DrawPhaseObligationFrame frame);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EndTurn();
    void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreShanheS5ShensuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksShanheS5PhaseSkips { get; }
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreLeastHandMarkerDrawProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedExtraDrawMarker(int seat);
    global::CardGame.Core.ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(global::CardGame.Core.DrawPhaseObligationFrame frame);
}

internal interface ICoreOwnedDrawPhaseRuntimePortsCoreNativeDrawAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNativeDrawAttemptCapability { get; }
}
