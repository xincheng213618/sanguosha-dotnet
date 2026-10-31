#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime))]
internal sealed partial class CoreGameEngineAdvancedLifecycleChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve) => new(resolve);
}
