#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEventHistoryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _resolve;
    private CoreGameEngineProgramEventHistoryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEventHistoryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> resolve) => new(resolve);
}
