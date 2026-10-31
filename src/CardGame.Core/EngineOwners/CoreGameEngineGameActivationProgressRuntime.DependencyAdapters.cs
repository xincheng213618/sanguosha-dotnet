#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramPhaseUsedProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseUsedProjections PhaseUsedProjections { get => this._targetFactory().PhaseUsedProjections; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CoreGameActivationProgressCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGameActivationProgressCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}
