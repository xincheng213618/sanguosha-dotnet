#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtPaymentObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> _resolve;
    private CoreGameEngineTurnDrawDebtPaymentObserversEntries(Func<EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDrawDebtPaymentObserversEntries Create(Func<EngineOwners.CoreGameEngineTurnDrawDebtPaymentObserversRuntime> resolve) => new(resolve);
}
