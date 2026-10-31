#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramSelectionModulesRuntime
{
    internal ProgramSelectionRegistry ProgramSelections => _programSelectionsOperations._programSelections ??= new(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesDefinitions.ProgramSelectionMetadata.Value, ProgramSelectionQueries);
    internal IProgramSelectionQueries ProgramSelectionQueries => _programSelectionQueriesOperations._programSelectionQueries ??= _kernelCoreProgramSelectionModulesOperations.CreateSelectionQueries();
}
