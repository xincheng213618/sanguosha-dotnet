#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEventHistoryEntries
{
    internal void AppendPendingProgramEvent(global::CardGame.Core.IGameEvent payload) => _resolve().AppendPendingProgramEvent(payload);
    internal void ClearPendingProgramEvents() => _resolve().ClearPendingProgramEvents();
    internal global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => _resolve().ProgramEventHistory<T>();
}
