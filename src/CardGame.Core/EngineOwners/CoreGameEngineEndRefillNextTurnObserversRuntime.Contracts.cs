#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEndRefillNextTurnObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreEndRefillNextTurnObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreEndRefillNextTurnObserversCoreEndRefillNextTurnObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EndRefillFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreEndRefillNextTurnObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreEndRefillNextTurnObserversCoreEndRefillNextTurnOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireEndRefillNextTurnReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreEndRefillNextTurnObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHandGainDebtObserverSuffix(int rootIndex);
}

internal interface ICoreEndRefillNextTurnObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}
