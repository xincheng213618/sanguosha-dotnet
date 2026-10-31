#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYiJiOlRuntime : global::CardGame.Core.IYiJiProgramHost
{
    private readonly ICharacterYiJiOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterYiJiOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterYiJiOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYiJiOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterYiJiOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYiJiOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYiJiOlCoreHappyWeiYanGainDestinationsOperations _coreHappyWeiYanGainDestinationsOperations;
    private readonly ICharacterYiJiOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterYiJiOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterYiJiOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYiJiOlRuntime(ICharacterYiJiOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterYiJiOlCardZonesOperations cardZonesOperations, ICharacterYiJiOlResolutionStackOperations resolutionStackOperations, ICharacterYiJiOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterYiJiOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYiJiOlCoreRuntimeOperations coreRuntimeOperations, ICharacterYiJiOlCoreHappyWeiYanGainDestinationsOperations coreHappyWeiYanGainDestinationsOperations, ICharacterYiJiOlCoreMatchLogOperations coreMatchLogOperations, ICharacterYiJiOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterYiJiOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
