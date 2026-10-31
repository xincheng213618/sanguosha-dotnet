#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramPhaseBoundaryRuntime))]
internal sealed class CoreTurnEndingOrderProgramPhaseBoundaryAdapter(Func<ProgramPhaseBoundaryRuntime> targetFactory) : ICoreTurnEndingOrderProgramPhaseBoundaryOperations
{
    private readonly Func<ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}
