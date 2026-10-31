#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRequestedDeckBasicOwnershipRuntime
{
    private readonly ICoreRequestedDeckBasicOwnershipCardZonesOperations _cardZonesOperations;
    private readonly ICoreRequestedDeckBasicOwnershipContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRequestedDeckBasicOwnershipResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRequestedDeckBasicOwnershipCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreRequestedDeckBasicOwnershipCoreRequestedDeckBasicsOperations _coreRequestedDeckBasicsOperations;
    private readonly ICoreRequestedDeckBasicOwnershipCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreRequestedDeckBasicOwnershipCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRequestedDeckBasicOwnershipRuntime(ICoreRequestedDeckBasicOwnershipCardZonesOperations cardZonesOperations, ICoreRequestedDeckBasicOwnershipContentRegistryOperations contentRegistryOperations, ICoreRequestedDeckBasicOwnershipResolutionStackOperations resolutionStackOperations, ICoreRequestedDeckBasicOwnershipCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreRequestedDeckBasicOwnershipCoreRequestedDeckBasicsOperations coreRequestedDeckBasicsOperations, ICoreRequestedDeckBasicOwnershipCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreRequestedDeckBasicOwnershipCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
