#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDrawOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairBenefitObserversEntries> getCoreGameEnginePairBenefitObserversRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime) : IProgramDrawOperations
{
    public void Cancel(ProgramSkillFrame frame, string reason) => getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(frame, reason);
    public IReadOnlyList<int> DrawCards(int seat, int count, bool log, CardMoveReason reason) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[seat], count, log, reason);
    public void CaptureFixedRecipientDraw(long frameId, int targetSeat, CardMoveReason reason, long sequenceBefore, int actualCount) => getCoreGameEnginePairBenefitObserversRuntime().CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
    public int FreezeNormalDrawBase(int seat) => getCharacterCaoXiOlRuntime().FreezeAndCaptureCaoXiNativeDrawBase(matchParticipantsState.ReferenceToPlayers[seat]);
}
