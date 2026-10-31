#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecipientContestObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientConsequencesObserversEntries _coreRecipientConsequencesObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientContestReceiptsEntries _coreRecipientContestReceiptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries _coreRoundPricedPileAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries _coreUnrespondableCounterspellsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineRecipientContestObserversRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientConsequencesObserversEntries coreRecipientConsequencesObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientContestReceiptsEntries coreRecipientContestReceiptsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries coreRoundPricedPileAlcoholOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries coreUnrespondableCounterspellsOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreRecipientConsequencesObserversOperations = coreRecipientConsequencesObserversOperations;
        this._coreRecipientContestReceiptsOperations = coreRecipientContestReceiptsOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRoundPricedPileAlcoholOperations = coreRoundPricedPileAlcoholOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreUnrespondableCounterspellsOperations = coreUnrespondableCounterspellsOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
