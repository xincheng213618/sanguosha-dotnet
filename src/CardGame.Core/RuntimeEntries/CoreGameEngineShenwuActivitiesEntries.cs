#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> _resolve;
    private CoreGameEngineShenwuActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve) => new(resolve);
}
