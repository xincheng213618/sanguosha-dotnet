#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HealthContinuationFacts(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IHealthContinuationFacts
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
