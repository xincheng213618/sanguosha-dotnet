#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSourceBiriObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheSourceBiriObserversCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OrderedPrintedSkillLossSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreShanheSourceBiriObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheSourceBiriObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreShanheSourceBiriObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreShanheSourceBiriObserversCoreShanheSourceBiriOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertShanheSourceBiri(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreShanheSourceBiriObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreShanheSourceBiriObserversCoreSuitPreventionBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SuitPlacementMovementRoot(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreShanheSourceBiriObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreShanheSourceBiriObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreShanheSourceBiriObserversCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreShanheSourceBiriObserversCoreShanheSourceBiriObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShanheSourceBiriFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
    bool ShanheSourceBiriObserverEdge(int index);
}

internal interface ICoreShanheSourceBiriObserversCoreProgramRecoveryReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramRecoveryReplacementReceiptMatches(global::CardGame.Core.ProgramSkillFrame owner, global::CardGame.Core.RecoveryReplacementFrame recovery, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
    bool ProgramRecoveryReceiptMatches(global::CardGame.Core.ProgramSkillFrame owner, global::CardGame.Core.HpChangedTriggerWindowFrame hp, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
}

internal interface ICoreShanheSourceBiriObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreShanheSourceBiriObserversCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreShanheSourceBiriObserversCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TypedRecoveryHpReturnMatches(global::CardGame.Core.RecoveryReplacementFrame recovery, global::CardGame.Core.HpChangedTriggerWindowFrame hp, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
    bool TypedRecoveryRewardReturnMatches(global::CardGame.Core.RecoveryReplacementFrame recovery, global::CardGame.Core.CardsMovedTriggerWindowFrame movement, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history);
}
