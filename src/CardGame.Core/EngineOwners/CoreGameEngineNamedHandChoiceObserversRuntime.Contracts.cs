#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNamedHandChoiceObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNamedHandChoiceObserversCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICoreNamedHandChoiceObserversCoreNamedAmbushOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireNamedAmbushReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreNamedHandChoiceObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverAttackHpLossDyingMatches(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreNamedHandChoiceObserversCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreNamedHandChoiceObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNamedHandChoiceObserversCoreNamedHandChoiceObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool NamedHandChoiceFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreNamedHandChoiceObserversCoreDamageSourceHandChoiceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreNamedHandChoiceObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNamedHandChoiceObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}
