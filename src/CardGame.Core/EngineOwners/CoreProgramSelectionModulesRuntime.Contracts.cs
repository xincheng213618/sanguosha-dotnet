#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSelectionModulesProgramSelectionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.IProgramSelectionQueries? _programSelectionQueries { get; }
}

internal interface ICoreProgramSelectionModulesKernelCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.SelectionQueries CreateSelectionQueries();
}

internal interface ICoreProgramSelectionModulesProgramSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramSelectionRegistry? _programSelections { get; }
}
