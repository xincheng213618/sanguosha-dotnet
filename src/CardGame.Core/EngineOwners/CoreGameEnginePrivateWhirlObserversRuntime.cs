#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePrivateWhirlObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateWhirlEntries _corePrivateWhirlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateWhirlObserversEntries _corePrivateWhirlObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEnginePrivateWhirlObserversRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateWhirlEntries corePrivateWhirlOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateWhirlObserversEntries corePrivateWhirlObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateWhirlOperations = corePrivateWhirlOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePrivateWhirlObserversOperations = corePrivateWhirlObserversOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
