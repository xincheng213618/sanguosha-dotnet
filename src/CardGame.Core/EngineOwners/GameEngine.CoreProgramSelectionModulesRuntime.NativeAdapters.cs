#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramSelectionModulesProgramSelectionQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionModulesProgramSelectionQueriesOperations
    {
        public ref global::CardGame.Core.IProgramSelectionQueries? _programSelectionQueries => ref engine._programSelectionQueries;
    }

    [ProgramCapability]
    private sealed class CoreProgramSelectionModulesKernelCoreProgramSelectionModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionModulesKernelCoreProgramSelectionModulesOperations
    {
        public global::CardGame.Core.NativeOwners.SelectionQueries CreateSelectionQueries() => engine.ComposeNativeSelectionQueries();
    }

    [ProgramCapability]
    private sealed class CoreProgramSelectionModulesProgramSelectionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionModulesProgramSelectionsOperations
    {
        public ref global::CardGame.Core.ProgramSelectionRegistry? _programSelections => ref engine._programSelections;
    }
}
