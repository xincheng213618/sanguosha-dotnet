#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramChildResolutionRuntime
{
    private readonly ICoreProgramChildResolutionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramChildResolutionCoreDamageObserverCursorQueriesOperations _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineProgramChildResolutionRuntime(ICoreProgramChildResolutionResolutionStackOperations resolutionStackOperations, ICoreProgramChildResolutionCoreDamageObserverCursorQueriesOperations coreDamageObserverCursorQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
