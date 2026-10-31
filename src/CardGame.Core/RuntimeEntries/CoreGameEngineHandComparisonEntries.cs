#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHandComparisonEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _resolve;
    private CoreGameEngineHandComparisonEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHandComparisonEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> resolve) => new(resolve);
}
