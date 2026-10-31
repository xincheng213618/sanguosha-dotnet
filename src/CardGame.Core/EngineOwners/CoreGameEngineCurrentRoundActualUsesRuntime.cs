#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentRoundActualUsesRuntime
{
    private readonly ICoreCurrentRoundActualUsesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreCurrentRoundActualUsesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreCurrentRoundActualUsesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreCurrentRoundActualUsesCoreWholeActualTurnUsesOperations _coreWholeActualTurnUsesOperations;
    private readonly ICoreCurrentRoundActualUsesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    internal CoreGameEngineCurrentRoundActualUsesRuntime(ICoreCurrentRoundActualUsesContentRegistryOperations contentRegistryOperations, ICoreCurrentRoundActualUsesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreCurrentRoundActualUsesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreCurrentRoundActualUsesCoreWholeActualTurnUsesOperations coreWholeActualTurnUsesOperations, ICoreCurrentRoundActualUsesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
    }
}
