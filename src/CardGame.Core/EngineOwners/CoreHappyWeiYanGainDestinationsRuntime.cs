#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreHappyWeiYanGainDestinationsRuntime
{
    private readonly ICoreHappyWeiYanGainDestinationsCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCharacterWeiYanHappyGodOperations _characterWeiYanHappyGodOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCardZonesOperations _cardZonesOperations;
    private readonly ICoreHappyWeiYanGainDestinationsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreHappyWeiYanGainDestinationsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreHappyWeiYanGainDestinationsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreHappyWeiYanGainDestinationsRuntime(ICoreHappyWeiYanGainDestinationsCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICoreHappyWeiYanGainDestinationsCharacterWeiYanHappyGodOperations characterWeiYanHappyGodOperations, ICoreHappyWeiYanGainDestinationsCardZonesOperations cardZonesOperations, ICoreHappyWeiYanGainDestinationsContentRegistryOperations contentRegistryOperations, ICoreHappyWeiYanGainDestinationsResolutionStackOperations resolutionStackOperations, ICoreHappyWeiYanGainDestinationsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreHappyWeiYanGainDestinationsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreHappyWeiYanGainDestinationsCoreRuntimeOperations coreRuntimeOperations, ICoreHappyWeiYanGainDestinationsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
