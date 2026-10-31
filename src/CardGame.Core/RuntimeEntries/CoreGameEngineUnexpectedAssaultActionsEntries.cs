#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime))]
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> _resolve;
    private CoreGameEngineUnexpectedAssaultActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnexpectedAssaultActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve) => new(resolve);
}
