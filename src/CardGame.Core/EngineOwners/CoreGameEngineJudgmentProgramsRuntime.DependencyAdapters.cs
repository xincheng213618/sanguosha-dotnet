#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreJudgmentProgramsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreJudgmentProgramsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}
