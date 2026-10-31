#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAdjacentDiscardStorageRuntime : global::CardGame.Core.IAdjacentDiscardAndRoundAlcoholHost
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUsePaymentsEntries _coreCompletedUsePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries _coreRoundPricedPileAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAdjacentDiscardStorageRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUsePaymentsEntries coreCompletedUsePaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries coreRoundPricedPileAlcoholOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCompletedUsePaymentsOperations = coreCompletedUsePaymentsOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRoundPricedPileAlcoholOperations = coreRoundPricedPileAlcoholOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
