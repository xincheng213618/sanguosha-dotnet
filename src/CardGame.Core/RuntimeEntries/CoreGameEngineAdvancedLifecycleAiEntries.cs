#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> resolve) => new(resolve);
}
