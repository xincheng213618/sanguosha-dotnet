#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveryObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveryObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve) => new(resolve);
}
