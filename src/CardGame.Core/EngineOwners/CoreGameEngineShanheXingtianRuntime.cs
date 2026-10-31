#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheXingtianRuntime : global::CardGame.Core.IShanheXingtianHost
{
    private readonly ICoreShanheXingtianCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheXingtianContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheXingtianResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheXingtianTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreShanheXingtianCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreShanheXingtianCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreShanheXingtianCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheXingtianCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheXingtianCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheXingtianCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheXingtianKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheXingtianRuntime(ICoreShanheXingtianCardZonesOperations cardZonesOperations, ICoreShanheXingtianContentRegistryOperations contentRegistryOperations, ICoreShanheXingtianResolutionStackOperations resolutionStackOperations, ICoreShanheXingtianTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreShanheXingtianCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreShanheXingtianCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreShanheXingtianCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheXingtianCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheXingtianCoreRuntimeOperations coreRuntimeOperations, ICoreShanheXingtianCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheXingtianKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
