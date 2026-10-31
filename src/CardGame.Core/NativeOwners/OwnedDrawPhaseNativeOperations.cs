#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedDrawPhaseNativeOperations(Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, Func<RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, Func<RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries> getCoreLeastHandMarkerDrawProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries> getCoreGameEngineShanheSourceChunlaoJudgment3161Runtime) : IOwnedDrawPhaseNativeOperations
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
    public void CompleteTurnStartAfterDraw(CharacterState owner, int delayedEffects) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(owner, (GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true, actualDrawCompletionCompleted: true);
}
