#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> _resolve;
    private CoreGameEngineUnexpectedAssaultActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnexpectedAssaultActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve) => new(resolve);
}
