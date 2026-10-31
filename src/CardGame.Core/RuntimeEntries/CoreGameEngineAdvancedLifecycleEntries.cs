#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed partial class CoreGameEngineAdvancedLifecycleEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> resolve) => new(resolve);
}
