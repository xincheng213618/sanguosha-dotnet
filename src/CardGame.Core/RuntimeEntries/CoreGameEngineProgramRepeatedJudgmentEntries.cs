#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime))]
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _resolve;
    private CoreGameEngineProgramRepeatedJudgmentEntries(Func<EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRepeatedJudgmentEntries Create(Func<EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve) => new(resolve);
}
