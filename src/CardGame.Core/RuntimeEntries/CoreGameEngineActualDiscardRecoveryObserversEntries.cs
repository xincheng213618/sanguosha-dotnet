#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveryObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveryObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve) => new(resolve);
}
