#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseNativeEntries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime) : IProgramPhaseNativeEntries
{
    public void EnterPlayPhase(int seat) => getCoreActualTurnsRuntime().EnterPlayPhase(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool TryBeginDrawPhaseProgramWindow(int seat) => getCoreGameEngineProgramLifecycleRuntime().TryBeginDrawPhaseProgramWindow(matchParticipantsState.ReferenceToPlayers[seat], false);
    public void ConsumeDuFuRenDrawPhase(int seat) => getCharacterDuFuRenOlRuntime().ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    public void ConsumeChengGongYingDraw(int seat) => getCharacterChengGongYingOlRuntime().ConsumeChengGongYingDraw(seat);
    public void DrawNormalCards(int seat) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[seat], getCharacterCaoXiOlRuntime().FreezeAndCaptureCaoXiNativeDrawBase(matchParticipantsState.ReferenceToPlayers[seat]), true);
    public void CompleteTurnStartAfterDraw(int seat) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(matchParticipantsState.ReferenceToPlayers[seat], global::CardGame.Core.GameEngine.DelayedTurnEffects.None);
}
