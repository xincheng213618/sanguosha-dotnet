#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed partial class CoreGameEngineCardUseLifecycleEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _resolve;
    private CoreGameEngineCardUseLifecycleEntries(Func<EngineOwners.CoreGameEngineCardUseLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseLifecycleEntries Create(Func<EngineOwners.CoreGameEngineCardUseLifecycleRuntime> resolve) => new(resolve);
}
