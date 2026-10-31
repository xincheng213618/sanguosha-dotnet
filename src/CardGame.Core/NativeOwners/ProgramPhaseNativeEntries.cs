#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseNativeEntries(Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, Func<RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, Func<RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime) : IProgramPhaseNativeEntries
{
    public void EnterPlayPhase(int seat) => getCoreActualTurnsRuntime().EnterPlayPhase(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool TryBeginDrawPhaseProgramWindow(int seat) => getCoreGameEngineProgramLifecycleRuntime().TryBeginDrawPhaseProgramWindow(matchParticipantsState.ReferenceToPlayers[seat], false);
    public void ConsumeDuFuRenDrawPhase(int seat) => getCharacterDuFuRenOlRuntime().ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    public void ConsumeChengGongYingDraw(int seat) => getCharacterChengGongYingOlRuntime().ConsumeChengGongYingDraw(seat);
    public void DrawNormalCards(int seat) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[seat], getCharacterCaoXiOlRuntime().FreezeAndCaptureCaoXiNativeDrawBase(matchParticipantsState.ReferenceToPlayers[seat]), true);
    public void CompleteTurnStartAfterDraw(int seat) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(matchParticipantsState.ReferenceToPlayers[seat], GameEngine.DelayedTurnEffects.None);
}
