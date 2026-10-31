#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime : global::CardGame.Core.IMobileZhuRongProgramHost
{
    private readonly ICharacterZhuRongBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhuRongBoundaryMobileModeRulesOperations _modeRulesOperations;
    private readonly ICharacterZhuRongBoundaryMobilePendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICharacterZhuRongBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhuRongBoundaryMobileCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterZhuRongBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhuRongBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhuRongBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhuRongBoundaryMobileCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhuRongBoundaryMobileRuntime(ICharacterZhuRongBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterZhuRongBoundaryMobileModeRulesOperations modeRulesOperations, ICharacterZhuRongBoundaryMobilePendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICharacterZhuRongBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterZhuRongBoundaryMobileCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterZhuRongBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhuRongBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhuRongBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterZhuRongBoundaryMobileCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
