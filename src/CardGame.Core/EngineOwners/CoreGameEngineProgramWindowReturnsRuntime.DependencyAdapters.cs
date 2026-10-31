#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramWindowReturnRuntime))]
internal sealed class CoreProgramWindowReturnsProgramWindowReturnAdapter(Func<ProgramWindowReturnRuntime> targetFactory) : ICoreProgramWindowReturnsProgramWindowReturnOperations
{
    private readonly Func<ProgramWindowReturnRuntime> _targetFactory = targetFactory;
    public ProgramWindowReturnRuntime Get() => this._targetFactory();
}
