#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed partial class CoreGameEngineJudgmentProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _resolve;
    private CoreGameEngineJudgmentProgramsEntries(Func<EngineOwners.CoreGameEngineJudgmentProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentProgramsEntries Create(Func<EngineOwners.CoreGameEngineJudgmentProgramsRuntime> resolve) => new(resolve);
}
