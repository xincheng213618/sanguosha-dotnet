#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterKeBiNengOlEntries _characterKeBiNengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterKongRongMouOlEntries _characterKongRongMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaTieAnniversaryEntries _characterMaTieAnniversaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries _coreActualTurnLegacyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries _coreDrawFundedDistinctBasicPaymentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries _coreDrawFundedDistinctBasicResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries _coreUnrespondableCounterspellsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineDrawFundedDistinctBasicObserversRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterKeBiNengOlEntries characterKeBiNengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterKongRongMouOlEntries characterKongRongMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterMaTieAnniversaryEntries characterMaTieAnniversaryOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnLegacyCompletionEntries coreActualTurnLegacyCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries coreDrawFundedDistinctBasicPaymentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries coreDrawFundedDistinctBasicResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries coreUnrespondableCounterspellsOperations, global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations, global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterKeBiNengOlOperations = characterKeBiNengOlOperations;
        this._characterKongRongMouOlOperations = characterKongRongMouOlOperations;
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._characterMaTieAnniversaryOperations = characterMaTieAnniversaryOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActualTurnLegacyCompletionOperations = coreActualTurnLegacyCompletionOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDrawFundedDistinctBasicPaymentOperations = coreDrawFundedDistinctBasicPaymentOperations;
        this._coreDrawFundedDistinctBasicResponsesOperations = coreDrawFundedDistinctBasicResponsesOperations;
        this._coreDrawFundedDistinctBasicReturnsOperations = coreDrawFundedDistinctBasicReturnsOperations;
        this._coreDrawFundedDistinctBasicUsesOperations = coreDrawFundedDistinctBasicUsesOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreUnrespondableCounterspellsOperations = coreUnrespondableCounterspellsOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
