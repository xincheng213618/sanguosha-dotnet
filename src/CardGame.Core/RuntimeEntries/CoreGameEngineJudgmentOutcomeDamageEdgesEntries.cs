#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> _resolve;
    private CoreGameEngineJudgmentOutcomeDamageEdgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgmentOutcomeDamageEdgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> resolve) => new(resolve);
}
