#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBossActivitiesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess _coreMatchDefinitionOptionsPlayerCountAccess;
    internal CoreGameEngineBossActivitiesRuntime(RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess coreMatchDefinitionOptionsPlayerCountAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreMatchDefinitionOptionsPlayerCountAccess = coreMatchDefinitionOptionsPlayerCountAccess;
    }
}
