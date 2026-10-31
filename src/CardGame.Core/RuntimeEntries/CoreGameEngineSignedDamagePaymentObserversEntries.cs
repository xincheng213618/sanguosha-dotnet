#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime))]
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> _resolve;
    private CoreGameEngineSignedDamagePaymentObserversEntries(Func<EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSignedDamagePaymentObserversEntries Create(Func<EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> resolve) => new(resolve);
}
