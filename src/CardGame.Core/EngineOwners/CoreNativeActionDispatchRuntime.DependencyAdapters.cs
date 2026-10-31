#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramActionModulesRuntime))]
internal sealed class CoreNativeActionDispatchCoreProgramActionModulesAdapter(Func<CoreProgramActionModulesRuntime> targetFactory) : ICoreNativeActionDispatchCoreProgramActionModulesOperations
{
    private readonly Func<CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}
