#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreOwnedDrawPhaseRuntimePortsRuntime
{
    Winner global::CardGame.Core.IOwnedDrawPhaseFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IOwnedDrawPhaseFacts.SkipDrawPhaseMask => (int)global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;

    bool global::CardGame.Core.IOwnedDrawPhaseFacts.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool global::CardGame.Core.IOwnedDrawPhaseFacts.TracksNativeDrawThresholds => _coreNativeDrawThresholdOperations.TracksNativeDrawThresholds;

    bool global::CardGame.Core.IOwnedDrawPhaseFacts.TracksShanheS5PhaseSkips => _coreShanheS5ShensuOperations.TracksShanheS5PhaseSkips;

    bool global::CardGame.Core.IOwnedDrawPhaseFacts.TracksBoundaryZhuHuanPhaseSkips => _characterZhuHuanBoundaryMobileOperations.TracksBoundaryZhuHuanPhaseSkips;

    bool global::CardGame.Core.IOwnedDrawPhaseFacts.HasIssuedExtraDrawMarker(int seat) => _coreLeastHandMarkerDrawProgramsOperations.HasIssuedExtraDrawMarker(seat);
    int global::CardGame.Core.IOwnedDrawPhaseFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    bool global::CardGame.Core.IOwnedDrawPhaseNativeOperations.BeginRecoveryReplacement(long frameId) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.DrawPhaseObligation);
    bool global::CardGame.Core.IOwnedDrawPhaseNativeOperations.BeginHpChangedWindow(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.DrawPhaseObligation);
    void global::CardGame.Core.IOwnedDrawPhaseNativeOperations.ConsumeDuFuRenDrawPhase(int seat) => _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    void global::CardGame.Core.IOwnedDrawPhaseNativeOperations.ConsumeChengGongYingDraw(int seat) => _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(seat);
    int global::CardGame.Core.IOwnedDrawPhaseNativeOperations.FreezeNativeDrawBase(CharacterState owner, bool normalDrawReplaced) => _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(owner, normalDrawReplaced);
    ProgramMarkerExtraDrawPolicy? global::CardGame.Core.IOwnedDrawPhaseNativeOperations.ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame) => _coreLeastHandMarkerDrawProgramsOperations.ConsumeIssuedExtraDrawMarker(frame);
    void global::CardGame.Core.IOwnedDrawPhaseNativeOperations.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IOwnedDrawPhaseNativeOperations.EndTurn() => _coreActualTurnsOperations.EndTurn();
    bool global::CardGame.Core.IOwnedDrawPhaseNativeOperations.CompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame) => _coreShanheSourceChunlaoJudgment3161Operations.TryCompleteShanheSourceChunlaoJudgmentDraw(frame);
    void global::CardGame.Core.IOwnedDrawPhaseNativeOperations.CompleteTurnStartAfterDraw(CharacterState owner, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(owner, (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true, actualDrawCompletionCompleted: true);
}
