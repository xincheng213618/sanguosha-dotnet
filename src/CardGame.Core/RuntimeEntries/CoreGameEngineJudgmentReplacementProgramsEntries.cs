#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _resolve;
    private CoreGameEngineJudgmentReplacementProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentReplacementProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> resolve) => new(resolve);
}
