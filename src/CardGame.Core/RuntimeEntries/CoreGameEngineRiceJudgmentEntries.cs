#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRiceJudgmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime> _resolve;
    private CoreGameEngineRiceJudgmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRiceJudgmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime> resolve) => new(resolve);
}
