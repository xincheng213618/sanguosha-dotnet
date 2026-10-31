#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardSubsetViewsRuntime
{
    private readonly ICoreCardSubsetViewsCardZonesOperations _cardZonesOperations;
    private readonly ICoreCardSubsetViewsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreCardSubsetViewsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreCardSubsetViewsCoreSnapshotValueProjectionOperations _coreSnapshotValueProjectionOperations;
    internal CoreGameEngineCardSubsetViewsRuntime(ICoreCardSubsetViewsCardZonesOperations cardZonesOperations, ICoreCardSubsetViewsResolutionStackOperations resolutionStackOperations, ICoreCardSubsetViewsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreCardSubsetViewsCoreSnapshotValueProjectionOperations coreSnapshotValueProjectionOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
    }
}
