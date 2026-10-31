#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime))]
internal sealed partial class CoreGameEngineOldHandDispositionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> _resolve;
    private CoreGameEngineOldHandDispositionObserversEntries(Func<EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOldHandDispositionObserversEntries Create(Func<EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> resolve) => new(resolve);
}
