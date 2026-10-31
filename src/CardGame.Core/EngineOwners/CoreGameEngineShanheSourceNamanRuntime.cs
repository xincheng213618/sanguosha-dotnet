#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceNamanRuntime : global::CardGame.Core.IShanheSourceNamanHost
{
    private readonly ICoreShanheSourceNamanCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheSourceNamanPendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICoreShanheSourceNamanResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceNamanCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreShanheSourceNamanCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheSourceNamanCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheSourceNamanCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceNamanCoreResponseCompletionOperations _coreResponseCompletionOperations;
    private readonly ICoreShanheSourceNamanCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceNamanRuntime(ICoreShanheSourceNamanCardZonesOperations cardZonesOperations, ICoreShanheSourceNamanPendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICoreShanheSourceNamanResolutionStackOperations resolutionStackOperations, ICoreShanheSourceNamanCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreShanheSourceNamanCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheSourceNamanCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheSourceNamanCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceNamanCoreResponseCompletionOperations coreResponseCompletionOperations, ICoreShanheSourceNamanCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
