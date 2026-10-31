#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed partial class CoreGameEngineHellActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _resolve;
    private CoreGameEngineHellActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> resolve) => new(resolve);
}
