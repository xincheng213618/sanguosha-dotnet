#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOtherDyingRecoveryRidesRuntime
{
    private readonly ICoreOtherDyingRecoveryRidesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreOtherDyingRecoveryRidesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreOtherDyingRecoveryRidesCoreProgramChildResolutionOperations _coreProgramChildResolutionOperations;
    private readonly ICoreOtherDyingRecoveryRidesCoreRecoveryReplacementDamageRidesOperations _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineOtherDyingRecoveryRidesRuntime(ICoreOtherDyingRecoveryRidesContentRegistryOperations contentRegistryOperations, ICoreOtherDyingRecoveryRidesResolutionStackOperations resolutionStackOperations, ICoreOtherDyingRecoveryRidesCoreProgramChildResolutionOperations coreProgramChildResolutionOperations, ICoreOtherDyingRecoveryRidesCoreRecoveryReplacementDamageRidesOperations coreRecoveryReplacementDamageRidesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
