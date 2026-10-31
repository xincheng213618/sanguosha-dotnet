#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedDrawPhaseNativeOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries> getCoreLeastHandMarkerDrawProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries> getCoreGameEngineShanheSourceChunlaoJudgment3161Runtime) : IOwnedDrawPhaseNativeOperations
{
    public bool BeginRecoveryReplacement(long frameId) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.DrawPhaseObligation);
    public bool BeginHpChangedWindow(long frameId) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.DrawPhaseObligation);
    public void ConsumeDuFuRenDrawPhase(int seat) => getCharacterDuFuRenOlRuntime().ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    public void ConsumeChengGongYingDraw(int seat) => getCharacterChengGongYingOlRuntime().ConsumeChengGongYingDraw(seat);
    public int FreezeNativeDrawBase(CharacterState owner, bool normalDrawReplaced) => getCharacterCaoXiOlRuntime().FreezeAndCaptureCaoXiNativeDrawBase(owner, normalDrawReplaced);
    public ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame) => getCoreLeastHandMarkerDrawProgramsRuntime().ConsumeIssuedExtraDrawMarker(frame);
    public void AdvanceRulesAndPublishState() => advanceRulesAndPublishState();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public bool CompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame) => getCoreGameEngineShanheSourceChunlaoJudgment3161Runtime().TryCompleteShanheSourceChunlaoJudgmentDraw(frame);
    public void CompleteTurnStartAfterDraw(CharacterState owner, int delayedEffects) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(owner, (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true, actualDrawCompletionCompleted: true);
}
