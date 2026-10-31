#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramPhaseUsedProjectionsRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramPhaseUsedProjectionsAdapter(Func<CoreProgramPhaseUsedProjectionsRuntime> targetFactory) : ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations
{
    private readonly Func<CoreProgramPhaseUsedProjectionsRuntime> _targetFactory = targetFactory;
    public ProgramPhaseUsedProjections PhaseUsedProjections { get => this._targetFactory().PhaseUsedProjections; }
}

[Core.ProgramCapability(typeof(CoreProgramSelectionModulesRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramSelectionModulesAdapter(Func<CoreProgramSelectionModulesRuntime> targetFactory) : ICoreGameActivationProgressCoreProgramSelectionModulesOperations
{
    private readonly Func<CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}
