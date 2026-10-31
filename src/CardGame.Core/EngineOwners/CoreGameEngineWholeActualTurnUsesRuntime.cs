#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWholeActualTurnUsesRuntime
{
    private readonly ICoreWholeActualTurnUsesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreWholeActualTurnUsesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWholeActualTurnUsesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreWholeActualTurnUsesCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWholeActualTurnUsesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    internal CoreGameEngineWholeActualTurnUsesRuntime(ICoreWholeActualTurnUsesContentRegistryOperations contentRegistryOperations, ICoreWholeActualTurnUsesResolutionStackOperations resolutionStackOperations, ICoreWholeActualTurnUsesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreWholeActualTurnUsesCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWholeActualTurnUsesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    }
}
