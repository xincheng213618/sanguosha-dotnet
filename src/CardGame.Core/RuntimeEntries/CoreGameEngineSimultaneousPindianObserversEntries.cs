#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianObserversEntries(Func<EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianObserversEntries Create(Func<EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> resolve) => new(resolve);
}
