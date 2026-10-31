#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeHealthRecoveryFacts(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : INativeHealthRecoveryFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long id) => getFrameStore().FindIndex(frame => frame.Id == id);
    public int FrameLastIndex(long id) => getFrameStore().FindLastIndex(frame => frame.Id == id);
    public IEnumerable<IGameEvent> CompleteProgramEventHistory() => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory();
    public int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
}
