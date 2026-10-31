#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorRuntime
{
    private readonly ICoreTypedRecoveryDamageCursorHealthContinuationOperations _healthContinuationOperations;
    internal CoreGameEngineTypedRecoveryDamageCursorRuntime(ICoreTypedRecoveryDamageCursorHealthContinuationOperations healthContinuationOperations)
    {
        this._healthContinuationOperations = healthContinuationOperations;
    }
}
