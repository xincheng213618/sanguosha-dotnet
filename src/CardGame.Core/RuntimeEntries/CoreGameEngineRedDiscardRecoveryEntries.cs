#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedDiscardRecoveryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> _resolve;
    private CoreGameEngineRedDiscardRecoveryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRedDiscardRecoveryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> resolve) => new(resolve);
}
