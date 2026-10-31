#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRicePileObserversRuntime))]
internal sealed partial class CoreGameEngineRicePileObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRicePileObserversRuntime> _resolve;
    private CoreGameEngineRicePileObserversEntries(Func<EngineOwners.CoreGameEngineRicePileObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileObserversEntries Create(Func<EngineOwners.CoreGameEngineRicePileObserversRuntime> resolve) => new(resolve);
}
