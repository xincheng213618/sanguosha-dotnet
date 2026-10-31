#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5QiangxiRuntime : global::CardGame.Core.IShanheS5QiangxiHost
{
    private readonly ICoreShanheS5QiangxiCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheS5QiangxiCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheS5QiangxiCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreShanheS5QiangxiCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5QiangxiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5QiangxiCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5QiangxiCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreShanheS5QiangxiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEngineShanheS5QiangxiRuntime(ICoreShanheS5QiangxiCardZonesOperations cardZonesOperations, ICoreShanheS5QiangxiCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheS5QiangxiCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreShanheS5QiangxiCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5QiangxiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5QiangxiCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5QiangxiCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreShanheS5QiangxiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
