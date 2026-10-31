#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed partial class CoreGameEngineForeignContestAidObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _resolve;
    private CoreGameEngineForeignContestAidObserversEntries(Func<EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignContestAidObserversEntries Create(Func<EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> resolve) => new(resolve);
}
