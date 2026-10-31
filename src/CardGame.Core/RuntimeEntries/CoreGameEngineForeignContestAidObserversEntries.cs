#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime))]
internal sealed partial class CoreGameEngineForeignContestAidObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> _resolve;
    private CoreGameEngineForeignContestAidObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignContestAidObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignContestAidObserversRuntime> resolve) => new(resolve);
}
