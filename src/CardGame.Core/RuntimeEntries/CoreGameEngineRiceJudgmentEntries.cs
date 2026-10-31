#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRiceJudgmentRuntime))]
internal sealed partial class CoreGameEngineRiceJudgmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRiceJudgmentRuntime> _resolve;
    private CoreGameEngineRiceJudgmentEntries(Func<EngineOwners.CoreGameEngineRiceJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRiceJudgmentEntries Create(Func<EngineOwners.CoreGameEngineRiceJudgmentRuntime> resolve) => new(resolve);
}
