#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEventHistoryEntries
{
    internal void AppendPendingProgramEvent(IGameEvent payload) => _resolve().AppendPendingProgramEvent(payload);
    internal void ClearPendingProgramEvents() => _resolve().ClearPendingProgramEvents();
    internal IReadOnlyList<T> ProgramEventHistory<T>()
        where T : IGameEvent => _resolve().ProgramEventHistory<T>();
}
