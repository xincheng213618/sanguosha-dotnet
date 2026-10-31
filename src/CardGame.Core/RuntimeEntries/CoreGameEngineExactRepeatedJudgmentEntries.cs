#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime))]
internal sealed partial class CoreGameEngineExactRepeatedJudgmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> _resolve;
    private CoreGameEngineExactRepeatedJudgmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExactRepeatedJudgmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> resolve) => new(resolve);
}
