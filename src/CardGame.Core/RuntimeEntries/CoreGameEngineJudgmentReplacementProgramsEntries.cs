#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _resolve;
    private CoreGameEngineJudgmentReplacementProgramsEntries(Func<EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentReplacementProgramsEntries Create(Func<EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> resolve) => new(resolve);
}
