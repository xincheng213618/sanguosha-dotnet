#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _resolve;
    private CoreGameEngineDelegatedJudgmentChoicesEntries(Func<EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelegatedJudgmentChoicesEntries Create(Func<EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> resolve) => new(resolve);
}
