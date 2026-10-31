#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime))]
internal sealed partial class CoreGameEngineUnexpectedAssaultActionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> _resolve;
    private CoreGameEngineUnexpectedAssaultActionsEntries(Func<EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnexpectedAssaultActionsEntries Create(Func<EngineOwners.CoreGameEngineUnexpectedAssaultActionsRuntime> resolve) => new(resolve);
}
