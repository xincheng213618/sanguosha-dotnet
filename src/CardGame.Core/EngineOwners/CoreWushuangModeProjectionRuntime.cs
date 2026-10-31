#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreWushuangModeProjectionRuntime
{
    private readonly RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess _coreActualTurnProgressStartedTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreWushuangModeProjectionRuntime(RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations, EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess coreActualTurnProgressStartedTurnNumberAccess, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._coreWushuangOperations = coreWushuangOperations;
        this._coreActualTurnProgressStartedTurnNumberAccess = coreActualTurnProgressStartedTurnNumberAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
