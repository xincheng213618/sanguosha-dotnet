#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _resolve;
    private CoreGameEngineDelegatedJudgmentChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelegatedJudgmentChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve) => new(resolve);
}
