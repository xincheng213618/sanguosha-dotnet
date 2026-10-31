#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRicePileObserversRuntime))]
internal sealed partial class CoreGameEngineRicePileObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileObserversRuntime> _resolve;
    private CoreGameEngineRicePileObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileObserversRuntime> resolve) => new(resolve);
}
