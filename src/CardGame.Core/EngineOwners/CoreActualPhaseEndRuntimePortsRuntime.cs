#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreActualPhaseEndRuntimePortsRuntime : global::CardGame.Core.IActualPhaseEndFacts, global::CardGame.Core.IActualPhaseEndIssuedItems, global::CardGame.Core.IActualPhaseEndNativeReturns
{
    private readonly ICoreActualPhaseEndRuntimePortsCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterHuangZuMobileShiOperations _characterHuangZuMobileShiOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterLuShiOlOperations _characterLuShiOlOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterLuYuShengMobileShiOperations _characterLuYuShengMobileShiOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterSimaShiMouOperations _characterSimaShiMouOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterYaDanOlOperations _characterYaDanOlOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCharacterZhongHuiAnniversaryGodOperations _characterZhongHuiAnniversaryGodOperations;
    private readonly ICoreActualPhaseEndRuntimePortsModeRulesOperations _modeRulesOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreActualPhaseEndRuntimePortsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreActualPhaseEndRuntimePortsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations _programPhaseSchedulingOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreActualPhaseEndRuntimePortsRuntime(ICoreActualPhaseEndRuntimePortsCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreActualPhaseEndRuntimePortsCharacterHuangZuMobileShiOperations characterHuangZuMobileShiOperations, ICoreActualPhaseEndRuntimePortsCharacterLuShiOlOperations characterLuShiOlOperations, ICoreActualPhaseEndRuntimePortsCharacterLuYuShengMobileShiOperations characterLuYuShengMobileShiOperations, ICoreActualPhaseEndRuntimePortsCharacterSimaShiMouOperations characterSimaShiMouOperations, ICoreActualPhaseEndRuntimePortsCharacterYaDanOlOperations characterYaDanOlOperations, ICoreActualPhaseEndRuntimePortsCharacterZhongHuiAnniversaryGodOperations characterZhongHuiAnniversaryGodOperations, ICoreActualPhaseEndRuntimePortsModeRulesOperations modeRulesOperations, ICoreActualPhaseEndRuntimePortsCoreActualTurnsOperations coreActualTurnsOperations, ICoreActualPhaseEndRuntimePortsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreActualPhaseEndRuntimePortsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations programPhaseSchedulingOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterHuangZuMobileShiOperations = characterHuangZuMobileShiOperations;
        this._characterLuShiOlOperations = characterLuShiOlOperations;
        this._characterLuYuShengMobileShiOperations = characterLuYuShengMobileShiOperations;
        this._characterSimaShiMouOperations = characterSimaShiMouOperations;
        this._characterYaDanOlOperations = characterYaDanOlOperations;
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._programPhaseSchedulingOperations = programPhaseSchedulingOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
