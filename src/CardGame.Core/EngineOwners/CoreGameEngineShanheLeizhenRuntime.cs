#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheLeizhenRuntime
{
    private readonly ICoreShanheLeizhenContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheLeizhenResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheLeizhenCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreShanheLeizhenCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreShanheLeizhenCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheLeizhenCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShanheLeizhenCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreShanheLeizhenCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheLeizhenCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheLeizhenCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheLeizhenKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheLeizhenRuntime(ICoreShanheLeizhenContentRegistryOperations contentRegistryOperations, ICoreShanheLeizhenResolutionStackOperations resolutionStackOperations, ICoreShanheLeizhenCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreShanheLeizhenCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreShanheLeizhenCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheLeizhenCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShanheLeizhenCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreShanheLeizhenCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheLeizhenCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheLeizhenCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheLeizhenKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
