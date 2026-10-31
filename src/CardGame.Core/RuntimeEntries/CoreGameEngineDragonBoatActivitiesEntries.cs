#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime))]
internal sealed partial class CoreGameEngineDragonBoatActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> _resolve;
    private CoreGameEngineDragonBoatActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve) => new(resolve);
}
