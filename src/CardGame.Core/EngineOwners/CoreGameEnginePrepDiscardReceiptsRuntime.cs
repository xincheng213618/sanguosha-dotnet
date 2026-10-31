#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePrepDiscardReceiptsRuntime
{
    private readonly ICorePrepDiscardReceiptsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICorePrepDiscardReceiptsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICorePrepDiscardReceiptsCorePrepDiscardEndingPromisesOperations _corePrepDiscardEndingPromisesOperations;
    private readonly ICorePrepDiscardReceiptsCorePrepDiscardImmediateDrawOperations _corePrepDiscardImmediateDrawOperations;
    private readonly ICorePrepDiscardReceiptsCorePrepDiscardPaymentsOperations _corePrepDiscardPaymentsOperations;
    private readonly ICorePrepDiscardReceiptsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEnginePrepDiscardReceiptsRuntime(ICorePrepDiscardReceiptsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICorePrepDiscardReceiptsCoreEquipmentContentOperations coreEquipmentContentOperations, ICorePrepDiscardReceiptsCorePrepDiscardEndingPromisesOperations corePrepDiscardEndingPromisesOperations, ICorePrepDiscardReceiptsCorePrepDiscardImmediateDrawOperations corePrepDiscardImmediateDrawOperations, ICorePrepDiscardReceiptsCorePrepDiscardPaymentsOperations corePrepDiscardPaymentsOperations, ICorePrepDiscardReceiptsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._corePrepDiscardEndingPromisesOperations = corePrepDiscardEndingPromisesOperations;
        this._corePrepDiscardImmediateDrawOperations = corePrepDiscardImmediateDrawOperations;
        this._corePrepDiscardPaymentsOperations = corePrepDiscardPaymentsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
