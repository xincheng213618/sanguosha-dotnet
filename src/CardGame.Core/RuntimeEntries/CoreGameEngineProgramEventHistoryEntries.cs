#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed partial class CoreGameEngineProgramEventHistoryEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _resolve;
    private CoreGameEngineProgramEventHistoryEntries(Func<EngineOwners.CoreGameEngineProgramEventHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEventHistoryEntries Create(Func<EngineOwners.CoreGameEngineProgramEventHistoryRuntime> resolve) => new(resolve);
}
