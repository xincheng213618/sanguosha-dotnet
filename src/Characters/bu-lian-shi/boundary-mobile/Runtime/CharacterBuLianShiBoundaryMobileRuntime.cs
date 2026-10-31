#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterBuLianShiBoundaryMobileRuntime : global::CardGame.Core.IBoundaryBuLianShiHost, global::CardGame.Core.IBoundaryBuLianShiQueries
{
    private readonly ICharacterBuLianShiBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterBuLianShiBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterBuLianShiBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterBuLianShiBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterBuLianShiBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterBuLianShiBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterBuLianShiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterBuLianShiBoundaryMobileRuntime(ICharacterBuLianShiBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterBuLianShiBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterBuLianShiBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterBuLianShiBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterBuLianShiBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterBuLianShiBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterBuLianShiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
