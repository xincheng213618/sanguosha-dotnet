#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCommandResultProjectionCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCommandResultProjectionCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTeamMode { get; }

    bool IsNationalWarMode { get; }
}

internal interface ICoreCommandResultProjectionKernelCoreGameEngineCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _revision { get; }
}

internal interface ICoreCommandResultProjectionCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot State { get; }
}
