#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime))]
internal sealed partial class CoreGameEngineShenwuActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> _resolve;
    private CoreGameEngineShenwuActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve) => new(resolve);
}
