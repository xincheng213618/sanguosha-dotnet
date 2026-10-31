#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenheActivitiesRuntime))]
internal sealed partial class CoreGameEngineWenheActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenheActivitiesRuntime> _resolve;
    private CoreGameEngineWenheActivitiesEntries(Func<EngineOwners.CoreGameEngineWenheActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheActivitiesEntries Create(Func<EngineOwners.CoreGameEngineWenheActivitiesRuntime> resolve) => new(resolve);
}
