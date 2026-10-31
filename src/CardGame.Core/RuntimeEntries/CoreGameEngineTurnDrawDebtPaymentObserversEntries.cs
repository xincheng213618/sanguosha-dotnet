#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtPaymentObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> _resolve;
    private CoreGameEngineTurnDrawDebtPaymentObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDrawDebtPaymentObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> resolve) => new(resolve);
}
