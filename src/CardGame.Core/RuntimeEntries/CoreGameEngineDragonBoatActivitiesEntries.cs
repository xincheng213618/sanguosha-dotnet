#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime))]
internal sealed partial class CoreGameEngineDragonBoatActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> _resolve;
    private CoreGameEngineDragonBoatActivitiesEntries(Func<EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatActivitiesEntries Create(Func<EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve) => new(resolve);
}
