#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchSetupRuntime
{
    private readonly ICoreMatchSetupModeRulesOperations _modeRulesOperations;
    private readonly ICoreMatchSetupModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreMatchSetupCoreModePresentationOperations _coreModePresentationOperations;
    private readonly ICoreMatchSetupCoreOpeningCustomizationOperations _coreOpeningCustomizationOperations;
    private readonly ICoreMatchSetupCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreMatchSetupKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreMatchSetupKernelCoreGameEngineRuntimeOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess _coreActualTurnProgressStartedAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreMatchSetupRuntime(ICoreMatchSetupModeRulesOperations modeRulesOperations, ICoreMatchSetupModeRuntimeOperations modeRuntimeOperations, ICoreMatchSetupCoreModePresentationOperations coreModePresentationOperations, ICoreMatchSetupCoreOpeningCustomizationOperations coreOpeningCustomizationOperations, ICoreMatchSetupCoreMatchLogOperations coreMatchLogOperations, ICoreMatchSetupKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreMatchSetupKernelCoreGameEngineRuntimeOperations kernelCoreGameEngineRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess coreActualTurnProgressStartedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
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
