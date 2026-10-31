#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed partial class CoreGameEngineJudgmentProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _resolve;
    private CoreGameEngineJudgmentProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> resolve) => new(resolve);
}
