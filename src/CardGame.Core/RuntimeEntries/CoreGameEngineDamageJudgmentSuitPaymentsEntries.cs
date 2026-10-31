#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> _resolve;
    private CoreGameEngineDamageJudgmentSuitPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageJudgmentSuitPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> resolve) => new(resolve);
}
