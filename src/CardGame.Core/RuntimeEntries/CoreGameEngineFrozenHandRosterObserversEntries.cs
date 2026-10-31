#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime))]
internal sealed partial class CoreGameEngineFrozenHandRosterObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> _resolve;
    private CoreGameEngineFrozenHandRosterObserversEntries(Func<EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenHandRosterObserversEntries Create(Func<EngineOwners.CoreGameEngineFrozenHandRosterObserversRuntime> resolve) => new(resolve);
}
