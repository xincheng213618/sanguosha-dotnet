#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramChildWindowRuntime))]
internal sealed class CoreProgramContinuationsProgramChildWindowAdapter(Func<ProgramChildWindowRuntime> targetFactory) : ICoreProgramContinuationsProgramChildWindowOperations
{
    private readonly Func<ProgramChildWindowRuntime> _targetFactory = targetFactory;
    public ProgramChildWindowRuntime Get() => this._targetFactory();
}
