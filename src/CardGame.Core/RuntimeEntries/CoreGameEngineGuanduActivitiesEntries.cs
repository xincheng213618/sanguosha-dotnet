#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed partial class CoreGameEngineGuanduActivitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _resolve;
    private CoreGameEngineGuanduActivitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduActivitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> resolve) => new(resolve);
}
