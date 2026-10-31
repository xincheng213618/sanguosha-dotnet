#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineScenarioGodFactionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> _resolve;
    private CoreGameEngineScenarioGodFactionsEntries(Func<EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineScenarioGodFactionsEntries Create(Func<EngineOwners.CoreGameEngineScenarioGodFactionsRuntime> resolve) => new(resolve);
}
