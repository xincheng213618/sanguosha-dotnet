#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramJudgmentClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> _resolve;
    private CoreGameEngineProgramJudgmentClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramJudgmentClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> resolve) => new(resolve);
}
