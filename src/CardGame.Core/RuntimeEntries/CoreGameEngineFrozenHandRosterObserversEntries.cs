#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> _resolve;
    private CoreGameEngineFrozenHandRosterObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenHandRosterObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve) => new(resolve);
}
