#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreOwnedDrawPhaseRuntimePortsRuntime
{
    Winner IOwnedDrawPhaseFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IOwnedDrawPhaseFacts.SkipDrawPhaseMask => (int)GameEngine.DelayedTurnEffects.SkipDrawPhase;

    bool IOwnedDrawPhaseFacts.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool IOwnedDrawPhaseFacts.TracksNativeDrawThresholds => _coreNativeDrawThresholdOperations.Value.TracksNativeDrawThresholds;

    bool IOwnedDrawPhaseFacts.TracksShanheS5PhaseSkips => _coreShanheS5ShensuOperations.TracksShanheS5PhaseSkips;

    bool IOwnedDrawPhaseFacts.TracksBoundaryZhuHuanPhaseSkips => _characterZhuHuanBoundaryMobileOperations.TracksBoundaryZhuHuanPhaseSkips;

    bool IOwnedDrawPhaseFacts.HasIssuedExtraDrawMarker(int seat) => _coreLeastHandMarkerDrawProgramsOperations.HasIssuedExtraDrawMarker(seat);
    int IOwnedDrawPhaseFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    bool IOwnedDrawPhaseNativeOperations.BeginRecoveryReplacement(long frameId) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.DrawPhaseObligation);
    bool IOwnedDrawPhaseNativeOperations.BeginHpChangedWindow(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.DrawPhaseObligation);
    void IOwnedDrawPhaseNativeOperations.ConsumeDuFuRenDrawPhase(int seat) => _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    void IOwnedDrawPhaseNativeOperations.ConsumeChengGongYingDraw(int seat) => _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(seat);
    int IOwnedDrawPhaseNativeOperations.FreezeNativeDrawBase(CharacterState owner, bool normalDrawReplaced) => _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(owner, normalDrawReplaced);
    ProgramMarkerExtraDrawPolicy? IOwnedDrawPhaseNativeOperations.ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame) => _coreLeastHandMarkerDrawProgramsOperations.ConsumeIssuedExtraDrawMarker(frame);
    void IOwnedDrawPhaseNativeOperations.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IOwnedDrawPhaseNativeOperations.EndTurn() => _coreActualTurnsOperations.EndTurn();
    bool IOwnedDrawPhaseNativeOperations.CompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame) => _coreShanheSourceChunlaoJudgment3161Operations.Value.TryCompleteShanheSourceChunlaoJudgmentDraw(frame);
    void IOwnedDrawPhaseNativeOperations.CompleteTurnStartAfterDraw(CharacterState owner, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(owner, (GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true, actualDrawCompletionCompleted: true);
}
