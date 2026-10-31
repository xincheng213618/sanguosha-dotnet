#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime))]
internal sealed partial class CoreGameEngineMatchedJudgmentPlacementsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> _resolve;
    private CoreGameEngineMatchedJudgmentPlacementsEntries(Func<EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchedJudgmentPlacementsEntries Create(Func<EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> resolve) => new(resolve);
}
