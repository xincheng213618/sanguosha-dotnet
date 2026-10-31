#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramJudgmentClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> _resolve;
    private CoreGameEngineProgramJudgmentClaimsEntries(Func<EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramJudgmentClaimsEntries Create(Func<EngineOwners.CoreGameEngineProgramJudgmentClaimsRuntime> resolve) => new(resolve);
}
