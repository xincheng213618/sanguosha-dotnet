#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveryPhasesEntries(Func<EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveryPhasesEntries Create(Func<EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> resolve) => new(resolve);
}
