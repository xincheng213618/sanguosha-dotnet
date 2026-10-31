#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhaseStartCardControlObserversCorePhaseStartCardControlObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PhaseStartCardControlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICorePhaseStartCardControlObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePhaseStartCardControlObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHandGainDebtObserverSuffix(int rootIndex);
}

internal interface ICorePhaseStartCardControlObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePhaseStartCardControlObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICorePhaseStartCardControlObserversCorePhaseStartCardControlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactPhaseStartCardControl(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PhaseStartCardControlChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePhaseStartCardControlObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICorePhaseStartCardControlObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICorePhaseStartCardControlObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
