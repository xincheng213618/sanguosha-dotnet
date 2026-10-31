#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramPhaseBoundaryRuntime))]
internal sealed class CoreProgramTriggerWindowsProgramPhaseBoundaryAdapter(Func<ProgramPhaseBoundaryRuntime> targetFactory) : ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations
{
    private readonly Func<ProgramPhaseBoundaryRuntime> _targetFactory = targetFactory;
    public ProgramPhaseBoundaryRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramLifecycleWindowRuntime))]
internal sealed class CoreProgramTriggerWindowsProgramLifecycleWindowAdapter(Func<ProgramLifecycleWindowRuntime> targetFactory) : ICoreProgramTriggerWindowsProgramLifecycleWindowOperations
{
    private readonly Func<ProgramLifecycleWindowRuntime> _targetFactory = targetFactory;
    public ProgramLifecycleWindowRuntime Get() => this._targetFactory();
}
