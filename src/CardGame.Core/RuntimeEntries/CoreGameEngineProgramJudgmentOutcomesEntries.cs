#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime))]
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> _resolve;
    private CoreGameEngineProgramJudgmentOutcomesEntries(Func<EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramJudgmentOutcomesEntries Create(Func<EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> resolve) => new(resolve);
}
