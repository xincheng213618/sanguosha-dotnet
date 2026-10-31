#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePrivateTurnHoldsRuntime : global::CardGame.Core.IPrivateTurnHoldProgramHost
{
    private readonly ICorePrivateTurnHoldsCardZonesOperations _cardZonesOperations;
    private readonly ICorePrivateTurnHoldsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePrivateTurnHoldsCoreAfterTurnEndedProgramsOperations _coreAfterTurnEndedProgramsOperations;
    private readonly ICorePrivateTurnHoldsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICorePrivateTurnHoldsCorePrivateCardOffersOperations _corePrivateCardOffersOperations;
    private readonly ICorePrivateTurnHoldsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePrivateTurnHoldsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePrivateTurnHoldsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICorePrivateTurnHoldsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICorePrivateTurnHoldsCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePrivateTurnHoldsRuntime(ICorePrivateTurnHoldsCardZonesOperations cardZonesOperations, ICorePrivateTurnHoldsContentRegistryOperations contentRegistryOperations, ICorePrivateTurnHoldsCoreAfterTurnEndedProgramsOperations coreAfterTurnEndedProgramsOperations, ICorePrivateTurnHoldsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICorePrivateTurnHoldsCorePrivateCardOffersOperations corePrivateCardOffersOperations, ICorePrivateTurnHoldsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePrivateTurnHoldsCoreRuntimeOperations coreRuntimeOperations, ICorePrivateTurnHoldsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICorePrivateTurnHoldsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICorePrivateTurnHoldsCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._corePrivateCardOffersOperations = corePrivateCardOffersOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
