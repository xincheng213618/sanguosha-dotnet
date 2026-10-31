#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramRandomSeatAcquisitionRuntime))]
internal sealed class CoreProgramRandomSeatAcquisitionRuntimeProgramRandomSeatAcquisitionAdapter(Func<ProgramRandomSeatAcquisitionRuntime> targetFactory) : ICoreProgramRandomSeatAcquisitionRuntimeProgramRandomSeatAcquisitionOperations
{
    private readonly Func<ProgramRandomSeatAcquisitionRuntime> _targetFactory = targetFactory;
    public ProgramRandomSeatAcquisitionRuntime Get() => this._targetFactory();
}
