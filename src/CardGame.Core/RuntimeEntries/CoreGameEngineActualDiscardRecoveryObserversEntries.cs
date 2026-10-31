#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveryObserversEntries(Func<EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveryObserversEntries Create(Func<EngineOwners.CoreGameEngineActualDiscardRecoveryObserversRuntime> resolve) => new(resolve);
}
