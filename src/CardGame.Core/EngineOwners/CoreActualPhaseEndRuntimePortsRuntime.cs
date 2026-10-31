#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreActualPhaseEndRuntimePortsRuntime : global::CardGame.Core.IActualPhaseEndFacts, global::CardGame.Core.IActualPhaseEndIssuedItems, global::CardGame.Core.IActualPhaseEndNativeReturns
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuShiOlEntries _characterLuShiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuYuShengMobileShiEntries _characterLuYuShengMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries _characterSimaShiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries _characterYaDanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations _programPhaseSchedulingOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreActualPhaseEndRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterLuShiOlEntries characterLuShiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuYuShengMobileShiEntries characterLuYuShengMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries characterSimaShiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterYaDanOlEntries characterYaDanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations programPhaseSchedulingOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
