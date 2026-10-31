#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualHandGainObserversRuntime))]
internal sealed partial class CoreGameEngineActualHandGainObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualHandGainObserversRuntime> _resolve;
    private CoreGameEngineActualHandGainObserversEntries(Func<EngineOwners.CoreGameEngineActualHandGainObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualHandGainObserversEntries Create(Func<EngineOwners.CoreGameEngineActualHandGainObserversRuntime> resolve) => new(resolve);
}
