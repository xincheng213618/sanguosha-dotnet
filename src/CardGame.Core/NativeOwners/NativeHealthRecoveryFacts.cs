#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeHealthRecoveryFacts(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : INativeHealthRecoveryFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long id) => getFrameStore().FindIndex(frame => frame.Id == id);
    public int FrameLastIndex(long id) => getFrameStore().FindLastIndex(frame => frame.Id == id);
    public IEnumerable<IGameEvent> CompleteProgramEventHistory() => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory();
    public int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
}
