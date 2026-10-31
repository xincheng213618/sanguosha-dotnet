#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedSlashPaymentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> _resolve;
    private CoreGameEngineAssistedSlashPaymentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedSlashPaymentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> resolve) => new(resolve);
}
