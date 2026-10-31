#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseUsedProjections PhaseUsedProjections { get; }
}

internal interface ICoreGameActivationProgressCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get; }
}
