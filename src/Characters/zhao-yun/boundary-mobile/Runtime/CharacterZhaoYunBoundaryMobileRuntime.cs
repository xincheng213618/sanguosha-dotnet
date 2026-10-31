#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhaoYunBoundaryMobileRuntime : global::CardGame.Core.IMobileZhaoYunProgramHost
{
    private readonly ICharacterZhaoYunBoundaryMobileCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhaoYunBoundaryMobileModeRulesOperations _modeRulesOperations;
    private readonly ICharacterZhaoYunBoundaryMobilePendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICharacterZhaoYunBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterZhaoYunBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhaoYunBoundaryMobileRuntime(ICharacterZhaoYunBoundaryMobileCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterZhaoYunBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterZhaoYunBoundaryMobileModeRulesOperations modeRulesOperations, ICharacterZhaoYunBoundaryMobilePendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICharacterZhaoYunBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterZhaoYunBoundaryMobileCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterZhaoYunBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhaoYunBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhaoYunBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterZhaoYunBoundaryMobileCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterZhaoYunBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
