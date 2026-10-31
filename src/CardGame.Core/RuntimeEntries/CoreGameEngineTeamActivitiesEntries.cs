#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTeamActivitiesRuntime))]
internal sealed partial class CoreGameEngineTeamActivitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTeamActivitiesRuntime> _resolve;
    private CoreGameEngineTeamActivitiesEntries(Func<EngineOwners.CoreGameEngineTeamActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTeamActivitiesEntries Create(Func<EngineOwners.CoreGameEngineTeamActivitiesRuntime> resolve) => new(resolve);
}
