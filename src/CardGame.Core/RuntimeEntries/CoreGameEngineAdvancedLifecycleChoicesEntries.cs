#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve) => new(resolve);
}
