#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedJudgmentDamageChildrenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> _resolve;
    private CoreGameEngineFinalizedJudgmentDamageChildrenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalizedJudgmentDamageChildrenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> resolve) => new(resolve);
}
