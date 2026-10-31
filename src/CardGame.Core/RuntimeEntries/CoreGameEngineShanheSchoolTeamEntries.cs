#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTeamEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> _resolve;
    private CoreGameEngineShanheSchoolTeamEntries(Func<EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolTeamEntries Create(Func<EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> resolve) => new(resolve);
}
