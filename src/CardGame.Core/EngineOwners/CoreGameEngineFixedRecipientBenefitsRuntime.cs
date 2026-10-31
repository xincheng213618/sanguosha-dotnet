#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFixedRecipientBenefitsRuntime
{
    private readonly ICoreFixedRecipientBenefitsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreFixedRecipientBenefitsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreFixedRecipientBenefitsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreFixedRecipientBenefitsCoreOwnedDeathBenefitReturnsOperations _coreOwnedDeathBenefitReturnsOperations;
    private readonly ICoreFixedRecipientBenefitsCorePairObtainShownGiftsOperations _corePairObtainShownGiftsOperations;
    private readonly ICoreFixedRecipientBenefitsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreFixedRecipientBenefitsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreFixedRecipientBenefitsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreFixedRecipientBenefitsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineFixedRecipientBenefitsRuntime(ICoreFixedRecipientBenefitsContentRegistryOperations contentRegistryOperations, ICoreFixedRecipientBenefitsResolutionStackOperations resolutionStackOperations, ICoreFixedRecipientBenefitsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreFixedRecipientBenefitsCoreOwnedDeathBenefitReturnsOperations coreOwnedDeathBenefitReturnsOperations, ICoreFixedRecipientBenefitsCorePairObtainShownGiftsOperations corePairObtainShownGiftsOperations, ICoreFixedRecipientBenefitsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreFixedRecipientBenefitsCoreRuntimeOperations coreRuntimeOperations, ICoreFixedRecipientBenefitsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreFixedRecipientBenefitsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreOwnedDeathBenefitReturnsOperations = coreOwnedDeathBenefitReturnsOperations;
        this._corePairObtainShownGiftsOperations = corePairObtainShownGiftsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
