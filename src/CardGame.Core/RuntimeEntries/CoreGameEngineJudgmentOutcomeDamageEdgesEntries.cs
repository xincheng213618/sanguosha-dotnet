#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime))]
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> _resolve;
    private CoreGameEngineJudgmentOutcomeDamageEdgesEntries(Func<EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentOutcomeDamageEdgesEntries Create(Func<EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> resolve) => new(resolve);
}
