#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed partial class CoreGameEnginePlayCardHistoryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _resolve;
    private CoreGameEnginePlayCardHistoryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayCardHistoryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> resolve) => new(resolve);
}
