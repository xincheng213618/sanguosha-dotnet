#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWuJingQueriesRuntime
{
    private readonly ICoreWuJingQueriesCharacterWuJingOlOperations _characterWuJingOlOperations;
    private readonly ICoreWuJingQueriesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWuJingQueriesCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreWuJingQueriesCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreWuJingQueriesCoreCuiYanNativeQueriesOperations _coreCuiYanNativeQueriesOperations;
    private readonly ICoreWuJingQueriesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreWuJingQueriesCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWuJingQueriesCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreWuJingQueriesCoreProgramParticipantPhaseOperationsOperations _coreProgramParticipantPhaseOperationsOperations;
    internal CoreGameEngineWuJingQueriesRuntime(ICoreWuJingQueriesCharacterWuJingOlOperations characterWuJingOlOperations, ICoreWuJingQueriesResolutionStackOperations resolutionStackOperations, ICoreWuJingQueriesCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreWuJingQueriesCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreWuJingQueriesCoreCuiYanNativeQueriesOperations coreCuiYanNativeQueriesOperations, ICoreWuJingQueriesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreWuJingQueriesCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWuJingQueriesCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreWuJingQueriesCoreProgramParticipantPhaseOperationsOperations coreProgramParticipantPhaseOperationsOperations)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCuiYanNativeQueriesOperations = coreCuiYanNativeQueriesOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
    }
}
