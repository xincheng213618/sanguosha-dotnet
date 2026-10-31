#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(HealthContinuationRuntime))]
internal sealed class CoreTypedRecoveryDamageCursorHealthContinuationAdapter(Func<HealthContinuationRuntime> targetFactory) : ICoreTypedRecoveryDamageCursorHealthContinuationOperations
{
    private readonly Func<HealthContinuationRuntime> _targetFactory = targetFactory;
    public HealthContinuationRuntime Get() => this._targetFactory();
}
