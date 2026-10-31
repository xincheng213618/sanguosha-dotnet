#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime))]
internal sealed partial class CoreGameEngineFrozenHandRosterObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> _resolve;
    private CoreGameEngineFrozenHandRosterObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenHandRosterObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve) => new(resolve);
}
