#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingRescueContinuationsRuntime
{
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    internal CoreGameEngineDyingRescueContinuationsRuntime(RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
    }
}
