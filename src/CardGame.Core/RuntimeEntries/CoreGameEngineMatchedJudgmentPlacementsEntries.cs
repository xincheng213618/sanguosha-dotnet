#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime))]
internal sealed partial class CoreGameEngineMatchedJudgmentPlacementsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> _resolve;
    private CoreGameEngineMatchedJudgmentPlacementsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchedJudgmentPlacementsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime> resolve) => new(resolve);
}
