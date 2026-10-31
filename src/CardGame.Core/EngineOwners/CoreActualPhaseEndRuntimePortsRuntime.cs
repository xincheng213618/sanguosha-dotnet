#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreActualPhaseEndRuntimePortsRuntime : IActualPhaseEndFacts, IActualPhaseEndIssuedItems, IActualPhaseEndNativeReturns
{
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CharacterHuangZuMobileShiEntries _characterHuangZuMobileShiOperations;
    private readonly RuntimeEntries.CharacterLuShiOlEntries _characterLuShiOlOperations;
    private readonly RuntimeEntries.CharacterLuYuShengMobileShiEntries _characterLuYuShengMobileShiOperations;
    private readonly RuntimeEntries.CharacterSimaShiMouEntries _characterSimaShiMouOperations;
    private readonly RuntimeEntries.CharacterYaDanOlEntries _characterYaDanOlOperations;
    private readonly RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations _programPhaseSchedulingOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreActualPhaseEndRuntimePortsRuntime(RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, RuntimeEntries.CharacterHuangZuMobileShiEntries characterHuangZuMobileShiOperations, RuntimeEntries.CharacterLuShiOlEntries characterLuShiOlOperations, RuntimeEntries.CharacterLuYuShengMobileShiEntries characterLuYuShengMobileShiOperations, RuntimeEntries.CharacterSimaShiMouEntries characterSimaShiMouOperations, RuntimeEntries.CharacterYaDanOlEntries characterYaDanOlOperations, RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, IKernelMatchModeRulesAccess modeRulesOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations programPhaseSchedulingOperations, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
