#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed partial class CoreGameEnginePlayCardHistoryEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _resolve;
    private CoreGameEnginePlayCardHistoryEntries(Func<EngineOwners.CoreGameEnginePlayCardHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayCardHistoryEntries Create(Func<EngineOwners.CoreGameEnginePlayCardHistoryRuntime> resolve) => new(resolve);
}
