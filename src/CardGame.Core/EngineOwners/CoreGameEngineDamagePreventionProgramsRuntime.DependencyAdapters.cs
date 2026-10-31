#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamagePreventionProgramsCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreDamagePreventionProgramsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreDamagePreventionProgramsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}
