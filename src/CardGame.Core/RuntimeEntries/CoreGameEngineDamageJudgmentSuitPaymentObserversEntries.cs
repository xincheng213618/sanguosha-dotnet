#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> _resolve;
    private CoreGameEngineDamageJudgmentSuitPaymentObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageJudgmentSuitPaymentObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> resolve) => new(resolve);
}
