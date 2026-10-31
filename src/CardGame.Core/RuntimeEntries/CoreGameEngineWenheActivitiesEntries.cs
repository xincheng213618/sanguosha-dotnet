#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheActivitiesRuntime> _resolve;
    private CoreGameEngineWenheActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheActivitiesRuntime> resolve) => new(resolve);
}
