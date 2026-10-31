#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFrozenHandRosterRuntime))]
internal sealed partial class CoreGameEngineFrozenHandRosterEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFrozenHandRosterRuntime> _resolve;
    private CoreGameEngineFrozenHandRosterEntries(Func<EngineOwners.CoreGameEngineFrozenHandRosterRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenHandRosterEntries Create(Func<EngineOwners.CoreGameEngineFrozenHandRosterRuntime> resolve) => new(resolve);
}
