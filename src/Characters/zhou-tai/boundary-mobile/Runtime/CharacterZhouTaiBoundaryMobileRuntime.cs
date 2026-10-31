#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterZhouTaiBoundaryMobileRuntime
{
    private readonly ICharacterZhouTaiBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhouTaiBoundaryMobileCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICharacterZhouTaiBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhouTaiBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhouTaiBoundaryMobileRuntime(ICharacterZhouTaiBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterZhouTaiBoundaryMobileCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICharacterZhouTaiBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhouTaiBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
