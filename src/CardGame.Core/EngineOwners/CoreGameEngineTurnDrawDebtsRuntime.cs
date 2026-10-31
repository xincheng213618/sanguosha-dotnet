#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineTurnDrawDebtsRuntime : global::CardGame.Core.ITurnDrawDebtProgramHost
{
    private readonly ICoreTurnDrawDebtsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreTurnDrawDebtsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTurnDrawDebtsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreTurnDrawDebtsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreTurnDrawDebtsCoreCurrentTurnUseKindsOperations _coreCurrentTurnUseKindsOperations;
    private readonly ICoreTurnDrawDebtsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreTurnDrawDebtsCoreProgramOwnedCardsOperations _coreProgramOwnedCardsOperations;
    private readonly ICoreTurnDrawDebtsCorePublicFactionCountsOperations _corePublicFactionCountsOperations;
    private readonly ICoreTurnDrawDebtsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreTurnDrawDebtsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreTurnDrawDebtsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineTurnDrawDebtsRuntime(ICoreTurnDrawDebtsContentRegistryOperations contentRegistryOperations, ICoreTurnDrawDebtsResolutionStackOperations resolutionStackOperations, ICoreTurnDrawDebtsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreTurnDrawDebtsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreTurnDrawDebtsCoreCurrentTurnUseKindsOperations coreCurrentTurnUseKindsOperations, ICoreTurnDrawDebtsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreTurnDrawDebtsCoreProgramOwnedCardsOperations coreProgramOwnedCardsOperations, ICoreTurnDrawDebtsCorePublicFactionCountsOperations corePublicFactionCountsOperations, ICoreTurnDrawDebtsCoreRuntimeOperations coreRuntimeOperations, ICoreTurnDrawDebtsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreTurnDrawDebtsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCurrentTurnUseKindsOperations = coreCurrentTurnUseKindsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramOwnedCardsOperations = coreProgramOwnedCardsOperations;
        this._corePublicFactionCountsOperations = corePublicFactionCountsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
