#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed partial class CoreGameEngineWenheChaosEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenheChaosRuntime> _resolve;
    private CoreGameEngineWenheChaosEntries(Func<EngineOwners.CoreGameEngineWenheChaosRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheChaosEntries Create(Func<EngineOwners.CoreGameEngineWenheChaosRuntime> resolve) => new(resolve);
}
