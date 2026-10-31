#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShenwuActivitiesRuntime))]
internal sealed partial class CoreGameEngineShenwuActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShenwuActivitiesRuntime> _resolve;
    private CoreGameEngineShenwuActivitiesEntries(Func<EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuActivitiesEntries Create(Func<EngineOwners.CoreGameEngineShenwuActivitiesRuntime> resolve) => new(resolve);
}
