#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineForeignContestAidObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidColorDamageClaimObserversEntries _corePaidColorDamageClaimObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries _coreUnrespondableCounterspellsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly ICoreForeignContestAidObserversForeignTurnContestOperations _foreignTurnContestOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineForeignContestAidObserversRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidColorDamageClaimObserversEntries corePaidColorDamageClaimObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries coreUnrespondableCounterspellsOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,ICoreForeignContestAidObserversForeignTurnContestOperations foreignTurnContestOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._corePaidColorDamageClaimObserversOperations = corePaidColorDamageClaimObserversOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreSameTypeActualUseAidOperations = coreSameTypeActualUseAidOperations;
        this._coreUnrespondableCounterspellsOperations = coreUnrespondableCounterspellsOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._foreignTurnContestOperations = foreignTurnContestOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
