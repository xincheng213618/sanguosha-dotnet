#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreProgramContinuationPermissionsCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreProgramContinuationPermissionsCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}
