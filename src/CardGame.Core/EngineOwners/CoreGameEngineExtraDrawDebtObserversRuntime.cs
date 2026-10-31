#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineExtraDrawDebtObserversRuntime
{
    private readonly ICoreExtraDrawDebtObserversContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreExtraDrawDebtObserversResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreExtraDrawDebtObserversCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreExtraDrawDebtObserversCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly ICoreExtraDrawDebtObserversCoreTurnDrawDebtsOperations _coreTurnDrawDebtsOperations;
    private readonly ICoreExtraDrawDebtObserversCoreParticipantHandProgramRescueRidesOperations _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineExtraDrawDebtObserversRuntime(ICoreExtraDrawDebtObserversContentRegistryOperations contentRegistryOperations, ICoreExtraDrawDebtObserversResolutionStackOperations resolutionStackOperations, ICoreExtraDrawDebtObserversCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreExtraDrawDebtObserversCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, ICoreExtraDrawDebtObserversCoreTurnDrawDebtsOperations coreTurnDrawDebtsOperations, ICoreExtraDrawDebtObserversCoreParticipantHandProgramRescueRidesOperations coreParticipantHandProgramRescueRidesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreTurnDrawDebtsOperations = coreTurnDrawDebtsOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
