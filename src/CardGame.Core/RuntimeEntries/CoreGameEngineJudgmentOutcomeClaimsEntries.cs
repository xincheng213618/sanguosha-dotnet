#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime))]
internal sealed partial class CoreGameEngineJudgmentOutcomeClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> _resolve;
    private CoreGameEngineJudgmentOutcomeClaimsEntries(Func<EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentOutcomeClaimsEntries Create(Func<EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> resolve) => new(resolve);
}
