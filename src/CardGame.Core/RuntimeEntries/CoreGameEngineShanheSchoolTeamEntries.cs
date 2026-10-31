#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTeamEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> _resolve;
    private CoreGameEngineShanheSchoolTeamEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolTeamEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTeamRuntime> resolve) => new(resolve);
}
