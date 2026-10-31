#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineNativeDrawRuntimeServicesRuntime : global::CardGame.Core.INativeDrawAttemptOperations, global::CardGame.Core.INativeDrawAttemptQueries, global::CardGame.Core.INativeDrawProducerOperations, global::CardGame.Core.INativeDrawProducerQueries
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiOlMagicEntries _characterZhangFeiOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries _coreActualDiscardRecoveryPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLiushiEffectsEntries _coreLiushiEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries _coreNativeDrawAttemptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries _coreOutsidePhaseDrawDiscardOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries _corePairObtainShownGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardImmediateDrawEntries _corePrepDiscardImmediateDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries _coreProgramCategoryTargetLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries _coreSkillOwnershipWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelReturnOperationOperations _nativeDrawReturnsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreGameEngineNativeDrawRuntimeServicesRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhangFeiOlMagicEntries characterZhangFeiOlMagicOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries coreActualDiscardRecoveryPhasesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineLiushiEffectsEntries coreLiushiEffectsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries coreNativeDrawAttemptsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries coreOutsidePhaseDrawDiscardOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries corePairObtainShownGiftsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardImmediateDrawEntries corePrepDiscardImmediateDrawOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries coreProgramCategoryTargetLedgerOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries coreSkillOwnershipWindowsOperations,global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations,global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineOwners.IKernelReturnOperationOperations nativeDrawReturnsOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterZhangFeiOlMagicOperations = characterZhangFeiOlMagicOperations;
        this._characterZhouFeiLeOperations = characterZhouFeiLeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreLiushiEffectsOperations = coreLiushiEffectsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeDrawAttemptsOperations = coreNativeDrawAttemptsOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._corePairObtainShownGiftsOperations = corePairObtainShownGiftsOperations;
        this._corePrepDiscardImmediateDrawOperations = corePrepDiscardImmediateDrawOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramCategoryTargetLedgerOperations = coreProgramCategoryTargetLedgerOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._nativeDrawReturnsOperations = nativeDrawReturnsOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
