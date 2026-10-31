#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRecoveryFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> _resolve;
    private CoreNativeRecoveryFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRecoveryFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> resolve) => new(resolve);
}
