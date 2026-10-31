#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime))]
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> _resolve;
    private CoreGameEngineDamageJudgmentSuitPaymentObserversEntries(Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageJudgmentSuitPaymentObserversEntries Create(Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> resolve) => new(resolve);
}
