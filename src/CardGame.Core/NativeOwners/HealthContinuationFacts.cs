#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HealthContinuationFacts(Func<FrameStore> getFrameStore, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, Func<RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, Func<RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IHealthContinuationFacts
{
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int FrameIndex(long id) => getFrameStore().FindIndex(frame => frame.Id == id);
    public int PlayerCount => matchParticipantsState.ReferenceToPlayers.Count;

    public bool IsValidPlayerSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public int MovementCount => cardMovementJournalState.ReferenceToCardMovements.Count;

    public CardMovementRecord MovementAt(int index) => cardMovementJournalState.ReferenceToCardMovements[index];
    public int PendingHpChangeCount => gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges.Count;

    public HpChangeContext PendingHpChangeAt(int index) => gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges[index];
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().ActiveDying;
    public DamageTriggerWindowFrame? ActiveDamageTrigger => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger;

    public SkillProgram? Program(string skillId) => getContentRegistry().Skills.TryGetValue(skillId, out var skill) ? skill.Program : null;
    public SkillProgram RequireProgram(string skillId) => getContentRegistry().GetSkill(skillId).Program!;
    public IEnumerable<IGameEvent> CompleteProgramEventHistory() => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory();
    public IReadOnlyList<T> ProgramEventHistory<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
}
