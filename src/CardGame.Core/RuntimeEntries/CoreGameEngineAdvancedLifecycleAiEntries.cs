#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleAiEntries(Func<EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleAiEntries Create(Func<EngineOwners.CoreGameEngineAdvancedLifecycleAiRuntime> resolve) => new(resolve);
}
