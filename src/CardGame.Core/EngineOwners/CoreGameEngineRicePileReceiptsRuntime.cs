#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRicePileReceiptsRuntime
{
    private readonly ICoreRicePileReceiptsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRicePileReceiptsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreRicePileReceiptsCoreRicePileOperations _coreRicePileOperations;
    private readonly ICoreRicePileReceiptsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreRicePileReceiptsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    internal CoreGameEngineRicePileReceiptsRuntime(ICoreRicePileReceiptsResolutionStackOperations resolutionStackOperations, ICoreRicePileReceiptsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreRicePileReceiptsCoreRicePileOperations coreRicePileOperations, ICoreRicePileReceiptsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreRicePileReceiptsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreRicePileOperations = coreRicePileOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
    }
}
