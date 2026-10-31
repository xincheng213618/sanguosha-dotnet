#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed partial class CoreGameEngineCardActionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _resolve;
    private CoreGameEngineCardActionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardActionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> resolve) => new(resolve);
}
