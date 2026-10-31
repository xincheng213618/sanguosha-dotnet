#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _resolve;
    private CoreGameEngineDelegatedJudgmentChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelegatedJudgmentChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve) => new(resolve);
}
