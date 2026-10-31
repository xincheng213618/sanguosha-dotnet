#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNonBasicSlashProfileRuntime
{
    private readonly ICoreNonBasicSlashProfileCardZonesOperations _cardZonesOperations;
    private readonly ICoreNonBasicSlashProfileContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNonBasicSlashProfileResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNonBasicSlashProfileCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreNonBasicSlashProfileCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreNonBasicSlashProfileCoreCurrentSlashFireOffersOperations _coreCurrentSlashFireOffersOperations;
    private readonly ICoreNonBasicSlashProfileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreNonBasicSlashProfileCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreNonBasicSlashProfileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEngineNonBasicSlashProfileRuntime(ICoreNonBasicSlashProfileCardZonesOperations cardZonesOperations, ICoreNonBasicSlashProfileContentRegistryOperations contentRegistryOperations, ICoreNonBasicSlashProfileResolutionStackOperations resolutionStackOperations, ICoreNonBasicSlashProfileCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreNonBasicSlashProfileCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreNonBasicSlashProfileCoreCurrentSlashFireOffersOperations coreCurrentSlashFireOffersOperations, ICoreNonBasicSlashProfileCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreNonBasicSlashProfileCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreNonBasicSlashProfileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
