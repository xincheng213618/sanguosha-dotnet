#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _resolve;
    private CoreGameEngineProgramRepeatedJudgmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRepeatedJudgmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> resolve) => new(resolve);
}
