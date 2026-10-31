#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramSelectionModulesRuntime
{
    internal ProgramSelectionRegistry ProgramSelections => _programSelectionsOperations._programSelections ??= new(CoreProgramSelectionModulesDefinitions.ProgramSelectionMetadata.Value, ProgramSelectionQueries);
    internal IProgramSelectionQueries ProgramSelectionQueries => _programSelectionQueriesOperations._programSelectionQueries ??= _kernelCoreProgramSelectionModulesOperations.CreateSelectionQueries();
}
