#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupRecoveryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> _resolve;
    private CoreNativeGroupRecoveryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupRecoveryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> resolve) => new(resolve);
}
