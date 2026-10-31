#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBossActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBossActivitiesRuntime> _resolve;
    private CoreGameEngineBossActivitiesEntries(Func<EngineOwners.CoreGameEngineBossActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBossActivitiesEntries Create(Func<EngineOwners.CoreGameEngineBossActivitiesRuntime> resolve) => new(resolve);
}
