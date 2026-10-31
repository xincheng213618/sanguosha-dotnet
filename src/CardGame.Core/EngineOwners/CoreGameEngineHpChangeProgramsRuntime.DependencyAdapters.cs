#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreHpChangeProgramsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreHpChangeProgramsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}
