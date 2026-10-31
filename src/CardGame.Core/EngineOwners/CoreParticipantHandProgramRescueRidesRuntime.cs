#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreParticipantHandProgramRescueRidesRuntime
{
    private readonly ICoreParticipantHandProgramRescueRidesCardZonesOperations _cardZonesOperations;
    private readonly ICoreParticipantHandProgramRescueRidesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreParticipantHandProgramRescueRidesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreParticipantHandProgramRescueRidesCoreRoundPricedPileAlcoholOperations _coreRoundPricedPileAlcoholOperations;
    private readonly ICoreParticipantHandProgramRescueRidesCoreParticipantHandDamageRidesOperations _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreParticipantHandProgramRescueRidesRuntime(ICoreParticipantHandProgramRescueRidesCardZonesOperations cardZonesOperations, ICoreParticipantHandProgramRescueRidesContentRegistryOperations contentRegistryOperations, ICoreParticipantHandProgramRescueRidesResolutionStackOperations resolutionStackOperations, ICoreParticipantHandProgramRescueRidesCoreRoundPricedPileAlcoholOperations coreRoundPricedPileAlcoholOperations, ICoreParticipantHandProgramRescueRidesCoreParticipantHandDamageRidesOperations coreParticipantHandDamageRidesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreRoundPricedPileAlcoholOperations = coreRoundPricedPileAlcoholOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
