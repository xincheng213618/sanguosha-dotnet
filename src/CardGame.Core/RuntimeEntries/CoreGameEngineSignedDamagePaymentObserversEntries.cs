#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime))]
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> _resolve;
    private CoreGameEngineSignedDamagePaymentObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSignedDamagePaymentObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentObserversRuntime> resolve) => new(resolve);
}
