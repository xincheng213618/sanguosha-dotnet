#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiuYouAnniversaryRuntime : global::CardGame.Core.IAnniversaryLiuYouHost, global::CardGame.Core.IAnniversaryLiuYouQueries
{
    private readonly ICharacterLiuYouAnniversaryCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICharacterLiuYouAnniversaryContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiuYouAnniversaryResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLiuYouAnniversaryCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICharacterLiuYouAnniversaryCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterLiuYouAnniversaryCoreEquipmentSlotGroupProgramsOperations _coreEquipmentSlotGroupProgramsOperations;
    private readonly ICharacterLiuYouAnniversaryCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLiuYouAnniversaryCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiuYouAnniversaryCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLiuYouAnniversaryCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLiuYouAnniversaryKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiuYouAnniversaryRuntime(ICharacterLiuYouAnniversaryCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICharacterLiuYouAnniversaryContentRegistryOperations contentRegistryOperations, ICharacterLiuYouAnniversaryResolutionStackOperations resolutionStackOperations, ICharacterLiuYouAnniversaryCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICharacterLiuYouAnniversaryCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterLiuYouAnniversaryCoreEquipmentSlotGroupProgramsOperations coreEquipmentSlotGroupProgramsOperations, ICharacterLiuYouAnniversaryCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLiuYouAnniversaryCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiuYouAnniversaryCoreRuntimeOperations coreRuntimeOperations, ICharacterLiuYouAnniversaryCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLiuYouAnniversaryKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentSlotGroupProgramsOperations = coreEquipmentSlotGroupProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
