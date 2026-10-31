#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed partial class CoreGameEngineCardActionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardActionsRuntime> _resolve;
    private CoreGameEngineCardActionsEntries(Func<EngineOwners.CoreGameEngineCardActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardActionsEntries Create(Func<EngineOwners.CoreGameEngineCardActionsRuntime> resolve) => new(resolve);
}
