#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePhysicalDrawFacadesRuntime
{
    private readonly RuntimeEntries.CoreGameEnginePhysicalCardDrawEntries _corePhysicalCardDrawOperations;
    internal CorePhysicalDrawFacadesRuntime(RuntimeEntries.CoreGameEnginePhysicalCardDrawEntries corePhysicalCardDrawOperations)
    {
        this._corePhysicalCardDrawOperations = corePhysicalCardDrawOperations;
    }
}
