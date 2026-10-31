#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> _resolve;
    private CoreGameEngineProgramJudgmentOutcomesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramJudgmentOutcomesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> resolve) => new(resolve);
}
