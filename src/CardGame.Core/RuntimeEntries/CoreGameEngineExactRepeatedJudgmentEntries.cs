#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime))]
internal sealed partial class CoreGameEngineExactRepeatedJudgmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> _resolve;
    private CoreGameEngineExactRepeatedJudgmentEntries(Func<EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExactRepeatedJudgmentEntries Create(Func<EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> resolve) => new(resolve);
}
