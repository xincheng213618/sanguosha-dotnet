#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramSelectionModulesRuntime))]
internal sealed class CoreProgramTargetResolversCoreProgramSelectionModulesAdapter(Func<CoreProgramSelectionModulesRuntime> targetFactory) : ICoreProgramTargetResolversCoreProgramSelectionModulesOperations
{
    private readonly Func<CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public IProgramSelectionQueries ProgramSelectionQueries { get => this._targetFactory().ProgramSelectionQueries; }
}
