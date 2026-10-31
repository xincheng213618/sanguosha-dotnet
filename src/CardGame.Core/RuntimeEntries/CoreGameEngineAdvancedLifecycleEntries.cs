#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed partial class CoreGameEngineAdvancedLifecycleEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleEntries(Func<EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleEntries Create(Func<EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> resolve) => new(resolve);
}
