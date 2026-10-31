#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed partial class CoreGameEngineHandComparisonEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHandComparisonRuntime> _resolve;
    private CoreGameEngineHandComparisonEntries(Func<EngineOwners.CoreGameEngineHandComparisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHandComparisonEntries Create(Func<EngineOwners.CoreGameEngineHandComparisonRuntime> resolve) => new(resolve);
}
