#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTeamActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> _resolve;
    private CoreGameEngineTeamActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTeamActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> resolve) => new(resolve);
}
