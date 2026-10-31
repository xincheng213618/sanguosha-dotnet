#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreActualDiscardRecoveryPhaseRuntimePortsRuntime : global::CardGame.Core.IActualDiscardRecoveryPhaseFacts, global::CardGame.Core.IActualPhaseQueryCache
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreActualDiscardRecoveryPhaseRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
    {
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreGameEngineActionQueriesActionQueryAccess = coreGameEngineActionQueriesActionQueryAccess;
    }
}
