#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramPhaseSchedulingRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsProgramPhaseSchedulingAdapter(Func<ProgramPhaseSchedulingRuntime> targetFactory) : ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations
{
    private readonly Func<ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}
