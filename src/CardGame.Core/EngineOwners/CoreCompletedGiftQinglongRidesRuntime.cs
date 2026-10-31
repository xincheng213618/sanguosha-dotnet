#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreCompletedGiftQinglongRidesRuntime
{
    private readonly ICoreCompletedGiftQinglongRidesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCompletedGiftQinglongRidesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCompletedGiftQinglongRidesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreCompletedGiftQinglongRidesCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreCompletedGiftQinglongRidesCoreCompletedCardGiftsOperations _coreCompletedCardGiftsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreCompletedGiftQinglongRidesRuntime(ICoreCompletedGiftQinglongRidesContentRegistryOperations contentRegistryOperations, ICoreCompletedGiftQinglongRidesResolutionStackOperations resolutionStackOperations, ICoreCompletedGiftQinglongRidesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreCompletedGiftQinglongRidesCoreCardActionsOperations coreCardActionsOperations, ICoreCompletedGiftQinglongRidesCoreCompletedCardGiftsOperations coreCompletedCardGiftsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCompletedCardGiftsOperations = coreCompletedCardGiftsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
