#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime))]
internal sealed partial class CoreGameEngineAdvancedLifecycleChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> _resolve;
    private CoreGameEngineAdvancedLifecycleChoicesEntries(Func<EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedLifecycleChoicesEntries Create(Func<EngineOwners.CoreGameEngineAdvancedLifecycleChoicesRuntime> resolve) => new(resolve);
}
