#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime))]
internal sealed partial class CoreGameEngineOldHandDispositionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> _resolve;
    private CoreGameEngineOldHandDispositionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOldHandDispositionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOldHandDispositionObserversRuntime> resolve) => new(resolve);
}
