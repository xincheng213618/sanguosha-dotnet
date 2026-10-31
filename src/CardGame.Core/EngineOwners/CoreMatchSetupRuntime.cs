#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchSetupRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModePresentationEntries _coreModePresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOpeningCustomizationEntries _coreOpeningCustomizationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess _coreActualTurnProgressStartedAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreMatchSetupRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineModePresentationEntries coreModePresentationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOpeningCustomizationEntries coreOpeningCustomizationOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations kernelCoreGameEngineRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess coreActualTurnProgressStartedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreModePresentationOperations = coreModePresentationOperations;
        this._coreOpeningCustomizationOperations = coreOpeningCustomizationOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._coreActualTurnProgressStartedAccess = coreActualTurnProgressStartedAccess;
        this._coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess = coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
