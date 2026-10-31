#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramEventHistoryRuntime
{
    internal ProgramEventHistoryIndex ProgramHistory => _programHistoryOperations._programHistory ??= new(_coreCommandJournalStorageEventsPendingEventsAccess._events, _coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents);

    internal void AppendPendingProgramEvent(IGameEvent payload) => ProgramHistory.Append(payload);
    internal void ClearPendingProgramEvents() => ProgramHistory.ClearPending();
    internal IReadOnlyList<T> ProgramEventHistory<T>()
        where T : IGameEvent => ProgramHistory.Read<T>();
}
