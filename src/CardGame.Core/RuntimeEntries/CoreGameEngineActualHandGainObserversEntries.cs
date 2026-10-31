#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainObserversRuntime))]
internal sealed partial class CoreGameEngineActualHandGainObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainObserversRuntime> _resolve;
    private CoreGameEngineActualHandGainObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualHandGainObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainObserversRuntime> resolve) => new(resolve);
}
