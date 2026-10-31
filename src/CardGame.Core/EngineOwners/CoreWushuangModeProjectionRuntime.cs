#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreWushuangModeProjectionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess _coreActualTurnProgressStartedTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreWushuangModeProjectionRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess coreActualTurnProgressStartedTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._coreWushuangOperations = coreWushuangOperations;
        this._coreActualTurnProgressStartedTurnNumberAccess = coreActualTurnProgressStartedTurnNumberAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
