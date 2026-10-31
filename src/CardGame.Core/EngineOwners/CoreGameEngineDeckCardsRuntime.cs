#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineDeckCardsRuntime : global::CardGame.Core.IProgramDeckCardsFacts, global::CardGame.Core.IProgramDeckCardsOperations
{
    private readonly ICoreDeckCardsCharacterZuoCiHappyGodOperations _characterZuoCiHappyGodOperations;
    private readonly ICoreDeckCardsCardZonesOperations _cardZonesOperations;
    private readonly ICoreDeckCardsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreDeckCardsCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreDeckCardsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreDeckCardsCoreHappyWeiYanGainDestinationsOperations _coreHappyWeiYanGainDestinationsOperations;
    private readonly ICoreDeckCardsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDeckCardsRuntime(ICoreDeckCardsCharacterZuoCiHappyGodOperations characterZuoCiHappyGodOperations, ICoreDeckCardsCardZonesOperations cardZonesOperations, ICoreDeckCardsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreDeckCardsCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreDeckCardsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreDeckCardsCoreHappyWeiYanGainDestinationsOperations coreHappyWeiYanGainDestinationsOperations, ICoreDeckCardsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
