#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterBuLianShiMobileRuntime : global::CardGame.Core.IMobileBuLianShiAnxuHost
{
    private readonly ICharacterBuLianShiMobileActiveCardMovementBatchIdsOperations _activeCardMovementBatchIdsOperations;
    private readonly ICharacterBuLianShiMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterBuLianShiMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterBuLianShiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterBuLianShiMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterBuLianShiMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterBuLianShiMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterBuLianShiMobileRuntime(ICharacterBuLianShiMobileActiveCardMovementBatchIdsOperations activeCardMovementBatchIdsOperations, ICharacterBuLianShiMobileResolutionStackOperations resolutionStackOperations, ICharacterBuLianShiMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterBuLianShiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterBuLianShiMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterBuLianShiMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterBuLianShiMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._activeCardMovementBatchIdsOperations = activeCardMovementBatchIdsOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
