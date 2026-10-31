#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveryPhasesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveryPhasesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> resolve) => new(resolve);
}
