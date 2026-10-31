#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreDeathProgramsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreDeathProgramsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}
