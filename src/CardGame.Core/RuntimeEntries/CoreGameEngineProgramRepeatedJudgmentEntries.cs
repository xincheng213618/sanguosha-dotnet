#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime))]
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _resolve;
    private CoreGameEngineProgramRepeatedJudgmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRepeatedJudgmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve) => new(resolve);
}
