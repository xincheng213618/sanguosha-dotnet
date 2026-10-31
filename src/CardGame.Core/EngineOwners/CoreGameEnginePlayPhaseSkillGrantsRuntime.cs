#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePlayPhaseSkillGrantsRuntime : global::CardGame.Core.IPlayPhaseSkillGrantHost
{
    private readonly ICorePlayPhaseSkillGrantsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePlayPhaseSkillGrantsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePlayPhaseSkillGrantsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICorePlayPhaseSkillGrantsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICorePlayPhaseSkillGrantsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePlayPhaseSkillGrantsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePlayPhaseSkillGrantsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICorePlayPhaseSkillGrantsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEnginePlayPhaseSkillGrantsRuntime(ICorePlayPhaseSkillGrantsContentRegistryOperations contentRegistryOperations, ICorePlayPhaseSkillGrantsResolutionStackOperations resolutionStackOperations, ICorePlayPhaseSkillGrantsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICorePlayPhaseSkillGrantsCoreCardUseModulesOperations coreCardUseModulesOperations, ICorePlayPhaseSkillGrantsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePlayPhaseSkillGrantsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePlayPhaseSkillGrantsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICorePlayPhaseSkillGrantsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
