#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBossActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBossActivitiesRuntime> _resolve;
    private CoreGameEngineBossActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBossActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBossActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBossActivitiesRuntime> resolve) => new(resolve);
}
