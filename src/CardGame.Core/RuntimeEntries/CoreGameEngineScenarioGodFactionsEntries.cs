#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineScenarioGodFactionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> _resolve;
    private CoreGameEngineScenarioGodFactionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineScenarioGodFactionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> resolve) => new(resolve);
}
