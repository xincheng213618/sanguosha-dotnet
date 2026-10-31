#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed partial class CoreGameEngineHellActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHellActivitiesRuntime> _resolve;
    private CoreGameEngineHellActivitiesEntries(Func<EngineOwners.CoreGameEngineHellActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellActivitiesEntries Create(Func<EngineOwners.CoreGameEngineHellActivitiesRuntime> resolve) => new(resolve);
}
