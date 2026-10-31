#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePrepDiscardReceiptsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly ICorePrepDiscardReceiptsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries _corePrepDiscardEndingPromisesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardImmediateDrawEntries _corePrepDiscardImmediateDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardPaymentsEntries _corePrepDiscardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEnginePrepDiscardReceiptsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, ICorePrepDiscardReceiptsCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries corePrepDiscardEndingPromisesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardImmediateDrawEntries corePrepDiscardImmediateDrawOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardPaymentsEntries corePrepDiscardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
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
