#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> _resolve;
    private CoreGameEngineDragonBoatActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatActivitiesRuntime> resolve) => new(resolve);
}
