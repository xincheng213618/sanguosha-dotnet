#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramTriggerRuntime))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerAdapter(Func<ProgramTriggerRuntime> targetFactory) : ICoreProgramWindowCandidateQueriesProgramTriggerOperations
{
    private readonly Func<ProgramTriggerRuntime> _targetFactory = targetFactory;
    public ProgramTriggerRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramTriggerFactProjector))]
internal sealed class CoreProgramWindowCandidateQueriesProgramTriggerFactProjectorAdapter(Func<ProgramTriggerFactProjector> targetFactory) : ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations
{
    private readonly Func<ProgramTriggerFactProjector> _targetFactory = targetFactory;
    public ProgramTriggerFactProjector Get() => this._targetFactory();
}
