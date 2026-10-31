#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime))]
internal sealed partial class CoreGameEngineJudgmentOutcomeClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> _resolve;
    private CoreGameEngineJudgmentOutcomeClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentOutcomeClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeClaimsRuntime> resolve) => new(resolve);
}
